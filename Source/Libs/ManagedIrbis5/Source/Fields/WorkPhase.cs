// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* WorkPhase.cs -- этап работы, подполе 907^c
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Этап работы, подполе 907^c.
    /// Согласно справочнику etr.mnu.
    /// </summary>
    public static class WorkPhase
    {
        #region Constants

        /// <summary>
        /// Создание записи, первичная каталогизация.
        /// </summary>
        [Description ("Первичная каталогизация")]
        public const string InitialCataloguing = "ПК";

        /// <summary>
        /// Размещение заказа.
        /// </summary>
        [Description ("Размещение заказа")]
        public const string PlaceAnOrder = "РЗ";

        /// <summary>
        /// Исполнение заказа.
        /// </summary>
        [Description ("Исполнение заказа")]
        public const string OrderExecution = "ИЗ";

        /// <summary>
        /// Каталогизация.
        /// </summary>
        [Description ("Каталогизация")]
        public const string Cataloguing = "КТ";

        /// <summary>
        /// Систематизация.
        /// </summary>
        [Description ("Систематизация")]
        public const string Systematization = "С";

        /// <summary>
        /// Обработка записи завершена.
        /// </summary>
        [Description ("Обработка записи завершена")]
        public const string Completed = "obrzv";

        /// <summary>
        /// Проферка фонда.
        /// </summary>
        [Description ("Проверка фонда")]
        public const string Inventarization = "ПРФ";

        /// <summary>
        /// Регистрация периодики.
        /// </summary>
        [Description ("Регистрация периодики")]
        public const string RegistrationOfPeriodicals = "РЖ";

        /// <summary>
        /// Докомплектование.
        /// </summary>
        [Description ("Докомплектование")]
        public const string Retrofitting = "ДК";

        /// <summary>
        /// Корректура.
        /// </summary>
        [Description ("Корректура")]
        public const string Correction = "КР";

        /// <summary>
        /// Выбытие.
        /// </summary>
        [Description ("Выбытие")]
        public const string WriteOff = "ВБ";

        /// <summary>
        /// Передача в другое подразделение.
        /// </summary>
        [Description ("Передача в другое подразделение")]
        public const string Transfer = "ПФ";

        /// <summary>
        /// Запись получена по обмену в формате UNIMARC.
        /// </summary>
        [Description ("Запись получена по обмену в формате UNIMARC")]
        public const string ImportUnimarc = "ZU";

        /// <summary>
        /// Запись получена по обмену в формате USMARC.
        /// </summary>
        [Description ("Запись получена по обмену в формате USMARC")]
        public const string ImportUsmarc = "ZS";

        /// <summary>
        /// Обработка не завершена (издание читателю не выдается).
        /// </summary>
        [Description ("Обработка не завершена")]
        public const string NotCompleted = "ОБРНЗ";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значение констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (WorkPhase));
        }

        /// <summary>
        /// Получение словаря "код" - "значение".
        /// </summary>
        public static Dictionary<string, string> ListValuesWithDescriptions()
        {
            return ReflectionUtility.ListConstantValuesWithDescriptions (typeof (WorkPhase));
        }

        #endregion
    }
}
