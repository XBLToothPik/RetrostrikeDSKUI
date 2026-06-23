using RetroStrike.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms.VisualStyles;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace RetroStrike.Pbl
{
    public class PblChunk
    {
        #region Constant
        public const int CHUNK_HEADER_SIZE = 8;
        #endregion

        #region Properties
        public Stream DataStream { get; internal set; }
        public PblFile ParentPBLFile { get; internal set; }
        public PblChunk ParentPBLChunk { get; internal set; }
        public List<PblChunk> Children { get; internal set; }
        public uint ID { get; private set; }
        public string IDAsString => Encoding.ASCII.GetString(BitConverter.GetBytes(ID));
        public int DataLength { get; private set; }
        public long DataStart { get; internal set; }
        public long DataEnd { get; internal set; }
        #endregion

        #region CTORS
        public PblChunk(Stream dataStream, PblFile parentPBLFile, PblChunk parentChunk = null) : this()
        {
            this.DataStream = dataStream;
            this.ParentPBLFile = parentPBLFile;
            this.ParentPBLChunk = parentChunk;
        }
        PblChunk()
        {
            this.Children = new List<PblChunk>();
        }
        #endregion

        #region Reading
        public void Read()
        {
            var reader = new BinaryReader(DataStream);
            ID = reader.ReadUInt32();
            DataLength = reader.ReadInt32();
            DataStart = DataStream.Position;
            DataEnd = DataStart + DataLength;

            if (ChildrenTileExactly())
            {
                while (DataStream.Position < DataEnd)
                {
                    var child = new PblChunk(DataStream, this.ParentPBLFile, this);
                    child.Read();
                    Children.Add(child);
                }
            }

            DataStream.Position = DataEnd;
            long pad = DataStream.Position % 4;
            if (pad != 0)
                DataStream.Position += (4 - pad);
        }
        bool ChildrenTileExactly()
        {
            if (DataLength < 8) return false;

            long save = DataStream.Position;
            var reader = new BinaryReader(DataStream);
            try
            {
                long pos = DataStart;
                int childCount = 0;

                while (pos < DataEnd)
                {
                    if (DataEnd - pos < 8) return false;

                    // ID must be printable — cheap extra constraint that random data often fails.
                    DataStream.Position = pos;
                    byte[] idBytes = reader.ReadBytes(4);
                    foreach (byte b in idBytes)
                        if (!IsValidIdByte(b)) return false;

                    int childLen = reader.ReadInt32();
                    if (childLen < 0) return false;

                    long childEnd = pos + 8 + childLen;
                    if (childEnd > DataEnd) return false;

                    long aligned = childEnd;
                    long p = aligned % 4;
                    if (p != 0) aligned += (4 - p);
                    pos = aligned;
                    childCount++;
                }

                // Must consume the region exactly, and actually contain something.
                return pos == DataEnd && childCount > 0;
            }
            finally
            {
                DataStream.Position = save;
            }
        }
        #endregion

        #region Writing
        public void WriteChunkTo(PblChunk targetChunkToWriteTo, bool includeHeaderData = false, bool addAsChild = false, bool setStream = false)
        {
            var xOut = targetChunkToWriteTo.DataStream;
            DataStream.Seek(DataStart, SeekOrigin.Begin);
            if (includeHeaderData)
                DataStream.Seek(-8, SeekOrigin.Current);
            xOut.Seek(0, SeekOrigin.End);
            int numAligned = IOUtils.PadStreamToAlignment(xOut, 4);
            int lenToCopy = includeHeaderData ? 8 + DataLength : DataLength;
            int numCopied = IOUtils.CopyFromToWithLength(DataStream, xOut, lenToCopy);
            numAligned += IOUtils.PadStreamToAlignment(xOut, 4);

            xOut.Seek(4, SeekOrigin.Begin);
            targetChunkToWriteTo.DataLength += numCopied + numAligned;
            Debug.WriteLine($"{this.IDAsString} LEN: {this.DataLength} (TARGET: \"{targetChunkToWriteTo.IDAsString}\" LEN: {targetChunkToWriteTo.DataLength})");
            xOut.Write(BitConverter.GetBytes(targetChunkToWriteTo.DataLength), 0, sizeof(int));
            xOut.Seek(0, SeekOrigin.End);
            if (addAsChild)
            {
                targetChunkToWriteTo.Children.Add(this);
                this.ParentPBLChunk = targetChunkToWriteTo;
            }
        }
        #endregion

        #region Methods
        public static PblChunk CreateNew(Stream newStream, PblFile parentPBLFile, uint id, int dataStart, int dataLength)
        {
            PblChunk newChunk = new PblChunk(newStream, parentPBLFile);
            newChunk.ID = id;
            newChunk.DataStart = dataStart;
            newChunk.DataLength = dataLength;
            newChunk.DataEnd = dataStart + dataLength;
            return newChunk;
        }
        internal static PblChunk CreateBlankMemoryChunk(uint id, int dataSize = 0)
        {
            PblChunk newChunk = new PblChunk();
            newChunk.ID = id;

            Stream xMem = new MemoryStream();
            xMem.Write(BitConverter.GetBytes(newChunk.ID), 0, sizeof(uint));
            xMem.Write(BitConverter.GetBytes(dataSize), 0, sizeof(int));
            newChunk.DataStream = xMem;
            return newChunk;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2022:Avoid inexact read with 'Stream.Read'", Justification = "<Pending>")]
        public byte[] GetData(bool includeHeaderData = false)
        {
            int dataLen = includeHeaderData ? 8 + DataLength : DataLength;
            long dataPos = includeHeaderData ? 8 + DataStart : DataStart;
            byte[] _data = new byte[dataLen];
            DataStream.Seek(dataPos, SeekOrigin.Begin);
            DataStream.Read(_data, 0, dataLen);
            return _data;
        }
        public string GetDataAsString(Encoding encoder)
        {
            return encoder.GetString(GetData()).TrimEnd('\0');
        }
        public int CopyDataTo(Stream xOut, bool includeHeaderData = false)
        {
            long dataPos = includeHeaderData ? 8 + DataStart : DataStart;
            DataStream.Seek(dataPos, SeekOrigin.Begin);
            return IOUtils.CopyFromToWithLength(DataStream, xOut, DataLength);
        }
        public PblChunk GetChildByID(uint id)
        {
            foreach (var child in Children)
            {
                if (child.ID == id)
                    return child;
            }
            return null;
        }
        public PblChunk GetChildByID(string fourCharacterID)
        {
            if (fourCharacterID.Length < 4 || fourCharacterID.Length > 4)
                return null;
            return GetChildByID(BitConverter.ToUInt32(Encoding.ASCII.GetBytes(fourCharacterID), 0));
        }
        static bool IsValidIdByte(byte b) =>
            (b >= 'A' && b <= 'Z') || (b >= 'a' && b <= 'z') ||
            (b >= '0' && b <= '9') || b == '_' || b == ' ';
        #endregion
    }
}
