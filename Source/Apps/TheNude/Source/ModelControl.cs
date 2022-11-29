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
using AM.Avalonia.Controls;
using AM.Avalonia.Converters;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;

using ReactiveUI;

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

        var gotoModelPage = ReactiveCommand.Create (ViewModel!.GotoModelPage);
        Content = new DockPanel
        {
            Children =
            {
                // имя модели
                new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeight.Bold,
                    [!ContentProperty] = new Binding (nameof (ViewModel.Name))
                }
                .DockTop(),

                // псевдонимы
                new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    [!ContentProperty] = new Binding (nameof (ViewModel.Aka))
                    {
                        Converter = ArrayToListConverter.Instance
                    }
                }
                .DockBottom(),

                // картинка
                new LeisurelyImage
                    {
                        MaxWidth = 100,
                        Margin = new Thickness (5),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        [!LeisurelyImage.PathProperty] = new Binding (nameof (ViewModel.Thumbnail)),
                    }
                    .Also (control =>
                    {
                        control.PointerPressed += ((_, _) =>
                        {
                            ViewModel.GotoModelPage();
                        });
                    }),
            }
        };
    }

    #endregion
}
