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
using System.Data;
using System.ComponentModel;
using System.Collections;

using AM.Reporting.Utils;

using System.Drawing.Design;

#endregion

#nullable enable

namespace AM.Reporting.Data
{
    /// <summary>
    /// Base class for all datasources such as <see cref="TableDataSource"/>.
    /// </summary>
    [TypeConverter (typeof (TypeConverters.DataSourceConverter))]
    [Editor ("AM.Reporting.TypeEditors.DataSourceEditor, AM.Reporting", typeof (UITypeEditor))]
    public abstract class DataSourceBase : Column
    {
        #region Fields

        private int currentRowNo;
        protected object currentRow;
        private Hashtable columnIndices;
        private Hashtable rowIndices;
        private Hashtable relation_SortedChildRows;
        private static bool FShowAccessDataMessage;

        #endregion

        #region Properties

        /// <summary>
        /// Occurs when the AM.Reporting engine loads data source with data.
        /// </summary>
        /// <remarks>
        /// Use this event if you want to implement load-on-demand. Event handler must load the data
        /// into the data object which this datasource is bound to (for example, the
        /// <b>TableDataSource</b> uses data from the <b>DataTable</b> object bound to
        /// the <b>Table</b> property).
        /// </remarks>
        public event EventHandler Load;

        /// <summary>
        /// Gets or sets alias of this object.
        /// </summary>
        /// <remarks>
        /// Alias is a human-friendly name of this object. It may contain any symbols (including
        /// spaces and national symbols).
        /// </remarks>
        [Category ("Design")]
        public new string Alias
        {
            get => base.Alias;
            set
            {
                UpdateExpressions (base.Alias, value);
                base.Alias = value;
            }
        }

        /// <summary>
        /// Gets a number of data rows in this datasource.
        /// </summary>
        /// <remarks>
        /// You should initialize the datasource by the <b>Init</b> method before using this property.
        /// </remarks>
        [Browsable (false)]
        public int RowCount => Rows.Count;

        /// <summary>
        /// Gets a value indicating that datasource has more rows, that is the <see cref="CurrentRowNo"/>
        /// is less than the <see cref="RowCount"/>.
        /// </summary>
        /// <remarks>
        /// <para>You should initialize the datasource by the <b>Init</b> method before using this property.</para>
        /// <para>Usually this property is used with the following code block:</para>
        /// <code>
        /// dataSource.Init();
        /// while (dataSource.HasMoreRows)
        /// {
        ///   // do something...
        ///   dataSource.Next();
        /// }
        /// </code>
        /// </remarks>
        [Browsable (false)]
        public bool HasMoreRows => CurrentRowNo < RowCount;

        /// <summary>
        /// Gets the current data row.
        /// </summary>
        /// <remarks>
        /// <para>This property is updated when you call the <see cref="Next"/> method.</para>
        /// </remarks>
        [Browsable (false)]
        public object CurrentRow
        {
            get
            {
                // in case we trying to print a datasource column in report title, init the datasource
                if (InternalRows.Count == 0)
                {
                    Init();
                }

                return currentRow;
            }
        }

        /// <summary>
        /// Gets an index of current data row.
        /// </summary>
        /// <remarks>
        /// <para>You should initialize the datasource by the <b>Init</b> method before using this property.</para>
        /// <para>This property is updated when you call the <see cref="Next"/> method.</para>
        /// </remarks>
        [Browsable (false)]
        public int CurrentRowNo
        {
            get => currentRowNo;
            set
            {
                currentRowNo = value;
                if (value >= 0 && value < Rows.Count)
                {
                    currentRow = Rows[value];
                }
                else
                {
                    currentRow = null;
                }
            }
        }

        /// <summary>
        /// Gets data stored in a specified column.
        /// </summary>
        /// <param name="columnAlias">Alias of a column.</param>
        /// <returns>The column's value.</returns>
        /// <remarks>
        /// You should initialize the datasource by the <b>Init</b> method before using this property.
        /// </remarks>
        [Browsable (false)]
        public object this [string columnAlias] => GetValue (columnAlias);

        /// <summary>
        /// Gets data stored in a specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns>The column's value.</returns>
        /// <remarks>
        /// You should initialize the datasource by the <b>Init</b> method before using this property.
        /// </remarks>
        [Browsable (false)]
        public object this [Column column]
        {
            get
            {
                if (InternalRows.Count == 0)
                {
                    Init();
                }

                if (column.Calculated)
                {
                    return column.Value;
                }

                return GetValue (column);
            }
        }

        /// <summary>
        /// Forces loading of data for this datasource.
        /// </summary>
        /// <remarks>
        /// This property is <b>false</b> by default. Set it to <b>true</b> if you need to reload data
        /// each time when the datasource initialized. Note that this may slow down the performance.
        /// </remarks>
        [DefaultValue (false)]
        public bool ForceLoadData { get; set; }

        /// <summary>
        /// This property is not relevant to this class.
        /// </summary>
        [Browsable (false)]
        public new Type DataType
        {
            get => base.DataType;
            set => base.DataType = value;
        }

        /// <summary>
        /// This property is not relevant to this class.
        /// </summary>
        [Browsable (false)]
        public new ColumnBindableControl BindableControl
        {
            get => base.BindableControl;
            set => base.BindableControl = value;
        }

        /// <summary>
        /// This property is not relevant to this class.
        /// </summary>
        [Browsable (false)]
        public new string CustomBindableControl
        {
            get => base.CustomBindableControl;
            set => base.CustomBindableControl = value;
        }

        /// <summary>
        /// This property is not relevant to this class.
        /// </summary>
        [Browsable (false)]
        public new ColumnFormat Format
        {
            get => base.Format;
            set => base.Format = value;
        }

        /// <summary>
        /// This property is not relevant to this class.
        /// </summary>
        [Browsable (false)]
        public new string Expression
        {
            get => base.Expression;
            set => base.Expression = value;
        }

        /// <summary>
        /// This property is not relevant to this class.
        /// </summary>
        [Browsable (false)]
        public new bool Calculated
        {
            get => base.Calculated;
            set => base.Calculated = value;
        }

        /// <summary>
        /// Gets the additional filter settings.
        /// </summary>
        internal Hashtable AdditionalFilter { get; }

        internal ArrayList Rows { get; }

        internal ArrayList InternalRows { get; }

        #endregion

        #region Private Methods

        private void GetChildRows (Relation relation)
        {
            // prepare consts
            var parentRow = relation.ParentDataSource.CurrentRow;
            var columnsCount = relation.ParentColumns.Length;
            object[] parentValues = new object[columnsCount];
            Column[] childColumns = new Column[columnsCount];
            for (var i = 0; i < columnsCount; i++)
            {
                parentValues[i] = relation.ParentDataSource[relation.ParentColumns[i]];
                childColumns[i] = Columns.FindByAlias (relation.ChildColumns[i]);
            }

            if (relation_SortedChildRows == null)
            {
                relation_SortedChildRows = new Hashtable();
            }

            // sort the child table at the first run. Use relation columns to sort.
            if (relation_SortedChildRows[relation] is not SortedList<Indices, ArrayList> sortedChildRows)
            {
                sortedChildRows = new SortedList<Indices, ArrayList>();
                relation_SortedChildRows[relation] = sortedChildRows;
                foreach (var row in InternalRows)
                {
                    SetCurrentRow (row);

                    object[] values = new object[columnsCount];
                    for (var i = 0; i < columnsCount; i++)
                    {
                        values[i] = this[childColumns[i]];
                    }

                    var indices = new Indices (values);
                    ArrayList rows = null;
                    var index = sortedChildRows.IndexOfKey (indices);
                    if (index == -1)
                    {
                        rows = new ArrayList();
                        sortedChildRows.Add (indices, rows);
                    }
                    else
                    {
                        rows = sortedChildRows.Values[index];
                    }

                    rows.Add (row);
                }
            }

            var indexOfKey = sortedChildRows.IndexOfKey (new Indices (parentValues));
            if (indexOfKey != -1)
            {
                var rows = sortedChildRows.Values[indexOfKey];
                foreach (var row in rows)
                {
                    this.Rows.Add (row);
                }
            }
        }

        private void ApplyAdditionalFilter()
        {
            for (var i = 0; i < Rows.Count; i++)
            {
                CurrentRowNo = i;
                foreach (DictionaryEntry de in AdditionalFilter)
                {
                    var value = Report.GetColumnValueNullable ((string)de.Key);
                    var filter = de.Value as DataSourceFilter;

                    if (!filter.ValueMatch (value))
                    {
                        Rows.RemoveAt (i);
                        i--;
                        break;
                    }
                }
            }
        }

        private void UpdateExpressions (string oldAlias, string newAlias)
        {
            if (Report != null)
            {
                // Update expressions in components.
                foreach (Base component in Report.AllObjects)
                {
                    // Update Text in TextObject instances.
                    if (component is TextObject text)
                    {
                        var bracket = text.Brackets.Split (new char[] { ',' })[0];
                        if (string.IsNullOrEmpty (bracket))
                        {
                            bracket = "[";
                        }

                        text.Text = text.Text.Replace (bracket + oldAlias + ".", bracket + newAlias + ".");
                    }

                    // Update DataColumn in PictureObject instances.
                    else if (component is PictureObject picture)
                    {
                        picture.DataColumn = picture.DataColumn.Replace (oldAlias + ".", newAlias + ".");
                    }

                    // Update Filter and Sort in DataBand instances.
                    else if (component is DataBand data)
                    {
                        data.Filter = data.Filter.Replace ("[" + oldAlias + ".", "[" + newAlias + ".");
                        foreach (Sort sort in data.Sort)
                        {
                            sort.Expression = sort.Expression.Replace ("[" + oldAlias + ".", "[" + newAlias + ".");
                        }
                    }
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets data stored in a specified column.
        /// </summary>
        /// <param name="alias">The column alias.</param>
        /// <returns>An object that contains the data.</returns>
        protected virtual object GetValue (string alias)
        {
            if (columnIndices[alias] is not Column column)
            {
                column = Columns.FindByAlias (alias);
                columnIndices[alias] = column;
            }

            return GetValue (column);
        }

        /// <summary>
        /// Gets data stored in a specified column.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns>An object that contains the data.</returns>
        protected abstract object GetValue (Column column);

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the datasource schema.
        /// </summary>
        /// <remarks>
        /// This method is used to support the AM.Reporting.Net infrastructure. Do not call it directly.
        /// </remarks>
        public abstract void InitSchema();

        /// <summary>
        /// Loads the datasource with data.
        /// </summary>
        /// <remarks>
        /// This method is used to support the AM.Reporting.Net infrastructure. Do not call it directly.
        /// </remarks>
        /// <param name="rows">Rows to fill with data.</param>
        public abstract void LoadData (ArrayList rows);

        internal void LoadData()
        {
            LoadData (InternalRows);
        }

        internal void OnLoad()
        {
            if (Load != null)
            {
                // clear internal rows to force reload data
                InternalRows.Clear();
                Load (this, EventArgs.Empty);
            }
        }

        internal void SetCurrentRow (object row)
        {
            currentRow = row;
        }

        internal void FindParentRow (Relation relation)
        {
            InitSchema();
            LoadData();

            var columnCount = relation.ChildColumns.Length;
            object[] childValues = new object[columnCount];
            for (var i = 0; i < columnCount; i++)
            {
                childValues[i] = relation.ChildDataSource[relation.ChildColumns[i]];
            }

            object result = null;
            if (childValues[0] == null)
            {
                SetCurrentRow (null);
                return;
            }

            // improve performance for single column index
            if (columnCount == 1)
            {
                if (rowIndices.Count == 0)
                {
                    foreach (var row in InternalRows)
                    {
                        SetCurrentRow (row);
                        rowIndices[this[relation.ParentColumns[0]]] = row;
                    }
                }

                result = rowIndices[childValues[0]];
                if (result != null)
                {
                    SetCurrentRow (result);
                    return;
                }
            }

            foreach (var row in InternalRows)
            {
                SetCurrentRow (row);
                var found = true;

                for (var i = 0; i < columnCount; i++)
                {
                    if (!this[relation.ParentColumns[i]].Equals (childValues[i]))
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    result = row;
                    break;
                }
            }

            if (columnCount == 1)
            {
                rowIndices[childValues[0]] = result;
            }

            SetCurrentRow (result);
        }

        /// <summary>
        /// Initializes this datasource.
        /// </summary>
        /// <remarks>
        /// This method fills the table with data. You should always call it before using most of
        /// datasource properties.
        /// </remarks>
        public void Init()
        {
            Init ("");
        }

        /// <summary>
        /// Initializes this datasource and applies the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression.</param>
        public void Init (string filter)
        {
            Init (filter, null);
        }

        /// <summary>
        /// Initializes this datasource, applies the specified filter and sorts the rows.
        /// </summary>
        /// <param name="filter">The filter expression.</param>
        /// <param name="sort">The collection of sort descriptors.</param>
        public void Init (string filter, SortCollection sort)
        {
            DataSourceBase parentData = null;
            Init (parentData, filter, sort);
        }

        /// <summary>
        /// Initializes this datasource and filters data rows according to the master-detail relation between
        /// this datasource and <b>parentData</b>.
        /// </summary>
        /// <param name="parentData">Parent datasource.</param>
        /// <remarks>
        /// To use master-detail relation, you must define the <see cref="Relation"/> object that describes
        /// the relation, and add it to the <b>Report.Dictionary.Relations</b> collection.
        /// </remarks>
        public void Init (DataSourceBase parentData)
        {
            Init (parentData, "", null);
        }

        /// <summary>
        /// Initializes this datasource and filters data rows according to the master-detail relation between
        /// this datasource and <b>parentData</b>. Also applies the specified filter and sorts the rows.
        /// </summary>
        /// <param name="parentData">Parent datasource.</param>
        /// <param name="filter">The filter expression.</param>
        /// <param name="sort">The collection of sort descriptors.</param>
        /// <remarks>
        /// To use master-detail relation, you must define the <see cref="Relation"/> object that describes
        /// the relation, and add it to the <b>Report.Dictionary.Relations</b> collection.
        /// </remarks>
        public void Init (DataSourceBase parentData, string filter, SortCollection sort)
        {
            Init (parentData, filter, sort, false);
        }

        /// <summary>
        /// Initializes this datasource and filters data rows according to the master-detail relation.
        /// Also applies the specified filter and sorts the rows.
        /// </summary>
        /// <param name="relation">The master-detail relation.</param>
        /// <param name="filter">The filter expression.</param>
        /// <param name="sort">The collection of sort descriptors.</param>
        /// <remarks>
        /// To use master-detail relation, you must define the <see cref="Relation"/> object that describes
        /// the relation, and add it to the <b>Report.Dictionary.Relations</b> collection.
        /// </remarks>
        public void Init (Relation relation, string filter, SortCollection sort)
        {
            Init (relation, filter, sort, false);
        }

        internal void Init (DataSourceBase parentData, string filter, SortCollection sort, bool useAllParentRows)
        {
            var relation =
                parentData != null ? DataHelper.FindRelation (Report.Dictionary, parentData, this) : null;
            Init (relation, filter, sort, useAllParentRows);
        }

        internal void Init (Relation relation, string filter, SortCollection sort, bool useAllParentRows)
        {
            if (FShowAccessDataMessage)
            {
                Config.ReportSettings.OnProgress (Report, Res.Get ("Messages,AccessingData"));
            }

            // InitSchema may fail sometimes (for example, when using OracleConnection with nested select).
            try
            {
                InitSchema();
            }
            catch
            {
            }

            LoadData();

            // fill rows, emulate relation
            Rows.Clear();
            if (relation != null && relation.Enabled)
            {
                if (useAllParentRows)
                {
                    var parentData = relation.ParentDataSource;

                    // parentData must be initialized prior to calling this method!
                    parentData.First();
                    while (parentData.HasMoreRows)
                    {
                        GetChildRows (relation);
                        parentData.Next();
                    }
                }
                else
                {
                    GetChildRows (relation);
                }
            }
            else
            {
                foreach (var row in InternalRows)
                {
                    Rows.Add (row);
                }
            }

            // filter data rows
            if (FShowAccessDataMessage && Rows.Count > 10000)
            {
                Config.ReportSettings.OnProgress (Report, Res.Get ("Messages,PreparingData"));
            }

            if (filter != null && filter.Trim() != "")
            {
                for (var i = 0; i < Rows.Count; i++)
                {
                    CurrentRowNo = i;
                    var match = Report.Calc (filter);
                    if (match is bool b && !b)
                    {
                        Rows.RemoveAt (i);
                        i--;
                    }
                }
            }

            // additional filter
            if (AdditionalFilter.Count > 0)
            {
                ApplyAdditionalFilter();
            }

            // sort data rows
            if (sort != null && sort.Count > 0)
            {
                string[] expressions = new string[sort.Count];
                var descending = new bool[sort.Count];
                for (var i = 0; i < sort.Count; i++)
                {
                    expressions[i] = sort[i].Expression;
                    descending[i] = sort[i].Descending;
                }

                Rows.Sort (new RowComparer (Report, this, expressions, descending));
            }

            FShowAccessDataMessage = false;
            First();
        }

        /// <summary>
        /// Initializes the data source if it is not initialized yet.
        /// </summary>
        public void EnsureInit()
        {
            if (InternalRows.Count == 0)
            {
                Init();
            }
        }

        /// <summary>
        /// Navigates to the first row.
        /// </summary>
        /// <remarks>
        /// You should initialize the datasource by the <b>Init</b> method before using this method.
        /// </remarks>
        public void First()
        {
            CurrentRowNo = 0;
        }

        /// <summary>
        /// Navigates to the next row.
        /// </summary>
        /// <remarks>
        /// You should initialize the datasource by the <b>Init</b> method before using this method.
        /// </remarks>
        public void Next()
        {
            CurrentRowNo++;
        }

        /// <summary>
        /// Navigates to the prior row.
        /// </summary>
        /// <remarks>
        /// You should initialize the datasource by the <b>Init</b> method before using this method.
        /// </remarks>
        public void Prior()
        {
            CurrentRowNo--;
        }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            base.Serialize (writer);
            if (Enabled)
            {
                writer.WriteBool ("Enabled", Enabled);
            }

            if (ForceLoadData)
            {
                writer.WriteBool ("ForceLoadData", ForceLoadData);
            }
        }

        /// <inheritdoc/>
        public override void Deserialize (FRReader reader)
        {
            // the Clear is needed to avoid duplicate columns in the inherited report
            // when the same datasource is exists in both base and inherited report
            Clear();
            base.Deserialize (reader);
        }

        internal void ClearData()
        {
            columnIndices.Clear();
            rowIndices.Clear();
            InternalRows.Clear();
            Rows.Clear();
            AdditionalFilter.Clear();
            relation_SortedChildRows = null;
            FShowAccessDataMessage = true;
        }

        /// <inheritdoc/>
        public override void InitializeComponent()
        {
            ClearData();
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceBase"/> class with default settings.
        /// </summary>
        public DataSourceBase()
        {
            InternalRows = new ArrayList();
            Rows = new ArrayList();
            AdditionalFilter = new Hashtable();
            columnIndices = new Hashtable();
            rowIndices = new Hashtable();
            SetFlags (Flags.HasGlobalName, true);
        }

        private class RowComparer : IComparer
        {
            private Report report;
            private DataSourceBase dataSource;
            private string[] expressions;
            private bool[] descending;
            private Column[] columns;

            public int Compare (object x, object y)
            {
                var result = 0;
                for (var i = 0; i < expressions.Length; i++)
                {
                    IComparable i1;
                    IComparable i2;
                    if (columns[i] == null)
                    {
                        dataSource.SetCurrentRow (x);
                        i1 = report.Calc (expressions[i]) as IComparable;
                        dataSource.SetCurrentRow (y);
                        i2 = report.Calc (expressions[i]) as IComparable;
                    }
                    else
                    {
                        dataSource.SetCurrentRow (x);
                        i1 = columns[i].Value as IComparable;
                        dataSource.SetCurrentRow (y);
                        i2 = columns[i].Value as IComparable;
                    }

                    if (i1 != null)
                    {
                        result = i1.CompareTo (i2);
                    }
                    else if (i2 != null)
                    {
                        result = -1;
                    }

                    if (descending[i])
                    {
                        result = -result;
                    }

                    if (result != 0)
                    {
                        break;
                    }
                }

                return result;
            }

            public RowComparer (Report report, DataSourceBase dataSource, string[] expressions, bool[] descending)
            {
                this.report = report;
                this.dataSource = dataSource;
                this.expressions = expressions;
                this.descending = descending;

                // optimize performance if expression is a single data column
                columns = new Column[expressions.Length];
                for (var i = 0; i < expressions.Length; i++)
                {
                    var expr = expressions[i];
                    if (expr.StartsWith ("[") && expr.EndsWith ("]"))
                    {
                        expr = expr.Substring (1, expr.Length - 2);
                    }

                    var column = DataHelper.GetColumn (this.report.Dictionary, expr);
                    var datasource = DataHelper.GetDataSource (this.report.Dictionary, expr);
                    if (column != null && column.Parent == datasource)
                    {
                        columns[i] = column;
                    }
                    else
                    {
                        columns[i] = null;
                    }
                }
            }
        }

        private class Indices : IComparable
        {
            private object[] values;

            public int CompareTo (object obj)
            {
                var indices = obj as Indices;

                var result = 0;
                for (var i = 0; i < values.Length; i++)
                {
                    var i2 = values[i] as IComparable;

                    if (indices.values[i] is IComparable i1)
                    {
                        result = i1.CompareTo (i2);
                    }
                    else if (i2 != null)
                    {
                        result = -1;
                    }

                    if (result != 0)
                    {
                        break;
                    }
                }

                return result;
            }

            public Indices (object[] values)
            {
                this.values = values;
            }
        }
    }
}
