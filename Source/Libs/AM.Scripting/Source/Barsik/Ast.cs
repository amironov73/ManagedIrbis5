// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* Ast.cs -- абстрактное синтаксическое дерево для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Узел AST.
    /// </summary>
    abstract class AstNode
    {
        /// <summary>
        /// Выполнение действий, связанных с данным узлом.
        /// </summary>
        /// <param name="context">Контекст исполнения программы.</param>
        public virtual void Execute (Context context)
        {
        }
    }

    /// <summary>
    /// Узел, в котором происходят какие-то вычисления.
    /// </summary>
    abstract class AtomNode : AstNode
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
    sealed class ConstantNode : AtomNode
    {
        #region Construction

        public ConstantNode (dynamic? value)
        {
            _value = value;
        }

        #endregion

        #region Private members

        private readonly dynamic? _value;

        #endregion

        #region AtomNode members

        /// <inheritdoc cref="AtomNode.Compute"/>
        public override dynamic? Compute (Context context) => _value;

        #endregion

        #region Object members

        public override string ToString() => $"constant '{_value}'";

        #endregion
    }

    /// <summary>
    /// Ссылка на переменную.
    /// </summary>
    sealed class VariableNode : AtomNode
    {
        #region Private members

        private readonly string _name;

        public VariableNode (string name) => _name = name;

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

    sealed class NegationNode : AtomNode
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public NegationNode (AtomNode inner)
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
            return - _inner.Compute (context);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"negation ({_inner})";
        }

        #endregion
    }

    /// <summary>
    /// Выражение в скобках.
    /// </summary>
    sealed class ParenthesisNode : AtomNode
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ParenthesisNode (AtomNode inner)
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
    /// Бинарная операция, например, сложение или сравнение двух чисел.
    /// </summary>
    sealed class BinaryNode : AtomNode
    {
        private readonly AtomNode _left, _right;
        private readonly string _op;

        public BinaryNode (AtomNode left, AtomNode right, string op)
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
    /// Вызов функции.
    /// </summary>
    sealed class CallNode : AtomNode
    {
        #region Construction

        public CallNode(string name, IEnumerable<AtomNode>? args)
        {
            _name = name;
            _args = new List<AtomNode> ();
            if (args is not null)
            {
                _args.AddRange (args);
            }
        }

        #endregion

        #region Private members

        private readonly string _name;
        private readonly List<AtomNode> _args;
        private FunctionDescriptor? _function;

        #endregion

        #region AtomNode members

        public override dynamic? Compute (Context context)
        {
            if (_function is null)
            {
                if (!context.Functions.TryGetValue (_name, out _function))
                {
                    if (!Builtins.Registry.TryGetValue (_name, out _function))
                    {
                        throw new Exception ($"function '{_name}' not found");
                    }
                }
            }

            var args = new List<dynamic?>();
            foreach (var node in _args)
            {
                var arg = node.Compute (context);
                args.Add (arg);
            }

            var result = _function.CallPoint (args.ToArray());

            return result;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            var builder = StringBuilderPool.Shared.Get();
            builder.Append ($"function '{_name}' (");
            var first = true;
            foreach (var node in _args)
            {
                if (!first)
                {
                    builder.Append (", ");
                }

                builder.Append (node);

                first = false;
            }
            builder.Append (')');

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;
        }

        #endregion
    }

    /// <summary>
    /// Условие, состоящие из серии сравнений.
    /// </summary>
    sealed class ConditionNode : AtomNode
    {
        private readonly AtomNode _left, _right;
        private readonly string _op;

        public ConditionNode (AtomNode left, AtomNode right, string op)
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
                "and" => left && right,
                "or" => left || right,
                _ => throw new Exception ($"Unknown operator '{_op}'")
            };
        }

        public override string ToString() => $"condition ({_left} {_op} {_right})";
    }

    /// <summary>
    /// Базовый класс для стейтментов.
    /// </summary>
    class StatementNode : AstNode
    {
    }

    /// <summary>
    /// Присваивание переменной значения выражения.
    /// </summary>
    sealed class AssignmentNode : StatementNode
    {
        public string VariableName { get; }
        public AtomNode Expression { get; }

        public AssignmentNode (string variableName, AtomNode expression)
        {
            VariableName = variableName;
            Expression = expression;
        }

        public override void Execute (Context context)
        {
            context.Variables[VariableName] = Expression.Compute (context);
        }

        public override string ToString() => $"Assignment: {VariableName} = {Expression};";
    }

    /// <summary>
    /// Распечатка (дамп) значений переменных.
    /// </summary>
    sealed class PrintNode : StatementNode
    {
        private readonly List<AtomNode> _nodes;
        private readonly bool _newLine;

        public PrintNode (IEnumerable<AtomNode> collection, bool newLine)
        {
            _nodes = new List<AtomNode> (collection);
            _newLine = newLine;
        }

        private void Print (AtomNode node, Context context)
        {
            context.Output.Write (node.Compute (context));
        }

        public override void Execute (Context context)
        {
            foreach (var node in _nodes)
            {
                Print (node, context);
            }

            if (_newLine)
            {
                context.Output.WriteLine();
            }
        }

        public override string ToString()
        {
            return "Print: " + string.Join (',', _nodes);
        }
    }

    /// <summary>
    /// Условный оператор if-then-else.
    /// </summary>
    sealed class IfNode : StatementNode
    {
        #region Construction

        public IfNode
            (
                AtomNode condition,
                IEnumerable<StatementNode> thenBlock,
                IEnumerable<StatementNode>? elseBlock
            )
        {
            _condition = condition;
            _thenBlock = new (thenBlock);
            if (elseBlock is not null)
            {
                _elseBlock = new (elseBlock);
            }
        }

        #endregion

        #region Private members

        private readonly AtomNode _condition;
        private readonly List<StatementNode> _thenBlock;
        private readonly List<StatementNode>? _elseBlock;

        #endregion

        #region AstNode members

        /// <inheritdoc cref="AstNode.Execute"/>
        public override void Execute (Context context)
        {
            if (_condition.Compute (context))
            {
                foreach (var statement in _thenBlock)
                {
                    statement.Execute (context);
                }
            }
            else if (_elseBlock is not null)
            {
                foreach (var statement in _elseBlock)
                {
                    statement.Execute (context);
                }
            }
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            var builder = StringBuilderPool.Shared.Get();
            builder.AppendLine ($"If: {_condition}");
            builder.Append ("Then: ");
            foreach (var statement in _thenBlock)
            {
                builder.AppendLine (statement.ToString());
            }

            if (_elseBlock is not null)
            {
                foreach (var statement in _elseBlock)
                {
                    builder.AppendLine (statement.ToString());
                }
            }

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;
        }

        #endregion
    }

    /// <summary>
    /// Цикл while.
    /// </summary>
    sealed class WhileNode : StatementNode
    {
        #region Construction

        public WhileNode (AtomNode condition, IEnumerable<StatementNode> statements)
        {
            _condition = condition;
            _statements = new List<StatementNode> (statements);
        }

        #endregion

        #region Private members

        private readonly AtomNode _condition;
        private readonly List<StatementNode> _statements;

        #endregion

        #region AstNode members

        /// <inheritdoc cref="AstNode.Execute"/>
        public override void Execute (Context context)
        {
            while (_condition.Compute (context))
            {
                foreach (var statement in _statements)
                {
                    statement.Execute (context);
                }
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            var builder = StringBuilderPool.Shared.Get();
            builder.AppendLine ($"While: {_condition}");
            builder.Append ("Statements: ");
            foreach (var statement in _statements)
            {
                builder.AppendLine (statement.ToString());
            }

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;
        }

        #endregion
    }

    /// <summary>
    /// Корень AST.
    /// </summary>
    sealed class ProgramNode
        : AstNode
    {
        #region Properties

        /// <summary>
        /// Стейтменты программы.
        /// </summary>
        public List<StatementNode> Statements { get; }

        /// <summary>
        /// Директивы
        /// </summary>
        public List<Directive> Directives { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ProgramNode
            (
                IEnumerable<Directive>? directives,
                IEnumerable<StatementNode> statements
            )
        {
            Statements = new List<StatementNode> (statements);
            Directives = new ();
            if (directives is not null)
            {
                Directives.AddRange (directives);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Выполнение директив перед скриптом.
        /// </summary>
        internal void ExecuteDirectives
            (
                Context context
            )
        {
            foreach (var directive in Directives)
            {
                directive.Execute (context);
            }
        }

        #endregion

        #region AstNode members

        /// <inheritdoc cref="AstNode.Execute"/>
        public override void Execute (Context context)
        {
            ExecuteDirectives (context);

            foreach (var statement in Statements)
            {
                statement.Execute (context);
            }
        }

        #endregion
    }
}
