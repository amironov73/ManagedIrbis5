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

/* WsCache.cs -- простейший кэш рабочих листов для ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Workspace;

using Microsoft.Extensions.Caching.Memory;

#endregion

#nullable enable

namespace ManagedIrbis.Caching;

/// <summary>
/// Простейший кэш рабочих листов для ИРБИС.
/// </summary>
public sealed class WsCache
    : DocumentCache
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public WsCache
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
    public WsCache
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
    public WsCache
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
    /// Получение рабочего листа из кэша.
    /// Если локальная копия отсутствует,
    /// она запрашивается с сервера.
    /// </summary>
    public WsFile? GetWs
        (
            FileSpecification specification
        )
    {
        Sure.NotNull (specification);

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
        Sure.NotNull (specification);

        var document = GetDocument (specification);
        if (document is not null)
        {
            var reader = new StringReader (document);

            return WssFile.ParseStream (reader);
        }

        return null;
    }

    /// <summary>
    /// Обновление рабочего листа на сервере и заодно в кэше.
    /// </summary>
    public void UpdateWs
        (
            FileSpecification specification,
            WsFile worksheet
        )
    {
        Sure.NotNull (specification);
        Sure.NotNull (worksheet);

        UpdateDocument (specification, worksheet.ToString());
    }

    /// <summary>
    /// Обновление рабочего листа на сервере и заодно в кэше.
    /// </summary>
    public void UpdateWss
        (
            FileSpecification specification,
            WssFile worksheet
        )
    {
        Sure.NotNull (specification);
        Sure.NotNull (worksheet);

        UpdateDocument (specification, worksheet.ToString());
    }

    #endregion
}
