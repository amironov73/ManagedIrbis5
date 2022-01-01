// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* ScopedLockFactory.cs -- фабрика ограниченных блокировок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading;

#endregion

#nullable enable

namespace AM.Threading;

/// <summary>
/// Фабрика ограниченных блокировок.
/// Создает экземпляры <see cref="ScopedLock"/>.
/// </summary>
/// <example>
/// <code>
/// using var factory = new ScopedLockFactory();
/// ...
/// using (var theLock = factory.CreateLock())
/// {
///    DoSomething();
/// }
/// </code>
/// </example>
public sealed class ScopedLockFactory
    : IDisposable
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ScopedLockFactory()
    {
        _semaphore = new SemaphoreSlim (1, 1);
    }

    #endregion

    #region Private members

    private readonly SemaphoreSlim _semaphore;

    #endregion

    #region Public methods

    /// <summary>
    /// Создает экземпляр блокировки.
    /// </summary>
    public ScopedLock CreateLock()
    {
        return new (_semaphore);
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        _semaphore.Dispose();
    }

    #endregion
}
