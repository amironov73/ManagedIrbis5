// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RtfStyleDescriptor.cs -- описывает RTF-стиль
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Описывает RTF-стиль.
/// </summary>
public sealed class RtfStyleDescriptor
{
    #region Properties

    /// <summary>
    /// Цвет текста.
    /// </summary>
    public Color ForeColor { get; set; }

    /// <summary>
    /// Цвет фона.
    /// </summary>
    public Color BackColor { get; set; }

    /// <summary>
    /// Дополнительные теги.
    /// </summary>
    public string? AdditionalTags { get; set; }

    #endregion
}
