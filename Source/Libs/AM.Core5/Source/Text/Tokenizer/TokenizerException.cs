// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* TokenizerException.cs -- exception for StringTokenizer
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Text.Tokenizer;

/// <summary>
/// Exception class for <see cref="StringTokenizer"/>.
/// </summary>
public sealed class TokenizerException
    : ArsMagnaException
{
    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public TokenizerException()
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public TokenizerException
        (
            string message
        )
        : base(message)
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public TokenizerException
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

    #endregion
}
