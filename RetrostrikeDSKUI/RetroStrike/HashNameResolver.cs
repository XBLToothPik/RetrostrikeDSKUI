using RetroStrike.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RetroStrike
{
    public class HashNameResolver
    {
        public enum eHashDictType
        {
            FileNames,
            FileTypes
        }
        public enum eHashTypeSelector
        {
            FileNames,
            FileTypes,
            All
        }
        public HashNameResolver()
        {
            FileNamesHashDict = new Dictionary<uint, string>();
            TypesHashDict = new Dictionary<uint, string>();
        }

        public Dictionary<uint, string> FileNamesHashDict { get; private set; }
        public Dictionary<uint, string> TypesHashDict { get; private set; }
        public void LoadHashDict(Stream xIn, eHashDictType hashesType)
        {
            StreamReader reader = new StreamReader(xIn);
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line == null || string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    continue;
                string textResolve = line.Trim();
                var hashResolve = Hashing.MakeFNV1A(textResolve);
                var targetDict = hashesType == eHashDictType.FileNames
                    ? FileNamesHashDict
                    : TypesHashDict;
                if (!targetDict.ContainsKey(hashResolve))
                    targetDict[hashResolve] = textResolve;
                else if (targetDict.ContainsKey(hashResolve) && targetDict[hashResolve].ToLower() != textResolve.ToLower())
                {
                    var stored = targetDict[hashResolve];
                    throw new Exception("Hash collision?");
                }
            }
        }
        public string ResolveHash(uint typeHash, eHashTypeSelector selector)
        {
            Dictionary<uint, string>[] selectorDicts = new Dictionary<uint, string>[selector == eHashTypeSelector.All ? 2 : 1];
            if (selector == eHashTypeSelector.FileNames)
                selectorDicts[0] = FileNamesHashDict;
            else if (selector == eHashTypeSelector.FileTypes)
                selectorDicts[0] = TypesHashDict;
            else
            {
                selectorDicts[0] = FileNamesHashDict;
                selectorDicts[1] = TypesHashDict;
            }
            for (int i = 0; i < selectorDicts.Length; i++)
            {
                var targetDict = selectorDicts[i];
                string resolve = string.Empty;
                if (targetDict.TryGetValue(typeHash, out resolve))
                    return resolve;
            }
            return $"{typeHash.ToString("X8").ToUpper()}";
        }

    }
}
