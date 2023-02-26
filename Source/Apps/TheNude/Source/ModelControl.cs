// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* ModelControl.cs -- контрол для отображения модели
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Avalonia;
using AM.Avalonia.Converters;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;

#endregion

#nullable enable

namespace TheNude;

/// <summary>
/// Контрол для отображения модели.
/// </summary>
public class ModelControl
    : ReactiveUserControl<ModelInfo>
{
    #region Control members

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Content = new Border
        {
            BorderThickness = new Thickness (1),
            Padding = new Thickness (5),
            BorderBrush = Brushes.Blue,
            Background = Brushes.AliceBlue,
            Width = MaxWidth = 300,
            Height = MaxHeight = 215,
            Margin = new Thickness (5),
            Child = new DockPanel
            {
                Children =
                {
                    // картинка
                    new Image
                        {
                            Stretch = Stretch.Uniform,
                            Height = 200,
                            Margin = new Thickness (5),
                            Cursor = new Cursor (StandardCursorType.Hand),
                            [!Image.SourceProperty] = new Binding (nameof (ViewModel.ThumbnailBitmap)),
                        }
                        .DockLeft()
                        .Also (control => control.PointerPressed += (_, _) => ViewModel!.GotoModelPage()),

                    // имя модели
                    new Label
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            HorizontalContentAlignment = HorizontalAlignment.Center,
                            Foreground = Brushes.Black,
                            FontWeight = FontWeight.Bold,
                            [!ContentProperty] = new Binding (nameof (ViewModel.Name))
                        }
                        .DockTop(),

                    // псевдонимы
                    new TextBlock
                    {
                        Foreground = Brushes.Black,
                        TextWrapping = TextWrapping.Wrap,
                        [!TextBlock.TextProperty] = new Binding (nameof (ViewModel.Aka))
                        {
                            Converter = ArrayToListConverter.Instance
                        }
                    },
                }
            }
        };
    }

    #endregion
}
