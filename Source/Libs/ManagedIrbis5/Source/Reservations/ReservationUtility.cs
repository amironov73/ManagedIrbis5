// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* ReservationUtility.cs -- полезные методы для работы с заявками
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives


#endregion

#nullable enable

namespace ManagedIrbis.Reservations
{
    /// <summary>
    /// Полезные методы для работы с заявками.
    /// </summary>
    public static class ReservationUtility
    {
        #region Constants

        /// <summary>
        /// Имя базы данных по умолчанию.
        /// </summary>
        public const string DefaultDatabaseName = "RESERV";

        /// <summary>
        /// Префикс для поиска номеров по умолчанию.
        /// </summary>
        public const string DefaultNumberPrefix = "N=";

        /// <summary>
        /// Префикс для поиска залов по умолчанию.
        /// </summary>
        public const string DefaultRoomPrefix = "ROOM=";

        /// <summary>
        /// Меню залов по умолчанию.
        /// </summary>
        public const string DefaultRoomMenu = "10.mnu";

        #endregion

        #region Properties

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public static string DatabaseName { get; set; } = DefaultDatabaseName;

        /// <summary>
        /// Префикс для поиска номеров.
        /// </summary>
        public static string NumberPrefix { get; set; } = DefaultNumberPrefix;

        /// <summary>
        /// Префикс для поиска залов.
        /// </summary>
        public static string RoomPrefix { get; set; } = DefaultRoomPrefix;

        /// <summary>
        /// Имя меню залов.
        /// </summary>
        public static string RoomMenu { get; set; } = DefaultRoomMenu;

        #endregion

    } // class ReservationUtility

} // namespace ManagedIrbis.Reservations
