namespace AM.Windows.DevEx
{
    partial class WordForm
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
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.commonBar1 = new DevExpress.XtraRichEdit.UI.CommonBar();
            this.richEditControl1 = new DevExpress.XtraRichEdit.RichEditControl();
            this.undoItem1 = new DevExpress.XtraRichEdit.UI.UndoItem();
            this.redoItem1 = new DevExpress.XtraRichEdit.UI.RedoItem();
            this.fileNewItem1 = new DevExpress.XtraRichEdit.UI.FileNewItem();
            this.fileOpenItem1 = new DevExpress.XtraRichEdit.UI.FileOpenItem();
            this.fileSaveItem1 = new DevExpress.XtraRichEdit.UI.FileSaveItem();
            this.fileSaveAsItem1 = new DevExpress.XtraRichEdit.UI.FileSaveAsItem();
            this.quickPrintItem1 = new DevExpress.XtraRichEdit.UI.QuickPrintItem();
            this.printItem1 = new DevExpress.XtraRichEdit.UI.PrintItem();
            this.printPreviewItem1 = new DevExpress.XtraRichEdit.UI.PrintPreviewItem();
            this.fileInfoBar1 = new DevExpress.XtraRichEdit.UI.FileInfoBar();
            this.encryptDocumentItem1 = new DevExpress.XtraRichEdit.UI.EncryptDocumentItem();
            this.showDocumentPropertiesFormItem1 = new DevExpress.XtraRichEdit.UI.ShowDocumentPropertiesFormItem();
            this.documentViewsBar1 = new DevExpress.XtraRichEdit.UI.DocumentViewsBar();
            this.switchToSimpleViewItem1 = new DevExpress.XtraRichEdit.UI.SwitchToSimpleViewItem();
            this.switchToDraftViewItem1 = new DevExpress.XtraRichEdit.UI.SwitchToDraftViewItem();
            this.switchToPrintLayoutViewItem1 = new DevExpress.XtraRichEdit.UI.SwitchToPrintLayoutViewItem();
            this.showBar1 = new DevExpress.XtraRichEdit.UI.ShowBar();
            this.toggleShowHorizontalRulerItem1 = new DevExpress.XtraRichEdit.UI.ToggleShowHorizontalRulerItem();
            this.toggleShowVerticalRulerItem1 = new DevExpress.XtraRichEdit.UI.ToggleShowVerticalRulerItem();
            this.zoomBar1 = new DevExpress.XtraRichEdit.UI.ZoomBar();
            this.zoomOutItem1 = new DevExpress.XtraRichEdit.UI.ZoomOutItem();
            this.zoomInItem1 = new DevExpress.XtraRichEdit.UI.ZoomInItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.richEditBarController1 = new DevExpress.XtraRichEdit.UI.RichEditBarController(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.richEditBarController1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3,
            this.commonBar1,
            this.fileInfoBar1,
            this.documentViewsBar1,
            this.showBar1,
            this.zoomBar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.undoItem1,
            this.redoItem1,
            this.fileNewItem1,
            this.fileOpenItem1,
            this.fileSaveItem1,
            this.fileSaveAsItem1,
            this.quickPrintItem1,
            this.printItem1,
            this.printPreviewItem1,
            this.encryptDocumentItem1,
            this.showDocumentPropertiesFormItem1,
            this.switchToSimpleViewItem1,
            this.switchToDraftViewItem1,
            this.switchToPrintLayoutViewItem1,
            this.toggleShowHorizontalRulerItem1,
            this.toggleShowVerticalRulerItem1,
            this.zoomOutItem1,
            this.zoomInItem1});
            this.barManager1.MaxItemId = 18;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // commonBar1
            // 
            this.commonBar1.Control = this.richEditControl1;
            this.commonBar1.DockCol = 0;
            this.commonBar1.DockRow = 0;
            this.commonBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.commonBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.undoItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.redoItem1),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.KeyTip, this.fileNewItem1, "", false, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard, "N", ""),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.KeyTip, this.fileOpenItem1, "", false, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard, "O", ""),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.KeyTip, this.fileSaveItem1, "", false, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard, "S", ""),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.KeyTip, this.fileSaveAsItem1, "", false, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard, "A", ""),
            new DevExpress.XtraBars.LinkPersistInfo(this.quickPrintItem1),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.KeyTip, this.printItem1, "", false, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard, "P", ""),
            new DevExpress.XtraBars.LinkPersistInfo(this.printPreviewItem1)});
            // 
            // richEditControl1
            // 
            this.richEditControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControl1.Location = new System.Drawing.Point(0, 49);
            this.richEditControl1.MenuManager = this.barManager1;
            this.richEditControl1.Name = "richEditControl1";
            this.richEditControl1.Options.Printing.PrintPreviewFormKind = DevExpress.XtraRichEdit.PrintPreviewFormKind.Bars;
            this.richEditControl1.Size = new System.Drawing.Size(758, 459);
            this.richEditControl1.TabIndex = 4;
            // 
            // undoItem1
            // 
            this.undoItem1.Id = 0;
            this.undoItem1.Name = "undoItem1";
            // 
            // redoItem1
            // 
            this.redoItem1.Id = 1;
            this.redoItem1.Name = "redoItem1";
            // 
            // fileNewItem1
            // 
            this.fileNewItem1.Id = 2;
            this.fileNewItem1.Name = "fileNewItem1";
            // 
            // fileOpenItem1
            // 
            this.fileOpenItem1.Id = 3;
            this.fileOpenItem1.Name = "fileOpenItem1";
            // 
            // fileSaveItem1
            // 
            this.fileSaveItem1.Id = 4;
            this.fileSaveItem1.Name = "fileSaveItem1";
            // 
            // fileSaveAsItem1
            // 
            this.fileSaveAsItem1.Id = 5;
            this.fileSaveAsItem1.Name = "fileSaveAsItem1";
            // 
            // quickPrintItem1
            // 
            this.quickPrintItem1.Id = 6;
            this.quickPrintItem1.Name = "quickPrintItem1";
            // 
            // printItem1
            // 
            this.printItem1.Id = 7;
            this.printItem1.Name = "printItem1";
            // 
            // printPreviewItem1
            // 
            this.printPreviewItem1.Id = 8;
            this.printPreviewItem1.Name = "printPreviewItem1";
            // 
            // fileInfoBar1
            // 
            this.fileInfoBar1.Control = this.richEditControl1;
            this.fileInfoBar1.DockCol = 1;
            this.fileInfoBar1.DockRow = 0;
            this.fileInfoBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.fileInfoBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.encryptDocumentItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.showDocumentPropertiesFormItem1)});
            // 
            // encryptDocumentItem1
            // 
            this.encryptDocumentItem1.Id = 9;
            this.encryptDocumentItem1.Name = "encryptDocumentItem1";
            // 
            // showDocumentPropertiesFormItem1
            // 
            this.showDocumentPropertiesFormItem1.Id = 10;
            this.showDocumentPropertiesFormItem1.Name = "showDocumentPropertiesFormItem1";
            // 
            // documentViewsBar1
            // 
            this.documentViewsBar1.Control = this.richEditControl1;
            this.documentViewsBar1.DockCol = 0;
            this.documentViewsBar1.DockRow = 1;
            this.documentViewsBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.documentViewsBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.KeyTip, this.switchToSimpleViewItem1, "", false, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard, "L", ""),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.KeyTip, this.switchToDraftViewItem1, "", false, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard, "E", ""),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.KeyTip, this.switchToPrintLayoutViewItem1, "", false, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard, "P", "")});
            // 
            // switchToSimpleViewItem1
            // 
            this.switchToSimpleViewItem1.Id = 11;
            this.switchToSimpleViewItem1.Name = "switchToSimpleViewItem1";
            // 
            // switchToDraftViewItem1
            // 
            this.switchToDraftViewItem1.Id = 12;
            this.switchToDraftViewItem1.Name = "switchToDraftViewItem1";
            // 
            // switchToPrintLayoutViewItem1
            // 
            this.switchToPrintLayoutViewItem1.Id = 13;
            this.switchToPrintLayoutViewItem1.Name = "switchToPrintLayoutViewItem1";
            // 
            // showBar1
            // 
            this.showBar1.Control = this.richEditControl1;
            this.showBar1.DockCol = 1;
            this.showBar1.DockRow = 1;
            this.showBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.showBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleShowHorizontalRulerItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.toggleShowVerticalRulerItem1)});
            this.showBar1.Offset = 132;
            // 
            // toggleShowHorizontalRulerItem1
            // 
            this.toggleShowHorizontalRulerItem1.Id = 14;
            this.toggleShowHorizontalRulerItem1.Name = "toggleShowHorizontalRulerItem1";
            // 
            // toggleShowVerticalRulerItem1
            // 
            this.toggleShowVerticalRulerItem1.Id = 15;
            this.toggleShowVerticalRulerItem1.Name = "toggleShowVerticalRulerItem1";
            // 
            // zoomBar1
            // 
            this.zoomBar1.Control = this.richEditControl1;
            this.zoomBar1.DockCol = 2;
            this.zoomBar1.DockRow = 1;
            this.zoomBar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.zoomBar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.zoomOutItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.zoomInItem1)});
            this.zoomBar1.Offset = 235;
            // 
            // zoomOutItem1
            // 
            this.zoomOutItem1.Id = 16;
            this.zoomOutItem1.Name = "zoomOutItem1";
            // 
            // zoomInItem1
            // 
            this.zoomInItem1.Id = 17;
            this.zoomInItem1.Name = "zoomInItem1";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(758, 49);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 508);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(758, 20);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 49);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 459);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(758, 49);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 459);
            // 
            // richEditBarController1
            // 
            this.richEditBarController1.BarItems.Add(this.undoItem1);
            this.richEditBarController1.BarItems.Add(this.redoItem1);
            this.richEditBarController1.BarItems.Add(this.fileNewItem1);
            this.richEditBarController1.BarItems.Add(this.fileOpenItem1);
            this.richEditBarController1.BarItems.Add(this.fileSaveItem1);
            this.richEditBarController1.BarItems.Add(this.fileSaveAsItem1);
            this.richEditBarController1.BarItems.Add(this.quickPrintItem1);
            this.richEditBarController1.BarItems.Add(this.printItem1);
            this.richEditBarController1.BarItems.Add(this.printPreviewItem1);
            this.richEditBarController1.BarItems.Add(this.encryptDocumentItem1);
            this.richEditBarController1.BarItems.Add(this.showDocumentPropertiesFormItem1);
            this.richEditBarController1.BarItems.Add(this.switchToSimpleViewItem1);
            this.richEditBarController1.BarItems.Add(this.switchToDraftViewItem1);
            this.richEditBarController1.BarItems.Add(this.switchToPrintLayoutViewItem1);
            this.richEditBarController1.BarItems.Add(this.toggleShowHorizontalRulerItem1);
            this.richEditBarController1.BarItems.Add(this.toggleShowVerticalRulerItem1);
            this.richEditBarController1.BarItems.Add(this.zoomOutItem1);
            this.richEditBarController1.BarItems.Add(this.zoomInItem1);
            this.richEditBarController1.Control = this.richEditControl1;
            // 
            // WordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 528);
            this.Controls.Add(this.richEditControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "WordForm";
            this.Text = "Under Word";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.richEditBarController1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private global::DevExpress.XtraBars.BarManager barManager1;
        private global::DevExpress.XtraBars.Bar bar3;
        private global::DevExpress.XtraBars.BarDockControl barDockControlTop;
        private global::DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private global::DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private global::DevExpress.XtraBars.BarDockControl barDockControlRight;
        private global::DevExpress.XtraRichEdit.RichEditControl richEditControl1;
        private global::DevExpress.XtraRichEdit.UI.CommonBar commonBar1;
        private global::DevExpress.XtraRichEdit.UI.UndoItem undoItem1;
        private global::DevExpress.XtraRichEdit.UI.RedoItem redoItem1;
        private global::DevExpress.XtraRichEdit.UI.FileNewItem fileNewItem1;
        private global::DevExpress.XtraRichEdit.UI.FileOpenItem fileOpenItem1;
        private global::DevExpress.XtraRichEdit.UI.FileSaveItem fileSaveItem1;
        private global::DevExpress.XtraRichEdit.UI.FileSaveAsItem fileSaveAsItem1;
        private global::DevExpress.XtraRichEdit.UI.QuickPrintItem quickPrintItem1;
        private global::DevExpress.XtraRichEdit.UI.PrintItem printItem1;
        private global::DevExpress.XtraRichEdit.UI.PrintPreviewItem printPreviewItem1;
        private global::DevExpress.XtraRichEdit.UI.FileInfoBar fileInfoBar1;
        private global::DevExpress.XtraRichEdit.UI.EncryptDocumentItem encryptDocumentItem1;
        private global::DevExpress.XtraRichEdit.UI.ShowDocumentPropertiesFormItem showDocumentPropertiesFormItem1;
        private global::DevExpress.XtraRichEdit.UI.RichEditBarController richEditBarController1;
        private global::DevExpress.XtraRichEdit.UI.DocumentViewsBar documentViewsBar1;
        private global::DevExpress.XtraRichEdit.UI.SwitchToSimpleViewItem switchToSimpleViewItem1;
        private global::DevExpress.XtraRichEdit.UI.SwitchToDraftViewItem switchToDraftViewItem1;
        private global::DevExpress.XtraRichEdit.UI.SwitchToPrintLayoutViewItem switchToPrintLayoutViewItem1;
        private global::DevExpress.XtraRichEdit.UI.ShowBar showBar1;
        private global::DevExpress.XtraRichEdit.UI.ToggleShowHorizontalRulerItem toggleShowHorizontalRulerItem1;
        private global::DevExpress.XtraRichEdit.UI.ToggleShowVerticalRulerItem toggleShowVerticalRulerItem1;
        private global::DevExpress.XtraRichEdit.UI.ZoomBar zoomBar1;
        private global::DevExpress.XtraRichEdit.UI.ZoomOutItem zoomOutItem1;
        private global::DevExpress.XtraRichEdit.UI.ZoomInItem zoomInItem1;
    }
}

