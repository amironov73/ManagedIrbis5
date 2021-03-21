// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforSquareBracket.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    //
    // Неописанный &unifor('[').
    // Очищает текст от команд контекстного выделения
    // (постредактура)
    //

    static class UniforSquareBracket
    {
        #region Public methods

        /// <summary>
        /// Remove the context markup commands.
        /// </summary>
        public static void CleanContextMarkup
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            context.GetRootContext().PostProcessing |= PftCleanup.ContextMarkup;
        }

        #endregion
    }
}
