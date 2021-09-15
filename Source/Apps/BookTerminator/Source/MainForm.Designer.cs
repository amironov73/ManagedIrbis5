
namespace BookTerminator
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
            this._topPanel = new System.Windows.Forms.Panel();
            this._deleteButton = new System.Windows.Forms.Button();
            this._goButton = new System.Windows.Forms.Button();
            this._mfnLabel = new System.Windows.Forms.Label();
            this._mfnBox = new System.Windows.Forms.TextBox();
            this._logBox = new AM.Windows.Forms.LogBox();
            this._dummyPanel = new System.Windows.Forms.Panel();
            this._idleTimer = new System.Windows.Forms.Timer(this.components);
            this._busyStripe = new AM.Windows.Forms.BusyStripe();
            this._topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _topPanel
            // 
            this._topPanel.Controls.Add(this._deleteButton);
            this._topPanel.Controls.Add(this._goButton);
            this._topPanel.Controls.Add(this._mfnLabel);
            this._topPanel.Controls.Add(this._mfnBox);
            this._topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._topPanel.Location = new System.Drawing.Point(0, 0);
            this._topPanel.Name = "_topPanel";
            this._topPanel.Size = new System.Drawing.Size(584, 63);
            this._topPanel.TabIndex = 0;
            // 
            // _deleteButton
            // 
            this._deleteButton.Location = new System.Drawing.Point(421, 26);
            this._deleteButton.Name = "_deleteButton";
            this._deleteButton.Size = new System.Drawing.Size(151, 23);
            this._deleteButton.TabIndex = 3;
            this._deleteButton.Text = "Удалить";
            this._deleteButton.UseVisualStyleBackColor = true;
            this._deleteButton.Click += new System.EventHandler(this._deleteButton_Click);
            // 
            // _goButton
            // 
            this._goButton.Location = new System.Drawing.Point(281, 27);
            this._goButton.Name = "_goButton";
            this._goButton.Size = new System.Drawing.Size(134, 23);
            this._goButton.TabIndex = 2;
            this._goButton.Text = "Перейти";
            this._goButton.UseVisualStyleBackColor = true;
            this._goButton.Click += new System.EventHandler(this._goButton_Click);
            // 
            // _mfnLabel
            // 
            this._mfnLabel.AutoSize = true;
            this._mfnLabel.Location = new System.Drawing.Point(12, 9);
            this._mfnLabel.Name = "_mfnLabel";
            this._mfnLabel.Size = new System.Drawing.Size(228, 15);
            this._mfnLabel.TabIndex = 1;
            this._mfnLabel.Text = "Инвентарный номер / штрих-код / RFID";
            // 
            // _mfnBox
            // 
            this._mfnBox.Location = new System.Drawing.Point(12, 27);
            this._mfnBox.Name = "_mfnBox";
            this._mfnBox.Size = new System.Drawing.Size(263, 23);
            this._mfnBox.TabIndex = 0;
            this._mfnBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this._mfnBox_KeyDown);
            // 
            // _logBox
            // 
            this._logBox.BackColor = System.Drawing.SystemColors.Window;
            this._logBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._logBox.Location = new System.Drawing.Point(0, 280);
            this._logBox.Multiline = true;
            this._logBox.Name = "_logBox";
            this._logBox.ReadOnly = true;
            this._logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._logBox.Size = new System.Drawing.Size(584, 81);
            this._logBox.TabIndex = 1;
            // 
            // _dummyPanel
            // 
            this._dummyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._dummyPanel.Location = new System.Drawing.Point(0, 80);
            this._dummyPanel.Name = "_dummyPanel";
            this._dummyPanel.Size = new System.Drawing.Size(584, 200);
            this._dummyPanel.TabIndex = 2;
            // 
            // _idleTimer
            // 
            this._idleTimer.Tick += new System.EventHandler(this._idleTimer_Tick);
            // 
            // _busyStripe
            // 
            this._busyStripe.Dock = System.Windows.Forms.DockStyle.Top;
            this._busyStripe.ForeColor = System.Drawing.Color.Lime;
            this._busyStripe.Location = new System.Drawing.Point(0, 63);
            this._busyStripe.Moving = false;
            this._busyStripe.Name = "_busyStripe";
            this._busyStripe.Size = new System.Drawing.Size(584, 17);
            this._busyStripe.TabIndex = 0;
            this._busyStripe.Text = "Выполнение команды на сервере ИРБИС64";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this._dummyPanel);
            this.Controls.Add(this._logBox);
            this.Controls.Add(this._busyStripe);
            this.Controls.Add(this._topPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "MainForm";
            this.Text = "Книжный терминатор";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this._FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this._topPanel.ResumeLayout(false);
            this._topPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel _topPanel;
        private System.Windows.Forms.Button _deleteButton;
        private System.Windows.Forms.Button _goButton;
        private System.Windows.Forms.Label _mfnLabel;
        private System.Windows.Forms.TextBox _mfnBox;
        private AM.Windows.Forms.LogBox _logBox;
        private System.Windows.Forms.Panel _dummyPanel;
        private System.Windows.Forms.Timer _idleTimer;
        private AM.Windows.Forms.BusyStripe _busyStripe;
    }
}

