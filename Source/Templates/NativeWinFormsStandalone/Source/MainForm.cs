// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainForm.cs -- главная форма приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.Windows.Forms;

#endregion

#nullable enable

namespace WinFormsAot;

/// <summary>
/// Главная форма приложения.
/// </summary>
public partial class MainForm 
    : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private void _sumButton_Click
        (
            object sender, 
            EventArgs eventArgs
        )
    {
        var invariant = CultureInfo.InvariantCulture;
        const NumberStyles style = NumberStyles.AllowLeadingWhite
                                   | NumberStyles.AllowTrailingWhite
                                   | NumberStyles.Float;
        if (double.TryParse(_firstTermBox.Text, style, invariant, out var firstTerm)
            && double.TryParse(_secondTermBox.Text, style, invariant, out var secondTerm))
        {
            var sum = firstTerm + secondTerm;
            _sumBox.Text = sum.ToString (invariant);
        }
    }

    private void _closeButton_Click
        (
            object sender, 
            EventArgs eventArgs
        )
    {
        Close();
    }
}
