using System;
using System.Collections.Generic;
using System.Text;

namespace RetroStrike.Enum
{
    public enum eXBoxTextureFormat : uint
    {
        XBOXFMT_UNKNOWN = 0xFFFFFFFF,

        XBOXFMT_P8 = 0x0000000B,

        XBOXFMT_A8 = 0x00000019,
        XBOXFMT_L8 = 0x00000000,

        XBOXFMT_A8L8 = 0x0000001A,
        XBOXFMT_A1R5G5B5 = 0x00000001,
        XBOXFMT_A4R4G4B4 = 0x00000004,
        XBOXFMT_R5G6B5 = 0x00000005,
        XBOXFMT_X1R5G5B5 = 0x00000003,

        XBOXFMT_A8R8G8B8 = 0x00000006,
        XBOXFMT_X8R8G8B8 = 0x00000007,

        XBOXFMT_DXT1 = 0x0000000C,
        XBOXFMT_DXT3 = 0x0000000E,

        XBOXFMT_D16_LOCKABLE = 0x0000002C,

        XBOXFMT_FORCE_DWORD = 0x7FFFFFFF,
    }
}
