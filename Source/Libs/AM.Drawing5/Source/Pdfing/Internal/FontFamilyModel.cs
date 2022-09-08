// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* FontFamilyModel.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;

using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace PdfSharpCore.Internal;

/// <summary>
///
/// </summary>
public class FontFamilyModel
{
    #region Properties

    /// <summary>
    /// Имя шрифта.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Соответствие стилей шрифтов дисковым файлам.
    /// </summary>
    public Dictionary<XFontStyle, string> FontFiles = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка, доступен ли указанный стиль шрифта/
    /// </summary>
    public bool IsStyleAvailable
        (
            XFontStyle fontStyle
        )
    {
        Sure.Defined (fontStyle);

        return FontFiles.ContainsKey (fontStyle);
    }

    #endregion
}
