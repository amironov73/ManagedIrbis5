// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* NotificationType.cs -- код доступности издания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// Код доступности издания.
    /// </summary>
    public static class NotificationType
    {
        #region Constants

        /// <summary>
        /// Издание, находящееся в плане, готовящееся к выпуску.
        /// </summary>
        public const string Planned = "01";

        /// <summary>
        /// Издание, находящееся в стадии тиражирования, в типографии.
        /// </summary>
        public const string InPrint = "02";

        /// <summary>
        /// Вышедшее в свет издание.
        /// </summary>
        public const string Published = "03";

        /// <summary>
        /// Распроданное издание.
        /// </summary>
        public const string SoldOut = "04";

        /// <summary>
        /// Код удаления записи.
        /// </summary>
        public const string Deletion = "05";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значений констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (NotificationType));
        }

        #endregion
    }
}
