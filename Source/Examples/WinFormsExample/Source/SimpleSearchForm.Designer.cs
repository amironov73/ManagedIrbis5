
namespace WinFormsExample
{
    partial class SimpleSearchForm
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
            if (disposing)
            {
                components?.Dispose();
                Engine?.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleSearchForm));
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this._logBox = new AM.Windows.Forms.LogBox();
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this._fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._databaseToolStrip = new System.Windows.Forms.ToolStrip();
            this._initializationTimer = new System.Windows.Forms.Timer(this.components);
            this._dictionaryPanel = new ManagedIrbis.WinForms.DictionaryPanel();
            this._foundPanel = new ManagedIrbis.WinForms.FoundPanel();
            this._previewPanel = new ManagedIrbis.WinForms.PreviewPanel();
            this._marcEditor = new ManagedIrbis.WinForms.Editors.SimplestMarcEditor();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this._menuStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // _statusStrip
            //
            this._statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._statusStrip.Location = new System.Drawing.Point(0, 0);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(800, 22);
            this._statusStrip.TabIndex = 0;
            this._statusStrip.Text = "statusStrip1";
            //
            // toolStripContainer1
            //
            //
            // toolStripContainer1.BottomToolStripPanel
            //
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this._statusStrip);
            //
            // toolStripContainer1.ContentPanel
            //
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this._logBox);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(800, 379);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(800, 450);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            //
            // toolStripContainer1.TopToolStripPanel
            //
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._menuStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this._databaseToolStrip);
            //
            // splitContainer1
            //
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // splitContainer1.Panel1
            //
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            //
            // splitContainer1.Panel2
            //
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(800, 320);
            this.splitContainer1.SplitterDistance = 210;
            this.splitContainer1.TabIndex = 0;
            //
            // splitContainer2
            //
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            //
            // splitContainer2.Panel1
            //
            this.splitContainer2.Panel1.Controls.Add(this._dictionaryPanel);
            //
            // splitContainer2.Panel2
            //
            this.splitContainer2.Panel2.Controls.Add(this._marcEditor);
            this.splitContainer2.Size = new System.Drawing.Size(800, 210);
            this.splitContainer2.SplitterDistance = 265;
            this.splitContainer2.TabIndex = 0;
            //
            // splitContainer3
            //
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            //
            // splitContainer3.Panel1
            //
            this.splitContainer3.Panel1.Controls.Add(this._foundPanel);
            //
            // splitContainer3.Panel2
            //
            this.splitContainer3.Panel2.Controls.Add(this._previewPanel);
            this.splitContainer3.Size = new System.Drawing.Size(800, 106);
            this.splitContainer3.SplitterDistance = 265;
            this.splitContainer3.TabIndex = 0;
            //
            // _logBox
            //
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._logBox.Location = new System.Drawing.Point(0, 320);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(800, 59);
            this._logBox.TabIndex = 1;
            //
            // _menuStrip
            //
            this._menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileToolStripMenuItem});
            this._menuStrip.Location = new System.Drawing.Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Size = new System.Drawing.Size(800, 24);
            this._menuStrip.TabIndex = 0;
            this._menuStrip.Text = "menuStrip1";
            //
            // _fileToolStripMenuItem
            //
            this._fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._exitToolStripMenuItem});
            this._fileToolStripMenuItem.Name = "_fileToolStripMenuItem";
            this._fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this._fileToolStripMenuItem.Text = "&File";
            //
            // _exitToolStripMenuItem
            //
            this._exitToolStripMenuItem.Name = "_exitToolStripMenuItem";
            this._exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this._exitToolStripMenuItem.Text = "E&xit";
            this._exitToolStripMenuItem.Click += new System.EventHandler(this._exitToolStripMenuItem_Click);
            //
            // _databaseToolStrip
            //
            this._databaseToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._databaseToolStrip.Location = new System.Drawing.Point(3, 24);
            this._databaseToolStrip.Name = "_databaseToolStrip";
            this._databaseToolStrip.Size = new System.Drawing.Size(111, 25);
            this._databaseToolStrip.TabIndex = 1;
            //
            // _initializationTimer
            //
            this._initializationTimer.Enabled = true;
            this._initializationTimer.Tick += new System.EventHandler(this._initializationTimer_Tick);
            //
            // _dictionaryPanel
            //
            this._dictionaryPanel.Adapter = null;
            this._dictionaryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dictionaryPanel.Location = new System.Drawing.Point(0, 0);
            this._dictionaryPanel.Margin = new System.Windows.Forms.Padding(2);
            this._dictionaryPanel.Name = "_dictionaryPanel";
            this._dictionaryPanel.Size = new System.Drawing.Size(265, 210);
            this._dictionaryPanel.TabIndex = 0;
            //
            // _foundPanel
            //
            this._foundPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._foundPanel.Location = new System.Drawing.Point(0, 0);
            this._foundPanel.Margin = new System.Windows.Forms.Padding(2);
            this._foundPanel.Name = "_foundPanel";
            this._foundPanel.Size = new System.Drawing.Size(265, 106);
            this._foundPanel.TabIndex = 0;
            //
            // _previewPanel
            //
            this._previewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._previewPanel.Location = new System.Drawing.Point(0, 0);
            this._previewPanel.Name = "_previewPanel";
            this._previewPanel.Size = new System.Drawing.Size(531, 106);
            this._previewPanel.TabIndex = 0;
            //
            // _marcEditor
            //
            this._marcEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this._marcEditor.Location = new System.Drawing.Point(0, 0);
            this._marcEditor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._marcEditor.Name = "_marcEditor";
            this._marcEditor.ReadOnly = false;
            this._marcEditor.Size = new System.Drawing.Size(531, 210);
            this._marcEditor.TabIndex = 0;
            //
            // SimpleSearchForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this._menuStrip;
            this.Name = "SimpleSearchForm";
            this.Text = "SimpleSearchForm";
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this._menuStrip.ResumeLayout(false);
            this._menuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStripMenuItem _fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _exitToolStripMenuItem;
        private System.Windows.Forms.ToolStrip _databaseToolStrip;
        private System.Windows.Forms.Timer _initializationTimer;
        private AM.Windows.Forms.LogBox _logBox;
        private ManagedIrbis.WinForms.DictionaryPanel _dictionaryPanel;
        private ManagedIrbis.WinForms.FoundPanel _foundPanel;
        private ManagedIrbis.WinForms.PreviewPanel _previewPanel;
        private ManagedIrbis.WinForms.Editors.SimplestMarcEditor _marcEditor;
    }
}

