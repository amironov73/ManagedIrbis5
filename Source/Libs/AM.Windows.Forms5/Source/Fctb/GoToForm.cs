// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* GoToForm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
///
/// </summary>
public partial class GoToForm
    : Form
{
    /// <summary>
    ///
    /// </summary>
    public int SelectedLineNumber { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int TotalLineCount { get; set; }

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public GoToForm()
    {
        InitializeComponent();
    }

    /// <inheritdoc cref="Form.OnLoad"/>
    protected override void OnLoad (EventArgs e)
    {
        base.OnLoad (e);

        tbLineNumber.Text = SelectedLineNumber.ToString();

        label.Text = String.Format ("Line number (1 - {0}):", TotalLineCount);
    }

    /// <inheritdoc cref="Form.OnShown"/>
    protected override void OnShown (EventArgs e)
    {
        base.OnShown (e);

        tbLineNumber.Focus();
    }

    private void btnOk_Click (object sender, EventArgs e)
    {
        int enteredLine;
        if (int.TryParse (tbLineNumber.Text, out enteredLine))
        {
            enteredLine = Math.Min (enteredLine, TotalLineCount);
            enteredLine = Math.Max (1, enteredLine);

            SelectedLineNumber = enteredLine;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click (object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
