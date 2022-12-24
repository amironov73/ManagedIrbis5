// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* EncodingFoundException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

using AM;

#endregion

#nullable enable

namespace HtmlAgilityPack;

/// <summary>
///
/// </summary>
internal class EncodingFoundException
    : Exception
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    internal Encoding Encoding { get; }

    #endregion

    #region Constructors

    /// <summary>
    ///
    /// </summary>
    /// <param name="encoding"></param>
    internal EncodingFoundException
        (
            Encoding encoding
        )
    {
        Sure.NotNull (encoding);

        Encoding = encoding;
    }

    #endregion
}
