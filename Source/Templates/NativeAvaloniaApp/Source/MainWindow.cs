// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainWindow.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;

using Avalonia.Controls;
using Avalonia.Layout;

#endregion

namespace NativeAvaloniaApp;

/// <summary>
/// Главное окно приложения.
/// </summary>
public class MainWindow
    : Window
{
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Title = "Native Avalonia UI application";
        Width = MinWidth = 400;
        Height = MinHeight = 250;

        const double width = 270;
        var firstTermBox = new TextBox
        {
            Width = width,
            Text = "123.45",
            HorizontalAlignment = HorizontalAlignment.Center
        };
        
        var secondTermBox = new TextBox
        {
            Width = width,
            Text = "456.78",
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var sumBox = new TextBox
        {
            Width = width,
            IsReadOnly = true,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        
        var sumButton = new Button
        {
            Width = width,
            Content = "Calculate the sum of these numbers",
            HorizontalAlignment = HorizontalAlignment.Center,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };
        
        sumButton.Click += (_, _) =>
        {
            const NumberStyles style = NumberStyles.Float
                | NumberStyles.AllowLeadingWhite
                | NumberStyles.AllowTrailingWhite;
            var invariant = CultureInfo.InvariantCulture;
            if (double.TryParse (firstTermBox.Text, style, invariant, out var firstTerm)
                && double.TryParse (secondTermBox.Text, style, invariant, out var secondTerm))
            {
                var sum = firstTerm + secondTerm;
                sumBox.Text = sum.ToString (invariant);
            }
        };

        Content = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 5,
            
            Children =
            {
                firstTermBox,
                secondTermBox,
                sumButton,
                sumBox
            }
        };
    }
}
