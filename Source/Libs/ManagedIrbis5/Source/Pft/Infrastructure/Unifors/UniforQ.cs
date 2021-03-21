// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforQ.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть заданную строку в нижнем регистре – &uf('Q')
    // Вид функции: Q.
    // Назначение: Вернуть заданную строку в нижнем регистре.
    // Формат (передаваемая строка):
    // Q<строка>
    //
    // Пример:
    //
    // &unifor("Q"v200)
    //

    /// <summary>
    ///
    /// </summary>
    public static class UniforQ
    {
        #region Public methods

        /// <summary>
        /// Convert the string to lower case.
        /// </summary>
        public static void ToLower
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {

                var output = IrbisText.ToLower(expression);

                context.WriteAndSetFlag(node, output);
            }
        }

        #endregion
    }
}
