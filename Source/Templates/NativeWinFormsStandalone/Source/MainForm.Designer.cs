using System.Windows.Forms;

namespace WinFormsAot
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
            this.label1 = new System.Windows.Forms.Label();
            this._firstTermBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._secondTermBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._sumBox = new System.Windows.Forms.TextBox();
            this._sumButton = new System.Windows.Forms.Button();
            this._closeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Первое слагаемое";
            // 
            // _firstTermBox
            // 
            this._firstTermBox.Location = new System.Drawing.Point(12, 27);
            this._firstTermBox.Name = "_firstTermBox";
            this._firstTermBox.Size = new System.Drawing.Size(223, 23);
            this._firstTermBox.TabIndex = 1;
            this._firstTermBox.Text = "123.45";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Второе слагаемое";
            // 
            // _secondTermBox
            // 
            this._secondTermBox.Location = new System.Drawing.Point(12, 85);
            this._secondTermBox.Name = "_secondTermBox";
            this._secondTermBox.Size = new System.Drawing.Size(223, 23);
            this._secondTermBox.TabIndex = 3;
            this._secondTermBox.Text = "456.78";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Сумма";
            // 
            // _sumBox
            // 
            this._sumBox.Location = new System.Drawing.Point(12, 146);
            this._sumBox.Name = "_sumBox";
            this._sumBox.ReadOnly = true;
            this._sumBox.Size = new System.Drawing.Size(223, 23);
            this._sumBox.TabIndex = 5;
            // 
            // _sumButton
            // 
            this._sumButton.Location = new System.Drawing.Point(255, 12);
            this._sumButton.Name = "_sumButton";
            this._sumButton.Size = new System.Drawing.Size(117, 23);
            this._sumButton.TabIndex = 6;
            this._sumButton.Text = "Сложить";
            this._sumButton.UseVisualStyleBackColor = true;
            this._sumButton.Click += new System.EventHandler(this._sumButton_Click);
            // 
            // _closeButton
            // 
            this._closeButton.Location = new System.Drawing.Point(255, 41);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(117, 23);
            this._closeButton.TabIndex = 7;
            this._closeButton.Text = "Закрыть";
            this._closeButton.UseVisualStyleBackColor = true;
            this._closeButton.Click += new System.EventHandler(this._closeButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 211);
            this.Controls.Add(this._closeButton);
            this.Controls.Add(this._sumButton);
            this.Controls.Add(this._sumBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._secondTermBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._firstTermBox);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 250);
            this.MinimumSize = new System.Drawing.Size(400, 250);
            this.Name = "MainForm";
            this.Text = "AOT-калькулятор";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox _firstTermBox;
        private Label label2;
        private TextBox _secondTermBox;
        private Label label3;
        private TextBox _sumBox;
        private Button _sumButton;
        private Button _closeButton;
    }
}