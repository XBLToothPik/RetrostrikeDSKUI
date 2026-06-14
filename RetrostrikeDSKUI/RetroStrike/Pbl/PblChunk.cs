using RetroStrike.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace RetroStrike.Pbl
{
    public class PblChunk
    {
        public PblFile ParentPBLFile { get; private set; }
        public PblChunk ParentPBLChunk { get; private set; }
        public List<PblChunk> Children { get; private set; }
        public uint ID { get; private set; }
        public string IDAsString => Encoding.ASCII.GetString(BitConverter.GetBytes(ID));
        public int DataLength { get; private set; }
        public byte[] Data { get; private set; }
        public long DataStart { get; private set; }
        public long DataEnd { get; private set; }

        Stream sourceStream;

        public PblChunk(Stream sourceStream, PblFile parentPBLFile, PblChunk parentChunk = null)
        {
            this.sourceStream = sourceStream;
            this.Children = new List<PblChunk>();
            this.ParentPBLFile = parentPBLFile;
            this.ParentPBLChunk = parentChunk;
        }

        public void Read()
        {
            var reader = new BinaryReader(sourceStream);
            ID = reader.ReadUInt32();
            DataLength = reader.ReadInt32();
            DataStart = sourceStream.Position;
            DataEnd = DataStart + DataLength;

            if (ChildrenTileExactly())
            {
                while (sourceStream.Position < DataEnd)
                {
                    var child = new PblChunk(sourceStream, this.ParentPBLFile, this);
                    child.Read();
                    Children.Add(child);
                }
            }

            sourceStream.Position = DataEnd;
            long pad = sourceStream.Position % 4;
            if (pad != 0)
                sourceStream.Position += (4 - pad);
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2022:Avoid inexact read with 'Stream.Read'", Justification = "<Pending>")]
        public byte[] GetData(bool includeHeaderData = false)
        {
            int dataLen = includeHeaderData ? 8 + DataLength : DataLength;
            long dataPos = includeHeaderData ? 8 + DataStart : DataStart;
            byte[] _data = new byte[dataLen];
            sourceStream.Seek(dataPos, SeekOrigin.Begin);
            sourceStream.Read(_data, 0, dataLen);
            return _data;
        }
        public string GetDataAsString(Encoding encoder)
        {
            return encoder.GetString(GetData());
        }
        public int CopyDataTo(Stream xOut, bool includeHeaderData = false)
        {
            long dataPos = includeHeaderData ? 8 + DataStart : DataStart;
            sourceStream.Seek(dataPos, SeekOrigin.Begin);
            return IOUtils.CopyFromToWithLength(sourceStream, xOut, DataLength);
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
        bool ChildrenTileExactly()
        {
            if (DataLength < 8) return false;

            long save = sourceStream.Position;
            var reader = new BinaryReader(sourceStream);
            try
            {
                long pos = DataStart;
                int childCount = 0;

                while (pos < DataEnd)
                {
                    if (DataEnd - pos < 8) return false;

                    // ID must be printable — cheap extra constraint that random data often fails.
                    sourceStream.Position = pos;
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
                sourceStream.Position = save;
            }
        }

        static bool IsValidIdByte(byte b) =>
            (b >= 'A' && b <= 'Z') || (b >= 'a' && b <= 'z') ||
            (b >= '0' && b <= '9') || b == '_' || b == ' ';
    }
}
