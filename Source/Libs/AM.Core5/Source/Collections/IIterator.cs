// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* IIterator.cs -- аналог итератора из C++
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Аналог итератора из C++.
/// </summary>
public interface IIterator<T>
    : IComparable<IIterator<T>>
    where T : unmanaged
{
    #region Properties

    /// <summary>
    /// Ссылка на значение.
    /// </summary>
    public ref T Value { get; }

    #endregion

    #region Public methods

    /// <summary>
    /// Движение итератора.
    /// </summary>
    void Advance (int delta = 1);

    #endregion
}
