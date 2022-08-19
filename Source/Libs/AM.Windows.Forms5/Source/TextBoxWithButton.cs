// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedMember.Global

/* TextBoxWithButton.cs -- однострочный текстовый редактор, снабженный кнопкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

// ReSharper disable MemberCanBePrivate.Global

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Однострочный текстовый редактор, снабженный кнопкой.
/// </summary>
[ToolboxBitmap (typeof (Clocks), "Images.TextBoxWithButton.bmp")]
[System.ComponentModel.DesignerCategory ("Code")]
public partial class TextBoxWithButton
    : UserControl
{
    #region Events

    /// <summary>
    /// Raised on button click.
    /// </summary>
    public event EventHandler? ButtonClick;

    /// <summary>
    /// Raised when text changed.
    /// </summary>
    public new event EventHandler? TextChanged;

    #endregion

    #region Properties

    /// <summary>
    /// Button.
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public Button? Button { get; private set; }

    /// <summary>
    /// Selection start.
    /// </summary>
    public int SelectionStart
    {
        get => TextBox?.SelectionStart ?? 0;
        set
        {
            if (TextBox is not null)
            {
                TextBox.SelectionStart = value;
            }
        }
    }

    /// <summary>
    /// Text box.
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public TextBox? TextBox { get; private set; }

    /// <summary>
    /// Text length.
    /// </summary>
    public int TextLength => TextBox?.TextLength ?? 0;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TextBoxWithButton()
    {
        InitializeComponent();

        Button!.Click += _Button_Click;
        Button.Width = 20;
        TextBox!.TextChanged += _TextBox_TextChanged;
    }

    #endregion

    #region Private members

    private void _Button_Click
        (
            object? sender,
            EventArgs e
        )
    {
        ButtonClick?.Invoke (this, e);
    }

    private void _TextBox_TextChanged
        (
            object? sender,
            EventArgs e
        )
    {
        TextChanged?.Invoke (this, e);
    }

    #endregion

    #region Control members

    /// <inheritdoc/>
    public override string Text
    {
        get => TextBox?.Text ?? string.Empty;
        set
        {
            if (TextBox != null)
            {
                TextBox.Text = value;
            }
        }
    }

    #endregion
}
