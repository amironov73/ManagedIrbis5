// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftParser.Parallel.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

using ManagedIrbis.Pft.Infrastructure.Ast;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    partial class PftParser
    {

        //=================================================

        private PftNode ParseParallel()
        {
            PftNode result;

            PftTokenKind nextToken = Tokens.Peek();
            switch (nextToken)
            {
                case PftTokenKind.LeftParenthesis:
                    result = ParseParallelGroup();
                    break;

                case PftTokenKind.For:
                    result = ParseParallelFor();
                    break;

                case PftTokenKind.ForEach:
                    result = ParseParallelForEach();
                    break;

                case PftTokenKind.With:
                    result = ParseParallelWith();
                    break;

                default:
                    Magna.Error
                        (
                            "PftParser::ParseParallel: "
                            + "unexpected token="
                            + nextToken
                        );

                    throw new PftSyntaxException(Tokens);
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// For loop.
        /// </summary>
        /// <example>
        /// parallel for $x=0; $x &lt; 10; $x = $x+1;
        /// do
        ///     $x, ') ',
        ///     'Прикольно же!'
        ///     #
        /// end
        /// </example>
        private PftNode ParseParallelFor()
        {
            PftParallelFor result = new PftParallelFor(Tokens.Current);

            return result;
        }

        //=================================================

        private PftNode ParseParallelForEach()
        {
            PftParallelForEach result = new PftParallelForEach(Tokens.Current);

            return result;
        }

        //=================================================

        private PftNode ParseParallelGroup()
        {
            PftParallelGroup result = new PftParallelGroup(Tokens.Current);

            if (_inGroup)
            {
                Magna.Error
                    (
                        "PftParser::ParseParallelGroup: "
                        + "nested group detected"
                    );

                throw new PftSyntaxException("no nested group enabled");
            }

            try
            {
                _inGroup = true;

                Tokens.RequireNext();
                ParseCall2(result);
            }
            finally
            {
                _inGroup = false;
            }

            return result;
        }

        //=================================================

        private PftNode ParseParallelWith()
        {
            PftParallelWith result = new PftParallelWith(Tokens.Current);

            return result;
        }

    }
}
