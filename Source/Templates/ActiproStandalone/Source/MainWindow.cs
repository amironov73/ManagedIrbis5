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
using Avalonia.Media;
using Avalonia.ReactiveUI;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using static AvaloniaApp.ActiproUtility;

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
        this.AttachDevTools();

        base.OnInitialized();

        Styles.Add (CreateStandardStyles());

        Title = "Форма ввода с Modern-темой Actipro";
        Width = MinWidth = 650;
        Height = MinHeight = 450;

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
                ButtonWithGlyph (GlyphTemplateKind.Menu16)
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
            Content = "Форма ввода с Modern-темой Actipro"
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
                    Header1 ("Profile Form"),
                    BodyText ("This sample demonstrates how Actipro's control themes can create professional-looking forms, such as for collecting user profile data."),

                    Header2 ("Public Profile"),
                    BodyText ("This profile information is shared publicly with all users."),

                    FormInputGroup
                        (
                            FormInput (LabeledControl
                                (
                                    "Bio",
                                    new TextBox
                                    {
                                        TextWrapping = TextWrapping.Wrap,
                                        MaxLines = 5,
                                        AcceptsReturn = true
                                    }
                                    .Accent(),
                                    "Any details such as age, occupation, interests."
                                ))
                        ),

                    FormInputGroup
                        (
                            new Grid
                            {
                                ColumnDefinitions = ColumnDefinitions.Parse ("*,30,*"),
                                Children =
                                {
                                    FormInput (LabeledControl
                                        (
                                            "First name",
                                            new TextBox().Success()
                                        )),

                                    FormInput (LabeledControl
                                        (
                                            "Last name",
                                            new TextBox().Warning()
                                        ))
                                        .SetColumn (2)
                                }
                            },

                            new Grid
                            {
                                ColumnDefinitions = ColumnDefinitions.Parse ("*,30,*,30,*"),
                                Children =
                                {
                                    FormInput (LabeledControl
                                        (
                                            "Phone number",
                                            new TextBox().Danger()
                                        ),
                                        new CheckBox
                                        {
                                            Margin = new Thickness (0, 10, 0, 0),
                                            Content = "Allow text messages"
                                        }
                                        .Danger()
                                        ),

                                    FormInput ( LabeledControl
                                        (
                                            "E-mail address",
                                            new TextBox()
                                        ))
                                        .SetColumn (2, 4)

                                }
                            },

                            FormInput ( LabeledControl
                                (
                                    "Preferred contact method",
                                    new StackPanel
                                    {
                                        Orientation = Orientation.Horizontal,
                                        Children =
                                        {
                                            new RadioButton
                                            {
                                                Content = "E-mail",
                                                IsChecked = true
                                            },
                                            new RadioButton
                                            {
                                                Content = "Voice"
                                            },
                                            new RadioButton
                                            {
                                                Content = "Text"
                                            }
                                        }
                                    }
                                )),

                            new Grid
                            {
                                ColumnDefinitions = ColumnDefinitions.Parse ("*,30,*"),
                                Children =
                                {
                                    FormInput ( LabeledControl
                                        (
                                            "Country",
                                            new ComboBox
                                            {
                                                Items =
                                                {
                                                    "Canada",
                                                    "Mexico",
                                                    "United States"
                                                },
                                                SelectedIndex = 2,
                                            }
                                        )),

                                    FormInput (LabeledControl
                                        (
                                            "Street address",
                                            new TextBox()
                                        ))
                                        .SetColumn (2)
                                }
                            },

                            new Grid
                            {
                                ColumnDefinitions = ColumnDefinitions.Parse ("*,30,*,30,*"),
                                Children =
                                {
                                    FormInput ( LabeledControl
                                        (
                                            "City",
                                            new TextBox()
                                        )),
                                    FormInput ( LabeledControl
                                        (
                                            "State / Province",
                                            new TextBox()
                                        ))
                                        .SetColumn (2),
                                    FormInput ( LabeledControl
                                        (
                                            "ZIP / Postal code",
                                            new TextBox()
                                        ))
                                        .SetColumn (4)
                                }
                            }
                        ),

                    new StackPanel
                    {
                        Height = 30,
                    },

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
