using RetroStrike.VirtualDisk;
using RetrostrikeDSKUI.Application;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace RetroStrike.Pbl
{
    public class PblFile
    {
        public DSKFile OwnerDSKFile { get; set; }

        /// <summary>
        /// This stream should be the same as the RootChunk's DataStream
        /// </summary>
        public Stream MainPBLFileStream { get; internal set; }
        public PblChunk RootChunk { get; internal set; }
        public PblFile(DSKFile owner, Stream xIn) : this(owner)
        {
            this.MainPBLFileStream = xIn;
        }
        public PblFile(DSKFile owner)
        {
            this.OwnerDSKFile = owner;
        }
        public PblFile Copy()
        {
            PblFile newFile = new PblFile(this.OwnerDSKFile);
            PblChunk newRootChunk = PblChunk.CreateBlankMemoryChunk(this.RootChunk.ID);
            newRootChunk.DataStart = this.RootChunk.DataStart;
            newRootChunk.DataEnd = this.RootChunk.DataEnd;
            newFile.MainPBLFileStream = newRootChunk.DataStream;
            Action<PblChunk, PblChunk> copyChunkChildren = null;
            copyChunkChildren = (PblChunk source, PblChunk target) =>
            {
                source.WriteChunkTo(target, false, false);
                target.DataStream = newRootChunk.DataStream;
                foreach (var childChunk in source.Children)
                {
                    var newChild = PblChunk.CreateBlankMemoryChunk(childChunk.ID);
                    newChild.DataStart = childChunk.DataStart;
                    newChild.DataEnd = childChunk.DataEnd;
                    target.Children.Add(newChild);
                    newChild.ParentPBLChunk = target;
                    copyChunkChildren(childChunk, newChild);
                }
            };
            copyChunkChildren(this.RootChunk, newRootChunk);
            newFile.RootChunk = newRootChunk;
            return newFile;
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
        public static PblFile CreateFromRootChunk(DSKFile ownerDSKFile, PblChunk rootChunk)
        {
            PblFile toRet = new PblFile(ownerDSKFile, rootChunk.DataStream);
            toRet.RootChunk = rootChunk;
            toRet.MainPBLFileStream = rootChunk.DataStream;
            return toRet;
        }

        public void Read()
        {
            PblChunk root = new PblChunk(this.MainPBLFileStream, this, null);
            root.Read();
            this.RootChunk = root;
        }
    }
}
