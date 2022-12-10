// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable VirtualMemberCallInConstructor

/* SyntaxTextBox.cs -- контрол для отображения текста с подсветкой синтаксиса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using Microsoft.Win32;

using Timer = System.Windows.Forms.Timer;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Контрол для отображения текста с подсветкой синтаксиса.
/// </summary>
public class SyntaxTextBox
    : UserControl,
    ISupportInitialize
{
    #region Constants

    internal const int minLeftIndent = 8;
    private const int maxBracketSearchIterations = 1000;
    private const int maxLinesForFolding = 3000;
    private const int minLinesForAccuracy = 100000;
    private const int WM_IME_SETCONTEXT = 0x0281;
    private const int WM_HSCROLL = 0x114;
    private const int WM_VSCROLL = 0x115;
    private const int SB_ENDSCROLL = 0x8;

    #endregion

    #region Properties

    /// <summary>
    ///
    /// </summary>
    public readonly List<LineInfo> LineInfos = new();

    /// <summary>
    ///
    /// </summary>
    public int TextHeight;

    /// <summary>
    ///
    /// </summary>
    public bool AllowInsertRemoveLines = true;

    /// <summary>
    ///
    /// </summary>
    public char[] AutoCompleteBracketsList
    {
        get => _autoCompleteBracketsList;
        set => _autoCompleteBracketsList = value;
    }

    /// <summary>
    /// AutoComplete brackets
    /// </summary>
    [DefaultValue (false)]
    [Description ("AutoComplete brackets.")]
    public bool AutoCompleteBrackets { get; set; }

    /// <summary>
    /// Colors of some service visual markers
    /// </summary>
    [Browsable (true)]
    [Description ("Colors of some service visual markers.")]
    [TypeConverter (typeof (ExpandableObjectConverter))]
    public ServiceColors ServiceColors { get; set; }

    /// <summary>
    /// Contains UniqueId of start lines of folded blocks
    /// </summary>
    /// <remarks>This dictionary remembers folding state of blocks.
    /// It is needed to restore child folding after user collapsed/expanded top-level folding block.</remarks>
    [Browsable (false)]
    public Dictionary<int, int> FoldedBlocks { get; }

    /// <summary>
    /// Strategy of search of brackets to highlighting
    /// </summary>
    [DefaultValue (typeof (BracketsHighlightStrategy), "Strategy1")]
    [Description ("Strategy of search of brackets to highlighting.")]
    public BracketsHighlightStrategy BracketsHighlightStrategy { get; set; }



    #endregion

    #region Construction

    /// <summary>
    /// Constructor
    /// </summary>
    public SyntaxTextBox()
    {
        //register type provider
        var prov = TypeDescriptor.GetProvider (GetType());
        var theProvider =
            prov.GetType().GetField ("Provider", BindingFlags.NonPublic | BindingFlags.Instance).GetValue (prov);
        if (theProvider.GetType() != typeof (FCTBDescriptionProvider))
        {
            TypeDescriptor.AddProvider (new FCTBDescriptionProvider (GetType()), GetType());
        }

        //drawing optimization
        SetStyle
            (
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw,
                true
            );

        //append monospace font
        Font = new Font (FontFamily.GenericMonospace, 9.75f);

        //create one line
        InitTextSource (CreateTextSource());
        if (lines.Count == 0)
        {
            lines.InsertLine (0, lines.CreateLine());
        }

        _selection = new TextRange (this) { Start = new Place (0, 0) };

        //default settings
        Cursor = Cursors.IBeam;
        BackColor = Color.White;
        LineNumberColor = Color.Teal;
        IndentBackColor = Color.WhiteSmoke;
        ServiceLinesColor = Color.Silver;
        FoldingIndicatorColor = Color.Green;
        CurrentLineColor = Color.Transparent;
        ChangedLineColor = Color.Transparent;
        HighlightFoldingIndicator = true;
        ShowLineNumbers = true;
        TabLength = 4;
        FoldedBlockStyle = new FoldedBlockStyle (Brushes.Gray, null, FontStyle.Regular);
        SelectionColor = Color.Blue;
        BracketsStyle = new MarkerStyle (new SolidBrush (Color.FromArgb (80, Color.Lime)));
        BracketsStyle2 = new MarkerStyle (new SolidBrush (Color.FromArgb (60, Color.Red)));
        DelayedEventsInterval = 100;
        DelayedTextChangedInterval = 100;
        AllowSeveralTextStyleDrawing = false;
        LeftBracket = '\x0';
        RightBracket = '\x0';
        LeftBracket2 = '\x0';
        RightBracket2 = '\x0';
        SyntaxHighlighter = new SyntaxHighlighter (this);
        language = Language.Custom;
        PreferredLineWidth = 0;
        needRecalc = true;
        lastNavigatedDateTime = DateTime.Now;
        AutoIndent = true;
        AutoIndentExistingLines = true;
        CommentPrefix = "//";
        lineNumberStartValue = 1;
        multiline = true;
        _scrollBars = true;
        AcceptsTab = true;
        AcceptsReturn = true;
        _caretVisible = true;
        CaretColor = Color.Black;
        WideCaret = false;
        Paddings = new Padding (0, 0, 0, 0);
        PaddingBackColor = Color.Transparent;
        DisabledColor = Color.FromArgb (100, 180, 180, 180);
        needRecalcFoldingLines = true;
        AllowDrop = true;
        FindEndOfFoldingBlockStrategy = FindEndOfFoldingBlockStrategy.Strategy1;
        VirtualSpace = false;
        bookmarks = new Bookmarks (this);
        BookmarkColor = Color.PowderBlue;
        ToolTip = new ToolTip();
        _timer3.Interval = 500;
        hints = new Hints (this);
        SelectionHighlightingForLineBreaksEnabled = true;
        textAreaBorder = TextAreaBorderType.None;
        textAreaBorderColor = Color.Black;
        _macroManager = new MacroManager (this);
        HotkeyMapping = new HotkeyMapping();
        HotkeyMapping.InitDefault();
        WordWrapAutoIndent = true;
        FoldedBlocks = new Dictionary<int, int>();
        AutoCompleteBrackets = false;
        AutoIndentCharsPatterns = @"^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>[^;=]+);
^\s*(case|default)\s*[^:]*(?<range>:)\s*(?<range>[^;]+);";
        AutoIndentChars = true;
        CaretBlinking = true;
        ServiceColors = new ServiceColors();

        //
        base.AutoScroll = true;
        _timer1.Tick += timer_Tick;
        _timer2.Tick += timer2_Tick;
        _timer3.Tick += timer3_Tick;
        middleClickScrollingTimer.Tick += middleClickScrollingTimer_Tick;
    }

    #endregion

    #region Private members

    private readonly TextRange _selection;

    private readonly Timer _timer1 = new();

    private readonly Timer _timer2 = new();

    private readonly Timer _timer3 = new();

    private readonly List<VisualMarker> visibleMarkers = new ();

    private Brush? backBrush;

    private BookmarksBase bookmarks;

    private bool _caretVisible;

    private Color _changedLineColor;

    private int _charHeight;

    private Color currentLineColor;

    private Cursor defaultCursor;

    private TextRange? _delayedTextChangedRange;

    private string _descriptionFile;

    private int _endFoldingLine = -1;

    private Color _foldingIndicatorColor;

    /// <summary>
    ///
    /// </summary>
    protected Dictionary<int, int> foldingPairs = new ();

    private bool _handledChar;

    private bool highlightFoldingIndicator;

    private Hints hints;

    private Color indentBackColor;

    private bool isChanged;

    private bool isLineSelect;

    private bool isReplaceMode;

    private Language language;

    private Keys lastModifiers;

    private Point lastMouseCoord;

    private DateTime lastNavigatedDateTime;

    private TextRange leftBracketPosition;

    private TextRange leftBracketPosition2;

    private int leftPadding;

    private int lineInterval;

    private Color lineNumberColor;

    private uint lineNumberStartValue;

    private LineNumberFormatting lineNumberFormatting;

    private int lineSelectFrom;

    private TextSource lines;

    private IntPtr m_hImc;

    private int maxLineLength;

    private bool mouseIsDrag;

    private bool mouseIsDragDrop;

    private bool multiline;

    /// <summary>
    ///
    /// </summary>
    protected bool needRecalc;

    /// <summary>
    ///
    /// </summary>
    protected bool needRecalcWordWrap;

    private Point needRecalcWordWrapInterval;

    private bool needRecalcFoldingLines;

    private bool needRiseSelectionChangedDelayed;

    private bool needRiseTextChangedDelayed;

    private bool needRiseVisibleRangeChangedDelayed;

    private Color paddingBackColor;

    private int preferredLineWidth;

    private TextRange rightBracketPosition;

    private TextRange rightBracketPosition2;

    private bool _scrollBars;

    private Color selectionColor;

    private Color serviceLinesColor;

    private bool _showFoldingLines;

    private bool _showLineNumbers;

    private SyntaxTextBox? sourceTextBox;

    private int _startFoldingLine = -1;

    private int _updating;

    private TextRange? _updatingRange;

    private TextRange? _visibleRange;

    private bool _wordWrap;

    private WordWrapMode wordWrapMode = WordWrapMode.WordWrapControlWidth;

    private int reservedCountOfLineNumberChars = 1;

    private int _zoom = 100;

    private Size localAutoScrollMinSize;

    private char[] _autoCompleteBracketsList = { '(', ')', '{', '}', '[', ']', '"', '"', '\'', '\'' };

    #endregion

    #region Public methods

    #endregion

    /// <summary>
    /// Automatically shifts secondary wordwrap lines on the shift amount of the first line
    /// </summary>
    [DefaultValue (true)]
    [Description ("Automatically shifts secondary wordwrap lines on the shift amount of the first line.")]
    public bool WordWrapAutoIndent { get; set; }

    /// <summary>
    /// Indent of secondary wordwrap lines (in chars)
    /// </summary>
    [DefaultValue (0)]
    [Description ("Indent of secondary wordwrap lines (in chars).")]
    public int WordWrapIndent { get; set; }

    private readonly MacroManager _macroManager;

    /// <summary>
    /// MacrosManager records, stores and executes the macroses
    /// </summary>
    [Browsable (false)]
    public MacroManager MacroManager => _macroManager;

    /// <summary>
    /// Allows drag and drop
    /// </summary>
    [DefaultValue (true)]
    [Description ("Allows drag and drop")]
    public override bool AllowDrop
    {
        get => base.AllowDrop;
        set => base.AllowDrop = value;
    }

    /// <summary>
    /// Collection of Hints.
    /// This is temporary buffer for currently displayed hints.
    /// </summary>
    /// <remarks>You can asynchronously add, remove and clear hints. Appropriate hints will be shown or hidden from the screen.</remarks>
    [Browsable (false), DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden),
     EditorBrowsable (EditorBrowsableState.Never)]
    public Hints Hints
    {
        get => hints;
        set => hints = value;
    }

    /// <summary>
    /// Delay (ms) of ToolTip
    /// </summary>
    [Browsable (true)]
    [DefaultValue (500)]
    [Description ("Delay(ms) of ToolTip.")]
    public int ToolTipDelay
    {
        get => _timer3.Interval;
        set => _timer3.Interval = value;
    }

    /// <summary>
    /// ToolTip component
    /// </summary>
    [Browsable (true)]
    [Description ("ToolTip component.")]
    public ToolTip? ToolTip { get; set; }

    /// <summary>
    /// Color of bookmarks
    /// </summary>
    [Browsable (true)]
    [DefaultValue (typeof (Color), "PowderBlue")]
    [Description ("Color of bookmarks.")]
    public Color BookmarkColor { get; set; }

    /// <summary>
    /// Bookmarks
    /// </summary>
    [Browsable (false), DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden),
     EditorBrowsable (EditorBrowsableState.Never)]
    public BookmarksBase Bookmarks
    {
        get => bookmarks;
        set => bookmarks = value;
    }

    /// <summary>
    /// Enables virtual spaces
    /// </summary>
    [DefaultValue (false)]
    [Description ("Enables virtual spaces.")]
    public bool VirtualSpace { get; set; }

    /// <summary>
    /// Strategy of search of end of folding block
    /// </summary>
    [DefaultValue (FindEndOfFoldingBlockStrategy.Strategy1)]
    [Description ("Strategy of search of end of folding block.")]
    public FindEndOfFoldingBlockStrategy FindEndOfFoldingBlockStrategy { get; set; }

    /// <summary>
    /// Indicates if tab characters are accepted as input
    /// </summary>
    [DefaultValue (true)]
    [Description ("Indicates if tab characters are accepted as input.")]
    public bool AcceptsTab { get; set; }

    /// <summary>
    /// Indicates if return characters are accepted as input
    /// </summary>
    [DefaultValue (true)]
    [Description ("Indicates if return characters are accepted as input.")]
    public bool AcceptsReturn { get; set; }

    /// <summary>
    /// Shows or hides the caret
    /// </summary>
    [DefaultValue (true)]
    [Description ("Shows or hides the caret")]
    public bool CaretVisible
    {
        get => _caretVisible;
        set
        {
            _caretVisible = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Enables caret blinking
    /// </summary>
    [DefaultValue (true)]
    [Description ("Enables caret blinking")]
    public bool CaretBlinking { get; set; }

    /// <summary>
    /// Draw caret when the control is not focused
    /// </summary>
    [DefaultValue (false)]
    public bool ShowCaretWhenInactive { get; set; }


    private Color textAreaBorderColor;

    /// <summary>
    /// Color of border of text area
    /// </summary>
    [DefaultValue (typeof (Color), "Black")]
    [Description ("Color of border of text area")]
    public Color TextAreaBorderColor
    {
        get => textAreaBorderColor;
        set
        {
            textAreaBorderColor = value;
            Invalidate();
        }
    }

    private TextAreaBorderType textAreaBorder;

    /// <summary>
    /// Type of border of text area
    /// </summary>
    [DefaultValue (typeof (TextAreaBorderType), "None")]
    [Description ("Type of border of text area")]
    public TextAreaBorderType TextAreaBorder
    {
        get => textAreaBorder;
        set
        {
            textAreaBorder = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Background color for current line
    /// </summary>
    [DefaultValue (typeof (Color), "Transparent")]
    [Description ("Background color for current line. Set to Color.Transparent to hide current line highlighting")]
    public Color CurrentLineColor
    {
        get => currentLineColor;
        set
        {
            currentLineColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Background color for highlighting of changed lines
    /// </summary>
    [DefaultValue (typeof (Color), "Transparent")]
    [Description (
        "Background color for highlighting of changed lines. Set to Color.Transparent to hide changed line highlighting")]
    public Color ChangedLineColor
    {
        get => _changedLineColor;
        set
        {
            _changedLineColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Fore color (default style color)
    /// </summary>
    public override Color ForeColor
    {
        get => base.ForeColor;
        set
        {
            base.ForeColor = value;
            lines.InitDefaultStyle();
            Invalidate();
        }
    }

    /// <summary>
    /// Height of char in pixels (includes LineInterval)
    /// </summary>
    [Browsable (false)]
    public int CharHeight
    {
        get => _charHeight;
        set
        {
            _charHeight = value;
            NeedRecalc();
            OnCharSizeChanged();
        }
    }

    /// <summary>
    /// Interval between lines (in pixels)
    /// </summary>
    [Description ("Interval between lines in pixels")]
    [DefaultValue (0)]
    public int LineInterval
    {
        get => lineInterval;
        set
        {
            lineInterval = value;
            SetFont (Font);
            Invalidate();
        }
    }

    /// <summary>
    /// Width of char in pixels
    /// </summary>
    [Browsable (false)]
    public int CharWidth { get; set; }

    /// <summary>
    /// Spaces count for tab
    /// </summary>
    [DefaultValue (4)]
    [Description ("Spaces count for tab")]
    public int TabLength { get; set; }

    /// <summary>
    /// Text was changed
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public bool IsChanged
    {
        get => isChanged;
        set
        {
            if (!value)

                //clear line's IsChanged property
            {
                lines.ClearIsChanged();
            }

            isChanged = value;
        }
    }

    /// <summary>
    /// Text version
    /// </summary>
    /// <remarks>This counter is incremented each time changes the text</remarks>
    [Browsable (false)]
    public int TextVersion { get; private set; }

    /// <summary>
    /// Read only
    /// </summary>
    [DefaultValue (false)]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Shows line numbers.
    /// </summary>
    [DefaultValue (true)]
    [Description ("Shows line numbers.")]
    public bool ShowLineNumbers
    {
        get => _showLineNumbers;
        set
        {
            _showLineNumbers = value;
            NeedRecalc();
            Invalidate();
        }
    }

    /// <summary>
    /// Shows vertical lines between folding start line and folding end line.
    /// </summary>
    [DefaultValue (false)]
    [Description ("Shows vertical lines between folding start line and folding end line.")]
    public bool ShowFoldingLines
    {
        get => _showFoldingLines;
        set
        {
            _showFoldingLines = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Rectangle where located text
    /// </summary>
    [Browsable (false)]
    public Rectangle TextAreaRect
    {
        get
        {
            var rightPaddingStartX = LeftIndent + maxLineLength * CharWidth + Paddings.Left + 1;
            rightPaddingStartX = Math.Max (ClientSize.Width - Paddings.Right, rightPaddingStartX);
            var bottomPaddingStartY = TextHeight + Paddings.Top;
            bottomPaddingStartY = Math.Max (ClientSize.Height - Paddings.Bottom, bottomPaddingStartY);
            var top = Math.Max (0, Paddings.Top - 1) - VerticalScroll.Value;
            var left = LeftIndent - HorizontalScroll.Value - 2 + Math.Max (0, Paddings.Left - 1);
            var rect = Rectangle.FromLTRB (left, top, rightPaddingStartX - HorizontalScroll.Value,
                bottomPaddingStartY - VerticalScroll.Value);
            return rect;
        }
    }

    /// <summary>
    /// Color of line numbers.
    /// </summary>
    [DefaultValue (typeof (Color), "Teal")]
    [Description ("Color of line numbers.")]
    public Color LineNumberColor
    {
        get => lineNumberColor;
        set
        {
            lineNumberColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Start value of first line number.
    /// </summary>
    [DefaultValue (typeof (uint), "1")]
    [Description ("Start value of first line number.")]
    public uint LineNumberStartValue
    {
        get => lineNumberStartValue;
        set
        {
            lineNumberStartValue = value;
            needRecalc = true;
            Invalidate();
        }
    }

    /// <summary>
    /// To create your own line number formatting, you have to implement the abstract LineNumberFormatting class
    /// </summary>
    [DefaultValue (typeof (LineNumberFormatting), null)]
    [Description ("Format of string displayed when ShowLineNumbers = true")]
    public LineNumberFormatting LineNumberFormatting
    {
        get => lineNumberFormatting;
        set
        {
            lineNumberFormatting = value;
            needRecalc = true;
            Invalidate();
        }
    }

    /// <summary>
    /// Background color of indent area
    /// </summary>
    [DefaultValue (typeof (Color), "WhiteSmoke")]
    [Description ("Background color of indent area")]
    public Color IndentBackColor
    {
        get => indentBackColor;
        set
        {
            indentBackColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Background color of padding area
    /// </summary>
    [DefaultValue (typeof (Color), "Transparent")]
    [Description ("Background color of padding area")]
    public Color PaddingBackColor
    {
        get => paddingBackColor;
        set
        {
            paddingBackColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Color of disabled component
    /// </summary>
    [DefaultValue (typeof (Color), "100;180;180;180")]
    [Description ("Color of disabled component")]
    public Color DisabledColor { get; set; }

    /// <summary>
    /// Color of caret
    /// </summary>
    [DefaultValue (typeof (Color), "Black")]
    [Description ("Color of caret.")]
    public Color CaretColor { get; set; }

    /// <summary>
    /// Wide caret
    /// </summary>
    [DefaultValue (false)]
    [Description ("Wide caret.")]
    public bool WideCaret { get; set; }

    /// <summary>
    /// Color of service lines (folding lines, borders of blocks etc.)
    /// </summary>
    [DefaultValue (typeof (Color), "Silver")]
    [Description ("Color of service lines (folding lines, borders of blocks etc.)")]
    public Color ServiceLinesColor
    {
        get => serviceLinesColor;
        set
        {
            serviceLinesColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Padings of text area
    /// </summary>
    [Browsable (true)]
    [Description ("Paddings of text area.")]
    public Padding Paddings { get; set; }

    /// <summary>
    /// --Do not use this property--
    /// </summary>
    [Browsable (false), DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden),
     EditorBrowsable (EditorBrowsableState.Never)]
    public new Padding Padding
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// hide RTL
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    [Browsable (false), DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden),
     EditorBrowsable (EditorBrowsableState.Never)]
    public new bool RightToLeft
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Color of folding area indicator
    /// </summary>
    [DefaultValue (typeof (Color), "Green")]
    [Description ("Color of folding area indicator.")]
    public Color FoldingIndicatorColor
    {
        get => _foldingIndicatorColor;
        set
        {
            _foldingIndicatorColor = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Enables folding indicator (left vertical line between folding bounds)
    /// </summary>
    [DefaultValue (true)]
    [Description ("Enables folding indicator (left vertical line between folding bounds)")]
    public bool HighlightFoldingIndicator
    {
        get => highlightFoldingIndicator;
        set
        {
            highlightFoldingIndicator = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Left distance to text beginning
    /// </summary>
    [Browsable (false)]
    [Description ("Left distance to text beginning.")]
    public int LeftIndent { get; private set; }

    /// <summary>
    /// Left padding in pixels
    /// </summary>
    [DefaultValue (0)]
    [Description ("Width of left service area (in pixels)")]
    public int LeftPadding
    {
        get => leftPadding;
        set
        {
            leftPadding = value;
            Invalidate();
        }
    }

    /// <summary>
    /// This property draws vertical line after defined char position.
    /// Set to 0 for disable drawing of vertical line.
    /// </summary>
    [DefaultValue (0)]
    [Description (
        "This property draws vertical line after defined char position. Set to 0 for disable drawing of vertical line.")]
    public int PreferredLineWidth
    {
        get => preferredLineWidth;
        set
        {
            preferredLineWidth = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Styles
    /// </summary>
    [Browsable (false)]
    public Style[] Styles => lines.Styles;

    /// <summary>
    /// Hotkeys. Do not use this property in your code, use HotkeysMapping property.
    /// </summary>
    [Description ("Here you can change hotkeys for FastColoredTextBox.")]
    [Editor (typeof (HotkeyEditor), typeof (UITypeEditor))]
    [DefaultValue ("Tab=IndentIncrease, Escape=ClearHints, PgUp=GoPageUp, PgDn=GoPageDown, End=GoEnd, Home=GoHome, Left=GoLeft, Up=GoUp, Right=GoRight, Down=GoDown, Ins=ReplaceMode, Del=DeleteCharRight, F3=FindNext, Shift+Tab=IndentDecrease, Shift+PgUp=GoPageUpWithSelection, Shift+PgDn=GoPageDownWithSelection, Shift+End=GoEndWithSelection, Shift+Home=GoHomeWithSelection, Shift+Left=GoLeftWithSelection, Shift+Up=GoUpWithSelection, Shift+Right=GoRightWithSelection, Shift+Down=GoDownWithSelection, Shift+Ins=Paste, Shift+Del=Cut, Ctrl+Back=ClearWordLeft, Ctrl+Space=AutocompleteMenu, Ctrl+End=GoLastLine, Ctrl+Home=GoFirstLine, Ctrl+Left=GoWordLeft, Ctrl+Up=ScrollUp, Ctrl+Right=GoWordRight, Ctrl+Down=ScrollDown, Ctrl+Ins=Copy, Ctrl+Del=ClearWordRight, Ctrl+0=ZoomNormal, Ctrl+A=SelectAll, Ctrl+B=BookmarkLine, Ctrl+C=Copy, Ctrl+E=MacroExecute, Ctrl+F=FindDialog, Ctrl+G=GoToDialog, Ctrl+H=ReplaceDialog, Ctrl+I=AutoIndentChars, Ctrl+M=MacroRecord, Ctrl+N=GoNextBookmark, Ctrl+R=Redo, Ctrl+U=UpperCase, Ctrl+V=Paste, Ctrl+X=Cut, Ctrl+Z=Undo, Ctrl+Add=ZoomIn, Ctrl+Subtract=ZoomOut, Ctrl+OemMinus=NavigateBackward, Ctrl+Shift+End=GoLastLineWithSelection, Ctrl+Shift+Home=GoFirstLineWithSelection, Ctrl+Shift+Left=GoWordLeftWithSelection, Ctrl+Shift+Right=GoWordRightWithSelection, Ctrl+Shift+B=UnbookmarkLine, Ctrl+Shift+C=CommentSelected, Ctrl+Shift+N=GoPrevBookmark, Ctrl+Shift+U=LowerCase, Ctrl+Shift+OemMinus=NavigateForward, Alt+Back=Undo, Alt+Up=MoveSelectedLinesUp, Alt+Down=MoveSelectedLinesDown, Alt+F=FindChar, Alt+Shift+Left=GoLeft_ColumnSelectionMode, Alt+Shift+Up=GoUp_ColumnSelectionMode, Alt+Shift+Right=GoRight_ColumnSelectionMode, Alt+Shift+Down=GoDown_ColumnSelectionMode")]
    public string Hotkeys
    {
        get => HotkeyMapping.ToString();
        set => HotkeyMapping = HotkeyMapping.Parse (value);
    }

    /// <summary>
    /// Hotkeys mapping
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public HotkeyMapping HotkeyMapping { get; set; }

    /// <summary>
    /// Default text style
    /// This style is using when no one other TextStyle is not defined in Char.style
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public TextStyle DefaultStyle
    {
        get => lines.DefaultStyle;
        set => lines.DefaultStyle = value;
    }

    /// <summary>
    /// Style for rendering Selection area
    /// </summary>
    [Browsable (false)]
    public SelectionStyle SelectionStyle { get; set; }

    /// <summary>
    /// Style for folded block rendering
    /// </summary>
    [Browsable (false)]
    public TextStyle FoldedBlockStyle { get; set; }

    /// <summary>
    /// Style for brackets highlighting
    /// </summary>
    [Browsable (false)]
    public MarkerStyle? BracketsStyle { get; set; }

    /// <summary>
    /// Style for alternative brackets highlighting
    /// </summary>
    [Browsable (false)]
    public MarkerStyle? BracketsStyle2 { get; set; }

    /// <summary>
    /// Opening bracket for brackets highlighting.
    /// Set to '\x0' for disable brackets highlighting.
    /// </summary>
    [DefaultValue ('\x0')]
    [Description ("Opening bracket for brackets highlighting. Set to '\\x0' for disable brackets highlighting.")]
    public char LeftBracket { get; set; }

    /// <summary>
    /// Closing bracket for brackets highlighting.
    /// Set to '\x0' for disable brackets highlighting.
    /// </summary>
    [DefaultValue ('\x0')]
    [Description ("Closing bracket for brackets highlighting. Set to '\\x0' for disable brackets highlighting.")]
    public char RightBracket { get; set; }

    /// <summary>
    /// Alternative opening bracket for brackets highlighting.
    /// Set to '\x0' for disable brackets highlighting.
    /// </summary>
    [DefaultValue ('\x0')]
    [Description (
        "Alternative opening bracket for brackets highlighting. Set to '\\x0' for disable brackets highlighting.")]
    public char LeftBracket2 { get; set; }

    /// <summary>
    /// Alternative closing bracket for brackets highlighting.
    /// Set to '\x0' for disable brackets highlighting.
    /// </summary>
    [DefaultValue ('\x0')]
    [Description (
        "Alternative closing bracket for brackets highlighting. Set to '\\x0' for disable brackets highlighting.")]
    public char RightBracket2 { get; set; }

    /// <summary>
    /// Comment line prefix.
    /// </summary>
    [DefaultValue ("//")]
    [Description ("Comment line prefix.")]
    public string CommentPrefix { get; set; }

    /// <summary>
    /// This property specifies which part of the text will be highlighted as you type (by built-in highlighter).
    /// </summary>
    /// <remarks>When a user enters text, a component refreshes highlighting (because the text was changed).
    /// This property specifies exactly which section of the text will be re-highlighted.
    /// This can be useful to highlight multi-line comments, for example.</remarks>
    [DefaultValue (typeof (HighlightingRangeType), "ChangedRange")]
    [Description ("This property specifies which part of the text will be highlighted as you type.")]
    public HighlightingRangeType HighlightingRangeType { get; set; }

    /// <summary>
    /// Is keyboard in replace mode (wide caret) ?
    /// </summary>
    [Browsable (false)]
    public bool IsReplaceMode
    {
        get =>
            isReplaceMode &&
            Selection is { IsEmpty: true, ColumnSelectionMode: false } &&
            Selection.Start.Column < lines[Selection.Start.Line].Count;
        set => isReplaceMode = value;
    }

    /// <summary>
    /// Allows text rendering several styles same time.
    /// </summary>
    [Browsable (true)]
    [DefaultValue (false)]
    [Description ("Allows text rendering several styles same time.")]
    public bool AllowSeveralTextStyleDrawing { get; set; }

    /// <summary>
    /// Allows to record macros.
    /// </summary>
    [Browsable (true)]
    [DefaultValue (true)]
    [Description ("Allows to record macros.")]
    public bool AllowMacroRecording
    {
        get => _macroManager.AllowMacroRecordingByUser;
        set => _macroManager.AllowMacroRecordingByUser = value;
    }

    /// <summary>
    /// Allows AutoIndent. Inserts spaces before new line.
    /// </summary>
    [DefaultValue (true)]
    [Description ("Allows auto indent. Inserts spaces before line chars.")]
    public bool AutoIndent { get; set; }

    /// <summary>
    /// Does autoindenting in existing lines. It works only if AutoIndent is True.
    /// </summary>
    [DefaultValue (true)]
    [Description ("Does autoindenting in existing lines. It works only if AutoIndent is True.")]
    public bool AutoIndentExistingLines { get; set; }

    /// <summary>
    /// Minimal delay(ms) for delayed events (except TextChangedDelayed).
    /// </summary>
    [Browsable (true)]
    [DefaultValue (100)]
    [Description ("Minimal delay(ms) for delayed events (except TextChangedDelayed).")]
    public int DelayedEventsInterval
    {
        get => _timer1.Interval;
        set => _timer1.Interval = value;
    }

    /// <summary>
    /// Minimal delay(ms) for TextChangedDelayed event.
    /// </summary>
    [Browsable (true)]
    [DefaultValue (100)]
    [Description ("Minimal delay(ms) for TextChangedDelayed event.")]
    public int DelayedTextChangedInterval
    {
        get => _timer2.Interval;
        set => _timer2.Interval = value;
    }

    /// <summary>
    /// Language for highlighting by built-in highlighter.
    /// </summary>
    [Browsable (true)]
    [DefaultValue (typeof (Language), "Custom")]
    [Description ("Language for highlighting by built-in highlighter.")]
    public Language Language
    {
        get => language;
        set
        {
            language = value;
            SyntaxHighlighter!.InitStyleSchema (language);

            Invalidate();
        }
    }

    /// <summary>
    /// Syntax Highlighter
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public SyntaxHighlighter? SyntaxHighlighter { get; set; }

    /// <summary>
    /// XML file with description of syntax highlighting.
    /// This property works only with Language == Language.Custom.
    /// </summary>
    [Browsable (true)]
    [DefaultValue (null)]
    [Editor (typeof (FileNameEditor), typeof (UITypeEditor))]
    [Description (
            "XML file with description of syntax highlighting. This property works only with Language == Language.Custom."
        )]
    public string DescriptionFile
    {
        get => _descriptionFile;
        set
        {
            _descriptionFile = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Position of left highlighted bracket.
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public TextRange LeftBracketPosition => leftBracketPosition;

    /// <summary>
    /// Position of right highlighted bracket.
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public TextRange RightBracketPosition => rightBracketPosition;

    /// <summary>
    /// Position of left highlighted alternative bracket.
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public TextRange LeftBracketPosition2 => leftBracketPosition2;

    /// <summary>
    /// Position of right highlighted alternative bracket.
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public TextRange RightBracketPosition2 => rightBracketPosition2;

    /// <summary>
    /// Start line index of current highlighted folding area. Return -1 if start of area is not found.
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public int StartFoldingLine => _startFoldingLine;

    /// <summary>
    /// End line index of current highlighted folding area. Return -1 if end of area is not found.
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public int EndFoldingLine => _endFoldingLine;

    /// <summary>
    /// TextSource
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public TextSource TextSource
    {
        get => lines;
        set => InitTextSource (value);
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public bool HasSourceTextBox => SourceTextBox != null;

    /// <summary>
    /// The source of the text.
    /// Allows to get text from other FastColoredTextBox.
    /// </summary>
    [Browsable (true)]
    [DefaultValue (null)]
    [Description ("Allows to get text from other FastColoredTextBox.")]

    //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public SyntaxTextBox? SourceTextBox
    {
        get => sourceTextBox;
        set
        {
            if (value == sourceTextBox)
            {
                return;
            }

            sourceTextBox = value;

            if (sourceTextBox == null)
            {
                InitTextSource (CreateTextSource());
                lines.InsertLine (0, TextSource.CreateLine());
                IsChanged = false;
            }
            else
            {
                InitTextSource (SourceTextBox!.TextSource);
                isChanged = false;
            }

            Invalidate();
        }
    }

    /// <summary>
    /// Returns current visible range of text
    /// </summary>
    [Browsable (false)]
    public TextRange VisibleRange => _visibleRange ?? GetRange
            (
                PointToPlace (new Point (LeftIndent, 0)),
                PointToPlace (new Point (ClientSize.Width, ClientSize.Height))
            );

    /// <summary>
    /// Current selection range
    /// </summary>
    [Browsable (false)]
    public TextRange Selection
    {
        get => _selection;
        set
        {
            if (value == _selection)
            {
                return;
            }

            _selection.BeginUpdate();
            _selection.Start = value.Start;
            _selection.End = value.End;
            _selection.EndUpdate();
            Invalidate();
        }
    }

    /// <summary>
    /// Background color.
    /// It is used if BackBrush is null.
    /// </summary>
    [DefaultValue (typeof (Color), "White")]
    [Description ("Background color.")]
    public override Color BackColor
    {
        get => base.BackColor;
        set => base.BackColor = value;
    }

    /// <summary>
    /// Background brush.
    /// If Null then BackColor is used.
    /// </summary>
    [Browsable (false)]
    public Brush? BackBrush
    {
        get => backBrush;
        set
        {
            backBrush = value;
            Invalidate();
        }
    }

    /// <summary>
    ///
    /// </summary>
    [Browsable (true)]
    [DefaultValue (true)]
    [Description ("Scollbars visibility.")]
    public bool ShowScrollBars
    {
        get => _scrollBars;
        set
        {
            if (value == _scrollBars)
            {
                return;
            }

            _scrollBars = value;
            needRecalc = true;
            Invalidate();
        }
    }

    /// <summary>
    /// Multiline
    /// </summary>
    [Browsable (true)]
    [DefaultValue (true)]
    [Description ("Multiline mode.")]
    public bool Multiline
    {
        get => multiline;
        set
        {
            if (multiline == value)
            {
                return;
            }

            multiline = value;
            needRecalc = true;
            if (multiline)
            {
                base.AutoScroll = true;
                ShowScrollBars = true;
            }
            else
            {
                base.AutoScroll = false;
                ShowScrollBars = false;
                if (lines.Count > 1)
                {
                    lines.RemoveLine (1, lines.Count - 1);
                }

                lines.Manager.ClearHistory();
            }

            Invalidate();
        }
    }

    /// <summary>
    /// WordWrap.
    /// </summary>
    [Browsable (true)]
    [DefaultValue (false)]
    [Description ("WordWrap.")]
    public bool WordWrap
    {
        get => _wordWrap;
        set
        {
            if (_wordWrap == value)
            {
                return;
            }

            _wordWrap = value;
            if (_wordWrap)
            {
                Selection.ColumnSelectionMode = false;
            }

            NeedRecalc (false, true);

            //RecalcWordWrap(0, LinesCount - 1);
            Invalidate();
        }
    }

    /// <summary>
    /// WordWrap mode.
    /// </summary>
    [Browsable (true)]
    [DefaultValue (typeof (WordWrapMode), "WordWrapControlWidth")]
    [Description ("WordWrap mode.")]
    public WordWrapMode WordWrapMode
    {
        get => wordWrapMode;
        set
        {
            if (wordWrapMode == value)
            {
                return;
            }

            wordWrapMode = value;
            NeedRecalc (false, true);

            //RecalcWordWrap(0, LinesCount - 1);
            Invalidate();
        }
    }

    private bool selectionHighlightingForLineBreaksEnabled;

    /// <summary>
    /// If <c>true</c> then line breaks included into the selection will be selected too.
    /// Then line breaks will be shown as selected blank character.
    /// </summary>
    [DefaultValue (true)]
    [Description ("If enabled then line ends included into the selection will be selected too. " +
                  "Then line ends will be shown as selected blank character.")]
    public bool SelectionHighlightingForLineBreaksEnabled
    {
        get => selectionHighlightingForLineBreaksEnabled;
        set
        {
            selectionHighlightingForLineBreaksEnabled = value;
            Invalidate();
        }
    }


    /// <summary>
    ///
    /// </summary>
    [Browsable (false)] public FindForm? findForm { get; private set; }

    /// <summary>
    ///
    /// </summary>
    [Browsable (false)] public ReplaceForm? replaceForm { get; private set; }

    /// <summary>
    /// Do not change this property
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public override bool AutoScroll
    {
        get => base.AutoScroll;
        set
        {
            // пустое тело метода
        }
    }

    /// <summary>
    /// Count of lines
    /// </summary>
    [Browsable (false)]
    public int LinesCount => lines.Count;

    /// <summary>
    /// Gets or sets char and styleId for given place
    /// This property does not fire OnTextChanged event
    /// </summary>
    public Character this [Place place]
    {
        get => lines[place.Line][place.Column];
        set => lines[place.Line][place.Column] = value;
    }

    /// <summary>
    /// Gets Line
    /// </summary>
    public Line this [int iLine] => lines[iLine];

    /// <summary>
    /// Text of control
    /// </summary>
    [Browsable (true)]
    [Localizable (true)]
    [Editor ("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
        typeof (UITypeEditor))]
    [SettingsBindable (true)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Visible)]
    [Description ("Text of the control.")]
    [Bindable (true)]
    public override string Text
    {
        get
        {
            if (LinesCount == 0)
            {
                return "";
            }

            var sel = new TextRange (this);
            sel.SelectAll();
            return sel.Text;
        }

        set
        {
            if (value == Text && value != "")
            {
                return;
            }

            SetAsCurrentTB();

            Selection.ColumnSelectionMode = false;

            Selection.BeginUpdate();
            try
            {
                Selection.SelectAll();
                InsertText (value);
                GoHome();
            }
            finally
            {
                Selection.EndUpdate();
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public int TextLength
    {
        get
        {
            if (LinesCount == 0)
            {
                return 0;
            }

            var sel = new TextRange (this);
            sel.SelectAll();
            return sel.Length;
        }
    }

    /// <summary>
    /// Text lines
    /// </summary>
    [Browsable (false)]
    public IList<string> Lines => lines.GetLines();

    /// <summary>
    /// Gets colored text as HTML
    /// </summary>
    /// <remarks>For more flexibility you can use ExportToHTML class also</remarks>
    [Browsable (false)]
    public string Html
    {
        get
        {
            var exporter = new ExportToHtml
            {
                UseNbsp = false,
                UseStyleTag = false,
                UseBr = false
            };
            return "<pre>" + exporter.GetHtml (this) + "</pre>";
        }
    }

    /// <summary>
    /// Gets colored text as RTF
    /// </summary>
    /// <remarks>For more flexibility you can use ExportToRTF class also</remarks>
    [Browsable (false)]
    public string Rtf
    {
        get
        {
            var exporter = new ExportToRtf();
            return exporter.GetRtf (this);
        }
    }

    /// <summary>
    /// Text of current selection
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public string SelectedText
    {
        get => Selection.Text;
        set => InsertText (value);
    }

    /// <summary>
    /// Start position of selection
    /// </summary>
    [Browsable (false)]
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public int SelectionStart
    {
        get => Math.Min (PlaceToPosition (Selection.Start), PlaceToPosition (Selection.End));
        set => Selection.Start = PositionToPlace (value);
    }

    /// <summary>
    /// Length of selected text
    /// </summary>
    [Browsable (false)]
    [DefaultValue (0)]
    public int SelectionLength
    {
        get => Selection.Length;
        set
        {
            if (value > 0)
            {
                Selection.End = PositionToPlace (SelectionStart + value);
            }
        }
    }

    /// <summary>
    /// Font
    /// </summary>
    /// <remarks>Use only monospaced font</remarks>
    [DefaultValue (typeof (Font), "Courier New, 9.75")]
    public override Font Font
    {
        get => BaseFont;
        set
        {
            originalFont = (Font)value.Clone();
            SetFont (value);
        }
    }


    /// <summary>
    /// Font
    /// </summary>
    /// <remarks>Use only monospaced font</remarks>
    [DefaultValue (typeof (Font), "Courier New, 9.75")]
    private Font BaseFont { get; set; }

    private void SetFont (Font newFont)
    {
        BaseFont = newFont;

        //check monospace font
        var sizeM = GetCharSize (BaseFont, 'M');
        var sizeDot = GetCharSize (BaseFont, '.');
        if (sizeM != sizeDot)
        {
            BaseFont = new Font ("Courier New", BaseFont.SizeInPoints, FontStyle.Regular, GraphicsUnit.Point);
        }

        //clac size
        var size = GetCharSize (BaseFont, 'M');
        CharWidth = (int)Math.Round (size.Width * 1f /*0.85*/) - 1 /*0*/;
        CharHeight = lineInterval + (int)Math.Round (size.Height * 1f /*0.9*/) - 1 /*0*/;

        //
        //if (wordWrap)
        //    RecalcWordWrap(0, Lines.Count - 1);
        NeedRecalc (false, _wordWrap);

        //
        Invalidate();
    }

    /// <summary>
    ///
    /// </summary>
    public new Size AutoScrollMinSize
    {
        set
        {
            if (_scrollBars)
            {
                if (!base.AutoScroll)
                {
                    base.AutoScroll = true;
                }

                var newSize = value;
                if (WordWrap && WordWrapMode != WordWrapMode.Custom)
                {
                    var maxWidth = GetMaxLineWordWrapedWidth();
                    newSize = new Size (Math.Min (newSize.Width, maxWidth), newSize.Height);
                }

                base.AutoScrollMinSize = newSize;
            }
            else
            {
                if (base.AutoScroll)
                {
                    base.AutoScroll = false;
                }

                base.AutoScrollMinSize = new Size (0, 0);
                VerticalScroll.Visible = false;
                HorizontalScroll.Visible = false;
                VerticalScroll.Maximum = Math.Max (0, value.Height - ClientSize.Height);
                HorizontalScroll.Maximum = Math.Max (0, value.Width - ClientSize.Width);
                localAutoScrollMinSize = value;
            }
        }

        get
        {
            if (_scrollBars)
            {
                return base.AutoScrollMinSize;
            }
            else

                //return new Size(HorizontalScroll.Maximum, VerticalScroll.Maximum);
            {
                return localAutoScrollMinSize;
            }
        }
    }

    /// <summary>
    /// Indicates that IME is allowed (for CJK language entering)
    /// </summary>
    [Browsable (false)]
    public bool ImeAllowed =>
        ImeMode != ImeMode.Disable &&
        ImeMode != ImeMode.Off &&
        ImeMode != ImeMode.NoControl;

    /// <summary>
    /// Is undo enabled?
    /// </summary>
    [Browsable (false)]
    public bool UndoEnabled => lines.Manager.UndoEnabled;

    /// <summary>
    /// Is redo enabled?
    /// </summary>
    [Browsable (false)]
    public bool RedoEnabled => lines.Manager.RedoEnabled;

    private int LeftIndentLine => LeftIndent - minLeftIndent / 2 - 3;

    /// <summary>
    /// Range of all text
    /// </summary>
    [Browsable (false)]
    public TextRange Range => new TextRange (this, new Place (0, 0), new Place (lines[^1].Count, lines.Count - 1));

    /// <summary>
    /// Color of selected area
    /// </summary>
    [DefaultValue (typeof (Color), "Blue")]
    [Description ("Color of selected area.")]
    public virtual Color SelectionColor
    {
        get => selectionColor;
        set
        {
            selectionColor = value;
            if (selectionColor.A == 255)
            {
                selectionColor = Color.FromArgb (60, selectionColor);
            }

            SelectionStyle = new SelectionStyle (new SolidBrush (selectionColor));
            Invalidate();
        }
    }

    /// <inheritdoc cref="Control.Cursor"/>
    public override Cursor Cursor
    {
        get => base.Cursor;
        set
        {
            defaultCursor = value;
            base.Cursor = value;
        }
    }

    /// <summary>
    /// Reserved space for line number characters.
    /// If smaller than needed (e. g. line count >= 10 and this value set to 1) this value will have no impact.
    /// If you want to reserve space, e. g. for line numbers >= 10 or >= 100 than you can set this value to 2 or 3 or higher.
    /// </summary>
    [DefaultValue (1)]
    [Description ("Reserved space for line number characters. If smaller than needed (e. g. line count >= 10 and " +
                  "this value set to 1) this value will have no impact. If you want to reserve space, e. g. for line " +
                  "numbers >= 10 or >= 100, than you can set this value to 2 or 3 or higher.")]
    public int ReservedCountOfLineNumberChars
    {
        get => reservedCountOfLineNumberChars;
        set
        {
            reservedCountOfLineNumberChars = value;
            NeedRecalc();
            Invalidate();
        }
    }

    /// <summary>
    /// Occurs when mouse is moving over text and tooltip is needed
    /// </summary>
    [Browsable (true)]
    [Description ("Occurs when mouse is moving over text and tooltip is needed.")]
    public event EventHandler<ToolTipNeededEventArgs>? ToolTipNeeded;

    /// <summary>
    /// Default size of the markers
    /// </summary>
    [Browsable (false)]
    [DefaultValue (0)]
    public int DefaultMarkerSize
    {
        get => defaultMarkerSize;
        set
        {
            defaultMarkerSize = value;
            Invalidate();
        }
    }

    private int defaultMarkerSize = 8;

    /// <summary>
    /// Removes all hints
    /// </summary>
    public void ClearHints()
    {
        Hints.Clear();
    }

    /// <summary>
    /// Add and shows the hint
    /// </summary>
    /// <param name="range">Linked range</param>
    /// <param name="innerControl">Inner control</param>
    /// <param name="scrollToHint">Scrolls textbox to the hint</param>
    /// <param name="inline">Inlining. If True then hint will moves apart text</param>
    /// <param name="dock">Docking. If True then hint will fill whole line</param>
    public virtual Hint AddHint (TextRange range, Control innerControl, bool scrollToHint, bool inline, bool dock)
    {
        var hint = new Hint (range, innerControl, inline, dock);
        Hints.Add (hint);
        if (scrollToHint)
        {
            hint.DoVisible();
        }

        return hint;
    }

    /// <summary>
    /// Add and shows the hint
    /// </summary>
    /// <param name="range">Linked range</param>
    /// <param name="innerControl">Inner control</param>
    public Hint AddHint (TextRange range, Control innerControl)
    {
        return AddHint (range, innerControl, true, true, true);
    }

    /// <summary>
    /// Add and shows simple text hint
    /// </summary>
    /// <param name="range">Linked range</param>
    /// <param name="text">Text of simple hint</param>
    /// <param name="scrollToHint">Scrolls textbox to the hint</param>
    /// <param name="inline">Inlining. If True then hint will moves apart text</param>
    /// <param name="dock">Docking. If True then hint will fill whole line</param>
    public virtual Hint AddHint (TextRange range, string text, bool scrollToHint, bool inline, bool dock)
    {
        var hint = new Hint (range, text, inline, dock);
        Hints.Add (hint);
        if (scrollToHint)
        {
            hint.DoVisible();
        }

        return hint;
    }

    /// <summary>
    /// Add and shows simple text hint
    /// </summary>
    /// <param name="range">Linked range</param>
    /// <param name="text">Text of simple hint</param>
    public Hint AddHint (TextRange range, string text)
    {
        return AddHint (range, text, true, true, true);
    }

    /// <summary>
    /// Occurs when user click on the hint
    /// </summary>
    /// <param name="hint"></param>
    public virtual void OnHintClick (Hint hint)
    {
        HintClick?.Invoke (this, new HintClickEventArgs (hint));
    }

    private void timer3_Tick (object? sender, EventArgs e)
    {
        _timer3.Stop();
        OnToolTip();
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnToolTip()
    {
        if (ToolTip == null)
        {
            return;
        }

        if (ToolTipNeeded == null)
        {
            return;
        }

        //get place under mouse
        var place = PointToPlace (lastMouseCoord);

        //check distance
        var p = PlaceToPoint (place);
        if (Math.Abs (p.X - lastMouseCoord.X) > CharWidth * 2 ||
            Math.Abs (p.Y - lastMouseCoord.Y) > CharHeight * 2)
        {
            return;
        }

        //get word under mouse
        var r = new TextRange (this, place, place);
        var hoveredWord = r.GetFragment ("[a-zA-Z]").Text;

        //event handler
        var ea = new ToolTipNeededEventArgs (place, hoveredWord);
        ToolTipNeeded (this, ea);

        if (ea.ToolTipText != null!)
        {
            //show tooltip
            ToolTip.ToolTipTitle = ea.ToolTipTitle;
            ToolTip.ToolTipIcon = ea.ToolTipIcon;

            //ToolTip.SetToolTip(this, ea.ToolTipText);
            ToolTip.Show (ea.ToolTipText, this, new Point (lastMouseCoord.X, lastMouseCoord.Y + CharHeight));
        }
    }

    /// <summary>
    /// Occurs when VisibleRange is changed
    /// </summary>
    public virtual void OnVisibleRangeChanged()
    {
        needRecalcFoldingLines = true;

        needRiseVisibleRangeChangedDelayed = true;
        ResetTimer (_timer1);
        VisibleRangeChanged?.Invoke (this, EventArgs.Empty);
    }

    /// <summary>
    /// Invalidates the entire surface of the control and causes the control to be redrawn.
    /// This method is thread safe and does not require Invoke.
    /// </summary>
    public new void Invalidate()
    {
        if (InvokeRequired)
        {
            BeginInvoke (new MethodInvoker (Invalidate));
        }
        else
        {
            base.Invalidate();
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnCharSizeChanged()
    {
        VerticalScroll.SmallChange = _charHeight;
        VerticalScroll.LargeChange = 10 * _charHeight;
        HorizontalScroll.SmallChange = CharWidth;
    }

    /// <summary>
    /// HintClick event.
    /// It occurs if user click on the hint.
    /// </summary>
    [Browsable (true)]
    [Description ("It occurs if user click on the hint.")]
    public event EventHandler<HintClickEventArgs>? HintClick;

    /// <summary>
    /// TextChanged event.
    /// It occurs after insert, delete, clear, undo and redo operations.
    /// </summary>
    [Browsable (true)]
    [Description ("It occurs after insert, delete, clear, undo and redo operations.")]
    public new event EventHandler<TextChangedEventArgs> TextChanged;

    /// <summary>
    /// Fake event for correct data binding
    /// </summary>
    [Browsable (false)]
    internal event EventHandler? BindingTextChanged;

    /// <summary>
    /// Occurs when user paste text from clipboard
    /// </summary>
    [Description ("Occurs when user paste text from clipboard")]
    public event EventHandler<TextChangingEventArgs>? Pasting;

    /// <summary>
    /// TextChanging event.
    /// It occurs before insert, delete, clear, undo and redo operations.
    /// </summary>
    [Browsable (true)]
    [Description ("It occurs before insert, delete, clear, undo and redo operations.")]
    public event EventHandler<TextChangingEventArgs>? TextChanging;

    /// <summary>
    /// SelectionChanged event.
    /// It occurs after changing of selection.
    /// </summary>
    [Browsable (true)]
    [Description ("It occurs after changing of selection.")]
    public event EventHandler? SelectionChanged;

    /// <summary>
    /// VisibleRangeChanged event.
    /// It occurs after changing of visible range.
    /// </summary>
    [Browsable (true)]
    [Description ("It occurs after changing of visible range.")]
    public event EventHandler? VisibleRangeChanged;

    /// <summary>
    /// TextChangedDelayed event.
    /// It occurs after insert, delete, clear, undo and redo operations.
    /// This event occurs with a delay relative to TextChanged, and fires only once.
    /// </summary>
    [Browsable (true)]
    [Description ("It occurs after insert, delete, clear, undo and redo operations. This event occurs with a delay relative to TextChanged, and fires only once.")]
    public event EventHandler<TextChangedEventArgs>? TextChangedDelayed;

    /// <summary>
    /// SelectionChangedDelayed event.
    /// It occurs after changing of selection.
    /// This event occurs with a delay relative to SelectionChanged, and fires only once.
    /// </summary>
    [Browsable (true)]
    [Description ("It occurs after changing of selection. This event occurs with a delay relative to SelectionChanged, and fires only once.")]
    public event EventHandler? SelectionChangedDelayed;

    /// <summary>
    /// VisibleRangeChangedDelayed event.
    /// It occurs after changing of visible range.
    /// This event occurs with a delay relative to VisibleRangeChanged, and fires only once.
    /// </summary>
    [Browsable (true)]
    [Description ("It occurs after changing of visible range. This event occurs with a delay relative to VisibleRangeChanged, and fires only once.")]
    public event EventHandler? VisibleRangeChangedDelayed;

    /// <summary>
    /// It occurs when user click on VisualMarker.
    /// </summary>
    [Browsable (true)]
    [Description ("It occurs when user click on VisualMarker.")]
    public event EventHandler<VisualMarkerEventArgs>? VisualMarkerClick;

    /// <summary>
    /// It occurs when visible char is enetering (alphabetic, digit, punctuation, DEL, BACKSPACE)
    /// </summary>
    /// <remarks>Set Handle to True for cancel key</remarks>
    [Browsable (true)]
    [Description ("It occurs when visible char is enetering (alphabetic, digit, punctuation, DEL, BACKSPACE).")]
    public event KeyPressEventHandler? KeyPressing;

    /// <summary>
    /// It occurs when visible char is enetered (alphabetic, digit, punctuation, DEL, BACKSPACE)
    /// </summary>
    [Browsable (true)]
    [Description ("It occurs when visible char is enetered (alphabetic, digit, punctuation, DEL, BACKSPACE).")]
    public event KeyPressEventHandler? KeyPressed;

    /// <summary>
    /// It occurs when calculates AutoIndent for new line
    /// </summary>
    [Browsable (true)]
    [Description ("It occurs when calculates AutoIndent for new line.")]
    public event EventHandler<AutoIndentEventArgs>? AutoIndentNeeded;

    /// <summary>
    /// It occurs when line background is painting
    /// </summary>
    [Browsable (true)]
    [Description ("It occurs when line background is painting.")]
    public event EventHandler<PaintLineEventArgs>? PaintLine;

    /// <summary>
    /// Occurs when line was inserted/added
    /// </summary>
    [Browsable (true)]
    [Description ("Occurs when line was inserted/added.")]
    public event EventHandler<LineInsertedEventArgs>? LineInserted;

    /// <summary>
    /// Occurs when line was removed
    /// </summary>
    [Browsable (true)]
    [Description ("Occurs when line was removed.")]
    public event EventHandler<LineRemovedEventArgs>? LineRemoved;

    /// <summary>
    /// Occurs when current highlighted folding area is changed.
    /// Current folding area see in StartFoldingLine and EndFoldingLine.
    /// </summary>
    /// <remarks></remarks>
    [Browsable (true)]
    [Description ("Occurs when current highlighted folding area is changed.")]
    public event EventHandler<EventArgs>? FoldingHighlightChanged;

    /// <summary>
    /// Occurs when undo/redo stack is changed
    /// </summary>
    /// <remarks></remarks>
    [Browsable (true)]
    [Description ("Occurs when undo/redo stack is changed.")]
    public event EventHandler<EventArgs>? UndoRedoStateChanged;

    /// <summary>
    /// Occurs when component was zoomed
    /// </summary>
    [Browsable (true)]
    [Description ("Occurs when component was zoomed.")]
    public event EventHandler? ZoomChanged;


    /// <summary>
    /// Occurs when user pressed key, that specified as CustomAction
    /// </summary>
    [Browsable (true)]
    [Description ("Occurs when user pressed key, that specified as CustomAction.")]
    public event EventHandler<CustomActionEventArgs>? CustomAction;

    /// <summary>
    /// Occurs when scroolbars are updated
    /// </summary>
    [Browsable (true)]
    [Description ("Occurs when scroolbars are updated.")]
    public event EventHandler? ScrollbarsUpdated;

    /// <summary>
    /// Occurs when custom wordwrap is needed
    /// </summary>
    [Browsable (true)]
    [Description ("Occurs when custom wordwrap is needed.")]
    public event EventHandler<WordWrapNeededEventArgs>? WordWrapNeeded;

    /// <summary>
    /// Returns list of styles of given place
    /// </summary>
    public List<Style> GetStylesOfChar (Place place)
    {
        var result = new List<Style>();
        if (place.Line < LinesCount && place.Column < this[place.Line].Count)
        {
#if Styles32
                var s = (uint) this[place].style;
                for (int i = 0; i < 32; i++)
                    if ((s & ((uint) 1) << i) != 0)
                        result.Add(Styles[i]);
#else
            var s = (ushort)this[place].style;
            for (var i = 0; i < 16; i++)
            {
                if ((s & (ushort)1 << i) != 0)
                {
                    result.Add (Styles[i]);
                }
            }
#endif
        }

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected virtual TextSource CreateTextSource()
    {
        return new TextSource (this);
    }

    private void SetAsCurrentTB()
    {
        TextSource.CurrentTextBox = this;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="ts"></param>
    protected virtual void InitTextSource (TextSource ts)
    {
        if (lines != null)
        {
            lines.LineInserted -= ts_LineInserted;
            lines.LineRemoved -= ts_LineRemoved;
            lines.TextChanged -= ts_TextChanged;
            lines.RecalcNeeded -= ts_RecalcNeeded;
            lines.RecalcWordWrap -= ts_RecalcWordWrap;
            lines.TextChanging -= ts_TextChanging;

            lines.Dispose();
        }

        LineInfos.Clear();
        ClearHints();
        Bookmarks?.Clear();

        lines = ts;

        if (ts != null)
        {
            ts.LineInserted += ts_LineInserted;
            ts.LineRemoved += ts_LineRemoved;
            ts.TextChanged += ts_TextChanged;
            ts.RecalcNeeded += ts_RecalcNeeded;
            ts.RecalcWordWrap += ts_RecalcWordWrap;
            ts.TextChanging += ts_TextChanging;

            while (LineInfos.Count < ts.Count)
                LineInfos.Add (new LineInfo (-1));
        }

        isChanged = false;
        needRecalc = true;
    }

    private void ts_RecalcWordWrap
        (
            object? sender,
            TextSource.TextChangedEventArgs eventArgs
        )
    {
        RecalcWordWrap (eventArgs.iFromLine, eventArgs.iToLine);
    }

    private void ts_TextChanging
        (
            object? sender,
            TextChangingEventArgs eventArgs
        )
    {
        if (TextSource.CurrentTextBox == this)
        {
            var text = eventArgs.InsertingText;
            OnTextChanging (ref text);
            eventArgs.InsertingText = text;
        }
    }

    private void ts_RecalcNeeded
        (
            object? sender,
            TextSource.TextChangedEventArgs eventArgs
        )
    {
        if (eventArgs.iFromLine == eventArgs.iToLine && !WordWrap && lines.Count > minLinesForAccuracy)
        {
            RecalcScrollByOneLine (eventArgs.iFromLine);
        }
        else
        {
            NeedRecalc (false, WordWrap);

            //needRecalc = true;
        }
    }

    /// <summary>
    /// Call this method if the recalc of the position of lines is needed.
    /// </summary>
    public void NeedRecalc()
    {
        NeedRecalc (false);
    }

    /// <summary>
    /// Call this method if the recalc of the position of lines is needed.
    /// </summary>
    public void NeedRecalc (bool forced)
    {
        NeedRecalc (forced, false);
    }

    /// <summary>
    /// Call this method if the recalc of the position of lines is needed.
    /// </summary>
    public void NeedRecalc (bool forced, bool wordWrapRecalc)
    {
        needRecalc = true;

        if (wordWrapRecalc)
        {
            needRecalcWordWrapInterval = new Point (0, LinesCount - 1);
            needRecalcWordWrap = true;
        }

        if (forced)
        {
            Recalc();
        }
    }

    private void ts_TextChanged
        (
            object? sender,
            TextSource.TextChangedEventArgs eventArgs
        )
    {
        if (eventArgs.iFromLine == eventArgs.iToLine && !WordWrap)
        {
            RecalcScrollByOneLine (eventArgs.iFromLine);
        }
        else
        {
            needRecalc = true;
        }

        Invalidate();
        if (TextSource.CurrentTextBox == this)
        {
            OnTextChanged (eventArgs.iFromLine, eventArgs.iToLine);
        }
    }

    private void ts_LineRemoved
        (
            object? sender,
            LineRemovedEventArgs eventArgs
        )
    {
        LineInfos.RemoveRange (eventArgs.Index, eventArgs.Count);
        OnLineRemoved (eventArgs.Index, eventArgs.Count, eventArgs.RemovedLineUniqueIds);
    }

    private void ts_LineInserted
        (
            object? sender,
            LineInsertedEventArgs eventArgs
        )
    {
        var newState = VisibleState.Visible;
        if (eventArgs.Index >= 0 && eventArgs.Index < LineInfos.Count && LineInfos[eventArgs.Index].VisibleState == VisibleState.Hidden)
        {
            newState = VisibleState.Hidden;
        }

        if (eventArgs.Count > 100000)
        {
            LineInfos.Capacity = LineInfos.Count + eventArgs.Count + 1000;
        }

        var temp = new LineInfo[eventArgs.Count];
        for (var i = 0; i < eventArgs.Count; i++)
        {
            temp[i].startY = -1;
            temp[i].VisibleState = newState;
        }

        LineInfos.InsertRange (eventArgs.Index, temp);

        /*
        for (int i = 0; i < e.Count; i++)
        {
            LineInfos.Add(new LineInfo(-1) { VisibleState = newState });//<---- needed Insert
            if(i % 1000000 == 0 && i > 0)
                GC.Collect();
        }*/

        if (eventArgs.Count > 1000000)
        {
            GC.Collect();
        }

        OnLineInserted (eventArgs.Index, eventArgs.Count);
    }

    /// <summary>
    /// Navigates forward (by Line.LastVisit property)
    /// </summary>
    public bool NavigateForward()
    {
        var min = DateTime.Now;
        var iLine = -1;
        for (var i = 0; i < LinesCount; i++)
        {
            if (lines.IsLineLoaded (i))
            {
                if (lines[i].LastVisit > lastNavigatedDateTime && lines[i].LastVisit < min)
                {
                    min = lines[i].LastVisit;
                    iLine = i;
                }
            }
        }

        if (iLine >= 0)
        {
            Navigate (iLine);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Navigates backward (by Line.LastVisit property)
    /// </summary>
    public bool NavigateBackward()
    {
        var max = new DateTime();
        var iLine = -1;
        for (var i = 0; i < LinesCount; i++)
        {
            if (lines.IsLineLoaded (i))
            {
                if (lines[i].LastVisit < lastNavigatedDateTime && lines[i].LastVisit > max)
                {
                    max = lines[i].LastVisit;
                    iLine = i;
                }
            }
        }

        if (iLine >= 0)
        {
            Navigate (iLine);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Navigates to defined line, without Line.LastVisit reseting
    /// </summary>
    public void Navigate (int iLine)
    {
        if (iLine >= LinesCount)
        {
            return;
        }

        lastNavigatedDateTime = lines[iLine].LastVisit;
        Selection.Start = new Place (0, iLine);
        DoSelectionVisible();
    }

    /// <inheritdoc cref="UserControl.OnLoad"/>
    protected override void OnLoad (EventArgs eventArgs)
    {
        base.OnLoad (eventArgs);
        m_hImc = ImmGetContext (Handle);
    }

    private void timer2_Tick (object? sender, EventArgs eventArgs)
    {
        _timer2.Enabled = false;
        if (needRiseTextChangedDelayed)
        {
            needRiseTextChangedDelayed = false;
            if (_delayedTextChangedRange == null)
            {
                return;
            }

            _delayedTextChangedRange = Range.GetIntersectionWith (_delayedTextChangedRange);
            _delayedTextChangedRange.Expand();
            OnTextChangedDelayed (_delayedTextChangedRange);
            _delayedTextChangedRange = null;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="marker"></param>
    public void AddVisualMarker (VisualMarker marker)
    {
        visibleMarkers.Add (marker);
    }

    private void timer_Tick (object? sender, EventArgs e)
    {
        _timer1.Enabled = false;
        if (needRiseSelectionChangedDelayed)
        {
            needRiseSelectionChangedDelayed = false;
            OnSelectionChangedDelayed();
        }

        if (needRiseVisibleRangeChangedDelayed)
        {
            needRiseVisibleRangeChangedDelayed = false;
            OnVisibleRangeChangedDelayed();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="changedRange"></param>
    public virtual void OnTextChangedDelayed (TextRange changedRange)
    {
        TextChangedDelayed?.Invoke (this, new TextChangedEventArgs (changedRange));
    }

    /// <summary>
    ///
    /// </summary>
    public virtual void OnSelectionChangedDelayed()
    {
        RecalcScrollByOneLine (Selection.Start.Line);

        //highlight brackets
        ClearBracketsPositions();
        if (LeftBracket != '\x0' && RightBracket != '\x0')
        {
            HighlightBrackets (LeftBracket, RightBracket, ref leftBracketPosition, ref rightBracketPosition);
        }

        if (LeftBracket2 != '\x0' && RightBracket2 != '\x0')
        {
            HighlightBrackets (LeftBracket2, RightBracket2, ref leftBracketPosition2, ref rightBracketPosition2);
        }

        //remember last visit time
        if (Selection.IsEmpty && Selection.Start.Line < LinesCount)
        {
            if (lastNavigatedDateTime != lines[Selection.Start.Line].LastVisit)
            {
                lines[Selection.Start.Line].LastVisit = DateTime.Now;
                lastNavigatedDateTime = lines[Selection.Start.Line].LastVisit;
            }
        }

        SelectionChangedDelayed?.Invoke (this, EventArgs.Empty);
    }

    /// <summary>
    ///
    /// </summary>
    public virtual void OnVisibleRangeChangedDelayed()
    {
        VisibleRangeChangedDelayed?.Invoke (this, EventArgs.Empty);
    }

    private readonly Dictionary<Timer, Timer> timersToReset = new ();

    private void ResetTimer (Timer timer)
    {
        if (InvokeRequired)
        {
            BeginInvoke (new MethodInvoker (() => ResetTimer (timer)));
            return;
        }

        timer.Stop();
        if (IsHandleCreated)
        {
            timer.Start();
        }
        else
        {
            timersToReset[timer] = timer;
        }
    }

    /// <inheritdoc cref="Control.OnHandleCreated"/>
    protected override void OnHandleCreated (EventArgs e)
    {
        base.OnHandleCreated (e);
        foreach (var timer in new List<Timer> (timersToReset.Keys))
        {
            ResetTimer (timer);
        }

        timersToReset.Clear();

        OnScrollbarsUpdated();
    }

    /// <summary>
    /// Add new style
    /// </summary>
    /// <returns>Layer index of this style</returns>
    public int AddStyle (Style? style)
    {
        if (style == null)
        {
            return -1;
        }

        var i = GetStyleIndex (style);
        if (i >= 0)
        {
            return i;
        }

        i = CheckStylesBufferSize();
        Styles[i] = style;
        return i;
    }

    /// <summary>
    /// Checks if the styles buffer has enough space to add one
    /// more element. If not, an exception is thrown. Otherwise,
    /// the index of a free slot is returned.
    /// </summary>
    /// <returns>Index of free styles buffer slot</returns>
    /// <exception cref="Exception">If maximum count of styles is exceeded</exception>
    public int CheckStylesBufferSize()
    {
        int i;
        for (i = Styles.Length - 1; i >= 0; i--)
        {
            if (Styles[i] != null!)
            {
                break;
            }
        }

        i++;
        if (i >= Styles.Length)
        {
            throw new Exception ("Maximum count of Styles is exceeded.");
        }

        return i;
    }

    /// <summary>
    /// Shows find dialog
    /// </summary>
    public virtual void ShowFindDialog()
    {
        ShowFindDialog (null);
    }

    /// <summary>
    /// Shows find dialog
    /// </summary>
    public virtual void ShowFindDialog
        (
            string? findText
        )
    {
        findForm ??= new FindForm (this);

        if (findText != null)
        {
            findForm.tbFind.Text = findText;
        }
        else if (!Selection.IsEmpty && Selection.Start.Line == Selection.End.Line)
        {
            findForm.tbFind.Text = Selection.Text;
        }

        findForm.tbFind.SelectAll();
        findForm.Show();
        findForm.Focus();
    }

    /// <summary>
    /// Shows replace dialog
    /// </summary>
    public virtual void ShowReplaceDialog()
    {
        ShowReplaceDialog (null);
    }

    /// <summary>
    /// Shows replace dialog
    /// </summary>
    public virtual void ShowReplaceDialog (string? findText)
    {
        if (ReadOnly)
        {
            return;
        }

        replaceForm ??= new ReplaceForm (this);

        if (findText != null)
        {
            replaceForm.tbFind.Text = findText;
        }
        else if (!Selection.IsEmpty && Selection.Start.Line == Selection.End.Line)
        {
            replaceForm.tbFind.Text = Selection.Text;
        }

        replaceForm.tbFind.SelectAll();
        replaceForm.Show();
        replaceForm.Focus();
    }

    /// <summary>
    /// Gets length of given line
    /// </summary>
    /// <param name="iLine">Line index</param>
    /// <returns>Length of line</returns>
    public int GetLineLength (int iLine)
    {
        if (iLine < 0 || iLine >= lines.Count)
        {
            throw new ArgumentOutOfRangeException (nameof (iLine), "Line index out of range");
        }

        return lines[iLine].Count;
    }

    /// <summary>
    /// Get range of line
    /// </summary>
    /// <param name="iLine">Line index</param>
    public TextRange GetLine (int iLine)
    {
        if (iLine < 0 || iLine >= lines.Count)
        {
            throw new ArgumentOutOfRangeException (nameof (iLine), "Line index out of range");
        }

        var sel = new TextRange (this)
        {
            Start = new Place (0, iLine),
            End = new Place (lines[iLine].Count, iLine)
        };
        return sel;
    }

    /// <summary>
    /// Copy selected text into Clipboard
    /// </summary>
    public virtual void Copy()
    {
        if (Selection.IsEmpty)
        {
            Selection.Expand();
        }

        if (!Selection.IsEmpty)
        {
            var data = new DataObject();
            OnCreateClipboardData (data);

            //
            var thread = new Thread (() => SetClipboard (data));
            thread.SetApartmentState (ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="data"></param>
    protected virtual void OnCreateClipboardData (DataObject data)
    {
        var exp = new ExportToHtml
        {
            UseBr = false,
            UseNbsp = false,
            UseStyleTag = true
        };
        var html = "<pre>" + exp.GetHtml (Selection.Clone()) + "</pre>";

        data.SetData (DataFormats.UnicodeText, true, Selection.Text);
        data.SetData (DataFormats.Html, PrepareHtmlForClipboard (html));
        data.SetData (DataFormats.Rtf, new ExportToRtf().GetRtf (Selection.Clone()));
    }

    [DllImport ("user32.dll")]
    private static extern IntPtr GetOpenClipboardWindow();

    [DllImport ("user32.dll")]
    private static extern IntPtr CloseClipboard();

    /// <summary>
    ///
    /// </summary>
    /// <param name="data"></param>
    protected void SetClipboard (DataObject data)
    {
        try
        {
            /*
            while (GetOpenClipboardWindow() != IntPtr.Zero)
                Thread.Sleep(0);*/
            CloseClipboard();
            Clipboard.SetDataObject (data, true, 5, 100);
        }
        catch (ExternalException)
        {
            //occurs if some other process holds open clipboard
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static MemoryStream PrepareHtmlForClipboard (string html)
    {
        var enc = Encoding.UTF8;

        var begin = "Version:0.9\r\nStartHTML:{0:000000}\r\nEndHTML:{1:000000}"
                    + "\r\nStartFragment:{2:000000}\r\nEndFragment:{3:000000}\r\n";

        var html_begin = "<html>\r\n<head>\r\n"
                         + "<meta http-equiv=\"Content-Type\""
                         + " content=\"text/html; charset=" + enc.WebName + "\">\r\n"
                         + "<title>HTML clipboard</title>\r\n</head>\r\n<body>\r\n"
                         + "<!--StartFragment-->";

        var html_end = "<!--EndFragment-->\r\n</body>\r\n</html>\r\n";

        var begin_sample = String.Format (begin, 0, 0, 0, 0);

        var count_begin = enc.GetByteCount (begin_sample);
        var count_html_begin = enc.GetByteCount (html_begin);
        var count_html = enc.GetByteCount (html);
        var count_html_end = enc.GetByteCount (html_end);

        var html_total = String.Format (
                begin
                , count_begin
                , count_begin + count_html_begin + count_html + count_html_end
                , count_begin + count_html_begin
                , count_begin + count_html_begin + count_html
            ) + html_begin + html + html_end;

        return new MemoryStream (enc.GetBytes (html_total));
    }


    /// <summary>
    /// Cut selected text into Clipboard
    /// </summary>
    public virtual void Cut()
    {
        if (!Selection.IsEmpty)
        {
            Copy();
            ClearSelected();
        }
        else if (LinesCount == 1)
        {
            Selection.SelectAll();
            Copy();
            ClearSelected();
        }
        else
        {
            //copy
            var data = new DataObject();
            OnCreateClipboardData (data);

            //
            var thread = new Thread (() => SetClipboard (data));
            thread.SetApartmentState (ApartmentState.STA);
            thread.Start();
            thread.Join();

            //remove current line
            if (Selection.Start.Line >= 0 && Selection.Start.Line < LinesCount)
            {
                var iLine = Selection.Start.Line;
                RemoveLines (new List<int> { iLine });
                Selection.Start = new Place (0, Math.Max (0, Math.Min (iLine, LinesCount - 1)));
            }
        }
    }

    /// <summary>
    /// Paste text from clipboard into selected position
    /// </summary>
    public virtual void Paste()
    {
        string? text = null;
        var thread = new Thread (() =>
        {
            if (Clipboard.ContainsText())
            {
                text = Clipboard.GetText();
            }
        });
        thread.SetApartmentState (ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (Pasting != null)
        {
            var args = new TextChangingEventArgs
            {
                Cancel = false,
                InsertingText = text!
            };

            Pasting (this, args);

            text = args.Cancel ? string.Empty : args.InsertingText;
        }

        if (!string.IsNullOrEmpty (text))
        {
            InsertText (text);
        }
    }

    /// <summary>
    /// Select all chars of text
    /// </summary>
    public void SelectAll()
    {
        Selection.SelectAll();
    }

    /// <summary>
    /// Move caret to end of text
    /// </summary>
    public void GoEnd()
    {
        Selection.Start = lines.Count > 0
            ? new Place (lines[^1].Count, lines.Count - 1)
            : new Place (0, 0);

        DoCaretVisible();
    }

    /// <summary>
    /// Move caret to first position
    /// </summary>
    public void GoHome()
    {
        Selection.Start = new Place (0, 0);

        DoCaretVisible();

        //VerticalScroll.Value = 0;
        //HorizontalScroll.Value = 0;
    }

    /// <summary>
    /// Clear text, styles, history, caches
    /// </summary>
    public virtual void Clear()
    {
        Selection.BeginUpdate();
        try
        {
            Selection.SelectAll();
            ClearSelected();
            lines.Manager.ClearHistory();
            Invalidate();
        }
        finally
        {
            Selection.EndUpdate();
        }
    }

    /// <summary>
    /// Clear buffer of styles
    /// </summary>
    public void ClearStylesBuffer()
    {
        for (var i = 0; i < Styles.Length; i++)
        {
            Styles[i] = null!;
        }
    }

    /// <summary>
    /// Clear style of all text
    /// </summary>
    public void ClearStyle (StyleIndex styleIndex)
    {
        foreach (var line in lines)
        {
            line.ClearStyle (styleIndex);
        }

        for (var i = 0; i < LineInfos.Count; i++)
        {
            SetVisibleState (i, VisibleState.Visible);
        }

        Invalidate();
    }


    /// <summary>
    /// Clears undo and redo stacks
    /// </summary>
    public void ClearUndo()
    {
        lines.Manager.ClearHistory();
    }

    /// <summary>
    /// Insert text into current selected position
    /// </summary>
    public virtual void InsertText (string? text)
    {
        InsertText (text, true);
    }

    /// <summary>
    /// Insert text into current selected position
    /// </summary>
    public virtual void InsertText (string? text, bool jumpToCaret)
    {
        if (text == null)
        {
            return;
        }

        if (text == "\r")
        {
            text = "\n";
        }

        lines.Manager.BeginAutoUndoCommands();
        try
        {
            if (!Selection.IsEmpty)
            {
                lines.Manager.ExecuteCommand (new ClearSelectedCommand (TextSource));
            }

            //insert virtual spaces
            if (this.TextSource.Count > 0)
            {
                if (Selection.IsEmpty && Selection.Start.Column > GetLineLength (Selection.Start.Line) && VirtualSpace)
                {
                    InsertVirtualSpaces();
                }
            }

            lines.Manager.ExecuteCommand (new InsertTextCommand (TextSource, text));
            if (_updating <= 0 && jumpToCaret)
            {
                DoCaretVisible();
            }
        }
        finally
        {
            lines.Manager.EndAutoUndoCommands();
        }

        //
        Invalidate();
    }

    /// <summary>
    /// Insert text into current selection position (with predefined style)
    /// </summary>
    public virtual TextRange? InsertText (string text, Style style)
    {
        return InsertText (text, style, true);
    }

    /// <summary>
    /// Insert text into current selection position (with predefined style)
    /// </summary>
    public virtual TextRange? InsertText (string? text, Style style, bool jumpToCaret)
    {
        if (text == null)
        {
            return null;
        }

        //remember last caret position
        var last = Selection.Start > Selection.End ? Selection.End : Selection.Start;

        //insert text
        InsertText (text, jumpToCaret);

        //get range
        var range = new TextRange (this, last, Selection.Start) { ColumnSelectionMode = Selection.ColumnSelectionMode };
        range = range.GetIntersectionWith (Range);

        //set style for range
        range.SetStyle (style);

        return range;
    }

    /// <summary>
    /// Insert text into replaceRange and restore previous selection
    /// </summary>
    public virtual TextRange? InsertTextAndRestoreSelection
        (
            TextRange replaceRange,
            string? text,
            Style style
        )
    {
        if (text == null)
        {
            return null;
        }

        var oldStart = PlaceToPosition (Selection.Start);
        var oldEnd = PlaceToPosition (Selection.End);
        var count = replaceRange.Text.Length;
        var pos = PlaceToPosition (replaceRange.Start);

        //
        Selection.BeginUpdate();
        Selection = replaceRange;
        var range = InsertText (text, style);

        //
        count = range!.Text.Length - count;
        Selection.Start = PositionToPlace (oldStart + (oldStart >= pos ? count : 0));
        Selection.End = PositionToPlace (oldEnd + (oldEnd >= pos ? count : 0));
        Selection.EndUpdate();

        return range;
    }

    /// <summary>
    /// Append string to end of the Text
    /// </summary>
    public virtual void AppendText (string text)
    {
        AppendText (text, null);
    }

    /// <summary>
    /// Append string to end of the Text
    /// </summary>
    public virtual void AppendText
        (
            string? text,
            Style? style
        )
    {
        if (text == null)
        {
            return;
        }

        Selection.ColumnSelectionMode = false;

        var oldStart = Selection.Start;
        var oldEnd = Selection.End;

        Selection.BeginUpdate();
        lines.Manager.BeginAutoUndoCommands();
        try
        {
            Selection.Start = lines.Count > 0
                ? new Place (lines[^1].Count, lines.Count - 1)
                : new Place (0, 0);

            //remember last caret position
            var last = Selection.Start;

            lines.Manager.ExecuteCommand (new InsertTextCommand (TextSource, text));

            if (style != null)
            {
                new TextRange (this, last, Selection.Start).SetStyle (style);
            }
        }
        finally
        {
            lines.Manager.EndAutoUndoCommands();
            Selection.Start = oldStart;
            Selection.End = oldEnd;
            Selection.EndUpdate();
        }

        //
        Invalidate();
    }

    /// <summary>
    /// Returns index of the style in Styles
    /// -1 otherwise
    /// </summary>
    /// <param name="style"></param>
    /// <returns>Index of the style in Styles</returns>
    public int GetStyleIndex (Style style)
    {
        return Array.IndexOf (Styles, style);
    }

    /// <summary>
    /// Returns StyleIndex mask of given styles
    /// </summary>
    /// <param name="styles"></param>
    /// <returns>StyleIndex mask of given styles</returns>
    public StyleIndex GetStyleIndexMask (Style[] styles)
    {
        var mask = StyleIndex.None;
        foreach (var style in styles)
        {
            var i = GetStyleIndex (style);
            if (i >= 0)
            {
                mask |= TextRange.ToStyleIndex (i);
            }
        }

        return mask;
    }

    internal int GetOrSetStyleLayerIndex (Style style)
    {
        var i = GetStyleIndex (style);
        if (i < 0)
        {
            i = AddStyle (style);
        }

        return i;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="font"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static SizeF GetCharSize (Font font, char c)
    {
        var sz2 = TextRenderer.MeasureText ("<" + c.ToString() + ">", font);
        var sz3 = TextRenderer.MeasureText ("<>", font);

        return new SizeF (sz2.Width - sz3.Width + 1, /*sz2.Height*/font.Height);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="hWnd"></param>
    /// <returns></returns>
    [DllImport ("Imm32.dll")]
    public static extern IntPtr ImmGetContext (IntPtr hWnd);

    /// <summary>
    ///
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="hIMC"></param>
    /// <returns></returns>
    [DllImport ("Imm32.dll")]
    public static extern IntPtr ImmAssociateContext (IntPtr hWnd, IntPtr hIMC);

    /// <inheritdoc cref="UserControl.WndProc"/>
    protected override void WndProc (ref Message m)
    {
        if (m.Msg is WM_HSCROLL or WM_VSCROLL)
        {
            if (m.WParam.ToInt32() != SB_ENDSCROLL)
            {
                Invalidate();
            }
        }

        base.WndProc (ref m);

        if (ImeAllowed)
        {
            if (m.Msg == WM_IME_SETCONTEXT && m.WParam.ToInt32() == 1)
            {
                ImmAssociateContext (Handle, m_hImc);
            }
        }
    }

    private readonly List<Control> tempHintsList = new ();

    private void HideHints()
    {
        //temporarly remove hints
        if (!ShowScrollBars && Hints.Count > 0)
        {
            SuspendLayout();
            foreach (Control c in Controls)
            {
                tempHintsList.Add (c);
            }

            Controls.Clear();
        }
    }

    private void RestoreHints()
    {
        //restore hints
        if (!ShowScrollBars && Hints.Count > 0)
        {
            foreach (var c in tempHintsList)
            {
                Controls.Add (c);
            }

            tempHintsList.Clear();
            ResumeLayout (false);

            if (!Focused)
            {
                Focus();
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void OnScroll (ScrollEventArgs eventArgs, bool alignByLines)
    {
        HideHints();

        if (eventArgs.ScrollOrientation == ScrollOrientation.VerticalScroll)
        {
            //align by line height
            var newValue = eventArgs.NewValue;
            if (alignByLines)
            {
                newValue = (int)(Math.Ceiling (1d * newValue / CharHeight) * CharHeight);
            }

            //
            VerticalScroll.Value = Math.Max (VerticalScroll.Minimum, Math.Min (VerticalScroll.Maximum, newValue));
        }

        if (eventArgs.ScrollOrientation == ScrollOrientation.HorizontalScroll)
        {
            HorizontalScroll.Value =
                Math.Max (HorizontalScroll.Minimum, Math.Min (HorizontalScroll.Maximum, eventArgs.NewValue));
        }

        UpdateScrollbars();

        RestoreHints();

        Invalidate();

        //
        base.OnScroll (eventArgs);
        OnVisibleRangeChanged();
    }

    /// <inheritdoc cref="ScrollableControl.OnScroll"/>
    protected override void OnScroll (ScrollEventArgs eventArgs)
    {
        OnScroll (eventArgs, true);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="c"></param>
    protected virtual void InsertChar (char c)
    {
        lines.Manager.BeginAutoUndoCommands();
        try
        {
            if (!Selection.IsEmpty)
            {
                lines.Manager.ExecuteCommand (new ClearSelectedCommand (TextSource));
            }

            //insert virtual spaces
            if (Selection.IsEmpty && Selection.Start.Column > GetLineLength (Selection.Start.Line) && VirtualSpace)
            {
                InsertVirtualSpaces();
            }

            //insert char
            lines.Manager.ExecuteCommand (new InsertCharCommand (TextSource, c));
        }
        finally
        {
            lines.Manager.EndAutoUndoCommands();
        }

        Invalidate();
    }

    private void InsertVirtualSpaces()
    {
        var lineLength = GetLineLength (Selection.Start.Line);
        var count = Selection.Start.Column - lineLength;
        Selection.BeginUpdate();
        try
        {
            Selection.Start = new Place (lineLength, Selection.Start.Line);
            lines.Manager.ExecuteCommand (new InsertTextCommand (TextSource, new string (' ', count)));
        }
        finally
        {
            Selection.EndUpdate();
        }
    }

    /// <summary>
    /// Deletes selected chars
    /// </summary>
    public virtual void ClearSelected()
    {
        if (!Selection.IsEmpty)
        {
            lines.Manager.ExecuteCommand (new ClearSelectedCommand (TextSource));
            Invalidate();
        }
    }

    /// <summary>
    /// Deletes current line(s)
    /// </summary>
    public void ClearCurrentLine()
    {
        Selection.Expand();

        lines.Manager.ExecuteCommand (new ClearSelectedCommand (TextSource));
        if (Selection.Start.Line == 0)
        {
            if (!Selection.GoRightThroughFolded())
            {
                return;
            }
        }

        if (Selection.Start.Line > 0)
        {
            lines.Manager.ExecuteCommand (new InsertCharCommand (TextSource, '\b')); //backspace
        }

        Invalidate();
    }

    private void Recalc()
    {
        if (!needRecalc)
        {
            return;
        }

#if debug
            var sw = Stopwatch.StartNew();
#endif

        needRecalc = false;

        //calc min left indent
        LeftIndent = LeftPadding;
        var maxLineNumber = LinesCount + lineNumberStartValue - 1;
        var charsForLineNumber = 2 + (maxLineNumber > 0 ? (int)Math.Log10 (maxLineNumber) : 0);

        // If there are reserved character for line numbers: correct this
        if (this.ReservedCountOfLineNumberChars + 1 > charsForLineNumber)
        {
            charsForLineNumber = this.ReservedCountOfLineNumberChars + 1;
        }

        if (Created)
        {
            if (ShowLineNumbers)
            {
                LeftIndent += charsForLineNumber * CharWidth + minLeftIndent + 1;
            }

            //calc wordwrapping
            if (needRecalcWordWrap)
            {
                RecalcWordWrap (needRecalcWordWrapInterval.X, needRecalcWordWrapInterval.Y);
                needRecalcWordWrap = false;
            }
        }
        else
        {
            needRecalc = true;
        }

        //calc max line length and count of wordWrapLines
        TextHeight = 0;

        maxLineLength = RecalcMaxLineLength();

        //adjust AutoScrollMinSize
        CalcMinAutosizeWidth (out var minWidth, ref maxLineLength);

        AutoScrollMinSize = new Size (minWidth, TextHeight + Paddings.Top + Paddings.Bottom);
        UpdateScrollbars();
#if debug
            sw.Stop();
            Console.WriteLine("Recalc: " + sw.ElapsedMilliseconds);
#endif
    }

    private void CalcMinAutosizeWidth (out int minWidth, ref int maxLineLength)
    {
        //adjust AutoScrollMinSize
        minWidth = LeftIndent + maxLineLength * CharWidth + 2 + Paddings.Left + Paddings.Right;
        if (_wordWrap)
        {
            switch (WordWrapMode)
            {
                case WordWrapMode.WordWrapControlWidth:
                case WordWrapMode.CharWrapControlWidth:
                    maxLineLength = Math.Min (maxLineLength,
                        (ClientSize.Width - LeftIndent - Paddings.Left - Paddings.Right) /
                        CharWidth);
                    minWidth = 0;
                    break;
                case WordWrapMode.WordWrapPreferredWidth:
                case WordWrapMode.CharWrapPreferredWidth:
                    maxLineLength = Math.Min (maxLineLength, PreferredLineWidth);
                    minWidth = LeftIndent + PreferredLineWidth * CharWidth + 2 + Paddings.Left + Paddings.Right;
                    break;
            }
        }
    }

    private void RecalcScrollByOneLine (int iLine)
    {
        if (iLine >= lines.Count)
        {
            return;
        }

        var maxLineLength = lines[iLine].Count;
        if (this.maxLineLength < maxLineLength && !WordWrap)
        {
            this.maxLineLength = maxLineLength;
        }

        CalcMinAutosizeWidth (out var minWidth, ref maxLineLength);

        if (AutoScrollMinSize.Width < minWidth)
        {
            AutoScrollMinSize = new Size (minWidth, AutoScrollMinSize.Height);
        }
    }

    private int RecalcMaxLineLength()
    {
        var maxLineLength = 0;
        var lines = this.lines;
        var count = lines.Count;
        var charHeight = CharHeight;
        var topIndent = Paddings.Top;
        TextHeight = topIndent;

        for (var i = 0; i < count; i++)
        {
            var lineLength = lines.GetLineLength (i);
            var lineInfo = LineInfos[i];
            if (lineLength > maxLineLength && lineInfo.VisibleState == VisibleState.Visible)
            {
                maxLineLength = lineLength;
            }

            lineInfo.startY = TextHeight;
            TextHeight += lineInfo.WordWrapStringsCount * charHeight + lineInfo.bottomPadding;
            LineInfos[i] = lineInfo;
        }

        TextHeight -= topIndent;

        return maxLineLength;
    }

    private int GetMaxLineWordWrapedWidth()
    {
        if (_wordWrap)
        {
            switch (wordWrapMode)
            {
                case WordWrapMode.WordWrapControlWidth:
                case WordWrapMode.CharWrapControlWidth:
                    return ClientSize.Width;
                case WordWrapMode.WordWrapPreferredWidth:
                case WordWrapMode.CharWrapPreferredWidth:
                    return LeftIndent + PreferredLineWidth * CharWidth + 2 + Paddings.Left + Paddings.Right;
            }
        }

        return int.MaxValue;
    }

    private void RecalcWordWrap (int fromLine, int toLine)
    {
        var maxCharsPerLine = 0;
        var charWrap = false;

        toLine = Math.Min (LinesCount - 1, toLine);

        switch (WordWrapMode)
        {
            case WordWrapMode.WordWrapControlWidth:
                maxCharsPerLine = (ClientSize.Width - LeftIndent - Paddings.Left - Paddings.Right) / CharWidth;
                break;
            case WordWrapMode.CharWrapControlWidth:
                maxCharsPerLine = (ClientSize.Width - LeftIndent - Paddings.Left - Paddings.Right) / CharWidth;
                charWrap = true;
                break;
            case WordWrapMode.WordWrapPreferredWidth:
                maxCharsPerLine = PreferredLineWidth;
                break;
            case WordWrapMode.CharWrapPreferredWidth:
                maxCharsPerLine = PreferredLineWidth;
                charWrap = true;
                break;
        }

        for (var iLine = fromLine; iLine <= toLine; iLine++)
        {
            if (lines.IsLineLoaded (iLine))
            {
                if (!_wordWrap)
                {
                    LineInfos[iLine].CutOffPositions.Clear();
                }
                else
                {
                    var li = LineInfos[iLine];

                    li.wordWrapIndent = WordWrapAutoIndent
                        ? lines[iLine].StartSpacesCount + WordWrapIndent
                        : WordWrapIndent;

                    if (WordWrapMode == WordWrapMode.Custom)
                    {
                        WordWrapNeeded?.Invoke (this,
                            new WordWrapNeededEventArgs (li.CutOffPositions, ImeAllowed, lines[iLine]));
                    }
                    else
                    {
                        CalcCutOffs (li.CutOffPositions, maxCharsPerLine, maxCharsPerLine - li.wordWrapIndent,
                            ImeAllowed, charWrap, lines[iLine]);
                    }

                    LineInfos[iLine] = li;
                }
            }
        }

        needRecalc = true;
    }

    /// <summary>
    /// Calculates wordwrap cutoffs
    /// </summary>
    public static void CalcCutOffs (List<int> cutOffPositions, int maxCharsPerLine, int maxCharsPerSecondaryLine,
        bool allowIME, bool charWrap, Line line)
    {
        if (maxCharsPerSecondaryLine < 1)
        {
            maxCharsPerSecondaryLine = 1;
        }

        if (maxCharsPerLine < 1)
        {
            maxCharsPerLine = 1;
        }

        var segmentLength = 0;
        var cutOff = 0;
        cutOffPositions.Clear();

        for (var i = 0; i < line.Count - 1; i++)
        {
            var c = line[i].c;
            if (charWrap)
            {
                //char wrapping
                cutOff = i + 1;
            }
            else
            {
                //word wrapping
                if (allowIME && IsCJKLetter (c)) //in CJK languages cutoff can be in any letter
                {
                    cutOff = i;
                }
                else if (!char.IsLetterOrDigit (c) && c != '_' && c != '\'' && c != '\xa0'
                         && (c != '.' && c != ',' || !char.IsDigit (line[i + 1].c))) //dot before digit
                {
                    cutOff = Math.Min (i + 1, line.Count - 1);
                }
            }

            segmentLength++;

            if (segmentLength == maxCharsPerLine)
            {
                if (cutOff == 0 || cutOffPositions.Count > 0 && cutOff == cutOffPositions[^1])
                {
                    cutOff = i + 1;
                }

                cutOffPositions.Add (cutOff);
                segmentLength = 1 + i - cutOff;
                maxCharsPerLine = maxCharsPerSecondaryLine;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static bool IsCJKLetter (char c)
    {
        var code = Convert.ToInt32 (c);
        return
            code is >= 0x3300 and <= 0x33FF
                or >= 0xFE30 and <= 0xFE4F
                or >= 0xF900 and <= 0xFAFF
                or >= 0x2E80 and <= 0x2EFF
                or >= 0x31C0 and <= 0x31EF
                or >= 0x4E00 and <= 0x9FFF
                or >= 0x3400 and <= 0x4DBF
                or >= 0x3200 and <= 0x32FF
                or >= 0x2460 and <= 0x24FF
                or >= 0x3040 and <= 0x309F
                or >= 0x2F00 and <= 0x2FDF
                or >= 0x31A0 and <= 0x31BF
                or >= 0x4DC0 and <= 0x4DFF
                or >= 0x3100 and <= 0x312F
                or >= 0x30A0 and <= 0x30FF
                or >= 0x31F0 and <= 0x31FF
                or >= 0x2FF0 and <= 0x2FFF
                or >= 0x1100 and <= 0x11FF
                or >= 0xA960 and <= 0xA97F
                or >= 0xD7B0 and <= 0xD7FF
                or >= 0x3130 and <= 0x318F
                or >= 0xAC00 and <= 0xD7AF;
    }

    /// <inheritdoc cref="Control.OnClientSizeChanged"/>
    protected override void OnClientSizeChanged (EventArgs eventArgs)
    {
        base.OnClientSizeChanged (eventArgs);
        if (WordWrap)
        {
            //RecalcWordWrap(0, lines.Count - 1);
            NeedRecalc (false, true);
            Invalidate();
        }

        OnVisibleRangeChanged();
        UpdateScrollbars();
    }

    /// <summary>
    /// Scroll control for display defined rectangle
    /// </summary>
    /// <param name="rect"></param>
    internal void DoVisibleRectangle (Rectangle rect)
    {
        HideHints();

        var oldV = VerticalScroll.Value;
        var v = VerticalScroll.Value;
        var h = HorizontalScroll.Value;

        if (rect.Bottom > ClientRectangle.Height)
        {
            v += rect.Bottom - ClientRectangle.Height;
        }
        else if (rect.Top < 0)
        {
            v += rect.Top;
        }

        if (rect.Right > ClientRectangle.Width)
        {
            h += rect.Right - ClientRectangle.Width;
        }
        else if (rect.Left < LeftIndent)
        {
            h += rect.Left - LeftIndent;
        }

        //
        if (!Multiline)
        {
            v = 0;
        }

        //
        v = Math.Max (VerticalScroll.Minimum, v); // was 0
        h = Math.Max (HorizontalScroll.Minimum, h); // was 0

        //
        try
        {
            if (VerticalScroll.Visible || !ShowScrollBars)
            {
                VerticalScroll.Value = Math.Min (v, VerticalScroll.Maximum);
            }

            if (HorizontalScroll.Visible || !ShowScrollBars)
            {
                HorizontalScroll.Value = Math.Min (h, HorizontalScroll.Maximum);
            }
        }
        catch (ArgumentOutOfRangeException exception)
        {
            Debug.WriteLine (exception.Message);
        }

        UpdateScrollbars();

        //
        RestoreHints();

        //
        if (oldV != VerticalScroll.Value)
        {
            OnVisibleRangeChanged();
        }
    }

    /// <summary>
    /// Updates scrollbar position after Value changed
    /// </summary>
    public void UpdateScrollbars()
    {
        if (ShowScrollBars)
        {
            OnMagicUpdateScrollBars();
        }
        else
        {
            PerformLayout();
        }

        if (IsHandleCreated)
        {
            BeginInvoke ((MethodInvoker)OnScrollbarsUpdated);
        }
    }

    private void OnMagicUpdateScrollBars()
    {
        if (this.InvokeRequired)
        {
            Invoke (new MethodInvoker (OnMagicUpdateScrollBars));
        }
        else
        {
            base.AutoScrollMinSize -= new Size (1, 0);
            base.AutoScrollMinSize += new Size (1, 0);
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnScrollbarsUpdated()
    {
        ScrollbarsUpdated?.Invoke (this, EventArgs.Empty);
    }

    /// <summary>
    /// Scroll control for display caret
    /// </summary>
    public void DoCaretVisible()
    {
        Invalidate();
        Recalc();
        var car = PlaceToPoint (Selection.Start);
        car.Offset (-CharWidth, 0);
        DoVisibleRectangle (new Rectangle (car, new Size (2 * CharWidth, 2 * CharHeight)));
    }

    /// <summary>
    /// Scroll control left
    /// </summary>
    public void ScrollLeft()
    {
        Invalidate();
        HorizontalScroll.Value = 0;
        AutoScrollMinSize -= new Size (1, 0);
        AutoScrollMinSize += new Size (1, 0);
    }

    /// <summary>
    /// Scroll control for display selection area
    /// </summary>
    public void DoSelectionVisible()
    {
        if (LineInfos[Selection.End.Line].VisibleState != VisibleState.Visible)
        {
            ExpandBlock (Selection.End.Line);
        }

        if (LineInfos[Selection.Start.Line].VisibleState != VisibleState.Visible)
        {
            ExpandBlock (Selection.Start.Line);
        }

        Recalc();
        DoVisibleRectangle (new Rectangle (PlaceToPoint (new Place (0, Selection.End.Line)),
            new Size (2 * CharWidth, 2 * CharHeight)));

        var car = PlaceToPoint (Selection.Start);
        var car2 = PlaceToPoint (Selection.End);
        car.Offset (-CharWidth, -ClientSize.Height / 2);
        DoVisibleRectangle (new Rectangle (car, new Size (Math.Abs (car2.X - car.X), ClientSize.Height)));

        //Math.Abs(car2.Y-car.Y) + 2 * CharHeight

        Invalidate();
    }

    /// <summary>
    /// Scroll control for display given range
    /// </summary>
    public void DoRangeVisible (TextRange range)
    {
        DoRangeVisible (range, false);
    }

    /// <summary>
    /// Scroll control for display given range
    /// </summary>
    public void DoRangeVisible (TextRange range, bool tryToCentre)
    {
        range = range.Clone();
        range.Normalize();
        range.End = new Place (range.End.Column,
            Math.Min (range.End.Line, range.Start.Line + ClientSize.Height / CharHeight));

        if (LineInfos[range.End.Line].VisibleState != VisibleState.Visible)
        {
            ExpandBlock (range.End.Line);
        }

        if (LineInfos[range.Start.Line].VisibleState != VisibleState.Visible)
        {
            ExpandBlock (range.Start.Line);
        }

        Recalc();
        var h = (1 + range.End.Line - range.Start.Line) * CharHeight;
        var p = PlaceToPoint (new Place (0, range.Start.Line));
        if (tryToCentre)
        {
            p.Offset (0, -ClientSize.Height / 2);
            h = ClientSize.Height;
        }

        DoVisibleRectangle (new Rectangle (p, new Size (2 * CharWidth, h)));

        Invalidate();
    }


    /// <inheritdoc cref="Control.OnKeyUp"/>
    protected override void OnKeyUp
        (
            KeyEventArgs eventArgs
        )
    {
        base.OnKeyUp (eventArgs);

        switch (eventArgs.KeyCode)
        {
            case Keys.ShiftKey:
                lastModifiers &= ~Keys.Shift;
                break;

            case Keys.Alt:
                lastModifiers &= ~Keys.Alt;
                break;

            case Keys.ControlKey:
                lastModifiers &= ~Keys.Control;
                break;
        }
    }

    private bool findCharMode;

    /// <inheritdoc cref="Control.OnKeyDown"/>
    protected override void OnKeyDown
        (
            KeyEventArgs eventArgs
        )
    {
        if (_middleClickScrollingActivated)
        {
            return;
        }

        base.OnKeyDown (eventArgs);

        if (Focused) //???
        {
            lastModifiers = eventArgs.Modifiers;
        }

        _handledChar = false;

        if (eventArgs.Handled)
        {
            _handledChar = true;
            return;
        }

        if (ProcessKey (eventArgs.KeyData))
        {
            return;
        }

        eventArgs.Handled = true;

        DoCaretVisible();
        Invalidate();
    }

    /// <inheritdoc cref="ContainerControl.ProcessDialogKey"/>
    protected override bool ProcessDialogKey (Keys keyData)
    {
        if ((keyData & Keys.Alt) > 0 && HotkeyMapping.ContainsKey (keyData))
        {
            ProcessKey (keyData);
            return true;
        }

        return base.ProcessDialogKey (keyData);
    }

    private static readonly Dictionary<ActionCode, bool> scrollActions = new ()
    {
        { ActionCode.ScrollDown, true }, { ActionCode.ScrollUp, true }, { ActionCode.ZoomOut, true },
        { ActionCode.ZoomIn, true }, { ActionCode.ZoomNormal, true }
    };

    /// <summary>
    /// Process control keys
    /// </summary>
    public virtual bool ProcessKey (Keys keyData)
    {
        var a = new KeyEventArgs (keyData);

        if (a.KeyCode == Keys.Tab && !AcceptsTab)
        {
            return false;
        }


        if (_macroManager != null!)
        {
            if (!HotkeyMapping.ContainsKey (keyData) || HotkeyMapping[keyData] != ActionCode.MacroExecute &&
                HotkeyMapping[keyData] != ActionCode.MacroRecord)
            {
                _macroManager.ProcessKey (keyData);
            }
        }

        if (HotkeyMapping.ContainsKey (keyData))
        {
            var act = HotkeyMapping[keyData];
            DoAction (act);
            if (scrollActions.ContainsKey (act))
            {
                return true;
            }

            if (keyData == Keys.Tab || keyData == (Keys.Tab | Keys.Shift))
            {
                _handledChar = true;
                return true;
            }
        }
        else
        {
            //
            if (a.KeyCode == Keys.Alt)
            {
                return true;
            }

            if ((a.Modifiers & Keys.Control) != 0)
            {
                return true;
            }

            if ((a.Modifiers & Keys.Alt) != 0)
            {
                if ((MouseButtons & MouseButtons.Left) != 0)
                {
                    CheckAndChangeSelectionType();
                }

                return true;
            }

            if (a.KeyCode == Keys.ShiftKey)
            {
                return true;
            }
        }

        return false;
    }

    private void DoAction (ActionCode action)
    {
        switch (action)
        {
            case ActionCode.ZoomIn:
                ChangeFontSize (2);
                break;

            case ActionCode.ZoomOut:
                ChangeFontSize (-2);
                break;

            case ActionCode.ZoomNormal:
                RestoreFontSize();
                break;

            case ActionCode.ScrollDown:
                DoScrollVertical (1, -1);
                break;

            case ActionCode.ScrollUp:
                DoScrollVertical (1, 1);
                break;

            case ActionCode.GoToDialog:
                ShowGoToDialog();
                break;

            case ActionCode.FindDialog:
                ShowFindDialog();
                break;

            case ActionCode.FindChar:
                findCharMode = true;
                break;

            case ActionCode.FindNext:
                if (findForm == null! || findForm.tbFind.Text == "")
                {
                    ShowFindDialog();
                }
                else
                {
                    findForm.FindNext (findForm.tbFind.Text);
                }

                break;

            case ActionCode.ReplaceDialog:
                ShowReplaceDialog();
                break;

            case ActionCode.Copy:
                Copy();
                break;

            case ActionCode.CommentSelected:
                CommentSelected();
                break;

            case ActionCode.Cut:
                if (!Selection.ReadOnly)
                {
                    Cut();
                }

                break;

            case ActionCode.Paste:
                if (!Selection.ReadOnly)
                {
                    Paste();
                }

                break;

            case ActionCode.SelectAll:
                Selection.SelectAll();
                break;

            case ActionCode.Undo:
                if (!ReadOnly)
                {
                    Undo();
                }

                break;

            case ActionCode.Redo:
                if (!ReadOnly)
                {
                    Redo();
                }

                break;

            case ActionCode.LowerCase:
                if (!Selection.ReadOnly)
                {
                    LowerCase();
                }

                break;

            case ActionCode.UpperCase:
                if (!Selection.ReadOnly)
                {
                    UpperCase();
                }

                break;

            case ActionCode.IndentDecrease:
                if (!Selection.ReadOnly)
                {
                    var sel = Selection.Clone();
                    if (sel.Start.Line == sel.End.Line)
                    {
                        var line = this[sel.Start.Line];
                        if (sel.Start.Column == 0 && sel.End.Column == line.Count)
                        {
                            Selection = new TextRange (this, line.StartSpacesCount, sel.Start.Line, line.Count,
                                sel.Start.Line);
                        }
                        else if (sel.Start.Column == line.Count && sel.End.Column == 0)
                        {
                            Selection = new TextRange (this, line.Count, sel.Start.Line, line.StartSpacesCount,
                                sel.Start.Line);
                        }
                    }


                    DecreaseIndent();
                }

                break;

            case ActionCode.IndentIncrease:
                if (!Selection.ReadOnly)
                {
                    var sel = Selection.Clone();
                    var inverted = sel.Start > sel.End;
                    sel.Normalize();
                    var spaces = this[sel.Start.Line].StartSpacesCount;
                    if (sel.Start.Line != sel.End.Line || //selected several lines
                        sel.Start.Column <= spaces &&
                        sel.End.Column == this[sel.Start.Line].Count || //selected whole line
                        sel.End.Column <= spaces) //selected space prefix
                    {
                        IncreaseIndent();
                        if (sel.Start.Line == sel.End.Line && !sel.IsEmpty)
                        {
                            Selection = new TextRange (this, this[sel.Start.Line].StartSpacesCount, sel.End.Line,
                                this[sel.Start.Line].Count, sel.End.Line); //select whole line
                            if (inverted)
                            {
                                Selection.Inverse();
                            }
                        }
                    }
                    else
                    {
                        ProcessKey ('\t', Keys.None);
                    }
                }

                break;

            case ActionCode.AutoIndentChars:
                if (!Selection.ReadOnly)
                {
                    DoAutoIndentChars (Selection.Start.Line);
                }

                break;

            case ActionCode.NavigateBackward:
                NavigateBackward();
                break;

            case ActionCode.NavigateForward:
                NavigateForward();
                break;

            case ActionCode.UnbookmarkLine:
                UnbookmarkLine (Selection.Start.Line);
                break;

            case ActionCode.BookmarkLine:
                BookmarkLine (Selection.Start.Line);
                break;

            case ActionCode.GoNextBookmark:
                GotoNextBookmark (Selection.Start.Line);
                break;

            case ActionCode.GoPrevBookmark:
                GotoPrevBookmark (Selection.Start.Line);
                break;

            case ActionCode.ClearWordLeft:
                if (OnKeyPressing ('\b')) //KeyPress event processed key
                {
                    break;
                }

                if (!Selection.ReadOnly)
                {
                    if (!Selection.IsEmpty)
                    {
                        ClearSelected();
                    }

                    Selection.GoWordLeft (true);
                    if (!Selection.ReadOnly)
                    {
                        ClearSelected();
                    }
                }

                OnKeyPressed ('\b');
                break;

            case ActionCode.ReplaceMode:
                if (!ReadOnly)
                {
                    isReplaceMode = !isReplaceMode;
                }

                break;

            case ActionCode.DeleteCharRight:
                if (!Selection.ReadOnly)
                {
                    if (OnKeyPressing ((char)0xff)) //KeyPress event processed key
                    {
                        break;
                    }

                    if (!Selection.IsEmpty)
                    {
                        ClearSelected();
                    }
                    else
                    {
                        //if line contains only spaces then delete line
                        if (this[Selection.Start.Line].StartSpacesCount == this[Selection.Start.Line].Count)
                        {
                            RemoveSpacesAfterCaret();
                        }

                        if (!Selection.IsReadOnlyRightChar())
                        {
                            if (Selection.GoRightThroughFolded())
                            {
                                var iLine = Selection.Start.Line;

                                InsertChar ('\b');

                                //if removed \n then trim spaces
                                if (iLine != Selection.Start.Line && AutoIndent)
                                {
                                    if (Selection.Start.Column > 0)
                                    {
                                        RemoveSpacesAfterCaret();
                                    }
                                }
                            }
                        }
                    }

                    if (AutoIndentChars)
                    {
                        DoAutoIndentChars (Selection.Start.Line);
                    }

                    OnKeyPressed ((char)0xff);
                }

                break;

            case ActionCode.ClearWordRight:
                if (OnKeyPressing ((char)0xff)) //KeyPress event processed key
                {
                    break;
                }

                if (!Selection.ReadOnly)
                {
                    if (!Selection.IsEmpty)
                    {
                        ClearSelected();
                    }

                    Selection.GoWordRight (true);
                    if (!Selection.ReadOnly)
                    {
                        ClearSelected();
                    }
                }

                OnKeyPressed ((char)0xff);
                break;

            case ActionCode.GoWordLeft:
                Selection.GoWordLeft (false);
                break;

            case ActionCode.GoWordLeftWithSelection:
                Selection.GoWordLeft (true);
                break;

            case ActionCode.GoLeft:
                Selection.GoLeft (false);
                break;

            case ActionCode.GoLeftWithSelection:
                Selection.GoLeft (true);
                break;

            case ActionCode.GoLeftColumnSelectionMode:
                CheckAndChangeSelectionType();
                if (Selection.ColumnSelectionMode)
                {
                    Selection.GoLeft_ColumnSelectionMode();
                }

                Invalidate();
                break;

            case ActionCode.GoWordRight:
                Selection.GoWordRight (false, true);
                break;

            case ActionCode.GoWordRightWithSelection:
                Selection.GoWordRight (true, true);
                break;

            case ActionCode.GoRight:
                Selection.GoRight (false);
                break;

            case ActionCode.GoRightWithSelection:
                Selection.GoRight (true);
                break;

            case ActionCode.GoRightColumnSelectionMode:
                CheckAndChangeSelectionType();
                if (Selection.ColumnSelectionMode)
                {
                    Selection.GoRight_ColumnSelectionMode();
                }

                Invalidate();
                break;

            case ActionCode.GoUp:
                Selection.GoUp (false);
                ScrollLeft();
                break;

            case ActionCode.GoUpWithSelection:
                Selection.GoUp (true);
                ScrollLeft();
                break;

            case ActionCode.GoUpColumnSelectionMode:
                CheckAndChangeSelectionType();
                if (Selection.ColumnSelectionMode)
                {
                    Selection.GoUp_ColumnSelectionMode();
                }

                Invalidate();
                break;

            case ActionCode.MoveSelectedLinesUp:
                if (!Selection.ColumnSelectionMode)
                {
                    MoveSelectedLinesUp();
                }

                break;

            case ActionCode.GoDown:
                Selection.GoDown (false);
                ScrollLeft();
                break;

            case ActionCode.GoDownWithSelection:
                Selection.GoDown (true);
                ScrollLeft();
                break;

            case ActionCode.GoDownColumnSelectionMode:
                CheckAndChangeSelectionType();
                if (Selection.ColumnSelectionMode)
                {
                    Selection.GoDown_ColumnSelectionMode();
                }

                Invalidate();
                break;

            case ActionCode.MoveSelectedLinesDown:
                if (!Selection.ColumnSelectionMode)
                {
                    MoveSelectedLinesDown();
                }

                break;
            case ActionCode.GoPageUp:
                Selection.GoPageUp (false);
                ScrollLeft();
                break;

            case ActionCode.GoPageUpWithSelection:
                Selection.GoPageUp (true);
                ScrollLeft();
                break;

            case ActionCode.GoPageDown:
                Selection.GoPageDown (false);
                ScrollLeft();
                break;

            case ActionCode.GoPageDownWithSelection:
                Selection.GoPageDown (true);
                ScrollLeft();
                break;

            case ActionCode.GoFirstLine:
                Selection.GoFirst (false);
                break;

            case ActionCode.GoFirstLineWithSelection:
                Selection.GoFirst (true);
                break;

            case ActionCode.GoHome:
                GoHome (false);
                ScrollLeft();
                break;

            case ActionCode.GoHomeWithSelection:
                GoHome (true);
                ScrollLeft();
                break;

            case ActionCode.GoLastLine:
                Selection.GoLast (false);
                break;

            case ActionCode.GoLastLineWithSelection:
                Selection.GoLast (true);
                break;

            case ActionCode.GoEnd:
                Selection.GoEnd (false);
                break;

            case ActionCode.GoEndWithSelection:
                Selection.GoEnd (true);
                break;

            case ActionCode.ClearHints:
                ClearHints();
                if (MacroManager != null!)
                {
                    MacroManager.IsRecording = false;
                }

                break;

            case ActionCode.MacroRecord:
                if (MacroManager != null!)
                {
                    if (MacroManager.AllowMacroRecordingByUser)
                    {
                        MacroManager.IsRecording = !MacroManager.IsRecording;
                    }

                    if (MacroManager.IsRecording)
                    {
                        MacroManager.ClearMacros();
                    }
                }

                break;

            case ActionCode.MacroExecute:
                if (MacroManager != null!)
                {
                    MacroManager.IsRecording = false;
                    MacroManager.ExecuteMacros();
                }

                break;
            case ActionCode.CustomAction1:
            case ActionCode.CustomAction2:
            case ActionCode.CustomAction3:
            case ActionCode.CustomAction4:
            case ActionCode.CustomAction5:
            case ActionCode.CustomAction6:
            case ActionCode.CustomAction7:
            case ActionCode.CustomAction8:
            case ActionCode.CustomAction9:
            case ActionCode.CustomAction10:
            case ActionCode.CustomAction11:
            case ActionCode.CustomAction12:
            case ActionCode.CustomAction13:
            case ActionCode.CustomAction14:
            case ActionCode.CustomAction15:
            case ActionCode.CustomAction16:
            case ActionCode.CustomAction17:
            case ActionCode.CustomAction18:
            case ActionCode.CustomAction19:
            case ActionCode.CustomAction20:
                OnCustomAction (new CustomActionEventArgs (action));
                break;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="eventArgs"></param>
    protected virtual void OnCustomAction (CustomActionEventArgs eventArgs)
    {
        CustomAction?.Invoke (this, eventArgs);
    }

    private Font originalFont;

    private void RestoreFontSize()
    {
        Zoom = 100;
    }

    /// <summary>
    /// Scrolls to nearest bookmark or to first bookmark
    /// </summary>
    /// <param name="iLine">Current bookmark line index</param>
    public bool GotoNextBookmark (int iLine)
    {
        Bookmark? nearestBookmark = null;
        var minNextLineIndex = int.MaxValue;
        Bookmark? minBookmark = null;
        var minLineIndex = int.MaxValue;
        foreach (var bookmark in bookmarks)
        {
            if (bookmark.LineIndex < minLineIndex)
            {
                minLineIndex = bookmark.LineIndex;
                minBookmark = bookmark;
            }

            if (bookmark.LineIndex > iLine && bookmark.LineIndex < minNextLineIndex)
            {
                minNextLineIndex = bookmark.LineIndex;
                nearestBookmark = bookmark;
            }
        }

        if (nearestBookmark != null)
        {
            nearestBookmark.DoVisible();
            return true;
        }
        else if (minBookmark != null)
        {
            minBookmark.DoVisible();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Scrolls to nearest previous bookmark or to last bookmark
    /// </summary>
    /// <param name="iLine">Current bookmark line index</param>
    public bool GotoPrevBookmark (int iLine)
    {
        Bookmark? nearestBookmark = null;
        var maxPrevLineIndex = -1;
        Bookmark? maxBookmark = null;
        var maxLineIndex = -1;
        foreach (var bookmark in bookmarks)
        {
            if (bookmark.LineIndex > maxLineIndex)
            {
                maxLineIndex = bookmark.LineIndex;
                maxBookmark = bookmark;
            }

            if (bookmark.LineIndex < iLine && bookmark.LineIndex > maxPrevLineIndex)
            {
                maxPrevLineIndex = bookmark.LineIndex;
                nearestBookmark = bookmark;
            }
        }

        if (nearestBookmark != null)
        {
            nearestBookmark.DoVisible();
            return true;
        }
        else if (maxBookmark != null)
        {
            maxBookmark.DoVisible();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Bookmarks line
    /// </summary>
    public virtual void BookmarkLine (int iLine)
    {
        if (!bookmarks.Contains (iLine))
        {
            bookmarks.Add (iLine);
        }
    }

    /// <summary>
    /// Unbookmarks current line
    /// </summary>
    public virtual void UnbookmarkLine (int iLine)
    {
        bookmarks.Remove (iLine);
    }

    /// <summary>
    /// Moves selected lines down
    /// </summary>
    public virtual void MoveSelectedLinesDown()
    {
        var prevSelection = Selection.Clone();
        Selection.Expand();
        if (!Selection.ReadOnly)
        {
            var iLine = Selection.Start.Line;
            if (Selection.End.Line >= LinesCount - 1)
            {
                Selection = prevSelection;
                return;
            }

            var text = SelectedText;
            var temp = new List<int>();
            for (var i = Selection.Start.Line; i <= Selection.End.Line; i++)
            {
                temp.Add (i);
            }

            RemoveLines (temp);
            Selection.Start = new Place (GetLineLength (iLine), iLine);
            SelectedText = "\n" + text;
            Selection.Start = new Place (prevSelection.Start.Column, prevSelection.Start.Line + 1);
            Selection.End = new Place (prevSelection.End.Column, prevSelection.End.Line + 1);
        }
        else
        {
            Selection = prevSelection;
        }
    }

    /// <summary>
    /// Moves selected lines up
    /// </summary>
    public virtual void MoveSelectedLinesUp()
    {
        var prevSelection = Selection.Clone();
        Selection.Expand();
        if (!Selection.ReadOnly)
        {
            var iLine = Selection.Start.Line;
            if (iLine == 0)
            {
                Selection = prevSelection;
                return;
            }

            var text = SelectedText;
            var temp = new List<int>();
            for (var i = Selection.Start.Line; i <= Selection.End.Line; i++)
            {
                temp.Add (i);
            }

            RemoveLines (temp);
            Selection.Start = new Place (0, iLine - 1);
            SelectedText = text + "\n";
            Selection.Start = new Place (prevSelection.Start.Column, prevSelection.Start.Line - 1);
            Selection.End = new Place (prevSelection.End.Column, prevSelection.End.Line - 1);
        }
        else
        {
            Selection = prevSelection;
        }
    }

    private void GoHome (bool shift)
    {
        Selection.BeginUpdate();
        try
        {
            var iLine = Selection.Start.Line;
            var spaces = this[iLine].StartSpacesCount;
            if (Selection.Start.Column <= spaces)
            {
                Selection.GoHome (shift);
            }
            else
            {
                Selection.GoHome (shift);
                for (var i = 0; i < spaces; i++)
                {
                    Selection.GoRight (shift);
                }
            }
        }
        finally
        {
            Selection.EndUpdate();
        }
    }

    /// <summary>
    /// Convert selected text to upper case
    /// </summary>
    public virtual void UpperCase()
    {
        var old = Selection.Clone();
        SelectedText = SelectedText.ToUpper();
        Selection.Start = old.Start;
        Selection.End = old.End;
    }

    /// <summary>
    /// Convert selected text to lower case
    /// </summary>
    public virtual void LowerCase()
    {
        var old = Selection.Clone();
        SelectedText = SelectedText.ToLower();
        Selection.Start = old.Start;
        Selection.End = old.End;
    }

    /// <summary>
    /// Convert selected text to title case
    /// </summary>
    public virtual void TitleCase()
    {
        var old = Selection.Clone();
        SelectedText = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase (SelectedText.ToLower());
        Selection.Start = old.Start;
        Selection.End = old.End;
    }

    /// <summary>
    /// Convert selected text to sentence case
    /// </summary>
    public virtual void SentenceCase()
    {
        var old = Selection.Clone();
        var lowerCase = SelectedText.ToLower();
        var r = new Regex (@"(^\S)|[\.\?!:]\s+(\S)", RegexOptions.ExplicitCapture);
        SelectedText = r.Replace (lowerCase, s => s.Value.ToUpper());
        Selection.Start = old.Start;
        Selection.End = old.End;
    }

    /// <summary>
    /// Insert/remove comment prefix into selected lines
    /// </summary>
    public void CommentSelected()
    {
        CommentSelected (CommentPrefix);
    }

    /// <summary>
    /// Insert/remove comment prefix into selected lines
    /// </summary>
    public virtual void CommentSelected (string commentPrefix)
    {
        if (string.IsNullOrEmpty (commentPrefix))
        {
            return;
        }

        Selection.Normalize();
        var isCommented = lines[Selection.Start.Line].Text.TrimStart().StartsWith (commentPrefix);
        if (isCommented)
        {
            RemoveLinePrefix (commentPrefix);
        }
        else
        {
            InsertLinePrefix (commentPrefix);
        }
    }

    /// <summary>
    /// /
    /// </summary>
    /// <param name="args"></param>
    public void OnKeyPressing (KeyPressEventArgs args)
    {
        KeyPressing?.Invoke (this, args);
    }

    private bool OnKeyPressing (char c)
    {
        if (findCharMode)
        {
            findCharMode = false;
            FindChar (c);
            return true;
        }

        var args = new KeyPressEventArgs (c);
        OnKeyPressing (args);
        return args.Handled;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="c"></param>
    public void OnKeyPressed (char c)
    {
        var args = new KeyPressEventArgs (c);
        KeyPressed?.Invoke (this, args);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="charCode"></param>
    /// <returns></returns>
    protected override bool ProcessMnemonic (char charCode)
    {
        if (_middleClickScrollingActivated)
        {
            return false;
        }

        if (Focused)
        {
            return ProcessKey (charCode, lastModifiers) || base.ProcessMnemonic (charCode);
        }
        else
        {
            return false;
        }
    }

    private const int WM_CHAR = 0x102;

    /// <summary>
    ///
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    protected override bool ProcessKeyMessage (ref Message m)
    {
        if (m.Msg == WM_CHAR)
        {
            ProcessMnemonic (Convert.ToChar (m.WParam.ToInt32()));
        }

        return base.ProcessKeyMessage (ref m);
    }

    /// <summary>
    /// Process "real" keys (no control)
    /// </summary>
    public virtual bool ProcessKey (char c, Keys modifiers)
    {
        if (_handledChar)
        {
            return true;
        }

        _macroManager?.ProcessKey (c, modifiers);
        /*  !!!!
        if (c == ' ')
            return true;*/

        //backspace
        if (c == '\b' && (modifiers is Keys.None or Keys.Shift || (modifiers & Keys.Alt) != 0))
        {
            if (ReadOnly || !Enabled)
            {
                return false;
            }

            if (OnKeyPressing (c))
            {
                return true;
            }

            if (Selection.ReadOnly)
            {
                return false;
            }

            if (!Selection.IsEmpty)
            {
                ClearSelected();
            }
            else if (!Selection.IsReadOnlyLeftChar()) //is not left char readonly?
            {
                InsertChar ('\b');
            }

            if (AutoIndentChars)
            {
                DoAutoIndentChars (Selection.Start.Line);
            }

            OnKeyPressed ('\b');
            return true;
        }

        /* !!!!
        if (c == '\b' && (modifiers & Keys.Alt) != 0)
            return true;*/

        if (char.IsControl (c) && c != '\r' && c != '\t')
        {
            return false;
        }

        if (ReadOnly || !Enabled)
        {
            return false;
        }


        if (modifiers != Keys.None &&
            modifiers != Keys.Shift &&
            modifiers != (Keys.Control | Keys.Alt) && //ALT+CTRL is special chars (AltGr)
            modifiers != (Keys.Shift | Keys.Control | Keys.Alt) && //SHIFT + ALT + CTRL is special chars (AltGr)
            (modifiers != Keys.Alt || char.IsLetterOrDigit (c)) //may be ALT+LetterOrDigit is mnemonic code
           )
        {
            return false; //do not process Ctrl+? and Alt+? keys
        }

        var sourceC = c;
        if (OnKeyPressing (sourceC)) //KeyPress event processed key
        {
            return true;
        }

        //
        if (Selection.ReadOnly)
        {
            return false;
        }

        //
        if (c == '\r' && !AcceptsReturn)
        {
            return false;
        }

        //replace \r on \n
        if (c == '\r')
        {
            c = '\n';
        }

        //replace mode? select forward char
        if (IsReplaceMode)
        {
            Selection.GoRight (true);
            Selection.Inverse();
        }

        //insert char
        if (!Selection.ReadOnly)
        {
            if (!DoAutocompleteBrackets (c))
            {
                InsertChar (c);
            }
        }

        //do autoindent
        if (c == '\n' || AutoIndentExistingLines)
        {
            DoAutoIndentIfNeed();
        }

        if (AutoIndentChars)
        {
            DoAutoIndentChars (Selection.Start.Line);
        }

        DoCaretVisible();
        Invalidate();

        OnKeyPressed (sourceC);

        return true;
    }

    #region AutoIndentChars

    /// <summary>
    /// Enables AutoIndentChars mode
    /// </summary>
    [Description ("Enables AutoIndentChars mode")]
    [DefaultValue (true)]
    public bool AutoIndentChars { get; set; }

    /// <summary>
    /// Regex patterns for AutoIndentChars (one regex per line)
    /// </summary>
    [Description ("Regex patterns for AutoIndentChars (one regex per line)")]
    [Editor ("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
        typeof (UITypeEditor))]
    [DefaultValue (@"^\s*[\w\.]+\s*(?<range>=)\s*(?<range>[^;]+);")]
    public string AutoIndentCharsPatterns { get; set; }

    /// <summary>
    /// Do AutoIndentChars
    /// </summary>
    public void DoAutoIndentChars (int iLine)
    {
        var patterns = AutoIndentCharsPatterns.Split (new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var pattern in patterns)
        {
            var m = Regex.Match (this[iLine].Text, pattern);
            if (m.Success)
            {
                DoAutoIndentChars (iLine, new Regex (pattern));
                break;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="iLine"></param>
    /// <param name="regex"></param>
    protected void DoAutoIndentChars (int iLine, Regex regex)
    {
        var oldSel = Selection.Clone();

        var captures = new SortedDictionary<int, CaptureCollection>();
        var texts = new SortedDictionary<int, string>();
        var maxCapturesCount = 0;

        var spaces = this[iLine].StartSpacesCount;

        for (var i = iLine; i >= 0; i--)
        {
            if (spaces != this[i].StartSpacesCount)
            {
                break;
            }

            var text = this[i].Text;
            var m = regex.Match (text);
            if (m.Success)
            {
                captures[i] = m.Groups["range"].Captures;
                texts[i] = text;

                if (captures[i].Count > maxCapturesCount)
                {
                    maxCapturesCount = captures[i].Count;
                }
            }
            else
            {
                break;
            }
        }

        for (var i = iLine + 1; i < LinesCount; i++)
        {
            if (spaces != this[i].StartSpacesCount)
            {
                break;
            }

            var text = this[i].Text;
            var m = regex.Match (text);
            if (m.Success)
            {
                captures[i] = m.Groups["range"].Captures;
                texts[i] = text;

                if (captures[i].Count > maxCapturesCount)
                {
                    maxCapturesCount = captures[i].Count;
                }
            }
            else
            {
                break;
            }
        }

        var changed = new Dictionary<int, bool>();
        var was = false;

        for (var iCapture = maxCapturesCount - 1; iCapture >= 0; iCapture--)
        {
            //find max dist
            var maxDist = 0;
            foreach (var i in captures.Keys)
            {
                var caps = captures[i];
                if (caps.Count <= iCapture)
                {
                    continue;
                }

                var dist = 0;
                var cap = caps[iCapture];

                var index = cap.Index;

                var text = texts[i];
                while (index > 0 && text[index - 1] == ' ') index--;

                if (iCapture == 0)
                {
                    dist = index;
                }
                else
                {
                    dist = index - caps[iCapture - 1].Index - 1;
                }

                if (dist > maxDist)
                {
                    maxDist = dist;
                }
            }

            //insert whitespaces
            foreach (var i in new List<int> (texts.Keys))
            {
                if (captures[i].Count <= iCapture)
                {
                    continue;
                }

                var dist = 0;
                var cap = captures[i][iCapture];

                if (iCapture == 0)
                {
                    dist = cap.Index;
                }
                else
                {
                    dist = cap.Index - captures[i][iCapture - 1].Index - 1;
                }

                var addSpaces = maxDist - dist + 1; //+1 because min space count is 1

                if (addSpaces == 0)
                {
                    continue;
                }

                if (oldSel.Start.Line == i && oldSel.Start.Column > cap.Index)
                {
                    oldSel.Start = new Place (oldSel.Start.Column + addSpaces, i);
                }

                if (addSpaces > 0)
                {
                    texts[i] = texts[i].Insert (cap.Index, new string (' ', addSpaces));
                }
                else
                {
                    texts[i] = texts[i].Remove (cap.Index + addSpaces, -addSpaces);
                }

                changed[i] = true;
                was = true;
            }
        }

        //insert text
        if (was)
        {
            Selection.BeginUpdate();
            BeginAutoUndo();
            BeginUpdate();

            TextSource.Manager.ExecuteCommand (new SelectCommand (TextSource));

            foreach (var i in texts.Keys)
            {
                if (changed.ContainsKey (i))
                {
                    Selection = new TextRange (this, 0, i, this[i].Count, i);
                    if (!Selection.ReadOnly)
                    {
                        InsertText (texts[i]);
                    }
                }
            }

            Selection = oldSel;

            EndUpdate();
            EndAutoUndo();
            Selection.EndUpdate();
        }
    }

    #endregion

    private bool DoAutocompleteBrackets (char c)
    {
        if (AutoCompleteBrackets)
        {
            if (!Selection.ColumnSelectionMode)
            {
                for (var i = 1; i < _autoCompleteBracketsList.Length; i += 2)
                {
                    if (c == _autoCompleteBracketsList[i] && c == Selection.CharAfterStart)
                    {
                        Selection.GoRight();
                        return true;
                    }
                }
            }

            for (var i = 0; i < _autoCompleteBracketsList.Length; i += 2)
            {
                if (c == _autoCompleteBracketsList[i])
                {
                    InsertBrackets (_autoCompleteBracketsList[i], _autoCompleteBracketsList[i + 1]);
                    return true;
                }
            }
        }

        return false;
    }

    private bool InsertBrackets (char left, char right)
    {
        if (Selection.ColumnSelectionMode)
        {
            var range = Selection.Clone();
            range.Normalize();
            Selection.BeginUpdate();
            BeginAutoUndo();
            Selection = new TextRange (this, range.Start.Column, range.Start.Line, range.Start.Column, range.End.Line)
                { ColumnSelectionMode = true };
            InsertChar (left);
            Selection = new TextRange (this, range.End.Column + 1, range.Start.Line, range.End.Column + 1, range.End.Line)
                { ColumnSelectionMode = true };
            InsertChar (right);
            if (range.IsEmpty)
            {
                Selection = new TextRange (this, range.End.Column + 1, range.Start.Line, range.End.Column + 1,
                    range.End.Line) { ColumnSelectionMode = true };
            }

            EndAutoUndo();
            Selection.EndUpdate();
        }
        else if (Selection.IsEmpty)
        {
            InsertText (left + "" + right);
            Selection.GoLeft();
        }
        else
        {
            InsertText (left + SelectedText + right);
        }

        return true;
    }

    /// <summary>
    /// Finds given char after current caret position, moves the caret to found pos.
    /// </summary>
    /// <param name="c"></param>
    protected virtual void FindChar (char c)
    {
        if (c == '\r')
        {
            c = '\n';
        }

        var r = Selection.Clone();
        while (r.GoRight())
        {
            if (r.CharBeforeStart == c)
            {
                Selection = r;
                DoCaretVisible();
                return;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public virtual void DoAutoIndentIfNeed()
    {
        if (Selection.ColumnSelectionMode)
        {
            return;
        }

        if (AutoIndent)
        {
            DoCaretVisible();
            var needSpaces = CalcAutoIndent (Selection.Start.Line);
            if (this[Selection.Start.Line].AutoIndentSpacesNeededCount != needSpaces)
            {
                DoAutoIndent (Selection.Start.Line);
                this[Selection.Start.Line].AutoIndentSpacesNeededCount = needSpaces;
            }
        }
    }

    private void RemoveSpacesAfterCaret()
    {
        if (!Selection.IsEmpty)
        {
            return;
        }

        var end = Selection.Start;
        while (Selection.CharAfterStart == ' ')
            Selection.GoRight (true);
        ClearSelected();
    }

    /// <summary>
    /// Inserts autoindent's spaces in the line
    /// </summary>
    public virtual void DoAutoIndent (int iLine)
    {
        if (Selection.ColumnSelectionMode)
        {
            return;
        }

        var oldStart = Selection.Start;

        //
        var needSpaces = CalcAutoIndent (iLine);

        //
        var spaces = lines[iLine].StartSpacesCount;
        var needToInsert = needSpaces - spaces;
        if (needToInsert < 0)
        {
            needToInsert = -Math.Min (-needToInsert, spaces);
        }

        //insert start spaces
        if (needToInsert == 0)
        {
            return;
        }

        Selection.Start = new Place (0, iLine);
        if (needToInsert > 0)
        {
            InsertText (new String (' ', needToInsert));
        }
        else
        {
            Selection.Start = new Place (0, iLine);
            Selection.End = new Place (-needToInsert, iLine);
            ClearSelected();
        }

        Selection.Start = new Place (Math.Min (lines[iLine].Count, Math.Max (0, oldStart.Column + needToInsert)), iLine);
    }

    /// <summary>
    /// Returns needed start space count for the line
    /// </summary>
    public virtual int CalcAutoIndent (int iLine)
    {
        if (iLine < 0 || iLine >= LinesCount)
        {
            return 0;
        }


        var calculator = AutoIndentNeeded;
        if (calculator == null)
        {
            if (Language != Language.Custom && SyntaxHighlighter != null!)
            {
                calculator = SyntaxHighlighter.AutoIndentNeeded!;
            }
            else
            {
                calculator = CalcAutoIndentShiftByCodeFolding!;
            }
        }

        var needSpaces = 0;

        var stack = new Stack<AutoIndentEventArgs>();

        //calc indent for previous lines, find stable line
        int i;
        for (i = iLine - 1; i >= 0; i--)
        {
            var args = new AutoIndentEventArgs (i, lines[i].Text, i > 0 ? lines[i - 1].Text : "", TabLength, 0);
            calculator (this, args);
            stack.Push (args);
            if (args.Shift == 0 && args.AbsoluteIndentation == 0 && args.LineText.Trim() != "")
            {
                break;
            }
        }

        var indent = lines[i >= 0 ? i : 0].StartSpacesCount;
        while (stack.Count != 0)
        {
            var arg = stack.Pop();
            if (arg.AbsoluteIndentation != 0)
            {
                indent = arg.AbsoluteIndentation + arg.ShiftNextLines;
            }
            else
            {
                indent += arg.ShiftNextLines;
            }
        }

        //clalc shift for current line
        var a = new AutoIndentEventArgs (iLine, lines[iLine].Text, iLine > 0 ? lines[iLine - 1].Text : "", TabLength,
            indent);
        calculator (this, a);
        needSpaces = a.AbsoluteIndentation + a.Shift;

        return needSpaces;
    }

    internal virtual void CalcAutoIndentShiftByCodeFolding (object sender, AutoIndentEventArgs args)
    {
        //inset TAB after start folding marker
        if (string.IsNullOrEmpty (lines[args.iLine].FoldingEndMarker) &&
            !string.IsNullOrEmpty (lines[args.iLine].FoldingStartMarker))
        {
            args.ShiftNextLines = TabLength;
            return;
        }

        //remove TAB before end folding marker
        if (!string.IsNullOrEmpty (lines[args.iLine].FoldingEndMarker) &&
            string.IsNullOrEmpty (lines[args.iLine].FoldingStartMarker))
        {
            args.Shift = -TabLength;
            args.ShiftNextLines = -TabLength;
        }
    }


    /// <summary>
    ///
    /// </summary>
    protected int GetMinStartSpacesCount
        (
            int fromLine,
            int toLine
        )
    {
        if (fromLine > toLine)
        {
            return 0;
        }

        var result = int.MaxValue;
        for (var i = fromLine; i <= toLine; i++)
        {
            var count = lines[i].StartSpacesCount;
            if (count < result)
            {
                result = count;
            }
        }

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    protected int GetMaxStartSpacesCount (int fromLine, int toLine)
    {
        if (fromLine > toLine)
        {
            return 0;
        }

        var result = 0;
        for (var i = fromLine; i <= toLine; i++)
        {
            var count = lines[i].StartSpacesCount;
            if (count > result)
            {
                result = count;
            }
        }

        return result;
    }

    /// <summary>
    /// Undo last operation
    /// </summary>
    public virtual void Undo()
    {
        lines.Manager.Undo();
        DoCaretVisible();
        Invalidate();
    }

    /// <summary>
    /// Redo
    /// </summary>
    public virtual void Redo()
    {
        lines.Manager.Redo();
        DoCaretVisible();
        Invalidate();
    }

    /// <inheritdoc cref="Control.IsInputKey"/>
    protected override bool IsInputKey (Keys keyData)
    {
        if ((keyData == Keys.Tab || keyData == (Keys.Shift | Keys.Tab)) && !AcceptsTab)
        {
            return false;
        }

        if (keyData == Keys.Enter && !AcceptsReturn)
        {
            return false;
        }

        if ((keyData & Keys.Alt) == Keys.None)
        {
            var keys = keyData & Keys.KeyCode;
            if (keys == Keys.Return)
            {
                return true;
            }
        }

        if ((keyData & Keys.Alt) != Keys.Alt)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Prior:
                case Keys.Next:
                case Keys.End:
                case Keys.Home:
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    return true;

                case Keys.Escape:
                    return false;

                case Keys.Tab:
                    return (keyData & Keys.Control) == Keys.None;
            }
        }

        return base.IsInputKey (keyData);
    }

    [DllImport ("User32.dll")]
    private static extern bool CreateCaret (IntPtr hWnd, int hBitmap, int nWidth, int nHeight);

    [DllImport ("User32.dll")]
    private static extern bool SetCaretPos (int x, int y);

    [DllImport ("User32.dll")]
    private static extern bool DestroyCaret();

    [DllImport ("User32.dll")]
    private static extern bool ShowCaret (IntPtr hWnd);

    [DllImport ("User32.dll")]
    private static extern bool HideCaret (IntPtr hWnd);

    /// <inheritdoc cref="ScrollableControl.OnPaintBackground"/>
    protected override void OnPaintBackground (PaintEventArgs e)
    {
        if (BackBrush == null)
        {
            base.OnPaintBackground (e);
        }
        else
        {
            e.Graphics.FillRectangle (BackBrush, ClientRectangle);
        }
    }

    /// <summary>
    /// Draws text to given Graphics
    /// </summary>
    /// <param name="gr"></param>
    /// <param name="start">Start place of drawing text</param>
    /// <param name="size">Size of drawing</param>
    public void DrawText (Graphics gr, Place start, Size size)
    {
        if (needRecalc)
        {
            Recalc();
        }

        if (needRecalcFoldingLines)
        {
            RecalcFoldingLines();
        }

        var startPoint = PlaceToPoint (start);
        var startY = startPoint.Y + VerticalScroll.Value;
        var startX = startPoint.X + HorizontalScroll.Value - LeftIndent - Paddings.Left;
        var firstChar = start.Column;
        var lastChar = (startX + size.Width) / CharWidth;

        var startLine = start.Line;

        //draw text
        for (var iLine = startLine; iLine < lines.Count; iLine++)
        {
            var line = lines[iLine];
            var lineInfo = LineInfos[iLine];

            //
            if (lineInfo.startY > startY + size.Height)
            {
                break;
            }

            if (lineInfo.startY + lineInfo.WordWrapStringsCount * CharHeight < startY)
            {
                continue;
            }

            if (lineInfo.VisibleState == VisibleState.Hidden)
            {
                continue;
            }

            var y = lineInfo.startY - startY;

            //
            gr.SmoothingMode = SmoothingMode.None;

            //draw line background
            if (lineInfo.VisibleState == VisibleState.Visible)
            {
                if (line.BackgroundBrush != null)
                {
                    gr.FillRectangle (line.BackgroundBrush,
                        new Rectangle (0, y, size.Width, CharHeight * lineInfo.WordWrapStringsCount));
                }
            }

            //
            gr.SmoothingMode = SmoothingMode.AntiAlias;

            //draw wordwrap strings of line
            for (var iWordWrapLine = 0; iWordWrapLine < lineInfo.WordWrapStringsCount; iWordWrapLine++)
            {
                y = lineInfo.startY + iWordWrapLine * CharHeight - startY;

                //indent
                var indent = iWordWrapLine == 0 ? 0 : lineInfo.wordWrapIndent * CharWidth;

                //draw chars
                DrawLineChars (gr, firstChar, lastChar, iLine, iWordWrapLine, -startX + indent, y);
            }
        }
    }

    /// <summary>
    /// Draw control
    /// </summary>
    protected override void OnPaint (PaintEventArgs e)
    {
        if (needRecalc)
        {
            Recalc();
        }

        if (needRecalcFoldingLines)
        {
            RecalcFoldingLines();
        }

        visibleMarkers.Clear();
        e.Graphics.SmoothingMode = SmoothingMode.None;

        //
        var servicePen = new Pen (ServiceLinesColor);
        Brush changedLineBrush = new SolidBrush (ChangedLineColor);
        Brush indentBrush = new SolidBrush (IndentBackColor);
        Brush paddingBrush = new SolidBrush (PaddingBackColor);
        Brush currentLineBrush =
            new SolidBrush (Color.FromArgb (CurrentLineColor.A == 255 ? 50 : CurrentLineColor.A, CurrentLineColor));

        //draw padding area
        var textAreaRect = TextAreaRect;

        //top
        e.Graphics.FillRectangle (paddingBrush, 0, -VerticalScroll.Value, ClientSize.Width,
            Math.Max (0, Paddings.Top - 1));

        //bottom
        e.Graphics.FillRectangle (paddingBrush, 0, textAreaRect.Bottom, ClientSize.Width, ClientSize.Height);

        //right
        e.Graphics.FillRectangle (paddingBrush, textAreaRect.Right, 0, ClientSize.Width, ClientSize.Height);

        //left
        e.Graphics.FillRectangle (paddingBrush, LeftIndentLine, 0, LeftIndent - LeftIndentLine - 1, ClientSize.Height);
        if (HorizontalScroll.Value <= Paddings.Left)
        {
            e.Graphics.FillRectangle (paddingBrush, LeftIndent - HorizontalScroll.Value - 2, 0,
                Math.Max (0, Paddings.Left - 1), ClientSize.Height);
        }

        //
        var leftTextIndent = Math.Max (LeftIndent, LeftIndent + Paddings.Left - HorizontalScroll.Value);
        var textWidth = textAreaRect.Width;

        //draw indent area
        e.Graphics.FillRectangle (indentBrush, 0, 0, LeftIndentLine, ClientSize.Height);
        if (LeftIndent > minLeftIndent)
        {
            e.Graphics.DrawLine (servicePen, LeftIndentLine, 0, LeftIndentLine, ClientSize.Height);
        }

        //draw preferred line width
        if (PreferredLineWidth > 0)
        {
            e.Graphics.DrawLine (servicePen,
                new Point (
                    LeftIndent + Paddings.Left + PreferredLineWidth * CharWidth -
                    HorizontalScroll.Value + 1, textAreaRect.Top + 1),
                new Point (
                    LeftIndent + Paddings.Left + PreferredLineWidth * CharWidth -
                    HorizontalScroll.Value + 1, textAreaRect.Bottom - 1));
        }

        //draw text area border
        DrawTextAreaBorder (e.Graphics);

        //
        var firstChar = Math.Max (0, HorizontalScroll.Value - Paddings.Left) / CharWidth;
        var lastChar = (HorizontalScroll.Value + ClientSize.Width) / CharWidth;

        //
        var x = LeftIndent + Paddings.Left - HorizontalScroll.Value;
        if (x < LeftIndent)
        {
            firstChar++;
        }

        //create dictionary of bookmarks
        var bookmarksByLineIndex = new Dictionary<int, Bookmark>();
        foreach (var item in bookmarks)
        {
            bookmarksByLineIndex[item.LineIndex] = item;
        }

        //
        var startLine = YtoLineIndex (VerticalScroll.Value);
        int iLine;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        //draw text
        for (iLine = startLine; iLine < lines.Count; iLine++)
        {
            var line = lines[iLine];
            var lineInfo = LineInfos[iLine];

            //
            if (lineInfo.startY > VerticalScroll.Value + ClientSize.Height)
            {
                break;
            }

            if (lineInfo.startY + lineInfo.WordWrapStringsCount * CharHeight < VerticalScroll.Value)
            {
                continue;
            }

            if (lineInfo.VisibleState == VisibleState.Hidden)
            {
                continue;
            }

            var y = lineInfo.startY - VerticalScroll.Value;

            //
            e.Graphics.SmoothingMode = SmoothingMode.None;

            //draw line background
            if (lineInfo.VisibleState == VisibleState.Visible)
            {
                if (line.BackgroundBrush != null)
                {
                    e.Graphics.FillRectangle (line.BackgroundBrush,
                        new Rectangle (textAreaRect.Left, y, textAreaRect.Width,
                            CharHeight * lineInfo.WordWrapStringsCount));
                }
            }

            //draw current line background
            if (CurrentLineColor != Color.Transparent && iLine == Selection.Start.Line)
            {
                if (Selection.IsEmpty)
                {
                    e.Graphics.FillRectangle (currentLineBrush,
                        new Rectangle (textAreaRect.Left, y, textAreaRect.Width, CharHeight));
                }
            }

            //draw changed line marker
            if (ChangedLineColor != Color.Transparent && line.IsChanged)
            {
                e.Graphics.FillRectangle (changedLineBrush,
                    new RectangleF (-10, y, LeftIndent - minLeftIndent - 2 + 10, CharHeight + 1));
            }

            //
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //
            //draw bookmark
            if (bookmarksByLineIndex.ContainsKey (iLine))
            {
                bookmarksByLineIndex[iLine].Paint (e.Graphics,
                    new Rectangle (LeftIndent, y, Width,
                        CharHeight * lineInfo.WordWrapStringsCount));
            }

            //OnPaintLine event
            if (lineInfo.VisibleState == VisibleState.Visible)
            {
                OnPaintLine (new PaintLineEventArgs (iLine,
                    new Rectangle (LeftIndent, y, Width,
                        CharHeight * lineInfo.WordWrapStringsCount),
                    e.Graphics, e.ClipRectangle));
            }

            //draw line number
            if (ShowLineNumbers)
            {
                var lineNumber = iLine + (int)lineNumberStartValue;
                var lineNumberText = LineNumberFormatting?.FromLineNumberToString (lineNumber) ?? $"{lineNumber}";

                using (var lineNumberBrush = new SolidBrush (LineNumberColor))
                    e.Graphics.DrawString (lineNumberText, Font, lineNumberBrush,
                        new RectangleF (-10, y, LeftIndent - minLeftIndent - 2 + 10,
                            CharHeight + (int)(lineInterval * 0.5f)),
                        new StringFormat (StringFormatFlags.DirectionRightToLeft)
                            { LineAlignment = StringAlignment.Center });
            }

            //create markers
            var markerSize = (int)(defaultMarkerSize * _zoom / 100f);
            var markerRadius = markerSize / 2;
            if (lineInfo.VisibleState == VisibleState.StartOfHiddenBlock)
            {
                visibleMarkers.Add (new ExpandFoldingMarker (iLine,
                    new Rectangle (LeftIndentLine - markerRadius, y + CharHeight / 2 - markerRadius + 1, markerSize,
                        markerSize)));
            }

            if (!string.IsNullOrEmpty (line.FoldingStartMarker) && lineInfo.VisibleState == VisibleState.Visible &&
                string.IsNullOrEmpty (line.FoldingEndMarker))
            {
                visibleMarkers.Add (new CollapseFoldingMarker (iLine,
                    new Rectangle (LeftIndentLine - markerRadius, y + CharHeight / 2 - markerRadius + 1, markerSize,
                        markerSize)));
            }

            if (lineInfo.VisibleState == VisibleState.Visible && !string.IsNullOrEmpty (line.FoldingEndMarker) &&
                string.IsNullOrEmpty (line.FoldingStartMarker))
            {
                e.Graphics.DrawLine (servicePen, LeftIndentLine, y + CharHeight * lineInfo.WordWrapStringsCount - 1,
                    LeftIndentLine + 4, y + CharHeight * lineInfo.WordWrapStringsCount - 1);
            }

            //draw wordwrap strings of line
            for (var iWordWrapLine = 0; iWordWrapLine < lineInfo.WordWrapStringsCount; iWordWrapLine++)
            {
                y = lineInfo.startY + iWordWrapLine * CharHeight - VerticalScroll.Value;

                // break if too long line (important for extremly big lines)
                if (y > VerticalScroll.Value + ClientSize.Height)
                {
                    break;
                }

                // continue if wordWrapLine isn't seen yet (important for extremly big lines)
                if (lineInfo.startY + iWordWrapLine * CharHeight < VerticalScroll.Value)
                {
                    continue;
                }

                //indent
                var indent = iWordWrapLine == 0 ? 0 : lineInfo.wordWrapIndent * CharWidth;

                //draw chars
                DrawLineChars (e.Graphics, firstChar, lastChar, iLine, iWordWrapLine, x + indent, y);
            }
        }

        var endLine = iLine - 1;

        //draw folding lines
        if (ShowFoldingLines)
        {
            DrawFoldingLines (e, startLine, endLine);
        }

        //draw column selection
        if (Selection.ColumnSelectionMode)
        {
            if (SelectionStyle.BackgroundBrush is SolidBrush brush)
            {
                var color = brush.Color;
                var p1 = PlaceToPoint (Selection.Start);
                var p2 = PlaceToPoint (Selection.End);
                using (var pen = new Pen (color))
                    e.Graphics.DrawRectangle (pen,
                        Rectangle.FromLTRB (Math.Min (p1.X, p2.X) - 1, Math.Min (p1.Y, p2.Y),
                            Math.Max (p1.X, p2.X),
                            Math.Max (p1.Y, p2.Y) + CharHeight));
            }
        }

        //draw brackets highlighting
        if (BracketsStyle != null && leftBracketPosition != null! && rightBracketPosition != null!)
        {
            BracketsStyle.Draw (e.Graphics, PlaceToPoint (leftBracketPosition.Start), leftBracketPosition);
            BracketsStyle.Draw (e.Graphics, PlaceToPoint (rightBracketPosition.Start), rightBracketPosition);
        }

        if (BracketsStyle2 != null && leftBracketPosition2 != null! && rightBracketPosition2 != null!)
        {
            BracketsStyle2.Draw (e.Graphics, PlaceToPoint (leftBracketPosition2.Start), leftBracketPosition2);
            BracketsStyle2.Draw (e.Graphics, PlaceToPoint (rightBracketPosition2.Start), rightBracketPosition2);
        }

        //
        e.Graphics.SmoothingMode = SmoothingMode.None;

        //draw folding indicator
        if ((_startFoldingLine >= 0 || _endFoldingLine >= 0) && Selection.Start == Selection.End)
        {
            if (_endFoldingLine < LineInfos.Count)
            {
                //folding indicator
                var startFoldingY = (_startFoldingLine >= 0 ? LineInfos[_startFoldingLine].startY : 0) -
                    VerticalScroll.Value + CharHeight / 2;
                var endFoldingY = (_endFoldingLine >= 0
                    ? LineInfos[_endFoldingLine].startY +
                      (LineInfos[_endFoldingLine].WordWrapStringsCount - 1) * CharHeight
                    : TextHeight + CharHeight) - VerticalScroll.Value + CharHeight;

                using (var indicatorPen = new Pen (Color.FromArgb (100, FoldingIndicatorColor), 4))
                    e.Graphics.DrawLine (indicatorPen, LeftIndent - 5, startFoldingY, LeftIndent - 5, endFoldingY);
            }
        }

        //draw hint's brackets
        PaintHintBrackets (e.Graphics);

        //draw markers
        DrawMarkers (e, servicePen);

        //draw caret
        var car = PlaceToPoint (Selection.Start);
        var caretHeight = CharHeight - lineInterval;
        car.Offset (0, lineInterval / 2);

        if ((Focused || IsDragDrop || ShowCaretWhenInactive) && car.X >= LeftIndent && CaretVisible)
        {
            var carWidth = IsReplaceMode || WideCaret ? CharWidth : 1;
            if (WideCaret)
            {
                using (var brush = new SolidBrush (CaretColor))
                    e.Graphics.FillRectangle (brush, car.X, car.Y, carWidth, caretHeight + 1);
            }
            else
            {
                using (var pen = new Pen (CaretColor))
                    e.Graphics.DrawLine (pen, car.X, car.Y, car.X, car.Y + caretHeight);
            }

            var caretRect = new Rectangle (HorizontalScroll.Value + car.X, VerticalScroll.Value + car.Y, carWidth,
                caretHeight + 1);

            if (CaretBlinking)
            {
                if (prevCaretRect != caretRect || !ShowScrollBars)
                {
                    CreateCaret (Handle, 0, carWidth, caretHeight + 1);
                    SetCaretPos (car.X, car.Y);
                    ShowCaret (Handle);
                }
            }

            prevCaretRect = caretRect;
        }
        else
        {
            HideCaret (Handle);
            prevCaretRect = Rectangle.Empty;
        }

        //draw disabled mask
        if (!Enabled)
        {
            using (var brush = new SolidBrush (DisabledColor))
                e.Graphics.FillRectangle (brush, ClientRectangle);
        }

        if (MacroManager.IsRecording)
        {
            DrawRecordingHint (e.Graphics);
        }

        if (_middleClickScrollingActivated)
        {
            DrawMiddleClickScrolling (e.Graphics);
        }

        //dispose resources
        servicePen.Dispose();
        changedLineBrush.Dispose();
        indentBrush.Dispose();
        currentLineBrush.Dispose();
        paddingBrush.Dispose();

        //
        base.OnPaint (e);
    }

    private void DrawMarkers (PaintEventArgs e, Pen servicePen)
    {
        foreach (var marker in visibleMarkers)
        {
            if (marker is CollapseFoldingMarker foldingMarker)
            {
                using (var bk = new SolidBrush (ServiceColors.CollapseMarkerBackColor))
                using (var fore = new Pen (ServiceColors.CollapseMarkerForeColor))
                using (var border = new Pen (ServiceColors.CollapseMarkerBorderColor))
                    foldingMarker.Draw (e.Graphics, border, bk, fore);
            }
            else if (marker is ExpandFoldingMarker expandFoldingMarker)
            {
                using (var bk = new SolidBrush (ServiceColors.ExpandMarkerBackColor))
                using (var fore = new Pen (ServiceColors.ExpandMarkerForeColor))
                using (var border = new Pen (ServiceColors.ExpandMarkerBorderColor))
                    expandFoldingMarker.Draw (e.Graphics, border, bk, fore);
            }
            else
            {
                marker.Draw (e.Graphics, servicePen);
            }
        }
    }

    private Rectangle prevCaretRect;

    private void DrawRecordingHint (Graphics graphics)
    {
        const int w = 75;
        const int h = 13;
        var rect = new Rectangle (ClientRectangle.Right - w, ClientRectangle.Bottom - h, w, h);
        var iconRect = new Rectangle (-h / 2 + 3, -h / 2 + 3, h - 7, h - 7);
        var state = graphics.Save();
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.TranslateTransform (rect.Left + h / 2, rect.Top + h / 2);
        graphics.RotateTransform (180 * (DateTime.Now.Millisecond / 1000f));
        using (var pen = new Pen (Color.Red, 2))
        {
            graphics.DrawArc (pen, iconRect, 0, 90);
            graphics.DrawArc (pen, iconRect, 180, 90);
        }

        graphics.DrawEllipse (Pens.Red, iconRect);
        graphics.Restore (state);
        using (var font = new Font (FontFamily.GenericSansSerif, 8f))
            graphics.DrawString ("Recording...", font, Brushes.Red, new PointF (rect.Left + h, rect.Top));
        System.Threading.Timer? tm = null;
        tm = new System.Threading.Timer (
            _ =>
            {
                Invalidate (rect);
                tm.Dispose();
            }, null, 200, Timeout.Infinite);
    }

    private void DrawTextAreaBorder (Graphics graphics)
    {
        if (TextAreaBorder == TextAreaBorderType.None)
        {
            return;
        }

        var rect = TextAreaRect;

        if (TextAreaBorder == TextAreaBorderType.Shadow)
        {
            const int shadowSize = 4;
            var rBottom = new Rectangle (rect.Left + shadowSize, rect.Bottom, rect.Width - shadowSize, shadowSize);
            var rCorner = new Rectangle (rect.Right, rect.Bottom, shadowSize, shadowSize);
            var rRight = new Rectangle (rect.Right, rect.Top + shadowSize, shadowSize, rect.Height - shadowSize);

            using (var brush = new SolidBrush (Color.FromArgb (80, TextAreaBorderColor)))
            {
                graphics.FillRectangle (brush, rBottom);
                graphics.FillRectangle (brush, rRight);
                graphics.FillRectangle (brush, rCorner);
            }
        }

        using (var pen = new Pen (TextAreaBorderColor))
            graphics.DrawRectangle (pen, rect);
    }

    private void PaintHintBrackets (Graphics gr)
    {
        foreach (var hint in hints)
        {
            var r = hint.Range.Clone();
            r.Normalize();
            var p1 = PlaceToPoint (r.Start);
            var p2 = PlaceToPoint (r.End);
            if (GetVisibleState (r.Start.Line) != VisibleState.Visible ||
                GetVisibleState (r.End.Line) != VisibleState.Visible)
            {
                continue;
            }

            using (var pen = new Pen (hint.BorderColor))
            {
                pen.DashStyle = DashStyle.Dash;
                if (r.IsEmpty)
                {
                    p1.Offset (1, -1);
                    gr.DrawLines (pen, new[] { p1, new Point (p1.X, p1.Y + _charHeight + 2) });
                }
                else
                {
                    p1.Offset (-1, -1);
                    p2.Offset (1, -1);
                    gr.DrawLines (pen,
                        new[]
                        {
                            new Point (p1.X + CharWidth / 2, p1.Y), p1,
                            new Point (p1.X, p1.Y + _charHeight + 2),
                            new Point (p1.X + CharWidth / 2, p1.Y + _charHeight + 2)
                        });
                    gr.DrawLines (pen,
                        new[]
                        {
                            new Point (p2.X - CharWidth / 2, p2.Y), p2,
                            new Point (p2.X, p2.Y + _charHeight + 2),
                            new Point (p2.X - CharWidth / 2, p2.Y + _charHeight + 2)
                        });
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void DrawFoldingLines (PaintEventArgs e, int startLine, int endLine)
    {
        e.Graphics.SmoothingMode = SmoothingMode.None;
        using (var pen = new Pen (Color.FromArgb (200, ServiceLinesColor)) { DashStyle = DashStyle.Dot })
            foreach (var iLine in foldingPairs)
            {
                if (iLine.Key < endLine && iLine.Value > startLine)
                {
                    var line = lines[iLine.Key];
                    var y = LineInfos[iLine.Key].startY - VerticalScroll.Value + CharHeight;
                    y += y % 2;

                    int y2;

                    if (iLine.Value >= LinesCount)
                    {
                        y2 = LineInfos[LinesCount - 1].startY + CharHeight - VerticalScroll.Value;
                    }
                    else if (LineInfos[iLine.Value].VisibleState == VisibleState.Visible)
                    {
                        var d = 0;
                        var spaceCount = line.StartSpacesCount;
                        if (lines[iLine.Value].Count <= spaceCount || lines[iLine.Value][spaceCount].c == ' ')
                        {
                            d = CharHeight;
                        }

                        y2 = LineInfos[iLine.Value].startY - VerticalScroll.Value + d;
                    }
                    else
                    {
                        continue;
                    }

                    var x = LeftIndent + Paddings.Left + line.StartSpacesCount * CharWidth - HorizontalScroll.Value;
                    if (x >= LeftIndent + Paddings.Left)
                    {
                        e.Graphics.DrawLine (pen, x, y >= 0 ? y : 0, x,
                            y2 < ClientSize.Height ? y2 : ClientSize.Height);
                    }
                }
            }
    }

    /// <summary>
    /// /
    /// </summary>
    private void DrawLineChars (Graphics gr, int firstChar, int lastChar, int iLine, int iWordWrapLine, int startX,
        int y)
    {
        var line = lines[iLine];
        var lineInfo = LineInfos[iLine];
        var from = lineInfo.GetWordWrapStringStartPosition (iWordWrapLine);
        var to = lineInfo.GetWordWrapStringFinishPosition (iWordWrapLine, line);

        lastChar = Math.Min (to - from, lastChar);

        gr.SmoothingMode = SmoothingMode.AntiAlias;

        //folded block ?
        if (lineInfo.VisibleState == VisibleState.StartOfHiddenBlock)
        {
            //rendering by FoldedBlockStyle
            FoldedBlockStyle.Draw (gr, new Point (startX + firstChar * CharWidth, y),
                new TextRange (this, from + firstChar, iLine, from + lastChar + 1, iLine));
        }
        else
        {
            //render by custom styles
            var currentStyleIndex = StyleIndex.None;
            var iLastFlushedChar = firstChar - 1;

            for (var iChar = firstChar; iChar <= lastChar; iChar++)
            {
                var style = line[from + iChar].style;
                if (currentStyleIndex != style)
                {
                    FlushRendering (gr, currentStyleIndex,
                        new Point (startX + (iLastFlushedChar + 1) * CharWidth, y),
                        new TextRange (this, from + iLastFlushedChar + 1, iLine, from + iChar, iLine));
                    iLastFlushedChar = iChar - 1;
                    currentStyleIndex = style;
                }
            }

            FlushRendering (gr, currentStyleIndex, new Point (startX + (iLastFlushedChar + 1) * CharWidth, y),
                new TextRange (this, from + iLastFlushedChar + 1, iLine, from + lastChar + 1, iLine));
        }

        //draw selection
        if (SelectionHighlightingForLineBreaksEnabled && iWordWrapLine == lineInfo.WordWrapStringsCount - 1)
        {
            lastChar++; //draw selection for CR
        }

        if (!Selection.IsEmpty && lastChar >= firstChar)
        {
            gr.SmoothingMode = SmoothingMode.None;
            var textRange = new TextRange (this, from + firstChar, iLine, from + lastChar + 1, iLine);
            textRange = Selection.GetIntersectionWith (textRange);
            if (textRange != null! && SelectionStyle != null!)
            {
                SelectionStyle.Draw (gr, new Point (startX + (textRange.Start.Column - from) * CharWidth, 1 + y),
                    textRange);
            }
        }
    }

    private void FlushRendering (Graphics gr, StyleIndex styleIndex, Point pos, TextRange range)
    {
        if (range.End > range.Start)
        {
            var mask = 1;
            var hasTextStyle = false;
            for (var i = 0; i < Styles.Length; i++)
            {
                if (Styles[i] != null! && ((int)styleIndex & mask) != 0)
                {
                    var style = Styles[i];
                    var isTextStyle = style is TextStyle;
                    if (!hasTextStyle || !isTextStyle || AllowSeveralTextStyleDrawing)

                        //cancelling secondary rendering by TextStyle
                    {
                        style.Draw (gr, pos, range); //rendering
                    }

                    hasTextStyle |= isTextStyle;
                }

                mask = mask << 1;
            }

            //draw by default renderer
            if (!hasTextStyle)
            {
                DefaultStyle.Draw (gr, pos, range);
            }
        }
    }

    /// <inheritdoc cref="Control.OnEnter"/>
    protected override void OnEnter (EventArgs e)
    {
        base.OnEnter (e);
        mouseIsDrag = false;
        mouseIsDragDrop = false;
        DraggedRange = null;
    }

    /// <inheritdoc cref="Control.OnMouseUp"/>
    protected override void OnMouseUp (MouseEventArgs e)
    {
        base.OnMouseUp (e);
        isLineSelect = false;

        if (e.Button == MouseButtons.Left && mouseIsDragDrop)
        {
            OnMouseClickText (e);
        }
    }

    /// <inheritdoc cref="UserControl.OnMouseDown"/>
    protected override void OnMouseDown (MouseEventArgs e)
    {
        base.OnMouseDown (e);

        if (_middleClickScrollingActivated)
        {
            DeactivateMiddleClickScrollingMode();
            mouseIsDrag = false;
            if (e.Button == MouseButtons.Middle)
            {
                RestoreScrollsAfterMiddleClickScrollingMode();
            }

            return;
        }

        MacroManager.IsRecording = false;

        Select();
        ActiveControl = null;

        if (e.Button == MouseButtons.Left)
        {
            var marker = FindVisualMarkerForPoint (e.Location);

            //click on marker
            if (marker != null)
            {
                mouseIsDrag = false;
                mouseIsDragDrop = false;
                DraggedRange = null;
                OnMarkerClick (e, marker);
                return;
            }

            mouseIsDrag = true;
            mouseIsDragDrop = false;
            DraggedRange = null;
            isLineSelect = e.Location.X < LeftIndentLine;

            if (!isLineSelect)
            {
                var p = PointToPlace (e.Location);

                if (e.Clicks == 2)
                {
                    mouseIsDrag = false;
                    mouseIsDragDrop = false;
                    DraggedRange = null;

                    SelectWord (p);
                    return;
                }

                if (Selection.IsEmpty || !Selection.Contains (p) || this[p.Line].Count <= p.Column || ReadOnly)
                {
                    OnMouseClickText (e);
                }
                else
                {
                    mouseIsDragDrop = true;
                    mouseIsDrag = false;
                }
            }
            else
            {
                CheckAndChangeSelectionType();

                Selection.BeginUpdate();

                //select whole line
                var iLine = PointToPlaceSimple (e.Location).Line;
                lineSelectFrom = iLine;
                Selection.Start = new Place (0, iLine);
                Selection.End = new Place (GetLineLength (iLine), iLine);
                Selection.EndUpdate();
                Invalidate();
            }
        }
        else if (e.Button == MouseButtons.Middle)
        {
            ActivateMiddleClickScrollingMode (e);
        }
    }

    private void OnMouseClickText (MouseEventArgs e)
    {
        //click on text
        var oldEnd = Selection.End;
        Selection.BeginUpdate();

        if (Selection.ColumnSelectionMode)
        {
            Selection.Start = PointToPlaceSimple (e.Location);
            Selection.ColumnSelectionMode = true;
        }
        else
        {
            if (VirtualSpace)
            {
                Selection.Start = PointToPlaceSimple (e.Location);
            }
            else
            {
                Selection.Start = PointToPlace (e.Location);
            }
        }

        if ((lastModifiers & Keys.Shift) != 0)
        {
            Selection.End = oldEnd;
        }

        CheckAndChangeSelectionType();

        Selection.EndUpdate();
        Invalidate();
        return;
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void CheckAndChangeSelectionType()
    {
        //change selection type to ColumnSelectionMode
        if ((ModifierKeys & Keys.Alt) != 0 && !WordWrap)
        {
            Selection.ColumnSelectionMode = true;
        }
        else

            //change selection type to Range
        {
            Selection.ColumnSelectionMode = false;
        }
    }

    /// <inheritdoc cref="ScrollableControl.OnMouseWheel"/>
    protected override void OnMouseWheel (MouseEventArgs e)
    {
        Invalidate();

        if (lastModifiers == Keys.Control)
        {
            ChangeFontSize (2 * Math.Sign (e.Delta));
            ((HandledMouseEventArgs)e).Handled = true;
        }
        else if (VerticalScroll.Visible || !ShowScrollBars)
        {
            //base.OnMouseWheel(e);

            // Determine scoll offset
            var mouseWheelScrollLinesSetting = GetControlPanelWheelScrollLinesValue();

            DoScrollVertical (mouseWheelScrollLinesSetting, e.Delta);

            ((HandledMouseEventArgs)e).Handled = true;
        }

        DeactivateMiddleClickScrollingMode();
    }

    private void DoScrollVertical (int countLines, int direction)
    {
        if (VerticalScroll.Visible || !ShowScrollBars)
        {
            var numberOfVisibleLines = ClientSize.Height / CharHeight;

            int offset;
            if (countLines == -1 || countLines > numberOfVisibleLines)
            {
                offset = CharHeight * numberOfVisibleLines;
            }
            else
            {
                offset = CharHeight * countLines;
            }

            var newScrollPos = VerticalScroll.Value - Math.Sign (direction) * offset;

            var ea =
                new ScrollEventArgs (direction > 0 ? ScrollEventType.SmallDecrement : ScrollEventType.SmallIncrement,
                    VerticalScroll.Value,
                    newScrollPos,
                    ScrollOrientation.VerticalScroll);

            OnScroll (ea);
        }
    }

    /// <summary>
    /// Gets the value for the system control panel mouse wheel scroll settings.
    /// The value returns the number of lines that shall be scolled if the user turns the mouse wheet one step.
    /// </summary>
    /// <remarks>
    /// This methods gets the "WheelScrollLines" value our from the registry key "HKEY_CURRENT_USER\Control Panel\Desktop".
    /// If the value of this option is 0, the screen will not scroll when the mouse wheel is turned.
    /// If the value of this option is -1 or is greater than the number of lines visible in the window,
    /// the screen will scroll up or down by one page.
    /// </remarks>
    /// <returns>
    /// Number of lines to scrol l when the mouse wheel is turned
    /// </returns>
    private static int GetControlPanelWheelScrollLinesValue()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey (@"Control Panel\Desktop", false);

            return Convert.ToInt32 (key!.GetValue ("WheelScrollLines"));
        }
        catch
        {
            // Use default value
            return 1;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void ChangeFontSize
        (
            int step
        )
    {
        var points = Font.SizeInPoints;
        using var gr = Graphics.FromHwnd (Handle);
        var dpi = gr.DpiY;
        var newPoints = points + step * 72f / dpi;
        if (newPoints < 1f)
        {
            return;
        }

        var k = newPoints / originalFont.SizeInPoints;
        Zoom = (int)Math.Round (100 * k);
    }

    /// <summary>
    /// Zooming (in percentages)
    /// </summary>
    [Browsable (false)]
    public int Zoom
    {
        get => _zoom;
        set
        {
            _zoom = value;
            DoZoom (_zoom / 100f);
            OnZoomChanged();
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnZoomChanged()
    {
        ZoomChanged?.Invoke (this, EventArgs.Empty);
    }

    private void DoZoom
        (
            float coefficient
        )
    {
        //remember first displayed line
        var iLine = YtoLineIndex (VerticalScroll.Value);

        //
        var points = originalFont.SizeInPoints;
        points *= coefficient;

        if (points is < 1f or > 300f)
        {
            return;
        }

        var oldFont = Font;
        SetFont (new Font (Font.FontFamily, points, Font.Style, GraphicsUnit.Point));
        oldFont.Dispose();

        NeedRecalc (true);

        //restore first displayed line
        if (iLine < LinesCount)
        {
            VerticalScroll.Value = Math.Min (VerticalScroll.Maximum, LineInfos[iLine].startY - Paddings.Top);
        }

        UpdateScrollbars();

        //
        Invalidate();
        OnVisibleRangeChanged();
    }

    /// <inheritdoc cref="Control.OnMouseLeave"/>
    protected override void OnMouseLeave (EventArgs e)
    {
        base.OnMouseLeave (e);

        CancelToolTip();
    }

    /// <summary>
    ///
    /// </summary>
    protected TextRange? DraggedRange;

    /// <inheritdoc cref="Control.OnMouseMove"/>
    protected override void OnMouseMove
        (
            MouseEventArgs e
        )
    {
        base.OnMouseMove (e);

        if (_middleClickScrollingActivated)
        {
            return;
        }

        if (lastMouseCoord != e.Location)
        {
            //restart tooltip timer
            CancelToolTip();
            _timer3.Start();
        }

        lastMouseCoord = e.Location;

        if (e.Button == MouseButtons.Left && mouseIsDragDrop)
        {
            DraggedRange = Selection.Clone();
            DoDragDrop (SelectedText, DragDropEffects.Copy);
            DraggedRange = null;
            return;
        }

        if (e.Button == MouseButtons.Left && mouseIsDrag)
        {
            Place place;
            if (Selection.ColumnSelectionMode || VirtualSpace)
            {
                place = PointToPlaceSimple (e.Location);
            }
            else
            {
                place = PointToPlace (e.Location);
            }

            if (isLineSelect)
            {
                Selection.BeginUpdate();

                var iLine = place.Line;
                if (iLine < lineSelectFrom)
                {
                    Selection.Start = new Place (0, iLine);
                    Selection.End = new Place (GetLineLength (lineSelectFrom), lineSelectFrom);
                }
                else
                {
                    Selection.Start = new Place (GetLineLength (iLine), iLine);
                    Selection.End = new Place (0, lineSelectFrom);
                }

                Selection.EndUpdate();
                DoCaretVisible();
                HorizontalScroll.Value = 0;
                UpdateScrollbars();
                Invalidate();
            }
            else if (place != Selection.Start)
            {
                var oldEnd = Selection.End;
                Selection.BeginUpdate();
                if (Selection.ColumnSelectionMode)
                {
                    Selection.Start = place;
                    Selection.ColumnSelectionMode = true;
                }
                else
                {
                    Selection.Start = place;
                }

                Selection.End = oldEnd;
                Selection.EndUpdate();
                DoCaretVisible();
                Invalidate();
                return;
            }
        }

        var marker = FindVisualMarkerForPoint (e.Location);
        if (marker != null)
        {
            base.Cursor = marker.Cursor;
        }
        else
        {
            if (e.Location.X < LeftIndentLine || isLineSelect)
            {
                base.Cursor = Cursors.Arrow;
            }
            else
            {
                base.Cursor = defaultCursor;
            }
        }
    }

    private void CancelToolTip()
    {
        _timer3.Stop();
        if (ToolTip != null && !string.IsNullOrEmpty (ToolTip.GetToolTip (this)))
        {
            ToolTip.Hide (this);
            ToolTip.SetToolTip (this, null);
        }
    }

    /// <inheritdoc cref="Control.OnMouseDoubleClick"/>
    protected override void OnMouseDoubleClick
        (
            MouseEventArgs e
        )
    {
        base.OnMouseDoubleClick (e);

        var m = FindVisualMarkerForPoint (e.Location);
        if (m != null)
        {
            OnMarkerDoubleClick (m);
        }
    }

    private void SelectWord (Place p)
    {
        var fromX = p.Column;
        var toX = p.Column;

        for (var i = p.Column; i < lines[p.Line].Count; i++)
        {
            var c = lines[p.Line][i].c;
            if (char.IsLetterOrDigit (c) || c == '_')
            {
                toX = i + 1;
            }
            else
            {
                break;
            }
        }

        for (var i = p.Column - 1; i >= 0; i--)
        {
            var c = lines[p.Line][i].c;
            if (char.IsLetterOrDigit (c) || c == '_')
            {
                fromX = i;
            }
            else
            {
                break;
            }
        }

        Selection = new TextRange (this, toX, p.Line, fromX, p.Line);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public int YtoLineIndex (int y)
    {
        var i = LineInfos.BinarySearch (new LineInfo (-10), new LineYComparer (y));
        i = i < 0 ? -i - 2 : i;
        if (i < 0)
        {
            return 0;
        }

        if (i > lines.Count - 1)
        {
            return lines.Count - 1;
        }

        return i;
    }

    /// <summary>
    /// Gets nearest line and char position from coordinates
    /// </summary>
    /// <param name="point">Point</param>
    /// <returns>Line and char position</returns>
    public Place PointToPlace (Point point)
    {
        point.Offset (HorizontalScroll.Value, VerticalScroll.Value);
        point.Offset (-LeftIndent - Paddings.Left, 0);
        var iLine = YtoLineIndex (point.Y);
        if (iLine < 0)
        {
            return Place.Empty;
        }

        var y = 0;

        for (; iLine < lines.Count; iLine++)
        {
            y = LineInfos[iLine].startY + LineInfos[iLine].WordWrapStringsCount * CharHeight;
            if (y > point.Y && LineInfos[iLine].VisibleState == VisibleState.Visible)
            {
                break;
            }
        }

        if (iLine >= lines.Count)
        {
            iLine = lines.Count - 1;
        }

        if (LineInfos[iLine].VisibleState != VisibleState.Visible)
        {
            iLine = FindPrevVisibleLine (iLine);
        }

        //
        var iWordWrapLine = LineInfos[iLine].WordWrapStringsCount;

        //set iWordWrapLine more accurate (important for extremly big lines)
        if (y > point.Y)
        {
            var approximatelyLines = (y - point.Y - CharHeight) / CharHeight;
            y -= approximatelyLines * CharHeight;
            iWordWrapLine -= approximatelyLines;
        }

        do
        {
            iWordWrapLine--;
            y -= CharHeight;
        } while (y > point.Y);

        if (iWordWrapLine < 0)
        {
            iWordWrapLine = 0;
        }

        //
        var start = LineInfos[iLine].GetWordWrapStringStartPosition (iWordWrapLine);
        var finish = LineInfos[iLine].GetWordWrapStringFinishPosition (iWordWrapLine, lines[iLine]);
        var x = (int)Math.Round ((float)point.X / CharWidth);
        if (iWordWrapLine > 0)
        {
            x -= LineInfos[iLine].wordWrapIndent;
        }

        x = x < 0 ? start : start + x;
        if (x > finish)
        {
            x = finish + 1;
        }

        if (x > lines[iLine].Count)
        {
            x = lines[iLine].Count;
        }

        return new Place (x, iLine);
    }

    private Place PointToPlaceSimple (Point point)
    {
        point.Offset (HorizontalScroll.Value, VerticalScroll.Value);
        point.Offset (-LeftIndent - Paddings.Left, 0);
        var iLine = YtoLineIndex (point.Y);
        var x = (int)Math.Round ((float)point.X / CharWidth);
        if (x < 0)
        {
            x = 0;
        }

        return new Place (x, iLine);
    }

    /// <summary>
    /// Gets nearest absolute text position for given point
    /// </summary>
    /// <param name="point">Point</param>
    /// <returns>Position</returns>
    public int PointToPosition (Point point)
    {
        return PlaceToPosition (PointToPlace (point));
    }

    /// <summary>
    /// Fires TextChanging event
    /// </summary>
    public virtual void OnTextChanging (ref string? text)
    {
        ClearBracketsPositions();
        if (TextChanging != null)
        {
            var args = new TextChangingEventArgs { InsertingText = text };
            TextChanging (this, args);
            text = args.InsertingText;
            if (args.Cancel)
            {
                text = string.Empty;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public virtual void OnTextChanging()
    {
        string? temp = null;
        OnTextChanging (ref temp);
    }

    /// <summary>
    /// Fires TextChanged event
    /// </summary>
    public virtual void OnTextChanged()
    {
        var r = new TextRange (this);
        r.SelectAll();
        OnTextChanged (new TextChangedEventArgs (r));
    }

    /// <summary>
    /// Fires TextChanged event
    /// </summary>
    public virtual void OnTextChanged (int fromLine, int toLine)
    {
        var r = new TextRange (this)
        {
            Start = new Place (0, Math.Min (fromLine, toLine)),
            End = new Place (lines[Math.Max (fromLine, toLine)].Count, Math.Max (fromLine, toLine))
        };
        OnTextChanged (new TextChangedEventArgs (r));
    }

    /// <summary>
    /// Fires TextChanged event
    /// </summary>
    public virtual void OnTextChanged (TextRange r)
    {
        OnTextChanged (new TextChangedEventArgs (r));
    }

    /// <summary>
    /// Call this method before multiple text changing
    /// </summary>
    public void BeginUpdate()
    {
        if (_updating == 0)
        {
            _updatingRange = null;
        }

        _updating++;
    }

    /// <summary>
    /// Call this method after multiple text changing
    /// </summary>
    public void EndUpdate()
    {
        _updating--;

        if (_updating == 0 && _updatingRange != null)
        {
            _updatingRange.Expand();
            OnTextChanged (_updatingRange);
        }
    }

    /// <summary>
    /// Fires TextChanged event
    /// </summary>
    protected virtual void OnTextChanged (TextChangedEventArgs args)
    {
        //
        args.ChangedRange.Normalize();

        //
        if (_updating > 0)
        {
            if (_updatingRange == null)
            {
                _updatingRange = args.ChangedRange.Clone();
            }
            else
            {
                if (_updatingRange.Start.Line > args.ChangedRange.Start.Line)
                {
                    _updatingRange.Start = new Place (0, args.ChangedRange.Start.Line);
                }

                if (_updatingRange.End.Line < args.ChangedRange.End.Line)
                {
                    _updatingRange.End = new Place (lines[args.ChangedRange.End.Line].Count,
                        args.ChangedRange.End.Line);
                }

                _updatingRange = _updatingRange.GetIntersectionWith (Range);
            }

            return;
        }

        //
        CancelToolTip();
        ClearHints();
        IsChanged = true;
        TextVersion++;
        MarkLinesAsChanged (args.ChangedRange);
        ClearFoldingState (args.ChangedRange);

        //
        if (_wordWrap)
        {
            RecalcWordWrap (args.ChangedRange.Start.Line, args.ChangedRange.End.Line);
        }

        //
        base.OnTextChanged (args);

        //dalayed event stuffs
        _delayedTextChangedRange = _delayedTextChangedRange == null
            ? args.ChangedRange.Clone()
            : _delayedTextChangedRange.GetUnionWith (args.ChangedRange);

        needRiseTextChangedDelayed = true;
        ResetTimer (_timer2);

        //
        OnSyntaxHighlight (args);

        //
        TextChanged?.Invoke (this, args);

        //
        BindingTextChanged?.Invoke (this, EventArgs.Empty);

        //
        base.OnTextChanged (EventArgs.Empty);

        //
        OnVisibleRangeChanged();
    }

    /// <summary>
    /// Clears folding state for range of text
    /// </summary>
    private void ClearFoldingState (TextRange range)
    {
        for (var iLine = range.Start.Line; iLine <= range.End.Line; iLine++)
        {
            if (iLine >= 0 && iLine < lines.Count)
            {
                FoldedBlocks.Remove (this[iLine].UniqueId);
            }
        }
    }


    private void MarkLinesAsChanged (TextRange range)
    {
        for (var iLine = range.Start.Line; iLine <= range.End.Line; iLine++)
        {
            if (iLine >= 0 && iLine < lines.Count)
            {
                lines[iLine].IsChanged = true;
            }
        }
    }

    /// <summary>
    /// Fires SelectionChanged event
    /// </summary>
    public virtual void OnSelectionChanged()
    {
        //find folding markers for highlighting
        if (HighlightFoldingIndicator)
        {
            HighlightFoldings();
        }

        //
        needRiseSelectionChangedDelayed = true;
        ResetTimer (_timer1);

        SelectionChanged?.Invoke (this, EventArgs.Empty);
    }

    //find folding markers for highlighting
    private void HighlightFoldings()
    {
        if (LinesCount == 0)
        {
            return;
        }

        //
        var prevStartFoldingLine = _startFoldingLine;
        var prevEndFoldingLine = _endFoldingLine;

        //
        _startFoldingLine = -1;
        _endFoldingLine = -1;
        var counter = 0;
        for (var i = Selection.Start.Line; i >= Math.Max (Selection.Start.Line - maxLinesForFolding, 0); i--)
        {
            var hasStartMarker = lines.LineHasFoldingStartMarker (i);
            var hasEndMarker = lines.LineHasFoldingEndMarker (i);

            if (hasEndMarker && hasStartMarker)
            {
                continue;
            }

            if (hasStartMarker)
            {
                counter--;
                if (counter == -1) //found start folding
                {
                    _startFoldingLine = i;
                    break;
                }
            }

            if (hasEndMarker && i != Selection.Start.Line)
            {
                counter++;
            }
        }

        if (_startFoldingLine >= 0)
        {
            //find end of block
            _endFoldingLine = FindEndOfFoldingBlock (_startFoldingLine, maxLinesForFolding);
            if (_endFoldingLine == _startFoldingLine)
            {
                _endFoldingLine = -1;
            }
        }

        if (_startFoldingLine != prevStartFoldingLine || _endFoldingLine != prevEndFoldingLine)
        {
            OnFoldingHighlightChanged();
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnFoldingHighlightChanged()
    {
        FoldingHighlightChanged?.Invoke (this, EventArgs.Empty);
    }

    /// <inheritdoc cref="Control.OnGotFocus"/>
    protected override void OnGotFocus
        (
            EventArgs e
        )
    {
        SetAsCurrentTB();
        base.OnGotFocus (e);
        Invalidate();
    }

    /// <inheritdoc cref="Control.OnLostFocus"/>
    protected override void OnLostFocus
        (
            EventArgs e
        )
    {
        lastModifiers = Keys.None;
        DeactivateMiddleClickScrollingMode();
        base.OnLostFocus (e);
        Invalidate();
    }

    /// <summary>
    /// Gets absolute text position from line and char position
    /// </summary>
    /// <param name="point">Line and char position</param>
    /// <returns>Point of char</returns>
    public int PlaceToPosition (Place point)
    {
        if (point.Line < 0 || point.Line >= lines.Count ||
            point.Column >= lines[point.Line].Count + Environment.NewLine.Length)
        {
            return -1;
        }

        var result = 0;
        for (var i = 0; i < point.Line; i++)
        {
            result += lines[i].Count + Environment.NewLine.Length;
        }

        result += point.Column;

        return result;
    }

    /// <summary>
    /// Gets line and char position from absolute text position
    /// </summary>
    public Place PositionToPlace (int pos)
    {
        if (pos < 0)
        {
            return new Place (0, 0);
        }

        for (var i = 0; i < lines.Count; i++)
        {
            var lineLength = lines[i].Count + Environment.NewLine.Length;
            if (pos < lines[i].Count)
            {
                return new Place (pos, i);
            }

            if (pos < lineLength)
            {
                return new Place (lines[i].Count, i);
            }

            pos -= lineLength;
        }

        if (lines.Count > 0)
        {
            return new Place (lines[^1].Count, lines.Count - 1);
        }
        else
        {
            return new Place (0, 0);
        }

        //throw new ArgumentOutOfRangeException("Position out of range");
    }

    /// <summary>
    /// Gets absolute char position from char position
    /// </summary>
    public Point PositionToPoint (int pos)
    {
        return PlaceToPoint (PositionToPlace (pos));
    }

    /// <summary>
    /// Gets point for given line and char position
    /// </summary>
    /// <param name="place">Line and char position</param>
    /// <returns>Coordiantes</returns>
    public Point PlaceToPoint (Place place)
    {
        if (place.Line >= LineInfos.Count)
        {
            return new Point();
        }

        var y = LineInfos[place.Line].startY;

        //
        var iWordWrapIndex = LineInfos[place.Line].GetWordWrapStringIndex (place.Column);
        y += iWordWrapIndex * CharHeight;
        var x = (place.Column - LineInfos[place.Line].GetWordWrapStringStartPosition (iWordWrapIndex)) * CharWidth;
        if (iWordWrapIndex > 0)
        {
            x += LineInfos[place.Line].wordWrapIndent * CharWidth;
        }

        //
        y = y - VerticalScroll.Value;
        x = LeftIndent + Paddings.Left + x - HorizontalScroll.Value;

        return new Point (x, y);
    }

    /// <summary>
    /// Get range of text
    /// </summary>
    /// <param name="fromPos">Absolute start position</param>
    /// <param name="toPos">Absolute finish position</param>
    /// <returns>Range</returns>
    public TextRange GetRange (int fromPos, int toPos)
    {
        var sel = new TextRange (this)
        {
            Start = PositionToPlace (fromPos),
            End = PositionToPlace (toPos)
        };
        return sel;
    }

    /// <summary>
    /// Get range of text
    /// </summary>
    /// <param name="fromPlace">Line and char position</param>
    /// <param name="toPlace">Line and char position</param>
    /// <returns>Range</returns>
    public TextRange GetRange (Place fromPlace, Place toPlace)
    {
        return new TextRange (this, fromPlace, toPlace);
    }

    /// <summary>
    /// Finds ranges for given regex pattern
    /// </summary>
    /// <param name="regexPattern">Regex pattern</param>
    /// <returns>Enumeration of ranges</returns>
    public IEnumerable<TextRange> GetRanges (string regexPattern)
    {
        var range = new TextRange (this);
        range.SelectAll();

        //
        foreach (var r in range.GetRanges (regexPattern, RegexOptions.None))
        {
            yield return r;
        }
    }

    /// <summary>
    /// Finds ranges for given regex pattern
    /// </summary>
    /// <param name="regexPattern">Regex pattern</param>
    /// <param name="options"></param>
    /// <returns>Enumeration of ranges</returns>
    public IEnumerable<TextRange> GetRanges
        (
            string regexPattern,
            RegexOptions options
        )
    {
        var range = new TextRange (this);
        range.SelectAll();

        //
        foreach (var r in range.GetRanges (regexPattern, options))
        {
            yield return r;
        }
    }

    /// <summary>
    /// Get text of given line
    /// </summary>
    /// <param name="lineIndex">Line index</param>
    /// <returns>Text</returns>
    public string GetLineText
        (
            int lineIndex
        )
    {
        if (lineIndex < 0 || lineIndex >= lines.Count)
        {
            throw new ArgumentOutOfRangeException (nameof (lineIndex), "Line index out of range");
        }

        var builder = new StringBuilder (lines[lineIndex].Count);
        foreach (var c in lines[lineIndex])
        {
            builder.Append (c.c);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Exapnds folded block
    /// </summary>
    /// <param name="lineIndex">Start line</param>
    public virtual void ExpandFoldedBlock (int lineIndex)
    {
        if (lineIndex < 0 || lineIndex >= lines.Count)
        {
            throw new ArgumentOutOfRangeException (nameof (lineIndex), "Line index out of range");
        }

        //find all hidden lines afetr iLine
        var end = lineIndex;
        for (; end < LinesCount - 1; end++)
        {
            if (LineInfos[end + 1].VisibleState != VisibleState.Hidden)
            {
                break;
            }
        }

        ExpandBlock (lineIndex, end);

        FoldedBlocks.Remove (this[lineIndex].UniqueId); //remove folded state for this line
        AdjustFolding();
    }

    /// <summary>
    /// Collapse folding blocks using FoldedBlocks dictionary.
    /// </summary>
    public virtual void AdjustFolding()
    {
        //collapse folded blocks
        for (var lineIndex = 0; lineIndex < LinesCount; lineIndex++)
        {
            if (LineInfos[lineIndex].VisibleState == VisibleState.Visible
                && FoldedBlocks.ContainsKey (this[lineIndex].UniqueId))
            {
                CollapseFoldingBlock (lineIndex);
            }
        }
    }

    /// <summary>
    /// Expand collapsed block
    /// </summary>
    public virtual void ExpandBlock (int fromLine, int toLine)
    {
        var from = Math.Min (fromLine, toLine);
        var to = Math.Max (fromLine, toLine);
        for (var i = from; i <= to; i++)
        {
            SetVisibleState (i, VisibleState.Visible);
        }

        needRecalc = true;

        Invalidate();
        OnVisibleRangeChanged();
    }

    /// <summary>
    /// Expand collapsed block
    /// </summary>
    /// <param name="iLine">Any line inside collapsed block</param>
    public void ExpandBlock (int iLine)
    {
        if (LineInfos[iLine].VisibleState == VisibleState.Visible)
        {
            return;
        }

        for (var i = iLine; i < LinesCount; i++)
        {
            if (LineInfos[i].VisibleState == VisibleState.Visible)
            {
                break;
            }

            SetVisibleState (i, VisibleState.Visible);
            needRecalc = true;
        }

        for (var i = iLine - 1; i >= 0; i--)
        {
            if (LineInfos[i].VisibleState == VisibleState.Visible)
            {
                break;
            }
            else
            {
                SetVisibleState (i, VisibleState.Visible);
                needRecalc = true;
            }
        }

        Invalidate();
        OnVisibleRangeChanged();
    }

    /// <summary>
    /// Collapses all folding blocks
    /// </summary>
    public virtual void CollapseAllFoldingBlocks()
    {
        for (var i = 0; i < LinesCount; i++)
        {
            if (lines.LineHasFoldingStartMarker (i))
            {
                var iFinish = FindEndOfFoldingBlock (i);
                if (iFinish >= 0)
                {
                    CollapseBlock (i, iFinish);
                    i = iFinish;
                }
            }
        }

        OnVisibleRangeChanged();
        UpdateScrollbars();
    }

    /// <summary>
    /// Exapnds all folded blocks
    /// </summary>
    public virtual void ExpandAllFoldingBlocks()
    {
        for (var i = 0; i < LinesCount; i++)
        {
            SetVisibleState (i, VisibleState.Visible);
        }

        FoldedBlocks.Clear();

        OnVisibleRangeChanged();
        Invalidate();
        UpdateScrollbars();
    }

    /// <summary>
    /// Collapses folding block
    /// </summary>
    /// <param name="iLine">Start folding line</param>
    public virtual void CollapseFoldingBlock (int iLine)
    {
        if (iLine < 0 || iLine >= lines.Count)
        {
            throw new ArgumentOutOfRangeException ("Line index out of range");
        }

        if (string.IsNullOrEmpty (lines[iLine].FoldingStartMarker))
        {
            throw new ArgumentOutOfRangeException ("This line is not folding start line");
        }

        //find end of block
        var i = FindEndOfFoldingBlock (iLine);

        //collapse
        if (i >= 0)
        {
            CollapseBlock (iLine, i);
            var id = this[iLine].UniqueId;
            FoldedBlocks[id] = id; //add folded state for line
        }
    }

    private int FindEndOfFoldingBlock (int iStartLine)
    {
        return FindEndOfFoldingBlock (iStartLine, int.MaxValue);
    }

    protected virtual int FindEndOfFoldingBlock (int iStartLine, int maxLines)
    {
        //find end of block
        int i;
        var marker = lines[iStartLine].FoldingStartMarker;
        var stack = new Stack<string>();

        switch (FindEndOfFoldingBlockStrategy)
        {
            case FindEndOfFoldingBlockStrategy.Strategy1:
                for (i = iStartLine /*+1*/; i < LinesCount; i++)
                {
                    if (lines.LineHasFoldingStartMarker (i))
                    {
                        stack.Push (lines[i].FoldingStartMarker!);
                    }

                    if (lines.LineHasFoldingEndMarker (i))
                    {
                        var m = lines[i].FoldingEndMarker;
                        while (stack.Count > 0 && stack.Pop() != m)
                        {
                            // пустое тело цикла
                        }
                        if (stack.Count == 0)
                        {
                            return i;
                        }
                    }

                    maxLines--;
                    if (maxLines < 0)
                    {
                        return i;
                    }
                }

                break;

            case FindEndOfFoldingBlockStrategy.Strategy2:
                for (i = iStartLine /*+1*/; i < LinesCount; i++)
                {
                    if (lines.LineHasFoldingEndMarker (i))
                    {
                        var m = lines[i].FoldingEndMarker;
                        while (stack.Count > 0 && stack.Pop() != m)
                        {
                            // пустое тело цикла
                        }
                        if (stack.Count == 0)
                        {
                            return i;
                        }
                    }

                    if (lines.LineHasFoldingStartMarker (i))
                    {
                        stack.Push (lines[i].FoldingStartMarker!);
                    }

                    maxLines--;
                    if (maxLines < 0)
                    {
                        return i;
                    }
                }

                break;
        }

        //return -1;
        return LinesCount - 1;
    }

    /// <summary>
    /// Start foilding marker for the line
    /// </summary>
    public string? GetLineFoldingStartMarker (int iLine)
    {
        if (lines.LineHasFoldingStartMarker (iLine))
        {
            return lines[iLine].FoldingStartMarker;
        }

        return null;
    }

    /// <summary>
    /// End foilding marker for the line
    /// </summary>
    public string? GetLineFoldingEndMarker (int iLine)
    {
        if (lines.LineHasFoldingEndMarker (iLine))
        {
            return lines[iLine].FoldingEndMarker;
        }

        return null;
    }

    protected virtual void RecalcFoldingLines()
    {
        if (!needRecalcFoldingLines)
        {
            return;
        }

        needRecalcFoldingLines = false;
        if (!ShowFoldingLines)
        {
            return;
        }

        foldingPairs.Clear();

        //
        var range = VisibleRange;
        var startLine = Math.Max (range.Start.Line - maxLinesForFolding, 0);
        var endLine = Math.Min (range.End.Line + maxLinesForFolding, Math.Max (range.End.Line, LinesCount - 1));
        var stack = new Stack<int>();
        for (var i = startLine; i <= endLine; i++)
        {
            var hasStartMarker = lines.LineHasFoldingStartMarker (i);
            var hasEndMarker = lines.LineHasFoldingEndMarker (i);

            if (hasEndMarker && hasStartMarker)
            {
                continue;
            }

            if (hasStartMarker)
            {
                stack.Push (i);
            }

            if (hasEndMarker)
            {
                var m = lines[i].FoldingEndMarker;
                while (stack.Count > 0)
                {
                    var iStartLine = stack.Pop();
                    foldingPairs[iStartLine] = i;
                    if (m == lines[iStartLine].FoldingStartMarker)
                    {
                        break;
                    }
                }
            }
        }

        while (stack.Count > 0)
            foldingPairs[stack.Pop()] = endLine + 1;
    }

    /// <summary>
    /// Collapse text block
    /// </summary>
    public virtual void CollapseBlock (int fromLine, int toLine)
    {
        var from = Math.Min (fromLine, toLine);
        var to = Math.Max (fromLine, toLine);
        if (from == to)
        {
            return;
        }

        //hide lines
        for (var i = from + 1; i <= to; i++)
        {
            SetVisibleState (i, VisibleState.Hidden);
        }

        SetVisibleState (from, VisibleState.StartOfHiddenBlock);
        Invalidate();

        //Move caret outside
        from = Math.Min (fromLine, toLine);
        to = Math.Max (fromLine, toLine);
        var newLine = FindNextVisibleLine (to);
        if (newLine == to)
        {
            newLine = FindPrevVisibleLine (from);
        }

        Selection.Start = new Place (0, newLine);

        //
        needRecalc = true;
        Invalidate();
        OnVisibleRangeChanged();
    }


    internal int FindNextVisibleLine (int iLine)
    {
        if (iLine >= lines.Count - 1)
        {
            return iLine;
        }

        var old = iLine;
        do
            iLine++;
        while (iLine < lines.Count - 1 && LineInfos[iLine].VisibleState != VisibleState.Visible);

        if (LineInfos[iLine].VisibleState != VisibleState.Visible)
        {
            return old;
        }
        else
        {
            return iLine;
        }
    }


    internal int FindPrevVisibleLine (int iLine)
    {
        if (iLine <= 0)
        {
            return iLine;
        }

        var old = iLine;
        do
            iLine--;
        while (iLine > 0 && LineInfos[iLine].VisibleState != VisibleState.Visible);

        if (LineInfos[iLine].VisibleState != VisibleState.Visible)
        {
            return old;
        }
        else
        {
            return iLine;
        }
    }

    private VisualMarker? FindVisualMarkerForPoint (Point p)
    {
        foreach (var m in visibleMarkers)
        {
            if (m.rectangle.Contains (p))
            {
                return m;
            }
        }

        return null;
    }

    /// <summary>
    /// Insert TAB into front of seletcted lines.
    /// </summary>
    public virtual void IncreaseIndent()
    {
        if (Selection.Start == Selection.End)
        {
            if (!Selection.ReadOnly)
            {
                Selection.Start = new Place (this[Selection.Start.Line].StartSpacesCount, Selection.Start.Line);

                //insert tab as spaces
                var spaces = TabLength - Selection.Start.Column % TabLength;

                //replace mode? select forward chars
                if (IsReplaceMode)
                {
                    for (var i = 0; i < spaces; i++)
                    {
                        Selection.GoRight (true);
                    }

                    Selection.Inverse();
                }

                InsertText (new String (' ', spaces));
            }

            return;
        }

        var carretAtEnd = Selection.Start > Selection.End && !Selection.ColumnSelectionMode;

        var startChar = 0; // Only move selection when in 'ColumnSelectionMode'
        if (Selection.ColumnSelectionMode)
        {
            startChar = Math.Min (Selection.End.Column, Selection.Start.Column);
        }

        BeginUpdate();
        Selection.BeginUpdate();
        lines.Manager.BeginAutoUndoCommands();

        var old = Selection.Clone();
        lines.Manager.ExecuteCommand (new SelectCommand (TextSource)); //remember selection

        //
        Selection.Normalize();
        var currentSelection = this.Selection.Clone();
        var from = Selection.Start.Line;
        var to = Selection.End.Line;

        if (!Selection.ColumnSelectionMode)
        {
            if (Selection.End.Column == 0)
            {
                to--;
            }
        }

        for (var i = from; i <= to; i++)
        {
            if (lines[i].Count == 0)
            {
                continue;
            }

            Selection.Start = new Place (startChar, i);
            lines.Manager.ExecuteCommand (new InsertTextCommand (TextSource, new String (' ', TabLength)));
        }

        // Restore selection
        if (Selection.ColumnSelectionMode == false)
        {
            var newSelectionStartCharacterIndex = currentSelection.Start.Column + this.TabLength;
            var newSelectionEndCharacterIndex =
                currentSelection.End.Column + (currentSelection.End.Line == to ? this.TabLength : 0);
            this.Selection.Start = new Place (newSelectionStartCharacterIndex, currentSelection.Start.Line);
            this.Selection.End = new Place (newSelectionEndCharacterIndex, currentSelection.End.Line);
        }
        else
        {
            Selection = old;
        }

        lines.Manager.EndAutoUndoCommands();

        if (carretAtEnd)
        {
            Selection.Inverse();
        }

        needRecalc = true;
        Selection.EndUpdate();
        EndUpdate();
        Invalidate();
    }

    /// <summary>
    /// Remove TAB from front of seletcted lines.
    /// </summary>
    public virtual void DecreaseIndent()
    {
        if (Selection.Start.Line == Selection.End.Line)
        {
            DecreaseIndentOfSingleLine();
            return;
        }

        var startCharIndex = 0;
        if (Selection.ColumnSelectionMode)
        {
            startCharIndex = Math.Min (Selection.End.Column, Selection.Start.Column);
        }

        BeginUpdate();
        Selection.BeginUpdate();
        lines.Manager.BeginAutoUndoCommands();
        var old = Selection.Clone();
        lines.Manager.ExecuteCommand (new SelectCommand (TextSource)); //remember selection

        // Remember current selection infos
        var currentSelection = this.Selection.Clone();
        Selection.Normalize();
        var from = Selection.Start.Line;
        var to = Selection.End.Line;

        if (!Selection.ColumnSelectionMode)
        {
            if (Selection.End.Column == 0)
            {
                to--;
            }
        }

        var numberOfDeletedWhitespacesOfFirstLine = 0;
        var numberOfDeletetWhitespacesOfLastLine = 0;

        for (var i = from; i <= to; i++)
        {
            if (startCharIndex > lines[i].Count)
            {
                continue;
            }

            // Select first characters from the line
            var endIndex = Math.Min (this.lines[i].Count, startCharIndex + this.TabLength);
            var wasteText = this.lines[i].Text.Substring (startCharIndex, endIndex - startCharIndex);

            // Only select the first whitespace characters
            endIndex = Math.Min (endIndex, startCharIndex + wasteText.Length - wasteText.TrimStart().Length);

            // Select the characters to remove
            this.Selection = new TextRange (this, new Place (startCharIndex, i), new Place (endIndex, i));

            // Remember characters to remove for first and last line
            var numberOfWhitespacesToRemove = endIndex - startCharIndex;
            if (i == currentSelection.Start.Line)
            {
                numberOfDeletedWhitespacesOfFirstLine = numberOfWhitespacesToRemove;
            }

            if (i == currentSelection.End.Line)
            {
                numberOfDeletetWhitespacesOfLastLine = numberOfWhitespacesToRemove;
            }

            // Remove marked/selected whitespace characters
            if (!Selection.IsEmpty)
            {
                this.ClearSelected();
            }
        }

        // Restore selection
        if (Selection.ColumnSelectionMode == false)
        {
            var newSelectionStartCharacterIndex =
                Math.Max (0, currentSelection.Start.Column - numberOfDeletedWhitespacesOfFirstLine);
            var newSelectionEndCharacterIndex =
                Math.Max (0, currentSelection.End.Column - numberOfDeletetWhitespacesOfLastLine);
            this.Selection.Start = new Place (newSelectionStartCharacterIndex, currentSelection.Start.Line);
            this.Selection.End = new Place (newSelectionEndCharacterIndex, currentSelection.End.Line);
        }
        else
        {
            Selection = old;
        }

        lines.Manager.EndAutoUndoCommands();

        needRecalc = true;
        Selection.EndUpdate();
        EndUpdate();
        Invalidate();
    }

    /// <summary>
    /// Remove TAB in front of the caret ot the selected line.
    /// </summary>
    protected virtual void DecreaseIndentOfSingleLine()
    {
        if (this.Selection.Start.Line != this.Selection.End.Line)
        {
            return;
        }

        // Remeber current selection infos
        var currentSelection = this.Selection.Clone();
        var currentLineIndex = this.Selection.Start.Line;
        var currentLeftSelectionStartIndex = Math.Min (this.Selection.Start.Column, this.Selection.End.Column);

        // Determine number of whitespaces to remove
        var lineText = this.lines[currentLineIndex].Text;
        var whitespacesLeftOfSelectionStartMatch =
            new Regex (@"\s*", RegexOptions.RightToLeft).Match (lineText, currentLeftSelectionStartIndex);
        var leftOffset = whitespacesLeftOfSelectionStartMatch.Index;
        var countOfWhitespaces = whitespacesLeftOfSelectionStartMatch.Length;
        var numberOfCharactersToRemove = 0;
        if (countOfWhitespaces > 0)
        {
            var remainder = this.TabLength > 0
                ? currentLeftSelectionStartIndex % this.TabLength
                : 0;
            numberOfCharactersToRemove = remainder != 0
                ? Math.Min (remainder, countOfWhitespaces)
                : Math.Min (this.TabLength, countOfWhitespaces);
        }

        // Remove whitespaces if available
        if (numberOfCharactersToRemove > 0)
        {
            // Start selection update
            this.BeginUpdate();
            this.Selection.BeginUpdate();
            lines.Manager.BeginAutoUndoCommands();
            lines.Manager.ExecuteCommand (new SelectCommand (TextSource)); //remember selection

            // Remove whitespaces
            this.Selection.Start = new Place (leftOffset, currentLineIndex);
            this.Selection.End = new Place (leftOffset + numberOfCharactersToRemove, currentLineIndex);
            ClearSelected();

            // Restore selection
            var newSelectionStartCharacterIndex = currentSelection.Start.Column - numberOfCharactersToRemove;
            var newSelectionEndCharacterIndex = currentSelection.End.Column - numberOfCharactersToRemove;
            this.Selection.Start = new Place (newSelectionStartCharacterIndex, currentLineIndex);
            this.Selection.End = new Place (newSelectionEndCharacterIndex, currentLineIndex);

            lines.Manager.ExecuteCommand (new SelectCommand (TextSource)); //remember selection

            // End selection update
            lines.Manager.EndAutoUndoCommands();
            this.Selection.EndUpdate();
            this.EndUpdate();
        }

        Invalidate();
    }


    /// <summary>
    /// Insert autoindents into selected lines
    /// </summary>
    public virtual void DoAutoIndent()
    {
        if (Selection.ColumnSelectionMode)
        {
            return;
        }

        var r = Selection.Clone();
        r.Normalize();

        //
        BeginUpdate();
        Selection.BeginUpdate();
        lines.Manager.BeginAutoUndoCommands();

        //
        for (var i = r.Start.Line; i <= r.End.Line; i++)
        {
            DoAutoIndent (i);
        }

        //
        lines.Manager.EndAutoUndoCommands();
        Selection.Start = r.Start;
        Selection.End = r.End;
        Selection.Expand();

        //
        Selection.EndUpdate();
        EndUpdate();
    }

    /// <summary>
    /// Insert prefix into front of seletcted lines
    /// </summary>
    public virtual void InsertLinePrefix (string prefix)
    {
        var from = Math.Min (Selection.Start.Line, Selection.End.Line);
        var to = Math.Max (Selection.Start.Line, Selection.End.Line);
        BeginUpdate();
        Selection.BeginUpdate();
        lines.Manager.BeginAutoUndoCommands();
        lines.Manager.ExecuteCommand (new SelectCommand (TextSource));
        var spaces = GetMinStartSpacesCount (from, to);
        for (var i = from; i <= to; i++)
        {
            Selection.Start = new Place (spaces, i);
            lines.Manager.ExecuteCommand (new InsertTextCommand (TextSource, prefix));
        }

        Selection.Start = new Place (0, from);
        Selection.End = new Place (lines[to].Count, to);
        needRecalc = true;
        lines.Manager.EndAutoUndoCommands();
        Selection.EndUpdate();
        EndUpdate();
        Invalidate();
    }

    /// <summary>
    /// Remove prefix from front of selected lines
    /// This method ignores forward spaces of the line
    /// </summary>
    public virtual void RemoveLinePrefix (string prefix)
    {
        var from = Math.Min (Selection.Start.Line, Selection.End.Line);
        var to = Math.Max (Selection.Start.Line, Selection.End.Line);
        BeginUpdate();
        Selection.BeginUpdate();
        lines.Manager.BeginAutoUndoCommands();
        lines.Manager.ExecuteCommand (new SelectCommand (TextSource));
        for (var i = from; i <= to; i++)
        {
            var text = lines[i].Text;
            var trimmedText = text.TrimStart();
            if (trimmedText.StartsWith (prefix))
            {
                var spaces = text.Length - trimmedText.Length;
                Selection.Start = new Place (spaces, i);
                Selection.End = new Place (spaces + prefix.Length, i);
                ClearSelected();
            }
        }

        Selection.Start = new Place (0, from);
        Selection.End = new Place (lines[to].Count, to);
        needRecalc = true;
        lines.Manager.EndAutoUndoCommands();
        Selection.EndUpdate();
        EndUpdate();
    }

    /// <summary>
    /// Begins AutoUndo block.
    /// All changes of text between BeginAutoUndo() and EndAutoUndo() will be canceled in one operation Undo.
    /// </summary>
    public void BeginAutoUndo()
    {
        lines.Manager.BeginAutoUndoCommands();
    }

    /// <summary>
    /// Ends AutoUndo block.
    /// All changes of text between BeginAutoUndo() and EndAutoUndo() will be canceled in one operation Undo.
    /// </summary>
    public void EndAutoUndo()
    {
        lines.Manager.EndAutoUndoCommands();
    }

    /// <summary>
    ///
    /// </summary>
    public virtual void OnVisualMarkerClick (MouseEventArgs args, StyleVisualMarker marker)
    {
        VisualMarkerClick?.Invoke (this, new VisualMarkerEventArgs (marker.Style, marker, args));

        marker.Style.OnVisualMarkerClick (this, new VisualMarkerEventArgs (marker.Style, marker, args));
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnMarkerClick (MouseEventArgs args, VisualMarker marker)
    {
        switch (marker)
        {
            case StyleVisualMarker visualMarker:
                OnVisualMarkerClick (args, visualMarker);
                return;

            case CollapseFoldingMarker foldingMarker:
                CollapseFoldingBlock (foldingMarker.iLine);
                return;

            case ExpandFoldingMarker expandFoldingMarker:
                ExpandFoldedBlock (expandFoldingMarker.iLine);
                return;

            case FoldedAreaMarker areaMarker:
            {
                //select folded block
                var iStart = areaMarker.iLine;
                var iEnd = FindEndOfFoldingBlock (iStart);
                if (iEnd < 0)
                {
                    return;
                }

                Selection.BeginUpdate();
                Selection.Start = new Place (0, iStart);
                Selection.End = new Place (lines[iEnd].Count, iEnd);
                Selection.EndUpdate();
                Invalidate();
                return;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnMarkerDoubleClick (VisualMarker marker)
    {
        if (marker is FoldedAreaMarker areaMarker)
        {
            ExpandFoldedBlock (areaMarker.iLine);
            Invalidate();
        }
    }

    private void ClearBracketsPositions()
    {
        leftBracketPosition = null!;
        rightBracketPosition = null!;
        leftBracketPosition2 = null!;
        rightBracketPosition2 = null!;
    }

    /// <summary>
    /// Highlights brackets around caret
    /// </summary>
    private void HighlightBrackets
        (
            char leftBracket,
            char rightBracket,
            ref TextRange leftBracketPosition,
            ref TextRange rightBracketPosition
        )
    {
        switch (BracketsHighlightStrategy)
        {
            case BracketsHighlightStrategy.Strategy1:
                HighlightBrackets1 (leftBracket, rightBracket, ref leftBracketPosition, ref rightBracketPosition);
                break;

            case BracketsHighlightStrategy.Strategy2:
                HighlightBrackets2 (leftBracket, rightBracket, ref leftBracketPosition, ref rightBracketPosition);
                break;
        }
    }

    private void HighlightBrackets1
        (
            char leftBracket,
            char rightBracket,
            ref TextRange leftBracketPosition,
            ref TextRange rightBracketPosition
        )
    {
        if (!Selection.IsEmpty)
        {
            return;
        }

        if (LinesCount == 0)
        {
            return;
        }

        //
        var oldLeftBracketPosition = leftBracketPosition;
        var oldRightBracketPosition = rightBracketPosition;
        var range = GetBracketsRange (Selection.Start, leftBracket, rightBracket, true);

        if (range != null)
        {
            leftBracketPosition = new TextRange (this, range.Start, new Place (range.Start.Column + 1, range.Start.Line));
            rightBracketPosition = new TextRange (this, new Place (range.End.Column - 1, range.End.Line), range.End);
        }

        if (oldLeftBracketPosition != leftBracketPosition ||
            oldRightBracketPosition != rightBracketPosition)
        {
            Invalidate();
        }
    }

    /// <summary>
    /// Returns range between brackets (or null if not found)
    /// </summary>
    public TextRange? GetBracketsRange (Place placeInsideBrackets, char leftBracket, char rightBracket, bool includeBrackets)
    {
        var startRange = new TextRange (this, placeInsideBrackets, placeInsideBrackets);
        var range = startRange.Clone();

        TextRange? leftBracketPosition = null;
        TextRange? rightBracketPosition = null;

        var counter = 0;
        var maxIterations = maxBracketSearchIterations;
        while (range.GoLeftThroughFolded()) //move caret left
        {
            if (range.CharAfterStart == leftBracket)
            {
                counter++;
            }

            if (range.CharAfterStart == rightBracket)
            {
                counter--;
            }

            if (counter == 1)
            {
                range.Start = new Place (range.Start.Column + (!includeBrackets ? 1 : 0), range.Start.Line);
                leftBracketPosition = range;
                break;
            }

            //
            maxIterations--;
            if (maxIterations <= 0)
            {
                break;
            }
        }

        //
        range = startRange.Clone();
        counter = 0;
        maxIterations = maxBracketSearchIterations;
        do
        {
            if (range.CharAfterStart == leftBracket)
            {
                counter++;
            }

            if (range.CharAfterStart == rightBracket)
            {
                counter--;
            }

            if (counter == -1)
            {
                range.End = new Place (range.Start.Column + (includeBrackets ? 1 : 0), range.Start.Line);
                rightBracketPosition = range;
                break;
            }

            //
            maxIterations--;
            if (maxIterations <= 0)
            {
                break;
            }
        } while (range.GoRightThroughFolded()); //move caret right

        if (leftBracketPosition != null && rightBracketPosition != null)
        {
            return new TextRange (this, leftBracketPosition.Start, rightBracketPosition.End);
        }
        else
        {
            return null;
        }
    }

    private void HighlightBrackets2
        (
            char leftBracket,
            char rightBracket,
            ref TextRange leftBracketPosition,
            ref TextRange rightBracketPosition
        )
    {
        if (!Selection.IsEmpty)
        {
            return;
        }

        if (LinesCount == 0)
        {
            return;
        }

        //
        var oldLeftBracketPosition = leftBracketPosition;
        var oldRightBracketPosition = rightBracketPosition;
        var range = Selection.Clone(); //need clone because we will move caret

        var found = false;
        var counter = 0;
        var maxIterations = maxBracketSearchIterations;
        if (range.CharBeforeStart == rightBracket)
        {
            rightBracketPosition = new TextRange (this, range.Start.Column - 1, range.Start.Line, range.Start.Column,
                range.Start.Line);
            while (range.GoLeftThroughFolded()) //move caret left
            {
                if (range.CharAfterStart == leftBracket)
                {
                    counter++;
                }

                if (range.CharAfterStart == rightBracket)
                {
                    counter--;
                }

                if (counter == 0)
                {
                    //highlighting
                    range.End = new Place (range.Start.Column + 1, range.Start.Line);
                    leftBracketPosition = range;
                    found = true;
                    break;
                }

                //
                maxIterations--;
                if (maxIterations <= 0)
                {
                    break;
                }
            }
        }

        //
        range = Selection.Clone(); //need clone because we will move caret
        counter = 0;
        maxIterations = maxBracketSearchIterations;
        if (!found)
        {
            if (range.CharAfterStart == leftBracket)
            {
                leftBracketPosition = new TextRange (this, range.Start.Column, range.Start.Line, range.Start.Column + 1,
                    range.Start.Line);
                do
                {
                    if (range.CharAfterStart == leftBracket)
                    {
                        counter++;
                    }

                    if (range.CharAfterStart == rightBracket)
                    {
                        counter--;
                    }

                    if (counter == 0)
                    {
                        //highlighting
                        range.End = new Place (range.Start.Column + 1, range.Start.Line);
                        rightBracketPosition = range;
                        found = true;
                        break;
                    }

                    //
                    maxIterations--;
                    if (maxIterations <= 0)
                    {
                        break;
                    }
                } while (range.GoRightThroughFolded()); //move caret right
            }
        }

        if (oldLeftBracketPosition != leftBracketPosition || oldRightBracketPosition != rightBracketPosition)
        {
            Invalidate();
        }
    }

    /// <summary>
    /// Selectes next fragment for given regex.
    /// </summary>
    public bool SelectNext (string regexPattern, bool backward = false, RegexOptions options = RegexOptions.None)
    {
        var sel = Selection.Clone();
        sel.Normalize();
        var range1 = backward ? new TextRange (this, Range.Start, sel.Start) : new TextRange (this, sel.End, Range.End);

        TextRange? res = null;
        foreach (var r in range1.GetRanges (regexPattern, options))
        {
            res = r;
            if (!backward)
            {
                break;
            }
        }

        if (res == null)
        {
            return false;
        }

        Selection = res;
        Invalidate();
        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="args"></param>
    public virtual void OnSyntaxHighlight (TextChangedEventArgs args)
    {
        TextRange range;

        switch (HighlightingRangeType)
        {
            case HighlightingRangeType.VisibleRange:
                range = VisibleRange.GetUnionWith (args.ChangedRange);
                break;

            case HighlightingRangeType.AllTextRange:
                range = Range;
                break;

            default:
                range = args.ChangedRange;
                break;
        }

        if (SyntaxHighlighter != null)
        {
            if (Language == Language.Custom && !string.IsNullOrEmpty (DescriptionFile))
            {
                SyntaxHighlighter.HighlightSyntax (DescriptionFile, range);
            }
            else
            {
                SyntaxHighlighter.HighlightSyntax (Language, range);
            }
        }
    }

    private void InitializeComponent()
    {
        SuspendLayout();

        //
        // FastColoredTextBox
        //
        Name = "FastColoredTextBox";
        ResumeLayout (false);
    }

    /// <summary>
    /// Prints range of text
    /// </summary>
    public virtual void Print
        (
            TextRange? range,
            PrintDialogSettings settings
        )
    {
        //prepare export with wordwrapping
        var exporter = new ExportToHtml
        {
            UseBr = true,
            UseForwardNbsp = true,
            UseNbsp = true,
            UseStyleTag = false,
            IncludeLineNumbers = settings.IncludeLineNumbers
        };

        if (range == null)
        {
            range = Range;
        }

        if (range.Text == string.Empty)
        {
            return;
        }

        //change visible range
        _visibleRange = range;
        try
        {
            //call handlers for VisibleRange
            VisibleRangeChanged?.Invoke (this, EventArgs.Empty);

            VisibleRangeChangedDelayed?.Invoke (this, EventArgs.Empty);
        }
        finally
        {
            //restore visible range
            _visibleRange = null;
        }

        //generate HTML
        var HTML = exporter.GetHtml (range);
        HTML = "<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=UTF-8\"><head><title>" +
               PrepareHtmlText (settings.Title) + "</title></head>" + HTML + "<br>" + SelectHTMLRangeScript();
        var tempFile = Path.GetTempPath() + "fctb.html";
        File.WriteAllText (tempFile, HTML);

        //clear wb page setup settings
        SetPageSetupSettings (settings);

        //create wb
        var wb = new WebBrowser();
        wb.Tag = settings;
        wb.Visible = false;
        wb.Location = new Point (-1000, -1000);
        wb.Parent = this;
        wb.StatusTextChanged += wb_StatusTextChanged;
        wb.Navigate (tempFile);
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual string PrepareHtmlText (string s)
    {
        return s.Replace ("<", "&lt;").Replace (">", "&gt;").Replace ("&", "&amp;");
    }

    private void wb_StatusTextChanged
        (
            object? sender,
            EventArgs e
        )
    {
        var wb = sender as WebBrowser;
        if (wb!.StatusText.Contains ("#print"))
        {
            var settings = wb.Tag as PrintDialogSettings;
            try
            {
                //show print dialog
                if (settings!.ShowPrintPreviewDialog)
                {
                    wb.ShowPrintPreviewDialog();
                }
                else
                {
                    if (settings.ShowPageSetupDialog)
                    {
                        wb.ShowPageSetupDialog();
                    }

                    if (settings.ShowPrintDialog)
                    {
                        wb.ShowPrintDialog();
                    }
                    else
                    {
                        wb.Print();
                    }
                }
            }
            finally
            {
                //destroy webbrowser
                wb.Parent = null;
                wb.Dispose();
            }
        }
    }

    /// <summary>
    /// Prints all text
    /// </summary>
    public void Print (PrintDialogSettings settings)
    {
        Print (Range, settings);
    }

    /// <summary>
    /// Prints all text, without any dialog windows
    /// </summary>
    public void Print()
    {
        Print (Range,
            new PrintDialogSettings
                { ShowPageSetupDialog = false, ShowPrintDialog = false, ShowPrintPreviewDialog = false });
    }

    private string SelectHTMLRangeScript()
    {
        var sel = Selection.Clone();
        sel.Normalize();
        var start = PlaceToPosition (sel.Start) - sel.Start.Line;
        var len = sel.Text.Length - (sel.End.Line - sel.Start.Line);
        return string.Format (
            @"<script type=""text/javascript"">
try{{
    var sel = document.selection;
    var rng = sel.createRange();
    rng.moveStart(""character"", {0});
    rng.moveEnd(""character"", {1});
    rng.select();
}}catch(ex){{}}
window.status = ""#print"";
</script>",
            start, len);
    }

    private static void SetPageSetupSettings (PrintDialogSettings settings)
    {
        var key = Registry.CurrentUser.OpenSubKey (@"Software\Microsoft\Internet Explorer\PageSetup", true);
        if (key != null)
        {
            key.SetValue ("footer", settings.Footer);
            key.SetValue ("header", settings.Header);
        }
    }

    /// <inheritdoc cref="ContainerControl.Dispose(bool)"/>
    protected override void Dispose (bool disposing)
    {
        base.Dispose (disposing);
        if (disposing)
        {
            SyntaxHighlighter?.Dispose();

            _timer1.Dispose();
            _timer2.Dispose();
            middleClickScrollingTimer.Dispose();

            findForm?.Dispose();

            replaceForm?.Dispose();
            /*
            if (Font != null)
                Font.Dispose();

            if (originalFont != null)
                originalFont.Dispose();*/

            TextSource?.Dispose();

            ToolTip?.Dispose();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="eventArgs"></param>
    protected virtual void OnPaintLine (PaintLineEventArgs eventArgs)
    {
        PaintLine?.Invoke (this, eventArgs);
    }

    internal void OnLineInserted (int index)
    {
        OnLineInserted (index, 1);
    }

    internal void OnLineInserted (int index, int count)
    {
        LineInserted?.Invoke (this, new LineInsertedEventArgs (index, count));
    }

    internal void OnLineRemoved (int index, int count, List<int> removedLineIds)
    {
        if (count > 0)
        {
            LineRemoved?.Invoke (this, new LineRemovedEventArgs (index, count, removedLineIds));
        }
    }

    /// <summary>
    /// Open text file
    /// </summary>
    public void OpenFile (string fileName, Encoding enc)
    {
        var ts = CreateTextSource();
        try
        {
            InitTextSource (ts);
            Text = File.ReadAllText (fileName, enc);
            ClearUndo();
            IsChanged = false;
            OnVisibleRangeChanged();
        }
        catch
        {
            InitTextSource (CreateTextSource());
            lines.InsertLine (0, TextSource.CreateLine());
            IsChanged = false;
            throw;
        }

        Selection.Start = Place.Empty;
        DoSelectionVisible();
    }

    /// <summary>
    /// Open text file (with automatic encoding detector)
    /// </summary>
    public void OpenFile (string fileName)
    {
        try
        {
            var enc = EncodingDetector.DetectTextFileEncoding (fileName);
            if (enc != null)
            {
                OpenFile (fileName, enc);
            }
            else
            {
                OpenFile (fileName, Encoding.Default);
            }
        }
        catch
        {
            InitTextSource (CreateTextSource());
            lines.InsertLine (0, TextSource.CreateLine());
            IsChanged = false;
            throw;
        }
    }

    /// <summary>
    /// Open file binding mode
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="enc"></param>
    public void OpenBindingFile (string fileName, Encoding enc)
    {
        var fts = new FileTextSource (this);
        try
        {
            InitTextSource (fts);
            fts.OpenFile (fileName, enc);
            IsChanged = false;
            OnVisibleRangeChanged();
        }
        catch
        {
            fts.CloseFile();
            InitTextSource (CreateTextSource());
            lines.InsertLine (0, TextSource.CreateLine());
            IsChanged = false;
            throw;
        }

        Invalidate();
    }

    /// <summary>
    /// Close file binding mode
    /// </summary>
    public void CloseBindingFile()
    {
        if (lines is FileTextSource fts)
        {
            fts.CloseFile();

            InitTextSource (CreateTextSource());
            lines.InsertLine (0, TextSource.CreateLine());
            IsChanged = false;
            Invalidate();
        }
    }

    /// <summary>
    /// Save text to the file
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="enc"></param>
    public void SaveToFile (string fileName, Encoding enc)
    {
        lines.SaveToFile (fileName, enc);
        IsChanged = false;
        OnVisibleRangeChanged();
        UpdateScrollbars();
    }

    /// <summary>
    /// Set VisibleState of line
    /// </summary>
    public void SetVisibleState (int iLine, VisibleState state)
    {
        var li = LineInfos[iLine];
        li.VisibleState = state;
        LineInfos[iLine] = li;
        needRecalc = true;
    }

    /// <summary>
    /// Returns VisibleState of the line
    /// </summary>
    public VisibleState GetVisibleState (int iLine)
    {
        return LineInfos[iLine].VisibleState;
    }

    /// <summary>
    /// Shows Goto dialog form
    /// </summary>
    public void ShowGoToDialog()
    {
        var form = new GoToForm();
        form.TotalLineCount = LinesCount;
        form.SelectedLineNumber = Selection.Start.Line + 1;

        if (form.ShowDialog() == DialogResult.OK)
        {
            SetSelectedLine (form.SelectedLineNumber);
        }
    }

    /// <summary>
    /// Set current line number and make it visible
    /// </summary>
    /// <param name="lineNumberToSelect"></param>
    public void SetSelectedLine (int lineNumberToSelect)
    {
        var line = Math.Min (LinesCount - 1, Math.Max (0, lineNumberToSelect - 1));
        Selection = new TextRange (this, 0, line, 0, line);
        DoSelectionVisible();
    }

    /// <summary>
    /// Occurs when undo/redo stack is changed
    /// </summary>
    public void OnUndoRedoStateChanged()
    {
        UndoRedoStateChanged?.Invoke (this, EventArgs.Empty);
    }

    /// <summary>
    /// Search lines by regex pattern
    /// </summary>
    public List<int> FindLines (string searchPattern, RegexOptions options)
    {
        var iLines = new List<int>();
        foreach (var r in Range.GetRangesByLines (searchPattern, options))
        {
            iLines.Add (r.Start.Line);
        }

        return iLines;
    }

    /// <summary>
    /// Removes given lines
    /// </summary>
    public void RemoveLines (List<int> iLines)
    {
        TextSource.Manager.ExecuteCommand (new RemoveLinesCommand (TextSource, iLines));
        if (iLines.Count > 0)
        {
            IsChanged = true;
        }

        if (LinesCount == 0)
        {
            Text = "";
        }

        NeedRecalc();
        Invalidate();
    }

    void ISupportInitialize.BeginInit()
    {
        //
    }

    void ISupportInitialize.EndInit()
    {
        OnTextChanged();
        Selection.Start = Place.Empty;
        DoCaretVisible();
        IsChanged = false;
        ClearUndo();
    }

    #region Drag and drop

    private bool IsDragDrop { get; set; }


    /// <inheritdoc cref="Control.OnDragEnter"/>
    protected override void OnDragEnter (DragEventArgs eventArgs)
    {
        if (eventArgs.Data!.GetDataPresent (DataFormats.Text) && AllowDrop)
        {
            eventArgs.Effect = DragDropEffects.Copy;
            IsDragDrop = true;
        }

        base.OnDragEnter (eventArgs);
    }

    /// <inheritdoc cref="Control.OnDragDrop"/>
    protected override void OnDragDrop (DragEventArgs eventArgs)
    {
        if (ReadOnly || !AllowDrop)
        {
            IsDragDrop = false;
            return;
        }

        if (eventArgs.Data!.GetDataPresent (DataFormats.Text))
        {
            ParentForm?.Activate();

            Focus();
            var p = PointToClient (new Point (eventArgs.X, eventArgs.Y));
            var text = eventArgs.Data.GetData (DataFormats.Text)!.ToString();
            var place = PointToPlace (p);
            DoDragDrop (place, text);
            IsDragDrop = false;
        }

        base.OnDragDrop (eventArgs);
    }

    private void DoDragDrop_old (Place place, string text)
    {
        var insertRange = new TextRange (this, place, place);

        // Abort, if insertRange is read only
        if (insertRange.ReadOnly)
        {
            return;
        }

        // Abort, if dragged range contains target place
        if (DraggedRange != null && DraggedRange.Contains (place))
        {
            return;
        }

        // Determine, if the dragged string should be copied or moved
        var copyMode =
            DraggedRange == null || // drag from outside
            DraggedRange.ReadOnly || // dragged range is read only
            (ModifierKeys & Keys.Control) != Keys.None;

        //drag from outside?
        if (DraggedRange == null)
        {
            Selection.BeginUpdate();

            // Insert text
            Selection.Start = place;
            InsertText (text);

            // Select inserted text
            Selection = new TextRange (this, place, Selection.Start);
            Selection.EndUpdate();
            return;
        }

        //drag from me
        Place caretPositionAfterInserting;
        BeginAutoUndo();
        Selection.BeginUpdate();

        //remember dragged selection for undo/redo
        Selection = DraggedRange;
        lines.Manager.ExecuteCommand (new SelectCommand (lines));

        //
        if (DraggedRange.ColumnSelectionMode)
        {
            DraggedRange.Normalize();
            insertRange =
                new TextRange (this, place,
                        new Place (place.Column, place.Line + DraggedRange.End.Line - DraggedRange.Start.Line))
                    { ColumnSelectionMode = true };
            for (var i = LinesCount; i <= insertRange.End.Line; i++)
            {
                Selection.GoLast (false);
                InsertChar ('\n');
            }
        }

        if (!insertRange.ReadOnly)
        {
            if (place < DraggedRange.Start)
            {
                // Delete dragged range if not in copy mode
                if (copyMode == false)
                {
                    Selection = DraggedRange;
                    ClearSelected();
                }

                // Insert text
                Selection = insertRange;
                Selection.ColumnSelectionMode = insertRange.ColumnSelectionMode;
                InsertText (text);
                caretPositionAfterInserting = Selection.Start;
            }
            else
            {
                // Insert text
                Selection = insertRange;
                Selection.ColumnSelectionMode = insertRange.ColumnSelectionMode;
                InsertText (text);
                caretPositionAfterInserting = Selection.Start;
                var lineLength = this[caretPositionAfterInserting.Line].Count;

                // Delete dragged range if not in copy mode
                if (copyMode == false)
                {
                    Selection = DraggedRange;
                    ClearSelected();
                }

                var shift = lineLength - this[caretPositionAfterInserting.Line].Count;
                caretPositionAfterInserting.Column = caretPositionAfterInserting.Column - shift;
                place.Column = place.Column - shift;
            }

            // Select inserted text
            if (!DraggedRange.ColumnSelectionMode)
            {
                Selection = new TextRange (this, place, caretPositionAfterInserting);
            }
            else
            {
                DraggedRange.Normalize();
                Selection = new TextRange (this, place,
                        new Place (place.Column + DraggedRange.End.Column - DraggedRange.Start.Column,
                            place.Line + DraggedRange.End.Line - DraggedRange.Start.Line))
                    { ColumnSelectionMode = true };
            }
        }

        Selection.EndUpdate();
        EndAutoUndo();
        DraggedRange = null;
    }

    protected virtual void DoDragDrop (Place place, string? text)
    {
        var insertRange = new TextRange (this, place, place);

        // Abort, if insertRange is read only
        if (insertRange.ReadOnly)
        {
            return;
        }

        // Abort, if dragged range contains target place
        if (DraggedRange != null && DraggedRange.Contains (place))
        {
            return;
        }

        // Determine, if the dragged string should be copied or moved
        var copyMode =
            DraggedRange == null || // drag from outside
            DraggedRange.ReadOnly || // dragged range is read only
            (ModifierKeys & Keys.Control) != Keys.None;

        if (DraggedRange == null) //drag from outside
        {
            Selection.BeginUpdate();

            // Insert text
            Selection.Start = place;
            InsertText (text);

            // Select inserted text
            Selection = new TextRange (this, place, Selection.Start);
            Selection.EndUpdate();
        }
        else //drag from me
        {
            if (!DraggedRange.Contains (place))
            {
                BeginAutoUndo();

                //remember dragged selection for undo/redo
                Selection = DraggedRange;
                lines.Manager.ExecuteCommand (new SelectCommand (lines));

                //
                if (DraggedRange.ColumnSelectionMode)
                {
                    DraggedRange.Normalize();
                    insertRange =
                        new TextRange (this, place,
                                new Place (place.Column,
                                    place.Line + DraggedRange.End.Line - DraggedRange.Start.Line))
                            { ColumnSelectionMode = true };
                    for (var i = LinesCount; i <= insertRange.End.Line; i++)
                    {
                        Selection.GoLast (false);
                        InsertChar ('\n');
                    }
                }

                if (!insertRange.ReadOnly)
                {
                    if (place < DraggedRange.Start)
                    {
                        // Delete dragged range if not in copy mode
                        if (copyMode == false)
                        {
                            Selection = DraggedRange;
                            ClearSelected();
                        }

                        // Insert text
                        Selection = insertRange;
                        Selection.ColumnSelectionMode = insertRange.ColumnSelectionMode;
                        InsertText (text);
                    }
                    else
                    {
                        // Insert text
                        Selection = insertRange;
                        Selection.ColumnSelectionMode = insertRange.ColumnSelectionMode;
                        InsertText (text);

                        // Delete dragged range if not in copy mode
                        if (copyMode == false)
                        {
                            Selection = DraggedRange;
                            ClearSelected();
                        }
                    }
                }

                // Selection start and end position
                var startPosition = place;
                var endPosition = Selection.Start;

                // Correct selection
                var dR = DraggedRange.End > DraggedRange.Start // dragged selection
                    ? this.GetRange (DraggedRange.Start, DraggedRange.End)
                    : this.GetRange (DraggedRange.End, DraggedRange.Start);
                var tP = place; // targetPlace
                int tS_S_Line; // targetSelection.Start.iLine
                int tS_S_Char; // targetSelection.Start.iChar
                int tS_E_Line; // targetSelection.End.iLine
                int tS_E_Char; // targetSelection.End.iChar
                if (place > DraggedRange.Start && copyMode == false)
                {
                    if (DraggedRange.ColumnSelectionMode == false)
                    {
                        // Normal selection mode:

                        // Determine character/column position of target selection
                        if (dR.Start.Line != dR.End.Line) // If more then one line was selected/dragged ...
                        {
                            tS_S_Char = dR.End.Line != tP.Line
                                ? tP.Column
                                : dR.Start.Column + (tP.Column - dR.End.Column);
                            tS_E_Char = dR.End.Column;
                        }
                        else // only one line was selected/dragged
                        {
                            if (dR.End.Line == tP.Line)
                            {
                                tS_S_Char = tP.Column - dR.Text.Length;
                                tS_E_Char = tP.Column;
                            }
                            else
                            {
                                tS_S_Char = tP.Column;
                                tS_E_Char = tP.Column + dR.Text.Length;
                            }
                        }

                        // Determine line/row of target selection
                        if (dR.End.Line != tP.Line)
                        {
                            tS_S_Line = tP.Line - (dR.End.Line - dR.Start.Line);
                            tS_E_Line = tP.Line;
                        }
                        else
                        {
                            tS_S_Line = dR.Start.Line;
                            tS_E_Line = dR.End.Line;
                        }

                        startPosition = new Place (tS_S_Char, tS_S_Line);
                        endPosition = new Place (tS_E_Char, tS_E_Line);
                    }
                }


                // Select inserted text
                if (!DraggedRange.ColumnSelectionMode)
                {
                    Selection = new TextRange (this, startPosition, endPosition);
                }
                else
                {
                    if (copyMode == false &&
                        place.Line >= dR.Start.Line && place.Line <= dR.End.Line &&
                        place.Column >= dR.End.Column)
                    {
                        tS_S_Char = tP.Column - (dR.End.Column - dR.Start.Column);
                        tS_E_Char = tP.Column;
                    }
                    else
                    {
                        tS_S_Char = tP.Column;
                        tS_E_Char = tP.Column + (dR.End.Column - dR.Start.Column);
                    }

                    tS_S_Line = tP.Line;
                    tS_E_Line = tP.Line + (dR.End.Line - dR.Start.Line);

                    startPosition = new Place (tS_S_Char, tS_S_Line);
                    endPosition = new Place (tS_E_Char, tS_E_Line);
                    Selection = new TextRange (this, startPosition, endPosition)
                    {
                        ColumnSelectionMode = true
                    };
                }

                EndAutoUndo();
            }

            this._selection.Inverse();
            OnSelectionChanged();
        }

        DraggedRange = null;
    }

    /// <inheritdoc cref="Control.OnDragOver"/>
    protected override void OnDragOver (DragEventArgs e)
    {
        if (e.Data!.GetDataPresent (DataFormats.Text))
        {
            var p = PointToClient (new Point (e.X, e.Y));
            Selection.Start = PointToPlace (p);
            if (p.Y < 6 && VerticalScroll.Visible && VerticalScroll.Value > 0)
            {
                VerticalScroll.Value = Math.Max (0, VerticalScroll.Value - _charHeight);
            }

            DoCaretVisible();
            Invalidate();
        }

        base.OnDragOver (e);
    }

    /// <inheritdoc cref="Control.OnDragLeave"/>
    protected override void OnDragLeave (EventArgs e)
    {
        IsDragDrop = false;
        base.OnDragLeave (e);
    }

    #endregion

    #region MiddleClickScrolling

    private bool _middleClickScrollingActivated;
    private Point _middleClickScrollingOriginPoint;
    private Point _middleClickScrollingOriginScroll;
    private readonly Timer middleClickScrollingTimer = new ();
    private ScrollDirection middleClickScollDirection = ScrollDirection.None;

    /// <summary>
    /// Activates the scrolling mode (middle click button).
    /// </summary>
    /// <param name="e">MouseEventArgs</param>
    private void ActivateMiddleClickScrollingMode (MouseEventArgs e)
    {
        if (!_middleClickScrollingActivated)
        {
            if (!HorizontalScroll.Visible && !VerticalScroll.Visible)
            {
                if (ShowScrollBars)
                {
                    return;
                }
            }

            _middleClickScrollingActivated = true;
            _middleClickScrollingOriginPoint = e.Location;
            _middleClickScrollingOriginScroll = new Point (HorizontalScroll.Value, VerticalScroll.Value);
            middleClickScrollingTimer.Interval = 50;
            middleClickScrollingTimer.Enabled = true;
            Capture = true;

            // Refresh the control
            Refresh();

            // Disable drawing
            SendMessage (Handle, WM_SETREDRAW, 0, 0);
        }
    }

    /// <summary>
    /// Deactivates the scrolling mode (middle click button).
    /// </summary>
    private void DeactivateMiddleClickScrollingMode()
    {
        if (_middleClickScrollingActivated)
        {
            _middleClickScrollingActivated = false;
            middleClickScrollingTimer.Enabled = false;
            Capture = false;
            base.Cursor = defaultCursor;

            // Enable drawing
            SendMessage (Handle, WM_SETREDRAW, 1, 0);
            Invalidate();
        }
    }

    /// <summary>
    /// Restore scrolls
    /// </summary>
    private void RestoreScrollsAfterMiddleClickScrollingMode()
    {
        var xea = new ScrollEventArgs (ScrollEventType.ThumbPosition,
            HorizontalScroll.Value,
            _middleClickScrollingOriginScroll.X,
            ScrollOrientation.HorizontalScroll);
        OnScroll (xea);

        var yea = new ScrollEventArgs (ScrollEventType.ThumbPosition,
            VerticalScroll.Value,
            _middleClickScrollingOriginScroll.Y,
            ScrollOrientation.VerticalScroll);
        OnScroll (yea);
    }

    [DllImport ("user32.dll")]
    private static extern int SendMessage (IntPtr hwnd, int wMsg, int wParam, int lParam);

    private const int WM_SETREDRAW = 0xB;

    private void middleClickScrollingTimer_Tick (object? sender, EventArgs e)
    {
        if (IsDisposed)
        {
            return;
        }

        if (!_middleClickScrollingActivated)
        {
            return;
        }

        var currentMouseLocation = PointToClient (Cursor.Position);

        Capture = true;

        // Calculate angle and distance between current position point and origin point
        var distanceX = this._middleClickScrollingOriginPoint.X - currentMouseLocation.X;
        var distanceY = this._middleClickScrollingOriginPoint.Y - currentMouseLocation.Y;

        if (!VerticalScroll.Visible && ShowScrollBars)
        {
            distanceY = 0;
        }

        if (!HorizontalScroll.Visible && ShowScrollBars)
        {
            distanceX = 0;
        }

        var angleInDegree = 180 - Math.Atan2 (distanceY, distanceX) * 180 / Math.PI;
        var distance = Math.Sqrt (Math.Pow (distanceX, 2) + Math.Pow (distanceY, 2));

        // determine scrolling direction depending on the angle
        if (distance > 10)
        {
            if (angleInDegree is >= 325 or <= 35)
            {
                this.middleClickScollDirection = ScrollDirection.Right;
            }
            else if (angleInDegree <= 55)
            {
                this.middleClickScollDirection = ScrollDirection.Right | ScrollDirection.Up;
            }
            else if (angleInDegree <= 125)
            {
                this.middleClickScollDirection = ScrollDirection.Up;
            }
            else if (angleInDegree <= 145)
            {
                this.middleClickScollDirection = ScrollDirection.Up | ScrollDirection.Left;
            }
            else if (angleInDegree <= 215)
            {
                this.middleClickScollDirection = ScrollDirection.Left;
            }
            else if (angleInDegree <= 235)
            {
                this.middleClickScollDirection = ScrollDirection.Left | ScrollDirection.Down;
            }
            else if (angleInDegree <= 305)
            {
                this.middleClickScollDirection = ScrollDirection.Down;
            }
            else
            {
                this.middleClickScollDirection = ScrollDirection.Down | ScrollDirection.Right;
            }
        }
        else
        {
            this.middleClickScollDirection = ScrollDirection.None;
        }

        // Set mouse cursor
        switch (this.middleClickScollDirection)
        {
            case ScrollDirection.Right:
                base.Cursor = Cursors.PanEast;
                break;
            case ScrollDirection.Right | ScrollDirection.Up:
                base.Cursor = Cursors.PanNE;
                break;
            case ScrollDirection.Up:
                base.Cursor = Cursors.PanNorth;
                break;
            case ScrollDirection.Up | ScrollDirection.Left:
                base.Cursor = Cursors.PanNW;
                break;
            case ScrollDirection.Left:
                base.Cursor = Cursors.PanWest;
                break;
            case ScrollDirection.Left | ScrollDirection.Down:
                base.Cursor = Cursors.PanSW;
                break;
            case ScrollDirection.Down:
                base.Cursor = Cursors.PanSouth;
                break;
            case ScrollDirection.Down | ScrollDirection.Right:
                base.Cursor = Cursors.PanSE;
                break;
            default:
                base.Cursor = defaultCursor;
                return;
        }

        var xScrollOffset = (int)(-distanceX / 5.0);
        var yScrollOffset = (int)(-distanceY / 5.0);

        var xea = new ScrollEventArgs (
            xScrollOffset < 0 ? ScrollEventType.SmallIncrement : ScrollEventType.SmallDecrement,
            HorizontalScroll.Value,
            HorizontalScroll.Value + xScrollOffset,
            ScrollOrientation.HorizontalScroll);

        var yea = new ScrollEventArgs (
            yScrollOffset < 0 ? ScrollEventType.SmallDecrement : ScrollEventType.SmallIncrement,
            VerticalScroll.Value,
            VerticalScroll.Value + yScrollOffset,
            ScrollOrientation.VerticalScroll);

        if ((middleClickScollDirection & (ScrollDirection.Down | ScrollDirection.Up)) > 0)

            //DoScrollVertical(1 + Math.Abs(yScrollOffset), Math.Sign(distanceY));
        {
            OnScroll (yea, false);
        }

        if ((middleClickScollDirection & (ScrollDirection.Right | ScrollDirection.Left)) > 0)
        {
            OnScroll (xea);
        }

        // Enable drawing
        SendMessage (Handle, WM_SETREDRAW, 1, 0);

        // Refresh the control
        Refresh();

        // Disable drawing
        SendMessage (Handle, WM_SETREDRAW, 0, 0);
    }

    private void DrawMiddleClickScrolling (Graphics gr)
    {
        // If mouse scrolling mode activated draw the scrolling cursor image
        var ableToScrollVertically = this.VerticalScroll.Visible || !ShowScrollBars;
        var ableToScrollHorizontally = this.HorizontalScroll.Visible || !ShowScrollBars;

        // Calculate inverse color
        var inverseColor =
            Color.FromArgb (100, (byte)~this.BackColor.R, (byte)~this.BackColor.G, (byte)~this.BackColor.B);
        using (var inverseColorBrush = new SolidBrush (inverseColor))
        {
            var p = _middleClickScrollingOriginPoint;

            var state = gr.Save();

            gr.SmoothingMode = SmoothingMode.HighQuality;
            gr.TranslateTransform (p.X, p.Y);
            gr.FillEllipse (inverseColorBrush, -2, -2, 4, 4);

            if (ableToScrollVertically)
            {
                DrawTriangle (gr, inverseColorBrush);
            }

            gr.RotateTransform (90);
            if (ableToScrollHorizontally)
            {
                DrawTriangle (gr, inverseColorBrush);
            }

            gr.RotateTransform (90);
            if (ableToScrollVertically)
            {
                DrawTriangle (gr, inverseColorBrush);
            }

            gr.RotateTransform (90);
            if (ableToScrollHorizontally)
            {
                DrawTriangle (gr, inverseColorBrush);
            }

            gr.Restore (state);
        }
    }

    private void DrawTriangle (Graphics g, Brush brush)
    {
        const int size = 5;
        var points = new Point[] { new (size, 2 * size), new (0, 3 * size), new (-size, 2 * size) };
        g.FillPolygon (brush, points);
    }

    #endregion


    #region Nested type: LineYComparer

    private class LineYComparer : IComparer<LineInfo>
    {
        private readonly int Y;

        public LineYComparer (int Y)
        {
            this.Y = Y;
        }

        #region IComparer<LineInfo> Members

        public int Compare (LineInfo x, LineInfo y)
        {
            if (x.startY == -10)
            {
                return -y.startY.CompareTo (Y);
            }
            else
            {
                return x.startY.CompareTo (Y);
            }
        }

        #endregion
    }

    #endregion
}

/// <summary>
///
/// </summary>
public class LineInsertedEventArgs
    : EventArgs
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public LineInsertedEventArgs (int index, int count)
    {
        Index = index;
        Count = count;
    }

    /// <summary>
    /// Inserted line index
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Count of inserted lines
    /// </summary>
    public int Count { get; }
}

/// <summary>
///
/// </summary>
public class LineRemovedEventArgs
    : EventArgs
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    /// <param name="removedLineIds"></param>
    public LineRemovedEventArgs (int index, int count, List<int> removedLineIds)
    {
        Index = index;
        Count = count;
        RemovedLineUniqueIds = removedLineIds;
    }

    /// <summary>
    /// Removed line index
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Count of removed lines
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// UniqueIds of removed lines
    /// </summary>
    public List<int> RemovedLineUniqueIds { get; }
}

/// <summary>
/// TextChanged event argument
/// </summary>
public class TextChangedEventArgs
    : EventArgs
{
    /// <summary>
    /// Constructor
    /// </summary>
    public TextChangedEventArgs (TextRange changedRange)
    {
        ChangedRange = changedRange;
    }

    /// <summary>
    /// This range contains changed area of text
    /// </summary>
    public TextRange ChangedRange { get; set; }
}

/// <summary>
///
/// </summary>
public class TextChangingEventArgs
    : EventArgs
{
    /// <summary>
    ///
    /// </summary>
    public string? InsertingText { get; set; }

    /// <summary>
    /// Set to true if you want to cancel text inserting
    /// </summary>
    public bool Cancel { get; set; }
}

/// <summary>
///
/// </summary>
public class WordWrapNeededEventArgs
    : EventArgs
{
    /// <summary>
    ///
    /// </summary>
    public List<int> CutOffPositions { get; }

    /// <summary>
    ///
    /// </summary>
    public bool ImeAllowed { get; }

    /// <summary>
    ///
    /// </summary>
    public Line Line { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="cutOffPositions"></param>
    /// <param name="imeAllowed"></param>
    /// <param name="line"></param>

    public WordWrapNeededEventArgs (List<int> cutOffPositions, bool imeAllowed, Line line)
    {
        this.CutOffPositions = cutOffPositions;
        this.ImeAllowed = imeAllowed;
        this.Line = line;
    }
}

/// <summary>
///
/// </summary>
public enum WordWrapMode
{
    /// <summary>
    /// Word wrapping by control width
    /// </summary>
    WordWrapControlWidth,

    /// <summary>
    /// Word wrapping by preferred line width (PreferredLineWidth)
    /// </summary>
    WordWrapPreferredWidth,

    /// <summary>
    /// Char wrapping by control width
    /// </summary>
    CharWrapControlWidth,

    /// <summary>
    /// Char wrapping by preferred line width (PreferredLineWidth)
    /// </summary>
    CharWrapPreferredWidth,

    /// <summary>
    /// Custom wrap (by event WordWrapNeeded)
    /// </summary>
    Custom
}

/// <summary>
///
/// </summary>
public class AutoIndentEventArgs
    : EventArgs
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="iLine"></param>
    /// <param name="lineText"></param>
    /// <param name="prevLineText"></param>
    /// <param name="tabLength"></param>
    /// <param name="currentIndentation"></param>
    public AutoIndentEventArgs (int iLine, string lineText, string prevLineText, int tabLength, int currentIndentation)
    {
        this.iLine = iLine;
        LineText = lineText;
        PrevLineText = prevLineText;
        TabLength = tabLength;
        AbsoluteIndentation = currentIndentation;
    }

    /// <summary>
    ///
    /// </summary>
    public int iLine { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public int TabLength { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public string LineText { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public string PrevLineText { get; internal set; }

    /// <summary>
    /// Additional spaces count for this line, relative to previous line
    /// </summary>
    public int Shift { get; set; }

    /// <summary>
    /// Additional spaces count for next line, relative to previous line
    /// </summary>
    public int ShiftNextLines { get; set; }

    /// <summary>
    /// Absolute indentation of current line. You can change this property if you want to set absolute indentation.
    /// </summary>
    public int AbsoluteIndentation { get; set; }
}

/// <summary>
/// Type of highlighting
/// </summary>
public enum HighlightingRangeType
{
    /// <summary>
    /// Highlight only changed range of text. Highest performance.
    /// </summary>
    ChangedRange,

    /// <summary>
    /// Highlight visible range of text. Middle performance.
    /// </summary>
    VisibleRange,

    /// <summary>
    /// Highlight all (visible and invisible) text. Lowest performance.
    /// </summary>
    AllTextRange
}

/// <summary>
/// Strategy of search of end of folding block
/// </summary>
public enum FindEndOfFoldingBlockStrategy
{
    /// <summary>
    ///
    /// </summary>
    Strategy1,

    /// <summary>
    ///
    /// </summary>
    Strategy2
}

/// <summary>
/// Strategy of search of brackets to highlighting
/// </summary>
public enum BracketsHighlightStrategy
{
    /// <summary>
    ///
    /// </summary>
    Strategy1,

    /// <summary>
    ///
    /// </summary>
    Strategy2
}

/// <summary>
/// ToolTipNeeded event args
/// </summary>
public class ToolTipNeededEventArgs
    : EventArgs
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="place"></param>
    /// <param name="hoveredWord"></param>
    public ToolTipNeededEventArgs (Place place, string hoveredWord)
    {
        HoveredWord = hoveredWord;
        Place = place;
    }

    /// <summary>
    ///
    /// </summary>
    public Place Place { get; }

    /// <summary>
    ///
    /// </summary>
    public string HoveredWord { get; }

    /// <summary>
    ///
    /// </summary>
    public string ToolTipTitle { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ToolTipText { get; set; }

    /// <summary>
    ///
    /// </summary>
    public ToolTipIcon ToolTipIcon { get; set; }
}

/// <summary>
/// HintClick event args
/// </summary>
public class HintClickEventArgs
    : EventArgs
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="hint"></param>
    public HintClickEventArgs (Hint hint)
    {
        Hint = hint;
    }

    /// <summary>
    ///
    /// </summary>
    public Hint Hint { get; }
}

/// <summary>
/// CustomAction event args
/// </summary>
public class CustomActionEventArgs
    : EventArgs
{
    /// <summary>
    ///
    /// </summary>
    public ActionCode Action { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="action"></param>
    public CustomActionEventArgs (ActionCode action)
    {
        Action = action;
    }
}

/// <summary>
///
/// </summary>
public enum TextAreaBorderType
{
    /// <summary>
    ///
    /// </summary>
    None,

    /// <summary>
    ///
    /// </summary>
    Single,

    /// <summary>
    ///
    /// </summary>
    Shadow
}

/// <summary>
///
/// </summary>
[Flags]
public enum ScrollDirection : ushort
{
    /// <summary>
    ///
    /// </summary>
    None = 0,

    /// <summary>
    ///
    /// </summary>
    Left = 1,

    /// <summary>
    ///
    /// </summary>
    Right = 2,

    /// <summary>
    ///
    /// </summary>
    Up = 4,

    /// <summary>
    ///
    /// </summary>
    Down = 8
}

/// <summary>
///
/// </summary>
[Serializable]
public class ServiceColors
{
    /// <summary>
    ///
    /// </summary>
    public Color CollapseMarkerForeColor { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color CollapseMarkerBackColor { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color CollapseMarkerBorderColor { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color ExpandMarkerForeColor { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color ExpandMarkerBackColor { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Color ExpandMarkerBorderColor { get; set; }

    /// <summary>
    ///
    /// </summary>
    public ServiceColors()
    {
        CollapseMarkerForeColor = Color.Silver;
        CollapseMarkerBackColor = Color.White;
        CollapseMarkerBorderColor = Color.Silver;
        ExpandMarkerForeColor = Color.Red;
        ExpandMarkerBackColor = Color.White;
        ExpandMarkerBorderColor = Color.Silver;
    }
}

#if Styles32
    /// <summary>
    /// Style index mask (32 styles)
    /// </summary>
    [Flags]
    public enum StyleIndex : uint
    {
        None = 0,
        Style0 = 0x1,
        Style1 = 0x2,
        Style2 = 0x4,
        Style3 = 0x8,
        Style4 = 0x10,
        Style5 = 0x20,
        Style6 = 0x40,
        Style7 = 0x80,
        Style8 = 0x100,
        Style9 = 0x200,
        Style10 = 0x400,
        Style11 = 0x800,
        Style12 = 0x1000,
        Style13 = 0x2000,
        Style14 = 0x4000,
        Style15 = 0x8000,

        Style16 = 0x10000,
        Style17 = 0x20000,
        Style18 = 0x40000,
        Style19 = 0x80000,
        Style20 = 0x100000,
        Style21 = 0x200000,
        Style22 = 0x400000,
        Style23 = 0x800000,
        Style24 = 0x1000000,
        Style25 = 0x2000000,
        Style26 = 0x4000000,
        Style27 = 0x8000000,
        Style28 = 0x10000000,
        Style29 = 0x20000000,
        Style30 = 0x40000000,
        Style31 = 0x80000000,

        All = 0xffffffff
    }
#else
/// <summary>
/// Style index mask (16 styles)
/// </summary>
[Flags]
public enum StyleIndex : ushort
{
    /// <summary>
    ///
    /// </summary>
    None = 0,

    /// <summary>
    ///
    /// </summary>
    Style0 = 0x1,

    /// <summary>
    ///
    /// </summary>
    Style1 = 0x2,

    /// <summary>
    ///
    /// </summary>
    Style2 = 0x4,

    /// <summary>
    ///
    /// </summary>
    Style3 = 0x8,

    /// <summary>
    ///
    /// </summary>
    Style4 = 0x10,

    /// <summary>
    ///
    /// </summary>
    Style5 = 0x20,

    /// <summary>
    ///
    /// </summary>
    Style6 = 0x40,

    /// <summary>
    ///
    /// </summary>
    Style7 = 0x80,

    /// <summary>
    ///
    /// </summary>
    Style8 = 0x100,

    /// <summary>
    ///
    /// </summary>
    Style9 = 0x200,

    /// <summary>
    ///
    /// </summary>
    Style10 = 0x400,

    /// <summary>
    ///
    /// </summary>
    Style11 = 0x800,

    /// <summary>
    ///
    /// </summary>
    Style12 = 0x1000,

    /// <summary>
    ///
    /// </summary>
    Style13 = 0x2000,

    /// <summary>
    ///
    /// </summary>
    Style14 = 0x4000,

    /// <summary>
    ///
    /// </summary>
    Style15 = 0x8000,

    /// <summary>
    ///
    /// </summary>
    All = 0xffff
}
#endif
