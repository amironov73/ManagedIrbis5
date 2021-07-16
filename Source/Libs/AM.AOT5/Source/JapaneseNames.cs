// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* JapaneseNames.cs -- достаточно широко распространенные японские имена и фамилии
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.AOT
{
    /// <summary>
    /// Достаточно широко распространенные японские имена и фамилии.
    /// </summary>
    public static class JapaneseNames
    {
        #region Public methods

        /// <summary>
        /// Японские женские имена.
        /// </summary>
        public static string[] GetFemalePersonalNames() => System.Array.Empty<string>();

        /// <summary>
        /// Японские мужские имена.
        /// </summary>
        public static string[] GetMalePersonalNames() => System.Array.Empty<string>();

        /// <summary>
        /// Список японских фамилий.
        /// </summary>
        public static string[] GetSurnames() => new[]
            {
                "Абэ", "Акияма", "Андо", "Аоки", "Араи", "Араки", "Асано",
                "Асаяма", "Баба", "Вада", "Ватанабэ", "Гото", "Ёкота",
                "Ёкояма", "Ёсида", "Ёсикава", "Ёсимура", "Ёсимото", "Ивамото",
                "Ивасаки", "Ивата", "Игараси", "Иендо", "Иида", "Икэда",
                "Имаи", "Иноэ", "Исида", "Исий", "Исикава", "Исихара",
                "Итикава", "Ито", "Кавагути", "Каваками", "Кавамура",
                "Кавасаки", "Камата", "Канэко", "Катаяма", "Като", "Кацура",
                "Кидо", "Кикути", "Кимура", "Киносита", "Кита", "Китамура",
                "Китано", "Кобаяси", "Кодзима", "Коикэ", "Комацу", "Кондо",
                "Кониси", "Коно", "Кояма", "Кубо", "Кубота", "Кудо", "Кумагаи",
                "Курихара", "Курода", "Куроки", "Маруяма", "Масуда", "Матида",
                "Мацуда", "Мацуи", "Маэда", "Минами", "Миура", "Моримото",
                "Морита", "Мураками", "Мурата", "Нагаи", "Накаи", "Накагава",
                "Накада", "Накамура", "Накано", "Накахара", "Накаяма",
                "Нарадзаки", "Огава", "Одзава", "Окада", "Оониси", "Ооно",
                "Ояма", "Савада", "Сакаи", "Сакамото", "Сано", "Сибата",
                "Судзуки", "Тагути", "Такано", "Тамура", "Танака", "Танигава",
                "Такахаси", "Татибана", "Такэда", "Утида", "Уэда", "Уэмацу",
                "Фудзита", "Фудзии", "Фудзимото", "Фукусима", "Хара",
                "Хаттори", "Хаяси", "Хирано", "Хонда", "Хосино", "Цубаки",
                "Эномото", "Юи", "Ямада", "Ямаки", "Яманака", "Ямасаки",
                "Ямамото", "Ямамура", "Ямасита", "Ямаути", "Яно", "Ясуда"

            }; // method GetSurnames

        #endregion

    } // class JapaneseNames

} // namespace AM.AOT
