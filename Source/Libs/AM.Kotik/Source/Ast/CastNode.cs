// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* CastNode.cs -- преобразование типа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Преобразование типа.
/// </summary>
public sealed class CastNode
    : PrefixNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CastNode
        (
            string typeName,
            AtomNode operand
        )
    {
        Sure.NotNull (typeName);
        Sure.NotNull (operand);

        _typeName = typeName;
        _operand = operand;
    }

    #endregion

    #region Private members

    private readonly string _typeName;
    private readonly AtomNode _operand;

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        throw new NotImplementedException();
    }

    #endregion
}
