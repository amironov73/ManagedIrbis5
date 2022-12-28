// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* GateModel.cs -- модель данных пропускного пункта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.ObjectModel;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace Gatekeeper;

/// <summary>
/// Модель данных пропускного пункта.
/// </summary>
internal sealed class GateModel
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// События.
    /// </summary>
    [Reactive]
    public ObservableCollection<EventModel>? Events { get; set; }

    /// <summary>
    /// Посещений за сегодня.
    /// </summary>
    [Reactive]
    public int VisitCount { get; set; }

    /// <summary>
    /// Количество читателей в библиотеке.
    /// </summary>
    [Reactive]
    public int InsiderCount { get; set; }

    /// <summary>
    /// Обращение к охранникам.
    /// </summary>
    [Reactive]
    public string? Message { get; set; }

    /// <summary>
    /// Последний посетитель.
    /// </summary>
    [Reactive]
    public string? Last { get; set; }

    /// <summary>
    /// Штрих-код приходящего/выходящего читателя.
    /// </summary>
    [Reactive]
    public string? Barcode { get; set; }

    #endregion

    #region Public methods

    public static GateModel GetTestModel()
    {
        var result = new GateModel
        {
            Message = "Смотрите, кто пришел",
            Last = "Пока никто не приходил",
            Events = new ObservableCollection<EventModel> (EventModel.GetTestEvents)
        };

        return result;
    }

    #endregion
}
