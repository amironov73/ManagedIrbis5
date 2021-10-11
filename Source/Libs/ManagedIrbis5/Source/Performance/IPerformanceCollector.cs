// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* IPerformanceCollector.cs -- интерфейс сборщика сведений о производительности
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Performance
{
    /// <summary>
    /// Интерфейс сборщика сведений о произволительности.
    /// </summary>
    public interface IPerformanceCollector
        : IDisposable
    {
        /// <summary>
        /// Сбор одной записи о сетевой транзакции.
        /// </summary>
        void Collect (PerfRecord record);

    } // interface IPerformanceCollector

} // namespace ManagedIrbis.Performance
