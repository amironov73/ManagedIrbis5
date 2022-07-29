// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Hints.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AM;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Collection of Hints.
/// This is temporary buffer for currently displayed hints.
/// </summary>
public class Hints : ICollection<Hint>, IDisposable
{
    #region Constructions

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Hints
        (
            SyntaxTextBox textBox
        )
    {
        Sure.NotNull (textBox);

        this._textBox = textBox;
        textBox.TextChanged += OnTextBoxTextChanged;
        textBox.KeyDown += OnTextBoxKeyDown;
        textBox.VisibleRangeChanged += OnTextBoxVisibleRangeChanged;
    }

    #endregion

    #region Private members

    private readonly SyntaxTextBox _textBox;
    private readonly List<Hint> _items = new ();

    /// <summary>
    /// Реакция на кнопку, нажатую в текстбоксе.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnTextBoxKeyDown
        (
            object? sender,
            KeyEventArgs e
        )
    {
        if (e.KeyCode == Keys.Escape && e.Modifiers == Keys.None)
        {
            Clear();
        }
    }

    /// <summary>
    /// Реакция на изменение текста в текстбоксе.
    /// </summary>
    protected virtual void OnTextBoxTextChanged
        (
            object? sender,
            TextChangedEventArgs e
        )
    {
        Clear();
    }

    private void OnTextBoxVisibleRangeChanged
        (
            object sender,
            EventArgs e
        )
    {
        if (_items.Count == 0)
        {
            return;
        }

        _textBox.NeedRecalc (true);
        foreach (var item in _items)
        {
            LayoutHint (item);
            item.HostPanel.Invalidate();
        }
    }

    private void LayoutHint
        (
            Hint hint
        )
    {
        if (hint.Inline)
        {
            if (hint.Range.Start.Line < _textBox.LineInfos.Count - 1)
            {
                hint.HostPanel.Top = _textBox.LineInfos[hint.Range.Start.Line + 1].startY - hint.TopPadding -
                                     hint.HostPanel.Height - _textBox.VerticalScroll.Value;
            }
            else
            {
                hint.HostPanel.Top = _textBox.TextHeight + _textBox.Paddings.Top - hint.HostPanel.Height - _textBox.VerticalScroll.Value;
            }
        }
        else
        {
            if (hint.Range.Start.Line > _textBox.LinesCount - 1)
            {
                return;
            }

            if (hint.Range.Start.Line == _textBox.LinesCount - 1)
            {
                var y = _textBox.LineInfos[hint.Range.Start.Line].startY - _textBox.VerticalScroll.Value + _textBox.CharHeight;

                if (y + hint.HostPanel.Height + 1 > _textBox.ClientRectangle.Bottom)
                {
                    hint.HostPanel.Top = Math.Max (0,
                        _textBox.LineInfos[hint.Range.Start.Line].startY - _textBox.VerticalScroll.Value - hint.HostPanel.Height);
                }
                else
                {
                    hint.HostPanel.Top = y;
                }
            }
            else
            {
                hint.HostPanel.Top = _textBox.LineInfos[hint.Range.Start.Line + 1].startY - _textBox.VerticalScroll.Value;
                if (hint.HostPanel.Bottom > _textBox.ClientRectangle.Bottom)
                {
                    hint.HostPanel.Top = _textBox.LineInfos[hint.Range.Start.Line + 1].startY - _textBox.CharHeight -
                                         hint.TopPadding - hint.HostPanel.Height - _textBox.VerticalScroll.Value;
                }
            }
        }

        if (hint.Dock == DockStyle.Fill)
        {
            hint.Width = _textBox.ClientSize.Width - _textBox.LeftIndent - 2;
            hint.HostPanel.Left = _textBox.LeftIndent;
        }
        else
        {
            var p1 = _textBox.PlaceToPoint (hint.Range.Start);
            var p2 = _textBox.PlaceToPoint (hint.Range.End);
            var cx = (p1.X + p2.X) / 2;
            var x = cx - hint.HostPanel.Width / 2;
            hint.HostPanel.Left = Math.Max (_textBox.LeftIndent, x);
            if (hint.HostPanel.Right > _textBox.ClientSize.Width)
            {
                hint.HostPanel.Left = Math.Max (_textBox.LeftIndent, x - (hint.HostPanel.Right - _textBox.ClientSize.Width));
            }
        }
    }

    #endregion

    public void Dispose()
    {
        _textBox.TextChanged -= OnTextBoxTextChanged;
        _textBox.KeyDown -= OnTextBoxKeyDown;
        _textBox.VisibleRangeChanged -= OnTextBoxVisibleRangeChanged;
    }

    public IEnumerator<Hint> GetEnumerator()
    {
        foreach (var item in _items)
            yield return item;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Clears all displayed hints
    /// </summary>
    public void Clear()
    {
        _items.Clear();
        if (_textBox.Controls.Count != 0)
        {
            var toDelete = new List<Control>();
            foreach (Control item in _textBox.Controls)
                if (item is UnfocusablePanel)
                {
                    toDelete.Add (item);
                }

            foreach (var item in toDelete)
                _textBox.Controls.Remove (item);

            for (var i = 0; i < _textBox.LineInfos.Count; i++)
            {
                var li = _textBox.LineInfos[i];
                li.bottomPadding = 0;
                _textBox.LineInfos[i] = li;
            }

            _textBox.NeedRecalc();
            _textBox.Invalidate();
            _textBox.Select();
            _textBox.ActiveControl = null;
        }
    }

    /// <summary>
    /// Add and shows the hint
    /// </summary>
    /// <param name="hint"></param>
    public void Add (Hint hint)
    {
        _items.Add (hint);

        if (hint.Inline /* || hint.Range.Start.iLine >= tb.LinesCount - 1*/)
        {
            var li = _textBox.LineInfos[hint.Range.Start.Line];
            hint.TopPadding = li.bottomPadding;
            li.bottomPadding += hint.HostPanel.Height;
            _textBox.LineInfos[hint.Range.Start.Line] = li;
            _textBox.NeedRecalc (true);
        }

        LayoutHint (hint);

        _textBox.OnVisibleRangeChanged();

        hint.HostPanel.Parent = _textBox;

        _textBox.Select();
        _textBox.ActiveControl = null;
        _textBox.Invalidate();
    }

    /// <summary>
    /// Is collection contains the hint?
    /// </summary>
    public bool Contains (Hint item)
    {
        return _items.Contains (item);
    }

    public void CopyTo (Hint[] array, int arrayIndex)
    {
        _items.CopyTo (array, arrayIndex);
    }

    /// <summary>
    /// Count of hints
    /// </summary>
    public int Count
    {
        get { return _items.Count; }
    }

    public bool IsReadOnly
    {
        get { return false; }
    }

    public bool Remove (Hint item)
    {
        throw new NotImplementedException();
    }
}
