using System;
using System.Collections.Generic;
using System.Text;

namespace RetroStrike.Utils
{
    public static class IOUtils
    {
        public static void CopyFromToWithLength(Stream source, Stream dest, long length,  int bufferSize = 81920)
        {
            int DefaultBufferSize = bufferSize;
            byte[] buffer = new byte[DefaultBufferSize];
            long bytesRemaining = length;

            while (bytesRemaining > 0)
            {
                int bytesToRead = (int)Math.Min(DefaultBufferSize, bytesRemaining);

                int bytesRead = source.Read(buffer, 0, bytesToRead);

                if (bytesRead == 0)
                    throw new EndOfStreamException(); //this should never happen

                dest.Write(buffer, 0, bytesRead);
                bytesRemaining -= bytesRead;
            }
        }
    }
}
