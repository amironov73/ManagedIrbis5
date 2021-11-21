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

/* RecordSourceType.cs -- коды видов организаций, создающих библиографические записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// Коды видов организаций, содающих библиографические записи.
    /// </summary>
    public static class RecordSourceType
    {
        #region Constants

        /// <summary>
        /// Не определено.
        /// </summary>
        public const string Undefined = "00";

        /// <summary>
        /// Издатель.
        /// </summary>
        public const string Publisher = "01";

        /// <summary>
        /// Дистрибьютор, назначенный издателем.
        /// </summary>
        public const string Distributor = "02";

        /// <summary>
        /// Оптовая книготорговая организация.
        /// </summary>
        public const string WholesaleTrader = "03";

        /// <summary>
        /// Библиографическое агентство.
        /// </summary>
        public const string Agency = "04";

        /// <summary>
        /// Библиотечный коллектор.
        /// </summary>
        public const string Collector = "05";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значений констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (RecordSourceType));
        }

        #endregion
    }
}
