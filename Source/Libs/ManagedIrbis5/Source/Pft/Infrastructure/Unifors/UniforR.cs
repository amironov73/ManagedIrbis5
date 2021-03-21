// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforR.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Генерация случайного числа – &uf('R')
    // Вид функции: R.
    // Назначение: Генерация случайного числа.
    // Формат (передаваемая строка):
    // RNN
    // где NN – кол-во знаков в случайном числе (по умолчанию – 6).
    //
    // Примеры:
    //
    // &unifor('R')
    // &unifor('R4')
    //

    static class UniforR
    {
        #region Public methods

        /// <summary>
        /// Generate random number.
        /// </summary>
        public static void RandomNumber
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var length = 6;
            if (!string.IsNullOrEmpty(expression))
            {
                Utility.TryParseInt32(expression, out length);
            }
            if (length <= 0 || length > 9)
            {
                return;
            }

            var maxValue = 1;
            for (var i = 0; i < length; i++)
            {
                maxValue = maxValue * 10;
            }

            var result = context.Provider
                .PlatformAbstraction
                .GetRandomGenerator()
                .Next(maxValue);
            var format = new string('0', length);
            var output = result.ToString(format, CultureInfo.InvariantCulture);
            context.WriteAndSetFlag(node, output);
        }

        #endregion
    }
}
