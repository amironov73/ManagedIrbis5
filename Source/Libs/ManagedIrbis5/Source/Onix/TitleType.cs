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

/* TitleType.cs -- вид заглавия многочастного издания в целом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// Вид заглавия издания.
    /// плюс
    /// Вид заглавия многочастного издания в целом.
    /// </summary>
    public static class TitleType
    {
        #region Constants

        /// <summary>
        /// Не определено.
        /// </summary>
        public const string Undefined = "00";

        /// <summary>
        /// Полное заглавие издания на титульном листе или на заменяющем
        /// его элементе издания (обложке, переплете).
        /// </summary>
        public const string TitlePage = "01";

        /// <summary>
        /// Ключевое заглавие сериального издания (серии) в системе ISSN.
        /// </summary>
        public const string Serial = "02";

        /// <summary>
        /// Заглавие не языке оригинала для переводных изданий.
        /// </summary>
        public const string Original = "03";

        /// <summary>
        /// Сокращенное заглавие в форме акронима.
        /// </summary>
        public const string Acronym = "04";

        /// <summary>
        /// Сокращенное заглавие в форме аббревиатуры.
        /// </summary>
        public const string Abbreviation = "05";

        /// <summary>
        /// Заглавие издания на другом языке.
        /// </summary>
        public const string Translated = "06";

        /// <summary>
        /// Тематическое заглавие выпуска журнала.
        /// </summary>
        public const string Issue = "07";

        /// <summary>
        /// Заглавие предыдущего издания,, если оно отличается от заглавия
        /// нового издания.
        /// </summary>
        public const string PreviousEdition = "08";

        /// <summary>
        /// Альтернативное (другое) заглавие на обложке или переплете.
        /// </summary>
        public const string FrontCover = "11";

        /// <summary>
        /// Альтернативное (другое) заглавие на задней сторонке переплета или
        /// последней странице обложки.
        /// </summary>
        public const string LastCoverPage = "13";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значений констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (TitleType));
        }

        #endregion
    }
}
