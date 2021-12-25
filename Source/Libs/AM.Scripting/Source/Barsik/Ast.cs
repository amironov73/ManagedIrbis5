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
/// Абстрактный базовый узел.
/// </summary>
public class AstNode
{
    // пока в классе нет членов
}

/// <summary>
/// Узел, в котором происходят какие-то вычисления.
/// </summary>
public abstract class AtomNode
    : AstNode
{
    #region Public methods

    /// <summary>
    /// Вычисление значения, связанного с данным узлом.
    /// </summary>
    public abstract dynamic? Compute (Context context);

    #endregion
}

/// <summary>
/// Констатное значение.
/// </summary>
sealed class ConstantNode
    : AtomNode
{
    #region Construction

    public ConstantNode
        (
            object? value
        )
    {
        _value = value;
    }

    #endregion

    #region Private members

    private readonly object? _value;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        return _value;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"constant '{_value}'";
    }

    #endregion
}

/// <summary>
/// Бинарная операция, например, сложение или сравнение двух чисел.
/// </summary>
sealed class BinaryNode
    : AtomNode
{
    private readonly AtomNode _left, _right;
    private readonly string _op;

    public BinaryNode
        (
            AtomNode left,
            AtomNode right, string op
        )
    {
        _left = left;
        _right = right;
        _op = op;
    }

    public override dynamic Compute (Context context)
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
            "^" => left ^ right,
            "<" => left < right,
            "<=" => left <= right,
            ">" => left > right,
            ">=" => left >= right,
            "==" => left == right,
            "!=" => left != right,
            _ => throw new Exception ($"Unknown operation '{_op}'")
        };
    }

    public override string ToString() => $"binary ({_left} {_op} {_right})";
}

/// <summary>
/// Ссылка на переменную.
/// </summary>
sealed class VariableNode
    : AtomNode
{
    #region Private members

    private readonly string _name;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public VariableNode
        (
            string name
        )
    {
        _name = name;
    }

    #endregion

    #region AtomNode members

    public override dynamic? Compute (Context context)
    {
        if (context.Variables.TryGetValue (_name, out var value))
        {
            return value;
        }

        Console.WriteLine ($"Variable '{_name}' not defined");

        return null;
    }

    #endregion

    #region Object members

    public override string ToString() => $"variable '{_name}'";

    #endregion
}

/// <summary>
/// Базовый класс для стейтментов.
/// </summary>
public class StatementNode
    : AstNode
{
    #region Private members

    /// <summary>
    /// Перед выполнением стейтмента.
    /// </summary>
    protected virtual void PreExecute
        (
            Context context
        )
    {
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Выполнение действий, связанных с данным узлом.
    /// </summary>
    /// <param name="context">Контекст исполнения программы.</param>
    public virtual void Execute
        (
            Context context
        )
    {
        PreExecute (context);
    }

    #endregion
}

/// <summary>
/// Присваивание переменной результата вычисления выражения.
/// </summary>
sealed class AssignmentNode
    : StatementNode
{
    #region Construction

    public AssignmentNode
        (
            string target,
            AtomNode expression
        )
    {
        _target = target;
        _expression = expression;
    }

    #endregion

    #region Private members

    private readonly string _target;
    private readonly AtomNode _expression;

    #endregion

    #region StatementNode members

    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        var variableName = _target;
        var computedValue = _expression.Compute (context);

        context.SetVariable (variableName, computedValue);

        BarsikUtility.PrintObject (context.Output, computedValue);
        context.Output.WriteLine ();
    }

    #endregion

    #region Object members

    public override string ToString()
    {
        return $"Assignment: {_target} = {_expression};";
    }

    #endregion
}

/// <summary>
/// Выражение в скобках.
/// </summary>
sealed class ParenthesisNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ParenthesisNode
        (
            AtomNode inner
        )
    {
        _inner = inner;
    }

    #endregion

    #region Private members

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
        return $"parenthesis ({_inner})";
    }

    #endregion
}

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

/// <summary>
/// Корневой узел AST.
/// </summary>
public sealed class ProgramNode
    : AstNode
{
    #region Properties

    /// <summary>
    /// Стейтменты программы.
    /// </summary>
    public List<StatementNode> Statements { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ProgramNode
        (
            IEnumerable<StatementNode> statements
        )
    {
        Statements = new List<StatementNode> (statements);
    }

    #endregion

    #region Private methods

    #endregion

    #region AstNode members

    /// <summary>
    /// Выполнение действий, предусмотренных данной программой.
    /// </summary>
    /// <param name="context">Контекст исполнения программы.</param>
    public void Execute (Context context)
    {
        foreach (var statement in Statements)
        {
            context.Output.WriteLine (statement);
            statement.Execute (context);
        }
    }

    #endregion
}
