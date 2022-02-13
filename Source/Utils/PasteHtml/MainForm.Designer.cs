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
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this._pasteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._copyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._textBox = new System.Windows.Forms.TextBox();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _menuStrip
            // 
            this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._pasteMenuItem,
            this._saveMenuItem,
            this._copyMenuItem});
            this._menuStrip.Location = new System.Drawing.Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Size = new System.Drawing.Size(800, 24);
            this._menuStrip.TabIndex = 0;
            this._menuStrip.Text = "menuStrip1";
            // 
            // _pasteMenuItem
            // 
            this._pasteMenuItem.Name = "_pasteMenuItem";
            this._pasteMenuItem.Size = new System.Drawing.Size(47, 20);
            this._pasteMenuItem.Text = "&Paste";
            this._pasteMenuItem.Click += new System.EventHandler(this._pasteMenuItem_Click);
            // 
            // _saveMenuItem
            // 
            this._saveMenuItem.Name = "_saveMenuItem";
            this._saveMenuItem.Size = new System.Drawing.Size(52, 20);
            this._saveMenuItem.Text = "&Save...";
            this._saveMenuItem.Click += new System.EventHandler(this._saveMenuItem_Click);
            // 
            // _copyMenuItem
            // 
            this._copyMenuItem.Name = "_copyMenuItem";
            this._copyMenuItem.Size = new System.Drawing.Size(47, 20);
            this._copyMenuItem.Text = "&Copy";
            this._copyMenuItem.Click += new System.EventHandler(this._copyMenuItem_Click);
            // 
            // _textBox
            // 
            this._textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._textBox.Location = new System.Drawing.Point(0, 24);
            this._textBox.Multiline = true;
            this._textBox.Name = "_textBox";
            this._textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textBox.Size = new System.Drawing.Size(800, 426);
            this._textBox.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this._textBox);
            this.Controls.Add(this._menuStrip);
            this.MainMenuStrip = this._menuStrip;
            this.Name = "MainForm";
            this.Text = "Paste text as HTML";
            this._menuStrip.ResumeLayout(false);
            this._menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip _menuStrip;
        private ToolStripMenuItem _pasteMenuItem;
        private TextBox _textBox;
        private ToolStripMenuItem _saveMenuItem;
        private ToolStripMenuItem _copyMenuItem;
        private SaveFileDialog _saveFileDialog;
    }
}