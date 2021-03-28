// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlusP.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Text;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // ibatrak
    // Неописанная функция unifor('+P')
    // Ищет термин, выводит результат функции IrbisPosting
    // Параметры: команда,терм
    // где команда:
    // 0 - MFN
    // 1 - Tag
    // 2 - Occ
    //

    static class UniforPlusP
    {
        #region Public methods

        /// <summary>
        /// Информация о первой ссылке на указанный терм.
        /// </summary>
        public static void GetPosting
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            /*

            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            var parts = expression.Split
                (
                    CommonSeparators.Comma,
                    2
                );
            if (parts.Length != 2)
            {
                return;
            }

            var command = parts[0].FirstChar();
            var startTerm = parts[1];
            if (string.IsNullOrEmpty(startTerm))
            {
                return;
            }

            startTerm = IrbisText.ToUpper(startTerm).ThrowIfNull();
            var provider = context.Provider;
            var parameters = new TermParameters
            {
                Database = provider.Database,
                StartTerm = startTerm,
                NumberOfTerms = 1
            };
            var terms = provider.ReadTerms(parameters);
            if (terms.Length < 1)
            {
                return;
            }

            var termText = terms[0].Text.ThrowIfNull();
            if (termText != startTerm)
            {
                return;
            }

            var links = provider.ExactSearchLinks(termText);
            if (links.Length < 1)
            {
                return;
            }

            var link = links[0];
            string? output = null;
            switch (command)
            {
                //mfn
                case '0':
                    output = link.Mfn.ToInvariantString();
                    break;

                //tag
                case '1':
                    output = link.Tag.ToInvariantString();
                    break;

                //occ
                case '2':
                    output = link.Occurrence.ToInvariantString();
                    break;

                default:
                    Magna.Error
                        (
                            nameof(UniforPlusP) + "::" + nameof(GetPosting)
                          + ": unknown command=" + command
                        );
                    break;
            }

            context.WriteAndSetFlag(node, output);

            */
        }

        #endregion
    }
}
