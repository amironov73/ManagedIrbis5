// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Ast.cs -- синтаксическое дерево Barsik
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;


/// <summary>
/// Префиксная операция.
/// </summary>
sealed class PrefixNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PrefixNode
        (
            string type,
            AtomNode inner
        )
    {
        _type = type;
        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly string _type;
    private readonly AtomNode _inner;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute (Context context)
    {
        return _inner.Compute (context);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"prefix ({_type} {_inner})";
    }

    #endregion
}

/// <summary>
/// Постфиксная операция.
/// </summary>
sealed class PostfixNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PostfixNode
        (
            string type,
            AtomNode inner
        )
    {
        _type = type;
        _inner = inner;
    }

    #endregion

    #region Private members

    private readonly string _type;
    private readonly AtomNode _inner;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute (Context context)
    {
        return _inner.Compute (context);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"postfix ({_inner} {_type})";
    }

    #endregion
}
