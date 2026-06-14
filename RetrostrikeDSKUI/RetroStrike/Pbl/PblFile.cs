using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace RetroStrike.Pbl
{
    public class PblFile
    {
        public Stream MainPBLFileStream { get; private set; }
        public PblChunk RootChunk { get; private set; }
        public PblFile(Stream xIn)
        {
            MainPBLFileStream = xIn;
        }
        public void Read()
        {
            PblChunk root = new PblChunk(this.MainPBLFileStream, this, null);
            root.Read();
            this.RootChunk = root;
        }
    }
    class PblRootFile
    {

    }
}
