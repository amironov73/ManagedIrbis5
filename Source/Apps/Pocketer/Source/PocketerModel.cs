// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* PocketerModel.cs -- модель данных главного окна
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using AM;
using AM.Avalonia;
using AM.Avalonia.AppServices;
using AM.Avalonia.Controls;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;

using ReactiveUI;

#endregion

namespace Pocketer;

/// <summary>
/// Модель данных главного окна.
/// </summary>
internal sealed class PocketerModel
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Интерфейс занятости.
    /// </summary>
    public IBusyState? Busy { get; set; }

    /// <summary>
    /// Поисковое выражение.
    /// </summary>
    public string? SearchExpression { get; set; }

    /// <summary>
    /// Команда Barsik.
    /// </summary>
    public string? BarsikCommand { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Поиск в ИРБИС.
    /// </summary>
    public async void Search()
    {
        var expression = SearchExpression?.Trim();
        if (!string.IsNullOrEmpty (expression))
        {
            try
            {
                if (Busy is not null)
                {
                    Busy.IsBusy = true;
                }

                await Task.Delay (1000);
            }
            finally
            {
                if (Busy is not null)
                {
                    Busy.IsBusy = false;
                }
            }
        }

    }

    #endregion
}
