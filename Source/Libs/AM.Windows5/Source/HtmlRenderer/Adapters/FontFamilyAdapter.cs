// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* FontFamilyAdapter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Markup;
using System.Windows.Media;

using AM.Drawing.HtmlRenderer.Adapters;

#endregion

#nullable enable

namespace AM.Windows.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for WPF Font family object for core.
/// </summary>
internal sealed class FontFamilyAdapter 
    : RFontFamily
{
    /// <summary>
    /// Default language to get font family name by
    /// </summary>
    private static readonly XmlLanguage _xmlLanguage = XmlLanguage.GetLanguage("en-us");

    /// <summary>
    /// the underline win-forms font.
    /// </summary>
    private readonly FontFamily _fontFamily;

    /// <summary>
    /// Init.
    /// </summary>
    public FontFamilyAdapter(FontFamily fontFamily)
    {
        _fontFamily = fontFamily;
    }

    /// <summary>
    /// the underline WPF font family.
    /// </summary>
    public FontFamily FontFamily => _fontFamily;

    public override string Name
    {
        get
        {
            string name =  _fontFamily.FamilyNames[_xmlLanguage];
            if (string.IsNullOrEmpty(name))
            {
                foreach (var familyName in _fontFamily.FamilyNames)
                {
                    return familyName.Value;
                }
            }
            return name;
        }
    }
}