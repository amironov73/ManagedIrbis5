// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainView.cs -- главное View приложения
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

namespace AvaloniaViewApp;

/// <summary>
/// Главное окно приложения
/// </summary>
public sealed class MainView
    : ReactiveUserControl<MainView.Model>
{
    #region View model

    /// <summary>
    /// Модель данных.
    /// </summary>
    public sealed class Model
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

    #endregion

    #region Window members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Width = MinWidth = 400;
        Height = MinHeight = 250;

        DataContext = new Model
        {
            FirstTerm = 123.45,
            SecondTerm = 567.89
        };

        Content = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 5,
            Children =
            {
                CreateTextBox (nameof (Model.FirstTerm)),
                CreateTextBox (nameof (Model.SecondTerm)),
                CreateTextBox (nameof (Model.Sum), isReadOnly: true)
            }
        };

        TextBox CreateTextBox (string propertyName, bool isReadOnly = false) =>
            new()
            {
                Name = propertyName,
                Width = 200,
                IsReadOnly = isReadOnly,
                HorizontalAlignment = HorizontalAlignment.Center,
                [!TextBox.TextProperty] = new Binding (propertyName)
            };
    }

    #endregion

    #region Program entry point

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    [STAThread]
    public static void Main
        (
            string[] args
        )
    {
        ViewApplication
            .Run<MainView> (args);
    }

    #endregion
}
