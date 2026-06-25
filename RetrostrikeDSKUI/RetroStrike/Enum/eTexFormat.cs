using System;
using System.Collections.Generic;
using System.Text;

namespace RetroStrike.Enum
{
    public enum eTexFormat
    {
        UNKNOWN,
        P8,
        A8,
        L8,
        A8L8,
        A1R5G5B5,
        A4R4G4B4,
        R5G6B5,
        X1R5G5B5,
        A8R8G8B8,
        X8R8G8B8,
        DXT1,
        DXT3,
        D16_LOCKABLE,
        FORCE_DWORD = 0x7FFFFFFF,
    }
}
