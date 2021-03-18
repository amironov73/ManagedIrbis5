﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* TokenStack.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;


#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    sealed class TokenStack
        : Stack<PftTokenKind>
    {
        #region Properties

        public PftTokenList Tokens { get; private set; }

        public TokenPair[] Pairs { get; private set; }

        #endregion

        #region Construction

        public TokenStack
            (
                PftTokenList tokens,
                TokenPair[] pairs
            )
        {
            Tokens = tokens;
            Pairs = pairs;
        }

        #endregion

        #region Public methods

        public void Pop(PftTokenKind current)
        {
            PftTokenKind open = Pop();
            PftTokenKind expected = Pairs.First(p => p.Open == open).Close;

            if (expected != current)
            {
                throw new PftSyntaxException(Tokens);
            }
        }

        public void Verify()
        {
            if (Count != 0)
            {
                throw new PftSyntaxException(Tokens);
            }
        }

        #endregion
    }
}
