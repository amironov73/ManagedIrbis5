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

using System.Collections.Generic;

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

            var positions = GetPositions(text, ' ');

            if (wordCount >= positions.Length)
            {
                return text;
            }

            var end = positions[wordCount];
            var result = text.Substring(0, end);

            return result;
        }

        /// <summary>
        /// Get positions of the symbol.
        /// </summary>
        internal static int[] GetPositions
            (
                string? text,
                char c
            )
        {
            var result = new List<int>();

            if (!string.IsNullOrEmpty(text))
            {
                var start = 0;
                var length = text.Length;

                while (start < length)
                {
                    var position = text.IndexOf(c, start);
                    if (position < 0)
                    {
                        break;
                    }
                    result.Add(position);
                    start = position + 1;
                }
            }

            return result.ToArray();
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
                    var text = navigator.GetRemainingText().ToString();
                    var output = GetFirstWords(text, wordCount);
                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        #endregion
    }
}
