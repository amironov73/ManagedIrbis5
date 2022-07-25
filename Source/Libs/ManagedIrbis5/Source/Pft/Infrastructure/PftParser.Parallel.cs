// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftParser.Parallel.cs -- часть PFT-парсера, связанная с параллельностью
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Pft.Infrastructure.Ast;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure;

//
// Часть PFT-парсера, связанная с параллельностью.
//
partial class PftParser
{
    //=================================================

    private PftNode ParseParallel()
    {
        PftNode result;

        var nextToken = Tokens.Peek();
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
                Magna.Logger.LogError
                    (
                        nameof (PftParser) + "::" + nameof (ParseParallel)
                        + ": unexpected token {Token}",
                        nextToken
                    );

                throw new PftSyntaxException (Tokens);
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
        var result = new PftParallelFor (Tokens.Current);

        return result;
    }

    //=================================================

    private PftNode ParseParallelForEach()
    {
        var result = new PftParallelForEach (Tokens.Current);

        return result;
    }

    //=================================================

    private PftNode ParseParallelGroup()
    {
        var result = new PftParallelGroup (Tokens.Current);

        if (_inGroup)
        {
            Magna.Logger.LogError
                (
                    nameof (PftParser) + "::" + nameof (ParseParallelGroup)
                    + ": nested group detected"
                );

            throw new PftSyntaxException ("no nested group enabled");
        }

        try
        {
            _inGroup = true;

            Tokens.RequireNext();
            ParseCall2 (result);
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
        var result = new PftParallelWith (Tokens.Current);

        return result;
    }
}
