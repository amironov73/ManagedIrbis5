// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* CaseInsensitiveExpandoObject.cs -- нечувствительный к регистру символов динамический объект
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Dynamic;

using JetBrains.Annotations;

#endregion

namespace AM;

/// <summary>
/// Нечувствительный к регистру символов динамический объект.
/// </summary>
[PublicAPI]
public sealed class CaseInsensitiveExpandoObject
    : DynamicObject
{
    #region Properties

    /// <summary>
    /// Словарь, используемый для хранения данных.
    /// </summary>
    public Dictionary<string, object?> Dictionary { get; }
        = new (StringComparer.OrdinalIgnoreCase);

    #endregion

    #region DynamicObject members

    ///  <inheritdoc cref="DynamicObject.TrySetIndex" />
    public override bool TrySetIndex
        (
            SetIndexBinder binder,
            object[] indexes,
            object? value
        )
    {
        Sure.Equals (indexes.Length, 1);

        Dictionary[indexes[0].ToInvariantString()] = value;

        return true;
    }

    /// <inheritdoc cref="DynamicObject.TryGetIndex"/>
    public override bool TryGetIndex
        (
            GetIndexBinder binder,
            object[] indexes,
            out object? result
        )
    {
        Sure.Equals (indexes.Length, 1);

        return Dictionary.TryGetValue (indexes[0].ToInvariantString(), out result);
    }

    /// <inheritdoc cref="DynamicObject.TrySetMember" />
    public override bool TrySetMember
        (
            SetMemberBinder binder, object? value
        )
    {
        Dictionary[binder.Name] = value;

        return true;
    }

    /// <inheritdoc cref="DynamicObject.TryGetMember" />
    public override bool TryGetMember
        (
            GetMemberBinder binder,
            out object? result
        )
        => Dictionary.TryGetValue (binder.Name, out result);

    #endregion
}
