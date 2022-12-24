// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* DataViewModel.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reactive.Linq;

using AM;
using AM.Avalonia;
using AM.Avalonia.AppServices;

using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.ReactiveUI;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace AvaloniaApp;

/// <summary>
///
/// </summary>
public class DataViewModel
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Первое слагаемое.
    /// </summary>
    [Reactive]
    public double FirstTerm { get; set; }

    /// <summary>
    /// Второе слагаемое.
    /// </summary>
    [Reactive]
    public double SecondTerm { get; set; }

    /// <summary>
    /// Сумма.
    /// </summary>
    [ObservableAsProperty]
    public double Sum => 0;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Model()
    {
        this.WhenAnyValue
                (
                    first => first.FirstTerm,
                    second => second.SecondTerm
                )
            .Select
                (
                    data => data.Item1 + data.Item2
                )
            .ToPropertyEx (this, vm => vm.Sum);
    }

    #endregion
}
