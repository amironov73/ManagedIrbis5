﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ConsoleControl.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;

using Timer = System.Windows.Forms.Timer;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Pseudo-console.
    /// </summary>
    // ReSharper disable once RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    [ToolboxBitmap(typeof(BusyStripe), "Images.ConsoleControl.bmp")]
    public sealed class ConsoleControl
        : Control
    {
        #region Constants

        /// <summary>
        /// Default cursor height (lines).
        /// </summary>
        public const int DefaultCursorHeight = 2;

        /// <summary>
        /// Default window height.
        /// </summary>
        public const int DefaultWindowHeight = 25;

        /// <summary>
        /// Default window width.
        /// </summary>
        public const int DefaultWindowWidth = 80;

        /// <summary>
        /// Default font name.
        /// </summary>
        public const string DefaultFontName = "Courier New";

        /// <summary>
        /// Default font size.
        /// </summary>
        public const float DefaultFontSize = 10f;

        /// <summary>
        /// Maximal window height.
        /// </summary>
        public const int MaximalWindowHeight = 100;

        /// <summary>
        /// Maximal window width.
        /// </summary>
        public const int MaximalWindowWidth = 100;

        /// <summary>
        /// Minimal window height.
        /// </summary>
        public const int MinimalWindowHeight = 2;

        /// <summary>
        /// Minimal window width.
        /// </summary>
        public const int MinimalWindowWidth = 2;

        #endregion

        #region Nested classes

        struct Cell
        {
            public bool Emphasized;

            public char Character;

            public Color ForeColor;

            public Color BackColor;

            public override string ToString()
            {
                return new string(Character, 1);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raised on input (Enter key).
        /// </summary>
        public event EventHandler<ConsoleInputEventArgs> Input;

        /// <summary>
        /// Raised when TAB key pressed.
        /// </summary>
        public event EventHandler<ConsoleInputEventArgs> TabPressed;

        #endregion

        #region Properties

        /// <summary>
        /// Allow input?
        /// </summary>
        [DefaultValue(false)]
        public bool AllowInput { get; set; }

        /// <summary>
        /// Echo input?
        /// </summary>
        [DefaultValue(true)]
        public bool EchoInput { get; set; }

        /// <summary>
        /// Bold version of the <see cref="P:Font"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Font ItalicFont { get { return _italicFont; } }

        /// <summary>
        /// Cursor height.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(DefaultCursorHeight)]
        public int CursorHeight
        {
            get { return _cursorHeight; }
            set
            {
                if (value < 1
                    || value > _cellHeight)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _cursorHeight = value;
            }
        }

        /// <summary>
        /// Column position of the cursor.
        /// </summary>
        [Browsable(false)]
        public int CursorLeft { get; set; }

        /// <summary>
        /// Row position of the cursor.
        /// </summary>
        [Browsable(false)]
        public int CursorTop { get; set; }

        /// <summary>
        /// Whether the cursor is visible.
        /// </summary>
        [DefaultValue(true)]
        public bool CursorVisible
        {
            get { return _cursorVisible; }
            set
            {
                _cursorVisible = value;
                _CursorHandler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Window height.
        /// </summary>
        [DefaultValue(DefaultWindowHeight)]
        public int WindowHeight
        {
            get { return _windowHeight; }
            set
            {
                if (value < MinimalWindowHeight
                    || value > MaximalWindowHeight)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _windowHeight = value;
                _SetupWindow();
            }
        }

        /// <summary>
        /// Window width.
        /// </summary>
        [DefaultValue(DefaultWindowWidth)]
        public int WindowWidth
        {
            get { return _windowWidth; }
            set
            {
                if (value < MinimalWindowWidth
                    || value > MaximalWindowWidth)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _windowWidth = value;
                _SetupWindow();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConsoleControl()
        {
            DoubleBuffered = true;

            BackColor = Color.Black;
            ForeColor = Color.LightGray;
            Font = new Font
                (
                    DefaultFontName,
                    DefaultFontSize,
                    FontStyle.Regular
                );

            WindowHeight = DefaultWindowHeight;
            WindowWidth = DefaultWindowWidth;
            _SetupWindow();

            _cursorTimer = new Timer
            {
                Enabled = true,
                Interval = 200
            };
            _cursorTimer.Tick += _CursorHandler;
            CursorHeight = DefaultCursorHeight;
            CursorVisible = true;

            EchoInput = true;
            _inputBuffer = new StringBuilder();
            _historyList = new List<string>();
            _historyPosition = 0;
        }

        #endregion

        #region Private members

        private Cell[] _cells;

        private int _cellWidth;
        private int _cellHeight;
        private int _windowWidth;
        private int _windowHeight;

        private int _cursorHeight;
        private bool _cursorVisible;
        private bool _cursorVisibleNow;

        private StringBuilder _inputBuffer;
        private int _inputRow;
        private int _inputColumn;

        private readonly Timer _cursorTimer;

        private Font _font;
        private Font _italicFont;

        private readonly List<string> _historyList;
        private int _historyPosition;

        private static Color[] _egaColors =
        {
            /* 0x00 */ Color.Black,
            /* 0x01 */ Color.Blue,
            /* 0x02 */ Color.Green,
            /* 0x03 */ Color.Cyan,
            /* 0x04 */ Color.Red,
            /* 0x05 */ Color.DarkMagenta,
            /* 0x06 */ Color.Brown,
            /* 0x07 */ Color.LightGray,
            /* 0x08 */ Color.DarkGray,
            /* 0x09 */ Color.LightSkyBlue,
            /* 0x0A */ Color.LightGreen,
            /* 0x0B */ Color.LightCyan,
            /* 0x0C */ Color.Orange,
            /* 0x0D */ Color.Magenta,
            /* 0x0E */ Color.Yellow,
            /* 0x0F */ Color.White
        };

        private void _AdvanceCursor()
        {
            CursorLeft++;
            if (CursorLeft >= WindowWidth)
            {
                CursorLeft = 0;
                CursorTop++;
            }
            if (CursorTop >= WindowHeight)
            {
                ScrollUp();
                //CursorTop--;
            }
        }

        private int _CellOffset
        (
            int row,
            int column
        )
        {
            int result = row * WindowWidth + column;

            return result;
        }

        private void _CursorHandler
            (
                object sender,
                EventArgs eventArgs
            )
        {
            if (!_cursorVisible
                || DesignMode)
            {
                return;
            }

            _cursorVisibleNow = !_cursorVisibleNow;

            using (Graphics graphics = Graphics.FromHwnd(Handle))
            {
                Rectangle rectangle = new Rectangle
                    (
                        _cellWidth * CursorLeft,
                        _cellHeight * CursorTop,
                        _cellWidth,
                        _cellHeight
                    );

                Cell cell = _cells[CursorTop * WindowWidth + CursorLeft];
                using (Brush foreBrush = new SolidBrush(cell.ForeColor))
                using (Brush backBrush = new SolidBrush(cell.BackColor))
                using (Brush cursorBrush = new SolidBrush(ForeColor))
                {
                    string s = new string(cell.Character, 1);
                    graphics.FillRectangle(backBrush, rectangle);
                    graphics.DrawString
                        (
                            s,
                            cell.Emphasized ? ItalicFont : Font,
                            foreBrush,
                            rectangle.Left - 2,
                            rectangle.Top
                        );

                    if (_cursorVisibleNow)
                    {
                        rectangle = new Rectangle
                            (
                                rectangle.Left,
                                rectangle.Top + _cellHeight - CursorHeight,
                                rectangle.Width,
                                CursorHeight
                            );
                        graphics.FillRectangle(cursorBrush, rectangle);
                        SystemSounds.Asterisk.Play();
                    }
                }
            }
        }

        private void _HandleEnter()
        {
            _HideCursorTemporary();
            WriteLine();

            string text = _inputBuffer.ToString();
            ConsoleInputEventArgs eventArgs
                = new ConsoleInputEventArgs
                {
                    Text = text
                };
            Input.Raise(this, eventArgs);

            AddHistoryEntry(text);

            _inputBuffer.Length = 0;
        }

        private void _HandleTab()
        {
            string text = _inputBuffer.ToString();
            ConsoleInputEventArgs eventArgs
                = new ConsoleInputEventArgs
                {
                    Text = text
                };
            TabPressed.Raise(this, eventArgs);
        }

        private void _HideCursorTemporary()
        {
            _cursorVisibleNow = true;
            _CursorHandler(this, EventArgs.Empty);
        }

        private int _InputPosition()
        {
            int result = (CursorTop - _inputRow) * WindowWidth
                         + CursorLeft - _inputColumn;

            return result;
        }

        private void _MoveInput(int delta)
        {
            int length = _inputBuffer.Length;
            int currentPosition = _InputPosition();
            int newPosition = currentPosition + delta;
            if (newPosition < 0)
            {
                newPosition = 0;
            }
            if (newPosition > length)
            {
                newPosition = length;
            }
            int newDelta = newPosition - currentPosition;
            MoveCursor(newDelta, 0);
        }

        private void _SetupCells()
        {
            if (WindowHeight == 0
                || WindowWidth == 0)
            {
                return;
            }

            int cellCount = WindowHeight * WindowWidth;
            _cells = new Cell[cellCount];
            Cell empty = new Cell
            {
                Character = ' ',
                ForeColor = ForeColor,
                BackColor = BackColor
            };
            for (int i = 0; i < cellCount; i++)
            {
                _cells[i] = empty;
            }

            Size clientSize = ClientSize;
            _cellHeight = clientSize.Height / WindowHeight;
            _cellWidth = clientSize.Width / WindowWidth;
        }

        private void _SetupFont()
        {
            _italicFont = new Font(_font, FontStyle.Italic);

            _SetupCells();
        }

        private void _SetupWindow()
        {
            _SetupCells();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add history entry.
        /// </summary>
        public void AddHistoryEntry
            (
                string text
            )
        {
            if (!string.IsNullOrEmpty(text)
                && !_historyList.Contains(text))
            {
                _historyList.Insert(0, text);
            }
            _historyPosition = 0;
        }

        /// <summary>
        /// Backspace.
        /// </summary>
        public bool Backspace()
        {
            int length = _inputBuffer.Length;

            if (length == 0)
            {
                return false;
            }

            _HideCursorTemporary();

            int position = _InputPosition();
            if (position != 0)
            {
                _inputBuffer.Remove(position - 1, 1);
            }

            string text = _inputBuffer + " ";
            Write(_inputRow, _inputColumn, text);

            MoveCursor(-1, 0);

            return true;
        }

        /// <summary>
        /// Clear the console.
        /// </summary>
        public void Clear()
        {
            CursorTop = 0;
            CursorLeft = 0;

            int total = WindowWidth * WindowHeight;

            Cell empty = new Cell
            {
                Character = ' ',
                ForeColor = ForeColor,
                BackColor = BackColor
            };
            for (int i = 0; i < total; i++)
            {
                _cells[i] = empty;
            }

            _inputBuffer.Length = 0;
            _inputColumn = 0;
            _inputRow = 0;

            Invalidate();
        }

        /// <summary>
        /// Clear from given position to end of the console.
        /// </summary>
        public void ClearFrom
            (
                int row,
                int column
            )
        {
            for (int x = column; x < WindowWidth; x++)
            {
                Write(row, x, ' ', ForeColor, BackColor, false);
            }
            for (int y = row + 1; y < WindowHeight; y++)
            {
                for (int x = 0; x < WindowWidth; x++)
                {
                    Write(y, x, ' ', ForeColor, BackColor, false);
                }
            }
        }

        /// <summary>
        /// Clear history list.
        /// </summary>
        public void ClearHistory()
        {
            _historyList.Clear();
            _historyPosition = 0;
        }

        /// <summary>
        /// Delete current character (DEL key).
        /// </summary>
        public bool DeleteCharacter()
        {
            int length = _inputBuffer.Length;

            if (length == 0)
            {
                return false;
            }

            _HideCursorTemporary();

            int position = _InputPosition();
            if (position < length)
            {
                _inputBuffer.Remove(position, 1);
            }

            string text = _inputBuffer + " ";
            Write(_inputRow, _inputColumn, text);

            return true;
        }

        /// <summary>
        /// Drop current input.
        /// </summary>
        public void DropInput()
        {
            if (_inputBuffer.Length == 0)
            {
                return;
            }

            _HideCursorTemporary();
            ClearFrom(_inputRow, _inputColumn);

            CursorLeft = _inputColumn;
            CursorTop = _inputRow;
            _inputBuffer.Length = 0;
        }

        /// <summary>
        /// Input one char.
        /// </summary>
        public bool InputChar
            (
                char c
            )
        {
            int screenSize = WindowWidth * WindowHeight;
            if (_inputBuffer.Length >= screenSize)
            {
                return false;
            }

            if (c >= ' ')
            {
                int position = _InputPosition();

                if (_inputBuffer.Length == 0)
                {
                    _inputColumn = CursorLeft;
                    _inputRow = CursorTop;
                }

                if (position >= _inputBuffer.Length)
                {
                    _inputBuffer.Append(c);
                }
                else
                {
                    _inputBuffer.Insert(position, c);
                }
                if (EchoInput)
                {
                    string text = _inputBuffer.ToString();
                    Write(_inputRow, _inputColumn, text);
                    _AdvanceCursor();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Move the cursor.
        /// </summary>
        public void MoveCursor
            (
                int deltaX,
                int deltaY
            )
        {
            _HideCursorTemporary();

            CursorLeft += deltaX;
            CursorTop += deltaY;

            while (CursorLeft < 0)
            {
                CursorTop--;
                CursorLeft += WindowWidth;
            }
            while (CursorLeft >= WindowWidth)
            {
                CursorTop++;
                CursorLeft -= WindowWidth;
            }
            if (CursorTop < 0)
            {
                CursorTop = 0;
            }
            if (CursorTop >= WindowHeight)
            {
                CursorTop = WindowHeight - 1;
            }
        }

        /// <summary>
        /// Read line from the console.
        /// </summary>
        public string? ReadLine()
        {
            bool done = false;
            string result = null;
            EventHandler<ConsoleInputEventArgs> inputHandler
                = (sender, args) =>
                {
                    result = args.Text;
                    done = true;
                };
            EventHandler disposeHandler = (sender, args) =>
            {
                done = true;
            };

            Input += inputHandler;
            Disposed += disposeHandler;

            ApplicationUtility.WaitFor(ref done);

            Input -= inputHandler;
            Disposed -= disposeHandler;

            return result;
        }

        /// <summary>
        /// Scroll up by one line.
        /// </summary>
        public void ScrollUp()
        {
            int total = WindowWidth * WindowHeight;
            int scrollingEnd = total - WindowWidth;

            for (int i = 0; i < scrollingEnd; i++)
            {
                _cells[i] = _cells[i + WindowWidth];
            }

            Cell empty = new Cell
            {
                Character = ' ',
                ForeColor = ForeColor,
                BackColor = BackColor
            };
            for (int i = scrollingEnd; i < total; i++)
            {
                _cells[i] = empty;
            }

            if (_inputRow != 0)
            {
                _inputRow--;
            }
            if (CursorTop != 0)
            {
                CursorTop--;
            }

            Invalidate();
        }

        /// <summary>
        /// Show current history entry.
        /// </summary>
        public void ShowHistoryEntry
            (
                bool advanceAfter
            )
        {
            int count = _historyList.Count;
            if (_historyPosition >= count)
            {
                DropInput();
                return;
            }

            string text = _historyList[_historyPosition];
            SetInput(text);

            if (advanceAfter)
            {
                if (_historyPosition < count)
                {
                    _historyPosition++;
                }
            }
        }

        /// <summary>
        /// Set input text.
        /// </summary>
        public void SetInput
            (
                string text
            )
        {
            if (_inputBuffer.Length == 0)
            {
                _inputRow = CursorTop;
                _inputColumn = CursorLeft;
            }
            DropInput();
            _inputBuffer.Append(text);
            Write(text);
        }

        /// <summary>
        /// WriteCharacter.
        /// </summary>
        public void Write
            (
                char c,
                Color foreColor,
                Color backColor,
                bool emphasize
            )
        {
            Cell cell = new Cell
            {
                Character = c,
                BackColor = backColor,
                ForeColor = foreColor,
                Emphasized = emphasize
            };

            _cells[CursorTop * WindowWidth + CursorLeft] = cell;
            _AdvanceCursor();
            Invalidate();
        }

        /// <summary>
        /// Write character.
        /// </summary>
        public void Write
            (
                int row,
                int column,
                char c,
                Color foreColor,
                Color backColor,
                bool emphasize
            )
        {
            if (
                row < 0
                || row >= WindowHeight
                || column < 0
                || column >= WindowWidth
                )
            {
                return;
            }

            Cell cell = new Cell
            {
                Character = c,
                BackColor = backColor,
                ForeColor = foreColor,
                Emphasized = emphasize
            };
            _cells[row * WindowWidth + column] = cell;
            Invalidate();
        }

        /// <summary>
        /// Write character.
        /// </summary>
        public void Write
            (
                int row,
                int column,
                char c
            )
        {
            if (
                row < 0
                || row >= WindowHeight
                || column < 0
                || column >= WindowWidth
                )
            {
                return;
            }

            int offset = row * WindowWidth + column;

            Cell previousCell = _cells[offset];

            Cell newCell = new Cell
            {
                Character = c,
                BackColor = previousCell.BackColor,
                ForeColor = previousCell.ForeColor,
                Emphasized = previousCell.Emphasized
            };
            _cells[offset] = newCell;
            Invalidate();
        }

        /// <summary>
        /// Write text.
        /// </summary>
        public void Write
            (
                Color foreColor,
                Color backColor,
                bool emphasize,
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            Color saveForeColor = foreColor;
            Color saveBackColor = backColor;
            int mode = 0;

            foreach (char c in text)
            {
                if (mode == 1)
                {
                    foreColor = _egaColors[c % 16];
                    mode = 0;
                    continue;
                }
                if (mode == 2)
                {
                    backColor = _egaColors[c % 16];
                    mode = 0;
                    continue;
                }

                switch (c)
                {
                    case '\x1':
                        mode = 1;
                        break;

                    case '\x2':
                        mode = 2;
                        break;

                    case '\x3':
                        foreColor = saveForeColor;
                        break;

                    case '\x4':
                        backColor = saveBackColor;
                        break;

                    case '\b':
                        MoveCursor(-1, 0);
                        Write(' ', foreColor, backColor, emphasize);
                        MoveCursor(-1, 0);
                        break;

                    case '\f':
                        Clear();
                        break;

                    case '\t':
                        WriteTab(backColor);
                        break;

                    case '\r':
                        CursorLeft = 0;
                        break;

                    case '\n':
                        WriteLine();
                        break;

                    default:
                        Write(c, foreColor, backColor, emphasize);
                        break;
                }
            }
        }

        /// <summary>
        /// Write text.
        /// </summary>
        public void Write
            (
                bool emphasize,
                string? text
            )
        {
            Write(ForeColor, BackColor, emphasize, text);
        }

        /// <summary>
        /// Write text.
        /// </summary>
        public void Write
            (
                Color foreColor,
                string? text
            )
        {
            Write(foreColor, BackColor, false, text);
        }

        /// <summary>
        /// Write text.
        /// </summary>
        public void Write
            (
                string? text
            )
        {
            Write(ForeColor, BackColor, false, text);
        }

        /// <summary>
        /// Write text.
        /// </summary>
        public void Write
            (
                int row,
                int column,
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            foreach (char c in text)
            {
                Write(row, column, c, ForeColor, BackColor, false);
                column++;
                if (column >= WindowWidth)
                {
                    column = 0;
                    row++;
                    if (row >= WindowHeight)
                    {
                        ScrollUp();
                        row--;
                    }
                }
            }
        }

        /// <summary>
        /// Move cursor to beginning of next line.
        /// </summary>
        public void WriteLine()
        {
            CursorLeft = 0;
            CursorTop++;
            if (CursorTop >= WindowHeight)
            {
                ScrollUp();
            }
        }

        /// <summary>
        /// Write text and move cursor to beginning of next line.
        /// </summary>
        public void WriteLine
            (
                string? text
            )
        {
            Write(text);
            WriteLine();
        }

        /// <summary>
        /// Write text and move cursor to beginning of next line.
        /// </summary>
        public void WriteLine
            (
                bool emphasize,
                string? text
            )
        {
            Write(emphasize, text);
            WriteLine();
        }

        /// <summary>
        /// Write text and move cursor to beginning of next line.
        /// </summary>
        public void WriteLine
            (
                Color foreColor,
                string? text
            )
        {
            Write(foreColor, text);
            WriteLine();
        }

        /// <summary>
        /// Tabulation.
        /// </summary>
        public void WriteTab
            (
                Color backColor
            )
        {
            int count = CursorLeft % 8;
            if (count == 0)
            {
                count = 8;
            }
            for (int i = 0; i < count; i++)
            {
                Write(' ', ForeColor, backColor, false);
            }
        }

        #endregion

        #region Control members

        /// <inheritdoc />
        protected override Size DefaultSize
        {
            get { return new Size(640, 375); }
        }

        /// <inheritdoc />
        protected override void Dispose
            (
                bool disposing
            )
        {
            if (!ReferenceEquals(_cursorTimer, null))
            {
                _cursorTimer.Dispose();
            }

            if (!ReferenceEquals(_italicFont, null))
            {
                _italicFont.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <inheritdoc />
        public override Font Font
        {
            get => _font;
            set
            {
                _font = value;
                _SetupFont();
            }
        }

        /// <inheritdoc />
        protected override void OnClientSizeChanged
        (
            EventArgs e
        )
        {
            base.OnClientSizeChanged(e);

            _SetupCells();
        }

        /// <inheritdoc />
        protected override void OnKeyPress
            (
                KeyPressEventArgs e
            )
        {
            base.OnKeyPress(e);

            if (!AllowInput)
            {
                return;
            }

            char c = e.KeyChar;
            e.Handled = InputChar(c);
        }

        /// <inheritdoc />
        protected override void OnPaint
            (
                PaintEventArgs paintEvent
            )
        {
            Graphics graphics = paintEvent.Graphics;

            int cellOffset = 0;
            int rowOffset = 0;

            Color backColor = BackColor;
            Brush backBrush = new SolidBrush(backColor);
            Color foreColor = ForeColor;
            Brush foreBrush = new SolidBrush(foreColor);

            try
            {
                for (int row = 0; row < WindowHeight; row++)
                {
                    int columnOffset = 0;

                    for (int column = 0; column < WindowWidth; column++)
                    {
                        Cell cell = _cells[cellOffset];

                        if (cell.BackColor != backColor)
                        {
                            backBrush.Dispose();
                            backColor = cell.BackColor;
                            backBrush = new SolidBrush(backColor);
                        }
                        if (cell.ForeColor != foreColor)
                        {
                            foreBrush.Dispose();
                            foreColor = cell.ForeColor;
                            foreBrush = new SolidBrush(foreColor);
                        }

                        Rectangle rectangle = new Rectangle
                        (
                            columnOffset,
                            rowOffset,
                            _cellWidth,
                            _cellHeight
                        );

                        graphics.FillRectangle(backBrush, rectangle);
                        string s = new string(cell.Character, 1);
                        graphics.DrawString
                        (
                            s,
                            cell.Emphasized ? ItalicFont : Font,
                            foreBrush,
                            columnOffset - 2,
                            rowOffset
                        );

                        cellOffset++;
                        columnOffset += _cellWidth;
                    }

                    rowOffset += _cellHeight;
                }
            }
            finally
            {
                backBrush.Dispose();
                foreBrush.Dispose();
            }

            base.OnPaint(paintEvent);
        }

        /// <inheritdoc />
        protected override void OnPreviewKeyDown
        (
            PreviewKeyDownEventArgs e
        )
        {
            base.OnPreviewKeyDown(e);

            if (!AllowInput)
            {
                return;
            }

            switch (e.KeyData)
            {
                case Keys.Tab:
                    e.IsInputKey = true;
                    _HandleTab();
                    return;

                case Keys.Delete:
                    e.IsInputKey = true;
                    DeleteCharacter();
                    return;

                case Keys.Back:
                    e.IsInputKey = true;
                    Backspace();
                    return;

                case Keys.Escape:
                    e.IsInputKey = true;
                    DropInput();
                    return;

                case Keys.Enter:
                    e.IsInputKey = true;
                    _HandleEnter();
                    return;

                case Keys.Left:
                    e.IsInputKey = true;
                    _MoveInput(-1);
                    return;

                case Keys.Right:
                    e.IsInputKey = true;
                    _MoveInput(1);
                    return;

                case Keys.Up:
                    e.IsInputKey = true;
                    ShowHistoryEntry(true);
                    return;

                case Keys.Down:
                    e.IsInputKey = true;
                    if (_historyPosition != 0)
                    {
                        _historyPosition--;
                        ShowHistoryEntry(false);
                    }
                    else
                    {
                        DropInput();
                    }
                    return;
            }
        }

        /// <inheritdoc />
        protected override void OnPaintBackground
            (
                PaintEventArgs paintEvent
            )
        {
            // Do nothing
        }

        #endregion
    }
}
