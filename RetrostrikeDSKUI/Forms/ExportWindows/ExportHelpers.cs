using System;
using System.Collections.Generic;
using System.Text;
using RetroStrike.Utils;
namespace RetrostrikeDSKUI.Forms.ExportWindows
{
    public static class ExportHelpers
    {
        public static readonly Dictionary<uint, string> SupportedExportTypes = new Dictionary<uint, string>
        {
            {  Hashing.MakeFNV1A("texture"), "texture" }
        };
        public static bool IsTypeExportSupported(string name)
        {
            return IsTypeExportSupported(Hashing.MakeFNV1A(name));
        }
        public static bool IsTypeExportSupported(uint fnvTypeHash)
        {
            return SupportedExportTypes.ContainsKey(fnvTypeHash);
        }

        public static Type GetExportFormForSupportedType(string name)
        {
            return GetExportFormForSupportedType(Hashing.MakeFNV1A(name));
        }
        public static Type GetExportFormForSupportedType(uint fnvTypeHash)
        {
            if (IsTypeExportSupported(fnvTypeHash))
            {
                switch (SupportedExportTypes[fnvTypeHash])
                {
                    case "texture":
                        return typeof(WindowExportTexture);
                }
            }
            return null;
        }
    }
}
