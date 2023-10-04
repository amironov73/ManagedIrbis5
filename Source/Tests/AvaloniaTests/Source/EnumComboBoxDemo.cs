// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

#region Using directives

using Avalonia.Controls;
using Avalonia.Layout;

using AM.Avalonia.Controls;

using Avalonia;

#endregion

namespace AvaloniaTests;

public class EnumComboBoxDemo
{
    public enum DemoEnum
    {
        FirstValue,
        SecondValue,
        ThirdValue,
        FourthValue
    }

    public async void Show
        (
            Window owner
        )
    {
        var window = new Window
        {
            Title = "EnumComboBox demo",
            Width = 600,
            Height = 400,

            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = new StackPanel
            {
                Margin = new Thickness (5),

                Children =
                {
                    new Label
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Content = "Чисто для контроля"
                    },

                    new EnumComboBox
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        EnumType = typeof (DemoEnum)
                    }
                }
            }
        };

        await window.ShowDialog (owner);
    }

}
