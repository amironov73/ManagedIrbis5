// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AsyncVoidLambda
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* LedIndicatorDemo.cs -- демонстрация контрола LedIndicator
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Avalonia.Controls;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Threading;

#endregion

namespace AvaloniaTests;

public sealed class LedIndicatorDemo
{
    public async void Show
        (
            Window owner
        )
    {
        var counter = 0;
        var led01 = CreateLed();
        var led02 = CreateLed();
        var led03 = CreateLed();
        var led04 = CreateLed();
        var led05 = CreateLed();
        var led06 = CreateLed();

        var window = new Window
        {
            Title = "LedIndicator demo",
            Width = 400,
            Height = 100,

            HorizontalContentAlignment = HorizontalAlignment.Center,
            VerticalContentAlignment = VerticalAlignment.Center,
            Content = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Children =
                {
                    led01, led02, led03, led04, led05, led06
                }
            }
        };

        window.AttachDevTools();
        var timer = DispatcherTimer.Run (Blink, TimeSpan.FromMilliseconds (200));

        await window.ShowDialog (owner);

        timer.Dispose();

        LedIndicator CreateLed() => new ()
        {
            Width = 50, Height = 50, Margin = new Thickness (5)
        };

        bool Blink()
        {
            counter++;
            led01.IsOn = (counter & 0x01) != 0;
            led02.IsOn = (counter & 0x02) != 0;
            led03.IsOn = (counter & 0x04) != 0;
            led04.IsOn = (counter & 0x08) != 0;
            led05.IsOn = (counter & 0x10) != 0;
            led06.IsOn = (counter & 0x20) != 0;
             return true;
        }
    }
}
