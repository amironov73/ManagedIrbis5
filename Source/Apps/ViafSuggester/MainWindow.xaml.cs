// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainWindows.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Windows;

using RestfulIrbis.Viaf;

#endregion

#nullable enable

namespace ViafSuggester;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void GoButton_OnClick (object sender, RoutedEventArgs e)
    {
        resultBox.Clear();
        var name = termBox.Text.Trim();
        if (string.IsNullOrEmpty (name))
        {
            return;
        }

        try
        {
            var client = new ViafClient();
            var suggestions = client.GetSuggestions (name);
            var text = string.Join
                (
                    Environment.NewLine,
                    suggestions.Select (s => s.DisplayForm)
                        .OrderBy (s => s)
                );
            resultBox.Text = text;
        }
        catch (Exception exception)
        {
            resultBox.Text = exception.ToString();
        }
    }
}
