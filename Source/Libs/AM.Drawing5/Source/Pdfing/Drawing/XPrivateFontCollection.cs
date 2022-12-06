// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

/* XPrivateFontCollection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing;

///<summary>
/// Makes fonts that are not installed on the system available within the current application domain.<br/>
/// In Silverlight required for all fonts used in PDF documents.
/// </summary>
public sealed class XPrivateFontCollection
{
    // This one is global and can only grow. It is not possible to remove fonts that have been added.

    /// <summary>
    /// Initializes a new instance of the <see cref="XPrivateFontCollection"/> class.
    /// </summary>
    XPrivateFontCollection()
    {
        // HACK: Use one global PrivateFontCollection in GDI+
    }

    /// <summary>
    /// Gets the global font collection.
    /// </summary>
    internal static XPrivateFontCollection Singleton => _singleton;

    internal static XPrivateFontCollection _singleton = new ();

    static string MakeKey(string familyName, XFontStyle style)
    {
        return MakeKey(familyName, (style & XFontStyle.Bold) != 0, (style & XFontStyle.Italic) != 0);
    }

    static string MakeKey(string familyName, bool bold, bool italic)
    {
        return familyName + "#" + (bold ? "b" : "") + (italic ? "i" : "");
    }

    readonly Dictionary<string, XGlyphTypeface> _typefaces = new ();
}
