// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AsnSyntaxException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

#endregion

#nullable enable

namespace AM.Asn1
{
    /// <summary>
    ///
    /// </summary>
    public class AsnSyntaxException
        : AsnException
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnSyntaxException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnSyntaxException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnSyntaxException
            (
                AsnToken token
            )
            : this("Unexpected token: " + token)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnSyntaxException
            (
                AsnTokenList tokenList
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
        public AsnSyntaxException
            (
                AsnTokenList tokenList,
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
        public AsnSyntaxException
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
        public AsnSyntaxException
            (
                AsnToken token,
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
        public AsnSyntaxException
            (
                TextNavigator navigator
            )
            : this("Syntax error at: " + navigator)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsnSyntaxException
            (
                AsnNode node
            )
            : this("Syntax error at: " + node)
        {
        }

        #endregion
    }
}
