// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* Token.cs -- синтаксический токен
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace SimplestLanguage
{
    /// <summary>
    /// Синтаксический токен.
    /// </summary>
    [DebuggerDisplay("{Kind} {Text}")]
    public sealed class Token
    {
        #region Properties

        /// <summary>
        /// Вид токена.
        /// </summary>
        public TokenKind Kind { get; }

        /// <summary>
        /// Текст токена.
        /// </summary>
        public string? Text { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Token
            (
                TokenKind kind,
                string? text
            )
        {
            Kind = kind;
            Text = text;

        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Токен должен быть указанного вида.
        /// </summary>
        public Token MustBe
            (
                TokenKind required
            )
        {
            if (Kind != required)
            {
                throw new SyntaxException ($"Token must be {required}, got {Kind} {Text}");
            }

            return this;

        } // method MustBe

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"{Kind} {Text}";

        #endregion

    } // class Token

} // namespace SimplestLanguage
