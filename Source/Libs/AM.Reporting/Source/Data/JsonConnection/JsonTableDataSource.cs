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

using AM.Reporting.Utils;
using AM.Reporting.Utils.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Reporting.Data.JsonConnection
{
    /// <summary>
    /// JsonTableDataSource present a json array object
    /// </summary>
    public class JsonTableDataSource : TableDataSource
    {
        #region Private Fields

        private JsonArray _json;
        private string tableData;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets value for force update schema on init schema
        /// </summary>
        [Browsable (false)]
        public bool UpdateSchema { get; set; }

        /// <summary>
        /// Get or sets simplify mode for array types
        /// </summary>
        [Browsable (false)]
        public bool SimpleStructure { get; set; }

        /// <inheritdoc />
        [Browsable (false)]
        public override string TableData
        {
            get
            {
                if (string.IsNullOrEmpty (tableData))
                {
                    tableData = Json.ToString();
                }

                return tableData;
            }

            set => tableData = value;
        }

        #endregion Public Properties

        #region Internal Properties

        internal JsonArray Json
        {
            get
            {
                if (_json == null)
                {
                    if (StoreData && !string.IsNullOrEmpty (tableData))
                    {
                        _json = JsonBase.FromString (tableData) as JsonArray;
                    }
                    else
                    {
                        _json = GetJson (Parent, this) as JsonArray;
                    }

                    if (_json == null)
                    {
                        _json = new JsonArray();
                    }
                }

                return _json;
            }
            set => _json = value;
        }

        #endregion Internal Properties

        #region Private Properties

        private int CurrentIndex
        {
            get
            {
                if (currentRow is int row)
                {
                    return row;
                }

                if (CurrentRowNo < Rows.Count)
                {
                    var result = Rows[CurrentRowNo];
                    if (result is int i)
                    {
                        return i;
                    }
                }

                return CurrentRowNo;
            }
        }

        #endregion Private Properties

        #region Public Constructors

        /// <inheritdoc/>
        public JsonTableDataSource()
        {
            DataType = typeof (JsonArray);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <inheritdoc/>
        public override void InitializeComponent()
        {
            base.InitializeComponent();
            Json = null;
        }

        /// <inheritdoc/>
        public override void InitSchema()
        {
            if (Columns.Count == 0 || UpdateSchema && !StoreData)
            {
                if (Connection is JsonDataSourceConnection)
                {
                    var con = Connection as JsonDataSourceConnection;

                    InitSchema (this, con.JsonSchema, con.SimpleStructure);
                }
            }

            UpdateSchema = false;
        }

        /// <inheritdoc/>
        public override void LoadData (ArrayList rows)
        {
            Json = null;

            // JSON is calculated property, no problem with null
            if (rows != null && Json != null)
            {
                rows.Clear();
                var count = Json.Count;
                for (var i = 0; i < count; i++)
                {
                    rows.Add (i);
                }
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal static object GetJsonBaseByColumn (Base parentColumn, Column column)
        {
            if (parentColumn is JsonTableDataSource jsonTableDataSource)
            {
                if (jsonTableDataSource.SimpleStructure)
                {
                    if (!string.IsNullOrEmpty (column.PropName))
                    {
                        var obj = jsonTableDataSource.Json[
                            jsonTableDataSource.CurrentIndex];

                        return (obj as JsonBase)[column.PropName];
                    }
                }
                else
                {
                    switch (column.PropName)
                    {
                        case "item":
                            return jsonTableDataSource.Json[
                                jsonTableDataSource.CurrentIndex];
                    }

                    var source = column as JsonTableDataSource;
                    return source.Json;
                }
            }

            if (parentColumn is Column @base && !string.IsNullOrEmpty (column.PropName))
            {
                var json = GetJsonBaseByColumn (@base.Parent, @base);
                if (json is JsonBase jsonBase)
                {
                    return jsonBase[column.PropName];
                }
            }

            return null;
        }

        internal static object GetValueByColumn (Base parentColumn, Column column)
        {
            if (parentColumn is JsonTableDataSource source)
            {
                switch (column.PropName)
                {
                    case "index":
                        return source.CurrentIndex;

                    case "array":
                        return source.Json;
                }
            }

            var json = GetJsonBaseByColumn (parentColumn, column);

            return json;
        }

        internal static void InitSchema (Column table, JsonSchema schema, bool simpleStructure)
        {
            List<Column> saveColumns = new List<Column>();
            switch (schema.Type)
            {
                case "object":
                    foreach (KeyValuePair<string, JsonSchema> kv in schema.Properties)
                    {
                        if (kv.Value.Type == "object")
                        {
                            var c = new Column();
                            c.Name = kv.Key;
                            c.Alias = kv.Key;
                            c.PropName = kv.Key;
                            c.DataType = kv.Value.DataType;
                            c = UpdateColumn (table, c, saveColumns);
                            InitSchema (c, kv.Value, simpleStructure);
                        }
                        else if (kv.Value.Type == "array")
                        {
                            Column c = new JsonTableDataSource();
                            c.Name = kv.Key;
                            c.Alias = kv.Key;
                            c.PropName = kv.Key;
                            c.DataType = kv.Value.DataType;
                            c = UpdateColumn (table, c, saveColumns);

                            InitSchema (c, kv.Value, simpleStructure);
                        }
                        else
                        {
                            var c = new Column();
                            c.Name = kv.Key;
                            c.Alias = kv.Key;
                            c.PropName = kv.Key;
                            c.DataType = kv.Value.DataType;
                            c.SetBindableControlType (c.DataType);
                            UpdateColumn (table, c, saveColumns);
                        }
                    }

                    break;

                case "array":
                    var items = schema.Items;

                    var simpleArray = false;

                    if (table is JsonTableDataSource jsonTableDataSource)
                    {
                        simpleArray = jsonTableDataSource.SimpleStructure =
                            simpleStructure & items.Type == "object";
                    }

                    if (simpleArray)
                    {
                        // remake schema in simplify mode
                        InitSchema (table, items, simpleStructure);

                        // and return, no need to clear column data
                        // in this case this method has no control to columns
                        return;
                    }


                {
                    var c = new Column();
                    c.Name = "index";
                    c.Alias = "index";
                    c.PropName = "index";
                    c.DataType = typeof (int);
                    UpdateColumn (table, c, saveColumns);
                }


                {
                    Column c;
                    var iSchema = false;

                    if (items.Type == "object")
                    {
                        c = new Column();
                        iSchema = true;
                    }
                    else if (items.Type == "array")
                    {
                        c = new JsonTableDataSource();
                        iSchema = true;
                    }
                    else
                    {
                        c = new Column();
                    }

                    c.Name = "item";
                    c.Alias = "item";
                    c.PropName = "item";
                    c.DataType = items.DataType;
                    c = UpdateColumn (table, c, saveColumns);

                    if (iSchema)
                    {
                        InitSchema (c, items, simpleStructure);
                    }
                }

                {
                    var c = new Column();
                    c.Name = "array";
                    c.Alias = "array";
                    c.PropName = "array";
                    c.DataType = typeof (JsonBase);
                    UpdateColumn (table, c, saveColumns);
                }

                    break;
            }

            for (var i = 0; i < table.Columns.Count; i++)
            {
                if (!(table.Columns[i].Calculated || saveColumns.Contains (table.Columns[i])))
                {
                    table.Columns.RemoveAt (i);
                    i--;
                }
            }
        }

        internal object GetJson (Base parentColumn, Column column)
        {
            if (parentColumn is IJsonProviderSourceConnection connection)
            {
                return connection.GetJson (this);
            }

            if (parentColumn is JsonTableDataSource source)
            {
                if (source.SimpleStructure)
                {
                    var parentJson = source.Json[source.CurrentIndex];
                    if (parentJson is JsonBase @base && !string.IsNullOrEmpty (column.PropName))
                    {
                        return @base[column.PropName];
                    }
                }
                else
                {
                    return source.Json[source.CurrentIndex] as object;
                }
            }
            else if (parentColumn is Column @base)
            {
                var parentJson = GetJson (@base.Parent, @base);
                if (parentJson is JsonBase jsonBase && !string.IsNullOrEmpty (column.PropName))
                {
                    return jsonBase[column.PropName];
                }
            }

            return null;
        }

        #endregion Internal Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected override object GetValue (Column column)
        {
            return GetValueByColumn (column.Parent, column);
        }

        /// <inheritdoc/>
        protected override object GetValue (string alias)
        {
            // TODO TEST
            Column column = this;
            string[] colAliases = alias.Split ('.');

            foreach (var colAlias in colAliases)
            {
                column = column.Columns.FindByAlias (colAlias);
                if (column == null)
                {
                    return null;
                }
            }

            return GetValueByColumn (column.Parent, column);
        }

        #endregion Protected Methods

        #region Private Methods

        private static Column UpdateColumn (Column table, Column c, List<Column> list)
        {
            foreach (Column child in table.Columns)
            {
                if (child.PropName == c.PropName)
                {
                    child.DataType = c.DataType;
                    list.Add (child);
                    return child;
                }
            }

            table.AddChild (c);
            list.Add (c);
            return c;
        }

        #endregion Private Methods


        ///  <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            base.Serialize (writer);

            if (SimpleStructure)
            {
                writer.WriteBool ("SimpleStructure", SimpleStructure);
            }
        }
    }
}
