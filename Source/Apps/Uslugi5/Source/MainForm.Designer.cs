namespace Uslugi5
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this._rfidBox = new System.Windows.Forms.TextBox();
            this._readButton = new System.Windows.Forms.Button();
            this._infoBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this._payButton = new System.Windows.Forms.Button();
            this._grid = new System.Windows.Forms.DataGridView();
            this._titleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._priceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AmountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._unitColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this._moneyBox = new System.Windows.Forms.TextBox();
            this._addButton = new System.Windows.Forms.Button();
            this._balanceButton = new System.Windows.Forms.Button();
            this._reportButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "RFID читателя";
            // 
            // _rfidBox
            // 
            this._rfidBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._rfidBox.Location = new System.Drawing.Point(16, 44);
            this._rfidBox.Name = "_rfidBox";
            this._rfidBox.Size = new System.Drawing.Size(494, 26);
            this._rfidBox.TabIndex = 1;
            // 
            // _readButton
            // 
            this._readButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._readButton.Location = new System.Drawing.Point(516, 12);
            this._readButton.Name = "_readButton";
            this._readButton.Size = new System.Drawing.Size(256, 58);
            this._readButton.TabIndex = 2;
            this._readButton.Text = "Считать";
            this._readButton.UseVisualStyleBackColor = true;
            this._readButton.Click += new System.EventHandler(this._readButton_Click);
            // 
            // _infoBox
            // 
            this._infoBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._infoBox.Location = new System.Drawing.Point(16, 104);
            this._infoBox.Name = "_infoBox";
            this._infoBox.ReadOnly = true;
            this._infoBox.Size = new System.Drawing.Size(756, 26);
            this._infoBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Сведения о читателе";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(211, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Перечень доступных услуг";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 451);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Количество ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(153, 451);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Сумма";
            // 
            // _payButton
            // 
            this._payButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._payButton.Location = new System.Drawing.Point(17, 540);
            this._payButton.Name = "_payButton";
            this._payButton.Size = new System.Drawing.Size(746, 48);
            this._payButton.TabIndex = 11;
            this._payButton.Text = "Оплатить";
            this._payButton.UseVisualStyleBackColor = true;
            this._payButton.Click += new System.EventHandler(this._payButton_Click);
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this._grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this._grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._grid.AutoGenerateColumns = false;
            this._grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._titleColumn,
            this._priceColumn,
            this.AmountColumn,
            this._unitColumn});
            this._grid.DataSource = this._bindingSource;
            this._grid.Location = new System.Drawing.Point(17, 170);
            this._grid.MultiSelect = false;
            this._grid.Name = "_grid";
            this._grid.RowHeadersVisible = false;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.Size = new System.Drawing.Size(746, 301);
            this._grid.TabIndex = 12;
            // 
            // _titleColumn
            // 
            this._titleColumn.DataPropertyName = "Title";
            this._titleColumn.FillWeight = 305.1777F;
            this._titleColumn.HeaderText = "Наименование услуги";
            this._titleColumn.MinimumWidth = 100;
            this._titleColumn.Name = "_titleColumn";
            this._titleColumn.ReadOnly = true;
            // 
            // _priceColumn
            // 
            this._priceColumn.DataPropertyName = "Price";
            this._priceColumn.FillWeight = 101.7259F;
            this._priceColumn.HeaderText = "Цена за единицу";
            this._priceColumn.Name = "_priceColumn";
            this._priceColumn.ReadOnly = true;
            // 
            // AmountColumn
            // 
            this.AmountColumn.DataPropertyName = "Amount";
            this.AmountColumn.FillWeight = 60F;
            this.AmountColumn.HeaderText = "Заказ";
            this.AmountColumn.Name = "AmountColumn";
            this.AmountColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.AmountColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _unitColumn
            // 
            this._unitColumn.DataPropertyName = "Unit";
            this._unitColumn.FillWeight = 101.7259F;
            this._unitColumn.HeaderText = "Единица измерения";
            this._unitColumn.Name = "_unitColumn";
            this._unitColumn.ReadOnly = true;
            // 
            // _bindingSource
            // 
            this._bindingSource.AllowNew = false;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 485);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(192, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "Вносимая сумма (рубли)";
            // 
            // _moneyBox
            // 
            this._moneyBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._moneyBox.Location = new System.Drawing.Point(17, 508);
            this._moneyBox.Name = "_moneyBox";
            this._moneyBox.Size = new System.Drawing.Size(307, 26);
            this._moneyBox.TabIndex = 15;
            // 
            // _addButton
            // 
            this._addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._addButton.Location = new System.Drawing.Point(330, 485);
            this._addButton.Name = "_addButton";
            this._addButton.Size = new System.Drawing.Size(232, 49);
            this._addButton.TabIndex = 16;
            this._addButton.Text = "Пополнить баланс";
            this._addButton.UseVisualStyleBackColor = true;
            this._addButton.Click += new System.EventHandler(this._addButton_Click);
            // 
            // _balanceButton
            // 
            this._balanceButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._balanceButton.Location = new System.Drawing.Point(17, 594);
            this._balanceButton.Name = "_balanceButton";
            this._balanceButton.Size = new System.Drawing.Size(746, 41);
            this._balanceButton.TabIndex = 17;
            this._balanceButton.Text = "Показать выписку";
            this._balanceButton.UseVisualStyleBackColor = true;
            this._balanceButton.Click += new System.EventHandler(this._balanceButton_Click);
            // 
            // _reportButton
            // 
            this._reportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._reportButton.Location = new System.Drawing.Point(568, 485);
            this._reportButton.Name = "_reportButton";
            this._reportButton.Size = new System.Drawing.Size(195, 49);
            this._reportButton.TabIndex = 18;
            this._reportButton.Text = "Отчет";
            this._reportButton.UseVisualStyleBackColor = true;
            this._reportButton.Click += new System.EventHandler(this._reportButton_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this._readButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 647);
            this.Controls.Add(this._reportButton);
            this.Controls.Add(this._balanceButton);
            this.Controls.Add(this._addButton);
            this.Controls.Add(this._moneyBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this._grid);
            this.Controls.Add(this._payButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._infoBox);
            this.Controls.Add(this._readButton);
            this.Controls.Add(this._rfidBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.Text = "НТБ ИРНИТУ. Оказание платных услуг";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _rfidBox;
        private System.Windows.Forms.Button _readButton;
        private System.Windows.Forms.TextBox _infoBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button _payButton;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.BindingSource _bindingSource;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox _moneyBox;
        private System.Windows.Forms.Button _addButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn _titleColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _priceColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AmountColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _unitColumn;
        private System.Windows.Forms.Button _balanceButton;
        private System.Windows.Forms.Button _reportButton;
    }
}

