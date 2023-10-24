// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo

/* MainWindow.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Reactive.Linq;

using ActiproSoftware.UI.Avalonia.Controls;
using ActiproSoftware.UI.Avalonia.Themes;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.ReactiveUI;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

namespace AvaloniaApp;

/// <summary>
/// Главное окно приложения.
/// </summary>
public sealed class MainWindow
    : ReactiveWindow<MainWindow.Model>
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

        Title = "Калькулятор Avalonia";
        Width = MinWidth = 450;
        Height = MinHeight = 250;

        DataContext = new Model
        {
            FirstTerm = 123.45,
            SecondTerm = 567.89
        };

        var leftContent = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5,
            Children =
            {
                ActiproUtility.ButtonWithGlyph (GlyphTemplateKind.Menu16)
            }
        };

        var rightContent = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 5,
            Children =
            {
                new ToggleThemeButton(),
            }
        };

        var titleBar = new ChromedTitleBar
        {
            LeftContent = leftContent,
            RightContent = rightContent,
            Content = "Калькулятор с Modern-темой"
        };

        var scroller = new ScrollViewer
        {
            [Grid.RowProperty] = 1,
            Content = new StackPanel
            {
                Margin = new Thickness (20),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Spacing = 5,
                Children =
                {
                    CreateTextBox (nameof (Model.FirstTerm)),
                    CreateTextBox (nameof (Model.SecondTerm)),
                    CreateTextBox (nameof (Model.Sum), isReadOnly: true)
                }
            }
        };

        Content = new Grid
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            RowDefinitions =
            {
                new RowDefinition (GridLength.Auto),
                new RowDefinition (GridLength.Star)
            },

            Children =
            {
                titleBar,
                scroller
            }
        };
    }

    #endregion

    #region Private members

    /// <summary>
    /// Создание строки ввода, связанной с  указанным свойством модели
    /// </summary>
    /// <param name="propertyName">Имя свойства.</param>
    /// <param name="isReadOnly">Строка ввода только для чтения?</param>
    /// <returns></returns>
    private static TextBox CreateTextBox
        (
            string propertyName,
            bool isReadOnly = false
        )
    {
        return new TextBox
        {
            Name = propertyName,
            Width = 200,
            IsReadOnly = isReadOnly,
            HorizontalAlignment = HorizontalAlignment.Center,
            [!TextBox.TextProperty] = new Binding (propertyName)
        };
    }

    #endregion
}
