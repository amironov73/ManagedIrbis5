// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* ScopedLock.cs -- экземпляр ограниченной блокировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading;

#endregion

#nullable enable

namespace AM.Threading
{
    /// <summary>
    /// Экземпляр ограниченной блокировки.
    /// См. <see cref="ScopedLockFactory"/>.
    /// </summary>
    public sealed class ScopedLock
        : IDisposable
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        internal ScopedLock
            (
                SemaphoreSlim semaphore
            )
        {
            _semaphore = semaphore;
            _semaphore.Wait();

        } // constructor

        #endregion

        #region Private members

        private readonly SemaphoreSlim _semaphore;

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose() => _semaphore.Release();

        #endregion

    } // class ScopedLock

} // namespace AM.Threading
