// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;

#endregion

#nullable enable

namespace HtmlAgilityPack;

internal static class Utilities
{
    public static TValue? GetDictionaryValueOrDefault<TKey, TValue>
        (
            Dictionary<TKey, TValue> dict, TKey key,
            TValue? defaultValue = default
        )
        where TKey : class
    {
        return (dict.TryGetValue (key, out var value) ? value : defaultValue)!;
    }

    internal static object? To
        (
            this object? obj,
            Type type
        )
    {
        if (obj is not null)
        {
            var targetType = type;
            if (obj.GetType() == targetType)
            {
                return obj;
            }

            var converter = TypeDescriptor.GetConverter (obj);
            if (converter is not null)
            {
                if (converter.CanConvertTo (targetType))
                {
                    return converter.ConvertTo (obj, targetType);
                }
            }

            converter = TypeDescriptor.GetConverter (targetType);
            if (converter is not null)
            {
                if (converter.CanConvertFrom (obj.GetType()))
                {
                    return converter.ConvertFrom (obj);
                }
            }

            if (obj == DBNull.Value)
            {
                return null;
            }
        }

        return obj;
    }
}
