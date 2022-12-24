// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace HtmlAgilityPack;

/// <summary>
/// Represents an exception thrown by the HtmlWeb utility class.
/// </summary>
public class HtmlWebException
    : Exception
{
    #region Constructors

    /// <summary>
    /// Creates an instance of the HtmlWebException.
    /// </summary>
    /// <param name="message">The exception's message.</param>
    public HtmlWebException(string message)
        : base(message)
    {
    }

    #endregion
}
