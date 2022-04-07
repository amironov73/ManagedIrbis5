// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

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

namespace Istu.OldModel;

/// <summary>
/// Книга художественного фонда.
/// </summary>
[Table ("hudtrans")]
[DebuggerDisplay ("{Inventory}: {Ticket}")]
public sealed class HudBook
{
    #region Properties

    /// <summary>
    /// Инвентарный номер.
    /// </summary>
    [Column ("invnum")]
    [JsonPropertyName ("inventory")]
    public string? Inventory { get; set; }

    /// <summary>
    /// Дата выдачи.
    /// </summary>
    [Column ("whn")]
    [JsonPropertyName ("moment")]
    public DateTime Moment { get; set; }

    /// <summary>
    /// Табельный номер оператора.
    /// </summary>
    [Column ("operator")]
    [JsonPropertyName ("operator")]
    public int Operator { get; set; }

    /// <summary>
    /// Номер читательского билета.
    /// </summary>
    [Column ("chb")]
    [JsonPropertyName ("ticket")]
    public string? Ticket { get; set; }

    /// <summary>
    /// Счетчик продлений
    /// </summary>
    [Column ("prodlen")]
    [JsonPropertyName ("prolong")]
    public int Prolong { get; set; }

    /// <summary>
    /// Предполагаемая дата возврата.
    /// </summary>
    [Column ("srok")]
    [JsonPropertyName ("deadline")]
    public DateTime Deadline { get; set; }

    /// <summary>
    /// Сообщение.
    /// </summary>
    [Column ("alert")]
    [JsonPropertyName ("alert")]
    public string? Alert { get; set; }

    /// <summary>
    /// RFID-метка.
    /// </summary>
    [Column ("rfid")]
    [JsonPropertyName ("rfid")]
    public string? Rfid { get; set; }

    #endregion
}
