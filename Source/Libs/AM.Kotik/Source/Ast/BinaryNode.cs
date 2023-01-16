// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BinaryNode.cs -- бинарная операция
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System;

namespace AM.Kotik;

/// <summary>
/// Бинарная операция, например, сложение.
/// </summary>
public sealed class BinaryNode
    : AtomNode
{
    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BinaryNode
        (
            AtomNode left,
            string operation,
            AtomNode right
        )
    {
        _left = left;
        _operation = operation;
        _right = right;
    }

    #endregion

    #region Private members

    private readonly AtomNode _left;
    private readonly string _operation;
    private readonly AtomNode _right;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic Compute
        (
            Context context
        )
    {
        dynamic? left = _left.Compute (context);
        dynamic? right = _right.Compute (context);
        dynamic? result = _operation switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => left / right,
            "%" => left % right,
            _ => throw new InvalidOperationException()
        };

        return result;
    }

    #endregion
}
