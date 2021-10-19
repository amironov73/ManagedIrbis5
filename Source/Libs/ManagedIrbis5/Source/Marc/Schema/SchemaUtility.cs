// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SchemaUtility.cs -- полезные методы для работы со схемой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Xml;
using System.Xml.Linq;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Marc.Schema
{
    /// <summary>
    /// Полезные методы для работы со схемой.
    /// </summary>
    public static class SchemaUtility
    {
        #region Public methods

        /// <summary>
        /// Get attribute boolean value for element.
        /// </summary>
        public static bool GetAttributeBoolean
            (
                this XElement element,
                string attributeName
            )
        {
            var value = element
                .Attribute (attributeName)
                .ThrowIfNull()
                .Value;

            return value.SameString ("y");
        }

        /// <summary>
        /// Get attribute boolean value for element.
        /// </summary>
        public static bool GetAttributeBoolean
            (
                this XElement element,
                string attributeName,
                bool defaultValue
            )
        {
            var attribute = element.Attribute (attributeName);
            if (ReferenceEquals (attribute, null))
            {
                return defaultValue;
            }

            return attribute.Value.SameString ("y");
        }

        /// <summary>
        /// Get first char of attribute value.
        /// </summary>
        /// <returns></returns>
        public static char GetAttributeCharacter
            (
                this XElement element,
                string attributeName
            )
        {
            var attribute = element.Attribute (attributeName);
            if (ReferenceEquals (attribute, null))
            {
                return '\0';
            }

            var value = attribute.Value;

            return string.IsNullOrEmpty (value)
                ? '\0'
                : value[0];
        }

        /// <summary>
        /// Get integer value for given attribute.
        /// </summary>
        public static int GetAttributeInt32
            (
                this XElement element,
                string attributeName,
                int defaultValue
            )
        {
            var attribute = element.Attribute (attributeName);
            if (ReferenceEquals (attribute, null))
            {
                return defaultValue;
            }

            if (!int.TryParse (attribute.Value, out var result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Get attribute text for element.
        /// </summary>
        public static string? GetAttributeText (this XElement element, string attributeName) =>
            element.Attribute (attributeName).ThrowIfNull().Value;

        /// <summary>
        /// Get attribute text for element.
        /// </summary>
        public static string? GetAttributeText
            (
                this XElement element,
                string attributeName,
                string? defaultValue
            )
            => element.Attribute (attributeName)?.Value ?? defaultValue;

        /// <summary>
        /// Get attribute text for element.
        /// </summary>
        public static string? GetElementText
            (
                this XElement element,
                string elementName,
                string? defaultValue
            )
            => element.Element (elementName)?.Value ?? defaultValue;

        /// <summary>
        /// Get inner XML for the element.
        /// </summary>
        public static string? GetInnerXml
            (
                this XElement element
            )
        {
            var reader = element.CreateReader();
            reader.MoveToContent();

            return reader.ReadInnerXml();

        } // method GetInnerXml

        /// <summary>
        /// Get inner XML for child element.
        /// </summary>
        public static string? GetInnerXml
            (
                this XElement element,
                string elementName
            )
        {
            var subElement = element.Element (elementName);
            if (ReferenceEquals (subElement, null))
            {
                return null;
            }

            var reader = subElement.CreateReader();
            reader.MoveToContent();

            return reader.ReadInnerXml();
        }

        /// <summary>
        ///
        /// </summary>
        public static bool ToBoolean
            (
                this XAttribute attribute
            )
        {
            var value = attribute.Value;

            return value.SameString ("y");
        }

        /// <summary>
        /// Преобразование схемы в JSON.
        /// </summary>
        public static string ToJson
            (
                this MarcSchema schema
            )
        {
            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //string result = JsonConvert.SerializeObject
            //    (
            //        schema,
            //        typeof (MarcSchema),
            //        Formatting.Indented,
            //        settings
            //    );

            // string result = JObject.FromObject(schema).ToString();

            // return result;

            throw new NotImplementedException();

        } // method ToJson

        #endregion

    } // class SchemaUtility

} // namespace ManagedIrbis.Marc.Schema
