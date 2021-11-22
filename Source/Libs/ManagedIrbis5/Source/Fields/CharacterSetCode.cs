// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* CharacterSetCode.cs -- коды наборов символов
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
    /// Коды наборов символов в соответствии с gr.mnu.
    /// </summary>
    public static class CharacterSetCode
    {
        #region Constants

        /// <summary>
        /// Основной латинский набор.
        /// </summary>
        [Description ("Основной латинский набор")]
        public const string BasicLatin = "01";

        /// <summary>
        /// Основной кириллический набор.
        /// </summary>
        [Description ("Основной кириллический набор")]
        public const string BasicCyrillic = "02";

        /// <summary>
        /// Расширенный латинский набор.
        /// </summary>
        [Description ("Расширенный латинский набор")]
        public const string ExtendedLatin = "03";

        /// <summary>
        /// Расширенный кириллический набор.
        /// </summary>
        [Description ("Расширенный кириллический набор")]
        public const string ExtendedCyrillic = "04";

        /// <summary>
        /// Греческий набор.
        /// </summary>
        [Description ("Греческий набор")]
        public const string Greek = "05";

        /// <summary>
        /// Набор символов африканских языков.
        /// </summary>
        [Description ("Набор символов африканских языков")]
        public const string African = "06";

        /// <summary>
        /// Набор символов грузинского алфавита.
        /// </summary>
        [Description ("Набор символов грузинского алфавита")]
        public const string Georgian = "07";

        /// <summary>
        /// Набор символов иврита(таблица 1).
        /// </summary>
        [Description ("Набор символов иврита(таблица 1)")]
        public const string Hebrew1 = "08";

        /// <summary>
        /// Набор символов иврита(таблица 2).
        /// </summary>
        [Description ("Набор символов иврита(таблица 2)")]
        public const string Hebrew2 = "09";

        /// <summary>
        /// ISO 10646 (Unicode).
        /// </summary>
        [Description ("ISO 10646 (Unicode)")]
        public const string ISO10646 = "50";

        /// <summary>
        /// Code Page 866 (MSDOS russian).
        /// </summary>
        [Description ("Code Page 866 (MSDOS russian)")]
        public const string CP866 = "79";

        /// <summary>
        /// WIN 1251 (Windows russian).
        /// </summary>
        [Description ("WIN 1251 (Windows russian)")]
        public const string WIN1251 = "89";

        /// <summary>
        /// KOI-8 (Unix russian).
        /// </summary>
        [Description ("KOI-8 (Unix russian)")]
        public const string KOI8 = "99";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значение констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (CharacterSetCode));
        }

        /// <summary>
        /// Получение словаря "код" - "значение".
        /// </summary>
        public static Dictionary<string, string> ListValuesWithDescriptions()
        {
            return ReflectionUtility.ListConstantValuesWithDescriptions (typeof (CharacterSetCode));
        }

        #endregion
    }
}
