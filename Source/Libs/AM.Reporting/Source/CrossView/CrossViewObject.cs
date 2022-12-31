// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

using AM.Reporting.Matrix;
using AM.Reporting.Utils;
using AM.Reporting.Data;
using AM.Reporting.Table;

using System.Drawing.Design;

#endregion

#nullable enable

namespace AM.Reporting.CrossView
{
    /// <summary>
    /// Represents the crossview object that is used to print cube slice or slicegrid.
    /// </summary>
    public partial class CrossViewObject : TableBase
    {
        #region Fields

        //private FastCubeSource fastCubeSource;
        private CubeSourceBase cubeSource;
        private bool showTitle;
        private bool showXAxisFieldsCaption;
        private bool showYAxisFieldsCaption;
        private string style;
        private bool saveVisible;

        #endregion

        #region Properties

        /// <summary>
        /// Allows to modify the prepared matrix elements such as cells, rows, columns.
        /// </summary>
        public event EventHandler ModifyResult;

        /// <summary>
        /// Gets or sets a value indicating whether to show a title row.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool ShowTitle
        {
            get => showTitle;
            set
            {
                showTitle = value;
                if (IsDesigning)
                {
                    //Data.CreateDescriptors();
                    //FHelper.CreateOtherDescriptor();
                    BuildTemplate();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show a X Axis fields Caption.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool ShowXAxisFieldsCaption
        {
            get => showXAxisFieldsCaption;
            set
            {
                showXAxisFieldsCaption = value;
                if (IsDesigning)
                {
                    //Data.CreateDescriptors();
                    //FHelper.CreateOtherDescriptor();
                    BuildTemplate();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show a Y Axis fields Caption.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool ShowYAxisFieldsCaption
        {
            get => showYAxisFieldsCaption;
            set
            {
                showYAxisFieldsCaption = value;
                if (IsDesigning)
                {
                    //Data.CreateDescriptors();
                    //FHelper.CreateOtherDescriptor();
                    BuildTemplate();
                }
            }
        }

        /// <summary>
        /// Gets or sets a matrix style.
        /// </summary>
        [Category ("Appearance")]
        [Editor ("AM.Reporting.TypeEditors.CrossViewStyleEditor, AM.Reporting", typeof (UITypeEditor))]
        public new string Style
        {
            get => style;
            set
            {
                style = value;
                Helper.UpdateStyle();
            }
        }

        /// <summary>
        /// Gets or sets a script method name that will be used to handle the
        /// <see cref="ModifyResult"/> event.
        /// </summary>
        /// <remarks>
        /// See the <see cref="ModifyResult"/> event for more details.
        /// </remarks>
        [Category ("Build")]
        public string ModifyResultEvent { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Browsable (false)]
        public string ColumnDescriptorsIndexes
        {
            get => Data.ColumnDescriptorsIndexes;
            set
            {
                if (!IsDesigning)
                {
                    Data.ColumnDescriptorsIndexes = value;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Browsable (false)]
        public string RowDescriptorsIndexes
        {
            get => Data.RowDescriptorsIndexes;
            set
            {
                if (!IsDesigning)
                {
                    Data.RowDescriptorsIndexes = value;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Browsable (false)]
        public string ColumnTerminalIndexes
        {
            get => Data.ColumnTerminalIndexes;
            set
            {
                if (!IsDesigning)
                {
                    Data.ColumnTerminalIndexes = value;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Browsable (false)]
        public string RowTerminalIndexes
        {
            get => Data.RowTerminalIndexes;
            set
            {
                if (!IsDesigning)
                {
                    Data.RowTerminalIndexes = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a cube source.
        /// </summary>
        [Category ("Data")]
        public CubeSourceBase CubeSource
        {
            get => cubeSource;
            set
            {
                if (cubeSource != value)
                {
                    if (cubeSource != null)
                    {
                        cubeSource.Disposed -= new EventHandler (CubeSource_Disposed);
                        cubeSource.OnChanged -= new EventHandler (CubeSource_OnChanged);
                    }

                    if (value != null)
                    {
                        value.Disposed += new EventHandler (CubeSource_Disposed);
                        value.OnChanged += new EventHandler (CubeSource_OnChanged);
                    }

                    cubeSource = value;
                    Data.CubeSource = value;
                    if (IsDesigning)
                    {
                        Data.CreateDescriptors();
                        Helper.CreateOtherDescriptor();
                        BuildTemplate();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the object that holds data of Cube
        /// </summary>
        /// <remarks>
        /// See the <see cref="CrossViewData"/> class for more details.
        /// </remarks>
        [Browsable (false)]
        public CrossViewData Data { get; }

        internal MatrixStyleSheet StyleSheet { get; }

        private CrossViewHelper Helper { get; }

        private bool IsResultCrossView => !IsDesigning /*&& Data.Columns.Count == 0 && Data.Rows.Count == 0 */;

        private BandBase ParentBand
        {
            get
            {
                var parentBand = Band;
                if (parentBand is ChildBand band)
                {
                    parentBand = band.GetTopParentBand;
                }

                return parentBand;
            }
        }

        private DataBand FootersDataBand
        {
            get
            {
                DataBand dataBand = null;
                if (ParentBand is GroupFooterBand)
                {
                    dataBand = ((ParentBand as GroupFooterBand).Parent as GroupHeaderBand).GroupDataBand;
                }
                else if (ParentBand is DataFooterBand)
                {
                    dataBand = ParentBand.Parent as DataBand;
                }

                return dataBand;
            }
        }

        private bool IsOnFooter
        {
            get
            {
                var dataBand = FootersDataBand;
                if (dataBand != null)
                {
                    //                    return DataSource == dataBand.DataSource;
                }

                return false;
            }
        }

        #endregion

        #region Private Methods

        private void CreateResultTable()
        {
            SetResultTable (new TableResult());

            // assign properties from this object. Do not use Assign method: TableResult is incompatible with MatrixObject.
            ResultTable.OriginalComponent = OriginalComponent;
            ResultTable.Alias = Alias;
            ResultTable.Border = Border.Clone();
            ResultTable.Fill = Fill.Clone();
            ResultTable.Bounds = Bounds;
            ResultTable.PrintOnParent = PrintOnParent;
            ResultTable.RepeatHeaders = RepeatHeaders;
            ResultTable.RepeatRowHeaders = RepeatRowHeaders;
            ResultTable.RepeatColumnHeaders = RepeatColumnHeaders;
            ResultTable.Layout = Layout;
            ResultTable.WrappedGap = WrappedGap;
            ResultTable.AdjustSpannedCellsWidth = AdjustSpannedCellsWidth;
            ResultTable.SetReport (Report);
            ResultTable.AfterData += new EventHandler (ResultTable_AfterData);
        }

        private void DisposeResultTable()
        {
            ResultTable.Dispose();
            SetResultTable (null);
        }

        private void ResultTable_AfterData (object sender, EventArgs e)
        {
            OnModifyResult (e);
        }

        private void CubeSource_Disposed (object sender, EventArgs e)
        {
            Data.CubeSource = null;
        }

        private void CubeSource_OnChanged (object sender, EventArgs e)
        {
            Data.CreateDescriptors();
            Helper.CreateOtherDescriptor();
            BuildTemplate();
        }

        private void WireEvents (bool wire)
        {
            if (IsOnFooter)
            {
                var dataBand = FootersDataBand;
                if (wire)
                {
                    dataBand.BeforePrint += new EventHandler (dataBand_BeforePrint);
                }
                else
                {
                    dataBand.BeforePrint -= new EventHandler (dataBand_BeforePrint);
                }
            }
        }

        private void dataBand_BeforePrint (object sender, EventArgs e)
        {
            /*
                        bool firstRow = (sender as DataBand).IsFirstRow;
                        if (firstRow)
                            Helper.StartPrint();
                        Helper.AddDataRow();
            */
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override void DeserializeSubItems (FRReader reader)
        {
            if (string.Compare (reader.ItemName, "CrossViewColumns", true) == 0)
            {
                reader.Read (Data.Columns);
            }
            else if (string.Compare (reader.ItemName, "CrossViewRows", true) == 0)
            {
                reader.Read (Data.Rows);
            }
            else if (string.Compare (reader.ItemName, "CrossViewCells", true) == 0)
            {
                reader.Read (Data.Cells);
            }
            else
            {
                base.DeserializeSubItems (reader);
            }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);

            var src = source as CrossViewObject;
            CubeSource = src.CubeSource;
            ShowTitle = src.ShowTitle;
            ShowXAxisFieldsCaption = src.ShowXAxisFieldsCaption;
            ShowYAxisFieldsCaption = src.ShowYAxisFieldsCaption;
            Style = src.Style;

            //            MatrixEvenStylePriority = src.MatrixEvenStylePriority;
        }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            if (writer.SerializeTo != SerializeTo.SourcePages)
            {
                writer.Write (Data.Columns);
                writer.Write (Data.Rows);
                writer.Write (Data.Cells);
            }
            else
            {
                RefreshTemplate (true);
            }

            base.Serialize (writer);
            var c = writer.DiffObject as CrossViewObject;

            if (CubeSource != c.CubeSource)
            {
                writer.WriteRef ("CubeSource", CubeSource);
            }

            if (ColumnDescriptorsIndexes != c.ColumnDescriptorsIndexes)
            {
                writer.WriteStr ("ColumnDescriptorsIndexes", ColumnDescriptorsIndexes);
            }

            if (RowDescriptorsIndexes != c.RowDescriptorsIndexes)
            {
                writer.WriteStr ("RowDescriptorsIndexes", RowDescriptorsIndexes);
            }

            if (ColumnTerminalIndexes != c.ColumnTerminalIndexes)
            {
                writer.WriteStr ("ColumnTerminalIndexes", ColumnTerminalIndexes);
            }

            if (RowTerminalIndexes != c.RowTerminalIndexes)
            {
                writer.WriteStr ("RowTerminalIndexes", RowTerminalIndexes);
            }

            if (ShowTitle != c.ShowTitle)
            {
                writer.WriteBool ("ShowTitle", ShowTitle);
            }

            if (ShowXAxisFieldsCaption != c.ShowXAxisFieldsCaption)
            {
                writer.WriteBool ("ShowXAxisFieldsCaption", ShowXAxisFieldsCaption);
            }

            if (ShowYAxisFieldsCaption != c.ShowYAxisFieldsCaption)
            {
                writer.WriteBool ("ShowYAxisFieldsCaption", ShowYAxisFieldsCaption);
            }

            if (Style != c.Style)
            {
                writer.WriteStr ("Style", Style);
            }

            //            if (MatrixEvenStylePriority != c.MatrixEvenStylePriority)
            //                writer.WriteValue("MatrixEvenStylePriority", MatrixEvenStylePriority);
            if (ModifyResultEvent != c.ModifyResultEvent)
            {
                writer.WriteStr ("ModifyResultEvent", ModifyResultEvent);
            }
        }

        /// <summary>
        /// Creates or updates the matrix template.
        /// </summary>
        /// <remarks>
        /// Call this method after you modify the matrix descriptors using the <see cref="Data"/>
        /// object's properties.
        /// </remarks>
        public void BuildTemplate()
        {
            Helper.BuildTemplate();
        }

        #endregion

        #region Report Engine

        /// <inheritdoc/>
        public override void InitializeComponent()
        {
            base.InitializeComponent();
            WireEvents (true);
        }

        /// <inheritdoc/>
        public override void FinalizeComponent()
        {
            base.FinalizeComponent();
            WireEvents (false);
        }

        /// <inheritdoc/>
        public override void SaveState()
        {
            saveVisible = Visible;
            var parent = Parent as BandBase;
            if (!Visible || parent is { Visible: false })
            {
                return;
            }

            // create the result table that will be rendered in the preview
            CreateResultTable();
            Visible = false;

            if (parent != null)
            {
                parent.Height = Top;
                parent.CanGrow = false;
                parent.CanShrink = false;
                parent.AfterPrint += new EventHandler (ResultTable.GeneratePages);
            }
        }

        /// <inheritdoc/>
        public override void GetData()
        {
            base.GetData();
            if (Data.SourceAssigned)
            {
                //      if (!IsOnFooter)
                //      {
                Helper.StartPrint();
                Helper.AddData();

//      }

                Helper.FinishPrint();
            }
        }

        /// <inheritdoc/>
        public override void RestoreState()
        {
            var parent = Parent as BandBase;
            if (!saveVisible || parent is { Visible: false })
            {
                return;
            }

            if (parent != null)
            {
                parent.AfterPrint -= new EventHandler (ResultTable.GeneratePages);
            }

            DisposeResultTable();
            Visible = saveVisible;
        }

        /// <summary>
        /// This method fires the <b>ModifyResult</b> event and the script code connected to the <b>ModifyResultEvent</b>.
        /// </summary>
        /// <param name="e">Event data.</param>
        public void OnModifyResult (EventArgs e)
        {
            if (ModifyResult != null)
            {
                ModifyResult (this, e);
            }

            InvokeEvent (ModifyResultEvent, e);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossViewObject"/> class.
        /// </summary>
        public CrossViewObject()
        {
            //FAutoSize = true;
            showXAxisFieldsCaption = true;
            showYAxisFieldsCaption = true;
            Data = new CrossViewData();
            Helper = new CrossViewHelper (this);
            StyleSheet = new MatrixStyleSheet();
            StyleSheet.Load (ResourceLoader.GetStream ("cross.frss"));
            style = "";
            RepeatHeaders = false;
            RepeatColumnHeaders = true;
            RepeatRowHeaders = true;

            //FFilter = "";
        }
    }
}
