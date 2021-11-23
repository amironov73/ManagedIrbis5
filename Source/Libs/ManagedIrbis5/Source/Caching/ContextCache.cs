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

/* ContextCache.cs -- контекстный кэш для ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Records;
using ManagedIrbis.Trees;
using ManagedIrbis.Workspace;

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace ManagedIrbis.Caching
{
    /// <summary>
    /// Контекстный кэш для ИРБИС.
    /// </summary>
    public sealed class ContextCache
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
        public ContextCache (ISyncProvider provider)
            : this (provider, new MemoryCacheOptions())
        {
        }

        /// <summary>
        /// Конструктор с опциями кэширования.
        /// </summary>
        public ContextCache (ISyncProvider provider, MemoryCacheOptions options)
            : this (provider, new MemoryCache (options))
        {
            _options = options;
        }

        /// <summary>
        /// Конструктор с внешним кэш-провайдером.
        /// </summary>
        public ContextCache
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

        private static string GetKey (FileSpecification specification)
        {
            return specification.ToString().ToUpperInvariant();
        }

        private static string GetKey (int mfn)
        {
            return string.Format
                (
                    CultureInfo.InvariantCulture,
                    "_record_{0}",
                    mfn
                );
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
        /// Получение меню из кэша.
        /// Если локальная копия отсутствует,
        /// она запрашивается с сервера.
        /// </summary>
        public MenuFile? GetMenu
            (
                FileSpecification specification
            )
        {
            var document = GetDocument (specification);
            if (document is not null)
            {
                var reader = new StringReader (document);

                return MenuFile.ParseStream (reader);
            }

            return null;
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
            } // if

            return result;
        }

        /// <summary>
        /// Получение "деревянного" меню из кэша.
        /// Если локальная копия отсутствует,
        /// она запрашивается с сервера.
        /// </summary>
        public TreeFile? GetTree
            (
                FileSpecification specification
            )
        {
            var document = GetDocument (specification);
            if (document is not null)
            {
                var reader = new StringReader (document);

                return TreeFile.ParseStream (reader);
            }

            return null;
        }

        /// <summary>
        /// Получение рабочего листа из кэша.
        /// Если локальная копия отсутствует,
        /// она запрашивается с сервера.
        /// </summary>
        public WsFile? GetWs
            (
                FileSpecification specification
            )
        {
            var document = GetDocument (specification);
            if (document is not null)
            {
                var reader = new StringReader (document);

                return WsFile.ParseStream (reader);
            }

            return null;
        }

        /// <summary>
        /// Получение рабочего листа из кэша.
        /// Если локальная копия отсутствует,
        /// она запрашивается с сервера.
        /// </summary>
        public WssFile? GetWss
            (
                FileSpecification specification
            )
        {
            var document = GetDocument (specification);
            if (document is not null)
            {
                var reader = new StringReader (document);

                return WssFile.ParseStream (reader);
            }

            return null;
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

        /// <summary>
        /// Обновление меню на сервере и заодно в кэше.
        /// </summary>
        public void UpdateMenu (FileSpecification specification, MenuFile menu) =>
            UpdateDocument (specification, menu.ToText());

        /// <summary>
        /// Обновление "деревянного" меню на сервере и заодно в кэше.
        /// </summary>
        public void UpdateTree (FileSpecification specification, TreeFile tree)
        {
            UpdateDocument (specification, tree.ToString() ?? string.Empty);
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

        /// <summary>
        /// Обновление рабочего листа на сервере и заодно в кэше.
        /// </summary>
        public void UpdateWs (FileSpecification specification, WsFile worksheet)
        {
            UpdateDocument (specification, worksheet.ToString());
        }

        /// <summary>
        /// Обновление рабочего листа на сервере и заодно в кэше.
        /// </summary>
        public void UpdateWss (FileSpecification specification, WssFile worksheet)
        {
            UpdateDocument (specification, worksheet.ToString());
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
