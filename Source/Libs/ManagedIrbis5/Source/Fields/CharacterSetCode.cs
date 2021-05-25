// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* CharacterSetCode.cs -- коды наборов символов
 * Ars Magna project, http://arsmagna.ru
 */

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
        public const string BasicLatin = "01";

        /// <summary>
        /// Основной кириллический набор.
        /// </summary>
        public const string BasicCyrillic = "02";

        /// <summary>
        /// Расширенный латинский набор.
        /// </summary>
        public const string ExtendedLatin = "03";

        /// <summary>
        /// Расширенный кириллический набор.
        /// </summary>
        public const string ExtendedCyrillic = "04";

        /// <summary>
        /// Греческий набор.
        /// </summary>
        public const string Greek = "05";

        /// <summary>
        /// Набор символов африканских языков.
        /// </summary>
        public const string African = "06";

        /// <summary>
        /// Набор символов грузинского алфавита.
        /// </summary>
        public const string Georgian = "07";

        /// <summary>
        /// Набор символов иврита(таблица 1).
        /// </summary>
        public const string Hebrew1 = "08";

        /// <summary>
        /// Набор символов иврита(таблица 2).
        /// </summary>
        public const string Hebrew2 = "09";

        /// <summary>
        /// ISO 10646 (Unicode).
        /// </summary>
        public const string ISO10646 = "50";

        /// <summary>
        /// Code Page 866 (MSDOS russian).
        /// </summary>
        public const string CP866 = "79";

        /// <summary>
        /// WIN 1251 (Windows russian).
        /// </summary>
        public const string WIN1251 = "89";

        /// <summary>
        /// KOI-8 (Unix russian).
        /// </summary>
        public const string KOI8 = "99";

        #endregion

    } // class CharacterSetCode

} // namespace ManagedIrbis.Fields
