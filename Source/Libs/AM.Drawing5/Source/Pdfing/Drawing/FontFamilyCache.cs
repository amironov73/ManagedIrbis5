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

using AM.Text;

using PdfSharpCore.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing;

/// <summary>
/// Global cache of all internal font family objects.
/// </summary>
internal sealed class FontFamilyCache
{
    FontFamilyCache()
    {
        _familiesByName = new Dictionary<string, FontFamilyInternal> (StringComparer.OrdinalIgnoreCase);
    }

    public static FontFamilyInternal? GetFamilyByName (string familyName)
    {
        try
        {
            Lock.EnterFontFactory();
            Singleton._familiesByName.TryGetValue (familyName, out var family);
            return family;
        }
        finally
        {
            Lock.ExitFontFactory();
        }
    }

    /// <summary>
    /// Caches the font family or returns a previously cached one.
    /// </summary>
    public static FontFamilyInternal CacheOrGetFontFamily (FontFamilyInternal fontFamily)
    {
        try
        {
            Lock.EnterFontFactory();

            // Recall that a font family is uniquely identified by its case insensitive name.
            if (Singleton._familiesByName.TryGetValue (fontFamily.Name, out var existingFontFamily))
            {
#if DEBUG_
                    if (fontFamily.Name == "xxx")
                        fontFamily.GetType();
#endif
                return existingFontFamily;
            }

            Singleton._familiesByName.Add (fontFamily.Name, fontFamily);
            return fontFamily;
        }
        finally
        {
            Lock.ExitFontFactory();
        }
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
                    _singleton ??= new FontFamilyCache();
                }
                finally
                {
                    Lock.ExitFontFactory();
                }
            }

            return _singleton;
        }
    }

    static volatile FontFamilyCache? _singleton;

    internal static string GetCacheState()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("====================\n");
        builder.Append ("Font families by name\n");
        var familyKeys = Singleton._familiesByName.Keys;
        var count = familyKeys.Count;
        var keys = new string[count];
        familyKeys.CopyTo (keys, 0);
        Array.Sort (keys, StringComparer.OrdinalIgnoreCase);
        foreach (var key in keys)
            builder.AppendFormat ("  {0}: {1}\n", key, Singleton._familiesByName[key].DebuggerDisplay);
        builder.Append ("\n");
        return builder.ReturnShared();
    }

    /// <summary>
    /// Maps family name to internal font family.
    /// </summary>
    readonly Dictionary<string, FontFamilyInternal> _familiesByName;
}
