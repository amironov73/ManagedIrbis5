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

/* MenuCache.cs -- простейший кэш меню для ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace ManagedIrbis.Caching
{
    /// <summary>
    /// Простейший кэш меню для ИРБИС
    /// </summary>
    public sealed class MenuCache
        : DocumentCache
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MenuCache (ISyncProvider provider) : base (provider)
        {
        }

        /// <summary>
        /// Конструктор с опциями кэширования.
        /// </summary>
        public MenuCache (ISyncProvider provider, MemoryCacheOptions options)
            : base (provider, options)
        {
        }

        /// <summary>
        /// Конструктор с внешним кэш-провайдером.
        /// </summary>
        public MenuCache (ISyncProvider provider, IMemoryCache cache)
            : base (provider, cache)
        {
        }

        #endregion

        #region Public methods

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
        /// Обновление меню на сервере и заодно в кэше.
        /// </summary>
        public void UpdateMenu
            (
                FileSpecification specification,
                MenuFile menu
            )
        {
            UpdateDocument (specification, menu.ToText());
        }

        #endregion
    }
}
