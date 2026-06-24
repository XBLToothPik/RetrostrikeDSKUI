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
        #region Properties
        public DSKFile OwnerDSKFile { get; set; }
        /// <summary>
        /// This stream should be the same as the RootChunk's DataStream
        /// </summary>
        public Stream MainPBLFileStream { get; internal set; }
        public PblChunk RootChunk { get; internal set; }
        #endregion

        #region CTORS
        public PblFile(DSKFile owner, Stream xIn) : this(owner)
        {
            this.MainPBLFileStream = xIn;
        }
        public PblFile(DSKFile owner)
        {
            this.OwnerDSKFile = owner;
        }
        #endregion

        #region Reading
        public void Read()
        {
            PblChunk root = new PblChunk(this.MainPBLFileStream, this, null);
            root.Read();
            this.RootChunk = root;
        }
        #endregion

        #region Methods
        public PblFile Copy()
        {
            PblFile newFile = new PblFile(this.OwnerDSKFile);
            PblChunk newRootChunk = PblChunk.CreateBlankMemoryChunk(this.RootChunk.ID);
            newRootChunk.DataStart = this.RootChunk.DataStart;
            newRootChunk.DataEnd = this.RootChunk.DataEnd;
            newFile.MainPBLFileStream = newRootChunk.DataStream;
            Action<PblChunk, PblChunk, bool> copyChunkChildren = null;
            copyChunkChildren = (PblChunk source, PblChunk target, bool copyHeader) =>
            {
                source.WriteChunkTo(target, copyHeader);
                target.DataStream = newRootChunk.DataStream;
                target.DataStart = source.DataStart;
                target.DataEnd = source.DataEnd;
                foreach (var childChunk in source.Children)
                {
                    var newChild = PblChunk.CreateBlankMemoryChunk(childChunk.ID);
                    target.Children.Add(newChild);
                    newChild.ParentPBLChunk = target;
                    copyChunkChildren(childChunk, newChild, true);
                }
            };
            copyChunkChildren(this.RootChunk, newRootChunk, false); //The "newRootChunk" already has the header written, so we don't need to copy it.
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
        #endregion
    }
}
