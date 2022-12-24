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
using System.Text;

#endregion

#nullable enable

namespace HtmlAgilityPack;

internal class EncodingFoundException
    : Exception
{
    #region Fields

    private Encoding _encoding;

    #endregion

    #region Constructors

    internal EncodingFoundException(Encoding encoding)
    {
        _encoding = encoding;
    }

    #endregion

    #region Properties

    internal Encoding Encoding
    {
        get { return _encoding; }
    }

    #endregion
}
