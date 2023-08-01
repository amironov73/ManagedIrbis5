// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* SiteIndexResponse.cs -- ответ на запрос списка сайтов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.SpaceWeb;

/// <summary>
/// Ответ на запрос списка сайтов.
/// </summary>
[PublicAPI]
public sealed class SiteIndexResponse
{
    #region Properties

    /// <summary>
    /// Версия JSON RPC.
    /// </summary>
    [JsonProperty ("jsonrpc")]
    public string? JsonRpcVersion { get; set; }

    /// <summary>
    /// Имя вызванного метода.
    /// </summary>
    [JsonProperty ("method")]
    public string? MethodName { get; set; }

    /// <summary>
    /// Массив сайтов.
    /// </summary>
    [JsonProperty ("params")]
    public BriefSiteInfo[] Sites { get; set; }

    #endregion
}
