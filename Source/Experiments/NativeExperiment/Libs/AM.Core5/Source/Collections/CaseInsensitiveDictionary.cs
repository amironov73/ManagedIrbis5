// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CaseInsensitiveDictionary.cs -- словарь, нечувствительный к регистру символов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Словарь, нечувствительный к регистру символов.
/// </summary>
public class CaseInsensitiveDictionary<T>
    : Dictionary<string, T>
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public CaseInsensitiveDictionary()
        : base (_GetComparer())
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CaseInsensitiveDictionary
        (
            int capacity
        )
        : base (capacity, _GetComparer())
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CaseInsensitiveDictionary
        (
            IDictionary<string, T> dictionary
        )
        : base (dictionary, _GetComparer())
    {
    }

    #endregion

    #region Private members

    /// <summary>
    /// Получение компарера для словаря.
    /// </summary>
    private static IEqualityComparer<string> _GetComparer()
    {
        return StringComparer.OrdinalIgnoreCase;
    }

    #endregion
}
