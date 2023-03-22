// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* InputBox.cs -- простой диалог для ввода строкового значения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Простой диалог для ввода строкового значения.
/// </summary>
[PublicAPI]
public sealed class InputBox
    : Form
{
    #region Properties

    /// <summary>
    /// Символ, который нужно использовать при вводе пароля.
    /// </summary>
    public static char PasswordChar { get; set; } = '*';

    #endregion

    #region Construction

    private InputBox()
    {
        InitializeComponent();
    }

    #endregion

    #region Private members

    private Panel? _mainPanel;
    private Label? _topLabel;
    private TextBox? _inputTextBox;
    private Button? _okButton;
    private Button? _cancelButton;
    private ImageList? _imageList;
    private Label? _promptLabel;
    private PictureBox? _pictureBox1;
    private System.ComponentModel.IContainer? _components;

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        _components = new System.ComponentModel.Container();
        var resources = new System.ComponentModel.ComponentResourceManager (typeof (InputBox));
        _mainPanel = new Panel();
        _pictureBox1 = new PictureBox();
        _topLabel = new Label();
        _promptLabel = new Label();
        _inputTextBox = new TextBox();
        _imageList = new ImageList (_components);
        _okButton = new Button();
        _cancelButton = new Button();
        _mainPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(_pictureBox1)).BeginInit();
        SuspendLayout();

        //
        // panel1
        //
        resources.ApplyResources (_mainPanel, "_mainPanel");
        _mainPanel.BackColor = System.Drawing.Color.White;
        _mainPanel.BorderStyle = BorderStyle.FixedSingle;
        _mainPanel.Controls.Add (_pictureBox1);
        _mainPanel.Controls.Add (_topLabel);
        _mainPanel.Name = "_mainPanel";

        //
        // pictureBox1
        //
        resources.ApplyResources (_pictureBox1, "_pictureBox1");
        _pictureBox1.Name = "_pictureBox1";
        _pictureBox1.TabStop = false;

        //
        // topLabel
        //
        resources.ApplyResources (_topLabel, "_topLabel");
        _topLabel.Name = "_topLabel";

        //
        // promptLabel
        //
        resources.ApplyResources (_promptLabel, "_promptLabel");
        _promptLabel.Name = "_promptLabel";

        //
        // inputTextBox
        //
        resources.ApplyResources (_inputTextBox, "_inputTextBox");
        _inputTextBox.Name = "_inputTextBox";

        //
        // imageList1
        //
        _imageList.ImageStream = ((ImageListStreamer?)(resources.GetObject ("_imageList.ImageStream")));
        _imageList.TransparentColor = System.Drawing.Color.White;
        _imageList.Images.SetKeyName (0, "");
        _imageList.Images.SetKeyName (1, "");

        //
        // okButton
        //
        resources.ApplyResources (_okButton, "_okButton");
        _okButton.DialogResult = DialogResult.OK;
        _okButton.ImageList = _imageList;
        _okButton.Name = "_okButton";

        //
        // cancelButton
        //
        resources.ApplyResources (_cancelButton, "_cancelButton");
        _cancelButton.DialogResult = DialogResult.Cancel;
        _cancelButton.ImageList = _imageList;
        _cancelButton.Name = "_cancelButton";

        //
        // InputBox
        //
        AcceptButton = _okButton;
        resources.ApplyResources (this, "$this");
        CancelButton = _cancelButton;
        ControlBox = false;
        Controls.Add (_cancelButton);
        Controls.Add (_okButton);
        Controls.Add (_inputTextBox);
        Controls.Add (_promptLabel);
        Controls.Add (_mainPanel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "InputBox";
        ShowInTaskbar = false;
        SizeGripStyle = SizeGripStyle.Hide;
        _mainPanel.ResumeLayout (false);
        ((System.ComponentModel.ISupportInitialize)(_pictureBox1)).EndInit();
        ResumeLayout (false);
        PerformLayout();
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose
        (
            bool disposing
        )
    {
        if (disposing)
        {
            _components?.Dispose();
        }

        base.Dispose (disposing);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Запрашивает у пользователя строковое значение
    /// (предлагая значение по умолчанию).
    /// </summary>
    /// <param name="caption">Заголовок окна.</param>
    /// <param name="prompt">Поясняющий текст.</param>
    /// <param name="theValue">Куда помещать результат.</param>
    /// <returns>Результат отработки диалогового окна.</returns>
    public static DialogResult Query
        (
            string caption,
            string prompt,
            ref string? theValue
        )
    {
        return Query
            (
                caption,
                prompt,
                null,
                ref theValue
            );
    }

    /// <summary>
    /// Запрашивает у пользователя строковое значение
    /// (предлагая значение по умолчанию).
    /// </summary>
    /// <param name="caption">Заголовок окна.</param>
    /// <param name="prompt">Поясняющий текст.</param>
    /// <param name="theValue">Куда помещать результат.</param>
    /// <param name="topText">Текст в верхней части окна.</param>
    /// <returns>Результат отработки диалогового окна.</returns>
    public static DialogResult Query
        (
            string caption,
            string prompt,
            string? topText,
            ref string? theValue
        )
    {
        using var box = new InputBox();
        if (topText is not null)
        {
            box._topLabel!.Text = topText;
        }

        box.Text = caption;
        box._promptLabel!.Text = prompt;
        box._inputTextBox!.Text = theValue;

        // box.inputTextBox!.PasswordChar = PasswordChar;

        var result = box.ShowDialog();
        theValue = box._inputTextBox.Text;

        return result;
    }

    #endregion
}
