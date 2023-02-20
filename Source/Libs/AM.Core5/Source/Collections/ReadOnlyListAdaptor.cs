// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* ReadOnlyListAdaptor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Most IList interface-implementing classes implement the IReadOnlyList interface.
/// This is for the rare class that does not implement the IList interface.
/// </summary>
internal readonly struct ReadOnlyListAdaptor<T>
    : IReadOnlyList<T>
{
    readonly IList<T> _list;

    public ReadOnlyListAdaptor (IList<T> list) => _list = list;

    public T this [int index] => _list[index];

    public int Count => _list.Count;

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
