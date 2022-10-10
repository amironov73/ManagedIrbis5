// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StaticMemberInGenericType

/* TypeHelper.cs -- полезные методы для возни с типами
 *  Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Reflection;

/// <summary>
/// Полезные методы для возни с типами.
/// </summary>
public static class TypeHelper<T>
{
    #region Private members

    private static Type GetUnderlyingType()
    {
        var type = typeof(T);
        if (IsNullable (type))
        {
            type = type.GetGenericArguments()[0];
        }

        return type.IsEnum
            ? type.GetEnumUnderlyingType()
            : type;
    }

    private static bool IsNullable
        (
            Type type
        )
    {
        return type.IsGenericType
               && !type.IsGenericTypeDefinition
               && type.GetGenericTypeDefinition() == typeof (Nullable<>);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Однократное вычисление кода типа данных.
    /// </summary>
    public static readonly TypeCode TypeCode = Type.GetTypeCode (GetUnderlyingType());

    #endregion
}
