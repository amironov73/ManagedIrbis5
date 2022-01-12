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

public partial class GoToForm : Form
{
    public int SelectedLineNumber { get; set; }
    public int TotalLineCount { get; set; }

    public GoToForm()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        this.tbLineNumber.Text = this.SelectedLineNumber.ToString();

        this.label.Text = String.Format("Line number (1 - {0}):", this.TotalLineCount);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);

        this.tbLineNumber.Focus();
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        int enteredLine;
        if (int.TryParse(this.tbLineNumber.Text, out enteredLine))
        {
            enteredLine = Math.Min(enteredLine, this.TotalLineCount);
            enteredLine = Math.Max(1, enteredLine);

            this.SelectedLineNumber = enteredLine;
        }

        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
