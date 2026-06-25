using ImageMagick;
using RetroStrike.Enum;
using RetroStrike.Pbl;
using RetroStrike.Platform.XBox;
using RetroStrike.Utils;
using RetrostrikeDSKUI.Application;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Text;
#pragma warning disable CS8629 // Nullable value type may be null.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace RetroStrike.VirtualDisk
{
    public class DSKFileHeader
    {
        public int EntryCount { get; internal set; } = 0;
        public int GridFileOffset { get; internal set; } = 0;

        internal long _headerPos;
        
        public void ReadHeader(BinaryReader reader)
        {
            Debug.WriteLine("Reading Header...");
            _headerPos = reader.BaseStream.Position;
            this.EntryCount = reader.ReadInt32();
            this.GridFileOffset = reader.ReadInt32();

            Debug.WriteLine("Header Values:");
            Debug.WriteLine($"\tNum Entries: {EntryCount}");
            Debug.WriteLine($"\tGridFileOffset: {GridFileOffset}");
        }
        public void WriteHeader(BinaryWriter writer)
        {
            writer.BaseStream.Seek(_headerPos, SeekOrigin.Begin);
            writer.Write(EntryCount);
            writer.Write(GridFileOffset);
        }
    }
    public class DSKFile
    {
        #region Struct
        public class RFI
        {
            public DSKFile OwnerDSKFile;
            public RFI(DSKFile ownerDSKFile)
            {
                this.OwnerDSKFile = ownerDSKFile;
                this.CustomData = new Dictionary<string, object>();
            }

            public uint NameHashOriginal;
            public uint FileTypeOriginal;
            public int FileSizeOriginal;
            public int FilePositionOriginal;


            public uint NameHashNew;
            public uint FileTypeNew;



            public bool IsBeingReplaced;
            public bool IsNewImportedFile;
            public bool IsBeingRemoved;
            public bool IsNewImportedFileOrReplaced => IsBeingReplaced || IsNewImportedFile;

            public bool ProcessAsFileType;
            public Dictionary<string, object> CustomData;

            public Stream NewIncomingFileStream;
            public string NewIncomingFileName;
            public Nullable<uint> NewIncomingTypeHash;

            public uint GetActiveNameHash()
            {
                if (!string.IsNullOrEmpty(NewIncomingFileName))
                    return Hashing.MakeFNV1A(NewIncomingFileName);
                return NameHashOriginal;
            }
            public uint GetActiveTypeHash()
            {
                if (IsNewImportedFileOrReplaced && NewIncomingTypeHash.HasValue)
                    return NewIncomingTypeHash.Value;
                return FileTypeOriginal;
            }
            public int GetActiveFileSize()
            {
                if (IsNewImportedFileOrReplaced)
                    return (int)NewIncomingFileStream.Length;
                return FileSizeOriginal;
            }
        }
        #endregion

        #region Properties
        public DSKFileHeader Header { get; private set; }
        public Dictionary<uint, List<RFI>> Files { get; private set; }
        public bool WasCreatedAsNewDSK { get; private set; }
        #endregion

        #region Fields
        Stream _mainFileStream;
        BinaryReader mainReader;
        public static Dictionary<uint, string> SupportedProcessingTypes { get; } = new Dictionary<uint, string>
        {
            {  Hashing.MakeFNV1A("texture"), "texture" }
        };
        #endregion

        public DSKFile(Stream xIn)
        {
            _mainFileStream = xIn;
            this.Header = new DSKFileHeader();
        }
        public DSKFile()
        {
            this.WasCreatedAsNewDSK = true;
        }
        public void Read()
        {
            mainReader = new BinaryReader(_mainFileStream);

            this.Header.ReadHeader(mainReader);

            this.Files = new Dictionary<uint, List<RFI>>(32);
            this.ReadRFI();

            ReadGridData();
        }
        public void ReadRFI() // Red File Info
        {
            Debug.WriteLine("Reading RFI...");

            int dataSize = this.Header.EntryCount * 3;
            Debug.WriteLine($"\nData Size: {dataSize}");

            Debug.WriteLine($"Reading Entries...");
            int iOffset = dataSize * sizeof(int) + (int)mainReader.BaseStream.Position;
            for (int i = 0; i < this.Header.EntryCount; i++)
            {
                RFI curInfo = new RFI(this);
                curInfo.FilePositionOriginal = iOffset;
                curInfo.FileSizeOriginal = mainReader.ReadInt32();
                curInfo.NameHashOriginal = mainReader.ReadUInt32();
                curInfo.FileTypeOriginal = mainReader.ReadUInt32();

                if (!Files.ContainsKey(curInfo.FileTypeOriginal))
                    Files[curInfo.FileTypeOriginal] = new List<RFI>();
                Files[curInfo.FileTypeOriginal].Add(curInfo);

                iOffset += curInfo.FileSizeOriginal;

                //CountOfTypes[curInfo.FileType]++;
            }
            mainReader.BaseStream.Seek(iOffset, SeekOrigin.Begin);
        }
        void ReadGridData()
        {
            while (mainReader.BaseStream.Length > mainReader.BaseStream.Position)
            {
                uint segmentID = mainReader.ReadUInt32();

                if (segmentID == 0x00FF8040)
                {
                    Debug.WriteLine("HIT");
                }
                else if (segmentID == 0)
                {

                }
                else
                {
                    Debug.WriteLine("HERE");
                }
            }
        }


        void WriteToCurrent(Stream xOut, Stream xOutTemp)
        {
            this.WriteTo(xOutTemp, true);
            xOut.Seek(0, SeekOrigin.Begin);
            xOutTemp.Seek(0, SeekOrigin.Begin);
            xOut.SetLength(0);
            xOutTemp.CopyTo(xOut);

        }
        public void WriteToNewFromCurrent(Stream xOut)
        {
            WriteTo(xOut, true);
        }
        void WriteTo(Stream xOut, bool isWritingFromCurrentData = false)
        {
            BinaryWriter writer = new BinaryWriter(xOut);

            //Write header placeholders
            this.Header = new DSKFileHeader();
            this.Header.WriteHeader(writer);

            //Write RFI TOC
            foreach (var pair in Files)
            {
                foreach (var file in pair.Value)
                {
                    //TODO: Also, if it's being replaced we can prompt the user
                    //  to ask if they want to replace the name and type too,
                    if (file.IsBeingRemoved)
                        continue;
                    if (file.ProcessAsFileType)
                    {
                        ProcessNewRFIAsType(file);
                    }

                    int fileSize = file.GetActiveFileSize();
                    uint nameHash = file.GetActiveNameHash();
                    uint typeHash = file.GetActiveTypeHash();

                    writer.Write(fileSize);
                    writer.Write(nameHash);
                    writer.Write(typeHash);

                }
            }

            //Write Data
            int numFilesWritten = 0;
            foreach (var pair in Files)
            {
                foreach (var file in pair.Value)
                {
                    if (file.IsBeingRemoved)
                        continue;
                    CopyRFITo(file, xOut);
                    numFilesWritten++;
                }
            }

            //We're not going to use GREF (Grid Reference Files) in writing.  They are not used.
            int gridWriteOffset = 0;

            //Add sector padding
            if (numFilesWritten > 0)
            {
                writer.Write(new byte[2048], 0, 2048);
            }

            //Update header with correct values and write it back to the beginning of the file
            this.Header.EntryCount = numFilesWritten;
            this.Header.GridFileOffset = gridWriteOffset;
            this.Header.WriteHeader(writer);
        }
        #region Processors
        void ProcessNewRFIAsType(RFI targetRFI)
        {
            switch (SupportedProcessingTypes[targetRFI.GetActiveTypeHash()])
            {
                case "texture":
                    ProcessNewRFIAsTexture(targetRFI);
                    break;
            }
        }
        void ProcessNewRFIAsTexture(RFI targetRFI)
        {
            //The targetRFI will be a new imported file.  So in this case it should be an image (png, tga..etc..)
            //Though we should really do most of this in the RedTextureXBox (or other platform) 
            //      !! dont forget about doing mips

            int numMips = (int)targetRFI.CustomData["tex_maxmaps"];
            int depth = (int)targetRFI.CustomData["tex_depth"];
            int version = (int)targetRFI.CustomData["tex_formatversion"];
            
            eTexFormat texFormat = (eTexFormat)targetRFI.CustomData["tex_format"];
            RedTextureXBox.eRedTextureType texType = (RedTextureXBox.eRedTextureType)targetRFI.CustomData["tex_type"];


            RedTextureXBox texture = RedTextureXBox.CreateFromImage(targetRFI.NewIncomingFileStream, targetRFI.NewIncomingFileName, ref numMips, depth, version, texFormat, texType);
            string encodeErrors = string.Empty;
            bool encodeSuccess = texture.EncodeMips(out numMips, out encodeErrors);
            if (!encodeSuccess)
            {

            }
            targetRFI.CustomData["tex_maxmaps"] = numMips;
            
            PblFile newPblFile = PblFile.CreateFromRootChunk(this, PblChunk.CreateBlankMemoryChunk(Hashing.MakeFNV1A("ucfb")));
            PblChunk tex_Chunk = texture.ToPblChunk(newPblFile);
            tex_Chunk.WriteChunkTo(newPblFile.RootChunk);


            //Reset the processing stage.  The new incoming stream is that of the processed stream and so if the DSK were saved with a new processed item, and then saved again,
            //it wouldn't try processing the stream again (it couldn't because the original stream was closed and replaced with the processsed one), so it should
            //just copy from the NewIncomingFileStream in that case, which is good.
            //TODO:
            //      1) Add "IsProcessing" to the RFI and update it here (or in the switch)
            //      2) Add "ProcessSuccess" bool to the RFI and update it here with a try statement (or in the switch)
            //      3) Add "ProcessError" if there is an error in the try statement as suggested above
            
            //TODO: Move this to the switch statement as this should be applicable for all processed items:
            targetRFI.NewIncomingFileStream.Close();
            targetRFI.NewIncomingFileStream = newPblFile.MainPBLFileStream;
            targetRFI.ProcessAsFileType = false;
        }
        #endregion


        public RFI GetRFI(uint typeHash, uint nameHash)
        {
            if (Files.ContainsKey(typeHash))
            {
                var targetRFI = Files[typeHash].First(rfi => rfi.GetActiveNameHash() == nameHash);
                return targetRFI;
            }
            return null;
        }
        public RFI GetRFI(uint typeHash, string name)
        {
            return GetRFI(typeHash, Hashing.MakeFNV1A(name));
        }
        public void CopyRFITo(RFI targetRFI, Stream xOut)
        {
            //We need to check in the UI side if the target file is being replaced, if so, prompt them about it.
            Stream sourceFileStream = (targetRFI.IsNewImportedFileOrReplaced || targetRFI.IsBeingReplaced)
                ? targetRFI.NewIncomingFileStream
                : _mainFileStream;
            long seekPos = WasCreatedAsNewDSK || targetRFI.IsNewImportedFileOrReplaced || targetRFI.IsBeingReplaced
                ? 0
                : targetRFI.FilePositionOriginal;
            long fileLen = WasCreatedAsNewDSK || targetRFI.IsNewImportedFileOrReplaced || targetRFI.IsBeingReplaced
                ? targetRFI.NewIncomingFileStream.Length
                : targetRFI.FileSizeOriginal;
            sourceFileStream.Seek(seekPos, SeekOrigin.Begin);

            IOUtils.CopyFromToWithLength(sourceFileStream, xOut, fileLen);
        }
        public bool CancelRFIReplace(RFI targetRFI, out string errors)
        {
            if (targetRFI.IsBeingReplaced)
            {
                uint targetHashToRevertTo = targetRFI.NameHashOriginal;
                bool wasTypeChanged = false;
                if (targetRFI.NewIncomingTypeHash != null)
                    wasTypeChanged = targetRFI.FileTypeOriginal != targetRFI.NewIncomingTypeHash.Value;

                if (wasTypeChanged && Files.ContainsKey(targetRFI.FileTypeOriginal))
                {
                    //If there is already a type defined for the original type then we need to check if there is also a file with the same name in that type dict
                    var originalTypeFiles = Files[targetRFI.FileTypeOriginal];
                    if (originalTypeFiles.Any(rfi => rfi.GetActiveNameHash() == targetHashToRevertTo))
                    {
                        //A file in the original RFI's type dict with the same namehash already exists
                        errors = $"A file with the name of " +
                            $"\"{RetroStrikeGlobals.HashResolver.ResolveHash(targetHashToRevertTo, RetroStrike.HashNameResolver.eHashTypeSelector.All)}\" " +
                            $"already exists in types \"{RetroStrikeGlobals.HashResolver.ResolveHash(targetRFI.FileTypeOriginal, RetroStrike.HashNameResolver.eHashTypeSelector.FileTypes)}\"";
                        return false;
                    }
                }
                else if (wasTypeChanged && !Files.ContainsKey(targetRFI.FileTypeOriginal))
                {
                    //There was no file with that type, so we create the type dict, remove it from the new type dict, create the original and put it in there.
                    Files.Add(targetRFI.FileTypeOriginal, new List<RFI>(1));
                    Files[targetRFI.NewIncomingTypeHash.Value].Remove(targetRFI);
                    Files[targetRFI.FileTypeOriginal].Add(targetRFI);
                }
                targetRFI.IsBeingReplaced = false;
                targetRFI.NewIncomingFileStream.Close();
                targetRFI.NewIncomingTypeHash = null;
                targetRFI.NewIncomingFileName = string.Empty;
                errors = string.Empty;
                ValidateDSKDict();
                return true;
            }
            errors = "File was not being replaced";
            return false;
        }
        public bool ReplaceRFIWithStream(RFI targetRFI, Stream xIn, out string errors, string newName = null, Nullable<uint> newTypeHash = null) 
        {
            bool changedFileTypes = false;
            uint hashToUseInCheck = newName != null ? Hashing.MakeFNV1A(newName) : targetRFI.NameHashOriginal;
            if (newTypeHash.HasValue)
            {
                if (newTypeHash.Value != targetRFI.FileTypeOriginal)
                {
                    changedFileTypes = true;
                    if (DoesHaveFileInType(newTypeHash.Value, hashToUseInCheck))
                    {
                        errors = "A file with that same name already exists in that type category.";
                        return false;
                    }
                }
            }
            targetRFI.IsBeingReplaced = true;
            targetRFI.NewIncomingFileStream = xIn;
            if (changedFileTypes)
                targetRFI.NewIncomingTypeHash = newTypeHash.Value;

            if (!string.IsNullOrEmpty(newName))
                targetRFI.NewIncomingFileName = newName;
            if (changedFileTypes)
            {
                Files[targetRFI.FileTypeOriginal].Remove(targetRFI);
                Files[targetRFI.NewIncomingTypeHash.Value].Add(targetRFI);
            }
            errors = string.Empty;
            return true;
        }
        public bool AddFile(uint typeHash, Stream xIn, string fileName, out RFI newFile, out string failReason)
        {
            if (CanAddFile(typeHash, fileName, out failReason))
            {
                newFile = new RFI(this);
                newFile.IsNewImportedFile = true;
                newFile.NewIncomingFileStream = xIn;
                newFile.NewIncomingFileName = fileName;
                newFile.NewIncomingTypeHash = typeHash;
                if (!Files.ContainsKey(typeHash))
                    Files.Add(typeHash, new List<RFI>(1));
                Files[typeHash].Add(newFile);
                return true;
            }
            newFile = null;
            return false;
        }
        public bool AddFile(string typeName, Stream xin, string fileName, out RFI newRFI, out string addFailReason)
        {
            return AddFile(Hashing.MakeFNV1A(typeName), xin, fileName, out newRFI, out addFailReason);
        }
        public bool CancelImportOfFile(RFI targetRFI, out string errors)
        {
            return CancelImportOfFile(targetRFI.GetActiveTypeHash(), targetRFI.GetActiveNameHash(), out errors);
        }
        public bool CancelImportOfFile(uint typeHash, uint fileNameHash, out string errors)
        {
            if (DoesHaveFileInType(typeHash, fileNameHash))
            {
                var targetRFI = GetRFI(typeHash, fileNameHash);
                if (!targetRFI.IsNewImportedFile)
                {
                    errors = "File was not being imported";
                    return false;
                }
                if (targetRFI != null)
                    targetRFI.NewIncomingFileStream.Close();
                int numRemoved = Files[typeHash].RemoveAll(rfi => rfi.GetActiveNameHash() == fileNameHash);
                if (numRemoved > 0)
                {
                    ValidateDSKDict();
                    errors = string.Empty;
                }
                if (numRemoved == 0)
                    errors = "No files removed";
                else
                    errors = string.Empty;
                return numRemoved > 0;
            }
            errors = string.Empty;
            return false;
        }
        public bool CancelImportOfFile(uint typeHash, string fileName, out string errors)
        {
            return CancelImportOfFile(typeHash, Hashing.MakeFNV1A(fileName), out errors);
        }
        public bool CancelRemovalOfFile(RFI targetRFI, out string errors)
        {
            if (targetRFI.IsBeingRemoved)
            {
                targetRFI.IsBeingRemoved = false;
                errors = string.Empty;
                return true;
            }
            else
            {
                errors = "File was not being removed.";
                return false;
            }
        }
        public bool CancelRemovalOfFile(uint typeHash, uint fileNameHash, out string errors)
        {
            var targetRFI = GetRFI(typeHash, fileNameHash);
            if (targetRFI != null)
            {
                return CancelRemovalOfFile(targetRFI, out errors);
            }
            errors = "RFI Not Found";
            return false;
        }
        public bool CancelRemovalOfFile(uint typeHash, string fileName, out string errors)
        {
            return CancelRemovalOfFile(typeHash, Hashing.MakeFNV1A(fileName), out errors);
        }
        public bool DoesHaveFileInType(uint targetType, string targetFileName)
        {
            return DoesHaveFileInType(targetType, Hashing.MakeFNV1A(targetFileName));
        }
        public bool DoesHaveFileInType(uint targetType, uint targetFileNameHash)
        {
            if (Files.ContainsKey(targetType))
            {
                if (Files[targetType].Any(item => (item.GetActiveNameHash() == targetFileNameHash)))
                {
                    return true;
                }
            }
            return false;
        }
        public bool DoesTypeExist(uint typeHash)
        {
            return Files.ContainsKey(typeHash);
        }
        public bool DoesTypeExist(string typeName)
        {
            return DoesTypeExist(Hashing.MakeFNV1A(typeName));
        }
        public bool CanAddFile(uint typeHash, uint fileNameHash, out string reason)
        {
            bool toRet = true;
            if (Files.ContainsKey(typeHash))
            {
                if (Files[typeHash].Count(item => item.NameHashOriginal == fileNameHash) > 0)
                {
                    toRet = false;
                    reason = "A file with that name already exists in that type";
                }
                else
                    reason = string.Empty;
            }
            else
                reason = "Type does not exist";
            
            return toRet;
        }
        public bool CanAddFile(uint typeHash, string fileName, out string reason)
        {
            return CanAddFile(typeHash, Hashing.MakeFNV1A(fileName), out reason);
        }

        void ValidateDSKDict()
        {
            List<uint> typesToRemove = new List<uint>(Files.Count);
            foreach (var pair in Files)
            {
                if (pair.Value.Count == 0)
                    typesToRemove.Add(pair.Key);
            }
            foreach (var pairToRemove in typesToRemove)
                Files.Remove(pairToRemove);
        }
    }
}
