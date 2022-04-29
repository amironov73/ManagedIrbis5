// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* PictureViewForm.cs -- простая форма для просмотра картинок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Простая форма для просмотра картинок.
/// </summary>
public sealed partial class PictureViewForm
    : Form
{
    #region Properties

    /// <summary>
    /// Просматриваемая картинка.
    /// </summary>
    public Image Image
    {
        get => _pictureBox.Image;
        set => _SetImage (value);
    }

    /// <summary>
    /// Режим просмотра.
    /// </summary>
    public PictureViewMode Mode { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="PictureViewForm"/> class.
    /// </summary>
    /// <param name="image">The image.</param>
    public PictureViewForm
        (
            Image? image
        )
    {
        InitializeComponent();
        Mode = PictureViewMode.Auto;
        _pictureBox.Image = image;
    }

    #endregion

    #region Private members

    private void _SetImage
        (
            Image image
        )
    {
        Sure.NotNull (image);

        _pictureBox.Image = image;
        Invalidate();
    }

    private void _copyButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        Clipboard.SetImage (_pictureBox.Image);
    }

    private void _openButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        if (_openFileDialog.ShowDialog (this) == DialogResult.OK)
        {
            _pictureBox.Image = Drawing.Utility.LoadFromFile (_openFileDialog.FileName);
        }
    }

    private void _saveButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        if (_saveFileDialog.ShowDialog (this) == DialogResult.OK)
        {
            _pictureBox.Image.Save (_saveFileDialog.FileName);
        }
    }

    private void _pasteButton_Click (object sender, EventArgs e)
    {
        if (Clipboard.ContainsImage())
        {
            _pictureBox.Image = Clipboard.GetImage();
        }
    }

    private void _printSetupButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        _pageSetupDialog.ShowDialog (this);
    }

    private void _printPreviewButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        _picturePrinter.Image = _pictureBox.Image;
        _picturePrinter.Preview();
    }

    private void _printButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        _picturePrinter.Image = _pictureBox.Image;
        _picturePrinter.Print();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Просмотр картинки в неблокирующем режиме.
    /// </summary>
    public static PictureViewForm Show
        (
            IWin32Window? parent,
            Image? image
        )
    {
        var result = new PictureViewForm (image);
        result.Show (parent);

        return result;
    }

    /// <summary>
    /// Просмотр картинки в (блокирующем) режиме диалога.
    /// </summary>
    public static void ShowDialog
        (
            IWin32Window? parent,
            Image? image
        )
    {
        using var form = new PictureViewForm (image);
        form.ShowDialog (parent);
    }

    #endregion
}
