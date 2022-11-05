// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* HudBook.cs -- книга художественного фонда
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.NewModel;

/// <summary>
/// Отдельный выпуск журнала.
/// </summary>
[Table ("magtrans")]
[DebuggerDisplay ("{Barcode}: {Ticket}")]
public sealed class MagazineIssue
{
    #region Properties

    /// <summary>
    /// Штрих-код.
    /// </summary>
    [Column ("barcode", IsPrimaryKey = true)]
    [JsonPropertyName ("barcode")]
    public string? Barcode { get; set; }

    /// <summary>
    /// Идентификатор журнала (см. таблицу <c>magazines</c>).
    /// </summary>
    [Column ("magazine")]
    public int Magazine { get; set; }

    /// <summary>
    /// Год выхода.
    /// </summary>
    [Column ("year")]
    [JsonPropertyName ("year")]
    public int Year { get; set; }

    /// <summary>
    /// Том журнала.
    /// </summary>
    [Column ("volume"), Nullable]
    [JsonPropertyName ("volume")]
    public int? Volume { get; set; }

    /// <summary>
    /// Номер выпуска.
    /// </summary>
    [Column ("number")]
    [JsonPropertyName ("number")]
    public int Number { get; set; }

    /// <summary>
    /// Идентификатор оператора, создавшего запись о данном выпуске.
    /// </summary>
    [Column ("operator")]
    [JsonPropertyName ("operator")]
    public int Operator { get; set; }

    /// <summary>
    /// Момент создания записи о данном выпуске.
    /// </summary>
    [Column ("moment"), Nullable]
    [JsonPropertyName ("moment")]
    public DateTime? Moment { get; set; }

    /// <summary>
    /// Машина, на которой создавалась запись о выпуске.
    /// </summary>
    [Column ("machine")]
    public string? Machine { get; set; }

    /// <summary>
    /// Сообщение для оператора.
    /// </summary>
    [Column ("alert"), Nullable]
    [JsonPropertyName ("alert")]
    public string? Alert { get; set; }

    /// <summary>
    /// Номер билета или место хранения (код читального зала).
    /// </summary>
    [Column ("chb"), Nullable]
    [JsonPropertyName ("ticket")]
    public string? Ticket { get; set; }

    /// <summary>
    /// Предполагаемая дата возврата.
    /// </summary>
    [Column ("srok"), Nullable]
    public DateTime? Deadline { get; set; }

    /// <summary>
    /// Идентификатор оператора, оформившего выдачу.
    /// </summary>
    [Column ("operator2"), Nullable]
    public int? Operator2 { get; set; }

    /// <summary>
    /// Идентификатор RFID-метки.
    /// </summary>
    [Column ("rfid"), Nullable]
    [JsonPropertyName ("rfid")]
    public string? Rfid { get; set; }

    /// <summary>
    /// Номер билета читателя, которому выдан данный выпуск
    /// (при выдаче из читального зала).
    /// </summary>
    [Column ("onhand"), Nullable]
    public string? OnHand { get; set; }

    /// <summary>
    /// Количество номеров в подшивке (для подшивки).
    /// </summary>
    [Column ("period"), Nullable]
    public int? Period { get; set; }

    /// <summary>
    /// Дата инвентаризации.
    /// </summary>
    [Column ("seen"), Nullable]
    public DateTime? Seen { get; set; }

    /// <summary>
    /// Оператор, проводивший инвентаризацию.
    /// </summary>
    [Column ("seenby"), Nullable]
    public int? SeenBy { get; set; }

    #endregion
}
