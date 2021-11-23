// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* RecordCache.cs -- простейший кэш записей для ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Records;

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace ManagedIrbis.Caching
{
    /// <summary>
    /// Простейший кэш записей для ИРБИС.
    /// </summary>
    public sealed class RecordCache
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Провайдер (на всякий случай).
        /// </summary>
        public ISyncProvider Provider { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public RecordCache (ISyncProvider provider)
            : this (provider, new MemoryCacheOptions())
        {
            _options = new MemoryCacheOptions();
        }

        /// <summary>
        /// Конструктор с опциями кэширования.
        /// </summary>
        public RecordCache (ISyncProvider provider, MemoryCacheOptions options)
            : this (provider, new MemoryCache (options))
        {
            _options = options;
        }

        /// <summary>
        /// Конструктор с внешним кэш-провайдером.
        /// </summary>
        public RecordCache
            (
                ISyncProvider provider,
                IMemoryCache cache
            )
        {
            Provider = provider;
            _options = new MemoryCacheOptions();
            _cache = cache;
        }

        #endregion

        #region Private members

        private readonly MemoryCacheOptions _options;
        private IMemoryCache _cache;

        private static string GetKey (int mfn) => string.Format
            (
                CultureInfo.InvariantCulture,
                "_record_{0}",
                mfn
            );

        #endregion

        #region Public methods

        /// <summary>
        /// Очистка кэша.
        /// </summary>
        public void Clear()
        {
            _cache.Dispose();
            _cache = new MemoryCache (_options);
        }

        /// <summary>
        /// Получение записи из кэша.
        /// Если локальная копия отсутствует,
        /// она запрашивается с сервера.
        /// </summary>
        public T? GetRecord<T>
            (
                int mfn
            )
            where T : class, IRecord, new()
        {
            var key = GetKey (mfn);
            if (!_cache.TryGetValue (key, out T? result))
            {
                var parameters = new ReadRecordParameters()
                {
                    Database = Provider.Database,
                    Mfn = mfn
                };
                result = Provider.ReadRecord<T> (parameters);
                if (result is not null)
                {
                    _cache.Set (key, result);
                }
            }

            return result;
        }

        /// <summary>
        /// Обновление записи на сервере.
        /// </summary>
        public void UpdateRecord<T>
            (
                T record
            )
            where T : class, IRecord
        {
            var parameters = new WriteRecordParameters()
            {
                Record = record
            };
            Provider.WriteRecord (parameters);
            var key = GetKey (record.Mfn);
            _cache.Set (key, record);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            // TODO выполнить очистку
        }

        #endregion
    }
}
