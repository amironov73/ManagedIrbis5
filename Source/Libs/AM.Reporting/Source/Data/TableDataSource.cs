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
using System.Data;
using System.ComponentModel;
using System.Reflection;
using System.Collections;

using AM.Reporting.Utils;

using System.IO;
using System.Drawing.Design;

#endregion

#nullable enable

namespace AM.Reporting.Data
{
    /// <summary>
    /// Represents a datasource based on <b>DataTable</b> class.
    /// </summary>
    /// <example>This example shows how to add a new table to the existing connection:
    /// <code>
    /// Report report1;
    /// DataConnectionBase conn = report1.Dictionary.Connections.FindByName("Connection1");
    /// TableDataSource table = new TableDataSource();
    /// table.TableName = "Employees";
    /// table.Name = "Table1";
    /// conn.Tables.Add(table);
    /// </code>
    /// </example>
    public partial class TableDataSource : DataSourceBase
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the underlying <b>DataTable</b> object.
        /// </summary>
        [Browsable (false)]
        public DataTable Table { get; set; }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        [Category ("Data")]
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets SQL "select" command.
        /// </summary>
        /// <remarks>
        /// If this command contains parameters, you should specify them in the <see cref="Parameters"/>
        /// property.
        /// </remarks>
        [Category ("Data")]
        [Editor ("AM.Reporting.TypeEditors.SqlEditor, AM.Reporting", typeof (UITypeEditor))]
        public string SelectCommand { get; set; }

        /// <summary>
        /// Gets a collection of parameters used by "select" command.
        /// </summary>
        /// <remarks>
        /// You must set up this property if the SQL query that you've specified in the
        /// <see cref="SelectCommand"/> property contains parameters.
        /// <para/>You can pass a value to the SQL parameter in two ways.
        /// <para/>The right way is to define a report parameter. You can do this in the
        /// "Data" window. Once you have defined the parameter, you can use it to pass a value
        /// to the SQL parameter. To do this, set the SQL parameter's <b>Expression</b> property
        /// to the report parameter's name (so it will look like [myReportParam]).
        /// To pass a value to the report parameter from your application, use the
        /// <see cref="Report.SetParameterValue"/> method.
        /// <para/>The other way (unrecommended) is to find a datasource object and set its parameter from a code:
        /// <code>
        /// TableDataSource ds = report.GetDataSource("My DataSource Name") as TableDataSource;
        /// ds.Parameters[0].Value = 10;
        /// </code>
        /// This way is not good because you hardcode the report object's name.
        /// </remarks>
        [Category ("Data")]
        [Editor ("AM.Reporting.TypeEditors.CommandParametersEditor, AM.Reporting", typeof (UITypeEditor))]
        public CommandParameterCollection Parameters { get; set; }

        /// <summary>
        /// Gets or sets the parent <see cref="DataConnectionBase"/> object.
        /// </summary>
        [Browsable (false)]
        public DataConnectionBase Connection
        {
            get => IgnoreConnection ? null : Parent as DataConnectionBase;
            set => Parent = value;
        }

        /// <summary>
        /// Gets or sets a value that determines whether it is necessary to store table data in a report file.
        /// </summary>
        [Category ("Data")]
        [DefaultValue (false)]
        public bool StoreData { get; set; }

        /// <summary>
        /// Gets or sets the table data.
        /// </summary>
        /// <remarks>
        /// This property is for internal use only.
        /// </remarks>
        [Browsable (false)]
        public virtual string TableData
        {
            get
            {
                var result = "";
                if (Table == null && Connection != null)
                {
                    Connection.CreateTable (this);
                    Connection.FillTable (this);
                }

                if (Table != null)
                {
                    using (var tempDs = new DataSet())
                    {
                        var tempTable = Table.Copy();
                        tempDs.Tables.Add (tempTable);
                        using (var stream = new MemoryStream())
                        {
                            tempDs.WriteXml (stream, XmlWriteMode.WriteSchema);
                            result = Convert.ToBase64String (stream.ToArray());
                        }

                        tempTable.Dispose();
                    }
                }

                return result;
            }
            set
            {
                if (!string.IsNullOrEmpty (value))
                {
                    using (var stream = new MemoryStream (Convert.FromBase64String (value)))
                    using (var tempDs = new DataSet())
                    {
                        tempDs.ReadXml (stream);
                        Table = tempDs.Tables[0];
                        Reference = Table;
                        tempDs.Tables.RemoveAt (0);
                    }
                }
            }
        }

        /// <summary>
        /// If set, ignores the Connection (always returns null). Needed when we replace the
        /// existing connection-based datasource with datatable defined in an application.
        /// </summary>
        internal bool IgnoreConnection { get; set; }

        /// <summary>
        /// Gets or sets the query builder schema.
        /// </summary>
        /// <remarks>
        /// This property is for internal use only.
        /// </remarks>
        [Browsable (false)]
        public string QbSchema { get; set; }

        #endregion

        #region Private Methods

        private Column CreateColumn (DataColumn column)
        {
            var c = new Column();
            c.Name = column.ColumnName;
            c.Alias = column.Caption;
            c.DataType = column.DataType;
            c.SetBindableControlType (c.DataType);
            return c;
        }

        private void DeleteTable()
        {
            if (Connection != null)
            {
                Connection.DeleteTable (this);
            }
        }

        private void CreateColumns()
        {
            Columns.Clear();
            if (Table != null)
            {
                foreach (DataColumn column in Table.Columns)
                {
                    var c = CreateColumn (column);
                    Columns.Add (c);
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override void Dispose (bool disposing)
        {
            if (disposing)
            {
                DeleteTable();
            }

            base.Dispose (disposing);
        }

        /// <inheritdoc/>
        protected override object GetValue (Column column)
        {
            if (column == null)
            {
                return null;
            }

            if (column.Tag == null)
            {
                var index = Table.Columns.IndexOf (column.Name);
                if (index == -1)
                {
                    return null;
                }

                column.Tag = index;
            }


            return CurrentRow == null ? null : ((DataRow)CurrentRow)[(int)column.Tag];
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void InitSchema()
        {
            if (Connection != null)
            {
                if (!StoreData)
                {
                    Connection.CreateTable (this);
                    if (Table.Columns.Count == 0)
                    {
                        Connection.FillTableSchema (Table, SelectCommand, Parameters);
                    }
                }
            }
            else
            {
                Table = Reference as DataTable;
            }

            if (Columns.Count == 0)
            {
                CreateColumns();
            }

            foreach (Column column in Columns)
            {
                column.Tag = null;
            }
        }

        /// <inheritdoc/>
        public override void LoadData (IList rows)
        {
            if (Connection != null)
            {
                if (!StoreData)
                {
                    Connection.FillTable (this);
                }
            }
            else
            {
                TryToLoadData();
            }

            if (Table == null)
            {
                throw new DataTableException (Alias);
            }

            // custom load data via Load event
            OnLoad();

            var needReload = ForceLoadData || rows.Count == 0 || Parameters.Count > 0;
            if (needReload)
            {
                // fill rows
                rows.Clear();
                foreach (DataRow row in Table.Rows)
                {
                    if (row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Detached)
                    {
                        rows.Add (row);
                    }
                }
            }
        }

        /// <summary>
        /// Refresh the table schema.
        /// </summary>
        public void RefreshTable()
        {
            DeleteTable();
            InitSchema();
            RefreshColumns (true);
        }

        internal void RefreshColumns (bool enableNew)
        {
            if (Table != null)
            {
                // add new columns
                foreach (DataColumn column in Table.Columns)
                {
                    if (Columns.FindByName (column.ColumnName) == null)
                    {
                        var c = CreateColumn (column);
                        c.Enabled = enableNew;
                        Columns.Add (c);
                    }
                }

                // delete obsolete columns
                var i = 0;
                while (i < Columns.Count)
                {
                    var c = Columns[i];
                    if (!c.Calculated && !Table.Columns.Contains (c.Name))
                    {
                        c.Dispose();
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override void Serialize (ReportWriter writer)
        {
            base.Serialize (writer);
            if (!string.IsNullOrEmpty (TableName))
            {
                writer.WriteStr ("TableName", TableName);
            }

            if (!string.IsNullOrEmpty (SelectCommand))
            {
                writer.WriteStr ("SelectCommand", SelectCommand);
            }

            if (!string.IsNullOrEmpty (QbSchema))
            {
                writer.WriteStr ("QbSchema", QbSchema);
            }

            if (StoreData)
            {
                writer.WriteBool ("StoreData", true);
                writer.WriteStr ("TableData", TableData);
            }
        }

        /// <inheritdoc/>
        public override void SetParent (Base value)
        {
            base.SetParent (value);
            SetFlags (Flags.CanEdit, Connection != null);
        }

        /// <inheritdoc/>
        public override void InitializeComponent()
        {
            base.InitializeComponent();
            foreach (CommandParameter par in Parameters)
            {
                par.ResetLastValue();
            }
        }

        #endregion

        #region IParent Members

        /// <inheritdoc/>
        public override bool CanContain (Base child)
        {
            return base.CanContain (child) || child is CommandParameter;
        }

        /// <inheritdoc/>
        public override void GetChildObjects (ObjectCollection list)
        {
            base.GetChildObjects (list);
            foreach (CommandParameter p in Parameters)
            {
                list.Add (p);
            }
        }

        /// <inheritdoc/>
        public override void AddChild (Base child)
        {
            if (child is CommandParameter parameter)
            {
                Parameters.Add (parameter);
            }
            else
            {
                base.AddChild (child);
            }
        }

        /// <inheritdoc/>
        public override void RemoveChild (Base child)
        {
            if (child is CommandParameter parameter)
            {
                Parameters.Remove (parameter);
            }
            else
            {
                base.RemoveChild (child);
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TableDataSource"/> class with default settings.
        /// </summary>
        public TableDataSource()
        {
            TableName = "";
            SelectCommand = "";
            QbSchema = "";
            Parameters = new CommandParameterCollection (this);
        }
    }
}
