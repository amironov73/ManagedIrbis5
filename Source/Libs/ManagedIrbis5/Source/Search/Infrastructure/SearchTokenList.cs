// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SearchTokenList.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// List of tokens.
    /// </summary>
    public sealed class SearchTokenList
    {
        #region Properties

        /// <summary>
        /// Current token.
        /// </summary>
        internal SearchToken Current
        {
            get { return _tokens[_position]; }
        }

        /// <summary>
        /// EOF reached?
        /// </summary>
        internal bool IsEof { get { return _position >= _tokens.Length; } }

        /// <summary>
        /// How many tokens?
        /// </summary>
        public int Length { get { return _tokens.Length; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        internal SearchTokenList
            (
                IEnumerable<SearchToken> tokens
            )
        {
            _tokens = tokens.ToArray();
            _position = 0;
        }

        #endregion

        #region Private members

        private int _position;

        private readonly SearchToken[] _tokens;

        #endregion

        #region Public methods

        /// <summary>
        /// Move to next token.
        /// </summary>
        internal bool MoveNext()
        {
            _position++;

            return _position < _tokens.Length;
        }

        /// <summary>
        /// Require next token.
        /// </summary>
        internal SearchTokenList RequireNext()
        {
            if (!MoveNext())
            {
                Magna.Error
                    (
                        "SearchTokenList::RequireNext"
                    );

                throw new SearchSyntaxException();
            }

            return this;
        }

        /// <summary>
        /// Require next token.
        /// </summary>
        internal SearchTokenList RequireNext
            (
                SearchTokenKind tokenKind
            )
        {
            if (!MoveNext())
            {
                Magna.Error
                    (
                        nameof(SearchTokenList) + "::" + nameof(RequireNext)
                        + ": unexpected end of stream"
                    );

                throw new SearchSyntaxException();
            }

            if (Current.Kind != tokenKind)
            {
                Magna.Error
                    (
                        nameof(SearchTokenList) + "::" + nameof(RequireNext)
                        + ": expected="
                        + tokenKind
                        + ", got="
                        + Current.Kind
                    );

                throw new SearchSyntaxException();
            }

            return this;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
            => IsEof ? "(EOF)" : Current.ToString();

        #endregion
    }
}
