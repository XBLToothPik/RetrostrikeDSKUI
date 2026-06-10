using System;
using System.Collections.Generic;
using System.Text;

namespace RetrostrikeDSKUI.Core
{
    public static class Utils
    {
        public static string ShortenPath(string path, int maxLength = 30)
        {
            if (string.IsNullOrEmpty(path) || path.Length <= maxLength)
                return path;

            const string ellipsis = "...";
            char sep = path.Contains('\\') ? '\\' : '/';

            string root = Path.GetPathRoot(path) ?? "";
            string fileName = Path.GetFileName(path);

            // Minimum we always show: root + ... + filename
            string minimal = $"{root}{ellipsis}{sep}{fileName}";
            if (minimal.Length >= maxLength)
                return minimal;

            string[] parts = path.Split(sep, StringSplitOptions.RemoveEmptyEntries);

            // Build from the start, adding leading directories until we'd exceed maxLength
            var sb = new StringBuilder(root.TrimEnd(sep));
            int startIndex = root.Length > 0 ? 1 : 0; // skip drive part already in root

            for (int i = startIndex; i < parts.Length - 1; i++)
            {
                string candidate = $"{sb}{sep}{parts[i]}{sep}{ellipsis}{sep}{fileName}";
                if (candidate.Length > maxLength)
                    break;
                sb.Append(sep).Append(parts[i]);
            }

            return $"{sb}{sep}{ellipsis}{sep}{fileName}";
        }
    }
}
