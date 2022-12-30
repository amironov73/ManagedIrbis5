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

#endregion

#nullable enable

namespace AM.Reporting.Utils.Json
{
    internal class JsonSchema
    {
        #region Private Fields

        #endregion Private Fields

        #region Public Properties

        public Type DataType
        {
            get
            {
                switch (Type)
                {
                    case "object": return typeof (JsonBase);
                    case "array": return typeof (JsonBase);
                    case "integer": return typeof (int);
                    case "number": return typeof (double);
                    case "string": return typeof (string);
                    case "boolean": return typeof (bool);
                    default: return typeof (object);
                }
            }
        }

        public string Description { get; set; }

        public JsonSchema Items { get; set; }

        public Dictionary<string, JsonSchema> Properties { get; private set; }

        public string Type { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static JsonSchema FromJson (object json)
        {
            var result = new JsonSchema();
            if (json is JsonObject jsonObject)
            {
                result.Type = "object";
                result.Properties = new Dictionary<string, JsonSchema>();

                foreach (var kv in jsonObject)
                {
                    result.Properties[kv.Key] = FromJson (kv.Value);
                }
            }
            else if (json is JsonArray array)
            {
                result.Type = "array";
                result.Items = null;
                foreach (var obj in array)
                {
                    var sub = FromJson (obj);
                    if (result.Items == null)
                    {
                        result.Items = sub;
                    }
                    else
                    {
                        result.Items.Union (sub);
                    }
                }

                if (result.Items == null)
                {
                    result.Items = new JsonSchema();
                }
            }
            else if (json is string)
            {
                result.Type = "string";
            }
            else if (json is double)
            {
                result.Type = "number";
            }
            else if (json is bool)
            {
                result.Type = "boolean";
            }
            else
            {
                result.Type = "null";
            }

            return result;
        }

        public static JsonSchema Load (JsonObject obj)
        {
            if (obj == null)
            {
                throw new NullReferenceException ("Unable to load schema from non-object");
            }

            var result = new JsonSchema();
            if (obj != null)
            {
                result.Type = obj.ReadString ("type");
                result.Description = obj.ReadString ("description");
                switch (result.Type)
                {
                    case "object":
                        result.Properties = new Dictionary<string, JsonSchema>();
                        if (obj.ContainsKey ("properties"))
                        {
                            if (obj["properties"] is JsonObject child)
                            {
                                foreach (var kv in child)
                                {
                                    if (kv.Value is JsonObject value)
                                    {
                                        result.Properties[kv.Key] = Load (value);
                                    }
                                    else
                                    {
                                        result.Properties[kv.Key] = new JsonSchema();
                                    }
                                }
                            }
                        }

                        break;

                    case "array":
                        if (obj.ContainsKey ("items"))
                        {
                            result.Items = Load (obj["items"] as JsonObject);
                        }
                        else
                        {
                            result.Items = new JsonSchema();
                        }

                        break;
                }
            }

            return result;
        }

        public void Save (JsonObject obj)
        {
            if (Type != null)
            {
                obj["type"] = Type;
            }

            if (Description != null)
            {
                obj["description"] = Description;
            }

            if (Items != null)
            {
                var child = new JsonObject();
                Items.Save (child);
                obj["items"] = child;
            }

            if (Properties != null && Properties.Count > 0)
            {
                var child = new JsonObject();
                obj["properties"] = child;
                foreach (KeyValuePair<string, JsonSchema> kv in Properties)
                {
                    var sub_child = new JsonObject();
                    kv.Value.Save (sub_child);
                    child[kv.Key] = sub_child;
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Union (JsonSchema sub)
        {
            if (sub == null || Type != sub.Type)
            {
                Items = null;
                Properties = null;
                Type = "null";
            }
            else if (Type == "object")
            {
                if (Properties == null)
                {
                    Properties = new Dictionary<string, JsonSchema>();
                }

                if (sub.Properties != null)
                {
                    foreach (KeyValuePair<string, JsonSchema> kv in sub.Properties)
                    {
                        if (Properties.TryGetValue (kv.Key, out var child))
                        {
                            child.Union (kv.Value);
                        }
                        else
                        {
                            Properties[kv.Key] = kv.Value;
                        }
                    }
                }
            }
            else if (Type == "array")
            {
                if (Items == null)
                {
                    Items = sub.Items;
                }
                else
                {
                    Items.Union (sub.Items);
                }
            }
        }

        #endregion Private Methods
    }
}
