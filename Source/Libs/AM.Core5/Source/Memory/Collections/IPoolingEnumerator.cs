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

namespace AM.Memory.Collections;

public interface IPoolingEnumerator : IDisposable
{
    /// <summary>Advances the enumerator to the next element of the collection.</summary>
    /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
    /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
    bool MoveNext();

    /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
    /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
    void Reset();

    // <summary>Gets the element in the collection at the current position of the enumerator.</summary>
    /// <returns>The element in the collection at the current position of the enumerator.</returns>
    object Current { get; }
}
