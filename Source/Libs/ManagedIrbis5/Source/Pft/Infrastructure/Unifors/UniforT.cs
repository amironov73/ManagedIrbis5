﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforT.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Транслитерация кириллических символов с помощью латиницы – &uf('T')
    // Вид функции: T.
    // Назначение: Транслитерация кириллических символов с помощью латиницы.
    // Формат (передаваемая строка):
    // TN<строка>
    // где N – вид таблицы транслитерации (0 или 1).
    // Примеры:
    //
    // &unifor("T0"V200)
    //
    // В оригинальном ИРБИС64:
    // * по факту таблицы 0 и 1 не различаются;
    // * буква Ё приводит к аварийному завершению скрипта.
    //

    static class UniforT
    {
        #region Private members

        private static readonly Dictionary<char, string> _transliterator = new()
            {
                {'а', "a"},  {'б', "b"},    {'в', "v"},  {'г', "g"},  {'д', "d"},
                {'е', "e"},  {'ё', "io"},   {'ж', "zh"}, {'з', "z"},  {'и', "i"},
                {'й', "i"},  {'к', "k"},    {'л', "l"},  {'м', "m"},  {'н', "n"},
                {'о', "o"},  {'п', "p"},    {'р', "r"},  {'с', "s"},  {'т', "t"},
                {'у', "u"},  {'ф', "f"},    {'х', "kh"}, {'ц', "ts"}, {'ч', "ch"},
                {'ш', "sh"}, {'щ', "shch"}, {'ь', "'"},  {'ы', "y"},  {'ъ', "\""},
                {'э', "e"},  {'ю', "iu"},   {'я', "ia"},
                {'А', "A"},  {'Б', "B"},    {'В', "V"},  {'Г', "G"},  {'Д', "D"},
                {'Е', "E"},  {'Ё', "IO"},   {'Ж', "ZH"}, {'З', "Z"},  {'И', "I"},
                {'Й', "I"},  {'К', "K"},    {'Л', "L"},  {'М', "M"},  {'Н', "N"},
                {'О', "O"},  {'П', "P"},    {'Р', "R"},  {'С', "S"},  {'Т', "T"},
                {'У', "U"},  {'Ф', "F"},    {'Х', "kh"}, {'Ц', "ts"}, {'Ч', "ch"},
                {'Ш', "sh"}, {'Щ', "shch"}, {'Ь', "'"},  {'Ы', "Y"},  {'Ъ', "\""},
                {'Э', "E"},  {'Ю', "IU"},   {'Я', "IA"}
            };

        #endregion

        #region Public methods

        /// <summary>
        /// Transliterate cyrillic to latin.
        /// </summary>
        public static void Transliterate
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            //
            // Attention: in original IRBIS &uf('T0ё') breaks the script
            //

            if (!string.IsNullOrEmpty(expression))
            {
                var result = new StringBuilder();

                for (var index = 1; index < expression.Length; index++)
                {
                    var c = expression[index];
                    if (_transliterator.TryGetValue(c, out var s))
                    {
                        result.Append(s);
                    }
                    else
                    {
                        result.Append(c);
                    }
                }

                context.WriteAndSetFlag(node, result.ToString());
            }
        }

        #endregion
    }
}

