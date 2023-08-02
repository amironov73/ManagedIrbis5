// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* SpaceWebResponse.cs -- стандартный ответ SpaceWeb
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.SpaceWeb;

/// <summary>
/// Стандартный ответ SpaceWeb.
/// </summary>
[PublicAPI]
public sealed class SpaceWebResponse<TResult>
    where TResult: class
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
    /// Результат.
    /// </summary>
    [JsonProperty ("result")]
    public TResult? Result { get; set; }

    #endregion
}
