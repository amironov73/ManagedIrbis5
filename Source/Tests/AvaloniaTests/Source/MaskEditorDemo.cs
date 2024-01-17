using System;
using System.IO;

using AM.Avalonia.Controls;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media.Imaging;

namespace AvaloniaTests;

internal sealed class MaskEditorDemo
{
    public async void Show
        (
            Window owner
        )
    {
        var imagePath = Path.Combine
            (
                AppContext.BaseDirectory,
                "Images",
                "cat-and-coffee.jpg"
            );
        var image = new Bitmap (imagePath);

        var maskPath = Path.Combine
            (
                AppContext.BaseDirectory,
                "Images",
                "cat-mask.png"
            );
        var mask = new Bitmap (maskPath);

        var editor = new MaskEditor
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };
        editor.SetImage (image);
        editor.SetMask (mask);

        var window = new Window
        {
            Title = "MaskEditor demo",
            Width = 600,
            Height = 400,

            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Stretch,
            Content = new Grid
            {
                Children =
                {
                    editor
                }
            }
        };

        window.AttachDevTools();

        await window.ShowDialog (owner);
    }
}
