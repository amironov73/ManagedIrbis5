// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Global

/* FieldModel.cs -- модель данных редактируемого поля
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable


namespace ManagedIrbis.Avalonia.Models;

/// <summary>
/// Модель данных редактируемого поля библиографической записи.
/// </summary>
public class FieldModel
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Метка поля.
    /// </summary>
    [Reactive]
    public int Tag { get; set; }

    /// <summary>
    /// Значение поля.
    /// </summary>
    [Reactive]
    public string? Text { get; set; }


    #endregion
}
