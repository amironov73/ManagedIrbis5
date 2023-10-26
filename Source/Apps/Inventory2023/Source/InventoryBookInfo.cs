// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo

/* InventoryBookInfo.cs -- информация о проверенном экземпляре книги
 */

#region Using directives

using JetBrains.Annotations;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

namespace Inventory2023;

/// <summary>
/// Информация о проверенном экземпляре книги.
/// </summary>
[PublicAPI]
public sealed class InventoryBookInfo
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Инвентарный номер проверенного экземпляра.
    /// </summary>
    [Reactive]
    public string? Number { get; set; }

    /// <summary>
    /// Библиографическое описание.
    /// </summary>
    [Reactive]
    public string? Description { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Number} - {Description}";

    #endregion
}
