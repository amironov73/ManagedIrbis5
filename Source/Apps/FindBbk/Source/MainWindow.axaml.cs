// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* BbkModel.cs -- модель для окна поиска в эталоне ББК
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Collections;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

#endregion

#nullable enable

namespace FindBbk;

public partial class MainWindow
    : Window
{
    public MainWindow()
    {
        AvaloniaXamlLoader.Load (this);
        DataContext = new BbkModel { window = this };

        _bbkList = this.FindControl<ListBox> ("BbkList");
    }

    private readonly ListBox _bbkList;

    private void List_HandleClick
        (
            object? sender,
            PointerPressedEventArgs e
        )
    {
        var model = (BbkModel?) DataContext;
        if (model is null)
        {
            return;
        }

        var entries = model.Found;
        if (entries.IsNullOrEmpty())
        {
            return;
        }

        var index = _bbkList.SelectedIndex;
        if (index < 0 || index >= entries.Length)
        {
            return;
        }

        var entry = entries[index];
        var text = entry.Index;
        if (!string.IsNullOrEmpty (text))
        {
            Application.Current?.Clipboard?.SetTextAsync (text);
        }
    }
}
