// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MemoryOwner.cs -- простая обертка для освобождения занятой памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;

#endregion

#nullable enable

namespace AM.Memory;

/// <summary>
/// Простая обертка для гарантированного освобождения занятой памяти.
/// </summary>
public class MemoryOwner<T>
    : IMemoryOwner<T>
{
    #region Properties

    /// <summary>
    /// Обернутый регион памяти.
    /// </summary>
    public Memory<T> Memory { get; set; }

    /// <summary>
    /// Заглушка.
    /// </summary>
    public static MemoryOwner<T> Empty = new (Memory<T>.Empty);

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected MemoryOwner
        (
            Memory<T> memory
        )
    {
        Memory = memory;
    }

    #endregion

    #region IDisposable memberes

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Memory = Memory<T>.Empty;
    }

    #endregion
}
