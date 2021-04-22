// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlusU.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    //
    // Неописанная функция unifor('+U')
    // Повторяет строку указанное количество раз.
    // Параметры: КОЛИЧЕСТВО#СТРОКА
    //

    static class UniforPlusU
    {
        #region Public methods

        /// <summary>
        /// ibatrak Повторяет строку указанное количество раз.
        /// </summary>
        public static void RepeatString
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

            var parts = expression.Split
                (
                    CommonSeparators.NumberSign,
                    2
                );
            if (parts.Length != 2
                || string.IsNullOrEmpty(parts[0])
                || string.IsNullOrEmpty(parts[1]))
            {
                return;
            }

            var count = parts[0].SafeToInt32();
            if (count <= 0)
            {
                return;
            }

            var text = parts[1];
            var output = new StringBuilder(count * text.Length);
            while (count > 0)
            {
                output.Append(text);
                count--;
            }
            context.WriteAndSetFlag(node, output.ToString());
        }

        #endregion
    }
}
