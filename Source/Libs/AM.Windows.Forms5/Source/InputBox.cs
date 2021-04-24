// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* InputBox.cs -- простой диалог для ввода строкового значения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Простой диалог для ввода строкового значения.
    /// </summary>
    public sealed class InputBox
        : Form
    {
        #region Properties

        /// <summary>
        /// Character for password entry.
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

        private Panel? panel1;
        private Label? topLabel;
        private TextBox? inputTextBox;
        private Button? okButton;
        private Button? cancelButton;
        private ImageList? imageList1;
        private Label? promptLabel;
        private PictureBox? pictureBox1;
        private System.ComponentModel.IContainer? components;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(InputBox));
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            topLabel = new Label();
            promptLabel = new Label();
            inputTextBox = new TextBox();
            imageList1 = new ImageList(components);
            okButton = new Button();
            cancelButton = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            SuspendLayout();
            //
            // panel1
            //
            resources.ApplyResources(panel1, "panel1");
            panel1.BackColor = System.Drawing.Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(topLabel);
            panel1.Name = "panel1";
            //
            // pictureBox1
            //
            resources.ApplyResources(pictureBox1, "pictureBox1");
            pictureBox1.Name = "pictureBox1";
            pictureBox1.TabStop = false;
            //
            // topLabel
            //
            resources.ApplyResources(topLabel, "topLabel");
            topLabel.Name = "topLabel";
            //
            // promptLabel
            //
            resources.ApplyResources(promptLabel, "promptLabel");
            promptLabel.Name = "promptLabel";
            //
            // inputTextBox
            //
            resources.ApplyResources(inputTextBox, "inputTextBox");
            inputTextBox.Name = "inputTextBox";
            //
            // imageList1
            //
            imageList1.ImageStream = ((ImageListStreamer?)(resources.GetObject("imageList1.ImageStream")));
            imageList1.TransparentColor = System.Drawing.Color.White;
            imageList1.Images.SetKeyName(0, "");
            imageList1.Images.SetKeyName(1, "");
            //
            // okButton
            //
            resources.ApplyResources(okButton, "okButton");
            okButton.DialogResult = DialogResult.OK;
            okButton.ImageList = imageList1;
            okButton.Name = "okButton";
            //
            // cancelButton
            //
            resources.ApplyResources(cancelButton, "cancelButton");
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.ImageList = imageList1;
            cancelButton.Name = "cancelButton";
            //
            // InputBox
            //
            AcceptButton = okButton;
            resources.ApplyResources(this, "$this");
            CancelButton = cancelButton;
            ControlBox = false;
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Controls.Add(inputTextBox);
            Controls.Add(promptLabel);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "InputBox";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
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
                ref string theValue
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
                ref string theValue
            )
        {
            using var box = new InputBox();
            if (!ReferenceEquals(topText, null))
            {
                box.topLabel!.Text = topText;
            }

            box.Text = caption;
            box.promptLabel!.Text = prompt;
            box.inputTextBox!.Text = theValue;
            box.inputTextBox!.PasswordChar = PasswordChar;

            var result = box.ShowDialog();
            theValue = box.inputTextBox.Text;

            return result;
        }

        #endregion
    }
}
