// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace AM.Windows.Forms
{
    partial class TextBoxWithButton
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
            this.TextBox = new System.Windows.Forms.TextBox();
            this.Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _textBox
            // 
            this.TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox.Location = new System.Drawing.Point(0, 0);
            this.TextBox.Name = "TextBox";
            this.TextBox.Size = new System.Drawing.Size(228, 20);
            this.TextBox.TabIndex = 0;
            // 
            // _button
            // 
            this.Button.Dock = System.Windows.Forms.DockStyle.Right;
            this.Button.Location = new System.Drawing.Point(228, 0);
            this.Button.Name = "Button";
            this.Button.Size = new System.Drawing.Size(23, 21);
            this.Button.TabIndex = 1;
            this.Button.UseVisualStyleBackColor = true;
            // 
            // TextBoxWithButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TextBox);
            this.Controls.Add(this.Button);
            this.Name = "TextBoxWithButton";
            this.Size = new System.Drawing.Size(251, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
