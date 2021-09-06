// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* Loan.cs -- абстрактная книговыдача
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

using AM.Data;

#endregion

#nullable enable

namespace Istu.OldModel.Loans
{
    /// <summary>
    /// Абстрактная книговыдача.
    /// </summary>
    public class Loan
    {
        #region Properties

        /// <summary>
        /// Какие-нибудь дополнительные данные,
        /// сопровождающие информацию о выданной книге.
        /// В настоящее время не используется.
        /// </summary>
        [HiddenColumn]
        [Browsable(false)]
        public object? Context { get; set; }

        ///<summary>
        /// MFN записи о книге в каталоге.
        ///</summary>
        [ColumnIndex(0)]
        [ColumnWidth(30)]
        [ColumnHeader("MFN")]
        [DisplayName("MFN")]
        [ReadOnly(true)]
        public int Mfn { get; set; }

        ///<summary>
        /// Инвентарный номер книги.
        ///</summary>
        [ColumnIndex(1)]
        [ColumnWidth(50)]
        [ColumnHeader("Номер")]
        [DisplayName("Номер")]
        [ReadOnly(true)]
        public string? Number { get; set; }

        ///<summary>
        /// Номер читательского билета,
        /// кому выдана книга.
        ///</summary>
        [ColumnIndex(3)]
        [ColumnWidth(50)]
        [ColumnHeader("На руках")]
        [DisplayName("На руках")]
        public string? Ticket { get; set; }

        ///<summary>
        /// ФИО читателя, кому выдана книга.
        ///</summary>
        [ColumnIndex(4)]
        [ColumnWidth(100)]
        [ColumnHeader("ФИО читателя")]
        [DisplayName("ФИО читателя")]
        public string? Name { get; set; }

        ///<summary>
        /// Краткое библиографическое описание книги.
        ///</summary>
        [ColumnIndex(2)]
        [ColumnWidth(150)]
        [ColumnHeader("Библиографическое описание")]
        [DisplayName("Библиографическое описание")]
        public string? Description { get; set; }

        ///<summary>
        /// Крайний срок возврата книги.
        ///</summary>
        [ColumnIndex(5)]
        [ColumnWidth(60)]
        [ColumnHeader("Срок")]
        [DisplayName("Срок")]
        public DateTime Deadline { get; set; }

        ///<summary>
        /// Счетчик продлений выдачи.
        ///</summary>
        [ColumnIndex(6)]
        [ColumnWidth(30)]
        [DefaultValue(0)]
        [ColumnHeader("Продл.")]
        [DisplayName("Продления")]
        public int ProlongationCount { get; set; }

        ///<summary>
        /// Идентификатор оператора.
        ///</summary>
        [HiddenColumn]
        [DisplayName("Оператор")]
        public int Operator { get; set; }

        ///<summary>
        /// Дата выдачи книги читателю.
        ///</summary>
        [HiddenColumn]
        [DisplayName("Дата выдачи")]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Признак контрольного экземпляра.
        /// </summary>
        /// <value>Если не пустая строка,
        /// значит, контрольный экземпляр.</value>
        [DisplayName("Контрольный экземпляр")]
        [ColumnHeader("")]
        [ColumnIndex(10)]
        [ColumnWidth(10)]
        public string? PilotCopy { get; set; }

        /// <summary>
        /// Читатель, которому выдана книга
        /// (для книг из подсобного фонда
        /// читального зала).
        /// </summary>
        [DisplayName("У читателя")]
        [ColumnHeader("")]
        [ColumnIndex(9)]
        [ColumnWidth(20)]
        public string? OnHand { get; set; }

        /// <summary>
        /// Номер карточки безинвентарного учета
        /// (для многоэкземплярной литературы).
        /// </summary>
        [DisplayName("Карточка комплектования")]
        [HiddenColumn]
        [ColumnWidth(20)]
        public string? CardNumber { get; set; }

        /// <summary>
        /// Примечания об экземпляре документа.
        /// </summary>
        [DisplayName("Примечания")]
        [HiddenColumn]
        public string? Alert { get; set; }

        /// <summary>
        /// Цена экземпляра
        /// </summary>
        [DisplayName("Цена")]
        [HiddenColumn]
        public decimal? Price { get; set; }

        /// <summary>
        /// Код записи в электронном каталоге
        /// (поле 903).
        /// </summary>
        [HiddenColumn]
        [DisplayName("Код в каталоге")]
        public string? BookID { get; set; }

        /// <summary>
        /// Идентификатор RFID-метки.
        /// </summary>
        [DisplayName("RFID")]
        [HiddenColumn]
        public string? Rfid { get; set; }

        /// <summary>
        /// Для обозначения книг ЦОР, ЦНИ и проч.
        /// </summary>
        [DisplayName("Сигла")]
        [ColumnHeader("Сигла")]
        [ColumnIndex(10)]
        [ColumnWidth(20)]
        [HiddenColumn]
        public string? Sigla { get; set; }

        /// <summary>
        /// Место: ЦОР или ИЗО.
        /// </summary>
        [DisplayName("Место")]
        [ColumnHeader("Место")]
        [ColumnIndex(11)]
        [ColumnWidth(20)]
        public string? Place { get; set; }

        #endregion

        #region Private members

        // /// <summary>
        // /// Registers the attendance.
        // /// </summary>
        // /// <param name="db">The db.</param>
        // /// <param name="attendance">The attendance.</param>
        // protected static void RegisterAttendance(DbManager db, Attendance attendance)
        // {
        //     bool disableCounting
        //         = ConfigurationUtility.GetBoolean("disableCounting", false);
        //     if (disableCounting)
        //     {
        //         return;
        //     }
        //
        //     if (attendance != null)
        //     {
        //         db.Insert<Attendance>("attendance", new[] {attendance});
        //     }
        //
        // } // method RegisterAttendance

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => Description ?? Number ?? "???";

        #endregion

    } // class Loan

} // namespace Istu.OldModel.Loans
