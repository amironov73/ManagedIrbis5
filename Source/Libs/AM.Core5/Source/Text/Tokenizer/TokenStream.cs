﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* TokenStream.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace AM.Text.Tokenizer
{
    /// <summary>
    /// Stream of tokens.
    /// </summary>
    public sealed class TokenStream
    {
        #region Properties

        /// <summary>
        /// Current token.
        /// </summary>
        /// <remarks><c>null</c> on end of stream.</remarks>
        public Token? Current
        {
            get
            {
                return _position == _tokens.Length
                    ? null
                    : _tokens[_position];
            }
        }

        /// <summary>
        /// Has next token?
        /// </summary>
        public bool HasNext
        {
            get { return _position + 1 < _tokens.Length; }
        }

        /// <summary>
        /// Position in the stream.
        /// </summary>
        public int Position { get { return _position; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TokenStream
            (
                string[] tokens
            )
            : this(Token.Convert(tokens))
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TokenStream
            (
                Token[] tokens
            )
        {
            _tokens = tokens;
        }

        #endregion

        #region Private members

        private Token[] _tokens;
        private int _position;

        #endregion

        #region Public methods

        /// <summary>
        /// Move to next token.
        /// </summary>
        public bool MoveNext()
        {
            if (_position == _tokens.Length)
            {
                return false;
            }

            _position++;
            if (_position == _tokens.Length)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Peek the next token.
        /// </summary>
        public Token? Peek()
        {
            if (_position + 1 >= _tokens.Length)
            {
                return null;
            }

            return _tokens[_position + 1];
        }

        #endregion
    }
}
