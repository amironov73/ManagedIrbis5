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

/* TreeCache.cs -- простейший кэш "деревянных" меню для ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Trees;

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace ManagedIrbis.Caching;

/// <summary>
/// Простейший кэш "деревянных" меню для ИРБИС
/// </summary>
public sealed class TreeCache
    : DocumentCache
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TreeCache
        (
            ISyncProvider provider
        )
        : base (provider)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор с опциями кэширования.
    /// </summary>
    public TreeCache
        (
            ISyncProvider provider,
            MemoryCacheOptions options
        )
        : base (provider, options)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор с внешним кэш-провайдером.
    /// </summary>
    public TreeCache
        (
            ISyncProvider provider,
            IMemoryCache cache
        )
        : base (provider, cache)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Public methods

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
        Sure.NotNull (specification);

        var document = GetDocument (specification);
        if (document is not null)
        {
            var reader = new StringReader (document);

            return TreeFile.ParseStream (reader);
        }

        return null;
    }

    /// <summary>
    /// Обновление "деревянного" меню на сервере и заодно в кэше.
    /// </summary>
    public void UpdateTree
        (
            FileSpecification specification,
            TreeFile tree
        )
    {
        Sure.NotNull (specification);
        Sure.NotNull (tree);

        UpdateDocument
            (
                specification,
                tree.ToString() ?? string.Empty
            );
    }

    #endregion
}
