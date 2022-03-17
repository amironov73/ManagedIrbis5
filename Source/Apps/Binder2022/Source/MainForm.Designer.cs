namespace Binder2022
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._magazineBox = new System.Windows.Forms.ListBox();
            this._yearBox = new System.Windows.Forms.ListBox();
            this._numberBox = new System.Windows.Forms.CheckedListBox();
            this._inventoryBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._rfidBox = new System.Windows.Forms.TextBox();
            this._bindButton = new System.Windows.Forms.Button();
            this._exemplarBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this._placeBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this._complectBox = new System.Windows.Forms.ComboBox();
            this._resultBrowser = new System.Windows.Forms.WebBrowser();
            this.label5 = new System.Windows.Forms.Label();
            this._descriptionBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this._destinationBox = new System.Windows.Forms.ComboBox();
            this._keyBox = new System.Windows.Forms.TextBox();
            this._timer1 = new System.Windows.Forms.Timer(this.components);
            this._fixButton = new System.Windows.Forms.Button();
            this._editButton = new System.Windows.Forms.Button();
            this._addBox = new System.Windows.Forms.TextBox();
            this._addButton = new System.Windows.Forms.Button();
            this._editMainButton = new System.Windows.Forms.Button();
            this._unbindButton = new System.Windows.Forms.Button();
            this._logBox = new System.Windows.Forms.TextBox();
            this._allBox = new System.Windows.Forms.CheckBox();
            this._updateButton = new System.Windows.Forms.Button();
            this._bindingBox = new System.Windows.Forms.ListBox();
            this._removeDublesButton = new System.Windows.Forms.Button();
            this._printButton = new System.Windows.Forms.Button();
            this._timer2 = new System.Windows.Forms.Timer(this.components);
            this._idleTimer = new System.Windows.Forms.Timer(this.components);
            this._deleteComplectButton = new System.Windows.Forms.Button();
            this._printButton2 = new System.Windows.Forms.Button();
            this._dontCheck = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // _magazineBox
            // 
            this._magazineBox.FormattingEnabled = true;
            this._magazineBox.Location = new System.Drawing.Point(11, 36);
            this._magazineBox.Name = "_magazineBox";
            this._magazineBox.Size = new System.Drawing.Size(412, 251);
            this._magazineBox.TabIndex = 0;
            this._magazineBox.SelectedIndexChanged += new System.EventHandler(this._magazineBox_SelectedIndexChanged);
            // 
            // _yearBox
            // 
            this._yearBox.FormattingEnabled = true;
            this._yearBox.Location = new System.Drawing.Point(429, 10);
            this._yearBox.Name = "_yearBox";
            this._yearBox.Size = new System.Drawing.Size(164, 394);
            this._yearBox.TabIndex = 1;
            this._yearBox.SelectedIndexChanged += new System.EventHandler(this._yearBox_SelectedIndexChanged);
            // 
            // _numberBox
            // 
            this._numberBox.CheckOnClick = true;
            this._numberBox.FormattingEnabled = true;
            this._numberBox.IntegralHeight = false;
            this._numberBox.Location = new System.Drawing.Point(599, 12);
            this._numberBox.Name = "_numberBox";
            this._numberBox.Size = new System.Drawing.Size(177, 390);
            this._numberBox.TabIndex = 2;
            this._numberBox.SelectedIndexChanged += new System.EventHandler(this._numberBox_SelectedIndexChanged);
            this._numberBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this._numberBox_ItemCheck);
            // 
            // _inventoryBox
            // 
            this._inventoryBox.Location = new System.Drawing.Point(407, 602);
            this._inventoryBox.Name = "_inventoryBox";
            this._inventoryBox.Size = new System.Drawing.Size(242, 20);
            this._inventoryBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(237, 609);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Инвентарный номер подшивки";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(237, 634);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "RFID-метка подшивки";
            // 
            // _rfidBox
            // 
            this._rfidBox.Location = new System.Drawing.Point(407, 627);
            this._rfidBox.Name = "_rfidBox";
            this._rfidBox.Size = new System.Drawing.Size(242, 20);
            this._rfidBox.TabIndex = 6;
            // 
            // _bindButton
            // 
            this._bindButton.Location = new System.Drawing.Point(655, 602);
            this._bindButton.Name = "_bindButton";
            this._bindButton.Size = new System.Drawing.Size(347, 45);
            this._bindButton.TabIndex = 7;
            this._bindButton.Text = "Сшить номера";
            this._bindButton.UseVisualStyleBackColor = true;
            this._bindButton.Click += new System.EventHandler(this._bindButton_Click);
            // 
            // _exemplarBox
            // 
            this._exemplarBox.FormattingEnabled = true;
            this._exemplarBox.Location = new System.Drawing.Point(782, 10);
            this._exemplarBox.Name = "_exemplarBox";
            this._exemplarBox.Size = new System.Drawing.Size(220, 82);
            this._exemplarBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 609);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Фонд";
            // 
            // _placeBox
            // 
            this._placeBox.FormattingEnabled = true;
            this._placeBox.Location = new System.Drawing.Point(83, 601);
            this._placeBox.Name = "_placeBox";
            this._placeBox.Size = new System.Drawing.Size(119, 21);
            this._placeBox.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 634);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Комплект";
            // 
            // _complectBox
            // 
            this._complectBox.FormattingEnabled = true;
            this._complectBox.Location = new System.Drawing.Point(83, 631);
            this._complectBox.Name = "_complectBox";
            this._complectBox.Size = new System.Drawing.Size(119, 21);
            this._complectBox.TabIndex = 13;
            // 
            // _resultBrowser
            // 
            this._resultBrowser.Location = new System.Drawing.Point(11, 293);
            this._resultBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this._resultBrowser.Name = "_resultBrowser";
            this._resultBrowser.Size = new System.Drawing.Size(412, 293);
            this._resultBrowser.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(429, 466);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(247, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Описание подшивки, например: янв.-дек. (1-12)";
            // 
            // _descriptionBox
            // 
            this._descriptionBox.Location = new System.Drawing.Point(684, 463);
            this._descriptionBox.Name = "_descriptionBox";
            this._descriptionBox.Size = new System.Drawing.Size(296, 20);
            this._descriptionBox.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(429, 492);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(251, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Место назначения подшивки (фонд назначения)";
            // 
            // _destinationBox
            // 
            this._destinationBox.FormattingEnabled = true;
            this._destinationBox.Location = new System.Drawing.Point(684, 489);
            this._destinationBox.Name = "_destinationBox";
            this._destinationBox.Size = new System.Drawing.Size(318, 21);
            this._destinationBox.TabIndex = 19;
            // 
            // _keyBox
            // 
            this._keyBox.Location = new System.Drawing.Point(14, 12);
            this._keyBox.Name = "_keyBox";
            this._keyBox.Size = new System.Drawing.Size(409, 20);
            this._keyBox.TabIndex = 20;
            this._keyBox.TextChanged += new System.EventHandler(this._keyBox_TextChanged);
            // 
            // _timer1
            // 
            this._timer1.Interval = 2000;
            this._timer1.Tick += new System.EventHandler(this._timer1_Tick);
            // 
            // _fixButton
            // 
            this._fixButton.Location = new System.Drawing.Point(432, 573);
            this._fixButton.Name = "_fixButton";
            this._fixButton.Size = new System.Drawing.Size(244, 23);
            this._fixButton.TabIndex = 21;
            this._fixButton.Text = "Исправить статусы и фонды";
            this._fixButton.UseVisualStyleBackColor = true;
            this._fixButton.Click += new System.EventHandler(this._fixButton_Click);
            // 
            // _editButton
            // 
            this._editButton.Location = new System.Drawing.Point(684, 573);
            this._editButton.Name = "_editButton";
            this._editButton.Size = new System.Drawing.Size(318, 23);
            this._editButton.TabIndex = 22;
            this._editButton.Text = "Редактировать номер";
            this._editButton.UseVisualStyleBackColor = true;
            this._editButton.Click += new System.EventHandler(this._editButton_Click);
            // 
            // _addBox
            // 
            this._addBox.Location = new System.Drawing.Point(433, 519);
            this._addBox.Name = "_addBox";
            this._addBox.Size = new System.Drawing.Size(568, 20);
            this._addBox.TabIndex = 23;
            // 
            // _addButton
            // 
            this._addButton.Location = new System.Drawing.Point(433, 545);
            this._addButton.Name = "_addButton";
            this._addButton.Size = new System.Drawing.Size(243, 23);
            this._addButton.TabIndex = 24;
            this._addButton.Text = "Добавить перечисленные номера";
            this._addButton.UseVisualStyleBackColor = true;
            this._addButton.Click += new System.EventHandler(this._addButton_Click);
            // 
            // _editMainButton
            // 
            this._editMainButton.Location = new System.Drawing.Point(433, 436);
            this._editMainButton.Name = "_editMainButton";
            this._editMainButton.Size = new System.Drawing.Size(160, 23);
            this._editMainButton.TabIndex = 25;
            this._editMainButton.Text = "Редактировать сводное описание";
            this._editMainButton.UseVisualStyleBackColor = true;
            this._editMainButton.Click += new System.EventHandler(this._editMainButton_Click);
            // 
            // _unbindButton
            // 
            this._unbindButton.Location = new System.Drawing.Point(599, 436);
            this._unbindButton.Name = "_unbindButton";
            this._unbindButton.Size = new System.Drawing.Size(177, 23);
            this._unbindButton.TabIndex = 26;
            this._unbindButton.Text = "Расшить номера подшивки";
            this._unbindButton.UseVisualStyleBackColor = true;
            this._unbindButton.Click += new System.EventHandler(this._unbindButton_Click);
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Location = new System.Drawing.Point(11, 658);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(992, 103);
            this._logBox.TabIndex = 27;
            // 
            // _allBox
            // 
            this._allBox.AutoSize = true;
            this._allBox.Location = new System.Drawing.Point(600, 408);
            this._allBox.Name = "_allBox";
            this._allBox.Size = new System.Drawing.Size(97, 17);
            this._allBox.TabIndex = 28;
            this._allBox.Text = "Выделить все";
            this._allBox.UseVisualStyleBackColor = true;
            this._allBox.CheckedChanged += new System.EventHandler(this._allBox_CheckedChanged);
            // 
            // _updateButton
            // 
            this._updateButton.Location = new System.Drawing.Point(433, 408);
            this._updateButton.Name = "_updateButton";
            this._updateButton.Size = new System.Drawing.Size(160, 23);
            this._updateButton.TabIndex = 29;
            this._updateButton.Text = "Обновить сведения";
            this._updateButton.UseVisualStyleBackColor = true;
            this._updateButton.Click += new System.EventHandler(this._updateButton_Click);
            // 
            // _bindingBox
            // 
            this._bindingBox.FormattingEnabled = true;
            this._bindingBox.Location = new System.Drawing.Point(782, 98);
            this._bindingBox.Name = "_bindingBox";
            this._bindingBox.Size = new System.Drawing.Size(221, 303);
            this._bindingBox.TabIndex = 30;
            // 
            // _removeDublesButton
            // 
            this._removeDublesButton.Location = new System.Drawing.Point(782, 407);
            this._removeDublesButton.Name = "_removeDublesButton";
            this._removeDublesButton.Size = new System.Drawing.Size(219, 23);
            this._removeDublesButton.TabIndex = 31;
            this._removeDublesButton.Text = "Удалить повторы комплектов";
            this._removeDublesButton.UseVisualStyleBackColor = true;
            this._removeDublesButton.Click += new System.EventHandler(this._removeDublesButton_Click);
            // 
            // _printButton
            // 
            this._printButton.Location = new System.Drawing.Point(782, 436);
            this._printButton.Name = "_printButton";
            this._printButton.Size = new System.Drawing.Size(119, 23);
            this._printButton.TabIndex = 32;
            this._printButton.Text = "Печатать этикетку";
            this._printButton.UseVisualStyleBackColor = true;
            this._printButton.Click += new System.EventHandler(this._printButton_Click);
            // 
            // _timer2
            // 
            this._timer2.Tick += new System.EventHandler(this._timer2_Tick);
            // 
            // _idleTimer
            // 
            this._idleTimer.Enabled = true;
            this._idleTimer.Interval = 60000;
            this._idleTimer.Tick += new System.EventHandler(this._idleTimer_Tick);
            // 
            // _deleteComplectButton
            // 
            this._deleteComplectButton.Location = new System.Drawing.Point(684, 544);
            this._deleteComplectButton.Name = "_deleteComplectButton";
            this._deleteComplectButton.Size = new System.Drawing.Size(317, 23);
            this._deleteComplectButton.TabIndex = 33;
            this._deleteComplectButton.Text = "Удалить комплект";
            this._deleteComplectButton.UseVisualStyleBackColor = true;
            this._deleteComplectButton.Click += new System.EventHandler(this._deleteComplectButton_Click);
            // 
            // _printButton2
            // 
            this._printButton2.Location = new System.Drawing.Point(907, 436);
            this._printButton2.Name = "_printButton2";
            this._printButton2.Size = new System.Drawing.Size(94, 23);
            this._printButton2.TabIndex = 34;
            this._printButton2.Text = "Печатать 2";
            this._printButton2.UseVisualStyleBackColor = true;
            this._printButton2.Click += new System.EventHandler(this._printButton2_Click);
            // 
            // _dontCheck
            // 
            this._dontCheck.AutoSize = true;
            this._dontCheck.Location = new System.Drawing.Point(986, 466);
            this._dontCheck.Name = "_dontCheck";
            this._dontCheck.Size = new System.Drawing.Size(15, 14);
            this._dontCheck.TabIndex = 35;
            this._dontCheck.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 773);
            this.Controls.Add(this._dontCheck);
            this.Controls.Add(this._printButton2);
            this.Controls.Add(this._deleteComplectButton);
            this.Controls.Add(this._printButton);
            this.Controls.Add(this._removeDublesButton);
            this.Controls.Add(this._bindingBox);
            this.Controls.Add(this._updateButton);
            this.Controls.Add(this._allBox);
            this.Controls.Add(this._logBox);
            this.Controls.Add(this._unbindButton);
            this.Controls.Add(this._editMainButton);
            this.Controls.Add(this._addButton);
            this.Controls.Add(this._addBox);
            this.Controls.Add(this._editButton);
            this.Controls.Add(this._fixButton);
            this.Controls.Add(this._keyBox);
            this.Controls.Add(this._destinationBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this._descriptionBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this._resultBrowser);
            this.Controls.Add(this._complectBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._placeBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._exemplarBox);
            this.Controls.Add(this._bindButton);
            this.Controls.Add(this._rfidBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._inventoryBox);
            this.Controls.Add(this._numberBox);
            this.Controls.Add(this._yearBox);
            this.Controls.Add(this._magazineBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Подшиватель";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox _magazineBox;
        private System.Windows.Forms.ListBox _yearBox;
        private System.Windows.Forms.CheckedListBox _numberBox;
        private System.Windows.Forms.TextBox _inventoryBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _rfidBox;
        private System.Windows.Forms.Button _bindButton;
        private System.Windows.Forms.ListBox _exemplarBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox _placeBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox _complectBox;
        private System.Windows.Forms.WebBrowser _resultBrowser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox _descriptionBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox _destinationBox;
        private System.Windows.Forms.TextBox _keyBox;
        private System.Windows.Forms.Timer _timer1;
        private System.Windows.Forms.Button _fixButton;
        private System.Windows.Forms.Button _editButton;
        private System.Windows.Forms.TextBox _addBox;
        private System.Windows.Forms.Button _addButton;
        private System.Windows.Forms.Button _editMainButton;
        private System.Windows.Forms.Button _unbindButton;
        private System.Windows.Forms.TextBox _logBox;
        private System.Windows.Forms.CheckBox _allBox;
        private System.Windows.Forms.Button _updateButton;
        private System.Windows.Forms.ListBox _bindingBox;
        private System.Windows.Forms.Button _removeDublesButton;
        private System.Windows.Forms.Button _printButton;
        private System.Windows.Forms.Timer _timer2;
        private System.Windows.Forms.Timer _idleTimer;
        private System.Windows.Forms.Button _deleteComplectButton;
        private System.Windows.Forms.Button _printButton2;
        private System.Windows.Forms.CheckBox _dontCheck;

    }
}

