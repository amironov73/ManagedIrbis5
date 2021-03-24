// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforF.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.RegularExpressions;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть часть строки, начиная со следующего слова
    // после указанного и до конца строки – &uf('F')
    // Вид функции: F.
    // Назначение: Вернуть часть строки, начиная со следующего
    // слова после указанного и до конца строки.
    // Формат (передаваемая строка):
    // FN<строка>
    // где N – количество слов (одна цифра).
    //
    // Пример:
    //
    // &unifor("F3"v200^a)
    //

    static class UniforF
    {
        #region Private members

        static string GetLastWords
            (
                string? text,
                int wordCount
            )
        {
            if (string.IsNullOrEmpty(text)
                || wordCount <= 0)
            {
                return string.Empty;
            }

            wordCount--;

            // TODO Use IrbisAlphabetTable?

            // ibatrak через ISISACW.TAB делать смысла нет
            // irbis64 ищет одиночные пробелы

            int[] positions = UniforE.GetPositions(text, ' ');

            if (wordCount >= positions.Length)
            {
                return text;
            }

            var end = positions[wordCount];
            var result = text.Substring
                (
                    end,
                    text.Length - end
                );

            return result;
        }


        #endregion

        #region Public methods

        /// <summary>
        /// Часть строки после N первых слов.
        /// </summary>
        public static void GetLastWords
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var navigator = new TextNavigator(expression);
                var countText = navigator.ReadChar().ToString();

                if (Utility.TryParseInt32(countText, out var wordCount))
                {
                    string text = navigator.GetRemainingText().ToString();
                    var output = GetLastWords(text, wordCount);
                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        #endregion
    }
}
