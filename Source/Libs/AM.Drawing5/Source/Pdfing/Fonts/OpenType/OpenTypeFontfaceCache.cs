// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* OpenTypeFontfaceCache.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using PdfSharpCore.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Fonts.OpenType
{
    /// <summary>
    /// Global table of all OpenType fontfaces chached by their face name and check sum.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    internal class OpenTypeFontfaceCache
    {
        OpenTypeFontfaceCache()
        {
            _fontfaceCache = new Dictionary<string, OpenTypeFontface>(StringComparer.OrdinalIgnoreCase);
            _fontfacesByCheckSum = new Dictionary<ulong, OpenTypeFontface>();
        }

        /// <summary>
        /// Tries to get fontface by its key.
        /// </summary>
        public static bool TryGetFontface(string key, out OpenTypeFontface? fontface)
        {
            try
            {
                Lock.EnterFontFactory();
                var result = Singleton._fontfaceCache.TryGetValue(key, out fontface);
                return result;
            }
            finally { Lock.ExitFontFactory(); }
        }

        /// <summary>
        /// Tries to get fontface by its check sum.
        /// </summary>
        public static bool TryGetFontface(ulong checkSum, out OpenTypeFontface? fontface)
        {
            try
            {
                Lock.EnterFontFactory();
                var result = Singleton._fontfacesByCheckSum.TryGetValue(checkSum, out fontface);
                return result;
            }
            finally { Lock.ExitFontFactory(); }
        }

        public static OpenTypeFontface AddFontface(OpenTypeFontface fontface)
        {
            try
            {
                Lock.EnterFontFactory();
                if (TryGetFontface(fontface.FullFaceName, out var fontfaceCheck))
                {
                    if (fontfaceCheck!.CheckSum != fontface.CheckSum)
                    {
                        throw new InvalidOperationException("OpenTypeFontface with same signature but different bytes.");
                    }

                    return fontfaceCheck;
                }
                Singleton._fontfaceCache.Add(fontface.FullFaceName, fontface);
                Singleton._fontfacesByCheckSum.Add(fontface.CheckSum, fontface);
                return fontface;
            }
            finally { Lock.ExitFontFactory(); }
        }

        /// <summary>
        /// Gets the singleton.
        /// </summary>
        static OpenTypeFontfaceCache Singleton
        {
            get
            {
                // ReSharper disable once InvertIf
                if (_singleton == null)
                {
                    try
                    {
                        Lock.EnterFontFactory();
                        _singleton ??= new OpenTypeFontfaceCache();
                    }
                    finally { Lock.ExitFontFactory(); }
                }
                return _singleton;
            }
        }

        static volatile OpenTypeFontfaceCache? _singleton;

        internal static string GetCacheState()
        {
            var state = new StringBuilder();
            state.Append("====================\n");
            state.Append("OpenType font faces by name\n");
            var familyKeys = Singleton._fontfaceCache.Keys;
            var count = familyKeys.Count;
            var keys = new string[count];
            familyKeys.CopyTo(keys, 0);
            Array.Sort(keys, StringComparer.OrdinalIgnoreCase);
            foreach (var key in keys)
                state.AppendFormat("  {0}: {1}\n", key, Singleton._fontfaceCache[key].DebuggerDisplay);
            state.Append("\n");
            return state.ToString();
        }

        /// <summary>
        /// Maps face name to OpenType fontface.
        /// </summary>
        readonly Dictionary<string, OpenTypeFontface> _fontfaceCache;

        /// <summary>
        /// Maps font source key to OpenType fontface.
        /// </summary>
        readonly Dictionary<ulong, OpenTypeFontface> _fontfacesByCheckSum;

        /// <summary>
        /// Gets the DebuggerDisplayAttribute text.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        string DebuggerDisplay => string.Format(CultureInfo.InvariantCulture, "Font faces: {0}", _fontfaceCache.Count); // ReSharper restore UnusedMember.Local
    }
}
