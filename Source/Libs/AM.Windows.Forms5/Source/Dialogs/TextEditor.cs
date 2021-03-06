// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TextEditor.cs -- диалог с простейшим текстовым редактором.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Диалог с простейшим текстовым редактором.
    /// </summary>
    public class TextEditor : Form
    {
        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public TextEditor()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Создает форму и заполняет редактор текстом.
        /// </summary>
        /// <param name="initialText">Начальный текст.</param>
        public TextEditor
            (
                string initialText
            )
            : this()
        {
            _textBox!.Text = initialText;
            _textBox!.SelectionLength = 0;
        }

        /// <summary>
        /// Редактирование текста.
        /// </summary>
        /// <param name="text">Текст для редактирования.</param>
        /// <returns><c>true</c>, если пользователь нажал OK.</returns>
        public static bool EditText
            (
                ref string text
            )
        {
            var result = false;

            using var textEditor = new TextEditor(text);
            if (textEditor.ShowDialog() == DialogResult.OK)
            {
                text = textEditor._textBox!.Text;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Редактирование текстового файла.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        /// <returns><c>true</c>, если все прошло нормально.</returns>
        public static bool EditFile
            (
                string fileName
            )
        {
            var result = false;

            try
            {
                var text = File.ReadAllText(fileName);
                if (EditText(ref text))
                {
                    File.WriteAllText(fileName, text);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return result;
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
                if (_components != null)
                {
                    _components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #region Private members

        private Panel? _panel1;
        private Label? _label1;
        private Panel? _panel2;
        private Button? _okButton;
        private Button? _cancelButton;
        private TextBox? _textBox;
        private ContextMenuStrip? _contextMenu;
        private ToolStripMenuItem? _menuItem1;
        private ToolStripMenuItem? _menuItem2;
        private ToolStripMenuItem? _menuItem3;
        private ToolStripMenuItem? _menuItem4;
        private ToolStripMenuItem? _menuItem5;
        private ToolStripMenuItem? _menuItem6;
        private ToolStripMenuItem? _menuItem7;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly Container? _components = null;

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._panel1 = new System.Windows.Forms.Panel();
            this._label1 = new System.Windows.Forms.Label();
            this._panel2 = new System.Windows.Forms.Panel();
            this._textBox = new System.Windows.Forms.TextBox();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._contextMenu = new System.Windows.Forms.ContextMenuStrip();
            this._menuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this._menuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this._menuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this._menuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this._menuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this._menuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this._menuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this._panel1.SuspendLayout();
            this._panel2.SuspendLayout();
            this.SuspendLayout();
            //
            // _panel1
            //
            this._panel1.BackColor = System.Drawing.Color.White;
            this._panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._panel1.Controls.Add(this._label1);
            this._panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this._panel1.Location = new System.Drawing.Point(0, 0);
            this._panel1.Name = "_panel1";
            this._panel1.Size = new System.Drawing.Size(480, 48);
            this._panel1.TabIndex = 0;
            //
            // _label1
            //
            this._label1.Dock = System.Windows.Forms.DockStyle.Left;
            this._label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point, ((System.Byte) (204)));
            this._label1.Location = new System.Drawing.Point(0, 0);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(392, 46);
            this._label1.TabIndex = 0;
            this._label1.Text = "Отредактируйте данный текст и нажмите OK для подтверждения, либо Отмена для отказ" +
                               "а.";
            this._label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // _panel2
            //
            this._panel2.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom)
                                                        | System.Windows.Forms.AnchorStyles.Left)
                                                       | System.Windows.Forms.AnchorStyles.Right)));
            this._panel2.Controls.Add(this._textBox);
            this._panel2.Location = new System.Drawing.Point(8, 56);
            this._panel2.Name = "_panel2";
            this._panel2.Size = new System.Drawing.Size(464, 256);
            this._panel2.TabIndex = 1;
            //
            // _textBox
            //
            this._textBox.AcceptsReturn = true;
            this._textBox.AcceptsTab = true;
            this._textBox.AllowDrop = true;
            this._textBox.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom)
                                                        | System.Windows.Forms.AnchorStyles.Left)
                                                       | System.Windows.Forms.AnchorStyles.Right)));
            this._textBox.Location = new System.Drawing.Point(8, 8);
            this._textBox.Multiline = true;
            this._textBox.Name = "_textBox";
            this._textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._textBox.Size = new System.Drawing.Size(448, 240);
            this._textBox.TabIndex = 0;
            this._textBox.Text = "";
            this._textBox.WordWrap = false;
            //
            // _okButton
            //
            this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom |
                                                                          System.Windows.Forms.AnchorStyles.Right)));
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Location = new System.Drawing.Point(312, 320);
            this._okButton.Name = "_okButton";
            this._okButton.TabIndex = 2;
            this._okButton.Text = "OK";
            //
            // _cancelButton
            //
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom |
                                                                              System.Windows.Forms.AnchorStyles
                                                                                  .Right)));
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(392, 320);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "Отмена";
            //
            // contextMenu
            //
            this._contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripMenuItem[]
            {
                this._menuItem1,
                this._menuItem2,
                this._menuItem3,
                this._menuItem4,
                this._menuItem5,
                this._menuItem6,
                this._menuItem7
            });
            //
            // menuItem1
            //
            this._menuItem1.Text = "Вырезать";
            //
            // menuItem2
            //
            this._menuItem2.Text = "Скопировать";
            //
            // menuItem3
            //
            this._menuItem3.Text = "Вставить";
            //
            // menuItem4
            //
            this._menuItem4.Text = "Удалить";
            //
            // menuItem5
            //
            this._menuItem5.Text = "-";
            //
            // menuItem6
            //
            this._menuItem6.Text = "Отменить";
            //
            // menuItem7
            //
            this._menuItem7.Text = "Повторить";
            //
            // TextEditor
            //
            this.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(480, 350);
            this.ControlBox = false;
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._panel2);
            this.Controls.Add(this._panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextEditor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Текстовый редактор";
            this._panel1.ResumeLayout(false);
            this._panel2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
