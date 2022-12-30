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
using System.Collections.Generic;
using System.ComponentModel;

using AM.Reporting.Utils;
using AM.Reporting.Data;

using System.Drawing.Design;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// This class represents the Data band.
    /// </summary>
    /// <remarks>
    /// Use the <see cref="DataSource"/> property to connect the band to a datasource. Set the
    /// <see cref="Filter"/> property if you want to filter data rows. The <see cref="Sort"/>
    /// property can be used to sort data rows.
    /// </remarks>
    public partial class DataBand : BandBase
    {
        #region Fields

        private DataHeaderBand header;
        private DataFooterBand footer;
        private DataSourceBase dataSource;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a header band.
        /// </summary>
        [Browsable (false)]
        public DataHeaderBand Header
        {
            get => header;
            set
            {
                SetProp (header, value);
                header = value;
            }
        }

        /// <summary>
        /// Gets a collection of detail bands.
        /// </summary>
        [Browsable (false)]
        public BandCollection Bands { get; }

        /// <summary>
        /// Gets or sets a footer band.
        /// </summary>
        [Browsable (false)]
        public DataFooterBand Footer
        {
            get => footer;
            set
            {
                SetProp (footer, value);
                footer = value;
            }
        }

        /// <summary>
        /// Gets or sets a data source.
        /// Please note: data source have to be enabled.
        /// </summary>
        [Category ("Data")]
        public DataSourceBase DataSource
        {
            get
            {
                if (dataSource != null && !dataSource.Enabled)
                {
                    return null;
                }

                return dataSource;
            }
            set
            {
                if (dataSource != value)
                {
                    if (dataSource != null)
                    {
                        dataSource.Disposed -= new EventHandler (DataSource_Disposed);
                    }

                    if (value != null)
                    {
                        value.Disposed += new EventHandler (DataSource_Disposed);
                    }
                }

                dataSource = value;
            }
        }

        /// <summary>
        /// Gets or sets a number of rows in the virtual data source.
        /// </summary>
        /// <remarks>
        /// Use this property if your data band is not connected to any data source. In this case
        /// the virtual data source with the specified number of rows will be used.
        /// </remarks>
        [Category ("Data")]
        [DefaultValue (1)]
        public int RowCount { get; set; }

        /// <summary>
        /// Limits the maximum number of rows in a datasource. 0 means no limit.
        /// </summary>
        [Category ("Data")]
        [DefaultValue (0)]
        public int MaxRows { get; set; }

        /// <summary>
        /// Gets or sets a relation used to establish a master-detail relationship between
        /// this band and its parent.
        /// </summary>
        /// <remarks>
        /// Use this property if there are several relations exist between two data sources.
        /// If there is only one relation (in most cases it is), you can leave this property empty.
        /// </remarks>
        [Category ("Data")]
        [Editor ("AM.Reporting.TypeEditors.RelationEditor, AM.Reporting", typeof (UITypeEditor))]
        public Relation Relation { get; set; }

        /// <summary>
        /// Gets the collection of sort conditions.
        /// </summary>
        [Browsable (false)]
        public SortCollection Sort { get; }

        /// <summary>
        /// Gets the row filter expression.
        /// </summary>
        /// <remarks>
        /// This property can contain any valid boolean expression. If the expression returns <b>false</b>,
        /// the corresponding data row will not be printed.
        /// </remarks>
        [Category ("Data")]
        [Editor ("AM.Reporting.TypeEditors.ExpressionEditor, AM.Reporting", typeof (UITypeEditor))]
        public string Filter { get; set; }

        /// <summary>
        /// Gets the band columns.
        /// </summary>
        [Category ("Appearance")]
        [Editor ("AM.Reporting.TypeEditors.DataBandColumnEditor, AM.Reporting", typeof (UITypeEditor))]
        public BandColumns Columns { get; }

        /// <summary>
        /// Gets or sets a value that determines whether to print a band if all its detail rows are empty.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool PrintIfDetailEmpty { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether to print a band if its datasource is empty.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool PrintIfDatasourceEmpty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that all band rows should be printed together on one page.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool KeepTogether { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that the band should be printed together with all its detail rows.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool KeepDetail { get; set; }

        /// <summary>
        /// Gets or sets the key column that identifies the data row.
        /// </summary>
        /// <remarks>
        /// <para>This property is used when printing a hierarchic list.</para>
        /// <para>To print the hierarchic list, you have to setup three properties: <b>IdColumn</b>,
        /// <b>ParentIdColumn</b> and <b>Indent</b>. First two properties are used to identify the data
        /// row and its parent; the <b>Indent</b> property specifies the indent that will be used to shift
        /// the databand according to its hierarchy level.</para>
        /// <para/>When printing hierarchy, AM.Reporting shifts the band to the right
        /// (by value specified in the <see cref="Indent"/> property), and also decreases the
        /// width of the band by the same value. You may use the <b>Anchor</b> property of the
        /// objects on a band to indicate whether the object should move with the band, or stay
        /// on its original position, or shrink.
        /// </remarks>
        [Category ("Hierarchy")]
        [Editor ("AM.Reporting.TypeEditors.DataColumnEditor, AM.Reporting", typeof (UITypeEditor))]
        public string IdColumn { get; set; }

        /// <summary>
        /// Gets or sets the column that identifies the parent data row.
        /// </summary>
        /// <remarks>
        /// This property is used when printing a hierarchic list. See description of the
        /// <see cref="IdColumn"/> property for more details.
        /// </remarks>
        [Category ("Hierarchy")]
        [Editor ("AM.Reporting.TypeEditors.DataColumnEditor, AM.Reporting", typeof (UITypeEditor))]
        public string ParentIdColumn { get; set; }

        /// <summary>
        /// Gets or sets the indent that will be used to shift the databand according to its hierarchy level.
        /// </summary>
        /// <remarks>
        /// This property is used when printing a hierarchic list. See description of the
        /// <see cref="IdColumn"/> property for more details.
        /// </remarks>
        [DefaultValue (37.8f)]
        [Category ("Hierarchy")]
        [TypeConverter ("AM.Reporting.TypeConverters.UnitsConverter, AM.Reporting")]
        public float Indent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that the databand should collect child data rows.
        /// </summary>
        /// <remarks>
        /// This property determines how the master-detail report is printed. Default behavior is:
        /// <para/>MasterData row1
        /// <para/>-- DetailData row1
        /// <para/>-- DetailData row2
        /// <para/>-- DetailData row3
        /// <para/>MasterData row2
        /// <para/>-- DetailData row1
        /// <para/>-- DetailData row2
        /// <para/>When you set this property to <b>true</b>, the master databand will collect all child data rows
        /// under a single master data row:
        /// <para/>MasterData row1
        /// <para/>-- DetailData row1
        /// <para/>-- DetailData row2
        /// <para/>-- DetailData row3
        /// <para/>-- DetailData row4
        /// <para/>-- DetailData row5
        /// </remarks>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool CollectChildRows { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether to reset the page numbers when this band starts print.
        /// </summary>
        /// <remarks>
        /// Typically you should set the <see cref="BandBase.StartNewPage"/> property to <b>true</b> as well.
        /// </remarks>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool ResetPageNumber { get; set; }

        internal bool IsDeepmostDataBand => Bands.Count == 0;

        internal bool KeepSummary { get; set; }

        internal bool IsHierarchical => !string.IsNullOrEmpty (IdColumn) && !string.IsNullOrEmpty (ParentIdColumn);

        internal bool IsDatasourceEmpty => DataSource == null || DataSource.RowCount == 0;

        #endregion

        #region Private Methods

        private void DataSource_Disposed (object sender, EventArgs e)
        {
            dataSource = null;
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override void DeserializeSubItems (FRReader reader)
        {
            if (string.Compare (reader.ItemName, "Sort", true) == 0)
            {
                reader.Read (Sort);
            }
            else
            {
                base.DeserializeSubItems (reader);
            }
        }

        #endregion

        #region IParent

        /// <inheritdoc/>
        public override void GetChildObjects (ObjectCollection list)
        {
            base.GetChildObjects (list);
            if (IsRunning)
            {
                return;
            }

            list.Add (header);
            foreach (BandBase band in Bands)
            {
                list.Add (band);
            }

            list.Add (footer);
        }

        /// <inheritdoc/>
        public override bool CanContain (Base child)
        {
            return base.CanContain (child) || (child is DataHeaderBand || child is DataFooterBand ||
                                               child is DataBand || child is GroupHeaderBand);
        }

        /// <inheritdoc/>
        public override void AddChild (Base child)
        {
            if (IsRunning)
            {
                base.AddChild (child);
                return;
            }

            if (child is DataHeaderBand band)
            {
                Header = band;
            }
            else if (child is DataFooterBand footerBand)
            {
                Footer = footerBand;
            }
            else if (child is DataBand || child is GroupHeaderBand)
            {
                Bands.Add (child as BandBase);
            }
            else
            {
                base.AddChild (child);
            }
        }

        /// <inheritdoc/>
        public override void RemoveChild (Base child)
        {
            base.RemoveChild (child);
            if (IsRunning)
            {
                return;
            }

            if (child is DataHeaderBand band && header == band)
            {
                Header = null;
            }

            if (child is DataFooterBand footerBand && footer == footerBand)
            {
                Footer = null;
            }

            if (child is DataBand || child is GroupHeaderBand)
            {
                Bands.Remove (child as BandBase);
            }
        }

        /// <inheritdoc/>
        public override int GetChildOrder (Base child)
        {
            if (child is BandBase @base && !IsRunning)
            {
                return Bands.IndexOf (@base);
            }

            return base.GetChildOrder (child);
        }

        /// <inheritdoc/>
        public override void SetChildOrder (Base child, int order)
        {
            if (child is BandBase @base && !IsRunning)
            {
                if (order > Bands.Count)
                {
                    order = Bands.Count;
                }

                var oldOrder = @base.ZOrder;
                if (oldOrder != -1 && order != -1 && oldOrder != order)
                {
                    if (oldOrder <= order)
                    {
                        order--;
                    }

                    Bands.Remove (@base);
                    Bands.Insert (order, @base);
                }
            }
            else
            {
                base.SetChildOrder (child, order);
            }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);

            var src = source as DataBand;
            DataSource = src.DataSource;
            RowCount = src.RowCount;
            MaxRows = src.MaxRows;
            Relation = src.Relation;
            Sort.Assign (src.Sort);
            Filter = src.Filter;
            Columns.Assign (src.Columns);
            PrintIfDetailEmpty = src.PrintIfDetailEmpty;
            PrintIfDatasourceEmpty = src.PrintIfDatasourceEmpty;
            KeepTogether = src.KeepTogether;
            KeepDetail = src.KeepDetail;
            IdColumn = src.IdColumn;
            ParentIdColumn = src.ParentIdColumn;
            Indent = src.Indent;
            CollectChildRows = src.CollectChildRows;
            ResetPageNumber = src.ResetPageNumber;
        }

        internal override void UpdateWidth()
        {
            if (Columns.Count > 1)
            {
                Width = Columns.ActualWidth;
            }
            else if (!string.IsNullOrEmpty (IdColumn) && !string.IsNullOrEmpty (ParentIdColumn))
            {
                if (PageWidth != 0)
                {
                    Width = PageWidth - Left;
                }
            }
            else
            {
                base.UpdateWidth();
            }
        }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            var c = writer.DiffObject as DataBand;
            base.Serialize (writer);
            if (writer.SerializeTo == SerializeTo.Preview)
            {
                return;
            }

            if (DataSource != c.DataSource)
            {
                writer.WriteRef ("DataSource", DataSource);
            }

            if (RowCount != c.RowCount)
            {
                writer.WriteInt ("RowCount", RowCount);
            }

            if (MaxRows != c.MaxRows)
            {
                writer.WriteInt ("MaxRows", MaxRows);
            }

            if (Relation != c.Relation)
            {
                writer.WriteRef ("Relation", Relation);
            }

            if (Sort.Count > 0)
            {
                writer.Write (Sort);
            }

            if (Filter != c.Filter)
            {
                writer.WriteStr ("Filter", Filter);
            }

            Columns.Serialize (writer, c.Columns);
            if (PrintIfDetailEmpty != c.PrintIfDetailEmpty)
            {
                writer.WriteBool ("PrintIfDetailEmpty", PrintIfDetailEmpty);
            }

            if (PrintIfDatasourceEmpty != c.PrintIfDatasourceEmpty)
            {
                writer.WriteBool ("PrintIfDatasourceEmpty", PrintIfDatasourceEmpty);
            }

            if (KeepTogether != c.KeepTogether)
            {
                writer.WriteBool ("KeepTogether", KeepTogether);
            }

            if (KeepDetail != c.KeepDetail)
            {
                writer.WriteBool ("KeepDetail", KeepDetail);
            }

            if (IdColumn != c.IdColumn)
            {
                writer.WriteStr ("IdColumn", IdColumn);
            }

            if (ParentIdColumn != c.ParentIdColumn)
            {
                writer.WriteStr ("ParentIdColumn", ParentIdColumn);
            }

            if (FloatDiff (Indent, c.Indent))
            {
                writer.WriteFloat ("Indent", Indent);
            }

            if (CollectChildRows != c.CollectChildRows)
            {
                writer.WriteBool ("CollectChildRows", CollectChildRows);
            }

            if (ResetPageNumber != c.ResetPageNumber)
            {
                writer.WriteBool ("ResetPageNumber", ResetPageNumber);
            }
        }

        /// <inheritdoc/>
        public override string[] GetExpressions()
        {
            List<string> list = new List<string>();
            foreach (Sort sort in Sort)
            {
                list.Add (sort.Expression);
            }

            list.Add (Filter);
            return list.ToArray();
        }

        /// <summary>
        /// Initializes the data source connected to this band.
        /// </summary>
        public void InitDataSource()
        {
            if (DataSource == null)
            {
                DataSource = new VirtualDataSource();
                DataSource.SetReport (Report);
            }

            if (DataSource is VirtualDataSource)
            {
                (DataSource as VirtualDataSource).VirtualRowsCount = RowCount;
            }

            var parentDataSource = ParentDataBand == null ? null : ParentDataBand.DataSource;
            var collectChildRows = ParentDataBand == null ? false : ParentDataBand.CollectChildRows;
            if (Relation != null)
            {
                DataSource.Init (Relation, Filter, Sort, collectChildRows);
            }
            else
            {
                DataSource.Init (parentDataSource, Filter, Sort, collectChildRows);
            }
        }

        internal bool IsDetailEmpty()
        {
            if (PrintIfDetailEmpty || Bands.Count == 0)
            {
                return false;
            }

            foreach (BandBase band in Bands)
            {
                if (!band.IsEmpty())
                {
                    return false;
                }
            }

            return true;
        }

        internal override bool IsEmpty()
        {
            InitDataSource();
            if (IsDatasourceEmpty)
            {
                return !PrintIfDatasourceEmpty;
            }

            DataSource.First();
            while (DataSource.HasMoreRows)
            {
                if (!IsDetailEmpty())
                {
                    return false;
                }

                DataSource.Next();
            }

            return true;
        }

        /// <inheritdoc/>
        public override void InitializeComponent()
        {
            base.InitializeComponent();
            KeepSummary = false;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DataBand"/> class.
        /// </summary>
        public DataBand()
        {
            Bands = new BandCollection (this);
            Sort = new SortCollection();
            Filter = "";
            Columns = new BandColumns (this);
            IdColumn = "";
            ParentIdColumn = "";
            Indent = 37.8f;
            RowCount = 1;
            SetFlags (Flags.HasSmartTag, true);
        }
    }
}
