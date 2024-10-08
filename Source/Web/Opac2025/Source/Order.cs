// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Order.cs -- информация о заказе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

// using LinqToDB.Mapping;

#endregion

namespace Opac2025;

/// <summary>
/// Информация о заказе.
/// </summary>
[PublicAPI]
// [Table ("orders")]
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
    [JsonPropertyName ("ticket")]
    public string? Ticket { get; set; }

    /// <summary>
    /// Библиографическое описание заказанной книги.
    /// </summary>
    [JsonPropertyName ("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Ссылка на заказанный экземпляр книги.
    /// </summary>
    [JsonPropertyName ("instance")]
    public Instance? Instance { get; set; }

    /// <summary>
    /// Дата создания заказа.
    /// </summary>
    [JsonPropertyName ("date")]
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// Статус заказа (см. константы в <see cref="Constants"/>).
    /// </summary>
    [JsonPropertyName ("status")]
    public string? Status { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Ticket}: {Instance}: {Date}: {Status}";

    #endregion
}
