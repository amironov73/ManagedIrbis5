// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ConsoleControl.cs -- контрол, отображающий псевдоконсоль
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

using AM.Avalonia.SourceGeneration;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;

using JetBrains.Annotations;

using Brushes = Avalonia.Media.Brushes;
using Point = Avalonia.Point;

#endregion

namespace AM.Avalonia.Controls;

/// <summary>
/// Контрол. отображающий псевдоконсоль.
/// </summary>
[PublicAPI]
public sealed class ConsoleControl
    : Control
{
    #region Properties

    /// <summary>
    /// Разрешено ли выводить эхо?
    /// </summary>
    public bool EchoEnabled { get; set; }

    /// <summary>
    /// Номер текущей колонки (нумерация с 0).
    /// </summary>
    public int CurrentColumn => _currentColumn;

    /// <summary>
    /// Номер текущей строки (нумерация с 0).
    /// </summary>
    public int CurrentRow => _currentRow;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ConsoleControl()
        : this (80, 25)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ConsoleControl
        (
            int columns,
            int rows
        )
    {
        Sure.Positive (columns);
        Sure.Positive (rows);

        Focusable = true;
        _columns = columns;
        _rows = rows;
        _buffer = new char[rows][];
        for (var i = 0; i < _rows; i++)
        {
            _buffer[i] = new char[_columns];
        }
        Clear();

        DispatcherTimer.Run
            (
                BlinkCursor,
                TimeSpan.FromMilliseconds (300)
            );
    }

    #endregion

    #region Private members

    private int _currentColumn;

    private int _currentRow;

    /// <summary>
    /// Количество колонок с символами.
    /// </summary>
    [StyledProperty] private int _columns;

    /// <summary>
    /// Количество строк с символами.
    /// </summary>
    [StyledProperty] private int _rows;

    private char[][] _buffer;

    private bool _cursorDrawn;

    /// <summary>
    /// Продвижение курсора.
    /// </summary>
    private void AdvanceCursor()
    {
        if (++_currentColumn >= _columns)
        {
            _currentColumn = 0;
            _currentRow++;
        }

        ScrollIfNecessarry();
    }

    private bool BlinkCursor()
    {
        _cursorDrawn = !_cursorDrawn;
        InvalidateVisual();

        return true;
    }

    private void ScrollDown()
    {
        for (var i = 0; i < _rows - 1; i++)
        {
            Array.Copy (_buffer[i + 1], _buffer[i], _columns);
        }

        Array.Fill (_buffer[_rows - 1], ' ');
    }

    private void ScrollIfNecessarry()
    {
        while (_currentRow >= _rows)
        {
            ScrollDown();
            _currentRow--;
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Очистка консоли, пермемещение курсора в левый верхний угол.
    /// </summary>
    public void Clear()
    {
        _currentColumn = _currentRow = 0;
        for (var i = 0; i < _rows; i++)
        {
            Array.Fill (_buffer[i], ' ');
        }

        InvalidateVisual();
    }

    /// <summary>
    /// Переход на новую строку.
    /// </summary>
    public void NewLine()
    {
        _currentColumn = 0;
        _currentRow++;
        ScrollIfNecessarry();
    }

    /// <summary>
    /// Вывод символа в указанной позиции.
    /// </summary>
    public void PutChar
        (
            int column,
            int row,
            char chr
        )
    {
        Sure.InRange (column, 0, _columns);
        Sure.InRange (row, 0, _rows);

        _buffer[row][column] = chr;
        InvalidateVisual();
    }

    /// <summary>
    /// Ввод одного символа.
    /// </summary>
    public char ReadChar() => '\0';

    /// <summary>
    /// Ввод одной строки.
    /// </summary>
    public string? ReadLine() => null;

    /// <summary>
    /// Вывод символа в текущую позицию плюс сдвиг курсора.
    /// </summary>
    public void Write
        (
            char chr,
            int repeats = 1
        )
    {
        switch (chr)
        {
            case '\r':
                Debug.WriteLine ("\\r");
                _currentColumn = 0;
                break;

            case '\n':
                Debug.WriteLine ("\\n");
                NewLine();
                break;

            case '\t':
                var delta = 8 - _currentColumn % 8;
                Write (' ', delta);
                break;

            default:
                while (--repeats >= 0)
                {
                    PutChar (CurrentColumn, CurrentRow, chr);
                    AdvanceCursor();
                }

                break;
        }
    }

    /// <summary>
    /// Вывод строки символов в текущую позицию плюс сдвиг курсора.
    /// </summary>
    public void Write
        (
            IEnumerable<char> text
        )
    {
        Sure.NotNull (text);

        foreach (var chr in text)
        {
            Write (chr);
        }
    }

    /// <summary>
    /// Перевод строки.
    /// </summary>
    public void WriteLine() => NewLine();

    /// <summary>
    /// Вывод текста с принудительным переводом строки.
    /// </summary>
    public void WriteLine
        (
            IEnumerable<char> text
        )
    {
        Sure.NotNull (text);

        Write (text);
        WriteLine();
    }

    #endregion

    #region Control members

    /// <inheritdoc cref="InputElement.OnKeyDown"/>
    protected override void OnKeyDown
        (
            KeyEventArgs eventArgs
        )
    {
        eventArgs.Handled = true;

        if (!EchoEnabled)
        {
            return;
        }

        if (eventArgs.Key == Key.Enter)
        {
            NewLine();
        }
        else
        {
            var key = eventArgs.KeySymbol;
            if (!string.IsNullOrEmpty (key))
            {
                Debug.WriteLine (key);
                Write (key);
            }
        }

        // base.OnKeyDown (eventArgs);
    }

    /// <inheritdoc cref="Visual.Render"/>
    public override void Render
        (
            DrawingContext context
        )
    {
        Sure.NotNull (context);

        context.FillRectangle
            (
                Brushes.Blue,
                Bounds
            );

        var firaMono = AvaloniaUtility.GetFiraMono();
        var typeface = new Typeface (firaMono);

        var y = 0;
        for (var i = 0; i < _rows; i++)
        {
            var text = new FormattedText
                (
                    new string (_buffer[i]),
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typeface,
                    14.0,
                    Brushes.White
                );
            var point = new Point (0, y);
            context.DrawText (text, point);
            y += 14;
        }

        if (_cursorDrawn)
        {
            var cursorX = 9.0 * _currentColumn;
            var cursorY = 14.0 * _currentRow;
            var pen = new Pen (Brushes.White);
            context.DrawLine
                (
                    pen,
                    new Point (cursorX, cursorY),
                    new Point (cursorX, cursorY + 14)
                );
            cursorX++;
            context.DrawLine
                (
                    pen,
                    new Point (cursorX, cursorY),
                    new Point (cursorX, cursorY + 14)
                );
        }
    }

    #endregion
}
