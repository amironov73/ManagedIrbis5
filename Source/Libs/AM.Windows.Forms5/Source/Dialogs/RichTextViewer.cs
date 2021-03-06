// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RichTextViewer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.ComponentModel;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Просмотрщик RTF-текста.
    /// </summary>
    public class RichTextViewer : Form
    {
        private Panel? _topPanel;
        private Label? _label1;
        private Panel? _panel1;
        private RichTextBox? _richTextBox;
        private Button? _okButton;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly Container? _components = null;

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public RichTextViewer()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Создает диалог и заполняет окно данным текстом.
        /// </summary>
        /// <param name="text">Текст.</param>
        public RichTextViewer
            (
                string text
            )
            : this()
        {
            using var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            _richTextBox!.LoadFile(stream, RichTextBoxStreamType.RichText);
        }

        /// <summary>
        /// Просмотр текста.
        /// </summary>
        /// <param name="text">Собственно текст.</param>
        public static void ViewText(ref string text)
        {
            using RichTextViewer viewer = new RichTextViewer(text);
            viewer.ShowDialog();
        }

        /// <summary>
        /// Просмотр файла.
        /// </summary>
        /// <param name="fileName">Имя файла.</param>
        public static void ViewFile(string fileName)
        {
            using RichTextViewer viewer = new RichTextViewer();
            viewer._richTextBox!.LoadFile(fileName);
            viewer.ShowDialog();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._topPanel = new System.Windows.Forms.Panel();
            this._label1 = new System.Windows.Forms.Label();
            this._panel1 = new System.Windows.Forms.Panel();
            this._richTextBox = new System.Windows.Forms.RichTextBox();
            this._okButton = new System.Windows.Forms.Button();
            this._topPanel.SuspendLayout();
            this._panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // topPanel
            //
            this._topPanel.BackColor = System.Drawing.Color.White;
            this._topPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._topPanel.Controls.Add(this._label1);
            this._topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._topPanel.Location = new System.Drawing.Point(0, 0);
            this._topPanel.Name = "_topPanel";
            this._topPanel.Size = new System.Drawing.Size(488, 48);
            this._topPanel.TabIndex = 0;
            //
            // label1
            //
            this._label1.Dock = System.Windows.Forms.DockStyle.Left;
            this._label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point, ((System.Byte) (204)));
            this._label1.Location = new System.Drawing.Point(0, 0);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(376, 46);
            this._label1.TabIndex = 0;
            this._label1.Text = "Внимательно прочитайте данный текст. По завершении нажмите OK или Enter.";
            this._label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // panel1
            //
            this._panel1.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom)
                                                        | System.Windows.Forms.AnchorStyles.Left)
                                                       | System.Windows.Forms.AnchorStyles.Right)));
            this._panel1.Controls.Add(this._richTextBox);
            this._panel1.Location = new System.Drawing.Point(8, 56);
            this._panel1.Name = "_panel1";
            this._panel1.Size = new System.Drawing.Size(472, 248);
            this._panel1.TabIndex = 1;
            //
            // richTextBox
            //
            this._richTextBox.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top |
                                                         System.Windows.Forms.AnchorStyles.Bottom)
                                                        | System.Windows.Forms.AnchorStyles.Left)
                                                       | System.Windows.Forms.AnchorStyles.Right)));
            this._richTextBox.Location = new System.Drawing.Point(8, 8);
            this._richTextBox.Name = "_richTextBox";
            this._richTextBox.ReadOnly = true;
            this._richTextBox.Size = new System.Drawing.Size(456, 232);
            this._richTextBox.TabIndex = 0;
            this._richTextBox.Text = "";
            //
            // okButton
            //
            this._okButton.Anchor =
                ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom |
                                                        System.Windows.Forms.AnchorStyles.Left)
                                                       | System.Windows.Forms.AnchorStyles.Right)));
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._okButton.Location = new System.Drawing.Point(8, 312);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(472, 23);
            this._okButton.TabIndex = 2;
            this._okButton.Text = "OK";
            //
            // RichTextViewer
            //
            this.AcceptButton = this._okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this._okButton;
            this.ClientSize = new System.Drawing.Size(488, 342);
            this.ControlBox = false;
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._panel1);
            this.Controls.Add(this._topPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RichTextViewer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Просмотр текста";
            this._topPanel.ResumeLayout(false);
            this._panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
