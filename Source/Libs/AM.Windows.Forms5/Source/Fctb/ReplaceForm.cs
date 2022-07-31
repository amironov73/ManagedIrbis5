// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* ReplaceForm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections.Generic;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
///
/// </summary>
public partial class ReplaceForm
    : Form
{
    private readonly SyntaxTextBox _textBox;
    private bool _firstSearch = true;
    private Place _startPlace;

    /// <summary>
    ///
    /// </summary>
    /// <param name="textBox"></param>
    public ReplaceForm (SyntaxTextBox textBox)
    {
        InitializeComponent();
        this._textBox = textBox;
    }

    private void btClose_Click (object sender, EventArgs e)
    {
        Close();
    }

    private void btFindNext_Click (object sender, EventArgs e)
    {
        try
        {
            if (!Find (tbFind.Text))
            {
                MessageBox.Show ("Not found");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show (ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public List<TextRange> FindAll (string pattern)
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
        var range = _textBox.Selection.IsEmpty ? _textBox.Range.Clone() : _textBox.Selection.Clone();

        //
        var list = new List<TextRange>();
        foreach (var r in range.GetRangesByLines (pattern, opt))
            list.Add (r);

        return list;
    }

    /// <summary>
    ///
    /// </summary>
    public bool Find (string pattern)
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
        if (range.Start >= _startPlace)
        {
            range.End = new Place (_textBox.GetLineLength (_textBox.LinesCount - 1), _textBox.LinesCount - 1);
        }
        else
        {
            range.End = _startPlace;
        }

        //
        foreach (var r in range.GetRangesByLines (pattern, opt))
        {
            _textBox.Selection.Start = r.Start;
            _textBox.Selection.End = r.End;
            _textBox.DoSelectionVisible();
            _textBox.Invalidate();
            return true;
        }

        if (range.Start >= _startPlace && _startPlace > Place.Empty)
        {
            _textBox.Selection.Start = new Place (0, 0);
            return Find (pattern);
        }

        return false;
    }

    private void tbFind_KeyPress (object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == '\r')
        {
            btFindNext_Click (sender, null);
        }

        if (e.KeyChar == '\x1b')
        {
            Hide();
        }
    }

    protected override bool ProcessCmdKey (ref Message msg, Keys keyData) // David
    {
        if (keyData == Keys.Escape)
        {
            this.Close();
            return true;
        }

        return base.ProcessCmdKey (ref msg, keyData);
    }

    private void ReplaceForm_FormClosing (object sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            Hide();
        }

        this._textBox.Focus();
    }

    private void btReplace_Click (object sender, EventArgs e)
    {
        try
        {
            if (_textBox.SelectionLength != 0)
            {
                if (!_textBox.Selection.ReadOnly)
                {
                    _textBox.InsertText (tbReplace.Text);
                }
            }

            btFindNext_Click (sender, null);
        }
        catch (Exception ex)
        {
            MessageBox.Show (ex.Message);
        }
    }

    private void btReplaceAll_Click (object sender, EventArgs e)
    {
        try
        {
            _textBox.Selection.BeginUpdate();

            //search
            var ranges = FindAll (tbFind.Text);

            //check readonly
            var ro = false;
            foreach (var r in ranges)
                if (r.ReadOnly)
                {
                    ro = true;
                    break;
                }

            //replace
            if (!ro)
            {
                if (ranges.Count > 0)
                {
                    _textBox.TextSource.Manager.ExecuteCommand (new ReplaceTextCommand (_textBox.TextSource, ranges,
                        tbReplace.Text));
                    _textBox.Selection.Start = new Place (0, 0);
                }
            }

            //
            _textBox.Invalidate();
            MessageBox.Show (ranges.Count + " occurrence(s) replaced");
        }
        catch (Exception ex)
        {
            MessageBox.Show (ex.Message);
        }

        _textBox.Selection.EndUpdate();
    }

    protected override void OnActivated (EventArgs e)
    {
        tbFind.Focus();
        ResetSerach();
    }

    void ResetSerach()
    {
        _firstSearch = true;
    }

    private void cbMatchCase_CheckedChanged (object sender, EventArgs e)
    {
        ResetSerach();
    }
}
