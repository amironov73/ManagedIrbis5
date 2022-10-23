// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IPoolingEnumerator.cs -- интерфейс перечислителя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;

#endregion

#nullable enable

namespace AM.Memory.Collections;

/// <summary>
/// Интерфейс перечислителя.
/// </summary>
public interface IPoolingEnumerator
    : IDisposable
{
    ///<inheritdoc cref="IEnumerator.MoveNext"/>
    bool MoveNext();

    /// <inheritdoc cref="IEnumerator.Reset"/>
    void Reset();

    /// <inheritdoc cref="IEnumerator.Current"/>
    object Current { get; }
}
