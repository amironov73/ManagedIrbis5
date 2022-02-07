// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* JetPool.cs -- пул поверх стека
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Memory;

/// <summary>
/// Пул поверх стека
/// </summary>
public class JetPool<T>
    where T : class, new()
{
    #region Private members

    private readonly JetStack<T> _freeObjectsQueue = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Выделение объекта из пула.
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public T Get()
    {
        return _freeObjectsQueue.Count > 0 ? _freeObjectsQueue.Pop() : new T();
    }

    /// <summary>
    /// Возврат объекта в стек.
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Return (T instance)
    {
        _freeObjectsQueue.Push (instance);
    }

    #endregion
}
