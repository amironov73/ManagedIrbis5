// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Order.cs -- информация о заказе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

namespace Opac2025;

/// <summary>
/// Информация о заказе.
/// </summary>
[PublicAPI]
public sealed class Order
{
    #region Properties

    /// <summary>
    /// Идентификатор заказа.
    /// </summary>
    [JsonPropertyName ("id")]
    public int Id { get; set; }

    /// <summary>
    /// Номер читательского билета.
    /// </summary>
    [JsonPropertyName ("description")]
    public string? Ticket { get; set; }

    /// <summary>
    /// Идентификатор заказанной книги.
    /// </summary>
    [JsonPropertyName ("book")]
    public string? Book { get; set; }

    /// <summary>
    /// Дата создания заказа.
    /// </summary>
    [JsonPropertyName ("date")]
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// Статус заказа.
    /// </summary>
    [JsonPropertyName ("status")]
    public string? Status { get; set; }

    #endregion
}
