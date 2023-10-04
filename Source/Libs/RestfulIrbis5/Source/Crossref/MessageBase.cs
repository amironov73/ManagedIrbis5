// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* MessageBase.cs -- базовый класс для сообщений-ответов Crossref
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace RestfulIrbis.Crossref;

/// <summary>
/// Базовый класс для сообщений-ответов Crossref.
/// </summary>
[PublicAPI]
public class MessageBase
{
    #region Properties

    /// <summary>
    /// Общее количество ответов на запрос.
    /// </summary>
    [JsonProperty ("total-results")]
    public long TotalResults { get; set; }

    #endregion
}
