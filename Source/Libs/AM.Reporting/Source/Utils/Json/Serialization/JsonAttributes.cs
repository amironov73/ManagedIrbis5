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

#endregion

#nullable enable

namespace AM.Reporting.Utils.Json.Serialization
{
    [AttributeUsage (AttributeTargets.Property | AttributeTargets.Enum,
        AllowMultiple = false)]
    public class JsonPropertyAttribute : Attribute
    {
        public string PropertyName { get; }

        public bool IgnoreNullValue { get; }

        public JsonPropertyAttribute (string propertyName, bool ignoreNullValue = true)
        {
            PropertyName = propertyName;
            IgnoreNullValue = ignoreNullValue;
        }
    }

    [AttributeUsage (AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class JsonIgnoreAttribute : Attribute
    {
    }
}
