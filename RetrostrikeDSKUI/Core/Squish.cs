// -----------------------------------------------------------------------------
//  C# P/Invoke wrapper for the squish DXT/BCn compression library (Squish.dll).
// -----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Squish
{
    /// <summary>Compression flags. Combine with bitwise OR.</summary>
    [Flags]
    public enum SquishFlags
    {
        /// <summary>Use DXT1 compression.</summary>
        Dxt1 = 1 << 0,
        /// <summary>Use DXT3 compression.</summary>
        Dxt3 = 1 << 1,
        /// <summary>Use DXT5 compression.</summary>
        Dxt5 = 1 << 2,
        /// <summary>Use BC4 compression.</summary>
        Bc4 = 1 << 3,
        /// <summary>Use BC5 compression.</summary>
        Bc5 = 1 << 4,
        /// <summary>Slow but high quality colour compressor (the default).</summary>
        ColourClusterFit = 1 << 5,
        /// <summary>Fast but low quality colour compressor.</summary>
        ColourRangeFit = 1 << 6,
        /// <summary>Weight the colour by alpha during cluster fit (off by default).</summary>
        WeightColourByAlpha = 1 << 7,
        /// <summary>Very slow but very high quality colour compressor.</summary>
        ColourIterativeClusterFit = 1 << 8,
        /// <summary>Source is BGRA rather than RGBA.</summary>
        SourceBGRA = 1 << 9,
    }

    /// <summary>
    /// Managed wrapper over Squish.dll. The Native class holds the raw P/Invoke
    /// declarations; the public static methods provide a friendlier, safe API
    /// using byte[] / float[] instead of raw pointers.
    /// </summary>
    public static class SquishLib
    {
        private const string Dll = "Squish.dll";
        internal static class Native
        {
            // void CompressMasked(u8 const* rgba, int mask, void* block, int flags, float* metric)
            [DllImport(Dll, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "?CompressMasked@squish@@YAXPEBEHPEAXHPEAM@Z")]
            internal static extern void CompressMasked(
                byte[] rgba, int mask, byte[] block, int flags, float[] metric);

            // void Decompress(u8* rgba, void const* block, int flags)
            [DllImport(Dll, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "?Decompress@squish@@YAXPEAEPEBXH@Z")]
            internal static extern void Decompress(
                byte[] rgba, byte[] block, int flags);

            // int GetStorageRequirements(int width, int height, int flags)
            [DllImport(Dll, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "?GetStorageRequirements@squish@@YAHHHH@Z")]
            internal static extern int GetStorageRequirements(
                int width, int height, int flags);

            // void CompressImage(u8 const* rgba, int width, int height, int pitch, void* blocks, int flags, float* metric)
            [DllImport(Dll, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "?CompressImage@squish@@YAXPEBEHHHPEAXHPEAM@Z")]
            internal static extern void CompressImagePitch(
                byte[] rgba, int width, int height, int pitch,
                byte[] blocks, int flags, float[] metric);

            // void CompressImage(u8 const* rgba, int width, int height, void* blocks, int flags, float* metric)
            [DllImport(Dll, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "?CompressImage@squish@@YAXPEBEHHPEAXHPEAM@Z")]
            internal static extern void CompressImage(
                byte[] rgba, int width, int height,
                byte[] blocks, int flags, float[] metric);

            // void DecompressImage(u8* rgba, int width, int height, int pitch, void const* blocks, int flags)
            [DllImport(Dll, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "?DecompressImage@squish@@YAXPEAEHHHPEBXH@Z")]
            internal static extern void DecompressImagePitch(
                byte[] rgba, int width, int height, int pitch,
                byte[] blocks, int flags);

            // void DecompressImage(u8* rgba, int width, int height, void const* blocks, int flags)
            [DllImport(Dll, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "?DecompressImage@squish@@YAXPEAEHHPEBXH@Z")]
            internal static extern void DecompressImage(
                byte[] rgba, int width, int height,
                byte[] blocks, int flags);

            // void ComputeMSE(u8 const* rgba, int w, int h, int pitch, u8 const* dxt, int flags, double& colourMSE, double& alphaMSE)
            [DllImport(Dll, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "?ComputeMSE@squish@@YAXPEBEHHH0HAEAN1@Z")]
            internal static extern void ComputeMSEPitch(
                byte[] rgba, int width, int height, int pitch,
                byte[] dxt, int flags, out double colourMSE, out double alphaMSE);

            // void ComputeMSE(u8 const* rgba, int w, int h, u8 const* dxt, int flags, double& colourMSE, double& alphaMSE)
            [DllImport(Dll, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "?ComputeMSE@squish@@YAXPEBEHH0HAEAN1@Z")]
            internal static extern void ComputeMSE(
                byte[] rgba, int width, int height,
                byte[] dxt, int flags, out double colourMSE, out double alphaMSE);
        }

        // ---------------------------------------------------------------------
        // Friendly public API.
        // ---------------------------------------------------------------------

        /// <summary>Bytes per compressed block: 8 for DXT1/BC4, 16 otherwise.</summary>
        public static int BytesPerBlock(SquishFlags flags)
        {
            if ((flags & SquishFlags.Dxt1) != 0) return 8;
            if ((flags & SquishFlags.Bc4) != 0) return 8;
            return 16;
        }

        /// <summary>
        /// Compresses a single 4x4 block (64 RGBA bytes) into a DXT/BCn block.
        /// </summary>
        /// <param name="rgba">16 pixels * 4 bytes = 64 bytes.</param>
        /// <param name="mask">Valid-pixel mask; low 16 bits enable pixels 1..16. Use 0xffff for all.</param>
        /// <param name="flags">Compression flags.</param>
        /// <param name="metric">Optional 3-float channel weights, or null for {1,1,1}.</param>
        public static byte[] CompressMasked(byte[] rgba, int mask, SquishFlags flags, float[] metric = null)
        {
            if (rgba == null) throw new ArgumentNullException(nameof(rgba));
            if (rgba.Length < 64) throw new ArgumentException("rgba must be at least 64 bytes (16 pixels).", nameof(rgba));
            ValidateMetric(metric);

            var block = new byte[BytesPerBlock(flags)];
            Native.CompressMasked(rgba, mask, block, (int)flags, metric);
            return block;
        }

        /// <summary>
        /// Compresses a single 4x4 block (all 16 pixels enabled).
        /// </summary>
        public static byte[] Compress(byte[] rgba, SquishFlags flags, float[] metric = null)
            => CompressMasked(rgba, 0xffff, flags, metric);

        /// <summary>
        /// Decompresses a single DXT/BCn block into 64 RGBA bytes.
        /// </summary>
        public static byte[] Decompress(byte[] block, SquishFlags flags)
        {
            if (block == null) throw new ArgumentNullException(nameof(block));
            var rgba = new byte[64];
            Native.Decompress(rgba, block, (int)flags);
            return rgba;
        }

        /// <summary>
        /// Computes the number of bytes needed to store the compressed image.
        /// </summary>
        public static int GetStorageRequirements(int width, int height, SquishFlags flags)
            => Native.GetStorageRequirements(width, height, (int)flags);

        /// <summary>
        /// Compresses a whole image. Returns the compressed block data.
        /// </summary>
        /// <param name="rgba">width*height*4 bytes of RGBA (or BGRA if SourceBGRA set).</param>
        /// <param name="pitch">Bytes per row; pass 0 to use width*4.</param>
        public static byte[] CompressImage(byte[] rgba, int width, int height, SquishFlags flags,
                                           int pitch = 0, float[] metric = null)
        {
            if (rgba == null) throw new ArgumentNullException(nameof(rgba));
            ValidateMetric(metric);

            var blocks = new byte[GetStorageRequirements(width, height, flags)];
            if (pitch > 0)
                Native.CompressImagePitch(rgba, width, height, pitch, blocks, (int)flags, metric);
            else
                Native.CompressImage(rgba, width, height, blocks, (int)flags, metric);
            return blocks;
        }

        /// <summary>
        /// Decompresses a whole image into RGBA bytes (width*height*4).
        /// </summary>
        /// <param name="pitch">Bytes per row of the output; pass 0 to use width*4.</param>
        public static byte[] DecompressImage(byte[] blocks, int width, int height, SquishFlags flags, int pitch = 0)
        {
            if (blocks == null) throw new ArgumentNullException(nameof(blocks));

            var rgba = new byte[width * height * 4];
            if (pitch > 0)
                Native.DecompressImagePitch(rgba, width, height, pitch, blocks, (int)flags);
            else
                Native.DecompressImage(rgba, width, height, blocks, (int)flags);
            return rgba;
        }

        /// <summary>
        /// Computes the mean squared error between an original image and its compressed form.
        /// </summary>
        public static (double colourMSE, double alphaMSE) ComputeMSE(
            byte[] rgba, int width, int height, byte[] dxt, SquishFlags flags, int pitch = 0)
        {
            if (rgba == null) throw new ArgumentNullException(nameof(rgba));
            if (dxt == null) throw new ArgumentNullException(nameof(dxt));

            double colour, alpha;
            if (pitch > 0)
                Native.ComputeMSEPitch(rgba, width, height, pitch, dxt, (int)flags, out colour, out alpha);
            else
                Native.ComputeMSE(rgba, width, height, dxt, (int)flags, out colour, out alpha);
            return (colour, alpha);
        }

        private static void ValidateMetric(float[] metric)
        {
            if (metric != null && metric.Length != 3)
                throw new ArgumentException("metric must be null or exactly 3 floats.", nameof(metric));
        }
    }
}
