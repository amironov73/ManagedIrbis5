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
using PdfSharpCore.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing
{
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
            _sourceName = _name = familyName;
        }

        internal static FontFamilyInternal GetOrCreateFromName(string familyName, bool createPlatformObject)
        {
            try
            {
                Lock.EnterFontFactory();
                FontFamilyInternal family = FontFamilyCache.GetFamilyByName(familyName);
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
        public string SourceName
        {
            get { return _sourceName; }
        }
        readonly string _sourceName;

        /// <summary>
        /// Gets the name that uniquely identifies this font family.
        /// </summary>
        public string Name
        {
            // In WPF this is the Win32FamilyName, not the WPF family name.
            get { return _name; }
        }
        readonly string _name;

        /// <summary>
        /// Gets the DebuggerDisplayAttribute text.
        /// </summary>
        // ReSha rper disable UnusedMember.Local
        internal string DebuggerDisplay
        // ReShar per restore UnusedMember.Local
        {
            get { return string.Format(CultureInfo.InvariantCulture, "FontFamiliy: '{0}'", Name); }
        }
    }
}
