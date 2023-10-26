// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo

/* InventoryModel.cs -- модель для главной формы инвентаризации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.ObjectModel;

using JetBrains.Annotations;

using ManagedIrbis;
using ManagedIrbis.Fields;
using ManagedIrbis.Menus;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

namespace Inventory2023;

/// <summary>
/// Модель для главной формы инвентаризации.
/// </summary>
[PublicAPI]
public class InventoryModel
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Перечень фондов библиотеки.
    /// </summary>
    [Reactive]
    public ObservableCollection<MenuEntry>? KnownFonds { get; set; }

    /// <summary>
    /// Текущий фонд.
    /// </summary>
    [Reactive]
    public MenuEntry? CurrentFond { get; set; }

    /// <summary>
    /// Проверяемый экземпляр.
    /// </summary>
    [Reactive]
    public string? CurrentNumber { get; set; }

    /// <summary>
    /// Библиографическое описание или сообщение об ошибке.
    /// </summary>
    [Reactive]
    public string? Description { get; set; }

    /// <summary>
    /// Текущая проверяемая запись.
    /// </summary>
    [Reactive]
    public Record? CurrentRecord { get; set; }

    /// <summary>
    /// Текущий проверяемый экземпляр.
    /// </summary>
    [Reactive]
    public ExemplarInfo? CurrentExemplar { get; set; }

    /// <summary>
    /// Есть ли ошибка?
    /// </summary>
    [Reactive]
    public bool HasError { get; set; }

    /// <summary>
    /// Список проверенных экземпляров.
    /// </summary>
    public ObservableCollection<InventoryBookInfo> ConfirmedBooks { get; } = new ();

    /// <summary>
    /// Логи.
    /// </summary>
    public ObservableCollection<object> Log { get; } = new ();

    #endregion
}
