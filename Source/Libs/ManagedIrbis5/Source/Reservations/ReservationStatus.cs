// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ReservationStatus.cs -- статус компьютера для резервирования
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Reservations
{
    /// <summary>
    /// Статус компьютера для резервирования.
    /// </summary>
    public static class ReservationStatus
    {
        #region Constants

        /// <summary>
        /// Свободен, доступен для резервирования.
        /// </summary>
        public const string Available = "0";

        /// <summary>
        /// Свободен, доступен для резервирования.
        /// </summary>
        public const string Free = "0";

        /// <summary>
        /// Занят.
        /// </summary>
        public const string Busy = "1";

        /// <summary>
        /// Зарезервирован (заказан), но пока не занят.
        /// </summary>
        public const string Reserved = "9";

        /// <summary>
        /// Недоступен для резервирования.
        /// </summary>
        public const string NotAvailable = "5";

        #endregion

    } // class ReservationStatus

} // namespace ManagedIrbis.Reservations
