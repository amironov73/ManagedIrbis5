// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforE.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть заданное количество слов с начала строки – &uf('E')
    // Вид функции: E.
    // Назначение: Вернуть заданное количество слов с начала строки.
    // Формат (передаваемая строка):
    // EN<строка>
    // где N – количество слов (одна цифра).
    //
    // Пример:
    //
    // &unifor("E3"v200^a)
    //

    static class UniforE
    {
        #region Private members

        internal static string GetFirstWords
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

            // ibatrak через ISISACW.TAB делать смысла нет
            // irbis64 ищет одиночные пробелы

            int[] positions = text.GetPositions(' ');

            if (wordCount >= positions.Length)
            {
                return text;
            }

            var end = positions[wordCount];
            var result = text.Substring(0, end);

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Первые N слов в строке.
        /// </summary>
        public static void GetFirstWords
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
                if (countText == "0")
                {
                    countText = "10";
                }

                if (Utility.TryParseInt32(countText, out var wordCount))
                {
                    string text = navigator.GetRemainingText();
                    var output = GetFirstWords(text, wordCount);
                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        #endregion
    }
}
