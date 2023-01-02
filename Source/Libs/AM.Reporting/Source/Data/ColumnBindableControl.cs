// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ColumnBindableControl.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Reporting.Data;

/// <summary>
/// Specifies the type of an object that will be created when you drop the
/// data column on a report page.
/// </summary>
public enum ColumnBindableControl
{
    /// <summary>
    /// The column will create the <see cref="TextObject"/> object.
    /// </summary>
    Text,

    /// <summary>
    /// The column will create the Rich Text object.
    /// </summary>
    RichText,

    /// <summary>
    /// The column will create the <see cref="PictureObject"/> object.
    /// </summary>
    Picture,

    /// <summary>
    /// The column will create the <see cref="CheckBoxObject"/> object.
    /// </summary>
    CheckBox,

    /// <summary>
    /// The column will create the custom object, specified in the
    /// <see cref="Column.CustomBindableControl"/> property.
    /// </summary>
    Custom
}
