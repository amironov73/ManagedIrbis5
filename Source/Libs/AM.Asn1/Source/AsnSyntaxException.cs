// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* AsnSyntaxException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

#endregion

#nullable enable

namespace AM.Asn1;

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
        // пустое тело конструктора
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public AsnSyntaxException
        (
            string message
        )
        : base (message)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public AsnSyntaxException
        (
            AsnToken token
        )
        : this ("Unexpected token: " + token)
    {
        // пустое тело конструктора
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
                + tokenList.ShowLastTokens (3)
            )
    {
        // пустое тело конструктора
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
                + tokenList.ShowLastTokens (3),
                innerException
            )
    {
        // пустое тело конструктора
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
        // пустое тело конструктора
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
        // пустое тело конструктора
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public AsnSyntaxException
        (
            TextNavigator navigator
        )
        : this ("Syntax error at: " + navigator)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public AsnSyntaxException
        (
            AsnNode node
        )
        : this ("Syntax error at: " + node)
    {
        // пустое тело конструктора
    }

    #endregion
}
