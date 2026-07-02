using ImageMagick;
using RetroStrike.Pbl;
using RetroStrike.Enum;
using RetroStrike.Image;
using Squish;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using static System.Windows.Forms.DataFormats;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
#pragma warning disable CS8601 // Possible null reference assignment.

namespace RetroStrike.Platform.XBox
{
    public class RedTextureXBox
    {
        public class cFaceData
        {
            public cFaceData()
            {

            }
            public cFaceData(int numMips)
            {
                this.MipData = new byte[numMips][];
            }
            public cFaceData(byte[][] mipData)
            {
                this.MipData = mipData;
            }
            public byte[][] MipData;
        }
        public bool WasCreatedFromPBLChunk { get; private set; }
        public bool WasCreatedFromImage { get; private set; }
        public PblChunk PblTexChunk { get; private set; }


        //This is defined in "HandyLib\Source\TexFormat.cpp".
        //The platform independent formats are defined in "HandyLib\Include\TexFormat.h"


        public string TextureName;
        public short Width;
        public short Height;
        public short Depth;
        public short MaxMaps;
        public eTexType RedTextureType;
        public short TextureFormatVersion;
        public eTexFormat TextureFormat;
        public float MipBias;

        public cFaceData[] FaceData { get; private set; }
        public bool MipsEncoded { get; private set; } //TODO:
        public int NumFaces => RedTextureType == eTexType.CUBEMAP ? 6 : 1;

        //public bool MipsProcessed { get; private set; }
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
                    texture.RedTextureType = (eTexType)reader.ReadInt16();
                    texture.TextureFormatVersion = reader.ReadInt16(); //expected to be 1
                    texture.TextureFormat = RedEnumUtils.XBoxTexFormatToeTexFormat((eXBoxTextureFormat)reader.ReadInt32()); //TODO: This is platform specific
                    texture.MipBias = reader.ReadSingle();
                    reader.ReadInt32(); //"TotalDataLength"
                }
            }
            return texture;
        }
        public static RedTextureXBox CreateFromFaceData(cFaceData[] faceData, string textureName, int imageWidth, int imageHeight, int numMips, int depth, int texFormatVersion, eTexFormat texFormat, eTexType redTexType)
        {
            RedTextureXBox toRet = new RedTextureXBox();
            toRet.TextureName = textureName;
            toRet.Width = (short)imageWidth;
            toRet.Height = (short)imageHeight;
            toRet.MaxMaps = (short)numMips;
            toRet.Depth = (short)depth;
            toRet.TextureFormat = texFormat;
            toRet.RedTextureType = redTexType;
            toRet.TextureFormatVersion = (short)texFormatVersion;
            toRet.WasCreatedFromImage = true;
            toRet.FaceData = faceData;
            toRet.MipsEncoded = false;
            return toRet;
        }

        public bool DecodeMips(out int numMipsDecoded, out string errors)
        {
            numMipsDecoded = 0;
            //We have to do more work to determine if Mips are encoded, like if it was created from a PblChunk but the texture isn't
            //  compressed or anything at all.
            //if (!MipsEncoded)
            //{
            //    errors = "Mips not encoded";
            //    return false;
            //}
            //TODO: IF the mips are already decoded, return false and error message

            if (WasCreatedFromPBLChunk)
            {
                //
                //  look at pcRedTexture.cpp in the port for the structure
                //
                var rawData = GetTextureData();
                int totalDataSize = rawData.Length;
                bool isDXT = FormatIsDXT(this.TextureFormat); //TODO: Check platform because DXT is only available for XBox files
                bool isSwizzled = FormatIsSwizzled(this.TextureFormat);
                int bpp = FormatBPP(this.TextureFormat);
                int numMips = (this.MaxMaps > 0) ? this.MaxMaps : 1;

                if (RedTextureType == eTexType.TEXTURE)
                {
                    FaceData = new cFaceData[this.NumFaces];
                    for (int face = 0; face < this.NumFaces; face++)
                        FaceData[face] = new cFaceData(numMips);

                    if (isDXT)
                    {
                        int blockBytes = FormatGetBlockBytes(TextureFormat);
                        int sourcePos = 0;
                        for (int face = 0; face < this.NumFaces; face++)
                        {
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
                                var mipDec = Squish.SquishLib.DecompressImage(mipData, mipWidth, mipHeight, GetSquishFlagFromFormat(TextureFormat));
                                FaceData[face].MipData[mip] = mipDec;
                                numMipsDecoded++;
                                sourcePos += rows * rowSize;
                            }
                        }
                    }
                    else
                    {
                        // Uncompressed textures also carry a prebuilt authored mip chain in BODY.
                        // Preserve those exact mip levels instead of regenerating them on PC,
                        // otherwise alpha-cutout textures and UI atlases shimmer/flicker and
                        // lose their intended coverage.
                        int sourcePos = 0;
                        for (int face = 0; face < this.NumFaces; face++)
                        {
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
                                FaceData[face].MipData[mip] = mipData;
                                numMipsDecoded++;
                                sourcePos += mipSize;
                            }
                        }
                    }
                }
                else if (RedTextureType == eTexType.CUBEMAP)
                {
                    //TODO:  Add cubemap.  It's pretty much the same as normal texture but with 6 faces that store the mipchain individually
                    //though none of the textures in streamed.dsk are CubeMap, so will have to really test it after I create the writer.


                }
            }
            if (numMipsDecoded > 0)
            {
                errors = string.Empty;
                MipsEncoded = false;
                return true;
            }
            errors = "Texture was not created from a PBLChunk.";
            return false;
        }

        public bool EncodeMips(out int numMipsEncoded, out string errors)
        {
            numMipsEncoded = 0;
            errors = string.Empty;
            for (int face = 0; face < this.FaceData.Length; face++)
            {
                for (int mip = 0; mip < this.FaceData[face].MipData.Length; mip++)
                {
                    int newMipSize = CalculateBufferSize(this.Width >> mip, this.Height >> mip, this.TextureFormat);
                    byte[] newMipData = new byte[newMipSize];

                    if (EncodeMip_Xbox(ref newMipData, this.FaceData[face].MipData[mip], this.Width >> mip, this.Height >> mip))
                    {
                        numMipsEncoded++;
                        this.FaceData[face].MipData[mip] = newMipData;
                    }
                    else
                    {
                        this.FaceData[face].MipData[mip] = null;
                        errors += $"Encode Error on Mip index{mip}";
                    }
                }
            }
            if (numMipsEncoded > 0)
            {
                errors = string.Empty;
                return true;
            }
            return false;
        }
        unsafe bool EncodeMip_Xbox(ref byte[] destination, byte[] source, int width, int height)
        {
            if (this.TextureFormat == eTexFormat.DXT1 || this.TextureFormat == eTexFormat.DXT3)
            {
                destination = SquishLib.CompressImage(source, width, height, GetSquishFlagFromFormat(this.TextureFormat));
                return true;
            }
            else
            {
                bool isSwizzled = FormatIsSwizzled(this.TextureFormat);
                int bpp = FormatBPP(this.TextureFormat); //dont think i need this in here

                int bufferSize = CalculateBufferSize(width, height, this.TextureFormat);
                byte[] _mipBuffer = new byte[bufferSize];
                byte[] usedBuffer;
                if (TextureFormat != eTexFormat.P8)
                {
                    PixelDesc outputPixels = new PixelDesc(this.TextureFormat);
                    fixed (byte* pDst = _mipBuffer)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                int color = ImageUtils.GetPixel(source, width, x, y, false);
                                byte* pPixel = pDst + (y * width + x) * bpp;

                                //Non-Xbox maybe?:
                                //outputPixels.SetRGBA(pPixel,
                                //    (byte)((color >> 16) & 0xFF),  // R  (ARGB: bits 23-16)
                                //    (byte)((color >> 8) & 0xFF),  // G  (ARGB: bits 15-8)
                                //    (byte)(color & 0xFF),  // B  (ARGB: bits 7-0)
                                //    (byte)((color >> 24) & 0xFF)); // A  (ARGB: bits 31-24)

                                //XBox is using BGRA 
                                outputPixels.SetRGBA(pPixel,
                                    (byte)(color & 0xFF),  // B → R slot (Xbox BGRA)
                                    (byte)((color >> 8) & 0xFF),  // G
                                    (byte)((color >> 16) & 0xFF),  // R → B slot (Xbox BGRA)
                                    (byte)((color >> 24) & 0xFF)); // A
                            }
                        }
                    }
                    usedBuffer = _mipBuffer;
                }
                else
                {
                    //Get IndexedPixelArray for Pallet (see TextureLib.cpp:780)
                    throw new NotImplementedException();
                }
                SwizzleTexture(usedBuffer, destination, width, height, bpp, width * bpp);
                return true;

            }
        }

        public PblChunk ToPblChunk(PblFile parentPBLFile)
        {
            //
            //  TODO: This needs massively refactored and portions of it need put directly into the PblChunk.cs
            //

            //Let's create the streams
            MemoryStream texChunkStream = new MemoryStream();
            MemoryStream nameChunkStream = new MemoryStream();
            MemoryStream infoChunkStream = new MemoryStream();
            MemoryStream bodyChunkStream = new MemoryStream();

            //Setup constants (can be more than this but this is what we're working with right now)
            //  ^ There is also PALT for Pallete, but it's not in the encoder yet.
            const string CHUNK_TEX_ID_STR = "tex_";
            const string CHUNK_NAME_ID_STR = "NAME";
            const string CHUNK_INFO_ID_STR = "INFO";
            const string CHUNK_BODY_ID_STR = "BODY";

            const int CHUNK_INFO_SIZE = 24;

            //First we'll create the NameChunk Data
            BinaryWriter writer = new BinaryWriter(nameChunkStream);
            writer.Write(Encoding.ASCII.GetBytes(CHUNK_NAME_ID_STR));
            writer.Write(this.TextureName.Length + 1); //+1 for null terminator
            writer.Write(Encoding.ASCII.GetBytes(this.TextureName));
            writer.Write((byte)0); //null term
            PblChunk newNameChunk = PblChunk.CreateNew(nameChunkStream, parentPBLFile, BitConverter.ToUInt32(Encoding.ASCII.GetBytes(CHUNK_NAME_ID_STR), 0), PblChunk.CHUNK_HEADER_SIZE, (int)(nameChunkStream.Length - PblChunk.CHUNK_HEADER_SIZE));

            //Next we'll create the Body chunk (before info because Info contains the body's content size)
            writer = new BinaryWriter(bodyChunkStream);
            writer.Write(Encoding.ASCII.GetBytes(CHUNK_BODY_ID_STR));
            writer.Write(FaceData.Sum(face => face.MipData.Sum(mip => mip.Length)));
            int totalDataLen = FaceData.Sum(face => face.MipData.Sum(mip => mip.Length));
            for (int face = 0; face < FaceData.Length; face++)
                for (int mip = 0; mip < FaceData[face].MipData.Length; mip++)
                    writer.Write(FaceData[face].MipData[mip]);

            PblChunk newBodyChunk = PblChunk.CreateNew(bodyChunkStream, parentPBLFile, BitConverter.ToUInt32(Encoding.ASCII.GetBytes(CHUNK_BODY_ID_STR), 0), PblChunk.CHUNK_HEADER_SIZE, (int)(bodyChunkStream.Length - PblChunk.CHUNK_HEADER_SIZE));


            //Next we'll create the Info Chunk
            writer = new BinaryWriter(infoChunkStream);
            writer.Write(Encoding.ASCII.GetBytes(CHUNK_INFO_ID_STR));
            writer.Write(CHUNK_INFO_SIZE);
            writer.Write((short)Width);
            writer.Write((short)Height);
            writer.Write((short)Depth);
            writer.Write((short)MaxMaps);
            writer.Write((short)RedTextureType);
            writer.Write((short)TextureFormatVersion);
            writer.Write((int)(RedEnumUtils.eTexFormatToXBoxTexFormat(TextureFormat)));
            writer.Write(MipBias);
            writer.Write((int)bodyChunkStream.Length - PblChunk.CHUNK_HEADER_SIZE);
            PblChunk newInfoChunk = PblChunk.CreateNew(infoChunkStream, parentPBLFile, BitConverter.ToUInt32(Encoding.ASCII.GetBytes(CHUNK_INFO_ID_STR), 0), PblChunk.CHUNK_HEADER_SIZE, (int)(infoChunkStream.Length - PblChunk.CHUNK_HEADER_SIZE));

            //Reposition the streams
            nameChunkStream.Seek(0, SeekOrigin.Begin);
            infoChunkStream.Seek(0, SeekOrigin.Begin);
            bodyChunkStream.Seek(0, SeekOrigin.Begin);

            //Finally, we'll create the tex_ chunk
            writer = new BinaryWriter(texChunkStream);
            writer.Write(Encoding.ASCII.GetBytes(CHUNK_TEX_ID_STR));
            writer.Write(0); //WriteChunkTo will re-write this as the datalength increases.


            PblChunk newTexChunk = PblChunk.CreateNew(texChunkStream, parentPBLFile, BitConverter.ToUInt32(Encoding.ASCII.GetBytes(CHUNK_TEX_ID_STR), 0), PblChunk.CHUNK_HEADER_SIZE, 0);
            newNameChunk.WriteChunkTo(newTexChunk, true);
            newInfoChunk.WriteChunkTo(newTexChunk, true);
            newBodyChunk.WriteChunkTo(newTexChunk, true);
            //We now have a full "tex_" chunk

            //When we call PblChunk.CopyDataTo(mainPblFilestream) into the main UCFB, we'll update the final start positions
            return newTexChunk;

        }

        SquishFlags GetSquishFlagFromFormat(eTexFormat format)
        {
            switch (format)
            {
                case eTexFormat.DXT1:
                    return SquishFlags.Dxt1;
                case eTexFormat.DXT3:
                    return SquishFlags.Dxt3;
                default:
                    return 0;
            }
        }

        int CalculateBufferSize(int width, int height, eTexFormat texFormat)
        {
            int size;
            const int BLOCK_WIDTH = 4;
            const int BLOCK_HEIGHT = 4;
            const int BLOCK_SIZE_IN_WORDS = 4;
            if ((texFormat == eTexFormat.DXT1) || (texFormat == eTexFormat.DXT3))
            {
                int blockSize;
                if (texFormat == eTexFormat.DXT1)
                    blockSize = 2 * BLOCK_SIZE_IN_WORDS;
                else
                    blockSize = 4 * BLOCK_SIZE_IN_WORDS;
                size = blockSize * (int)MathF.Ceiling(width / (float)BLOCK_WIDTH) * (int)MathF.Ceiling(height / (float)BLOCK_HEIGHT);
            }
            else
            {
                size = width * height * FormatBPP(texFormat);
            }

            return size;
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
        static void SwizzleTexture(byte[] src, byte[] dst, int W, int H, int bpp, int srcPitch)
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

                    int srcOffset = y * srcPitch + x * bpp;
                    int dstOffset = (int)(morton * (uint)bpp);
                    for (int b = 0; b < bpp; b++)
                        dst[dstOffset + b] = src[srcOffset + b];
                }
            }
        }
        public bool FormatIsDXT(eTexFormat textureFormat)
        {
            return textureFormat == eTexFormat.DXT1 || TextureFormat == eTexFormat.DXT3;
        }
        public int FormatGetBlockBytes(eTexFormat textureFormat)
        {
            return textureFormat == eTexFormat.DXT1 ? 8 : 16;
        }
        //TODO: When I made the RedTexture.cs file, I Need to make this a virtual
        public bool FormatIsSwizzled(eTexFormat textureFormat)
        {
            return ((uint)textureFormat <= 0x0B) || ((uint)textureFormat == 0x19) || ((uint)textureFormat == 0x1A);
        }
        //TODO: When I made the RedTexture.cs file, I Need to make this a virtual
        public int FormatBPP(eTexFormat textureformat)
        {
            switch (textureformat)
            {

                // 1 byte formats
                case eTexFormat.P8:
                case eTexFormat.A8:
                case eTexFormat.L8:
                    return 1;

                // 2 byte formats
                case eTexFormat.A8L8:
                case eTexFormat.A1R5G5B5:
                case eTexFormat.A4R4G4B4:
                case eTexFormat.R5G6B5:
                case eTexFormat.X1R5G5B5:
                case eTexFormat.D16_LOCKABLE:
                    return 2;

                // 4 byte formats
                case eTexFormat.A8R8G8B8:
                case eTexFormat.X8R8G8B8:
                    return 4;

                // compressed formats return block size in bytes
                case eTexFormat.DXT1:
                    return 8;

                case eTexFormat.DXT3:
                    return 16;

                default:
                    return 0;
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
