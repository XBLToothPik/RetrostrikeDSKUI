using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using RetroStrike.VirtualDisk;
using RetrostrikeDSKUI.Application;
using System.Runtime.CompilerServices;
namespace RetroStrike.Pbl
{
    public class PblFile
    {
        public DSKFile OwnerDSKFile { get; set; }

        public Stream MainPBLFileStream { get; private set; }
        public PblChunk RootChunk { get; private set; }
        public PblFile(DSKFile owner, Stream xIn)
        {
            MainPBLFileStream = xIn;
        }
        public static PblFile CreateFromRFI(DSKFile.RFI rfi, bool read = false)
        {
            MemoryStream xMem = new MemoryStream(rfi.GetActiveFileSize());
            rfi.OwnerDSKFile.CopyRFITo(rfi, xMem);
            xMem.Seek(0, SeekOrigin.Begin);
            PblFile pbl = new PblFile(rfi.OwnerDSKFile, xMem);
            if (read)
                pbl.Read();
            return pbl;
        }
        public void Read()
        {
            PblChunk root = new PblChunk(this.MainPBLFileStream, this, null);
            root.Read();
            this.RootChunk = root;
        }
    }
}
