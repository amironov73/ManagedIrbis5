// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlusW.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.RegularExpressions;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{

    //
    // ibatrak
    //
    // Неописанная функция &unifor('+W')
    // Параметры: А#Б
    // Первый параметр отбрасывает, обрабатывает второй.
    // Похожа на unifor('+C), только ищет группу цифр в любом месте строки.
    // Если группа цифр не одна или есть минус в начале, то не делает ничего.
    // Очень странная функция.
    // Ищет группу цифр в любом месте строки.
    // Если группа цифр одна, парсит число как int32 прибавляет 1.
    // Если цифр больше, чем влазит в int32, не обрабатывает.
    // Если число переполняется, выводится с минусом.
    //

    static class UniforPlusW
    {
        #region Public methods

        /// <summary>
        /// Инкремент строки.
        /// </summary>
        public static void Increment
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            // ibatrak
            // если в строке больше, чем 1 символ #, лишние части отбрасываются
            var parts = expression.Split
                (
                    CommonSeparators.NumberSign,
                    StringSplitOptions.None
                );
            if (parts.Length < 2)
            {
                return;
            }

            if (string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]))
            {
                return;
            }

            var output = Increment(parts[1]);
            context.WriteAndSetFlag(node, output);
        }

        public static string Increment
            (
                string input
            )
        {
            input = input.ToUpperInvariant();
            var result = input;
            var regex = new Regex("(-)?([\\d]+)");
            var matches = regex.Matches(input);
            if (matches.Count == 1)
            {
                var match = matches[0];

                if (!match.Groups[1].Success
                    && Utility.TryParseInt32(match.Groups[2].Value, out var number))
                {
                    result = input.Substring(0, match.Index)
                             + unchecked (number + 1).ToInvariantString()
                             + input.Substring(match.Index + match.Length);
                }
            }

            return result;
        }

        #endregion
    }
}

