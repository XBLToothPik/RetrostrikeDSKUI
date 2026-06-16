using RetroStrike.Pbl;
using Squish;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace RetroStrike.Platform.XBox
{
    public class RedTextureXBox
    {
        public bool WasCreatedFromPBLChunk { get; private set; }
        public PblChunk PblTexChunk { get; private set; }
        public enum eRedTextureType : short
        {
            TEXTURE = 1,
            CUBEMAP = 2,
            VOLUME = 3
        }
        public enum eXBoxD3DFormat : uint
        {
            XBOXFMT_UNKNOWN = 0xFFFFFFFF,

            XBOXFMT_P8 = 0x0000000B,

            XBOXFMT_A8 = 0x00000019,
            XBOXFMT_L8 = 0x00000000,

            XBOXFMT_A8L8 = 0x0000001A,
            XBOXFMT_A1R5G5B5 = 0x00000001,
            XBOXFMT_A4R4G4B4 = 0x00000004,
            XBOXFMT_R5G6B5 = 0x00000005,
            XBOXFMT_X1R5G5B5 = 0x00000003,

            XBOXFMT_A8R8G8B8 = 0x00000006,
            XBOXFMT_X8R8G8B8 = 0x00000007,

            XBOXFMT_DXT1 = 0x0000000C,
            XBOXFMT_DXT3 = 0x0000000E,

            XBOXFMT_D16_LOCKABLE = 0x0000002C,

            XBOXFMT_FORCE_DWORD = 0x7FFFFFFF,
        }
        public string TextureName;
        public short Width;
        public short Height;
        public short Depth;
        public short MaxMaps;
        public eRedTextureType RedTextureType;
        public short TextureFormatVersion;
        public eXBoxD3DFormat TextureFormat;
        public float MipBias;
        public static RedTextureXBox CreateFromPBLChunk(PblChunk tex_chunk)
        {
            RedTextureXBox texture = new RedTextureXBox();
            texture.WasCreatedFromPBLChunk = true;
            texture.PblTexChunk = tex_chunk;

            texture.TextureName = tex_chunk.GetChildByID("NAME").GetDataAsString(Encoding.ASCII);
            byte[] _infoData = tex_chunk.GetChildByID("INFO").GetData();
            using (MemoryStream xMem = new MemoryStream(_infoData))
            {
                using (BinaryReader reader = new BinaryReader(xMem))
                {
                    texture.Width = reader.ReadInt16();
                    texture.Height = reader.ReadInt16();
                    texture.Depth = reader.ReadInt16(); //always 1 (atleast on XBox)
                    texture.MaxMaps = reader.ReadInt16();
                    texture.RedTextureType = (eRedTextureType)reader.ReadInt16();
                    texture.TextureFormatVersion = reader.ReadInt16(); //expected to be 1
                    texture.TextureFormat = (eXBoxD3DFormat)reader.ReadInt32();
                    texture.MipBias = reader.ReadSingle();
                }
            }
            return texture;
        }
        SquishFlags GetSquishFlagFromFormat(eXBoxD3DFormat format)
        {
            switch (format)
            {
                case eXBoxD3DFormat.XBOXFMT_DXT1:
                    return SquishFlags.Dxt1;
                case eXBoxD3DFormat.XBOXFMT_DXT3:
                    return SquishFlags.Dxt3;
                default:
                    return 0;
            }
        }
        public bool Exp(Stream xOut, out string errors)
        {
            if (WasCreatedFromPBLChunk)
            {
                //
                //  look at pcRedTexture.cpp in the port for the structure
                //
                var rawData = GetTextureData();
                int totalDataSize = rawData.Length;
                bool isDXT = FormatIsDXT((uint)TextureFormat); //TODO: Check platform because DXT is only available for XBox files
                bool isSwizzled = FormatIsSwizzled((uint)TextureFormat);
                int bpp = FormatBPP((uint)TextureFormat);
                //If it's a CubeMap then the number of faces is DataSize / 6 and we have to iterate over that as their own textures kinda
                if (RedTextureType == eRedTextureType.TEXTURE)
                {
                    int numMips = (MaxMaps > 0) ? MaxMaps : 1;
                    if (isDXT)
                    {
                        int blockBytes = FormatGetBlockBytes((uint)TextureFormat);
                        int sourcePos = 0;

                        for (int mip = 0; mip < numMips; mip++)
                        {
                            int mipWidth = Math.Max(1, Width >> mip);
                            int mipHeight = Math.Max(1, Height >> mip);
                            int cols = Math.Max(1, (mipWidth + 3) / 4);
                            int rows = Math.Max(1, (mipHeight + 3) / 4);
                            int rowSize = cols * blockBytes;
                            byte[] mipData = new byte[mipWidth * mipHeight * blockBytes];

                            for (int row = 0; row < rows; ++row)
                            {
                                int destPos = row * rowSize;
                                Array.Copy(rawData, sourcePos + (row * rowSize), mipData, destPos, rowSize);
                            }
                            sourcePos += rows * rowSize;
                            var mipDec = Squish.SquishLib.DecompressImage(mipData, mipWidth, mipHeight, GetSquishFlagFromFormat(TextureFormat));
                            SaveRgba(mipDec, mipWidth, mipHeight, $"testimg_mip{mip}.bmp"); //TODO: don't save to a file here obviously.
                        }
                    }
                    else
                    {
                        // Uncompressed textures also carry a prebuilt authored mip chain in BODY.
                        // Preserve those exact mip levels instead of regenerating them on PC,
                        // otherwise alpha-cutout textures and UI atlases shimmer/flicker and
                        // lose their intended coverage.
                        int sourcePos = 0;
                        for (int mip = 0; mip < numMips; ++mip)
                        {
                            int mipWidth = Math.Max(1, Width >> mip);
                            int mipHeight = Math.Max(1, Height >> mip);
                            int srcRowPitch = mipWidth * bpp;
                            int mipSize = srcRowPitch * mipHeight;
                            byte[] mipData = new byte[mipSize];
                            if (isSwizzled)
                            {
                                byte[] tempMip = new byte[mipSize];
                                Array.Copy(rawData, sourcePos, mipData, 0, mipSize);
                                UnswizzleTexture(mipData, tempMip, mipWidth, mipHeight, bpp, srcRowPitch);
                                mipData = tempMip;
                            }
                            else
                            {
                                //Haven't actually tested this part because none of the textures in streamed.dsk meet the criteria (NOT DXT && NOT SWIZZLED), but it should work (hopefully)
                                for (int row = 0; row < mipHeight; ++row)
                                {
                                    int destPos = sourcePos + (row * srcRowPitch);
                                    Array.Copy(rawData, sourcePos, mipData, destPos, srcRowPitch);
                                }
                            }
                            SaveRgba(mipData, mipWidth, mipHeight, $"testimg_nondxt_mip{mip}.bmp"); //TODO: don't save to a file here obviously.
                            sourcePos += mipSize;
                        }
                    }
                }
                else if (RedTextureType == eRedTextureType.CUBEMAP)
                {
                    //TODO:  Add cubemap.  It's pretty much the same as normal texture but with 6 faces that store the mipchain individually
                             //though none of the textures in streamed.dsk are CubeMap, so will have to really test it after I create the writer.
                    
                
                }
            }
            errors = "Texture was not created from a PBLChunk.";
            return false;
        }
        static uint MortonExpand(uint v)
        {
            v = (v | (v << 8)) & 0x00ff00ff;
            v = (v | (v << 4)) & 0x0f0f0f0f;
            v = (v | (v << 2)) & 0x33333333;
            v = (v | (v << 1)) & 0x55555555;
            return v;
        }

        static void UnswizzleTexture(byte[] src, byte[] dst, int W, int H, int bpp, int dstPitch)
        {
            int minDim = Math.Min(W, H);
            int logMin = 0;
            while ((1 << logMin) < minDim) logMin++;

            for (int y = 0; y < H; y++)
            {
                for (int x = 0; x < W; x++)
                {
                    uint morton;
                    if (W == H)
                    {
                        morton = MortonExpand((uint)x) | (MortonExpand((uint)y) << 1);
                    }
                    else if (W > H)
                    {
                        uint xLow = (uint)x & ((uint)minDim - 1);
                        uint xHigh = (uint)x >> logMin;
                        morton = MortonExpand(xLow) | (MortonExpand((uint)y) << 1) | (xHigh << (2 * logMin));
                    }
                    else
                    {
                        uint yLow = (uint)y & ((uint)minDim - 1);
                        uint yHigh = (uint)y >> logMin;
                        morton = MortonExpand((uint)x) | (MortonExpand(yLow) << 1) | (yHigh << (2 * logMin));
                    }

                    int srcOffset = (int)(morton * (uint)bpp);
                    int dstOffset = y * dstPitch + x * bpp;
                    for (int b = 0; b < bpp; b++)
                        dst[dstOffset + b] = src[srcOffset + b];
                }
            }
        }
        void SaveRgba(byte[] rgba, int width, int height, string path)
        {
            // GDI+ uses BGRA byte order for 32bppArgb, so swap R and B.
            byte[] bgra = new byte[rgba.Length];
            for (int i = 0; i < rgba.Length; i += 4)
            {
                bgra[i + 0] = rgba[i + 2]; // B
                bgra[i + 1] = rgba[i + 1]; // G
                bgra[i + 2] = rgba[i + 0]; // R
                bgra[i + 3] = rgba[i + 3]; // A
            }

            using var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var rect = new Rectangle(0, 0, width, height);
            var data = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            // Copy row by row in case stride != width*4.
            int rowBytes = width * 4;
            for (int y = 0; y < height; y++)
                Marshal.Copy(bgra, y * rowBytes, data.Scan0 + y * data.Stride, rowBytes);

            bmp.UnlockBits(data);
            bmp.Save(path, ImageFormat.Png);
        }
        public bool FormatIsDXT(uint textureFormat)
        {
            return textureFormat == 0x0C || textureFormat == 0x0E || textureFormat == 0x0F;
        }
        public int FormatGetBlockBytes(uint textureFormat)
        {
            return ((eXBoxD3DFormat)textureFormat) == eXBoxD3DFormat.XBOXFMT_DXT1 ? 8 : 16;
        }
        //TODO: When I made the RedTexture.cs file, I Need to make this a virtual
        public bool FormatIsSwizzled(uint textureFormat)
        {
            return (textureFormat <= 0x0B) || (textureFormat == 0x19) || (textureFormat == 0x1A);
        }
        //TODO: When I made the RedTexture.cs file, I Need to make this a virtual
        public int FormatBPP(uint textureformat)
        {
            switch (textureformat)
            {
                case 0:         
                case 0x19:
                    return 1;   // L8, A8
                case 0x01:
                case 0x03:
                case 0x04:
                case 0x05:
                case 0x10:
                case 0x11:
                case 0x1A:
                    return 2;   // 16-bit (A1R5G5B5, X1R5G5B5, A4R4G4B4, R5G6B5, LIN_*, A8L8)
                case 0x06:
                case 0x07:
                case 0x12:
                case 0x13:
                    return 4;   // 32-bit
                default:
                    return 0;   //DXT or unkown


            }
        }
        public byte[] GetTextureData()
        {
            if (WasCreatedFromPBLChunk)
            {
                var body = PblTexChunk.GetChildByID("BODY");
                return body.GetData();
            }

            //TODO: If it's from a new incoming stream...?  Probably don't support it as there's not really a reason to.
            return null;
        }

    }
}
