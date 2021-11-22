// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TitleCharacterSet.cs -- графика заглавия согласно grz.mnu
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
    /// Графика заглавия согласно grz.mnu.
    /// </summary>
    public static class TitleCharacterSet
    {
        #region Constants

        /// <summary>
        /// Латинская.
        /// </summary>
        [Description ("")]
        public const string Latin = "ba";

        /// <summary>
        /// Кириллическая.
        /// </summary>
        [Description ("Кириллическая")]
        public const string Cyrillic = "ca";

        /// <summary>
        /// Японская неопределенная.
        /// </summary>
        [Description ("Японская неопределенна")]
        public const string JapaneseUndefined = "da";

        /// <summary>
        /// Японская - канджи.
        /// </summary>
        [Description ("Японская - канджи")]
        public const string JapaneseKanji = "db";

        /// <summary>
        /// Японская - кана.
        /// </summary>
        [Description ("Японская - кана")]
        public const string JapaneseKana = "dc";

        /// <summary>
        /// Китайская.
        /// </summary>
        [Description ("Китайская")]
        public const string Chinese = "ea";

        /// <summary>
        /// Арабская.
        /// </summary>
        [Description ("Арабская")]
        public const string Arab = "fa";

        /// <summary>
        /// Греческая.
        /// </summary>
        [Description ("Греческая")]
        public const string Greek = "ga";

        /// <summary>
        /// Иврит.
        /// </summary>
        [Description ("Иврит")]
        public const string Hebrew = "ha";

        /// <summary>
        /// Тайская.
        /// </summary>
        [Description ("Тайская")]
        public const string Thai = "ia";

        /// <summary>
        /// Девангари.
        /// </summary>
        [Description ("Девангари")]
        public const string Devanagari = "ja";

        /// <summary>
        /// Корейская.
        /// </summary>
        [Description ("Корейская")]
        public const string Korean = "ka";

        /// <summary>
        /// Тамильская.
        /// </summary>
        [Description ("Тамильская")]
        public const string Tamil = "la";

        /// <summary>
        /// Грузинская.
        /// </summary>
        [Description ("Грузинская")]
        public const string Georgian = "ma";

        /// <summary>
        /// Армянская.
        /// </summary>
        [Description ("Армянская")]
        public const string Armenian = "mb";

        /// <summary>
        /// Другая.
        /// </summary>
        [Description ("Другая")]
        public const string Other = "zz";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значение констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (TitleCharacterSet));
        }

        /// <summary>
        /// Получение словаря "код" - "значение".
        /// </summary>
        public static Dictionary<string, string> ListValuesWithDescriptions()
        {
            return ReflectionUtility.ListConstantValuesWithDescriptions (typeof (TitleCharacterSet));
        }

        #endregion
    }
}
