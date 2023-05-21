using System.ComponentModel;

namespace MachineInfo
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var listViewGroup1 = new System.Windows.Forms.ListViewGroup("System", System.Windows.Forms.HorizontalAlignment.Left);
            var listViewGroup2 = new System.Windows.Forms.ListViewGroup("Network", System.Windows.Forms.HorizontalAlignment.Left);
            var listViewGroup3 = new System.Windows.Forms.ListViewGroup("Memory", System.Windows.Forms.HorizontalAlignment.Left);
            var listViewGroup4 = new System.Windows.Forms.ListViewGroup("Drives", System.Windows.Forms.HorizontalAlignment.Left);
            var resources = new ComponentResourceManager(typeof(MainForm));
            _listView = new System.Windows.Forms.ListView();
            _nameColumn = new System.Windows.Forms.ColumnHeader();
            _valueColumn = new System.Windows.Forms.ColumnHeader();
            SuspendLayout();
            // 
            // _listView
            // 
            _listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { _nameColumn, _valueColumn });
            _listView.Dock = System.Windows.Forms.DockStyle.Fill;
            _listView.FullRowSelect = true;
            _listView.GridLines = true;
            listViewGroup1.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup1.Header = "System";
            listViewGroup1.Name = "System";
            listViewGroup2.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup2.Header = "Network";
            listViewGroup2.Name = "Network";
            listViewGroup3.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup3.Header = "Memory";
            listViewGroup3.Name = "Memory";
            listViewGroup4.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup4.Header = "Drives";
            listViewGroup4.Name = "Drives";
            _listView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] { listViewGroup1, listViewGroup2, listViewGroup3, listViewGroup4 });
            _listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            _listView.Location = new System.Drawing.Point(0, 0);
            _listView.MultiSelect = false;
            _listView.Name = "_listView";
            _listView.Size = new System.Drawing.Size(624, 441);
            _listView.TabIndex = 0;
            _listView.UseCompatibleStateImageBehavior = false;
            _listView.View = System.Windows.Forms.View.Details;
            _listView.ClientSizeChanged += _listView_ClientSizeChanged;
            _listView.DoubleClick += _listView_DoubleClick;
            // 
            // _nameColumn
            // 
            _nameColumn.Text = "Property";
            _nameColumn.Width = 240;
            // 
            // _valueColumn
            // 
            _valueColumn.Text = "Value";
            _valueColumn.Width = 360;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(624, 441);
            Controls.Add(_listView);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MinimumSize = new System.Drawing.Size(640, 480);
            Name = "MainForm";
            Text = "Machine info";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListView _listView;
        private System.Windows.Forms.ColumnHeader _nameColumn;
        private System.Windows.Forms.ColumnHeader _valueColumn;
    }
}
