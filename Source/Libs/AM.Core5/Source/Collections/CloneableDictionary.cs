// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CloneableDictionary.cs -- словарь, состоящий из клонируемых элементов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Словарь, состоящий из клонируемых элементов.
/// </summary>
[DebuggerDisplay ("Count={" + nameof (Count) + "}")]
public class CloneableDictionary<TKey, TValue>
    : Dictionary<TKey, TValue>,
        ICloneable
    where TKey : notnull
{
    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public object Clone()
    {
        var result = new CloneableDictionary<TKey, TValue>();

        var keyType = typeof (TKey);
        var valueType = typeof (TValue);
        var cloneKeys = false;
        var cloneValues = false;

        if (!keyType.IsValueType)
        {
            if (keyType.IsAssignableFrom (typeof (ICloneable)))
            {
                Magna.Logger.LogError
                    (
                        nameof (CloneableDictionary<TKey, TValue>) + "::" + nameof (Clone)
                        + ": type {Type} is not cloneable",
                        keyType.FullName
                    );

                throw new ArgumentException (keyType.Name);
            }

            cloneKeys = true;
        }

        if (!valueType.IsValueType)
        {
            if (valueType.IsAssignableFrom (typeof (ICloneable)))
            {
                Magna.Logger.LogError
                    (
                        nameof (CloneableDictionary<TKey, TValue>) + "::" + nameof (Clone)
                        + "type {Type} is not cloneable",
                        valueType.FullName
                    );

                throw new ArgumentException (valueType.Name);
            }

            cloneValues = true;
        }

        foreach (var pair in this)
        {
            var keyCopy = pair.Key;
            var valueCopy = pair.Value;
            if (cloneKeys)
            {
                keyCopy = (TKey)((ICloneable)keyCopy).Clone();
            }

            if (cloneValues
                && !ReferenceEquals (valueCopy, null))
            {
                valueCopy = (TValue)((ICloneable)valueCopy).Clone();
            }

            result.Add (keyCopy, valueCopy);
        }

        return result;
    }

    #endregion
}
