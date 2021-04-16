namespace AM.Windows.Forms
{
    partial class RichEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose ();
            }
            base.Dispose ( disposing );
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.components = new System.ComponentModel.Container ();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( RichEditor ) );
            this.toolStrip = new System.Windows.Forms.ToolStrip ();
            this.newButton = new System.Windows.Forms.ToolStripButton ();
            this.openButton = new System.Windows.Forms.ToolStripButton ();
            this.saveButton = new System.Windows.Forms.ToolStripButton ();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator ();
            this.printButton = new System.Windows.Forms.ToolStripButton ();
            this.setupButton = new System.Windows.Forms.ToolStripButton ();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator ();
            this.findButton = new System.Windows.Forms.ToolStripButton ();
            this.replaceButton = new System.Windows.Forms.ToolStripButton ();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator ();
            this.cutButton = new System.Windows.Forms.ToolStripButton ();
            this.copyButton = new System.Windows.Forms.ToolStripButton ();
            this.pasteButton = new System.Windows.Forms.ToolStripButton ();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator ();
            this.fontButton = new System.Windows.Forms.ToolStripSplitButton ();
            this.fontMenu = new System.Windows.Forms.ContextMenuStrip ( this.components );
            this.sizeBox = new System.Windows.Forms.ToolStripComboBox ();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator ();
            this.colorButton = new System.Windows.Forms.ToolStripSplitButton ();
            this.colorMenu = new System.Windows.Forms.ContextMenuStrip ( this.components );
            this.blackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
            this.blueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
            this.redToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
            this.greenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator ();
            this.boldButton = new System.Windows.Forms.ToolStripButton ();
            this.italicButton = new System.Windows.Forms.ToolStripButton ();
            this.underlineButton = new System.Windows.Forms.ToolStripButton ();
            this.strikeoutButton = new System.Windows.Forms.ToolStripButton ();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator ();
            this.leftButton = new System.Windows.Forms.ToolStripButton ();
            this.centerButton = new System.Windows.Forms.ToolStripButton ();
            this.rightButton = new System.Windows.Forms.ToolStripButton ();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator ();
            this.undoButton = new System.Windows.Forms.ToolStripButton ();
            this.redoButton = new System.Windows.Forms.ToolStripButton ();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator ();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog ();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog ();
            this.fontDialog = new System.Windows.Forms.FontDialog ();
            this.colorDialog = new System.Windows.Forms.ColorDialog ();
            this.rtfBox = new System.Windows.Forms.RichTextBox ();
            this.toolStrip.SuspendLayout ();
            this.colorMenu.SuspendLayout ();
            this.SuspendLayout ();
            //
            // toolStrip
            //
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange ( new System.Windows.Forms.ToolStripItem [] {
            this.newButton,
            this.openButton,
            this.saveButton,
            this.toolStripSeparator8,
            this.printButton,
            this.setupButton,
            this.toolStripSeparator1,
            this.findButton,
            this.replaceButton,
            this.toolStripSeparator9,
            this.cutButton,
            this.copyButton,
            this.pasteButton,
            this.toolStripSeparator5,
            this.fontButton,
            this.sizeBox,
            this.toolStripSeparator7,
            this.colorButton,
            this.toolStripSeparator6,
            this.boldButton,
            this.italicButton,
            this.underlineButton,
            this.strikeoutButton,
            this.toolStripSeparator2,
            this.leftButton,
            this.centerButton,
            this.rightButton,
            this.toolStripSeparator3,
            this.undoButton,
            this.redoButton,
            this.toolStripSeparator4} );
            this.toolStrip.Location = new System.Drawing.Point ( 0, 0 );
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size ( 640, 25 );
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            //
            // newButton
            //
            this.newButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newButton.Image = AM.Windows.Forms.Properties.Resources.NEW;
            this.newButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.newButton.Name = "newButton";
            this.newButton.Text = "New";
            this.newButton.Click += new System.EventHandler ( this.newButton_Click );
            //
            // openButton
            //
            this.openButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openButton.Image = AM.Windows.Forms.Properties.Resources.OPEN;
            this.openButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.openButton.Name = "openButton";
            this.openButton.Text = "Open...";
            this.openButton.Click += new System.EventHandler ( this.openButton_Click );
            //
            // saveButton
            //
            this.saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveButton.Image = AM.Windows.Forms.Properties.Resources.SAVE;
            this.saveButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.saveButton.Name = "saveButton";
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler ( this.saveButton_Click );
            //
            // toolStripSeparator8
            //
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            //
            // printButton
            //
            this.printButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printButton.Image = AM.Windows.Forms.Properties.Resources.PRINT;
            this.printButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.printButton.Name = "printButton";
            this.printButton.Text = "Print";
            this.printButton.Click += new System.EventHandler ( this.printButton_Click );
            //
            // setupButton
            //
            this.setupButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.setupButton.Image = AM.Windows.Forms.Properties.Resources.PROP;
            this.setupButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.setupButton.Name = "setupButton";
            this.setupButton.Text = "Setup";
            //
            // toolStripSeparator1
            //
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            //
            // findButton
            //
            this.findButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.findButton.Image = AM.Windows.Forms.Properties.Resources.FIND;
            this.findButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.findButton.Name = "findButton";
            this.findButton.Text = "Find";
            this.findButton.Click += new System.EventHandler ( this.findButton_Click );
            //
            // replaceButton
            //
            this.replaceButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.replaceButton.Image = AM.Windows.Forms.Properties.Resources.SMALLCAP;
            this.replaceButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.replaceButton.Name = "replaceButton";
            this.replaceButton.Text = "Replace";
            //
            // toolStripSeparator9
            //
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            //
            // cutButton
            //
            this.cutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cutButton.Image = AM.Windows.Forms.Properties.Resources.CUT;
            this.cutButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.cutButton.Name = "cutButton";
            this.cutButton.Text = "Cut";
            this.cutButton.Click += new System.EventHandler ( this.cutButton_Click );
            //
            // copyButton
            //
            this.copyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyButton.Image = AM.Windows.Forms.Properties.Resources.COPY;
            this.copyButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.copyButton.Name = "copyButton";
            this.copyButton.Text = "Copy";
            this.copyButton.Click += new System.EventHandler ( this.copyButton_Click );
            //
            // pasteButton
            //
            this.pasteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pasteButton.Image = AM.Windows.Forms.Properties.Resources.PASTE;
            this.pasteButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.pasteButton.Name = "pasteButton";
            this.pasteButton.Text = "Paste";
            this.pasteButton.Click += new System.EventHandler ( this.pasteButton_Click );
            //
            // toolStripSeparator5
            //
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            //
            // fontButton
            //
            this.fontButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fontButton.DropDown = this.fontMenu;
            this.fontButton.Image = ( (System.Drawing.Image) ( resources.GetObject ( "fontButton.Image" ) ) );
            this.fontButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fontButton.Name = "fontButton";
            this.fontButton.Text = "Font";
            this.fontButton.ButtonClick += new System.EventHandler ( this.fontButton_Click );
            this.fontButton.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler ( this.fontButton_DropDownItemClicked );
            //
            // fontMenu
            //
            this.fontMenu.Enabled = true;
            this.fontMenu.GripMargin = new System.Windows.Forms.Padding ( 2 );
            this.fontMenu.ImageScalingSize = new System.Drawing.Size ( 8, 8 );
            this.fontMenu.Location = new System.Drawing.Point ( 269, 37 );
            this.fontMenu.Name = "fontMenu";
            this.fontMenu.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fontMenu.ShowImageMargin = false;
            this.fontMenu.Size = new System.Drawing.Size ( 36, 4 );
            //
            // sizeBox
            //
            this.sizeBox.AutoSize = false;
            this.sizeBox.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.sizeBox.Items.AddRange ( new object [] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "16",
            "18",
            "20",
            "26",
            "32",
            "72"} );
            this.sizeBox.Name = "sizeBox";
            this.sizeBox.Size = new System.Drawing.Size ( 40, 25 );
            this.sizeBox.SelectedIndexChanged += new System.EventHandler ( this.sizeBox_SelectedIndexChanged );
            //
            // toolStripSeparator7
            //
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            //
            // colorButton
            //
            this.colorButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.colorButton.DropDown = this.colorMenu;
            this.colorButton.Image = ( (System.Drawing.Image) ( resources.GetObject ( "colorButton.Image" ) ) );
            this.colorButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.colorButton.Name = "colorButton";
            this.colorButton.Text = "Color";
            this.colorButton.ButtonClick += new System.EventHandler ( this.colorButton_Click );
            this.colorButton.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler ( this.colorButton_DropDownItemClicked );
            //
            // colorMenu
            //
            this.colorMenu.Enabled = true;
            this.colorMenu.GripMargin = new System.Windows.Forms.Padding ( 2 );
            this.colorMenu.Items.AddRange ( new System.Windows.Forms.ToolStripItem [] {
            this.blackToolStripMenuItem,
            this.blueToolStripMenuItem,
            this.redToolStripMenuItem,
            this.greenToolStripMenuItem} );
            this.colorMenu.Location = new System.Drawing.Point ( 358, 37 );
            this.colorMenu.Name = "colorMenu";
            this.colorMenu.OwnerItem = this.colorButton;
            this.colorMenu.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.colorMenu.Size = new System.Drawing.Size ( 153, 111 );
            this.colorMenu.Visible = true;
            //
            // blackToolStripMenuItem
            //
            this.blackToolStripMenuItem.Name = "blackToolStripMenuItem";
            this.blackToolStripMenuItem.Text = "Black";
            //
            // blueToolStripMenuItem
            //
            this.blueToolStripMenuItem.Name = "blueToolStripMenuItem";
            this.blueToolStripMenuItem.Text = "Blue";
            //
            // redToolStripMenuItem
            //
            this.redToolStripMenuItem.Name = "redToolStripMenuItem";
            this.redToolStripMenuItem.Text = "Red";
            //
            // greenToolStripMenuItem
            //
            this.greenToolStripMenuItem.Name = "greenToolStripMenuItem";
            this.greenToolStripMenuItem.Text = "Green";
            //
            // toolStripSeparator6
            //
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            //
            // boldButton
            //
            this.boldButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.boldButton.Image = AM.Windows.Forms.Properties.Resources.BLD;
            this.boldButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.boldButton.Name = "boldButton";
            this.boldButton.Text = "Bold";
            this.boldButton.Click += new System.EventHandler ( this.boldButton_Click );
            //
            // italicButton
            //
            this.italicButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.italicButton.Image = AM.Windows.Forms.Properties.Resources.ITL;
            this.italicButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.italicButton.Name = "italicButton";
            this.italicButton.Text = "Italic";
            this.italicButton.Click += new System.EventHandler ( this.italicButton_Click );
            //
            // underlineButton
            //
            this.underlineButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.underlineButton.Image = AM.Windows.Forms.Properties.Resources.UNDRLN;
            this.underlineButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.underlineButton.Name = "underlineButton";
            this.underlineButton.Text = "Underline";
            this.underlineButton.Click += new System.EventHandler ( this.underlineButton_Click );
            //
            // strikeoutButton
            //
            this.strikeoutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.strikeoutButton.Image = AM.Windows.Forms.Properties.Resources.STRIKTHR;
            this.strikeoutButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.strikeoutButton.Name = "strikeoutButton";
            this.strikeoutButton.Text = "Strikeout";
            this.strikeoutButton.Click += new System.EventHandler ( this.strikeoutButton_Click );
            //
            // toolStripSeparator2
            //
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            //
            // leftButton
            //
            this.leftButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.leftButton.Image = AM.Windows.Forms.Properties.Resources.LFT;
            this.leftButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.leftButton.Name = "leftButton";
            this.leftButton.Text = "Left";
            this.leftButton.Click += new System.EventHandler ( this.leftButton_Click );
            //
            // centerButton
            //
            this.centerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.centerButton.Image = AM.Windows.Forms.Properties.Resources.CTR;
            this.centerButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.centerButton.Name = "centerButton";
            this.centerButton.Text = "Center";
            this.centerButton.Click += new System.EventHandler ( this.centerButton_Click );
            //
            // rightButton
            //
            this.rightButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rightButton.Image = AM.Windows.Forms.Properties.Resources.RT;
            this.rightButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.rightButton.Name = "rightButton";
            this.rightButton.Text = "Right";
            this.rightButton.Click += new System.EventHandler ( this.rightButton_Click );
            //
            // toolStripSeparator3
            //
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            //
            // undoButton
            //
            this.undoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.undoButton.Image = AM.Windows.Forms.Properties.Resources.UNDO;
            this.undoButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.undoButton.Name = "undoButton";
            this.undoButton.Text = "Undo";
            this.undoButton.Click += new System.EventHandler ( this.undoButton_Click );
            //
            // redoButton
            //
            this.redoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.redoButton.Image = AM.Windows.Forms.Properties.Resources.REDO;
            this.redoButton.ImageTransparentColor = System.Drawing.Color.Silver;
            this.redoButton.Name = "redoButton";
            this.redoButton.Text = "Redo";
            this.redoButton.Click += new System.EventHandler ( this.redoButton_Click );
            //
            // toolStripSeparator4
            //
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            //
            // openFileDialog
            //
            this.openFileDialog.FileName = "document.rtf";
            this.openFileDialog.Filter = "Rich text files|*.rtf|Plain text files|*.txt|All files|*.*";
            this.openFileDialog.Title = "Open file";
            //
            // saveFileDialog
            //
            this.saveFileDialog.DefaultExt = "rtf";
            this.saveFileDialog.FileName = "document.rtf";
            this.saveFileDialog.Filter = "Rich text files|*.rtf|Plain text files|*.txt|All files|*.*";
            this.saveFileDialog.Title = "Save file";
            //
            // rtfBox
            //
            this.rtfBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfBox.Location = new System.Drawing.Point ( 0, 25 );
            this.rtfBox.Name = "rtfBox";
            this.rtfBox.Size = new System.Drawing.Size ( 640, 175 );
            this.rtfBox.TabIndex = 1;
            this.rtfBox.Text = "";
            this.rtfBox.SelectionChanged += new System.EventHandler ( this.rtfBox_SelectionChanged );
            this.rtfBox.TextChanged += new System.EventHandler ( this.rtfBox_TextChanged );
            //
            // RichEditor
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add ( this.rtfBox );
            this.Controls.Add ( this.toolStrip );
            this.Name = "RichEditor";
            this.Size = new System.Drawing.Size ( 640, 200 );
            this.Load += new System.EventHandler ( this.RichEditor_Load );
            this.toolStrip.ResumeLayout ( false );
            this.colorMenu.ResumeLayout ( false );
            this.ResumeLayout ( false );
            this.PerformLayout ();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton openButton;
        private System.Windows.Forms.ToolStripButton saveButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton boldButton;
        private System.Windows.Forms.ToolStripButton italicButton;
        private System.Windows.Forms.ToolStripButton underlineButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton strikeoutButton;
        private System.Windows.Forms.ToolStripButton leftButton;
        private System.Windows.Forms.ToolStripButton centerButton;
        private System.Windows.Forms.ToolStripButton rightButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton undoButton;
        private System.Windows.Forms.ToolStripButton redoButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton cutButton;
        private System.Windows.Forms.ToolStripButton copyButton;
        private System.Windows.Forms.ToolStripButton pasteButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton newButton;
        private System.Windows.Forms.ToolStripSplitButton fontButton;
        private System.Windows.Forms.ToolStripSplitButton colorButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripComboBox sizeBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ContextMenuStrip fontMenu;
        private System.Windows.Forms.ContextMenuStrip colorMenu;
        private System.Windows.Forms.ToolStripMenuItem blackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem greenToolStripMenuItem;
        private System.Windows.Forms.RichTextBox rtfBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton printButton;
        private System.Windows.Forms.ToolStripButton setupButton;
        private System.Windows.Forms.ToolStripButton findButton;
        private System.Windows.Forms.ToolStripButton replaceButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.ColorDialog colorDialog;
    }
}
