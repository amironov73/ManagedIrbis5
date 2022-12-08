// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* NullLocking.cs -- игнорирование блокировок базы данных
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Direct;

/// <summary>
/// Игнорирование блокировок базы данных.
/// </summary>
public sealed class NullLocking
    : IDirectLockingStrategy
{
    #region IDirectLockingStrategy members

    /// <inheritdoc cref="IDirectLockingStrategy.IsDatabaseLocked"/>
    public bool IsDatabaseLocked (DirectProvider provider, string databaseName) => false;

    /// <inheritdoc cref="IDirectLockingStrategy.LockDatabase"/>
    public bool LockDatabase (DirectProvider provider, string databaseName) => true;

    /// <inheritdoc cref="IDirectLockingStrategy.UnlockDatabase"/>
    public bool UnlockDatabase (DirectProvider provider, string databaseName) => true;

    #endregion
}
