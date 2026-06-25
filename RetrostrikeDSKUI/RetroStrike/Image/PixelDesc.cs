using System;
using System.Collections.Generic;
using System.Text;
using System;
using RetroStrike.Enum;
namespace RetroStrike.Image
{

    public unsafe class PixelDesc
    {
        private delegate float GetChannel(byte* pBits);
        private delegate void SetChannel(byte* pBits, float val);

        private readonly uint bpp;
        private uint roffset, goffset, boffset, aoffset;
        private uint rbits, gbits, bbits, abits;
        private uint rmask, gmask, bmask, amask;
        private float rscale, gscale, bscale, ascale;
        private float rscaleinv, gscaleinv, bscaleinv, ascaleinv;

        private GetChannel pGetR, pGetG, pGetB, pGetA;
        private SetChannel pSetR, pSetG, pSetB, pSetA;

        public PixelDesc(eTexFormat format)
        {
            bpp = BytesPerPixelFromFormat(format);
            pGetR = pGetG = pGetB = pGetA = GetNull;
            pSetR = pSetG = pSetB = pSetA = SetNull;

            switch (format)
            {
                case eTexFormat.A8: abits = 8; break;
                case eTexFormat.L8: rbits = gbits = bbits = 8; break;
                case eTexFormat.R5G6B5: rbits = bbits = 5; gbits = 6; roffset = 11; goffset = 5; break;
                case eTexFormat.X1R5G5B5: rbits = gbits = bbits = 5; roffset = 10; goffset = 5; break;
                case eTexFormat.A1R5G5B5: rbits = gbits = bbits = 5; abits = 1; roffset = 10; goffset = 5; aoffset = 15; break;
                case eTexFormat.A4R4G4B4: rbits = gbits = bbits = abits = 4; roffset = 8; goffset = 4; aoffset = 12; break;
                case eTexFormat.A8L8: rbits = gbits = bbits = abits = 8; aoffset = 8; break;
                case eTexFormat.A8R8G8B8: rbits = gbits = bbits = abits = 8; roffset = 16; goffset = 8; aoffset = 24; break;
                case eTexFormat.X8R8G8B8: rbits = gbits = bbits = 8; roffset = 16; goffset = 8; break;
            }

            Init();
        }

        public float GetR(byte* pBits) => pGetR(pBits);
        public float GetG(byte* pBits) => pGetG(pBits);
        public float GetB(byte* pBits) => pGetB(pBits);
        public float GetA(byte* pBits) => pGetA(pBits);
        public void SetR(byte* pBits, float val) => pSetR(pBits, val);
        public void SetG(byte* pBits, float val) => pSetG(pBits, val);
        public void SetB(byte* pBits, float val) => pSetB(pBits, val);
        public void SetA(byte* pBits, float val) => pSetA(pBits, val);

        public void SetRGBA(byte* pBitsOut, byte r, byte g, byte b, byte a = 255)
        {
            pSetR(pBitsOut, r / 255f);
            pSetG(pBitsOut, g / 255f);
            pSetB(pBitsOut, b / 255f);
            pSetA(pBitsOut, a / 255f);
        }

        public void SetRGBA(byte* pBitsOut, float fR, float fG, float fB, float fA = 1.0f)
        {
            pSetR(pBitsOut, fR);
            pSetG(pBitsOut, fG);
            pSetB(pBitsOut, fB);
            pSetA(pBitsOut, fA);
        }

        public void SetRGBA(byte* pBitsOut, byte* pBitsIn, PixelDesc srcDesc)
        {
            pSetR(pBitsOut, srcDesc.pGetR(pBitsIn));
            pSetG(pBitsOut, srcDesc.pGetG(pBitsIn));
            pSetB(pBitsOut, srcDesc.pGetB(pBitsIn));
            pSetA(pBitsOut, srcDesc.pGetA(pBitsIn));
        }

        // Null channel handlers
        private static float GetNull(byte* pBits) => 1.0f;
        private static void SetNull(byte* pBits, float val) { }

        // 32-bit (4 bytes per pixel)
        private float GetR32(byte* p) => (*(uint*)p & rmask) * rscale;
        private float GetG32(byte* p) => (*(uint*)p & gmask) * gscale;
        private float GetB32(byte* p) => (*(uint*)p & bmask) * bscale;
        private float GetA32(byte* p) => (*(uint*)p & amask) * ascale;

        private void SetR32(byte* p, float v) => *(uint*)p = (*(uint*)p & ~rmask) | ((uint)(v * rscaleinv) & rmask);
        private void SetG32(byte* p, float v) => *(uint*)p = (*(uint*)p & ~gmask) | ((uint)(v * gscaleinv) & gmask);
        private void SetB32(byte* p, float v) => *(uint*)p = (*(uint*)p & ~bmask) | ((uint)(v * bscaleinv) & bmask);
        private void SetA32(byte* p, float v) => *(uint*)p = (*(uint*)p & ~amask) | ((uint)(v * ascaleinv) & amask);

        // 16-bit (2 bytes per pixel)
        private float GetR16(byte* p) => (*(ushort*)p & rmask) * rscale;
        private float GetG16(byte* p) => (*(ushort*)p & gmask) * gscale;
        private float GetB16(byte* p) => (*(ushort*)p & bmask) * bscale;
        private float GetA16(byte* p) => (*(ushort*)p & amask) * ascale;

        private void SetR16(byte* p, float v) => *(ushort*)p = (ushort)((*(ushort*)p & ~rmask) | ((uint)(v * rscaleinv) & rmask));
        private void SetG16(byte* p, float v) => *(ushort*)p = (ushort)((*(ushort*)p & ~gmask) | ((uint)(v * gscaleinv) & gmask));
        private void SetB16(byte* p, float v) => *(ushort*)p = (ushort)((*(ushort*)p & ~bmask) | ((uint)(v * bscaleinv) & bmask));
        private void SetA16(byte* p, float v) => *(ushort*)p = (ushort)((*(ushort*)p & ~amask) | ((uint)(v * ascaleinv) & amask));

        // 8-bit (1 byte per pixel)
        private float GetR8(byte* p) => (*p & rmask) * rscale;
        private float GetG8(byte* p) => (*p & gmask) * gscale;
        private float GetB8(byte* p) => (*p & bmask) * bscale;
        private float GetA8(byte* p) => (*p & amask) * ascale;

        private void SetR8(byte* p, float v) => *p = (byte)((*p & ~rmask) | ((byte)(v * rscaleinv) & rmask));
        private void SetG8(byte* p, float v) => *p = (byte)((*p & ~gmask) | ((byte)(v * gscaleinv) & gmask));
        private void SetB8(byte* p, float v) => *p = (byte)((*p & ~bmask) | ((byte)(v * bscaleinv) & bmask));
        private void SetA8(byte* p, float v) => *p = (byte)((*p & ~amask) | ((byte)(v * ascaleinv) & amask));

        private void Init()
        {
            if (rbits != 0)
            {
                pGetR = bpp == 1 ? (GetChannel)GetR8 : bpp == 2 ? GetR16 : GetR32;
                pSetR = bpp == 1 ? (SetChannel)SetR8 : bpp == 2 ? SetR16 : SetR32;
                rmask = ((1u << (int)rbits) - 1) << (int)roffset;
                rscaleinv = rmask;
                rscale = 1.0f / rscaleinv;
            }
            if (gbits != 0)
            {
                pGetG = bpp == 1 ? (GetChannel)GetG8 : bpp == 2 ? GetG16 : GetG32;
                pSetG = bpp == 1 ? (SetChannel)SetG8 : bpp == 2 ? SetG16 : SetG32;
                gmask = ((1u << (int)gbits) - 1) << (int)goffset;
                gscaleinv = gmask;
                gscale = 1.0f / gscaleinv;
            }
            if (bbits != 0)
            {
                pGetB = bpp == 1 ? (GetChannel)GetB8 : bpp == 2 ? GetB16 : GetB32;
                pSetB = bpp == 1 ? (SetChannel)SetB8 : bpp == 2 ? SetB16 : SetB32;
                bmask = ((1u << (int)bbits) - 1) << (int)boffset;
                bscaleinv = bmask;
                bscale = 1.0f / bscaleinv;
            }
            if (abits != 0)
            {
                pGetA = bpp == 1 ? (GetChannel)GetA8 : bpp == 2 ? GetA16 : GetA32;
                pSetA = bpp == 1 ? (SetChannel)SetA8 : bpp == 2 ? SetA16 : SetA32;
                amask = ((1u << (int)abits) - 1) << (int)aoffset;
                ascaleinv = amask;
                ascale = 1.0f / ascaleinv;
            }
        }

        private static uint BytesPerPixelFromFormat(eTexFormat format)
        {
            switch (format)
            {
                case eTexFormat.A8:
                case eTexFormat.L8:
                    return 1;
                case eTexFormat.R5G6B5:
                case eTexFormat.X1R5G5B5:
                case eTexFormat.A1R5G5B5:
                case eTexFormat.A4R4G4B4:
                case eTexFormat.A8L8:
                    return 2;
                case eTexFormat.A8R8G8B8:
                case eTexFormat.X8R8G8B8:
                    return 4;
                default:
                    return 0;
            }
        }
    }

}
