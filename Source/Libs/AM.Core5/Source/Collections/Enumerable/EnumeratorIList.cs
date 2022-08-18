// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* EnumaratorIList.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections.Generic;

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public struct EnumeratorIList<T>
    : IEnumerator<T>
{
    private readonly IList<T> _list;
    private int _index;

    /// <summary>
    ///
    /// </summary>
    /// <param name="list"></param>
    public EnumeratorIList(IList<T> list)
    {
        _index = -1;
        _list = list;
    }

    /// <summary>
    ///
    /// </summary>
    public T Current => _list[_index];

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public bool MoveNext()
    {
        _index++;

        return _index < (_list?.Count ?? 0);
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="IEnumerator.Current"/>
    object? IEnumerator.Current => Current;

    /// <inheritdoc cref="IEnumerator.Reset"/>
    public void Reset() => _index = -1;
}
