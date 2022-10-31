// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

#endregion

#nullable enable

namespace AM.Windows.Forms.Dialogs;

partial class InputDialogForm
    : ExtendedForm
{
    private SizeF _textMargin = new SizeF (12, 9);

    public event EventHandler<OkButtonClickedEventArgs>? OkButtonClicked;

    public InputDialogForm()
    {
        InitializeComponent();
    }

    public string MainInstruction { get; set; }

    [SuppressMessage ("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public string Content { get; set; }

    public string Input
    {
        get => _inputTextBox.Text;
        set => _inputTextBox.Text = value;
    }

    public int MaxLength
    {
        get => _inputTextBox.MaxLength;
        set => _inputTextBox.MaxLength = value;
    }

    public bool UsePasswordMasking
    {
        get => _inputTextBox.UseSystemPasswordChar;
        set => _inputTextBox.UseSystemPasswordChar = value;
    }

    public bool Multiline
    {
        get => _inputTextBox.Multiline;
        set
        {
            if (value == _inputTextBox.Multiline)
            {
                return;
            }

            _inputTextBox.Multiline = value;

            int offset;

            if (value)
            {
                AcceptButton = null;

                offset = _inputTextBox.Height * 2;
                _inputTextBox.Top -= offset;
                _inputTextBox.Height += offset;
                Height += offset;
                return;
            }

            AcceptButton = _okButton;

            offset = _inputTextBox.Height / 3 * 2;
            Height -= offset;
            _inputTextBox.Top += offset;
            _inputTextBox.Height -= offset;
        }
    }

    /// <summary>
    /// Displays the folder browser dialog.
    /// </summary>
    /// <param name="owner">The <see cref="IntPtr"/> Win32 handle that is the owner of this dialog.</param>
    /// <returns>If the user clicks the OK button, <see langword="true" /> is returned; otherwise, <see langword="false" />.</returns>
    public DialogResult ShowDialog (IntPtr owner)
    {
        var ownerHandle = owner == default ? NativeMethods.GetActiveWindow() : owner;

        var nativeWindow = new NativeWindow();
        nativeWindow.AssignHandle (ownerHandle);

        try
        {
            return ShowDialog (nativeWindow);
        }
        finally
        {
            nativeWindow.ReleaseHandle();
        }
    }

    protected virtual void OnOkButtonClicked (OkButtonClickedEventArgs e)
    {
        if (OkButtonClicked != null)
        {
            OkButtonClicked (this, e);
        }
    }

    protected override void ScaleControl (SizeF factor, BoundsSpecified specified)
    {
        _textMargin = new SizeF (_textMargin.Width * factor.Width, _textMargin.Height * factor.Height);
        base.ScaleControl (factor, specified);
    }

    private void SizeDialog()
    {
        int horizontalSpacing = (int)_textMargin.Width * 2;
        int verticalSpacing = ClientSize.Height - _inputTextBox.Top + (int)_textMargin.Height * 3;
        using (Graphics graphics = _primaryPanel.CreateGraphics())
        {
            ClientSize = DialogHelper.SizeDialog (graphics, MainInstruction, Content, Screen.FromControl (this),
                new Font (Font, FontStyle.Bold), Font, horizontalSpacing, verticalSpacing, ClientSize.Width, 0);
        }
    }

    private static void DrawThemeBackground (IDeviceContext dc, VisualStyleElement element, Rectangle bounds,
        Rectangle clipRectangle)
    {
        if (DialogHelper.IsTaskDialogThemeSupported)
        {
            VisualStyleRenderer renderer = new VisualStyleRenderer (element);
            renderer.DrawBackground (dc, bounds, clipRectangle);
        }
    }

    private void DrawText (IDeviceContext dc, ref Point location, bool measureOnly, int width)
    {
        DialogHelper.DrawText (dc, MainInstruction, Content, ref location, new Font (Font, FontStyle.Bold), Font,
            measureOnly, width);
    }

    private void _primaryPanel_Paint (object sender, PaintEventArgs e)
    {
        DrawThemeBackground (e.Graphics, AdditionalVisualStyleElements.TaskDialog.PrimaryPanel,
            _primaryPanel.ClientRectangle, e.ClipRectangle);
        Point location = new Point ((int)_textMargin.Width, (int)_textMargin.Height);
        DrawText (e.Graphics, ref location, false, ClientSize.Width - (int)_textMargin.Width * 2);
    }

    private void _secondaryPanel_Paint (object sender, PaintEventArgs e)
    {
        DrawThemeBackground (e.Graphics, AdditionalVisualStyleElements.TaskDialog.SecondaryPanel,
            _secondaryPanel.ClientRectangle, e.ClipRectangle);
    }

    private void NewInputBoxForm_Load (object sender, EventArgs e)
    {
        SizeDialog();
        CenterToScreen();
    }

    private void _okButton_Click (object sender, EventArgs e)
    {
        OkButtonClickedEventArgs okButtonClickedEventArgs = new OkButtonClickedEventArgs (_inputTextBox.Text, this);
        OnOkButtonClicked (okButtonClickedEventArgs);
        if (!okButtonClickedEventArgs.Cancel)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
