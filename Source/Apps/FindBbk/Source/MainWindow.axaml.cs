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

using Avalonia.Controls;
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
        DataContext = new BbkModel();
    }
}
