﻿namespace ManagedIrbis.WinForms.Pft
{
    partial class PftVariableGrid
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this._grid = new System.Windows.Forms.DataGridView();
            this._nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._buttonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.SuspendLayout();
            //
            // _grid
            //
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this._grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._nameColumn,
            this._valueColumn,
            this._buttonColumn});
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.MultiSelect = false;
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.RowHeadersVisible = false;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.Size = new System.Drawing.Size(465, 193);
            this._grid.TabIndex = 0;
            //
            // _nameColumn
            //
            this._nameColumn.DataPropertyName = "Name";
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._nameColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this._nameColumn.HeaderText = "Name";
            this._nameColumn.MinimumWidth = 100;
            this._nameColumn.Name = "_nameColumn";
            this._nameColumn.ReadOnly = true;
            //
            // _valueColumn
            //
            this._valueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._valueColumn.DataPropertyName = "Value";
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._valueColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this._valueColumn.HeaderText = "Value";
            this._valueColumn.MinimumWidth = 100;
            this._valueColumn.Name = "_valueColumn";
            this._valueColumn.ReadOnly = true;
            //
            // _buttonColumn
            //
            this._buttonColumn.HeaderText = "Edit";
            this._buttonColumn.MinimumWidth = 40;
            this._buttonColumn.Name = "_buttonColumn";
            this._buttonColumn.ReadOnly = true;
            this._buttonColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this._buttonColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this._buttonColumn.Text = "...";
            this._buttonColumn.Width = 40;
            //
            // PftVariableGrid
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._grid);
            this.Name = "PftVariableGrid";
            this.Size = new System.Drawing.Size(465, 193);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn _nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _valueColumn;
        private System.Windows.Forms.DataGridViewButtonColumn _buttonColumn;
    }
}
