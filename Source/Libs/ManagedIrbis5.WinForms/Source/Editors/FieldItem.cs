// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Global

/* FieldItem.cs -- содержимое редактируемого поля
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.WinForms.Editors;

/// <summary>
/// Содержимое редактируемого поля библиографической записи
/// для <see cref="SimplestMarcEditor"/>.
/// </summary>
internal sealed class FieldItem
{
    #region Properties

    /// <summary>
    /// Метка поля.
    /// </summary>
    public int Tag { get; set; }

    /// <summary>
    /// Значение поля.
    /// </summary>
    public string? Text { get; set; }

    #endregion
}
