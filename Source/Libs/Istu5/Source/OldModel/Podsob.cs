// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* Podsob.cs -- подсобные фонды
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 */

#region Using directives

using System;

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.OldModel
{
    /// <summary>
    /// Подсобные фонды
    /// </summary>
    [Table]
    public class Podsob
    {
        #region Properties

        /// <summary>
        /// Инвента
        /// </summary>
        [Column (Name = "invent"), PrimaryKey]
        public long Inventory { get; set; }

        /// <summary>
        /// Номер читательского билета, кому выдана книга.
        /// </summary>
        [Column (Name = "chb"), Nullable]
        public string? Ticket { get; set; }

        /// <summary>
        /// Дополнительная информация о читателе.
        /// </summary>
        [Column ("ident"), Nullable]
        public string? AdditionalInfo { get; set; }

        /// <summary>
        /// Дата выдачи.
        /// </summary>
        [Column ("whe")]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Идентификатор оператора.
        /// </summary>
        [Column]
        public int Operator { get; set; }

        /// <summary>
        /// Предполагаемая дата возврата.
        /// </summary>
        [Column (Name = "srok")]
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Количество продлений.
        /// </summary>
        [Column (Name = "prodlen")]
        public int Prolongation { get; set; }

        /// <summary>
        /// На руках у читателя? Для читальных залов.
        /// </summary>
        [Column, Nullable]
        public string? OnHand { get; set; }

        /// <summary>
        /// Примечания об экземпляре книги.
        /// </summary>
        [Column, Nullable]
        public string? Alert { get; set; }

        /// <summary>
        /// Контрольный экземпляр.
        /// </summary>
        [Column]
        public char Pilot { get; set; }

        /// <summary>
        /// Место хранения: ЦОР, ЦНИ и т. д.
        /// </summary>
        [Column, Nullable]
        public string? Sigla { get; set; }

        #endregion
    }
}
