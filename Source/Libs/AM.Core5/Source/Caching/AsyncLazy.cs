// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* AsyncLazy.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Caching;

/// <summary>
///     See https://blogs.msdn.microsoft.com/pfxteam/2011/01/15/asynclazyt/
/// </summary>
public class AsyncLazy<T>
    : Lazy<Task<T>>
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="valueFactory"></param>
    public AsyncLazy
        (
            Func<T> valueFactory
        )
        : base (() => Task.Factory.StartNew (valueFactory))
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="taskFactory"></param>
    public AsyncLazy
        (
            Func<Task<T>> taskFactory
        )
        : base (() => Task.Factory.StartNew (taskFactory).Unwrap())
    {
        // пустое тело конструктора
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public TaskAwaiter<T> GetAwaiter()
    {
        return Value.GetAwaiter();
    }

    #endregion
}
