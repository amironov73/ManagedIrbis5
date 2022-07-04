// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* FontFamilyCache.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

using PdfSharpCore.Fonts;
using PdfSharpCore.Fonts.OpenType;
using PdfSharpCore.Internal;
using PdfSharpCore.Pdf;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Global cache of all internal font family objects.
    /// </summary>
    internal sealed class FontFamilyCache
    {
        FontFamilyCache()
        {
            _familiesByName = new Dictionary<string, FontFamilyInternal>(StringComparer.OrdinalIgnoreCase);
        }

        public static FontFamilyInternal GetFamilyByName(string familyName)
        {
            try
            {
                Lock.EnterFontFactory();
                FontFamilyInternal family;
                Singleton._familiesByName.TryGetValue(familyName, out family);
                return family;
            }
            finally { Lock.ExitFontFactory(); }
        }

        /// <summary>
        /// Caches the font family or returns a previously cached one.
        /// </summary>
        public static FontFamilyInternal CacheOrGetFontFamily(FontFamilyInternal fontFamily)
        {
            try
            {
                Lock.EnterFontFactory();
                // Recall that a font family is uniquely identified by its case insensitive name.
                FontFamilyInternal existingFontFamily;
                if (Singleton._familiesByName.TryGetValue(fontFamily.Name, out existingFontFamily))
                {
#if DEBUG_
                    if (fontFamily.Name == "xxx")
                        fontFamily.GetType();
#endif
                    return existingFontFamily;
                }
                Singleton._familiesByName.Add(fontFamily.Name, fontFamily);
                return fontFamily;
            }
            finally { Lock.ExitFontFactory(); }
        }

        /// <summary>
        /// Gets the singleton.
        /// </summary>
        static FontFamilyCache Singleton
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
                            _singleton = new FontFamilyCache();
                    }
                    finally { Lock.ExitFontFactory(); }
                }
                return _singleton;
            }
        }
        static volatile FontFamilyCache _singleton;

        internal static string GetCacheState()
        {
            StringBuilder state = new StringBuilder();
            state.Append("====================\n");
            state.Append("Font families by name\n");
            Dictionary<string, FontFamilyInternal>.KeyCollection familyKeys = Singleton._familiesByName.Keys;
            int count = familyKeys.Count;
            string[] keys = new string[count];
            familyKeys.CopyTo(keys, 0);
            Array.Sort(keys, StringComparer.OrdinalIgnoreCase);
            foreach (string key in keys)
                state.AppendFormat("  {0}: {1}\n", key, Singleton._familiesByName[key].DebuggerDisplay);
            state.Append("\n");
            return state.ToString();
        }

        /// <summary>
        /// Maps family name to internal font family.
        /// </summary>
        readonly Dictionary<string, FontFamilyInternal> _familiesByName;
    }
}
