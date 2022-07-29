// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TextRange.cs -- диапазон символов текста
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Диапазон символов текста.
/// </summary>
public class TextRange
    : IEnumerable<Place>
{
    #region Properties

    /// <summary>
    /// Return true if no selected text
    /// </summary>
    public virtual bool IsEmpty
    {
        get
        {
            if (ColumnSelectionMode)
            {
                return Start.Column == End.Column;
            }

            return Start == End;
        }
    }

    public readonly SyntaxTextBox _textBox;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TextRange
        (
            SyntaxTextBox textBox
        )
    {
        Sure.NotNull (textBox);

        _textBox = textBox;
    }

    #endregion

    #region Private members

    private Place _start;

    private Place _end;

    private int _preferedPos = -1;

    private int _updating = 0;

    private string? _cachedText;

    private List<Place>? _cachedCharIndexToPlace;

    private int _cachedTextVersion = -1;

    #endregion

    /// <summary>
    /// Column selection mode
    /// </summary>
    public bool ColumnSelectionMode { get; set; }

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TextRange
        (
            SyntaxTextBox textBox,
            int iStartChar,
            int iStartLine,
            int iEndChar,
            int iEndLine
        )
        : this (textBox)
    {
        _start = new Place (iStartChar, iStartLine);
        _end = new Place (iEndChar, iEndLine);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TextRange
        (
            SyntaxTextBox textBox,
            Place start,
            Place end
        )
        : this (textBox)
    {
        this._start = start;
        this._end = end;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TextRange
        (
            SyntaxTextBox textBox,
            int iLine
        )
        : this (textBox)
    {
        _start = new Place (0, iLine);
        _end = new Place (textBox[iLine].Count, iLine);
    }

    #endregion

    #region Public methods

    public bool Contains (Place place)
    {
        if (place.Line < Math.Min (_start.Line, _end.Line))
        {
            return false;
        }

        if (place.Line > Math.Max (_start.Line, _end.Line))
        {
            return false;
        }

        var s = _start;
        var e = _end;

        //normalize start and end
        if (s.Line > e.Line || (s.Line == e.Line && s.Column > e.Column))
        {
            var temp = s;
            s = e;
            e = temp;
        }

        if (ColumnSelectionMode)
        {
            if (place.Column < s.Column || place.Column > e.Column)
            {
                return false;
            }
        }
        else
        {
            if (place.Line == s.Line && place.Column < s.Column)
            {
                return false;
            }

            if (place.Line == e.Line && place.Column > e.Column)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns intersection with other range,
    /// empty range returned otherwise
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public virtual TextRange GetIntersectionWith (TextRange range)
    {
        if (ColumnSelectionMode)
        {
            return GetIntersectionWith_ColumnSelectionMode (range);
        }

        var r1 = this.Clone();
        var r2 = range.Clone();
        r1.Normalize();
        r2.Normalize();
        var newStart = r1.Start > r2.Start ? r1.Start : r2.Start;
        var newEnd = r1.End < r2.End ? r1.End : r2.End;
        if (newEnd < newStart)
        {
            return new TextRange (_textBox, _start, _start);
        }

        return _textBox.GetRange (newStart, newEnd);
    }

    /// <summary>
    /// Returns union with other range.
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public TextRange GetUnionWith (TextRange range)
    {
        var r1 = this.Clone();
        var r2 = range.Clone();
        r1.Normalize();
        r2.Normalize();
        var newStart = r1.Start < r2.Start ? r1.Start : r2.Start;
        var newEnd = r1.End > r2.End ? r1.End : r2.End;

        return _textBox.GetRange (newStart, newEnd);
    }

    /// <summary>
    /// Select all chars of control
    /// </summary>
    public void SelectAll()
    {
        ColumnSelectionMode = false;

        Start = new Place (0, 0);
        if (_textBox.LinesCount == 0)
        {
            Start = new Place (0, 0);
        }
        else
        {
            _end = new Place (0, 0);
            _start = new Place (_textBox[_textBox.LinesCount - 1].Count, _textBox.LinesCount - 1);
        }

        if (this == _textBox.Selection)
        {
            _textBox.Invalidate();
        }
    }

    /// <summary>
    /// Start line and char position
    /// </summary>
    public Place Start
    {
        get { return _start; }
        set
        {
            _end = _start = value;
            _preferedPos = -1;
            OnSelectionChanged();
        }
    }

    /// <summary>
    /// Finish line and char position
    /// </summary>
    public Place End
    {
        get { return _end; }
        set
        {
            _end = value;
            OnSelectionChanged();
        }
    }

    /// <summary>
    /// Text of range
    /// </summary>
    /// <remarks>This property has not 'set' accessor because undo/redo stack works only with
    /// FastColoredTextBox.Selection range. So, if you want to set text, you need to use FastColoredTextBox.Selection
    /// and FastColoredTextBox.InsertText() mehtod.
    /// </remarks>
    public virtual string Text
    {
        get
        {
            if (ColumnSelectionMode)
            {
                return Text_ColumnSelectionMode;
            }

            var fromLine = Math.Min (_end.Line, _start.Line);
            var toLine = Math.Max (_end.Line, _start.Line);
            var fromChar = FromX;
            var toChar = ToX;
            if (fromLine < 0)
            {
                return null;
            }

            //
            var sb = new StringBuilder();
            for (var y = fromLine; y <= toLine; y++)
            {
                var fromX = y == fromLine ? fromChar : 0;
                var toX = y == toLine ? Math.Min (_textBox[y].Count - 1, toChar - 1) : _textBox[y].Count - 1;
                for (var x = fromX; x <= toX; x++)
                    sb.Append (_textBox[y][x].c);
                if (y != toLine && fromLine != toLine)
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
    }

    public int Length
    {
        get
        {
            if (ColumnSelectionMode)
            {
                return Length_ColumnSelectionMode (false);
            }

            var fromLine = Math.Min (_end.Line, _start.Line);
            var toLine = Math.Max (_end.Line, _start.Line);
            var cnt = 0;
            if (fromLine < 0)
            {
                return 0;
            }

            for (var y = fromLine; y <= toLine; y++)
            {
                var fromX = y == fromLine ? FromX : 0;
                var toX = y == toLine ? Math.Min (_textBox[y].Count - 1, ToX - 1) : _textBox[y].Count - 1;

                cnt += toX - fromX + 1;

                if (y != toLine && fromLine != toLine)
                {
                    cnt += Environment.NewLine.Length;
                }
            }

            return cnt;
        }
    }

    public int TextLength
    {
        get
        {
            if (ColumnSelectionMode)
            {
                return Length_ColumnSelectionMode (true);
            }
            else
            {
                return Length;
            }
        }
    }

    internal void GetText (out string text, out List<Place> charIndexToPlace)
    {
        //try get cached text
        if (_textBox.TextVersion == _cachedTextVersion)
        {
            text = _cachedText;
            charIndexToPlace = _cachedCharIndexToPlace;
            return;
        }

        //
        var fromLine = Math.Min (_end.Line, _start.Line);
        var toLine = Math.Max (_end.Line, _start.Line);
        var fromChar = FromX;
        var toChar = ToX;

        var sb = new StringBuilder ((toLine - fromLine) * 50);
        charIndexToPlace = new List<Place> (sb.Capacity);
        if (fromLine >= 0)
        {
            for (var y = fromLine; y <= toLine; y++)
            {
                var fromX = y == fromLine ? fromChar : 0;
                var toX = y == toLine ? Math.Min (toChar - 1, _textBox[y].Count - 1) : _textBox[y].Count - 1;
                for (var x = fromX; x <= toX; x++)
                {
                    sb.Append (_textBox[y][x].c);
                    charIndexToPlace.Add (new Place (x, y));
                }

                if (y != toLine && fromLine != toLine)
                {
                    foreach (var c in Environment.NewLine)
                    {
                        sb.Append (c);
                        charIndexToPlace.Add (new Place (_textBox[y].Count /*???*/, y));
                    }
                }
            }
        }

        text = sb.ToString();
        charIndexToPlace.Add (End > Start ? End : Start);

        //caching
        _cachedText = text;
        _cachedCharIndexToPlace = charIndexToPlace;
        _cachedTextVersion = _textBox.TextVersion;
    }

    /// <summary>
    /// Returns first char after Start place
    /// </summary>
    public char CharAfterStart
    {
        get
        {
            if (Start.Column >= _textBox[Start.Line].Count)
            {
                return '\n';
            }

            return _textBox[Start.Line][Start.Column].c;
        }
    }

    /// <summary>
    /// Returns first char before Start place
    /// </summary>
    public char CharBeforeStart
    {
        get
        {
            if (Start.Column > _textBox[Start.Line].Count)
            {
                return '\n';
            }

            if (Start.Column <= 0)
            {
                return '\n';
            }

            return _textBox[Start.Line][Start.Column - 1].c;
        }
    }

    /// <summary>
    /// Returns required char's number before start of the Range
    /// </summary>
    public string GetCharsBeforeStart (int charsCount)
    {
        var pos = _textBox.PlaceToPosition (Start) - charsCount;
        if (pos < 0)
        {
            pos = 0;
        }

        return new TextRange (_textBox, _textBox.PositionToPlace (pos), Start).Text;
    }

    /// <summary>
    /// Returns required char's number after start of the Range
    /// </summary>
    public string GetCharsAfterStart (int charsCount)
    {
        return GetCharsBeforeStart (-charsCount);
    }

    /// <summary>
    /// Clone range
    /// </summary>
    /// <returns></returns>
    public TextRange Clone()
    {
        return (TextRange)MemberwiseClone();
    }

    /// <summary>
    /// Return minimum of end.X and start.X
    /// </summary>
    internal int FromX
    {
        get
        {
            if (_end.Line < _start.Line)
            {
                return _end.Column;
            }

            if (_end.Line > _start.Line)
            {
                return _start.Column;
            }

            return Math.Min (_end.Column, _start.Column);
        }
    }

    /// <summary>
    /// Return maximum of end.X and start.X
    /// </summary>
    internal int ToX
    {
        get
        {
            if (_end.Line < _start.Line)
            {
                return _start.Column;
            }

            if (_end.Line > _start.Line)
            {
                return _end.Column;
            }

            return Math.Max (_end.Column, _start.Column);
        }
    }

    public int FromLine
    {
        get { return Math.Min (Start.Line, End.Line); }
    }

    public int ToLine
    {
        get { return Math.Max (Start.Line, End.Line); }
    }

    /// <summary>
    /// Move range right
    /// </summary>
    /// <remarks>This method jump over folded blocks</remarks>
    public bool GoRight()
    {
        var prevStart = _start;
        GoRight (false);
        return prevStart != _start;
    }

    /// <summary>
    /// Move range left
    /// </summary>
    /// <remarks>This method can to go inside folded blocks</remarks>
    public virtual bool GoRightThroughFolded()
    {
        if (ColumnSelectionMode)
        {
            return GoRightThroughFolded_ColumnSelectionMode();
        }

        if (_start.Line >= _textBox.LinesCount - 1 && _start.Column >= _textBox[_textBox.LinesCount - 1].Count)
        {
            return false;
        }

        if (_start.Column < _textBox[_start.Line].Count)
        {
            _start.Offset (1, 0);
        }
        else
        {
            _start = new Place (0, _start.Line + 1);
        }

        _preferedPos = -1;
        _end = _start;
        OnSelectionChanged();
        return true;
    }

    /// <summary>
    /// Move range left
    /// </summary>
    /// <remarks>This method jump over folded blocks</remarks>
    public bool GoLeft()
    {
        ColumnSelectionMode = false;

        var prevStart = _start;
        GoLeft (false);
        return prevStart != _start;
    }

    /// <summary>
    /// Move range left
    /// </summary>
    /// <remarks>This method can to go inside folded blocks</remarks>
    public bool GoLeftThroughFolded()
    {
        ColumnSelectionMode = false;

        if (_start.Column == 0 && _start.Line == 0)
        {
            return false;
        }

        if (_start.Column > 0)
        {
            _start.Offset (-1, 0);
        }
        else
        {
            _start = new Place (_textBox[_start.Line - 1].Count, _start.Line - 1);
        }

        _preferedPos = -1;
        _end = _start;
        OnSelectionChanged();
        return true;
    }

    public void GoLeft (bool shift)
    {
        ColumnSelectionMode = false;

        if (!shift)
        {
            if (_start > _end)
            {
                Start = End;
                return;
            }
        }

        if (_start.Column != 0 || _start.Line != 0)
        {
            if (_start.Column > 0 && _textBox.LineInfos[_start.Line].VisibleState == VisibleState.Visible)
            {
                _start.Offset (-1, 0);
            }
            else
            {
                var i = _textBox.FindPrevVisibleLine (_start.Line);
                if (i == _start.Line)
                {
                    return;
                }

                _start = new Place (_textBox[i].Count, i);
            }
        }

        if (!shift)
        {
            _end = _start;
        }

        OnSelectionChanged();

        _preferedPos = -1;
    }

    public void GoRight (bool shift)
    {
        ColumnSelectionMode = false;

        if (!shift)
        {
            if (_start < _end)
            {
                Start = End;
                return;
            }
        }

        if (_start.Line < _textBox.LinesCount - 1 || _start.Column < _textBox[_textBox.LinesCount - 1].Count)
        {
            if (_start.Column < _textBox[_start.Line].Count && _textBox.LineInfos[_start.Line].VisibleState == VisibleState.Visible)
            {
                _start.Offset (1, 0);
            }
            else
            {
                var i = _textBox.FindNextVisibleLine (_start.Line);
                if (i == _start.Line)
                {
                    return;
                }

                _start = new Place (0, i);
            }
        }

        if (!shift)
        {
            _end = _start;
        }

        OnSelectionChanged();

        _preferedPos = -1;
    }

    internal void GoUp (bool shift)
    {
        ColumnSelectionMode = false;

        if (!shift)
        {
            if (_start.Line > _end.Line)
            {
                Start = End;
                return;
            }
        }

        if (_preferedPos < 0)
        {
            _preferedPos = _start.Column - _textBox.LineInfos[_start.Line]
                .GetWordWrapStringStartPosition (_textBox.LineInfos[_start.Line].GetWordWrapStringIndex (_start.Column));
        }

        var iWW = _textBox.LineInfos[_start.Line].GetWordWrapStringIndex (_start.Column);
        if (iWW == 0)
        {
            if (_start.Line <= 0)
            {
                return;
            }

            var i = _textBox.FindPrevVisibleLine (_start.Line);
            if (i == _start.Line)
            {
                return;
            }

            _start.Line = i;
            iWW = _textBox.LineInfos[_start.Line].WordWrapStringsCount;
        }

        if (iWW > 0)
        {
            var finish = _textBox.LineInfos[_start.Line].GetWordWrapStringFinishPosition (iWW - 1, _textBox[_start.Line]);
            _start.Column = _textBox.LineInfos[_start.Line].GetWordWrapStringStartPosition (iWW - 1) + _preferedPos;
            if (_start.Column > finish + 1)
            {
                _start.Column = finish + 1;
            }
        }

        if (!shift)
        {
            _end = _start;
        }

        OnSelectionChanged();
    }

    internal void GoPageUp (bool shift)
    {
        ColumnSelectionMode = false;

        if (_preferedPos < 0)
        {
            _preferedPos = _start.Column - _textBox.LineInfos[_start.Line]
                .GetWordWrapStringStartPosition (_textBox.LineInfos[_start.Line].GetWordWrapStringIndex (_start.Column));
        }

        var pageHeight = _textBox.ClientRectangle.Height / _textBox.CharHeight - 1;

        for (var i = 0; i < pageHeight; i++)
        {
            var iWW = _textBox.LineInfos[_start.Line].GetWordWrapStringIndex (_start.Column);
            if (iWW == 0)
            {
                if (_start.Line <= 0)
                {
                    break;
                }

                //pass hidden
                var newLine = _textBox.FindPrevVisibleLine (_start.Line);
                if (newLine == _start.Line)
                {
                    break;
                }

                _start.Line = newLine;
                iWW = _textBox.LineInfos[_start.Line].WordWrapStringsCount;
            }

            if (iWW > 0)
            {
                var finish = _textBox.LineInfos[_start.Line].GetWordWrapStringFinishPosition (iWW - 1, _textBox[_start.Line]);
                _start.Column = _textBox.LineInfos[_start.Line].GetWordWrapStringStartPosition (iWW - 1) + _preferedPos;
                if (_start.Column > finish + 1)
                {
                    _start.Column = finish + 1;
                }
            }
        }

        if (!shift)
        {
            _end = _start;
        }

        OnSelectionChanged();
    }

    internal void GoDown (bool shift)
    {
        ColumnSelectionMode = false;

        if (!shift)
        {
            if (_start.Line < _end.Line)
            {
                Start = End;
                return;
            }
        }

        if (_preferedPos < 0)
        {
            _preferedPos = _start.Column - _textBox.LineInfos[_start.Line]
                .GetWordWrapStringStartPosition (_textBox.LineInfos[_start.Line].GetWordWrapStringIndex (_start.Column));
        }

        var iWW = _textBox.LineInfos[_start.Line].GetWordWrapStringIndex (_start.Column);
        if (iWW >= _textBox.LineInfos[_start.Line].WordWrapStringsCount - 1)
        {
            if (_start.Line >= _textBox.LinesCount - 1)
            {
                return;
            }

            //pass hidden
            var i = _textBox.FindNextVisibleLine (_start.Line);
            if (i == _start.Line)
            {
                return;
            }

            _start.Line = i;
            iWW = -1;
        }

        if (iWW < _textBox.LineInfos[_start.Line].WordWrapStringsCount - 1)
        {
            var finish = _textBox.LineInfos[_start.Line].GetWordWrapStringFinishPosition (iWW + 1, _textBox[_start.Line]);
            _start.Column = _textBox.LineInfos[_start.Line].GetWordWrapStringStartPosition (iWW + 1) + _preferedPos;
            if (_start.Column > finish + 1)
            {
                _start.Column = finish + 1;
            }
        }

        if (!shift)
        {
            _end = _start;
        }

        OnSelectionChanged();
    }

    internal void GoPageDown (bool shift)
    {
        ColumnSelectionMode = false;

        if (_preferedPos < 0)
        {
            _preferedPos = _start.Column - _textBox.LineInfos[_start.Line]
                .GetWordWrapStringStartPosition (_textBox.LineInfos[_start.Line].GetWordWrapStringIndex (_start.Column));
        }

        var pageHeight = _textBox.ClientRectangle.Height / _textBox.CharHeight - 1;

        for (var i = 0; i < pageHeight; i++)
        {
            var iWW = _textBox.LineInfos[_start.Line].GetWordWrapStringIndex (_start.Column);
            if (iWW >= _textBox.LineInfos[_start.Line].WordWrapStringsCount - 1)
            {
                if (_start.Line >= _textBox.LinesCount - 1)
                {
                    break;
                }

                //pass hidden
                var newLine = _textBox.FindNextVisibleLine (_start.Line);
                if (newLine == _start.Line)
                {
                    break;
                }

                _start.Line = newLine;
                iWW = -1;
            }

            if (iWW < _textBox.LineInfos[_start.Line].WordWrapStringsCount - 1)
            {
                var finish = _textBox.LineInfos[_start.Line].GetWordWrapStringFinishPosition (iWW + 1, _textBox[_start.Line]);
                _start.Column = _textBox.LineInfos[_start.Line].GetWordWrapStringStartPosition (iWW + 1) + _preferedPos;
                if (_start.Column > finish + 1)
                {
                    _start.Column = finish + 1;
                }
            }
        }

        if (!shift)
        {
            _end = _start;
        }

        OnSelectionChanged();
    }

    internal void GoHome (bool shift)
    {
        ColumnSelectionMode = false;

        if (_start.Line < 0)
        {
            return;
        }

        if (_textBox.LineInfos[_start.Line].VisibleState != VisibleState.Visible)
        {
            return;
        }

        _start = new Place (0, _start.Line);

        if (!shift)
        {
            _end = _start;
        }

        OnSelectionChanged();

        _preferedPos = -1;
    }

    internal void GoEnd (bool shift)
    {
        ColumnSelectionMode = false;

        if (_start.Line < 0)
        {
            return;
        }

        if (_textBox.LineInfos[_start.Line].VisibleState != VisibleState.Visible)
        {
            return;
        }

        _start = new Place (_textBox[_start.Line].Count, _start.Line);

        if (!shift)
        {
            _end = _start;
        }

        OnSelectionChanged();

        _preferedPos = -1;
    }

    /// <summary>
    /// Set style for range
    /// </summary>
    public void SetStyle (Style style)
    {
        //search code for style
        var code = _textBox.GetOrSetStyleLayerIndex (style);

        //set code to chars
        SetStyle (ToStyleIndex (code));

        //
        _textBox.Invalidate();
    }

    /// <summary>
    /// Set style for given regex pattern
    /// </summary>
    public void SetStyle (Style style, string regexPattern)
    {
        //search code for style
        var layer = ToStyleIndex (_textBox.GetOrSetStyleLayerIndex (style));
        SetStyle (layer, regexPattern, RegexOptions.None);
    }

    /// <summary>
    /// Set style for given regex
    /// </summary>
    public void SetStyle (Style style, Regex regex)
    {
        //search code for style
        var layer = ToStyleIndex (_textBox.GetOrSetStyleLayerIndex (style));
        SetStyle (layer, regex);
    }


    /// <summary>
    /// Set style for given regex pattern
    /// </summary>
    public void SetStyle (Style style, string regexPattern, RegexOptions options)
    {
        //search code for style
        var layer = ToStyleIndex (_textBox.GetOrSetStyleLayerIndex (style));
        SetStyle (layer, regexPattern, options);
    }

    /// <summary>
    /// Set style for given regex pattern
    /// </summary>
    public void SetStyle (StyleIndex styleLayer, string regexPattern, RegexOptions options)
    {
        if (Math.Abs (Start.Line - End.Line) > 1000)
        {
            options |= SyntaxHighlighter.RegexCompiledOption;
        }

        //
        foreach (var range in GetRanges (regexPattern, options))
            range.SetStyle (styleLayer);

        //
        _textBox.Invalidate();
    }

    /// <summary>
    /// Set style for given regex pattern
    /// </summary>
    public void SetStyle (StyleIndex styleLayer, Regex regex)
    {
        foreach (var range in GetRanges (regex))
            range.SetStyle (styleLayer);

        //
        _textBox.Invalidate();
    }

    /// <summary>
    /// Appends style to chars of range
    /// </summary>
    public void SetStyle (StyleIndex styleIndex)
    {
        //set code to chars
        var fromLine = Math.Min (End.Line, Start.Line);
        var toLine = Math.Max (End.Line, Start.Line);
        var fromChar = FromX;
        var toChar = ToX;
        if (fromLine < 0)
        {
            return;
        }

        //
        for (var y = fromLine; y <= toLine; y++)
        {
            var fromX = y == fromLine ? fromChar : 0;
            var toX = y == toLine ? Math.Min (toChar - 1, _textBox[y].Count - 1) : _textBox[y].Count - 1;
            for (var x = fromX; x <= toX; x++)
            {
                var c = _textBox[y][x];
                c.style |= styleIndex;
                _textBox[y][x] = c;
            }
        }
    }

    /// <summary>
    /// Sets folding markers
    /// </summary>
    /// <param name="startFoldingPattern">Pattern for start folding line</param>
    /// <param name="finishFoldingPattern">Pattern for finish folding line</param>
    public void SetFoldingMarkers (string startFoldingPattern, string finishFoldingPattern)
    {
        SetFoldingMarkers (startFoldingPattern, finishFoldingPattern, SyntaxHighlighter.RegexCompiledOption);
    }

    /// <summary>
    /// Sets folding markers
    /// </summary>
    /// <param name="startFoldingPattern">Pattern for start folding line</param>
    /// <param name="finishFoldingPattern">Pattern for finish folding line</param>
    public void SetFoldingMarkers (string startFoldingPattern, string finishFoldingPattern, RegexOptions options)
    {
        if (startFoldingPattern == finishFoldingPattern)
        {
            SetFoldingMarkers (startFoldingPattern, options);
            return;
        }

        foreach (var range in GetRanges (startFoldingPattern, options))
            _textBox[range.Start.Line].FoldingStartMarker = startFoldingPattern;

        foreach (var range in GetRanges (finishFoldingPattern, options))
            _textBox[range.Start.Line].FoldingEndMarker = startFoldingPattern;

        //
        _textBox.Invalidate();
    }

    /// <summary>
    /// Sets folding markers
    /// </summary>
    /// <param name="startEndFoldingPattern">Pattern for start and end folding line</param>
    public void SetFoldingMarkers (string foldingPattern, RegexOptions options)
    {
        foreach (var range in GetRanges (foldingPattern, options))
        {
            if (range.Start.Line > 0)
            {
                _textBox[range.Start.Line - 1].FoldingEndMarker = foldingPattern;
            }

            _textBox[range.Start.Line].FoldingStartMarker = foldingPattern;
        }

        _textBox.Invalidate();
    }

    /// <summary>
    /// Finds ranges for given regex pattern
    /// </summary>
    /// <param name="regexPattern">Regex pattern</param>
    /// <returns>Enumeration of ranges</returns>
    public IEnumerable<TextRange> GetRanges (string regexPattern)
    {
        return GetRanges (regexPattern, RegexOptions.None);
    }

    /// <summary>
    /// Finds ranges for given regex pattern
    /// </summary>
    /// <param name="regexPattern">Regex pattern</param>
    /// <returns>Enumeration of ranges</returns>
    public IEnumerable<TextRange> GetRanges (string regexPattern, RegexOptions options)
    {
        //get text
        string text;
        List<Place> charIndexToPlace;
        GetText (out text, out charIndexToPlace);

        //create regex
        var regex = new Regex (regexPattern, options);

        //
        foreach (Match m in regex.Matches (text))
        {
            var r = new TextRange (this._textBox);

            //try get 'range' group, otherwise use group 0
            var group = m.Groups["range"];
            if (!group.Success)
            {
                group = m.Groups[0];
            }

            //
            r.Start = charIndexToPlace[group.Index];
            r.End = charIndexToPlace[group.Index + group.Length];
            yield return r;
        }
    }

    /// <summary>
    /// Finds ranges for given regex pattern.
    /// Search is separately in each line.
    /// This method requires less memory than GetRanges().
    /// </summary>
    /// <param name="regexPattern">Regex pattern</param>
    /// <returns>Enumeration of ranges</returns>
    public IEnumerable<TextRange> GetRangesByLines (string regexPattern, RegexOptions options)
    {
        var regex = new Regex (regexPattern, options);
        foreach (var r in GetRangesByLines (regex))
            yield return r;
    }

    /// <summary>
    /// Finds ranges for given regex.
    /// Search is separately in each line.
    /// This method requires less memory than GetRanges().
    /// </summary>
    /// <param name="regex">Regex</param>
    /// <returns>Enumeration of ranges</returns>
    public IEnumerable<TextRange> GetRangesByLines (Regex regex)
    {
        Normalize();

        var fts = _textBox.TextSource as FileTextSource; //<----!!!! ugly

        //enumaerate lines
        for (var iLine = Start.Line; iLine <= End.Line; iLine++)
        {
            //
            var isLineLoaded = fts != null ? fts.IsLineLoaded (iLine) : true;

            //
            var r = new TextRange (_textBox, new Place (0, iLine), new Place (_textBox[iLine].Count, iLine));
            if (iLine == Start.Line || iLine == End.Line)
            {
                r = r.GetIntersectionWith (this);
            }

            foreach (var foundRange in r.GetRanges (regex))
                yield return foundRange;

            if (!isLineLoaded)
            {
                fts.UnloadLine (iLine);
            }
        }
    }

    /// <summary>
    /// Finds ranges for given regex pattern.
    /// Search is separately in each line (order of lines is reversed).
    /// This method requires less memory than GetRanges().
    /// </summary>
    /// <param name="regexPattern">Regex pattern</param>
    /// <returns>Enumeration of ranges</returns>
    public IEnumerable<TextRange> GetRangesByLinesReversed (string regexPattern, RegexOptions options)
    {
        Normalize();

        //create regex
        var regex = new Regex (regexPattern, options);

        //
        var fts = _textBox.TextSource as FileTextSource; //<----!!!! ugly

        //enumaerate lines
        for (var iLine = End.Line; iLine >= Start.Line; iLine--)
        {
            //
            var isLineLoaded = fts != null ? fts.IsLineLoaded (iLine) : true;

            //
            var r = new TextRange (_textBox, new Place (0, iLine), new Place (_textBox[iLine].Count, iLine));
            if (iLine == Start.Line || iLine == End.Line)
            {
                r = r.GetIntersectionWith (this);
            }

            var list = new List<TextRange>();

            foreach (var foundRange in r.GetRanges (regex))
                list.Add (foundRange);

            for (var i = list.Count - 1; i >= 0; i--)
                yield return list[i];

            if (!isLineLoaded)
            {
                fts.UnloadLine (iLine);
            }
        }
    }

    /// <summary>
    /// Finds ranges for given regex
    /// </summary>
    /// <returns>Enumeration of ranges</returns>
    public IEnumerable<TextRange> GetRanges (Regex regex)
    {
        //get text
        string text;
        List<Place> charIndexToPlace;
        GetText (out text, out charIndexToPlace);

        //
        foreach (Match m in regex.Matches (text))
        {
            var r = new TextRange (this._textBox);

            //try get 'range' group, otherwise use group 0
            var group = m.Groups["range"];
            if (!group.Success)
            {
                group = m.Groups[0];
            }

            //
            r.Start = charIndexToPlace[group.Index];
            r.End = charIndexToPlace[group.Index + group.Length];
            yield return r;
        }
    }

    /// <summary>
    /// Clear styles of range
    /// </summary>
    public void ClearStyle (params Style[] styles)
    {
        try
        {
            ClearStyle (_textBox.GetStyleIndexMask (styles));
        }
        catch
        {
            ;
        }
    }

    /// <summary>
    /// Clear styles of range
    /// </summary>
    public void ClearStyle (StyleIndex styleIndex)
    {
        //set code to chars
        var fromLine = Math.Min (End.Line, Start.Line);
        var toLine = Math.Max (End.Line, Start.Line);
        var fromChar = FromX;
        var toChar = ToX;
        if (fromLine < 0)
        {
            return;
        }

        //
        for (var y = fromLine; y <= toLine; y++)
        {
            var fromX = y == fromLine ? fromChar : 0;
            var toX = y == toLine ? Math.Min (toChar - 1, _textBox[y].Count - 1) : _textBox[y].Count - 1;
            for (var x = fromX; x <= toX; x++)
            {
                var c = _textBox[y][x];
                c.style &= ~styleIndex;
                _textBox[y][x] = c;
            }
        }

        //
        _textBox.Invalidate();
    }

    /// <summary>
    /// Clear folding markers of all lines of range
    /// </summary>
    public void ClearFoldingMarkers()
    {
        //set code to chars
        var fromLine = Math.Min (End.Line, Start.Line);
        var toLine = Math.Max (End.Line, Start.Line);
        if (fromLine < 0)
        {
            return;
        }

        //
        for (var y = fromLine; y <= toLine; y++)
            _textBox[y].ClearFoldingMarkers();

        //
        _textBox.Invalidate();
    }

    void OnSelectionChanged()
    {
        //clear cache
        _cachedTextVersion = -1;
        _cachedText = null;
        _cachedCharIndexToPlace = null;

        //
        if (_textBox.Selection == this)
        {
            if (_updating == 0)
            {
                _textBox.OnSelectionChanged();
            }
        }
    }

    /// <summary>
    /// Starts selection position updating
    /// </summary>
    public void BeginUpdate()
    {
        _updating++;
    }

    /// <summary>
    /// Ends selection position updating
    /// </summary>
    public void EndUpdate()
    {
        _updating--;
        if (_updating == 0)
        {
            OnSelectionChanged();
        }
    }

    /// <summary>
    /// Exchanges Start and End if End appears before Start
    /// </summary>
    public void Normalize()
    {
        if (Start > End)
        {
            Inverse();
        }
    }

    /// <summary>
    /// Exchanges Start and End
    /// </summary>
    public void Inverse()
    {
        (_start, _end) = (_end, _start);
    }

    /// <summary>
    /// Expands range from first char of Start line to last char of End line
    /// </summary>
    public void Expand()
    {
        Normalize();
        _start = new Place (0, _start.Line);
        _end = new Place (_textBox.GetLineLength (_end.Line), _end.Line);
    }

    IEnumerator<Place> IEnumerable<Place>.GetEnumerator()
    {
        if (ColumnSelectionMode)
        {
            foreach (var p in GetEnumerator_ColumnSelectionMode())
                yield return p;
            yield break;
        }

        var fromLine = Math.Min (_end.Line, _start.Line);
        var toLine = Math.Max (_end.Line, _start.Line);
        var fromChar = FromX;
        var toChar = ToX;
        if (fromLine < 0)
        {
            yield break;
        }

        //
        for (var y = fromLine; y <= toLine; y++)
        {
            var fromX = y == fromLine ? fromChar : 0;
            var toX = y == toLine ? Math.Min (toChar - 1, _textBox[y].Count - 1) : _textBox[y].Count - 1;
            for (var x = fromX; x <= toX; x++)
                yield return new Place (x, y);
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return (this as IEnumerable<Place>).GetEnumerator();
    }

    /// <summary>
    /// Chars of range (exclude \n)
    /// </summary>
    public IEnumerable<Character> Chars
    {
        get
        {
            if (ColumnSelectionMode)
            {
                foreach (var p in GetEnumerator_ColumnSelectionMode())
                    yield return _textBox[p];
                yield break;
            }

            var fromLine = Math.Min (_end.Line, _start.Line);
            var toLine = Math.Max (_end.Line, _start.Line);
            var fromChar = FromX;
            var toChar = ToX;
            if (fromLine < 0)
            {
                yield break;
            }

            //
            for (var y = fromLine; y <= toLine; y++)
            {
                var fromX = y == fromLine ? fromChar : 0;
                var toX = y == toLine ? Math.Min (toChar - 1, _textBox[y].Count - 1) : _textBox[y].Count - 1;
                var line = _textBox[y];
                for (var x = fromX; x <= toX; x++)
                    yield return line[x];
            }
        }
    }

    /// <summary>
    /// Get fragment of text around Start place. Returns maximal matched to pattern fragment.
    /// </summary>
    /// <param name="allowedSymbolsPattern">Allowed chars pattern for fragment</param>
    /// <returns>Range of found fragment</returns>
    public TextRange GetFragment (string allowedSymbolsPattern)
    {
        return GetFragment (allowedSymbolsPattern, RegexOptions.None);
    }

    /// <summary>
    /// Get fragment of text around Start place. Returns maximal matched to given Style.
    /// </summary>
    /// <param name="style">Allowed style for fragment</param>
    /// <returns>Range of found fragment</returns>
    public TextRange GetFragment (Style style, bool allowLineBreaks)
    {
        var mask = _textBox.GetStyleIndexMask (new Style[] { style });

        //
        var r = new TextRange (_textBox);
        r.Start = Start;

        //go left, check style
        while (r.GoLeftThroughFolded())
        {
            if (!allowLineBreaks && r.CharAfterStart == '\n')
            {
                break;
            }

            if (r.Start.Column < _textBox.GetLineLength (r.Start.Line))
            {
                if ((_textBox[r.Start].style & mask) == 0)
                {
                    r.GoRightThroughFolded();
                    break;
                }
            }
        }

        var startFragment = r.Start;

        r.Start = Start;

        //go right, check style
        do
        {
            if (!allowLineBreaks && r.CharAfterStart == '\n')
            {
                break;
            }

            if (r.Start.Column < _textBox.GetLineLength (r.Start.Line))
            {
                if ((_textBox[r.Start].style & mask) == 0)
                {
                    break;
                }
            }
        } while (r.GoRightThroughFolded());

        var endFragment = r.Start;

        return new TextRange (_textBox, startFragment, endFragment);
    }

    /// <summary>
    /// Get fragment of text around Start place. Returns maximal mathed to pattern fragment.
    /// </summary>
    /// <param name="allowedSymbolsPattern">Allowed chars pattern for fragment</param>
    /// <returns>Range of found fragment</returns>
    public TextRange GetFragment (string allowedSymbolsPattern, RegexOptions options)
    {
        var r = new TextRange (_textBox);
        r.Start = Start;
        var regex = new Regex (allowedSymbolsPattern, options);

        //go left, check symbols
        while (r.GoLeftThroughFolded())
        {
            if (!regex.IsMatch (r.CharAfterStart.ToString()))
            {
                r.GoRightThroughFolded();
                break;
            }
        }

        var startFragment = r.Start;

        r.Start = Start;

        //go right, check symbols
        do
        {
            if (!regex.IsMatch (r.CharAfterStart.ToString()))
            {
                break;
            }
        } while (r.GoRightThroughFolded());

        var endFragment = r.Start;

        return new TextRange (_textBox, startFragment, endFragment);
    }

    bool IsIdentifierChar (char c)
    {
        return char.IsLetterOrDigit (c) || c == '_';
    }

    bool IsSpaceChar (char c)
    {
        return c == ' ' || c == '\t';
    }

    public void GoWordLeft (bool shift)
    {
        ColumnSelectionMode = false;

        if (!shift && _start > _end)
        {
            Start = End;
            return;
        }

        var range = this.Clone(); //to OnSelectionChanged disable
        var wasSpace = false;
        while (IsSpaceChar (range.CharBeforeStart))
        {
            wasSpace = true;
            range.GoLeft (shift);
        }

        var wasIdentifier = false;
        while (IsIdentifierChar (range.CharBeforeStart))
        {
            wasIdentifier = true;
            range.GoLeft (shift);
        }

        if (!wasIdentifier && (!wasSpace || range.CharBeforeStart != '\n'))
        {
            range.GoLeft (shift);
        }

        this.Start = range.Start;
        this.End = range.End;

        if (_textBox.LineInfos[Start.Line].VisibleState != VisibleState.Visible)
        {
            GoRight (shift);
        }
    }

    public void GoWordRight (bool shift, bool goToStartOfNextWord = false)
    {
        ColumnSelectionMode = false;

        if (!shift && _start < _end)
        {
            Start = End;
            return;
        }

        var range = this.Clone(); //to OnSelectionChanged disable

        var wasNewLine = false;


        if (range.CharAfterStart == '\n')
        {
            range.GoRight (shift);
            wasNewLine = true;
        }

        var wasSpace = false;
        while (IsSpaceChar (range.CharAfterStart))
        {
            wasSpace = true;
            range.GoRight (shift);
        }

        if (!((wasSpace || wasNewLine) && goToStartOfNextWord))
        {
            var wasIdentifier = false;
            while (IsIdentifierChar (range.CharAfterStart))
            {
                wasIdentifier = true;
                range.GoRight (shift);
            }

            if (!wasIdentifier)
            {
                range.GoRight (shift);
            }

            if (goToStartOfNextWord && !wasSpace)
            {
                while (IsSpaceChar (range.CharAfterStart))
                    range.GoRight (shift);
            }
        }

        this.Start = range.Start;
        this.End = range.End;

        if (_textBox.LineInfos[Start.Line].VisibleState != VisibleState.Visible)
        {
            GoLeft (shift);
        }
    }

    internal void GoFirst (bool shift)
    {
        ColumnSelectionMode = false;

        _start = new Place (0, 0);
        if (_textBox.LineInfos[Start.Line].VisibleState != VisibleState.Visible)
        {
            _textBox.ExpandBlock (Start.Line);
        }

        if (!shift)
        {
            _end = _start;
        }

        OnSelectionChanged();
    }

    internal void GoLast (bool shift)
    {
        ColumnSelectionMode = false;

        _start = new Place (_textBox[_textBox.LinesCount - 1].Count, _textBox.LinesCount - 1);
        if (_textBox.LineInfos[Start.Line].VisibleState != VisibleState.Visible)
        {
            _textBox.ExpandBlock (Start.Line);
        }

        if (!shift)
        {
            _end = _start;
        }

        OnSelectionChanged();
    }

    public static StyleIndex ToStyleIndex (int i)
    {
        return (StyleIndex)(1 << i);
    }

    public RangeRect Bounds
    {
        get
        {
            var minX = Math.Min (Start.Column, End.Column);
            var minY = Math.Min (Start.Line, End.Line);
            var maxX = Math.Max (Start.Column, End.Column);
            var maxY = Math.Max (Start.Line, End.Line);
            return new RangeRect (minY, minX, maxY, maxX);
        }
    }

    public IEnumerable<TextRange> GetSubRanges (bool includeEmpty)
    {
        if (!ColumnSelectionMode)
        {
            yield return this;
            yield break;
        }

        var rect = Bounds;
        for (var y = rect.StartLine; y <= rect.EndLine; y++)
        {
            if (rect.StartChar > _textBox[y].Count && !includeEmpty)
            {
                continue;
            }

            var r = new TextRange (_textBox, rect.StartChar, y, Math.Min (rect.EndChar, _textBox[y].Count), y);
            yield return r;
        }
    }

    /// <summary>
    /// Range is readonly?
    /// This property return True if any char of the range contains ReadOnlyStyle.
    /// Set this property to True/False to mark chars of the range as Readonly/Writable.
    /// </summary>
    public bool ReadOnly
    {
        get
        {
            if (_textBox.ReadOnly)
            {
                return true;
            }

            ReadOnlyStyle readonlyStyle = null;
            foreach (var style in _textBox.Styles)
                if (style is ReadOnlyStyle)
                {
                    readonlyStyle = (ReadOnlyStyle)style;
                    break;
                }

            if (readonlyStyle != null)
            {
                var si = ToStyleIndex (_textBox.GetStyleIndex (readonlyStyle));

                if (IsEmpty)
                {
                    //check previous and next chars
                    var line = _textBox[_start.Line];
                    if (ColumnSelectionMode)
                    {
                        foreach (var sr in GetSubRanges (false))
                        {
                            line = _textBox[sr._start.Line];
                            if (sr._start.Column < line.Count && sr._start.Column > 0)
                            {
                                var left = line[sr._start.Column - 1];
                                var right = line[sr._start.Column];
                                if ((left.style & si) != 0 &&
                                    (right.style & si) != 0)
                                {
                                    return true; //we are between readonly chars
                                }
                            }
                        }
                    }
                    else if (_start.Column < line.Count && _start.Column > 0)
                    {
                        var left = line[_start.Column - 1];
                        var right = line[_start.Column];
                        if ((left.style & si) != 0 &&
                            (right.style & si) != 0)
                        {
                            return true; //we are between readonly chars
                        }
                    }
                }
                else
                {
                    foreach (var c in Chars)
                        if ((c.style & si) != 0) //found char with ReadonlyStyle
                        {
                            return true;
                        }
                }
            }

            return false;
        }

        set
        {
            //find exists ReadOnlyStyle of style buffer
            ReadOnlyStyle readonlyStyle = null;
            foreach (var style in _textBox.Styles)
                if (style is ReadOnlyStyle)
                {
                    readonlyStyle = (ReadOnlyStyle)style;
                    break;
                }

            //create ReadOnlyStyle
            if (readonlyStyle == null)
            {
                readonlyStyle = new ReadOnlyStyle();
            }

            //set/clear style
            if (value)
            {
                SetStyle (readonlyStyle);
            }
            else
            {
                ClearStyle (readonlyStyle);
            }
        }
    }

    /// <summary>
    /// Is char before range readonly
    /// </summary>
    /// <returns></returns>
    public bool IsReadOnlyLeftChar()
    {
        if (_textBox.ReadOnly)
        {
            return true;
        }

        var r = Clone();

        r.Normalize();
        if (r._start.Column == 0)
        {
            return false;
        }

        if (ColumnSelectionMode)
        {
            r.GoLeft_ColumnSelectionMode();
        }
        else
        {
            r.GoLeft (true);
        }

        return r.ReadOnly;
    }

    /// <summary>
    /// Is char after range readonly
    /// </summary>
    /// <returns></returns>
    public bool IsReadOnlyRightChar()
    {
        if (_textBox.ReadOnly)
        {
            return true;
        }

        var r = Clone();

        r.Normalize();
        if (r._end.Column >= _textBox[_end.Line].Count)
        {
            return false;
        }

        if (ColumnSelectionMode)
        {
            r.GoRight_ColumnSelectionMode();
        }
        else
        {
            r.GoRight (true);
        }

        return r.ReadOnly;
    }

    public IEnumerable<Place> GetPlacesCyclic
        (
            Place startPlace,
            bool backward = false
        )
    {
        if (backward)
        {
            var r = new TextRange (this._textBox, startPlace, startPlace);
            while (r.GoLeft() && r._start >= Start)
            {
                if (r.Start.Column < _textBox[r.Start.Line].Count)
                {
                    yield return r.Start;
                }
            }

            r = new TextRange (this._textBox, End, End);
            while (r.GoLeft() && r._start >= startPlace)
            {
                if (r.Start.Column < _textBox[r.Start.Line].Count)
                {
                    yield return r.Start;
                }
            }
        }
        else
        {
            var r = new TextRange (this._textBox, startPlace, startPlace);
            if (startPlace < End)
            {
                do
                {
                    if (r.Start.Column < _textBox[r.Start.Line].Count)
                    {
                        yield return r.Start;
                    }
                } while (r.GoRight());
            }

            r = new TextRange (this._textBox, Start, Start);
            if (r.Start < startPlace)
            {
                do
                {
                    if (r.Start.Column < _textBox[r.Start.Line].Count)
                    {
                        yield return r.Start;
                    }
                } while (r.GoRight() && r.Start < startPlace);
            }
        }
    }

    #region ColumnSelectionMode

    private TextRange GetIntersectionWith_ColumnSelectionMode (TextRange range)
    {
        if (range.Start.Line != range.End.Line)
        {
            return new TextRange (_textBox, Start, Start);
        }

        var rect = Bounds;
        if (range.Start.Line < rect.StartLine || range.Start.Line > rect.EndLine)
        {
            return new TextRange (_textBox, Start, Start);
        }

        return new TextRange (_textBox, rect.StartChar, range.Start.Line, rect.EndChar, range.Start.Line)
            .GetIntersectionWith (range);
    }

    private bool GoRightThroughFolded_ColumnSelectionMode()
    {
        var boundes = Bounds;
        var endOfLines = true;
        for (var iLine = boundes.StartLine; iLine <= boundes.EndLine; iLine++)
            if (boundes.EndChar < _textBox[iLine].Count)
            {
                endOfLines = false;
                break;
            }

        if (endOfLines)
        {
            return false;
        }

        var start = Start;
        var end = End;
        start.Offset (1, 0);
        end.Offset (1, 0);
        BeginUpdate();
        Start = start;
        End = end;
        EndUpdate();

        return true;
    }

    private IEnumerable<Place> GetEnumerator_ColumnSelectionMode()
    {
        var bounds = Bounds;
        if (bounds.StartLine < 0)
        {
            yield break;
        }

        //
        for (var y = bounds.StartLine; y <= bounds.EndLine; y++)
        {
            for (var x = bounds.StartChar; x < bounds.EndChar; x++)
            {
                if (x < _textBox[y].Count)
                {
                    yield return new Place (x, y);
                }
            }
        }
    }

    private string Text_ColumnSelectionMode
    {
        get
        {
            var sb = new StringBuilder();
            var bounds = Bounds;
            if (bounds.StartLine < 0)
            {
                return "";
            }

            //
            for (var y = bounds.StartLine; y <= bounds.EndLine; y++)
            {
                for (var x = bounds.StartChar; x < bounds.EndChar; x++)
                {
                    if (x < _textBox[y].Count)
                    {
                        sb.Append (_textBox[y][x].c);
                    }
                }

                if (bounds.EndLine != bounds.StartLine && y != bounds.EndLine)
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }
    }

    private int Length_ColumnSelectionMode (bool withNewLines)
    {
        var bounds = Bounds;
        if (bounds.StartLine < 0)
        {
            return 0;
        }

        var cnt = 0;

        //
        for (var y = bounds.StartLine; y <= bounds.EndLine; y++)
        {
            for (var x = bounds.StartChar; x < bounds.EndChar; x++)
            {
                if (x < _textBox[y].Count)
                {
                    cnt++;
                }
            }

            if (withNewLines && bounds.EndLine != bounds.StartLine && y != bounds.EndLine)
            {
                cnt += Environment.NewLine.Length;
            }
        }

        return cnt;
    }

    internal void GoDown_ColumnSelectionMode()
    {
        var iLine = _textBox.FindNextVisibleLine (End.Line);
        End = new Place (End.Column, iLine);
    }

    internal void GoUp_ColumnSelectionMode()
    {
        var iLine = _textBox.FindPrevVisibleLine (End.Line);
        End = new Place (End.Column, iLine);
    }

    internal void GoRight_ColumnSelectionMode()
    {
        End = new Place (End.Column + 1, End.Line);
    }

    internal void GoLeft_ColumnSelectionMode()
    {
        if (End.Column > 0)
        {
            End = new Place (End.Column - 1, End.Line);
        }
    }

    #endregion

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return "Start: " + Start + " End: " + End;
    }

    #endregion
}
