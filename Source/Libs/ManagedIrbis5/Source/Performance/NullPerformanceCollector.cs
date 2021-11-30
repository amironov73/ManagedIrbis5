// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* NullPerformanceCollector.cs -- пустой сборщик для целей тестирования
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Performance
{
    /// <summary>
    /// Пустой коллектор для целей тестирования.
    /// </summary>
    public sealed class NullPerformanceCollector
        : IPerformanceCollector
    {
        #region Events

        /// <summary>
        /// Событие, возникающее при добавлении в сборщик записи.
        /// </summary>
        public event EventHandler? RecordCollected;

        #endregion

        #region IPerformanceCollector members

        /// <inheritdoc cref="IPerformanceCollector.Collect"/>
        public void Collect
            (
                PerfRecord record
            )
        {
            RecordCollected?.Invoke (this, EventArgs.Empty);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            // nothing to do here
        }

        #endregion
    }
}
