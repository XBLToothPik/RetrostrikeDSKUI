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
        #region Properties
        public Stream DataStream { get; private set; }
        public PblFile ParentPBLFile { get; private set; }
        public PblChunk ParentPBLChunk { get; private set; }
        public List<PblChunk> Children { get; private set; }
        public uint ID { get; private set; }
        public string IDAsString => Encoding.ASCII.GetString(BitConverter.GetBytes(ID));
        public int DataLength { get; private set; }
        public long DataStart { get; private set; }
        public long DataEnd { get; private set; }
        #endregion

        #region CTORS
        public PblChunk(Stream sourceStream, PblFile parentPBLFile, PblChunk parentChunk = null)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        {
            this.DataStream = sourceStream;
            this.Children = new List<PblChunk>();
            this.ParentPBLFile = parentPBLFile;
            this.ParentPBLChunk = parentChunk;
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
