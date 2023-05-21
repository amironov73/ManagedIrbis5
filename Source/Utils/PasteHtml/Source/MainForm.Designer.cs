namespace PasteHtml
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            _menuStrip = new System.Windows.Forms.MenuStrip();
            _pasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _copyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _textBox = new System.Windows.Forms.TextBox();
            _saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            _menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _menuStrip
            // 
            _menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _pasteMenuItem, _saveMenuItem, _copyMenuItem });
            _menuStrip.Location = new System.Drawing.Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Size = new System.Drawing.Size(800, 24);
            _menuStrip.TabIndex = 0;
            _menuStrip.Text = "menuStrip1";
            // 
            // _pasteMenuItem
            // 
            _pasteMenuItem.Name = "_pasteMenuItem";
            _pasteMenuItem.Size = new System.Drawing.Size(47, 20);
            _pasteMenuItem.Text = "&Paste";
            _pasteMenuItem.Click += _pasteMenuItem_Click;
            // 
            // _saveMenuItem
            // 
            _saveMenuItem.Name = "_saveMenuItem";
            _saveMenuItem.Size = new System.Drawing.Size(52, 20);
            _saveMenuItem.Text = "&Save...";
            _saveMenuItem.Click += _saveMenuItem_Click;
            // 
            // _copyMenuItem
            // 
            _copyMenuItem.Name = "_copyMenuItem";
            _copyMenuItem.Size = new System.Drawing.Size(47, 20);
            _copyMenuItem.Text = "&Copy";
            _copyMenuItem.Click += _copyMenuItem_Click;
            // 
            // _textBox
            // 
            _textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            _textBox.Location = new System.Drawing.Point(0, 24);
            _textBox.Multiline = true;
            _textBox.Name = "_textBox";
            _textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            _textBox.Size = new System.Drawing.Size(800, 426);
            _textBox.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(_textBox);
            Controls.Add(_menuStrip);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = _menuStrip;
            Name = "MainForm";
            Text = "Paste text as HTML";
            _menuStrip.ResumeLayout(false);
            _menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStripMenuItem _pasteMenuItem;
        private System.Windows.Forms.TextBox _textBox;
        private System.Windows.Forms.ToolStripMenuItem _saveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _copyMenuItem;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
    }
}
