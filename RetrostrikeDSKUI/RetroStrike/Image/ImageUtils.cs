using System;
using System.Collections.Generic;
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
    }
}
