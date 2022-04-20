namespace AM.Windows.Forms
{
    partial class LogBoxEx
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._copyButton = new System.Windows.Forms.ToolStripButton();
            this._saveButton = new System.Windows.Forms.ToolStripButton();
            this._clearButton = new System.Windows.Forms.ToolStripButton();
            this._listBox = new System.Windows.Forms.ListBox();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.Dock = System.Windows.Forms.DockStyle.Left;
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._copyButton,
            this._saveButton,
            this._clearButton});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(24, 150);
            this._toolStrip.Stretch = true;
            this._toolStrip.TabIndex = 0;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _copyButton
            // 
            this._copyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._copyButton.Image = global::AM.Windows.Forms.Properties.Resources.COPY;
            this._copyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._copyButton.Name = "_copyButton";
            this._copyButton.Size = new System.Drawing.Size(29, 20);
            this._copyButton.Text = "Copy";
            // 
            // _saveButton
            // 
            this._saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._saveButton.Image = global::AM.Windows.Forms.Properties.Resources.SAVE;
            this._saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._saveButton.Name = "_saveButton";
            this._saveButton.Size = new System.Drawing.Size(29, 20);
            this._saveButton.Text = "Save";
            // 
            // _clearButton
            // 
            this._clearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._clearButton.Image = global::AM.Windows.Forms.Properties.Resources.DELETE;
            this._clearButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._clearButton.Name = "_clearButton";
            this._clearButton.Size = new System.Drawing.Size(29, 20);
            this._clearButton.Text = "Clear";
            // 
            // _listBox
            // 
            this._listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this._listBox.FormattingEnabled = true;
            this._listBox.Location = new System.Drawing.Point(24, 0);
            this._listBox.Name = "_listBox";
            this._listBox.ScrollAlwaysVisible = true;
            this._listBox.Size = new System.Drawing.Size(495, 150);
            this._listBox.TabIndex = 1;
            // 
            // LogBoxEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._listBox);
            this.Controls.Add(this._toolStrip);
            this.Name = "LogBoxEx";
            this.Size = new System.Drawing.Size(519, 150);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _copyButton;
        private System.Windows.Forms.ToolStripButton _saveButton;
        private System.Windows.Forms.ToolStripButton _clearButton;
        private System.Windows.Forms.ListBox _listBox;
    }
}
