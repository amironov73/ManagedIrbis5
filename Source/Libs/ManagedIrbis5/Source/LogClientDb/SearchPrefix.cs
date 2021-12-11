// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* SearchPrefix.cs -- поисковые префиксы для клиентского лога
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.LogClientDb
{
    /// <summary>
    /// Поисковые префиксы для клиентского лога.
    /// </summary>
    public static class SearchPrefix
    {
        #region Constants

        /// <summary>
        /// Дата/время, поле 1.
        /// </summary>
        [Description ("Момент действия - дата/время")]
        public const string Moment = "1=";

        /// <summary>
        /// Логин клиента, поле 2.
        /// </summary>
        [Description ("Логин клиента")]
        public const string Login = "2=";

        /// <summary>
        /// IP-адрес клиента, поле 3.
        /// </summary>
        [Description ("IP-адрес клиента")]
        public const string IpAddress = "3=";

        /// <summary>
        /// Имя базы данных, поле 4.
        /// </summary>
        [Description ("Имя базы данных")]
        public const string Database = "4=";

        /// <summary>
        /// Код действия, поле 5.
        /// См. константы в <see cref="EventCode"/>.
        /// </summary>
        [Description ("Код действия")]
        public const string ActionCode = "5=";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значение констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (SearchPrefix));
        }

        /// <summary>
        /// Получение словаря "код" - "значение" для статусов экземпляров.
        /// </summary>
        public static Dictionary<string, string> ListValuesWithDescriptions()
        {
            return ReflectionUtility.ListConstantValuesWithDescriptions (typeof (SearchPrefix));
        }

        #endregion
    }
}
