using ImageMagick;
using RetroStrike.Enum;
using RetroStrike.Platform.XBox;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace RetroStrike.Image
{
    public static class ImageUtils
    {
        public static int GetPixel(byte[] rgbaPixels, int width, int x, int y, bool isIndexed)
        {
            if (!isIndexed)
            {
                int index = (y * width + x) * 4;
                return (rgbaPixels[index + 3] << 24) |
                       (rgbaPixels[index + 0] << 16) |
                       (rgbaPixels[index + 1] << 8) |
                        rgbaPixels[index + 2];
            }
            else
                throw new NotImplementedException();
        }
        public static int GetMaxNumMips(int width, int height)
        {
            if (width == 0 || height == 0)
                return 0;
            for (int mip = 1; mip < 32; mip++)
            {
                int mipWidth = width >> mip;
                int mipHeight = height >> mip;
                if (mipWidth == 0 || mipHeight == 0)
                {
                    return mip;
                }
            }
            return 1;
        }
        public static RedTextureXBox.cFaceData CreateRawFaceDataFromImage(MagickImage img, ref int numMips, eTexType texType)
        {
            //TODO: UNTESTED 6.28.2026

            int numFaces = texType == eTexType.CUBEMAP ? 6 : 1;
            int actualNumMips = 0;
            bool resizeMipsData = false;
            RedTextureXBox.cFaceData faceData = new RedTextureXBox.cFaceData(numMips);
            for (int mip = 0; mip < numMips; mip++)
            {
                int curMipWidth = (int)img.Width >> mip;
                int curMipHeight = (int)img.Height >> mip;
                int nextMipWidth = (int)img.Width >> (mip + 1);
                int nextMipHeight = (int)img.Height >> (mip + 1);
                var mipData = img.GetPixels().ToByteArray(PixelMapping.RGBA);
                faceData.MipData[mip] = mipData;
                //resize to next mip 
                if (mip + 1 < numMips && nextMipWidth > 0 && nextMipHeight > 0)
                {
                    img.Resize((uint)nextMipWidth, (uint)nextMipHeight);
                    numMips = mip + 1;
                    actualNumMips += 1;
                }
                else
                {
                    numMips = mip + 1;
                    actualNumMips = mip + 1;
                    resizeMipsData = true;
                    break;
                }
            }

            if (resizeMipsData)
                Array.Resize(ref faceData.MipData, actualNumMips);

            return faceData;

        }
    }
}
