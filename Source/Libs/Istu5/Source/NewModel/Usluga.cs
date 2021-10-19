// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* Usluga.cs -- запись о выполненной услуге читателю
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.NewModel
{
    /// <summary>
    /// Запись о выполненной услуге читателю.
    /// </summary>
    [Serializable]
    [Table ("uslugi")]
    [DebuggerDisplay ("{Moment} {Ticket}")]
    public sealed class Usluga
    {
        #region Properties

        /// <summary>
        /// Идентификатор услуги.
        /// </summary>
        [PrimaryKey]
        [Column ("id", IsIdentity = true)]
        [JsonPropertyName ("id")]
        public int Id { get; set; }

        /// <summary>
        /// Момент оказания услуги.
        /// </summary>
        [Column ("moment")]
        [JsonPropertyName ("moment")]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Номер читательского билета.
        /// </summary>
        [Column ("ticket")]
        [JsonPropertyName ("ticket")]
        public string? Ticket { get; set; }

        /// <summary>
        /// Наименование услуги, например, "Ксерокопирование А4".
        /// </summary>
        [Column ("title")]
        [JsonPropertyName ("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Цена за единицу (на момент оказания услуги).
        /// </summary>
        [Column ("price")]
        [JsonPropertyName ("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Количество единиц оказанной услуги
        /// (например, количество страниц).
        /// </summary>
        [Column ("amount")]
        [JsonPropertyName ("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// Общая сумма, на которую была оказана услуга.
        /// </summary>
        [Column ("summa")]
        [JsonPropertyName ("summa")]
        public decimal Summa { get; set; }

        /// <summary>
        /// Оператор, оказавший услугу.
        /// </summary>
        [Column ("operator")]
        [JsonPropertyName ("operator")]
        public string? Operator { get; set; }

        /// <summary>
        /// Остаток на сервисном счете сразу после оплаты услуги.
        /// </summary>
        [Column ("debet")]
        [JsonPropertyName ("debet")]
        public decimal Debet { get; set; }

        /// <summary>
        /// Услуга оплачена?
        /// </summary>
        [Column ("paid")]
        [JsonPropertyName ("paid")]
        public bool Paid { get; set; }

        #endregion

        #region Public methods

        #endregion

    } // class Usluga

} // namespace Istu.NewModel
