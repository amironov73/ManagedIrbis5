// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* PftSyntaxException.cs -- исключение, возникающее при разборе PFT-скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

using ManagedIrbis.Pft.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Исключение, возникающее при разборе PFT-скрипта.
    /// </summary>
    public sealed class PftSyntaxException
        : PftException
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                PftToken token
            )
            : this("Unexpected token: " + token)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                PftTokenList tokenList
            )
            : this
                (
                  "Unexpected end of file:"
                  + tokenList.ShowLastTokens(3)
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                PftTokenList tokenList,
                Exception innerException
            )
            : this
                (
                    "Unexpected end of file: "
                        + tokenList.ShowLastTokens(3),
                    innerException
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                string message,
                Exception innerException
            )
            : base
                (
                    message,
                    innerException
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                PftToken token,
                Exception innerException
            )
            : this
                (
                    "Unexpected token: " + token,
                    innerException
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                TextNavigator navigator
            )
            : this("Syntax error at: " + navigator)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftSyntaxException
            (
                PftNode node
            )
            : this("Syntax error at: " + node)
        {
        }

        #endregion

    } // class PftSyntaxException

} // namespace ManagedIrbis.Pft
