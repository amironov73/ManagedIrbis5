// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FlcConstants.cs -- константы, связанные с формальным контролем
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Flc
{
    /// <summary>
    /// Константы, связанные с формальным контролем записей.
    /// </summary>
    public static class FlcConstants
    {
        #region Constants

        /// <summary>
        /// Имя сценария по умолчанию, запускаемого при создании
        /// или обновлении записи на сервере.
        /// </summary>
        public const string DbnFlc = "@dbnflc";

        /// <summary>
        /// Имя сценария по умолчанию, запускаемого перед удалением
        /// записи из базы данных.
        /// </summary>
        public const string DelFlc = "@delflc";

        #endregion

    } // class FlcConstants

} // namespace ManagedIrbis.Flc
