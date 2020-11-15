// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* Attendance.cs -- посещение библиотеки
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
    /// Посещение библиотеки.
    /// </summary>
    [Table]
    public class Attendance
    {
        #region Properties

        /// <summary>
        /// Идентификатор.
        /// </summary>
        [Identity, PrimaryKey, Column]
        public int ID { get; set; }

        /// <summary>
        /// Имя машины, с которой зарегистрировано посещение.
        /// </summary>
        [Column, Nullable]
        public string? Machine { get; set; }

        /// <summary>
        /// Наименование абонемента, на котором произошло
        /// посещение.
        /// </summary>
        [Column, Nullable]
        public string? Abonement { get; set; }

        /// <summary>
        /// Номер читательского билета.
        /// </summary>
        [Column]
        public string? Ticket { get; set; }

        /// <summary>
        /// Момент посещения (по часам машины,
        /// зарегистрировавшей посещение).
        /// </summary>
        [Column]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Идентификатор оператора,
        /// зарегистрировавшего посещение.
        /// </summary>
        [Column]
        public int Operator { get; set; }

        /// <summary>
        /// Тип посещения.
        /// </summary>
        [Column(Name = "typ")]
        public char Type { get; set; }

        /// <summary>
        /// Инвентарный номер документа, выданного
        /// или полученного в ходе посещения.
        /// </summary>
        [Column, Nullable]
        public string? Number { get; set; }

        #endregion
    }
}
