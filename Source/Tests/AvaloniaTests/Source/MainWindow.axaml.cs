// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MainWindow.axaml.cs -- главное окно
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Interactivity;

using ManagedIrbis;
using ManagedIrbis.Avalonia;
using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace AvaloniaTests;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void FieldEditorButton_OnClick (object? sender, RoutedEventArgs e)
    {
        var lines = new List<FieldLine>();
        for (var i = 0; i < 100; i++)
        {
            var line = new FieldLine
            {
                Item = new WorksheetItem
                {
                    Title = $"Какое-то поле {i + 101}"
                },
                Instance = new Field (i + 1, $"Значение поля {i + 101}")
            };
            lines.Add (line);
        }

        var window = new FieldEditorWindow();
        window.SetLines (lines);
        window.SetHint ("Тут какая-то подсказка");
        window.ShowDialog (this);
    }

    private async void LoginWindowButton_OnClick (object? sender, RoutedEventArgs e)
    {
        var window = new LoginWindow();
        var result = await window.ShowDialog<bool> (this);
        Debug.WriteLine ($"Dialog result is {result}");
    }

    private async void AboutWindowButton_OnClick (object? sender, RoutedEventArgs e)
    {
        var window = new AboutWindow();
        await window.ShowDialog<bool> (this);
    }
}
