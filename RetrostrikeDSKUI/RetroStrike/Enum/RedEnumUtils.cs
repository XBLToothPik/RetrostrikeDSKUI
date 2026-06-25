using System;
using System.Collections.Generic;
using System.Text;

namespace RetroStrike.Enum
{
    public static class RedEnumUtils
    {
        public static eTexFormat XBoxTexFormatToeTexFormat(eXBoxTextureFormat xboxFormat)
        {
            switch (xboxFormat)
            {
                case eXBoxTextureFormat.XBOXFMT_P8: return eTexFormat.P8;
                case eXBoxTextureFormat.XBOXFMT_A8: return eTexFormat.A8;
                case eXBoxTextureFormat.XBOXFMT_L8: return eTexFormat.L8;
                case eXBoxTextureFormat.XBOXFMT_A8L8: return eTexFormat.A8L8;
                case eXBoxTextureFormat.XBOXFMT_A1R5G5B5: return eTexFormat.A1R5G5B5;
                case eXBoxTextureFormat.XBOXFMT_A4R4G4B4: return eTexFormat.A4R4G4B4;
                case eXBoxTextureFormat.XBOXFMT_R5G6B5: return eTexFormat.R5G6B5;
                case eXBoxTextureFormat.XBOXFMT_X1R5G5B5: return eTexFormat.X1R5G5B5;
                case eXBoxTextureFormat.XBOXFMT_A8R8G8B8: return eTexFormat.A8R8G8B8;
                case eXBoxTextureFormat.XBOXFMT_X8R8G8B8: return eTexFormat.X8R8G8B8;
                case eXBoxTextureFormat.XBOXFMT_DXT1: return eTexFormat.DXT1;
                case eXBoxTextureFormat.XBOXFMT_DXT3: return eTexFormat.DXT3;
                case eXBoxTextureFormat.XBOXFMT_D16_LOCKABLE: return eTexFormat.D16_LOCKABLE;
                default: return eTexFormat.UNKNOWN;
            }
        }
        public static eXBoxTextureFormat eTexFormatToXBoxTexFormat(eTexFormat texFormat)
        {
            switch (texFormat)
            {
                case eTexFormat.P8: return eXBoxTextureFormat.XBOXFMT_P8;
                case eTexFormat.A8: return eXBoxTextureFormat.XBOXFMT_A8;
                case eTexFormat.L8: return eXBoxTextureFormat.XBOXFMT_L8;
                case eTexFormat.A8L8: return eXBoxTextureFormat.XBOXFMT_A8L8;
                case eTexFormat.A1R5G5B5: return eXBoxTextureFormat.XBOXFMT_A1R5G5B5;
                case eTexFormat.A4R4G4B4: return eXBoxTextureFormat.XBOXFMT_A4R4G4B4;
                case eTexFormat.R5G6B5: return eXBoxTextureFormat.XBOXFMT_R5G6B5;
                case eTexFormat.X1R5G5B5: return eXBoxTextureFormat.XBOXFMT_X1R5G5B5;
                case eTexFormat.A8R8G8B8: return eXBoxTextureFormat.XBOXFMT_A8R8G8B8;
                case eTexFormat.X8R8G8B8: return eXBoxTextureFormat.XBOXFMT_X8R8G8B8;
                case eTexFormat.DXT1: return eXBoxTextureFormat.XBOXFMT_DXT1;
                case eTexFormat.DXT3: return eXBoxTextureFormat.XBOXFMT_DXT3;
                case eTexFormat.D16_LOCKABLE: return eXBoxTextureFormat.XBOXFMT_D16_LOCKABLE;
                default: return eXBoxTextureFormat.XBOXFMT_UNKNOWN;
            }
        }
    }
}
