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

/* ActiproUtility.cs -- полезные методы для Actipro
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ActiproSoftware.UI.Avalonia.Themes;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Styling;

using JetBrains.Annotations;

#endregion

namespace AvaloniaApp;

/// <summary>
/// Полезные методы для Actipro.
/// </summary>
[PublicAPI]
public static class ActiproUtility
{
    #region Public methods

    /// <summary>
    /// Установка акцента на указанный контрол.
    /// </summary>
    public static TControl Accent<TControl>
        (
            this TControl control
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Classes.Add ("accent");

        return control;
    }

    /// <summary>
    /// Основной текст на форме.
    /// </summary>
    public static TextBlock BodyText
        (
            string? text = null
        )
        => new ()
        {
            Classes = { "theme-text-body" },
            Text = text
        };

    /// <summary>
    /// Основной текст на форме.
    /// </summary>
    public static TControl BodyText<TControl>
        (
            this TControl control
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Classes.Add ("theme-text-body");

        return control;
    }

    /// <summary>
    /// Создание кнопки с Actipro-глифом.
    /// </summary>
    public static Button ButtonWithGlyph
        (
            GlyphTemplateKind kind
        )
        => new ()
        {
            [!ContentControl.ContentTemplateProperty]
                = new DynamicResourceExtension (kind.ToResourceKey())
        };

    /// <summary>
    /// Моноширинный текст на форме.
    /// </summary>
    public static TControl CodeText<TControl>
        (
            this TControl control
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Classes.Add ("theme-text-code");

        return control;
    }

    /// <summary>
    /// Метка для контрола, принимающего ввод.
    /// </summary>
    public static TextBlock ControlLabel
        (
            string? text = null
        )
        => new ()
        {
            Classes = { "theme-form-control-label" },
            Text = text
        };

    /// <summary>
    /// Подпись для контрола.
    /// </summary>
    public static TControl ControlLabel<TControl>
        (
            this TControl control
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Classes.Add ("theme-form-control-label");

        return control;
    }

    /// <summary>
    /// Поясняющее сообщение для контрола.
    /// </summary>
    public static TextBlock ControlMessage
        (
            string? text
        )
        => new ()
        {
            Classes = { "theme-form-control-message" },
            Text = text
        };

    /// <summary>
    /// Создание стандартных стилей.
    /// </summary>
    public static IStyle CreateStandardStyles()
    {
        return new Style
        {
            Children =
            {
                // стиль h1
                new Style
                {
                    Selector = Selectors
                        .OfType (null, typeof (TextBlock))
                        .Class ("h1"),
                    Setters =
                    {
                        new Setter
                        {
                            Property = Layoutable.MarginProperty,
                            Value = new Thickness (0.0, 0.0, 0.0, 10.0)
                        },
                        new Setter
                        {
                            Property = StyledElement.ThemeProperty,
                            Value = new DynamicResourceExtension (ControlThemeKind.TextBlockHeading.ToResourceKey())
                        },
                        new Setter
                        {
                            Property = TextBlock.FontSizeProperty,
                            Value = new DynamicResourceExtension (ThemeResourceKind.HeadingFontSizeExtraLarge.ToResourceKey())
                        },
                        new Setter
                        {
                            Property = TextBlock.FontWeightProperty,
                            Value = new DynamicResourceExtension (ThemeResourceKind.HeadingFontWeightExtraLarge.ToResourceKey())
                        },
                        new Setter
                        {
                            Property = TextBlock.ForegroundProperty,
                            Value = new DynamicResourceExtension (ThemeResourceKind.HeadingForegroundBrushExtraLarge.ToResourceKey())
                        }
                    }
                },

                // стиль h2
                new Style
                {
                    Selector = Selectors
                        .OfType (null, typeof (TextBlock))
                        .Class ("h2"),
                    Setters =
                    {
                        new Setter
                        {
                            Property = Layoutable.MarginProperty,
                            Value = new Thickness (0.0, 30.0, 0.0, 10.0)
                        },
                        new Setter
                        {
                            Property = StyledElement.ThemeProperty,
                            Value = new DynamicResourceExtension (ControlThemeKind.TextBlockHeading.ToResourceKey())
                        },
                        new Setter
                        {
                            Property = TextBlock.FontSizeProperty,
                            Value = new DynamicResourceExtension (ThemeResourceKind.HeadingFontSizeMedium.ToResourceKey())
                        },
                        new Setter
                        {
                            Property = TextBlock.FontWeightProperty,
                            Value = new DynamicResourceExtension (ThemeResourceKind.HeadingFontWeightMedium.ToResourceKey())
                        },
                        new Setter
                        {
                            Property = TextBlock.ForegroundProperty,
                            Value = new DynamicResourceExtension (ThemeResourceKind.HeadingForegroundBrushMedium.ToResourceKey())
                        }
                    }
                },

                // стиль form-input-group
                new Style
                {
                    Selector = Selectors
                        .OfType (null, typeof (StackPanel))
                        .Class ("form-input-group"),
                    Setters =
                    {
                        new Setter
                        {
                            Property = Layoutable.MarginProperty,
                            Value = new Thickness (0.0, 20.0, 0.0, 0.0)
                        },
                        new Setter
                        {
                            Property = StackPanel.SpacingProperty,
                            Value = 20.0
                        }
                    }
                },

                // стиль form-input
                new Style
                {
                    Selector = Selectors.OfType (null, typeof (StackPanel))
                        .Class ("form-input"),
                    Children =
                    {
                        new Style
                        {
                            Selector = Selectors.Nesting (null)
                                .Descendant().OfType (typeof (CheckBox)),
                            Setters =
                            {
                                new Setter
                                {
                                    Property = Layoutable.MarginProperty,
                                    Value = new Thickness (0.0, 5.0, 0.0, 0.0)
                                }
                            }
                        },

                        new Style
                        {
                            Selector = Selectors.Nesting (null)
                                .Descendant().OfType (typeof (RadioButton)),
                            Setters =
                            {
                                new Setter
                                {
                                    Property = Layoutable.MarginProperty,
                                    Value = new Thickness (0.0, 5.0, 20.0, 0.0)
                                }
                            }
                        }
                    }
                },

                // стиль switch-input-group
                new Style
                {
                    Selector = Selectors.OfType (null, typeof (StackPanel))
                        .Class ("switch-input-group"),
                    Setters =
                    {
                        new Setter
                        {
                            Property = Layoutable.MarginProperty,
                            Value = new Thickness (0.0, 20.0, 0.0, 0.0)
                        },
                        new Setter
                        {
                            Property = StackPanel.SpacingProperty,
                            Value = 10.0
                        }
                    },
                    Children =
                    {
                        new Style
                        {
                            Selector = Selectors.Nesting (null)
                                .Descendant().OfType (typeof (ToggleSwitch)),
                            Setters =
                            {
                                new Setter
                                {
                                    Property = ToggleSwitch.OnContentProperty,
                                    Value = string.Empty
                                },
                                new Setter
                                {
                                    Property = ToggleSwitch.OffContentProperty,
                                    Value = string.Empty
                                }
                            }
                        }
                    }
                },

                // стиль form-buttons
                new Style
                {
                    Selector  = Selectors
                        .OfType (null, typeof (UniformGrid))
                        .Class ("form-buttons"),
                    Setters =
                    {
                        new Setter
                        {
                            Property = Layoutable.HorizontalAlignmentProperty,
                            Value = HorizontalAlignment.Left
                        }
                    },
                    Children =
                    {
                        new Style
                        {
                            Selector = Selectors.Nesting (null)
                                .Descendant().OfType (typeof (Button)),
                            Setters =
                            {
                                new Setter
                                {
                                    Property = Layoutable.MarginProperty,
                                    Value = new Thickness (0.0, 50.0, 10.0, 0.0)
                                }
                            }
                        }
                    }
                },

            }
        };
    }

    /// <summary>
    /// Установка опасности на указанный контрол.
    /// </summary>
    public static TControl Danger<TControl>
        (
            this TControl control
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Classes.Add ("danger");

        return control;
    }

    /// <summary>
    /// Панель для ввода данных.
    /// </summary>
    public static StackPanel FormInput
        (
            params Control[] children
        )
    {
        var result = new StackPanel
        {
            Classes = { "form-input" }
        };

        result.Children.AddRange (children);

        return result;
    }

    /// <summary>
    /// Группа ввода данных.
    /// </summary>
    public static StackPanel FormInputGroup
        (
            params Control[] children
        )
    {
        var result = new StackPanel
        {
            Classes = { "form-input-group" }
        };

        result.Children.AddRange (children);

        return result;
    }

    /// <summary>
    /// Получение модерновой темы от Actipro.
    /// </summary>
    public static ModernTheme? GetModernTheme()
    {
        var app = Application.Current.ThrowIfNull();
        foreach (var style in app.Styles)
        {
            if (style is ModernTheme result)
            {
                return result;
            }
        }

        return null;
    }

    /// <summary>
    /// Текстовый блок стиля "Заголовок 1".
    /// </summary>
    public static TextBlock Header1
        (
            string? text = null
        )
        => new ()
        {
            Classes = { "h1" },
            Text = text
        };

    /// <summary>
    /// Текстовый блок стиля "Заголовок 2".
    /// </summary>
    public static TextBlock Header2
        (
            string? text = null
        )
        => new ()
        {
            Classes = { "h2" },
            Text = text
        };

    /// <summary>
    /// Добавление класса <c>theme-text-heading</c>.
    /// </summary>
    public static TControl Heading<TControl>
        (
            this TControl control
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Classes.Add ("theme-text-heading");

        return control;
    }

    /// <summary>
    /// Контрол, снабженный меткой.
    /// </summary>
    public static StackPanel LabeledControl
        (
            string? labelText,
            Control control,
            string? messageText = null
        )
    {
        var result = new StackPanel
        {
            Children =
            {
                ControlLabel (labelText),
                control
            }
        };

        if (!string.IsNullOrEmpty (messageText))
        {
            result.Children.Add (ControlMessage (messageText));
        }

        return result;
    }

    /// <summary>
    /// Добавление типографского размера текста указанному контролу.
    /// </summary>
    public static TControl SetSize<TControl>
        (
            this TControl control,
            string sizeClass
        )
        where TControl: Control
    {
        Sure.NotNull (control);
        Sure.NotNullNorEmpty (sizeClass);

        control.Classes.Add (sizeClass);

        return control;
    }

    /// <summary>
    /// Установка типографского размера <c>size-xl</c> для указанного контрола.
    /// </summary>
    public static TControl SizeXL<TControl> (this TControl control)
        where TControl : Control
        => SetSize (control, "size-xl");

    /// <summary>
    /// Установка типографского размера <c>size-lg</c> для указанного контрола.
    /// </summary>
    public static TControl SizeLG<TControl> (this TControl control)
        where TControl : Control
        => SetSize (control, "size-ld");

    /// <summary>
    /// Установка типографского размера <c>size-sm</c> для указанного контрола.
    /// </summary>
    public static TControl SizeSM<TControl> (this TControl control)
        where TControl : Control
        => SetSize (control, "size-sm");

    /// <summary>
    /// Установка типографского размера <c>size-xs</c> для указанного контрола.
    /// </summary>
    public static TControl SizeXS<TControl> (this TControl control)
        where TControl : Control
        => SetSize (control, "size-xs");

    /// <summary>
    /// Установка успеха на указанный контрол.
    /// </summary>
    public static TControl Success<TControl>
        (
            this TControl control
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Classes.Add ("success");

        return control;
    }

    public static string ToResourceKey (this ControlThemeKind resourceKind) =>
        "ActiproControlTheme" + resourceKind;

    public static string ToResourceKey (this GlyphTemplateKind resourceKind) =>
        "ActiproGlyphTemplate" + resourceKind;

    public static string ToResourceKey (this ThemeResourceKind resourceKind) =>
        "ActiproTheme" + resourceKind;

    /// <summary>
    /// Установка предупреждения на указанный контрол.
    /// </summary>
    public static TControl Warning<TControl>
        (
            this TControl control
        )
        where TControl: Control
    {
        Sure.NotNull (control);

        control.Classes.Add ("warning");

        return control;
    }

    #endregion
}
