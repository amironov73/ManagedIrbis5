// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LanguageCode.cs -- коды языков в соответствии с jz.mnu
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
    /// Коды языков в соответствии с jz.mnu.
    /// </summary>
    public static class LanguageCode
    {
        #region Constants

        /// <summary>
        /// Английский.
        /// </summary>
        [Description ("Английский")]
        public const string English = "eng";

        /// <summary>
        /// Белорусский.
        /// </summary>
        [Description ("Белорусский")]
        public const string Belorussian = "bel";

        /// <summary>
        /// Бурятский.
        /// </summary>
        [Description ("Бурятский")]
        public const string Buryat = "bua";

        /// <summary>
        /// Испанский.
        /// </summary>
        [Description ("Испанский")]
        public const string Spanish = "spa";

        /// <summary>
        /// Итальянский.
        /// </summary>
        [Description ("Итальянский")]
        public const string Italian = "ita";

        /// <summary>
        /// Казахский.
        /// </summary>
        [Description ("Казахский")]
        public const string Kazakh = "kaz";

        /// <summary>
        /// Китайский.
        /// </summary>
        [Description ("Китайский")]
        public const string Chinese = "chi";

        /// <summary>
        /// Корейский.
        /// </summary>
        [Description ("Корейский")]
        public const string Korean = "kor";

        /// <summary>
        /// Латинский.
        /// </summary>
        [Description ("Латинский")]
        public const string Latin = "lat";

        /// <summary>
        /// Многоязычное издание.
        /// </summary>
        [Description ("Многоязычное издание")]
        public const string Multilanguage = "mul";

        /// <summary>
        /// Не определено.
        /// </summary>
        [Description ("Не определено")]
        public const string Undefined = "und";

        /// <summary>
        /// Немецкий.
        /// </summary>
        [Description ("Немецкий")]
        public const string German = "ger";

        /// <summary>
        /// Польский.
        /// </summary>
        [Description ("Польский")]
        public const string Polish = "pol";

        /// <summary>
        /// Португальский.
        /// </summary>
        [Description ("Португальский")]
        public const string Portuguese = "por";

        /// <summary>
        /// Румынский.
        /// </summary>
        [Description ("Румынский")]
        public const string Rumanian = "rum";

        /// <summary>
        /// Русский.
        /// </summary>
        [Description ("Русский")]
        public const string Russian = "rus";

        /// <summary>
        /// Татарский.
        /// </summary>
        [Description ("Татарский")]
        public const string Tartarian = "tar";

        /// <summary>
        /// Украинский.
        /// </summary>
        [Description ("Украинский")]
        public const string Ukrainian = "ukr";

        /// <summary>
        /// Французский.
        /// </summary>
        [Description ("Французский")]
        public const string French = "fre";

        /// <summary>
        /// Хинди.
        /// </summary>
        [Description ("Хинди")]
        public const string Hindi = "hin";

        /// <summary>
        /// Чешский.
        /// </summary>
        [Description ("Чешский")]
        public const string Czech = "cze";

        /// <summary>
        /// Японский.
        /// </summary>
        [Description ("Японский")]
        public const string Japanese = "jpn";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значение констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (LanguageCode));
        }

        /// <summary>
        /// Получение словаря "код" - "значение".
        /// </summary>
        public static Dictionary<string, string> ListValuesWithDescriptions()
        {
            return ReflectionUtility.ListConstantValuesWithDescriptions (typeof (LanguageCode));
        }

        #endregion
    }
}
