// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforL.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Вернуть окончание термина – &uf('L')
    // Вид функции: L.
    // Назначение: Вернуть окончание термина.
    // Формат (передаваемая строка):
    // L<начало_термина>
    //
    // Пример:
    //
    // &unifor('L', 'JAZ=', 'рус')
    //
    // выдаст 'СКИЙ'
    //

    static class UniforL
    {
        #region Public methods

        public static void ContinueTerm
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

            expression = expression.ToUpperInvariant();

            var provider = context.Provider;
            var parameters = new TermParameters
            {
                Database = provider.Database,
                StartTerm = expression,
                NumberOfTerms = 10
            };

            Term[]? terms;
            try
            {
                terms = provider.ReadTerms(parameters);
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        "UniforL::ContinueTerm",
                        exception
                    );

                return;
            }

            if (terms is null || terms.Length == 0)
            {
                return;
            }

            var firstTerm = terms[0].Text;
            if (string.IsNullOrEmpty(firstTerm))
            {
                return;
            }

            if (!firstTerm.StartsWith(expression))
            {
                return;
            }

            var result = firstTerm.Substring(expression.Length);
            context.WriteAndSetFlag(node, result);
        }

        #endregion
    }
}
