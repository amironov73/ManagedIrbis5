// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* RenderDemo.cs -- демонстрация рендеринга контролов в картинку
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM.Avalonia;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Skia;

using SkiaSharp;

using static AM.Avalonia.AvaloniaUtility;

#endregion

namespace AvaloniaTests;

/// <summary>
/// Демонстрация рендеринга контролов в картинку.
/// </summary>
internal sealed class RenderDemo
{
    public sealed class DemoControl
        : UserControl
    {
        public DemoControl()
        {
            Content = new StackPanel
            {
                Background = Brushes.Yellow,
                Children =
                {
                    MakeLabel ("Это метка"),
                    new TextBox
                    {
                        Text = "Какой-то текст"
                    }
                }
            };
        }
    }


    public void DoRender()
    {
        const int renderWidth = 300;
        const int renderHeight = 200;

        var container = new DemoControl
        {
            Width = renderWidth,
            Height = renderHeight
        };

        var infinity = new Size (double.PositiveInfinity, double.PositiveInfinity);
        container.Measure (infinity);
        container.Arrange (new Rect (container.DesiredSize));
        container.UpdateLayout();

        var info = new SKImageInfo (600, 800);
        var surface = SKSurface.Create (info);
        var canvas = surface.Canvas;
        canvas.Clear (SKColors.Red);

        Render (canvas, container);

        var image = surface.Snapshot();
        var data = image.Encode (SKEncodedImageFormat.Png, 100);
        using var stream = File.Create ("render.png");
        data.SaveTo (stream);
    }
}
