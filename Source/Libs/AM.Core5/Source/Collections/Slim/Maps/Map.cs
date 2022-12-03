// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Map.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections.Slim;

internal abstract partial class Map<TKey, TValue>
    where TKey: notnull
{
    private static readonly EqualityComparer<TKey> Comparer = EqualityComparer<TKey>.Default;

    public static Map<TKey, TValue> Empty { get; } = new EmptyMap();

    public abstract int Count { get; }

    public abstract Map<TKey, TValue> Set (TKey key, TValue value);

    public abstract Map<TKey, TValue> TryRemove (TKey key, out bool success);

    public abstract bool TryGetValue (TKey key, out TValue value);

    public virtual DictionarySlim<TKey, TValue>.Enumerator GetEnumerator() =>
        new DictionarySlim<TKey, TValue>.Enumerator (this);

    public abstract bool TryGetNext (ref int index, out KeyValuePair<TKey, TValue> value);

    public abstract ICollection<TKey> Keys { get; }

    public abstract ICollection<TValue> Values { get; }
}
