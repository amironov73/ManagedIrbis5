namespace SDHelper;

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
        components = new System.ComponentModel.Container();
        var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        _toolStripContainer = new ToolStripContainer();
        _splitContainer = new SplitContainer();
        _dataGrid = new DataGridView();
        _titleColumn = new DataGridViewTextBoxColumn();
        _valueColumn = new DataGridViewComboBoxColumn();
        _suggestionsSource = new BindingSource(components);
        _bindingSource = new BindingSource(components);
        _resultBox = new TextBox();
        flowLayoutPanel1 = new FlowLayoutPanel();
        _toolStrip = new ToolStrip();
        _copyButton = new ToolStripButton();
        toolStripSeparator1 = new ToolStripSeparator();
        _countLabel = new ToolStripLabel();
        _toolStripContainer.ContentPanel.SuspendLayout();
        _toolStripContainer.TopToolStripPanel.SuspendLayout();
        _toolStripContainer.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_splitContainer).BeginInit();
        _splitContainer.Panel1.SuspendLayout();
        _splitContainer.Panel2.SuspendLayout();
        _splitContainer.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_dataGrid).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_suggestionsSource).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_bindingSource).BeginInit();
        _toolStrip.SuspendLayout();
        SuspendLayout();
        //
        // toolStripContainer1
        //
        //
        // toolStripContainer1.ContentPanel
        //
        _toolStripContainer.ContentPanel.Controls.Add(_splitContainer);
        _toolStripContainer.ContentPanel.Controls.Add(flowLayoutPanel1);
        _toolStripContainer.ContentPanel.Size = new Size(784, 386);
        _toolStripContainer.Dock = DockStyle.Fill;
        _toolStripContainer.Location = new Point(0, 0);
        _toolStripContainer.Name = "_toolStripContainer";
        _toolStripContainer.Size = new Size(784, 411);
        _toolStripContainer.TabIndex = 0;
        _toolStripContainer.Text = "toolStripContainer1";
        //
        // toolStripContainer1.TopToolStripPanel
        //
        _toolStripContainer.TopToolStripPanel.Controls.Add(_toolStrip);
        //
        // _splitContainer
        //
        _splitContainer.Dock = DockStyle.Fill;
        _splitContainer.FixedPanel = FixedPanel.Panel2;
        _splitContainer.Location = new Point(0, 0);
        _splitContainer.Name = "_splitContainer";
        _splitContainer.Orientation = Orientation.Horizontal;
        //
        // _splitContainer.Panel1
        //
        _splitContainer.Panel1.Controls.Add(_dataGrid);
        //
        // _splitContainer.Panel2
        //
        _splitContainer.Panel2.Controls.Add(_resultBox);
        _splitContainer.Size = new Size(784, 386);
        _splitContainer.SplitterDistance = 320;
        _splitContainer.TabIndex = 0;
        //
        // _dataGrid
        //
        _dataGrid.AllowUserToAddRows = false;
        _dataGrid.AllowUserToDeleteRows = false;
        _dataGrid.AllowUserToResizeRows = false;
        _dataGrid.AutoGenerateColumns = false;
        _dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        _dataGrid.Columns.AddRange(new DataGridViewColumn[] { _titleColumn, _valueColumn });
        _dataGrid.DataSource = _bindingSource;
        _dataGrid.Dock = DockStyle.Fill;
        _dataGrid.EditMode = DataGridViewEditMode.EditOnEnter;
        _dataGrid.Location = new Point(0, 0);
        _dataGrid.Name = "_dataGrid";
        _dataGrid.RowHeadersVisible = false;
        _dataGrid.Size = new Size(784, 300);
        _dataGrid.TabIndex = 1;
        _dataGrid.CellContentClick += DataGrid_CellContentClick;
        _dataGrid.DataError += DataGrid_DataError;
        _dataGrid.PreviewKeyDown += MainForm_PreviewKeyDown;
        //
        // _titleColumn
        //
        _titleColumn.DataPropertyName = "Title";
        _titleColumn.HeaderText = "Property";
        _titleColumn.Name = "_titleColumn";
        _titleColumn.ReadOnly = true;
        _titleColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        _titleColumn.Width = 150;
        //
        // _valueColumn
        //
        _valueColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        _valueColumn.DataPropertyName = "Value";
        _valueColumn.DataSource = _suggestionsSource;
        _valueColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
        _valueColumn.DisplayStyleForCurrentCellOnly = true;
        _valueColumn.FlatStyle = FlatStyle.System;
        _valueColumn.HeaderText = "Value";
        _valueColumn.Name = "_valueColumn";
        _valueColumn.Resizable = DataGridViewTriState.True;
        //
        // _suggestionsSource
        //
        _suggestionsSource.DataMember = "Suggestions";
        _suggestionsSource.DataSource = _bindingSource;
        //
        // _bindingSource
        //
        _bindingSource.AllowNew = false;
        _bindingSource.DataSource = typeof(PromptItem);
        _bindingSource.CurrentChanged += BindingSource_CurrentChanged;
        //
        // _resultBox
        //
        _resultBox.Dock = DockStyle.Fill;
        _resultBox.Location = new Point(0, 0);
        _resultBox.Multiline = true;
        _resultBox.Name = "_resultBox";
        _resultBox.ScrollBars = ScrollBars.Vertical;
        _resultBox.Size = new Size(784, 82);
        _resultBox.TabIndex = 0;
        //
        // flowLayoutPanel1
        //
        flowLayoutPanel1.Dock = DockStyle.Fill;
        flowLayoutPanel1.Location = new Point(0, 0);
        flowLayoutPanel1.Name = "flowLayoutPanel1";
        flowLayoutPanel1.Size = new Size(784, 386);
        flowLayoutPanel1.TabIndex = 0;
        //
        // _toolStrip
        //
        _toolStrip.Dock = DockStyle.None;
        _toolStrip.Items.AddRange(new ToolStripItem[] { _copyButton, toolStripSeparator1, _countLabel });
        _toolStrip.Location = new Point(3, 0);
        _toolStrip.Name = "_toolStrip";
        _toolStrip.Size = new Size(85, 25);
        _toolStrip.TabIndex = 0;
        //
        // _copyButton
        //
        _copyButton.Image = Properties.Resources.Copy_6524_24;
        _copyButton.ImageTransparentColor = Color.Magenta;
        _copyButton.Name = "_copyButton";
        _copyButton.Size = new Size(55, 22);
        _copyButton.Text = "Copy";
        _copyButton.ToolTipText = "Copy";
        _copyButton.Click += CopyButton_Click;
        //
        // toolStripSeparator1
        //
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(6, 25);
        //
        // _countLabel
        //
        _countLabel.Name = "_countLabel";
        _countLabel.Size = new Size(12, 22);
        _countLabel.Text = "_";
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 411);
        Controls.Add(_toolStripContainer);
        Icon = (Icon)resources.GetObject("$this.Icon");
        KeyPreview = true;
        MinimumSize = new Size(800, 450);
        Name = "MainForm";
        Text = "Stable Diffusion Prompt Helper";
        Load += MainForm_Load;
        PreviewKeyDown += MainForm_PreviewKeyDown;
        _toolStripContainer.ContentPanel.ResumeLayout(false);
        _toolStripContainer.TopToolStripPanel.ResumeLayout(false);
        _toolStripContainer.TopToolStripPanel.PerformLayout();
        _toolStripContainer.ResumeLayout(false);
        _toolStripContainer.PerformLayout();
        _splitContainer.Panel1.ResumeLayout(false);
        _splitContainer.Panel2.ResumeLayout(false);
        _splitContainer.Panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)_splitContainer).EndInit();
        _splitContainer.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_dataGrid).EndInit();
        ((System.ComponentModel.ISupportInitialize)_suggestionsSource).EndInit();
        ((System.ComponentModel.ISupportInitialize)_bindingSource).EndInit();
        _toolStrip.ResumeLayout(false);
        _toolStrip.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private ToolStripContainer _toolStripContainer;
    private ToolStrip _toolStrip;
    private FlowLayoutPanel flowLayoutPanel1;
    private DataGridView _dataGrid;
    private BindingSource _bindingSource;
    private SplitContainer _splitContainer;
    private TextBox _resultBox;
    private BindingSource _suggestionsSource;
    private ToolStripButton _copyButton;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripLabel _countLabel;
    private DataGridViewTextBoxColumn _titleColumn;
    private DataGridViewComboBoxColumn _valueColumn;
}
