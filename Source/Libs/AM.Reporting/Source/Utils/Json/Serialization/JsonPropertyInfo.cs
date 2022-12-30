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
using System.Collections;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Reporting.Utils.Json.Serialization
{
    internal class JsonPropertyInfo
    {
        public PropertyInfo Info { get; }

        public string Name { get; }

        public bool IgnoreNullValue { get; }

        public bool IsPrimitive =>
            Info.PropertyType.IsPrimitive;

        public bool IsDateTime =>
            Info.PropertyType == typeof(DateTime);

        public bool IsCollection
        {
            get
            {
                var type = Info.PropertyType;
                var result = type.IsArray || typeof(IEnumerable).IsAssignableFrom(type);
                return result;
            }
        }

        public bool IsEnum => Info.PropertyType.IsEnum;

        internal static JsonPropertyInfo Parse(PropertyInfo propertyInfo)
        {
            var attr = Attribute.GetCustomAttribute(propertyInfo, typeof(JsonPropertyAttribute)) as JsonPropertyAttribute;
            string propName = attr?.PropertyName ?? propertyInfo.Name;
            bool ignoreNull = attr?.IgnoreNullValue ?? true;
            var propInfo = new JsonPropertyInfo(propertyInfo, propName, ignoreNull);
            return propInfo;
        }

        public JsonPropertyInfo(PropertyInfo propertyInfo,
            string propertyName,
            bool ignoreNullValue = true)
        {
            Info = propertyInfo;
            Name = propertyName;
            IgnoreNullValue = ignoreNullValue;
        }
    }
}
