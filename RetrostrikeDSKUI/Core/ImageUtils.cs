using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace RetrostrikeDSKUI.Core
{
    public static class ImageUtils
    {
        public static Bitmap MipToBMP(byte[] mipData, int width, int height)
        {
            byte[] bgra = new byte[mipData.Length];
            for (int i = 0; i < mipData.Length; i += 4)
            {
                bgra[i + 0] = mipData[i + 2]; // B
                bgra[i + 1] = mipData[i + 1]; // G
                bgra[i + 2] = mipData[i + 0]; // R
                bgra[i + 3] = mipData[i + 3]; // A
            }

            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var rect = new Rectangle(0, 0, width, height);
            var data = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            // Copy row by row in case stride != width*4.
            int rowBytes = width * 4;
            for (int y = 0; y < height; y++)
                Marshal.Copy(bgra, y * rowBytes, data.Scan0 + y * data.Stride, rowBytes);

            bmp.UnlockBits(data);
            return bmp;
        }
    }
}
