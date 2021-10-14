// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* TokenList.cs -- список токенов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace SimplestLanguage
{
    /// <summary>
    /// Список токенов.
    /// </summary>
    public sealed class TokenList
    {
        #region Properties

        /// <summary>
        /// Текущий токен.
        /// </summary>
        public Token Current
        {
            get
            {
                Token result;
                try
                {
                    result = _tokens[_position];
                }
                catch (Exception exception)
                {
                    throw new SyntaxException ("Syntax error", exception);
                }

                return result;
            }
        }

        /// <summary>
        /// Достигнут конец списка?
        /// </summary>
        public bool IsEof => _position >= _tokens.Length;

        /// <summary>
        /// Количество токенов в списке.
        /// </summary>
        public int Length => _tokens.Length;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public TokenList
            (
                IEnumerable<Token> tokens
            )
        {
            _tokens = tokens.ToArray();
            _position = 0;
        }

        #endregion

        #region Private members

        private int _position;
        private Token[] _tokens;

        #endregion

        #region Public methods

        /// <summary>
        /// Add a token.
        /// </summary>
        public void Add
            (
                TokenKind kind
            )
        {
            var token = new Token (kind);
            var tokens = new List<Token> (_tokens)
            {
                token
            };
            _tokens = tokens.ToArray();
        }

        /// <summary>
        /// Return number of remaining tokens.
        /// </summary>
        public int CountRemainingTokens()
        {
            var length = _tokens.Length;
            if (_position >= length)
            {
                return 0;
            }

            return _tokens.Length - _position - 1;
        }

        /// <summary>
        /// Dump token list.
        /// </summary>
        public void Dump
            (
                TextWriter writer
            )
        {
            writer.WriteLine
                (
                    "Total tokens: {0}",
                    _tokens.Length
                );
            foreach (var token in _tokens)
            {
                writer.WriteLine(token);
            }
        }

        /// <summary>
        /// Move to next token.
        /// </summary>
        public bool MoveNext()
        {
            _position++;

            var result = _position < _tokens.Length;

            if (!result)
            {
                Magna.Trace
                    (
                        "PftTokenList::MoveNext: "
                        + "end of list"
                    );
            }

            return result;
        }

        /// <summary>
        /// Peek next token.
        /// </summary>
        public TokenKind Peek()
        {
            var newPosition = _position + 1;
            if (newPosition >= _tokens.Length)
            {
                Magna.Trace
                    (
                        "PftTokenList::Peek: "
                        + "end of list"
                    );

                return TokenKind.None;
            }

            return _tokens[newPosition].Kind;
        }

        /// <summary>
        /// Peek token at arbitrary position.
        /// </summary>
        public TokenKind Peek
            (
                int delta
            )
        {
            var newPosition = _position + delta;
            if (newPosition < 0
                || newPosition >= _tokens.Length)
            {
                Magna.Trace
                    (
                        "PftTokenList::Peek: "
                        + "end of list"
                    );

                return TokenKind.None;
            }

            return _tokens[newPosition].Kind;
        }

        /// <summary>
        /// Require next token.
        /// </summary>
        public TokenList RequireNext()
        {
            if (!MoveNext())
            {
                Magna.Error
                    (
                        "PftTokenList::RequireNext: "
                        + "no next token"
                    );

                throw new SyntaxException(Current.ToString());
            }

            return this;
        }

        /// <summary>
        /// Require next token.
        /// </summary>
        public TokenList RequireNext
            (
                TokenKind kind
            )
        {
            RequireNext();
            if (Current.Kind != kind)
            {
                Magna.Error
                    (
                        "PftTokenList::RequireNext: "
                        + "expected="
                        + kind
                        + ", got="
                        + Current.Kind
                    );

                throw new SyntaxException(Current.ToString());
            }

            return this;
        }

        /// <summary>
        /// Move to begin of the list.
        /// </summary>
        public TokenList Reset()
        {
            _position = 0;

            return this;
        }

        /// <summary>
        /// Restore position.
        /// </summary>
        public TokenList RestorePosition
            (
                int position
            )
        {
            _position = position;

            return this;
        }

        /// <summary>
        /// Save position.
        /// </summary>
        public int SavePosition()
        {
            return _position;
        }

        /// <summary>
        /// Get segment (span) of the token list.
        /// </summary>
        public TokenList? Segment
            (
                TokenKind[] stop
            )
        {
            var savePosition = _position;
            var foundPosition = -1;

            while (!IsEof)
            {
                var current = Current.Kind;

                if (stop.Contains(current))
                {
                    foundPosition = _position;
                    break;
                }

                MoveNext();
            }

            if (foundPosition < 0)
            {
                _position = savePosition;

                return null;
            }

            var tokens = new List<Token>();
            for (
                    var position = savePosition;
                    position < _position;
                    position++
                )
            {
                tokens.Add(_tokens[position]);
            }

            var result = new TokenList(tokens);

            return result;
        }

        /// <summary>
        /// Get segment (span) of the token list.
        /// </summary>
        internal TokenList? Segment
            (
                TokenPair[] pairs,
                TokenKind[] open,
                TokenKind[] close,
                TokenKind[] stop
            )
        {
            var savePosition = _position;
            var foundPosition = -1;

            var stack = new TokenStack(this, pairs);
            while (!IsEof)
            {
                var current = Current.Kind;

                if (open.Contains(current))
                {
                    stack.Push(current);
                }
                else if (close.Contains(current))
                {
                    if (stack.Count == 0)
                    {
                        if (stop.Contains(current))
                        {
                            foundPosition = _position;
                            break;
                        }
                    }
                    stack.Pop(current);
                }
                else if (stop.Contains(current))
                {
                    if (stack.Count == 0)
                    {
                        foundPosition = _position;
                        break;
                    }
                }

                MoveNext();
            }

            stack.Verify();
            if (foundPosition < 0)
            {
                Magna.Trace
                    (
                        "PftTokenList::Segment: "
                        + "not found"
                    );

                _position = savePosition;

                return null;
            }

            var tokens = new List<Token>();
            for (
                    var position = savePosition;
                    position < _position;
                    position++
                )
            {
                tokens.Add(_tokens[position]);
            }

            var result = new TokenList(tokens);

            return result;
        }

        /// <summary>
        /// Get segment (span) of the token list.
        /// </summary>
        public TokenList? Segment
            (
                TokenKind[] open,
                TokenKind[] close,
                TokenKind[] stop
            )
        {
            var savePosition = _position;
            var foundPosition = -1;

            var level = 0;
            while (!IsEof)
            {
                var current = Current.Kind;

                if (open.Contains(current))
                {
                    level++;
                }
                else if (close.Contains(current))
                {
                    if (level == 0)
                    {
                        if (stop.Contains(current))
                        {
                            foundPosition = _position;
                            break;
                        }
                    }
                    level--;
                }
                else if (stop.Contains(current))
                {
                    if (level == 0)
                    {
                        foundPosition = _position;
                        break;
                    }
                }

                MoveNext();
            }

            if (level != 0)
            {
                Magna.Error
                (
                    "PftTokenList::Segment: "
                    + "unbalanced="
                    + level
                );

                throw new SyntaxException();
            }
            if (foundPosition < 0)
            {
                Magna.Trace
                    (
                        "PftTokenList::Segment: "
                        + "not found"
                    );

                _position = savePosition;

                return null;
            }

            var tokens = new List<Token>();
            for (
                    var position = savePosition;
                    position < _position;
                    position++
                )
            {
                tokens.Add(_tokens[position]);
            }

            var result = new TokenList(tokens);

            return result;
        }

        /// <summary>
        /// Show last tokens.
        /// </summary>
        public string ShowLastTokens
            (
                int howMany
            )
        {
            var result = new StringBuilder();
            var index = _position - howMany;
            if (index < 0)
            {
                index = 0;
            }
            var first = true;
            while (index < Length)
            {
                if (!first)
                {
                    result.Append(' ');
                }
                result.Append(_tokens[index]);

                index++;
                first = false;
            }

            return result.ToString();
        }

        /// <summary>
        /// Get array of tokens.
        /// </summary>
        public Token[] ToArray()
        {
            return _tokens;
        }

        /// <summary>
        /// Convert token list to text.
        /// </summary>
        public string ToText()
        {
            var builder = StringBuilderPool.Shared.Get();

            foreach (var token in _tokens)
            {
                builder.Append (token.Text);
            }

            return builder.ToString();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => IsEof ? "(EOF)" : $"{_position} of {_tokens.Length}: {Current}";

        #endregion

    } // class TokenList

} // namespace SimplestLanguage
