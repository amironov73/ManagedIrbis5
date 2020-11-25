// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* JsonUtility.cs -- возня с JSON
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json;

#endregion

#nullable enable

namespace AM.Json
{
    /// <summary>
    /// Возня с JSON.
    /// </summary>
    public static class JsonUtility
    {
        #region Public methods

        /*
        /// <summary>
        /// Expand $type's.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public static void ExpandTypes
            (
                JObject obj,
                string nameSpace,
                string assembly
            )
        {
            Sure.NotNull(obj, nameof(obj));
            Sure.NotNullNorEmpty(nameSpace, nameof(nameSpace));
            Sure.NotNullNorEmpty(assembly, nameof(assembly));

            IEnumerable<JValue> values = obj
                .SelectTokens("$..$type")
                .OfType<JValue>();
            foreach (JValue value in values)
            {
                if (value.Value != null)
                {
                    var typeName = value.Value.ToString();
                    if (!string.IsNullOrEmpty(typeName))
                    {
                        if (!typeName.Contains('.'))
                        {
                            typeName = nameSpace
                                       + "."
                                       + typeName
                                       + ", "
                                       + assembly;
                            value.Value = typeName;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public static void Include
            (
                JObject obj,
                Action<JProperty> resolver
            )
        {
            Sure.NotNull(obj, nameof(obj));
            Sure.NotNull(resolver, nameof(resolver));

            JValue[] values = obj
                .SelectTokens("$..$include")
                .OfType<JValue>()
                .ToArray();

            foreach (JValue value in values)
            {
                if (value.Parent != null)
                {
                    JProperty property = (JProperty)value.Parent;
                    resolver(property);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public static void Include
            (
                JObject obj
            )
        {
            Sure.NotNull(obj, nameof(obj));

            JToken[] tokens = obj
                .SelectTokens("$..$include")
                .ToArray();

            foreach (JToken token in tokens)
            {
                if (token.Parent != null)
                {
                    JProperty property = (JProperty)token.Parent;
                    Resolve(property);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public static void Include
            (
                JObject obj,
                string newName
            )
        {
            Sure.NotNull(obj, nameof(obj));
            Sure.NotNullNorEmpty(newName, nameof(newName));

            void Resolver(JProperty prop)
            {
                Resolve(prop, newName);
            }

            Include(obj, Resolver);
        }

        /// <summary>
        /// Read <see cref="JArray"/> from specified
        /// local file.
        /// </summary>
        public static JArray ReadArrayFromFile
            (
                string fileName
            )
        {
            Sure.FileExists(fileName, nameof(fileName));

            string text = File.ReadAllText(fileName);
            JArray result = JArray.Parse(text);

            return result;
        }

        /// <summary>
        /// Read <see cref="JObject"/> from specified
        /// local JSON file.
        /// </summary>
        public static JObject ReadObjectFromFile
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            string text = File.ReadAllText(fileName);
            JObject result = JObject.Parse(text);

            return result;
        }

        /// <summary>
        /// Read arbitrary object from specified
        /// local JSON file.
        /// </summary>
        public static T ReadObjectFromFile<T>
            (
                string fileName
            )
        {
            Sure.FileExists(fileName, nameof(fileName));

            string text = File.ReadAllText(fileName);
            T result = JsonConvert.DeserializeObject<T>(text);

            return result;
        }

        /// <summary>
        /// Save the <see cref="JArray"/>
        /// to the specified local file.
        /// </summary>
        public static void SaveArrayToFile
            (
                JArray array,
                string fileName
            )
        {
            Sure.NotNull(array, nameof(array));
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            string text = array.ToString(Formatting.Indented);
            File.WriteAllText(fileName, text);
        }

        /// <summary>
        /// Save object to the specified local JSON file.
        /// </summary>
        public static void SaveObjectToFile
            (
                JObject obj,
                string fileName
            )
        {
            Sure.NotNull(obj, nameof(obj));
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            string text = obj.ToString(Formatting.Indented);
            File.WriteAllText(fileName, text);
        }

        /// <summary>
        /// Save object to the specified local JSON file.
        /// </summary>
        public static void SaveObjectToFile
            (
                object obj,
                string fileName
            )
        {
            JObject json = JObject.FromObject(obj);

            SaveObjectToFile(json, fileName);
        }

        /// <summary>
        /// Resolver for <see cref="Include(JObject,Action{JProperty})"/>.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public static void Resolve
            (
                JProperty property,
                string newName
            )
        {
            Sure.NotNull(property, nameof(property));
            Sure.NotNull(newName, nameof(newName));

            // TODO use path for searching

            string fileName = property.Value.ToString();
            string text = File.ReadAllText(fileName);
            JObject value = JObject.Parse(text);
            JProperty newProperty = new JProperty(newName, value);
            property.Replace(newProperty);
        }

        /// <summary>
        /// Resolver for <see cref="Include(JObject,Action{JProperty})"/>.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public static void Resolve
            (
                JProperty property
            )
        {
            Sure.NotNull(property, nameof(property));

            // TODO use path for searching

            var obj = (JObject)property.Value;
            var newName = obj["name"]?.Value<string>();
            var fileName = obj["file"]?.Value<string>();
            if (!string.IsNullOrEmpty(newName)
                && !string.IsNullOrEmpty(fileName))
            {
                var text = File.ReadAllText(fileName);
                var value = JObject.Parse(text);
                var newProperty = new JProperty(newName, value);
                property.Replace(newProperty);
            }
        }

        */

        /// <summary>
        /// Serialize to short string.
        /// </summary>
        public static string SerializeShort<T>
            (
                T obj
            )
        {
            var options = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                WriteIndented = true
            };
            var result = JsonSerializer.Serialize(obj, options);

            return result;
        }

        #endregion
    }
}
