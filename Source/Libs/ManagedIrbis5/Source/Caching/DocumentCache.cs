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

/* DocumentCache.cs -- простейший текстовый кэш для ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace ManagedIrbis.Caching
{
    /// <summary>
    /// Простейший текстовый кэк для ИРБИС
    /// </summary>
    public class DocumentCache
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
        public DocumentCache (ISyncProvider provider)
            : this (provider, new MemoryCacheOptions())
        {
            _options = new MemoryCacheOptions();
        }

        /// <summary>
        /// Конструктор с опциями кэширования.
        /// </summary>
        public DocumentCache (ISyncProvider provider, MemoryCacheOptions options)
            : this (provider, new MemoryCache (options))
        {
            _options = options;
        }

        /// <summary>
        /// Конструктор с внешним кэш-провайдером.
        /// </summary>
        public DocumentCache
            (
                ISyncProvider provider,
                IMemoryCache cache
            )
        {
            Provider = provider;
            _cache = cache;
            _options = new MemoryCacheOptions();
        }

        #endregion

        #region Private members

        private readonly MemoryCacheOptions _options;
        private IMemoryCache _cache;

        /// <summary>
        /// Получение ключа для указанной спецификации.
        /// </summary>
        protected static string GetKey (FileSpecification specification)
        {
            return specification.ToString().ToUpperInvariant();
        }

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
        /// Получение документа из кэша.
        /// Если локальная копия отсутствует,
        /// она запрашивается с сервера.
        /// </summary>
        public string? GetDocument
            (
                FileSpecification specification
            )
        {
            var key = GetKey (specification);
            if (!_cache.TryGetValue (key, out string? result))
            {
                result = Provider.ReadTextFile (specification);
                if (!string.IsNullOrEmpty (result))
                {
                    _cache.Set (key, result);
                }
            }

            return result;
        }

        /// <summary>
        /// Обновление документа на сервере
        /// и заодно в кэше.
        /// </summary>
        public void UpdateDocument
            (
                FileSpecification specification,
                string documentText
            )
        {
            var withContent = specification.Clone();
            withContent.Content = documentText;
            Provider.WriteTextFile (withContent);
            var key = GetKey (specification);

            _cache.Set (key, documentText);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            _cache.Dispose();
        }

        #endregion
    }
}
