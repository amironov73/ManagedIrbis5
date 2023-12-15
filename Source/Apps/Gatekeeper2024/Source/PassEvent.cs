// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* PassEvent.cs -- событие прохода читателя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using AM.Json;

using JetBrains.Annotations;

#endregion

namespace Gatekeeper2024;

/// <summary>
/// Событие прохода читателя
/// для отправки на сервер ИРБИС64.
/// </summary>
[PublicAPI]
internal sealed class PassEvent
{
    #region Properties

    /// <summary>
    /// Номер турникета: вход или выход.
    /// </summary>
    [JsonPropertyName ("point")]
    public int Point { get; set; }

    /// <summary>
    /// Момент.
    /// </summary>
    [JsonPropertyName ("moment")]
    public DateTimeOffset Moment { get; set; }

    /// <summary>
    /// Идентификатор читателя.
    /// </summary>
    [JsonPropertyName ("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Данные для отправки.
    /// </summary>
    [JsonPropertyName ("data")]
    public string? Data { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => JsonUtility.SerializeWithReadableCyrillic (this);

    #endregion
}
