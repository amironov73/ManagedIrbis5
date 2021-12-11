// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* EventCode.cs -- коды действий клиента
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
    /// Коды действий клиента, см. <c>5.mnu</c>.
    /// </summary>
    public static class EventCode
    {
        #region Constants

        /// <summary>
        /// Корректировка текущей записи.
        /// </summary>
        [Description ("Корректировка текущей записи")]
        public const string Correction = "1";

        /// <summary>
        /// Импорт/заимствование.
        /// </summary>
        [Description ("Импорт/заимствование")]
        public const string Import = "2";

        /// <summary>
        /// Копирование.
        /// </summary>
        [Description ("Копирование")]
        public const string Copy = "3";

        /// <summary>
        /// Экспорт.
        /// </summary>
        [Description ("Экспорт")]
        public const string Export = "4";

        /// <summary>
        /// Глобальная корректировка.
        /// </summary>
        [Description ("Глобальная корректировка")]
        public const string GlobalCorrection = "5";

        /// <summary>
        /// Выполнение пакетного задания.
        /// </summary>
        [Description ("Выполнение пакетного задания")]
        public const string Batch = "6";

        /// <summary>
        /// Печать списочная.
        /// </summary>
        [Description ("Печать списочная")]
        public const string ListPrint = "7";

        /// <summary>
        /// Печать табличная.
        /// </summary>
        [Description ("Печать табличная")]
        public const string TablePrint = "8";

        /// <summary>
        /// Регистрация (начало сеанса).
        /// </summary>
        [Description ("Регистрация")]
        public const string Connect = "9";

        /// <summary>
        /// Разрегистрация (завершение сеанса).
        /// </summary>
        [Description ("Разрегистрация")]
        public const string Disconnect = "10";

        /// <summary>
        /// Корректировка справочника.
        /// </summary>
        [Description ("Корректировка справочника")]
        public const string DictionaryCorrection = "11";

        /// <summary>
        /// Печать статистической формы.
        /// </summary>
        [Description ("Печать статистической формы")]
        public const string StatFormPrint = "12";

        /// <summary>
        /// Печать статистики.
        /// </summary>
        [Description ("Печать статистики")]
        public const string StatisticPrint = "13";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значение констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (EventCode));
        }

        /// <summary>
        /// Получение словаря "код" - "значение" для статусов экземпляров.
        /// </summary>
        public static Dictionary<string, string> ListValuesWithDescriptions()
        {
            return ReflectionUtility.ListConstantValuesWithDescriptions (typeof (EventCode));
        }

        #endregion
    }
}
