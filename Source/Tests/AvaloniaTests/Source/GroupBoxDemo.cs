// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* GroupBox.cs -- группа контролов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Avalonia.Controls;
using AM.Logging;

using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

#endregion

namespace AvaloniaTests;

internal sealed class GroupBoxDemo
{
    public async void Show
        (
            Window owner
        )
    {
        MagnaTarget.AddToNlogConfiguration();

        var window = new Window
        {
            Title = "GroupBox demo",
            Width = 600,
            Height = 400,

            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = new StackPanel
            {
                Children =
                {
                    new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Content = "Чисто для контроля"
                    },

                    new GroupBox
                    {
                        Header = "First group",
                        Width = 600,
                        Height = 200,
                        Background = Brushes.Aqua,

                        Content = new StackPanel
                        {
                            Background = Brushes.Brown,
                            Children =
                            {
                                new TextBlock { Text = "Первая строчка" },
                                new TextBlock { Text = "Вторая строчка" },
                                new TextBlock { Text = "Третья строчка" },
                            }
                        }
                    }
                }
            }
        };

        await window.ShowDialog (owner);
    }
}
