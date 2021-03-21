// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforColon.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Неописанный UNIFOR, используемый при выводе полных текстов
    // в right_ft.pft

    static class UniforColon
    {
        #region Public methods

        public static void CheckRights
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

            var output = "0";
            context.WriteAndSetFlag(node, output);
        }

        #endregion
    }
}
