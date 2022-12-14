// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* FindForm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
///
/// </summary>
public partial class FindForm
    : Form
{
    private bool _firstSearch = true;
    private Place _startPlace;
    private readonly SyntaxTextBox _textBox;

    /// <summary>
    ///
    /// </summary>
    /// <param name="textBox"></param>
    public FindForm (SyntaxTextBox textBox)
    {
        InitializeComponent();
        _textBox = textBox;
    }

    private void btClose_Click (object sender, EventArgs e)
    {
        Close();
    }

    private void btFindNext_Click (object sender, EventArgs e)
    {
        FindNext (tbFind.Text);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="pattern"></param>
    public virtual void FindNext (string pattern)
    {
        try
        {
            var opt = cbMatchCase.Checked ? RegexOptions.None : RegexOptions.IgnoreCase;
            if (!cbRegex.Checked)
            {
                pattern = Regex.Escape (pattern);
            }

            if (cbWholeWord.Checked)
            {
                pattern = "\\b" + pattern + "\\b";
            }

            //
            var range = _textBox.Selection.Clone();
            range.Normalize();

            //
            if (_firstSearch)
            {
                _startPlace = range.Start;
                _firstSearch = false;
            }

            //
            range.Start = range.End;
            range.End = range.Start >= _startPlace
                ? new Place (_textBox.GetLineLength (_textBox.LinesCount - 1), _textBox.LinesCount - 1)
                : _startPlace;

            //
            foreach (var r in range.GetRangesByLines (pattern, opt))
            {
                _textBox.Selection = r;
                _textBox.DoSelectionVisible();
                _textBox.Invalidate();
                return;
            }

            //
            if (range.Start >= _startPlace && _startPlace > Place.Empty)
            {
                _textBox.Selection.Start = new Place (0, 0);
                FindNext (pattern);
                return;
            }

            MessageBox.Show ("Not found");
        }
        catch (Exception ex)
        {
            MessageBox.Show (ex.Message);
        }
    }

    private void tbFind_KeyPress
        (
            object? sender,
            KeyPressEventArgs eventArgs
        )
    {
        if (eventArgs.KeyChar == '\r')
        {
            btFindNext.PerformClick();
            eventArgs.Handled = true;
            return;
        }

        if (eventArgs.KeyChar == '\x1b')
        {
            Hide();
            eventArgs.Handled = true;
        }
    }

    private void FindForm_FormClosing
        (
            object? sender,
            FormClosingEventArgs eventArgs
        )
    {
        if (eventArgs.CloseReason == CloseReason.UserClosing)
        {
            eventArgs.Cancel = true;
            Hide();
        }

        this._textBox.Focus();
    }

    /// <inheritdoc cref="Form.ProcessCmdKey"/>
    protected override bool ProcessCmdKey (ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Escape)
        {
            this.Close();
            return true;
        }

        return base.ProcessCmdKey (ref msg, keyData);
    }

    /// <inheritdoc cref="Form.OnActivated"/>
    protected override void OnActivated (EventArgs eventArgs)
    {
        tbFind.Focus();
        ResetSerach();
    }

    private void ResetSerach()
    {
        _firstSearch = true;
    }

    private void cbMatchCase_CheckedChanged (object? sender, EventArgs eventArgs)
    {
        ResetSerach();
    }
}
