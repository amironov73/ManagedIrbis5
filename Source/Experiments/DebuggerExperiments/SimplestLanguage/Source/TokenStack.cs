// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* TokenStack.cs -- стек токенов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;


#endregion

namespace SimplestLanguage
{
    /// <summary>
    /// Стек токенов
    /// </summary>
    internal sealed class TokenStack
        : Stack<TokenKind>
    {
        #region Properties

        public TokenList Tokens { get; private set; }

        public TokenPair[] Pairs { get; private set; }

        #endregion

        #region Construction

        public TokenStack
            (
                TokenList tokens,
                TokenPair[] pairs
            )
        {
            Tokens = tokens;
            Pairs = pairs;
        }

        #endregion

        #region Public methods

        public void Pop (TokenKind current)
        {
            TokenKind open = Pop();
            TokenKind expected = Pairs.First(p => p.Open == open).Close;

            if (expected != current)
            {
                throw new SyntaxException();
            }
        }

        public void Verify()
        {
            if (Count != 0)
            {
                throw new SyntaxException();
            }
        }

        #endregion
    }
}
