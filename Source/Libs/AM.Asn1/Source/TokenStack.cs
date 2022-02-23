// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TokenStack.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Asn1;

internal sealed class TokenStack
    : Stack<AsnTokenKind>
{
    #region Properties

    public AsnTokenList Tokens { get; }

    public TokenPair[] Pairs { get; }

    #endregion

    #region Construction

    public TokenStack
        (
            AsnTokenList tokens,
            TokenPair[] pairs
        )
    {
        Tokens = tokens;
        Pairs = pairs;
    }

    #endregion

    #region Public methods

    public void Pop (AsnTokenKind current)
    {
        AsnTokenKind open = Pop();
        AsnTokenKind expected = Pairs.First (p => p.Open == open).Close;

        if (expected != current)
        {
            throw new AsnSyntaxException (Tokens);
        }
    }

    public void Verify()
    {
        if (Count != 0)
        {
            throw new AsnSyntaxException (Tokens);
        }
    }

    #endregion
}
