// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* IDirectAccess64Strategy.cs -- интерфейс стратегии прямого доступа к базам данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Интерфейс стратегии прямого доступа к базам данных.
    /// </summary>
    public interface IDirectAccess64Strategy
        : IDisposable
    {
        /// <summary>
        /// Создание акцессора.
        /// </summary>
        DirectAccessProxy64 CreateAccessor(DirectProvider provider, string? databaseName, IServiceProvider? serviceProvider);

        /// <summary>
        /// Временное освобождение акцессора.
        /// </summary>
        void ReleaseAccessor(DirectProvider? provider, DirectAccess64 accessor);

    } // interface IDirectAccess64Strategy

} // namespace ManagedIrbis.Direct
