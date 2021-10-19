// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* UchBook.cs -- книга учебного фонда
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.NewModel
{
    /// <summary>
    /// Книга учебного фонда.
    /// </summary>
    [Table ("uchtrans")]
    public sealed class UchBook
    {
        #region Properties

        /// <summary>
        /// Штрих-код книги.
        /// </summary>
        [Column ("barcode")]
        public string? Barcode { get; set; }

        /// <summary>
        /// Номер карточки комплектования.
        /// </summary>
        [Column ("cardnum")]
        public string? CardNumber { get; set; }

        /// <summary>
        /// Момент выдачи.
        /// </summary>
        [Column ("whn")]
        public DateTime Moment { get; set; }

        ///<summary>
        /// Табельный номер оператора.
        ///</summary>
        [Column ("operator")]
        public int Operator { get; set; }

        /// <summary>
        /// Примечания.
        /// </summary>
        [Column ("remarks")]
        public string? Remarks { get; set; }

        /// <summary>
        /// Номер читательского билета.
        /// </summary>
        [Column ("chb")]
        public string? Ticket { get; set; }

        /// <summary>
        /// Инвентарный номер.
        /// </summary>
        [Column ("invnum")]
        public string? Inventory { get; set; }

        /// <summary>
        /// Цена экземпляра
        /// </summary>
        [Column ("price")]
        public decimal Price { get; set; }

        ///<summary>
        /// Количество продлений.
        ///</summary>
        [Column ("prodlen")]
        public int Prolongation { get; set; }

        ///<summary>
        /// Предполагаемый срок возврата.
        ///</summary>
        [Column ("srok")]
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Снова оператор.
        /// </summary>
        [Column ("operator2")]
        public int Operator2 { get; set; }

        /// <summary>
        /// Имя машины.
        /// </summary>
        [Column ("machine")]
        public string? Machine { get; set; }

        /// <summary>
        /// Находится на руках у читателя, номер билета.
        /// </summary>
        [Column ("onhand")]
        public string? OnHand { get; set; }

        /// <summary>
        /// Сообщение о книге.
        /// </summary>
        [Column ("alert")]
        public string? Alert { get; set; }

        /// <summary>
        /// Дата инвентаризации.
        /// </summary>
        [Column ("seen")]
        public DateTime Seen { get; set; }

        /// <summary>
        /// Оператор инвентаризации.
        /// </summary>
        [Column ("seenby")]
        public int SeenBy { get; set; }

        /// <summary>
        /// RFID-метка.
        /// </summary>
        [Column ("rfid")]
        public string? Rfid { get; set; }

        /// <summary>
        /// Место хранения ЦОР, ЦНИ и т. д.
        /// </summary>
        [Column ("sigla")]
        public string? Sigla { get; set; }

        #endregion

    } // class UchBook

} // namespace Istu.NewModel
