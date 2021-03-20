// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TokenStream.cs -- поток токенов
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Text.Tokenizer
{
    /// <summary>
    /// Поток токенов.
    /// </summary>
    public sealed class TokenStream
    {
        #region Properties

        /// <summary>
        /// Current token.
        /// </summary>
        /// <remarks><c>null</c> on end of stream.</remarks>
        public Token? Current =>
            _position == _tokens.Length
                ? null
                : _tokens[_position];

        /// <summary>
        /// Has next token?
        /// </summary>
        public bool HasNext => _position + 1 < _tokens.Length;

        /// <summary>
        /// Position in the stream.
        /// </summary>
        public int Position => _position;

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

        private readonly Token[] _tokens;
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

    } // class TokenStream

} // namespace AM.Text.Tokenizer
