// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BinaryNode.cs -- бинарная инфиксная операция
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Бинарная инфиксная операция, например, сложение.
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

    /// <summary>
    /// Оператор сравнения.
    /// </summary>
    private static dynamic? Shuttle
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        return OmnipotentComparer.Default.Compare (left, right);
    }

    /// <summary>
    /// Расширенная операция "В".
    /// </summary>
    /// <param name="context">Контекст.</param>
    /// <param name="left">Что ищем.</param>
    /// <param name="right">Где ищем.</param>
    private static dynamic? In
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        if (left is null || right is null)
        {
            return false;
        }

        if (right is string text)
        {
            if (left is char chr)
            {
                return text.Contains (chr);
            }

            if (left is string sub)
            {
                return text.Contains (sub);
            }
        }

        if (right is Array array)
        {
            return Array.IndexOf (array, (object) left) >= 0;
        }

        if (right is IDictionary dictionary)
        {
            return dictionary.Contains ((object) left);
        }

        if (right is IList list)
        {
            return list.Contains ((object) left);
        }

        return false;
    }

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

        var result = _operation switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => left / right,
            "%" => left % right,
            "<" => left < right,
            ">" => left > right,
            "<=" => left <= right,
            ">=" => left >= right,
            "==" => left == right,
            "!=" => left != right,
            "||" => left || right,
            "&&" => left && right,
            "in" => In (context, left, right),
            "<=>" => Shuttle (context, left, right),
            _ => throw new InvalidOperationException()
        };

        // context.Output.WriteLine ($"Compute {left} {_operation} {right} = {result}");

        return result;
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter,string?)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer, ToString());

        _left.DumpHierarchyItem ("Left", level + 1, writer);
        DumpHierarchyItem ("Op", level + 1, writer, _operation);
        _right.DumpHierarchyItem ("Right", level + 1, writer);
    }

    #endregion
}
