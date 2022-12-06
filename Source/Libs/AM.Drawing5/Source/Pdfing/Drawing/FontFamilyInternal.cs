// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* FontFamilyInternal.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Globalization;

using AM;

using PdfSharpCore.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing;

/// <summary>
/// Internal implementation class of XFontFamily.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay}")]
internal class FontFamilyInternal
{
    // Implementation Notes
    // FontFamilyInternal implements an XFontFamily.
    //
    // * Each XFontFamily object is just a handle to its FontFamilyInternal singleton.
    //
    // * A FontFamilyInternal is is uniquely identified by its name. It
    //    is not possible to use two different fonts that have the same
    //    family name.

    FontFamilyInternal(string familyName, bool createPlatformObjects)
    {
        createPlatformObjects.NotUsed();

        SourceName = Name = familyName;
    }

    internal static FontFamilyInternal GetOrCreateFromName
        (
            string familyName,
            bool createPlatformObject
        )
    {
        try
        {
            Lock.EnterFontFactory();
            var family = FontFamilyCache.GetFamilyByName(familyName);
            if (family == null)
            {
                family = new FontFamilyInternal(familyName, createPlatformObject);
                family = FontFamilyCache.CacheOrGetFontFamily(family);
            }
            return family;
        }
        finally { Lock.ExitFontFactory(); }
    }

    /// <summary>
    /// Gets the family name this family was originally created with.
    /// </summary>
    public string SourceName { get; }

    /// <summary>
    /// Gets the name that uniquely identifies this font family.
    /// </summary>
    public string Name
    {
        // In WPF this is the Win32FamilyName, not the WPF family name.
        get;
    }

    /// <summary>
    /// Gets the DebuggerDisplayAttribute text.
    /// </summary>
    // ReSha rper disable UnusedMember.Local
    internal string DebuggerDisplay => string.Format(CultureInfo.InvariantCulture, "FontFamily: '{0}'", Name); // ReShar per restore UnusedMember.Local
}
