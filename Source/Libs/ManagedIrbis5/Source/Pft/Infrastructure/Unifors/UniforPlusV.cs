// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlusV.cs --
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
    // ibatrak
    //
    // Неописанная функция unifor('+V')
    //
    // Выводит подстроку указанной длины.
    // Параметры: ДЛИНА#СТРОКА
    //

    static class UniforPlusV
    {
        #region Public methods

        /// <summary>
        /// ibatrak Выводит подстроку указанной длины
        /// </summary>
        public static void Substring
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
               || string.IsNullOrEmpty(parts[1]))
            {
                return;
            }

            var length = parts[0].SafeToInt32();
            if (length <= 0)
            {
                return;
            }

            var output = parts[1].SafeSubstring(0, length);
            context.WriteAndSetFlag(node, output);
        }

        #endregion
    }
}
