// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* IDirectLockingStrategy.cs -- стратегия блокирования базы данных
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Стратегия блокирования базы данных.
    /// </summary>
    public interface IDirectLockingStrategy
    {
        /// <summary>
        /// Проверка, не заблокирвоана ли база данных.
        /// </summary>
        bool IsDatabaseLocked(DirectProvider provider, string databaseName);

        /// <summary>
        /// Блокировка базы данных.
        /// </summary>
        bool LockDatabase(DirectProvider provider, string databaseName);

        /// <summary>
        /// Разблокирование базы данных.
        /// </summary>
        bool UnlockDatabase(DirectProvider provider, string databaseName);

    } // interface IDirectLockingStrategy

} // namespace ManagedIrbis.Direct
