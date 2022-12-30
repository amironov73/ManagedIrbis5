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

using AM.Reporting.Utils.Json;

using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;

#endregion

#nullable enable

namespace AM.Reporting.Data.JsonConnection
{
    /// <summary>
    /// AM.Reporting json connection
    /// </summary>
    public partial class JsonDataSourceConnection : DataConnectionBase, IJsonProviderSourceConnection
    {
        #region Public Fields

        /// <summary>
        /// Name of json object table
        /// </summary>
        public const string TABLE_NAME = "JSON";

        #endregion Public Fields

        #region Private Fields

        private JsonArray? jsonInternal;
        private JsonSchema? jsonSchema;
        private string jsonSchemaString = "";

        #endregion Private Fields

        #region Internal Properties

        internal JsonArray Json
        {
            get
            {
                if (jsonInternal == null)
                {
                    InitConnection();
                }

                return jsonInternal;
            }
        }

        internal JsonSchema JsonSchema
        {
            get
            {
                if (jsonSchema == null)
                {
                    InitConnection();
                }

                return jsonSchema;
            }
        }

        internal bool SimpleStructure { get; private set; }

        #endregion Internal Properties

        #region Public Constructors

        /// <summary>
        /// Initialize a new instance
        /// </summary>
        public JsonDataSourceConnection()
        {
            IsSqlBased = false;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <inheritdoc/>
        public override void CreateAllTables (bool initSchema)
        {
            var found = false;
            foreach (Base b in Tables)
            {
                if (b is JsonTableDataSource source)
                {
                    source.UpdateSchema = true;
                    source.InitSchema();
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                var jsonDataSource = new JsonTableDataSource();

                var fixedTableName = TABLE_NAME;
                jsonDataSource.TableName = fixedTableName;

                if (Report != null)
                {
                    jsonDataSource.Name = Report.Dictionary.CreateUniqueName (fixedTableName);
                    jsonDataSource.Alias = Report.Dictionary.CreateUniqueAlias (jsonDataSource.Alias);
                }
                else
                {
                    jsonDataSource.Name = fixedTableName;
                }

                jsonDataSource.Parent = this;
                jsonDataSource.InitSchema();
                jsonDataSource.Enabled = true;
            }

            // init table schema
            if (initSchema)
            {
                foreach (TableDataSource table in Tables)
                {
                    table.InitSchema();
                }
            }
        }

        /// <inheritdoc/>
        public override void CreateRelations()
        {
        }

        /// <inheritdoc/>
        public override void CreateTable (TableDataSource source)
        {
            //throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void DeleteTable (TableDataSource source)
        {
            //throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void FillTableData (DataTable table, string selectCommand,
            CommandParameterCollection parameters)
        {
        }

        /// <inheritdoc/>
        public override void FillTableSchema (DataTable table, string selectCommand,
            CommandParameterCollection parameters)
        {
        }

        /// <inheritdoc/>
        public override string[] GetTableNames()
        {
            return new string[] { TABLE_NAME };
        }

        /// <inheritdoc/>
        public override string QuoteIdentifier (string value, DbConnection connection)
        {
            return value;
        }

        /// <inheritdoc/>
        public JsonBase GetJson (TableDataSource tableDataSource)
        {
            return Json;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected override DataSet CreateDataSet()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected override void SetConnectionString (string value)
        {
            jsonInternal = null;
            base.SetConnectionString (value);
        }

        #endregion Protected Methods

        #region Private Methods

        private void InitConnection()
        {
            InitConnection (false);
        }

        private void InitConnection (bool rebuildSchema)
        {
            var
                builder = new JsonDataSourceConnectionStringBuilder (ConnectionString);
            SimpleStructure = builder.SimpleStructure;
            JsonBase? obj = null;
            var jsonText = builder.Json.Trim();
            if (jsonText.Length > 0)
            {
                if (!(jsonText[0] == '{' || jsonText[0] == '['))
                {
                    //using (WebClient client = new WebClient())
                    //{
                    //    try
                    //    {
                    //        client.Encoding = Encoding.GetEncoding(builder.Encoding);
                    //    }
                    //    catch
                    //    {
                    //        client.Encoding = Encoding.UTF8;
                    //    }
                    //    jsonText = client.DownloadString(jsonText);
                    //}

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    var req = WebRequest.Create (jsonText);

                    foreach (var header in builder.Headers)
                    {
                        req.Headers.Add (header.Key, header.Value);
                    }

                    using (var response = req.GetResponse() as HttpWebResponse)
                    {
                        var encoding = Encoding.GetEncoding (response.CharacterSet);

                        using (var responseStream = response.GetResponseStream())
                        using (var reader = new System.IO.StreamReader (responseStream, encoding))
                            jsonText = reader.ReadToEnd();
                    }
                }

                obj = JsonBase.FromString (jsonText) as JsonBase;
            }

            var schema = builder.JsonSchema;

            // have to update schema
            if (schema != jsonSchemaString || jsonSchema == null || string.IsNullOrEmpty (jsonSchemaString))
            {
                JsonSchema? schemaObj = null;
                if (string.IsNullOrEmpty (schema) || rebuildSchema)
                {
                    if (obj != null)
                    {
                        schemaObj = JsonSchema.FromJson (obj);
                        var child = new JsonObject();
                        schemaObj.Save (child);
                        jsonSchemaString = child.ToString();
                    }
                }
                else
                {
                    schemaObj = JsonSchema.Load (JsonBase.FromString (schema) as JsonObject);
                    jsonSchemaString = schema;
                }

                if (schemaObj == null)
                {
                    schemaObj = new JsonSchema
                    {
                        Type = "array"
                    };
                }

                if (schemaObj.Type != "array")
                {
                    var parentSchema = new JsonSchema
                    {
                        Items = schemaObj,
                        Type = "array"
                    };
                    schemaObj = parentSchema;
                }

                jsonSchema = schemaObj;
            }

            if (obj is JsonArray array)
            {
                jsonInternal = array;
            }
            else
            {
                var result = new JsonArray();
                if (obj != null)
                {
                    result.Add (obj);
                }

                jsonInternal = result;
            }
        }

        #endregion Private Methods
    }
}
