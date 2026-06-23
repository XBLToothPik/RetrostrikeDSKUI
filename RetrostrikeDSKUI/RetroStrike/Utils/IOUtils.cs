using System;
using System.Collections.Generic;
using System.Text;

namespace RetroStrike.Utils
{
    public static class IOUtils
    {
        public static int CopyFromToWithLength(Stream source, Stream dest, long length,  int bufferSize = 81920)
        {
            int DefaultBufferSize = bufferSize;
            byte[] buffer = new byte[DefaultBufferSize];
            long bytesRemaining = length;
            int bytesCopied = 0;
            while (bytesRemaining > 0)
            {
                int bytesToRead = (int)Math.Min(DefaultBufferSize, bytesRemaining);

                int bytesRead = source.Read(buffer, 0, bytesToRead);

                if (bytesRead == 0)
                    throw new EndOfStreamException(); //this should never happen

                dest.Write(buffer, 0, bytesRead);
                bytesCopied += bytesRead;
                bytesRemaining -= bytesRead;
            }
            return bytesCopied;
        }
        public static int PadStreamToAlignment(Stream targetStream, int numAlignment)
        {
            if (numAlignment <= 0)
                throw new ArgumentOutOfRangeException(nameof(numAlignment));

            long remainder = targetStream.Position % numAlignment;
            if (remainder == 0)
                return 0;

            int padBytes = (int)(numAlignment - remainder);
            targetStream.Write(new byte[padBytes], 0, padBytes);
            return padBytes;
        }
    }
}
