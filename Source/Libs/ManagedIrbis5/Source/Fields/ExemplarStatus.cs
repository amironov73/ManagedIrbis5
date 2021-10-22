// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ExemplarStatus.cs -- коды для статуса экземпляра книги/журнала/газеты
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Коды для статуса экземпляра книги/журнала/газеты.
    /// </summary>
    public static class ExemplarStatus
    {
        #region Constants

        /// <summary>
        /// Отдельный экземпляр (на индивидуальном учете)
        /// свободен и доступен для выдачи.
        /// </summary>
        public const string Free = "0";

        /// <summary>
        /// Экземпляр выдан читателю.
        /// </summary>
        public const string Loan = "1";

        /// <summary>
        /// Данный экземпляр ещё не поступил в библиотеку, ожидается.
        /// </summary>
        public const string Wait = "2";

        /// <summary>
        /// Находится в переплетной мастерской.
        /// </summary>
        public const string Bindery = "3";

        /// <summary>
        /// Экземпляр утерян.
        /// </summary>
        public const string Lost = "4";

        /// <summary>
        /// Временно не выдается.
        /// </summary>
        public const string NotAvailable = "5";

        /// <summary>
        /// Экземпляр списан.
        /// </summary>
        public const string WrittenOff = "6";

        /// <summary>
        /// Номер журнала/газеты поступил, но еще не дошел до места хранения.
        /// </summary>
        public const string OnTheWay = "8";

        /// <summary>
        /// Экземпляр на бронеполке.
        /// </summary>
        public const string Reserved = "9";

        /// <summary>
        /// Группа экземпляров для библиотеки сети.
        /// </summary>
        public const string BiblioNet = "C";

        /// <summary>
        /// Номер журнала/газеты переплетен (входит в подшивку).
        /// </summary>
        public const string Bound = "P";

        /// <summary>
        /// Группа экземпляров на размножение с вводом инвентарных номеров.
        /// </summary>
        public const string Reproduction = "R";

        /// <summary>
        /// Группа экземпляров безинвентарного учета.
        /// </summary>
        public const string Summary = "U";

        #endregion

    } // class ExemplarStatus

} // namespace ManagedIrbis.Fields
