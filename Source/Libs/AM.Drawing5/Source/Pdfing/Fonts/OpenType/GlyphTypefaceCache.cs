// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* GlyphTypefaceCache.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

using PdfSharpCore.Drawing;
using PdfSharpCore.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Fonts.OpenType
{
    /// <summary>
    /// Global table of all glyph typefaces.
    /// </summary>
    internal class GlyphTypefaceCache
    {
        GlyphTypefaceCache()
        {
            _glyphTypefacesByKey = new ConcurrentDictionary<string, XGlyphTypeface>();
        }

        public static bool TryGetGlyphTypeface(string key, out XGlyphTypeface glyphTypeface)
        {
            try
            {
                Lock.EnterFontFactory();
                bool result = Singleton._glyphTypefacesByKey.TryGetValue(key, out glyphTypeface);
                return result;
            }
            finally { Lock.ExitFontFactory(); }
        }

        public static void AddGlyphTypeface(XGlyphTypeface glyphTypeface)
        {
            try
            {
                Lock.EnterFontFactory();
                GlyphTypefaceCache cache = Singleton;
                cache._glyphTypefacesByKey.TryAdd(glyphTypeface.Key, glyphTypeface);
            }
            finally { Lock.ExitFontFactory(); }
        }

        /// <summary>
        /// Gets the singleton.
        /// </summary>
        static GlyphTypefaceCache Singleton
        {
            get
            {
                // ReSharper disable once InvertIf
                if (_singleton == null)
                {
                    try
                    {
                        Lock.EnterFontFactory();
                        if (_singleton == null)
                            _singleton = new GlyphTypefaceCache();
                    }
                    finally { Lock.ExitFontFactory(); }
                }
                return _singleton;
            }
        }
        static volatile GlyphTypefaceCache _singleton;

        internal static string GetCacheState()
        {
            StringBuilder state = new StringBuilder();
            state.Append("====================\n");
            state.Append("Glyph typefaces by name\n");
            var familyKeys = Singleton._glyphTypefacesByKey.Keys;
            int count = familyKeys.Count;
            string[] keys = new string[count];
            familyKeys.CopyTo(keys, 0);
            Array.Sort(keys, StringComparer.OrdinalIgnoreCase);
            foreach (string key in keys)
                state.AppendFormat("  {0}: {1}\n", key, Singleton._glyphTypefacesByKey[key].DebuggerDisplay);
            state.Append("\n");
            return state.ToString();
        }

        /// <summary>
        /// Maps typeface key to glyph typeface.
        /// </summary>
        readonly ConcurrentDictionary<string, XGlyphTypeface> _glyphTypefacesByKey;
    }
}
