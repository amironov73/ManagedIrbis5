// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* PenInfoControl.cs -- контрол, отображающий свойства пера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Контрол, отображающий свойства пера.
    /// </summary>
    class PenInfoControl
        : UserControl
    {
        private readonly IWindowsFormsEditorService _editorService;

        private ComboBox? alignment;
        private Panel? colorPanel;
        private ComboBox? dashStyle;
        private ComboBox? startCap;
        private ComboBox? endcap;
        private ComboBox? lineJoin;
        private NumericUpDown? width;
        private Button? okButton;
        private Button? cancelButton;
        private Label? label1;
        private Label? label2;
        private Label? label3;
        private Label? label4;
        private Label? label5;
        private Label? label6;
        private Label? label7;
        private Panel? panel1;
        private ComboBox? dashCap;
        private Label? label8;

        public PenInfo? Result;

        public PenInfoControl
            (
                PenInfo pinfo,
                IWindowsFormsEditorService editorService
            )
        {
            _editorService = editorService;
            InitializeComponent ();

            colorPanel!.BackColor = pinfo.Color;

            foreach ( object o in Enum.GetValues ( typeof ( PenAlignment ) ) )
            {
                alignment!.Items.Add ( o );
            }

            alignment!.SelectedItem = pinfo.Alignment;

            foreach ( object o in Enum.GetValues ( typeof ( DashStyle ) ) )
            {
                dashStyle!.Items.Add ( o );
            }

            dashStyle!.SelectedItem= pinfo.DashStyle;

            foreach ( object o in Enum.GetValues ( typeof ( LineCap ) ) )
            {
                startCap!.Items.Add ( o );
                endcap!.Items.Add ( o );
            }

            startCap!.SelectedItem = pinfo.StartCap;
            endcap!.SelectedItem = pinfo.EndCap;

            foreach ( object o in Enum.GetValues ( typeof ( LineJoin ) ) )
            {
                lineJoin!.Items.Add ( o );
            }

            lineJoin!.SelectedItem = pinfo.LineJoin;

            foreach ( object o in Enum.GetValues ( typeof ( DashCap ) ) )
            {
                dashCap!.Items.Add ( o );
            }

            dashCap!.SelectedItem = pinfo.DashCap;
            width!.Value = (decimal) pinfo.Width;
        }

        private PenInfo GetPenInfo ()
        {
            var result = new PenInfo
            {
                Alignment = (PenAlignment) alignment!.SelectedItem,
                DashStyle = (DashStyle) dashStyle!.SelectedItem,
                Color = colorPanel!.BackColor,
                StartCap = (LineCap) startCap!.SelectedItem,
                EndCap = (LineCap) endcap!.SelectedItem,
                LineJoin = (LineJoin) lineJoin!.SelectedItem,
                DashCap = (DashCap) dashCap!.SelectedItem,
                Width = (float) width!.Value
            };
            return result;
        }

        private void okButton_Click
            (
                object? sender,
                EventArgs e
            )
        {
            Result = GetPenInfo ();
            _editorService.CloseDropDown ();
        }

        private void cancelButton_Click
            (
                object? sender,
                EventArgs e
            )
        {
            Result = null;
            _editorService.CloseDropDown ();
        }

        private void panel1_Paint
            (
                object? sender,
                PaintEventArgs e
            )
        {
            Graphics g = e.Graphics;
            using var pen = GetPenInfo ().ToPen ();
            int y = panel1!.Height / 2;
            g.DrawLine ( pen, 10, y, panel1.Width - 10, y );
        }

        private void color_Click
            (
                object? sender,
                EventArgs e
            )
        {
            using var dialog = new ColorDialog ();
            dialog.Color = colorPanel!.BackColor;
            if ( dialog.ShowDialog () == DialogResult.OK )
            {
                colorPanel.BackColor = dialog.Color;
                alignment_SelectedIndexChanged ( sender, e );
            }
        }

        private void alignment_SelectedIndexChanged
            (
                object? sender,
                EventArgs e
            )
        {
            panel1!.Invalidate ();
        }

        #nullable disable

        private void InitializeComponent ()
        {
            alignment = new ComboBox();
            colorPanel = new Panel();
            dashStyle = new ComboBox();
            startCap = new ComboBox();
            endcap = new ComboBox();
            lineJoin = new ComboBox();
            width = new NumericUpDown();
            okButton = new Button();
            cancelButton = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            panel1 = new Panel();
            dashCap = new ComboBox();
            label8 = new Label();
            ((ISupportInitialize)(width)).BeginInit();
            SuspendLayout();
            //
            // alignment
            //
            alignment.DropDownStyle = ComboBoxStyle.DropDownList;
            alignment.FormattingEnabled = true;
            alignment.Location = new Point(92, 9);
            alignment.Margin = new Padding(2, 3, 3, 3);
            alignment.Name = "alignment";
            alignment.Size = new Size(121, 24);
            alignment.TabIndex = 0;
            alignment.SelectedIndexChanged += alignment_SelectedIndexChanged;
            //
            // color
            //
            colorPanel.BorderStyle = BorderStyle.FixedSingle;
            colorPanel.Location = new Point(93, 37);
            colorPanel.Name = "colorPanel";
            colorPanel.Size = new Size(120, 24);
            colorPanel.TabIndex = 1;
            colorPanel.Click += color_Click;
            //
            // dashStyle
            //
            dashStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            dashStyle.FormattingEnabled = true;
            dashStyle.Location = new Point(93, 68);
            dashStyle.Margin = new Padding(1, 3, 3, 3);
            dashStyle.Name = "dashStyle";
            dashStyle.Size = new Size(121, 24);
            dashStyle.TabIndex = 2;
            dashStyle.SelectedIndexChanged += alignment_SelectedIndexChanged;
            //
            // startCap
            //
            startCap.DropDownStyle = ComboBoxStyle.DropDownList;
            startCap.FormattingEnabled = true;
            startCap.Location = new Point(92, 96);
            startCap.Name = "startCap";
            startCap.Size = new Size(121, 24);
            startCap.TabIndex = 3;
            startCap.SelectedIndexChanged += alignment_SelectedIndexChanged;
            //
            // endcap
            //
            endcap.DropDownStyle = ComboBoxStyle.DropDownList;
            endcap.FormattingEnabled = true;
            endcap.Location = new Point(92, 124);
            endcap.Name = "endcap";
            endcap.Size = new Size(121, 24);
            endcap.TabIndex = 4;
            endcap.SelectedIndexChanged += alignment_SelectedIndexChanged;
            //
            // lineJoin
            //
            lineJoin.DropDownStyle = ComboBoxStyle.DropDownList;
            lineJoin.FormattingEnabled = true;
            lineJoin.Location = new Point(93, 152);
            lineJoin.Name = "lineJoin";
            lineJoin.Size = new Size(121, 24);
            lineJoin.TabIndex = 5;
            lineJoin.SelectedIndexChanged += alignment_SelectedIndexChanged;
            //
            // width
            //
            width.Location = new Point(94, 180);
            width.Name = "width";
            width.Size = new Size(120, 22);
            width.TabIndex = 6;
            width.ValueChanged += alignment_SelectedIndexChanged;
            //
            // okButton
            //
            okButton.Location = new Point(12, 307);
            okButton.Name = "okButton";
            okButton.Size = new Size(93, 23);
            okButton.TabIndex = 7;
            okButton.Text = "Ok";
            okButton.Click += okButton_Click;
            //
            // cancelButton
            //
            cancelButton.Location = new Point(111, 307);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(102, 23);
            cancelButton.TabIndex = 8;
            cancelButton.Text = "Cancel";
            cancelButton.Click += cancelButton_Click;
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Location = new Point(8, 16);
            label1.Margin = new Padding(3, 3, 1, 3);
            label1.Name = "label1";
            label1.Size = new Size(70, 17);
            label1.TabIndex = 9;
            label1.Text = "Alignment";
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Location = new Point(8, 46);
            label2.Name = "label2";
            label2.Size = new Size(41, 17);
            label2.TabIndex = 10;
            label2.Text = "Color";
            //
            // label3
            //
            label3.AutoSize = true;
            label3.Location = new Point(8, 75);
            label3.Margin = new Padding(3, 3, 1, 3);
            label3.Name = "label3";
            label3.Size = new Size(74, 17);
            label3.TabIndex = 11;
            label3.Text = "Dash style";
            //
            // label4
            //
            label4.AutoSize = true;
            label4.Location = new Point(8, 103);
            label4.Name = "label4";
            label4.Size = new Size(65, 17);
            label4.TabIndex = 12;
            label4.Text = "Start cap";
            //
            // label5
            //
            label5.AutoSize = true;
            label5.Location = new Point(8, 131);
            label5.Name = "label5";
            label5.Size = new Size(60, 17);
            label5.TabIndex = 13;
            label5.Text = "End cap";
            //
            // label6
            //
            label6.AutoSize = true;
            label6.Location = new Point(8, 159);
            label6.Name = "label6";
            label6.Size = new Size(61, 17);
            label6.TabIndex = 14;
            label6.Text = "Line join";
            //
            // label7
            //
            label7.AutoSize = true;
            label7.Location = new Point(9, 185);
            label7.Name = "label7";
            label7.Size = new Size(44, 17);
            label7.TabIndex = 15;
            label7.Text = "Width";
            //
            // panel1
            //
            panel1.Location = new Point(10, 239);
            panel1.Name = "panel1";
            panel1.Size = new Size(203, 61);
            panel1.TabIndex = 16;
            panel1.Paint += panel1_Paint;
            //
            // dashCap
            //
            dashCap.DropDownStyle = ComboBoxStyle.DropDownList;
            dashCap.FormattingEnabled = true;
            dashCap.Location = new Point(94, 207);
            dashCap.Name = "dashCap";
            dashCap.Size = new Size(121, 24);
            dashCap.TabIndex = 17;
            dashCap.SelectedIndexChanged += alignment_SelectedIndexChanged;
            //
            // label8
            //
            label8.AutoSize = true;
            label8.Location = new Point(9, 214);
            label8.Name = "label8";
            label8.Size = new Size(68, 17);
            label8.TabIndex = 18;
            label8.Text = "Dash cap";
            //
            // PenInfoControl
            //
            Controls.Add(label8);
            Controls.Add(dashCap);
            Controls.Add(panel1);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Controls.Add(width);
            Controls.Add(lineJoin);
            Controls.Add(endcap);
            Controls.Add(startCap);
            Controls.Add(dashStyle);
            Controls.Add(colorPanel);
            Controls.Add(alignment);
            Name = "PenInfoControl";
            Size = new Size(229, 338);
            ((ISupportInitialize)(width)).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

    } // class PenInfoControl

} // namespace AM.Windows.Forms
