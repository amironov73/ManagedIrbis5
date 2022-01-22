// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable VirtualMemberCallInConstructor

/* Hint.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Hint of FastColoredTextbox
/// </summary>
public class Hint
{
    #region Properties

    /// <summary>
    /// Text of simple hint
    /// </summary>
    public string? Text
    {
        get => HostPanel!.Text;
        set => HostPanel!.Text = value;
    }

    /// <summary>
    /// Linked range
    /// </summary>
    public TextRange Range { get; set; }

    /// <summary>
    /// Backcolor
    /// </summary>
    public Color BackColor
    {
        get => HostPanel!.BackColor;
        set => HostPanel!.BackColor = value;
    }

    /// <summary>
    /// Second backcolor
    /// </summary>
    public Color BackColor2
    {
        get => HostPanel!.BackColor2;
        set => HostPanel!.BackColor2 = value;
    }

    /// <summary>
    /// Border color
    /// </summary>
    public Color BorderColor
    {
        get => HostPanel!.BorderColor;
        set => HostPanel!.BorderColor = value;
    }

    /// <summary>
    /// Fore color
    /// </summary>
    public Color ForeColor
    {
        get => HostPanel!.ForeColor;
        set => HostPanel!.ForeColor = value;
    }

    /// <summary>
    /// Text alignment
    /// </summary>
    public StringAlignment TextAlignment
    {
        get => HostPanel!.TextAlignment;
        set => HostPanel!.TextAlignment = value;
    }

    /// <summary>
    /// Font
    /// </summary>
    public Font Font
    {
        get => HostPanel!.Font;
        set => HostPanel!.Font = value;
    }

    /// <summary>
    /// Occurs when user click on simple hint
    /// </summary>
    public event EventHandler Click
    {
        add => HostPanel!.Click += value;
        remove => HostPanel!.Click -= value;
    }

    /// <summary>
    /// Inner control
    /// </summary>
    public Control? InnerControl { get; set; }

    /// <summary>
    /// Docking (allows None and Fill only)
    /// </summary>
    public DockStyle Dock { get; set; }

    /// <summary>
    /// Width of hint (if Dock is None)
    /// </summary>
    public int Width
    {
        get => HostPanel!.Width;
        set => HostPanel!.Width = value;
    }

    /// <summary>
    /// Height of hint
    /// </summary>
    public int Height
    {
        get => HostPanel!.Height;
        set => HostPanel!.Height = value;
    }

    /// <summary>
    /// Host panel
    /// </summary>
    public UnfocusablePanel? HostPanel { get; private set; }

    internal int TopPadding { get; set; }

    /// <summary>
    /// Tag
    /// </summary>
    public object? Tag { get; set; }

    /// <summary>
    /// Cursor
    /// </summary>
    public Cursor Cursor
    {
        get => HostPanel!.Cursor;
        set => HostPanel!.Cursor = value;
    }

    #endregion

    /// <summary>
    /// Inlining. If True then hint will moves apart text.
    /// </summary>
    public bool Inline { get; set; }

    /// <summary>
    /// Scroll textbox to the hint
    /// </summary>
    public virtual void DoVisible()
    {
        Range._textBox.DoRangeVisible (Range, true);
        Range._textBox.DoVisibleRectangle (HostPanel!.Bounds);

        Range._textBox.Invalidate();
    }

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    private Hint
        (
            TextRange range,
            Control? innerControl,
            string? text = null,
            bool inline = true,
            bool dock = true
        )
    {
        Range = range;
        Inline = inline;
        InnerControl = innerControl;

        Init();

        Dock = dock ? DockStyle.Fill : DockStyle.None;
        Text = text;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Hint
        (
            TextRange range,
            string? text,
            bool inline,
            bool dock
        )
        : this (range, null, text, inline, dock)
    {
    }


    /// <summary>
    /// Конструктор.
    /// </summary>
    public Hint
        (
            TextRange range,
            Control innerControl,
            bool inline,
            bool dock
        )
        : this (range, innerControl, null, inline, dock)
    {
    }

    #endregion

    /// <summary>
    /// Инициализация.
    /// </summary>
    protected virtual void Init()
    {
        HostPanel = new UnfocusablePanel();
        HostPanel.Click += OnClick;

        Cursor = Cursors.Default;
        BorderColor = Color.Silver;
        BackColor2 = Color.White;
        BackColor = InnerControl == null ? Color.Silver : SystemColors.Control;
        ForeColor = Color.Black;
        TextAlignment = StringAlignment.Near;
        Font = Range._textBox.Parent == null ? Range._textBox.Font : Range._textBox.Parent.Font;

        if (InnerControl != null)
        {
            HostPanel.Controls.Add (InnerControl);
            var size = InnerControl.GetPreferredSize (InnerControl.Size);
            HostPanel.Width = size.Width + 2;
            HostPanel.Height = size.Height + 2;
            InnerControl.Dock = DockStyle.Fill;
            InnerControl.Visible = true;
            BackColor = SystemColors.Control;
        }
        else
        {
            HostPanel.Height = Range._textBox.CharHeight + 5;
        }
    }

    /// <summary>
    /// Реакция на клик мышью.
    /// </summary>
    protected virtual void OnClick (object? sender, EventArgs e)
    {
        Range._textBox.OnHintClick (this);
    }
}
