// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BorderInfoControl.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    class BorderInfoControl
        : UserControl
    {
        private readonly IWindowsFormsEditorService _editorService;

        private CheckBox? drawBox;
        private CheckBox? draw3D;
        private ComboBox? style2D;
        private ComboBox? style3D;
        private Panel? panel1;
        private Label? label1;
        private Label? label2;
        private Label? label3;
        private Button? okButton;
        private Button? cancelButton;
        private Panel? color2D;

        public BorderInfo? Result;

        public BorderInfoControl
            (
                BorderInfo binfo,
                IWindowsFormsEditorService editorService
            )
        {
            _editorService = editorService;
            InitializeComponent();

            drawBox!.Checked = binfo.DrawBorder;
            draw3D!.Checked = binfo.Draw3D;

            foreach (var o in Enum.GetValues(typeof(ButtonBorderStyle)))
            {
                style2D!.Items.Add(o);
            }

            style2D!.SelectedItem = binfo.Style2D;

            foreach (var o in Enum.GetValues(typeof(Border3DStyle)))
            {
                style3D!.Items.Add(o);
            }

            style3D!.SelectedItem = binfo.Style3D;
            color2D!.BackColor = binfo.BorderColor;
        }

        private BorderInfo _Border()
        {
            var result = new BorderInfo
            {
                DrawBorder = drawBox!.Checked,
                Draw3D = draw3D!.Checked,
                Style2D = (ButtonBorderStyle) style2D!.SelectedItem,
                Style3D = (Border3DStyle) style3D!.SelectedItem,
                BorderColor = color2D!.BackColor
            };
            return result;
        }

        private void okButton_Click
            (
                object? sender,
                EventArgs e
            )
        {
            Result = _Border();
            _editorService.CloseDropDown();
        }

        private void cancelButton_Click
            (
                object? sender,
                EventArgs e
            )
        {
            Result = null;
            _editorService.CloseDropDown();
        }

        private void drawBox_CheckedChanged
            (
                object? sender,
                EventArgs e
            )
        {
            panel1?.Invalidate();
        }

        private void panel1_Paint
            (
                object? sender,
                PaintEventArgs e
            )
        {
            var b = _Border();
            b.Draw(e.Graphics, panel1!.ClientRectangle);
        }

        private void color2D_Click
            (
                object? sender,
                EventArgs e
            )
        {
            using var dialog = new ColorDialog
            {
                Color = color2D!.BackColor
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                color2D.BackColor = dialog.Color;
                drawBox_CheckedChanged(sender, e);
            }
        }

        private void BorderInfoControl_Load
            (
                object? sender,
                EventArgs e
            )
        {
            BackColor = SystemColors.Control;
            //ParentForm.AcceptButton = okButton;
            //ParentForm.CancelButton = cancelButton;
        }

        private void InitializeComponent()
        {
            drawBox = new CheckBox();
            draw3D = new CheckBox();
            style2D = new ComboBox();
            style3D = new ComboBox();
            panel1 = new Panel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            okButton = new Button();
            cancelButton = new Button();
            color2D = new Panel();
            SuspendLayout();
            //
            // drawBox
            //
            drawBox.AutoSize = true;
            drawBox.Location = new Point(13, 13);
            drawBox.Name = "drawBox";
            drawBox.Size = new Size(108, 21);
            drawBox.TabIndex = 0;
            drawBox.Text = "Draw border";
            drawBox.CheckedChanged += drawBox_CheckedChanged;
            //
            // draw3D
            //
            draw3D.AutoSize = true;
            draw3D.Location = new Point(122, 13);
            draw3D.Name = "draw3D";
            draw3D.Size = new Size(94, 21);
            draw3D.TabIndex = 1;
            draw3D.Text = "3D border";
            draw3D.CheckedChanged += drawBox_CheckedChanged;
            //
            // style2D
            //
            style2D.DropDownStyle = ComboBoxStyle.DropDownList;
            style2D.FormattingEnabled = true;
            style2D.Location = new Point(73, 37);
            style2D.Margin = new Padding(2, 3, 3, 3);
            style2D.Name = "style2D";
            style2D.Size = new Size(139, 24);
            style2D.TabIndex = 2;
            style2D.SelectedIndexChanged += drawBox_CheckedChanged;
            //
            // style3D
            //
            style3D.DropDownStyle = ComboBoxStyle.DropDownList;
            style3D.FormattingEnabled = true;
            style3D.Location = new Point(73, 95);
            style3D.Name = "style3D";
            style3D.Size = new Size(139, 24);
            style3D.TabIndex = 4;
            style3D.SelectedIndexChanged += drawBox_CheckedChanged;
            //
            // panel1
            //
            panel1.Location = new Point(14, 124);
            panel1.Name = "panel1";
            panel1.Size = new Size(198, 72);
            panel1.TabIndex = 5;
            panel1.Paint += panel1_Paint;
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new Point(15, 44);
            label1.Margin = new Padding(3, 3, 1, 3);
            label1.Name = "label1";
            label1.Size = new Size(59, 17);
            label1.TabIndex = 6;
            label1.Text = "2D style";
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Location = new Point(14, 74);
            label2.Name = "label2";
            label2.Size = new Size(61, 17);
            label2.TabIndex = 7;
            label2.Text = "2D color";
            //
            // label3
            //
            label3.AutoSize = true;
            label3.Location = new Point(13, 103);
            label3.Name = "label3";
            label3.Size = new Size(59, 17);
            label3.TabIndex = 8;
            label3.Text = "3D style";
            //
            // okButton
            //
            okButton.Location = new Point(14, 203);
            okButton.Name = "okButton";
            okButton.Size = new Size(107, 23);
            okButton.TabIndex = 9;
            okButton.Text = "OK";
            okButton.Click += okButton_Click;
            //
            // cancelButton
            //
            cancelButton.Location = new Point(122, 203);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(90, 23);
            cancelButton.TabIndex = 10;
            cancelButton.Text = "Cancel";
            cancelButton.Click += cancelButton_Click;
            //
            // color2D
            //
            color2D.BorderStyle = BorderStyle.FixedSingle;
            color2D.Location = new Point(73, 68);
            color2D.Name = "color2D";
            color2D.Size = new Size(139, 19);
            color2D.TabIndex = 11;
            color2D.Click += color2D_Click;
            //
            // BorderInfoControl
            //
            Controls.Add(color2D);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(panel1);
            Controls.Add(style3D);
            Controls.Add(style2D);
            Controls.Add(draw3D);
            Controls.Add(drawBox);
            Name = "BorderInfoControl";
            Size = new Size(224, 239);
            Load += BorderInfoControl_Load;
            ResumeLayout(false);
            PerformLayout();

        }
    }
}
