// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* InMemoryPerformanceCollector.cs -- реализация сборщика сведений о производительности в оперативной памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Performance
{
    /// <summary>
    /// Реализация сборщика сведений о произволительности в оперативной памяти.
    /// </summary>
    public sealed class InMemoryPerformanceCollector
        : IPerformanceCollector
    {
        #region Events

        /// <summary>
        /// Событие, возникающее при добавлении в сборщик записи.
        /// </summary>
        public event EventHandler? RecordCollected;

        #endregion

        #region Properties

        /// <summary>
        /// Предельное количество.
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Коллекция собранных записей.
        /// </summary>
        public IReadOnlyCollection<PerfRecord> List => _list;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public InMemoryPerformanceCollector() : this(0) {}

        /// <summary>
        /// Конструктор.
        /// </summary>
        public InMemoryPerformanceCollector
            (
                int limit
            )
        {
            _list = new ConcurrentQueue<PerfRecord>();
            Limit = limit;

        } // constructor

        #endregion

        #region Private members

        private readonly ConcurrentQueue<PerfRecord> _list;

        #endregion

        #region IPerformanceCollector members

        /// <inheritdoc cref="IPerformanceCollector.Collect"/>
        public void Collect
            (
                PerfRecord record
            )
        {
            if (Limit > 0)
            {
                while (_list.Count >= Limit)
                {
                    if (!_list.TryDequeue(out _))
                    {
                        break;
                    }
                }
            }

            _list.Enqueue (record);
            RecordCollected?.Invoke (this, EventArgs.Empty);

        } // method Collect

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose() {}

        #endregion

    } // class InMemoryPerformanceCollector

} // namespace ManagedIrbis.Performance
