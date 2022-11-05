// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* BookRecord.cs -- общая информация о книге по данным таблицы books
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

using AM;

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.NewModel;

/// <summary>
/// Общая информация о книге по данным таблицы <c>books</c>.
/// </summary>
[Table ("books")]
public sealed class BookRecord
{
    #region Properties

    /// <summary>
    /// Идентификатор и первичный ключ.
    /// </summary>
    [PrimaryKey]
    [Identity]
    [Column ("id")]
    [JsonPropertyName ("id")]
    [Browsable (false)]
    public int Id { get; set; }

    /// <summary>
    /// Каталог.
    /// </summary>
    [Column ("catalog")]
    [JsonPropertyName ("catalog")]
    [DisplayName ("Каталог")]
    public string? Catalog { get; set; }

    /// <summary>
    /// Инвентарный номер.
    /// </summary>
    [Nullable]
    [Column ("number")]
    [JsonPropertyName ("number")]
    [DisplayName ("Инвентарный номер")]
    public string? Number { get; set; }

    /// <summary>
    /// Номер карточки суммарного учета.
    /// </summary>
    [Nullable]
    [Column ("card")]
    [JsonPropertyName ("card")]
    [DisplayName ("Номер карточки комплектования")]
    public string? Card { get; set; }

    /// <summary>
    /// Место хранения экземпляра.
    /// </summary>
    [Nullable (false)]
    [Column ("place")]
    [JsonPropertyName ("place")]
    [DisplayName ("Место хранения")]
    public string? Place { get; set; }

    /// <summary>
    /// Штрих-код.
    /// </summary>
    [Nullable]
    [Column ("barcode")]
    [JsonPropertyName ("barcode")]
    [DisplayName ("Штрих-код")]
    public string? Barcode { get; set; }

    /// <summary>
    /// RFID-метка.
    /// </summary>
    [Nullable]
    [Column ("rfid")]
    [JsonPropertyName ("rfid")]
    [DisplayName ("RFID-метка")]
    public string? Rfid { get; set; }

    /// <summary>
    /// Номер читательского билета.
    /// </summary>
    [Nullable]
    [Column ("ticket")]
    [JsonPropertyName ("ticket")]
    [DisplayName ("Читательский билет")]
    public string? Ticket { get; set; }

    /// <summary>
    /// Дата выдачи экземпляра.
    /// </summary>
    [Nullable]
    [Column ("moment")]
    [JsonPropertyName ("moment")]
    [DisplayName ("Дата выдачи")]
    public DateTime? Moment { get; set; }

    /// <summary>
    /// Предполагаемая дата возврата.
    /// </summary>
    [Nullable]
    [Column ("deadline")]
    [JsonPropertyName ("deadline")]
    [DisplayName ("Дата возврата")]
    public DateTime? Deadline { get; set; }

    /// <summary>
    /// Табельный номер оператора.
    /// </summary>
    [Nullable]
    [Column ("operator")]
    [JsonPropertyName ("operator")]
    [DisplayName ("Оператор")]
    public int Operator { get; set; }

    /// <summary>
    /// Счетчик продлений.
    /// </summary>
    [Nullable]
    [Column ("prolong")]
    [JsonPropertyName ("prolong")]
    [DisplayName ("Продления")]
    public int Prolongations { get; set; }

    /// <summary>
    /// Сообщение.
    /// </summary>
    [Nullable]
    [Column ("alert")]
    [JsonPropertyName ("alert")]
    [DisplayName ("Сообщение")]
    public string? Alert { get; set; }

    /// <summary>
    /// Контрольный экземпляр?
    /// </summary>
    [Nullable (false)]
    [Column ("pilot")]
    [JsonPropertyName ("pilot")]
    [DisplayName ("Контрольный экземпляр")]
    public bool Pilot { get; set; }

    /// <summary>
    /// Цена экземпляра (опциональная).
    /// </summary>
    [Nullable]
    [Column ("price")]
    [JsonPropertyName ("price")]
    [DisplayName ("Цена")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Дата инвентаризации.
    /// </summary>
    [Nullable]
    [Column ("seen")]
    [JsonPropertyName ("seen")]
    [DisplayName ("Дата инвентаризации")]
    public DateTime? Seen { get; set; }

    /// <summary>
    /// Оператор инвентаризации.
    /// </summary>
    [Nullable]
    [Column ("seenby")]
    [JsonPropertyName ("seenby")]
    [DisplayName ("Оператор инвентаризации")]
    public int SeenBy { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
    private bool Equals
        (
            BookRecord other
        )
    {
        Sure.NotNull (other);

        return Id == other.Id;
    }

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        return ReferenceEquals (this, obj) || obj is BookRecord other && Equals (other);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() => Id;

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Catalog}:{Number}:{Card}:{Place}:{Ticket}";

    #endregion
}
