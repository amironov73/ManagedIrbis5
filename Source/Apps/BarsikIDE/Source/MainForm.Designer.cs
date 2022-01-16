using System.Windows.Forms;

namespace BarsikIDE
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._syntaxTextBox = new Fctb.SyntaxTextBox();
            this._logBox = new AM.Windows.Forms.LogBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._runMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._openButton = new System.Windows.Forms.ToolStripButton();
            this._runButton = new System.Windows.Forms.ToolStripButton();
            this._saveButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._resetCheckBox = new AM.Windows.Forms.ToolStripCheckBox();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._syntaxTextBox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(800, 379);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(800, 450);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._syntaxTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this._logBox);
            this.splitContainer1.Size = new System.Drawing.Size(800, 379);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 0;
            // 
            // _syntaxTextBox
            // 
            this._syntaxTextBox.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this._syntaxTextBox.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:]*" +
    "(?<range>:)\\s*(?<range>[^;]+);";
            this._syntaxTextBox.AutoScrollMinSize = new System.Drawing.Size(29, 18);
            this._syntaxTextBox.BackBrush = null;
            this._syntaxTextBox.CharHeight = 18;
            this._syntaxTextBox.CharWidth = 9;
            this._syntaxTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._syntaxTextBox.DefaultMarkerSize = 8;
            this._syntaxTextBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this._syntaxTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._syntaxTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._syntaxTextBox.IsReplaceMode = false;
            this._syntaxTextBox.Location = new System.Drawing.Point(0, 0);
            this._syntaxTextBox.Name = "_syntaxTextBox";
            this._syntaxTextBox.Paddings = new System.Windows.Forms.Padding(0);
            this._syntaxTextBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this._syntaxTextBox.ServiceColors = ((Fctb.ServiceColors)(resources.GetObject("_syntaxTextBox.ServiceColors")));
            this._syntaxTextBox.Size = new System.Drawing.Size(800, 266);
            this._syntaxTextBox.TabIndex = 0;
            this._syntaxTextBox.Zoom = 100;
            this._syntaxTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this._PreviewKeyDown);
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._logBox.Location = new System.Drawing.Point(0, 0);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(800, 109);
            this._logBox.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this._runMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._openMenuItem,
            this._saveMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // _openMenuItem
            // 
            this._openMenuItem.Name = "_openMenuItem";
            this._openMenuItem.Size = new System.Drawing.Size(103, 22);
            this._openMenuItem.Text = "&Open";
            this._openMenuItem.Click += new System.EventHandler(this._openButton_Click);
            // 
            // _saveMenuItem
            // 
            this._saveMenuItem.Name = "_saveMenuItem";
            this._saveMenuItem.Size = new System.Drawing.Size(103, 22);
            this._saveMenuItem.Text = "&Save";
            // 
            // _runMenuItem
            // 
            this._runMenuItem.Name = "_runMenuItem";
            this._runMenuItem.Size = new System.Drawing.Size(40, 20);
            this._runMenuItem.Text = "&Run";
            this._runMenuItem.Click += new System.EventHandler(this._runButton_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._openButton,
            this._runButton,
            this._saveButton,
            this.toolStripSeparator1,
            this._resetCheckBox});
            this.toolStrip1.Location = new System.Drawing.Point(3, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(268, 25);
            this.toolStrip1.TabIndex = 1;
            // 
            // _openButton
            // 
            this._openButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._openButton.Image = ((System.Drawing.Image)(resources.GetObject("_openButton.Image")));
            this._openButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._openButton.Name = "_openButton";
            this._openButton.Size = new System.Drawing.Size(40, 22);
            this._openButton.Text = "Open";
            this._openButton.Click += new System.EventHandler(this._openButton_Click);
            // 
            // _runButton
            // 
            this._runButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._runButton.Image = ((System.Drawing.Image)(resources.GetObject("_runButton.Image")));
            this._runButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._runButton.Name = "_runButton";
            this._runButton.Size = new System.Drawing.Size(32, 22);
            this._runButton.Text = "Run";
            this._runButton.Click += new System.EventHandler(this._runButton_Click);
            // 
            // _saveButton
            // 
            this._saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._saveButton.Image = ((System.Drawing.Image)(resources.GetObject("_saveButton.Image")));
            this._saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._saveButton.Name = "_saveButton";
            this._saveButton.Size = new System.Drawing.Size(35, 22);
            this._saveButton.Text = "Save";
            this._saveButton.Click += new System.EventHandler(this._saveButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // _resetCheckBox
            // 
            this._resetCheckBox.BackColor = System.Drawing.Color.Transparent;
            // 
            // _resetCheckBox
            // 
            this._resetCheckBox.CheckBox.AccessibleName = "_resetCheckBox";
            this._resetCheckBox.CheckBox.BackColor = System.Drawing.Color.Transparent;
            this._resetCheckBox.CheckBox.Checked = true;
            this._resetCheckBox.CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._resetCheckBox.CheckBox.Location = new System.Drawing.Point(122, 1);
            this._resetCheckBox.CheckBox.Name = "_resetCheckBox";
            this._resetCheckBox.CheckBox.Size = new System.Drawing.Size(112, 22);
            this._resetCheckBox.CheckBox.TabIndex = 0;
            this._resetCheckBox.CheckBox.Text = "Reset interpreter";
            this._resetCheckBox.CheckBox.UseVisualStyleBackColor = false;
            this._resetCheckBox.Name = "_resetCheckBox";
            this._resetCheckBox.Size = new System.Drawing.Size(112, 22);
            this._resetCheckBox.Text = "Reset interpreter";
            // 
            // _openFileDialog
            // 
            this._openFileDialog.Filter = "Barsik scripts|*.barsik|All files|*.*";
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.Filter = "Barsik files|*.barsik|All files|*.*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStripContainer1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Barsik IDE";
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this._PreviewKeyDown);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._syntaxTextBox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ToolStripContainer toolStripContainer1;
        private StatusStrip statusStrip1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem _openMenuItem;
        private ToolStripMenuItem _runMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripButton _openButton;
        private ToolStripButton _runButton;
        private SplitContainer splitContainer1;
        private AM.Windows.Forms.LogBox _logBox;
        private Fctb.SyntaxTextBox _syntaxTextBox;
        private OpenFileDialog _openFileDialog;
        private ToolStripMenuItem _saveMenuItem;
        private ToolStripButton _saveButton;
        private SaveFileDialog _saveFileDialog;
        private ToolStripSeparator toolStripSeparator1;
        private AM.Windows.Forms.ToolStripCheckBox _resetCheckBox;
    }
}
