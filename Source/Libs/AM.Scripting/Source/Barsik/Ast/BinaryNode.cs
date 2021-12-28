// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* BinaryNode.cs -- бинарная операция, например, сложение или сравнение двух чисел
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Бинарная операция, например, сложение или сравнение двух чисел.
/// </summary>
internal sealed class BinaryNode : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BinaryNode
        (
            AtomNode left,
            AtomNode right,
            string op
        )
    {
        _left = left;
        _right = right;
        _op = op;
    }

    #endregion

    #region Private members

    private readonly AtomNode _left, _right;
    private readonly string _op;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic Compute
        (
            Context context
        )
    {
        var left = _left.Compute (context);
        var right = _right.Compute (context);

        return _op switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => left / right,
            "%" => left % right,
            "<<" => left << right,
            "<" => left < right,
            "<=" => left <= right,
            ">>" => left >> right,
            ">" => left > right,
            ">=" => left >= right,
            "==" => left == right,
            "!=" => left != right,
            "||" or "or" => BarsikUtility.ToBoolean (left) || BarsikUtility.ToBoolean (right),
            "|" => left | left,
            "&&" or "and" => BarsikUtility.ToBoolean (left) && BarsikUtility.ToBoolean (right),
            "&" => left & right,
            "^^" => BarsikUtility.ToBoolean (left) != BarsikUtility.ToBoolean (right),
            "^" => left ^ right,
            _ => throw new Exception ($"Unknown operation '{_op}'")
        };
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"binary ({_left} {_op} {_right})";
    }

    #endregion
}
