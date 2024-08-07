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

using Avalonia.Controls;
using Avalonia.Layout;

using Ursa.Controls;

#endregion

namespace UrsaApp;

/// <summary>
/// Главное окно приложения.
/// </summary>
public sealed class MainWindow
    : Window
{
    #region Window members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Title = "Контролы Ursa";
        Width = MinWidth = 400;
        Height = MinHeight = 250;


        Content = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Spacing = 5,
                Children =
                {
                    new ButtonGroup
                    {
                        Classes = { "Solid", "Warning" },
                        Items =
                        {
                            new Button
                            {
                                Content = "Hello"
                            },

                            new Button
                            {
                                Content = "World"
                            }
                        }
                    },

                    new TagInput
                    {
                        Width = 300,
                        Tags =
                        {
                            "Avalonia",
                            ".NET",
                            "UI"
                        }
                    }

                }
            };
    }

    #endregion
}
