// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* AbstractMsBoxViewModel.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using AM.Avalonia.DTO;
using AM.Avalonia.Enums;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Avalonia.ViewModels;

public abstract class AbstractMsBoxViewModel
    : INotifyPropertyChanged

{
    protected AbstractMsBoxViewModel(AbstractMessageBoxParams parameters, Icon icon = Icon.None, Bitmap bitmap = null)
    {
        if (bitmap != null)
        {
            ImagePath = bitmap;
        }
        else if (icon != Icon.None)
        {
            ImagePath = new Bitmap(AvaloniaLocator.Current.GetService<IAssetLoader>()
                .Open(new Uri(
                    $" avares://MessageBox.Avalonia/Assets/{icon.ToString().ToLowerInvariant()}.png")));
        }

        MinWidth = parameters.MinWidth;
        MaxWidth = parameters.MaxWidth;
        Width = parameters.Width;
        MinHeight = parameters.MinHeight;
        MaxHeight = parameters.MaxHeight;
        Height = parameters.Height;
        CanResize = parameters.CanResize;
        FontFamily = parameters.FontFamily;
        ContentTitle = parameters.ContentTitle;
        ContentHeader = parameters.ContentHeader;
        ContentMessage = parameters.ContentMessage;
        Markdown = parameters.Markdown;
        WindowIconPath = parameters.WindowIcon;
        SizeToContent = parameters.SizeToContent;
        LocationOfMyWindow = parameters.WindowStartupLocation;
        SystemDecorations = parameters.SystemDecorations;
        Topmost = parameters.Topmost;
    }

    public bool CanResize { get; }
    public bool HasHeader => !string.IsNullOrEmpty(ContentHeader);
    public bool HasIcon => ImagePath is not null;
    public FontFamily FontFamily { get; }
    public string ContentTitle { get; }
    public string ContentHeader { get; }
    public string ContentMessage { get; set; }
    public bool Markdown { get; set; }
    public WindowIcon WindowIconPath { get; }
    public Bitmap ImagePath { get; }
    public double MinWidth { get; set; }
    public double MaxWidth { get; set; }
    public double Width { get; set; }

    public double MinHeight { get; set; }
    public double MaxHeight { get; set; }
    public double Height { get; set; }

    public SystemDecorations SystemDecorations { get; set; }
    public bool Topmost { get; set; }

    public SizeToContent SizeToContent { get; set; } = SizeToContent.Height;

    public WindowStartupLocation LocationOfMyWindow { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    public async Task Copy()
    {
        await AvaloniaLocator.Current.GetService<IClipboard>().SetTextAsync(ContentMessage);
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
