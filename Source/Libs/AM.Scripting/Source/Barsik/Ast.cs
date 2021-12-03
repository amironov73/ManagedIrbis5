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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
    /// Обращение к свойству объекта.
    /// </summary>
    sealed class PropertyNode : AtomNode
    {
        #region Construction

        public PropertyNode (string objectName, string propertyName)
        {
            _objectName = objectName;
            _propertyName = propertyName;
        }

        #endregion

        #region Private members

        private readonly string _objectName;
        private readonly string _propertyName;

        private dynamic? ComputeStatic (Context context)
        {
            var type = context.FindType (_objectName);
            if (type is null)
            {
                context.Error.WriteLine ($"Variable or type {_objectName} not found");

                return null;
            }

            var property = type.GetProperty (_propertyName);
            if (property is not null)
            {
                return property.GetValue (null);
            }

            var field = type.GetField (_propertyName);
            if (field is not null)
            {
                return field.GetValue (null);
            }

            return null;
        }

        #endregion

        #region AtomNode members

        public override dynamic? Compute (Context context)
        {
            if (!context.TryGetVariable (_objectName, out var obj))
            {
                return ComputeStatic (context);
            }

            if (obj is null)
            {
                return null;
            }

            var type = ((object) obj).GetType();
            var property = type.GetProperty (_propertyName);
            if (property is not null)
            {
                return property.GetValue (obj);
            }

            var field = type.GetField (_propertyName);
            if (field is not null)
            {
                return field.GetValue (obj);
            }

            return null;
        }

        #endregion
    }

    /// <summary>
    /// Вызов функции-члена.
    /// </summary>
    sealed class MethodNode : AtomNode
    {
        #region Construction

        public MethodNode(string thisName, string methodName, IEnumerable<AtomNode>? arguments)
        {
            _thisName = thisName;
            _methodName = methodName;
            _arguments = new ();
            if (arguments is not null)
            {
                _arguments.AddRange (arguments);
            }
        }

        #endregion

        #region Private members

        private readonly string _thisName;
        private readonly string _methodName;
        private readonly List<AtomNode> _arguments;

        private dynamic? ComputeStatic (Context context)
        {
            var type = context.FindType (_thisName);
            if (type is null)
            {
                context.Error.WriteLine ($"Variable or type {_thisName} not found");

                return null;
            }

            var method = type
                    .GetMethods (BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault (info => info.Name == _methodName
                        && info.GetParameters().Length == _arguments.Count);

            if (method is null)
            {
                return null;
            }

            var parameters = new List<object?>();
            foreach (var argument in _arguments)
            {
                var parameter = (object?) argument.Compute (context);
                parameters.Add (parameter);
            }

            var result = method.Invoke (null, parameters.ToArray());

            return result;
        }

        #endregion

        #region AtomNode members

        public override dynamic? Compute (Context context)
        {
            if (!context.TryGetVariable (_thisName, out var thisValue))
            {
                return ComputeStatic (context);
            }

            if (thisValue is null)
            {
                return null;
            }

            var argumentValues = new List<dynamic>();
            var argumentTypes = new List<Type>();
            foreach (var argument in _arguments)
            {
                var value = argument.Compute (context);
                argumentValues.Add (value);
                var argType = value is null
                    ? typeof (object)
                    : value.GetType();
                argumentTypes.Add (argType);
            }

            var type = ((object) thisValue).GetType();
            var method = type.GetMethod (_methodName, argumentTypes.ToArray());
            method ??= type.GetMethod (_methodName);
            if (method is null)
            {
                return null;
            }

            var result = method.Invoke (thisValue, argumentValues.ToArray());

            return result;
        }

        #endregion
    }

    /// <summary>
    /// Оператор new.
    /// </summary>
    sealed class NewNode : AtomNode
    {
        public NewNode(string typeName, IEnumerable<AtomNode>? arguments)
        {
            _typeName = typeName;
            _arguments = new ();
            if (arguments is not null)
            {
                _arguments.AddRange (arguments);
            }
        }

        private readonly string _typeName;
        private readonly List<AtomNode> _arguments;

        public override dynamic? Compute (Context context)
        {
            var type = context.FindType (_typeName);
            if (type is null)
            {
                context.Error.WriteLine ($"Type {_typeName} not found");
                return null;
            }

            object? result;
            if (_arguments.Count == 0)
            {
                result = Activator.CreateInstance (type);
            }
            else
            {
                var parameters = new List<object?>();
                foreach (var argument in _arguments)
                {
                    var parameter = (object?) argument.Compute (context);
                    parameters.Add (parameter);
                }

                result = Activator.CreateInstance (type, parameters.ToArray());
            }

            return result;
        }
    }

    /// <summary>
    /// Список вида <c>[1, 2, 3]</c>.
    /// </summary>
    sealed class ListNode : AtomNode
    {
        public ListNode(IEnumerable<AtomNode>? items)
        {
            _items = new ();
            if (items is not null)
            {
                _items.AddRange (items);
            }
        }

        private readonly List<AtomNode> _items;

        public override dynamic? Compute (Context context)
        {
            var result = new List<dynamic?>();
            foreach (var item in _items)
            {
                var value = item.Compute (context);
                result.Add (value);
            }

            return result;
        }
    }

    /// <summary>
    /// Обращение по индексу.
    /// </summary>
    sealed class IndexNode : AtomNode
    {
        // TODO: сделать индексацию произвольного выражения

        #region Construction

        public IndexNode (string variableName, AtomNode index)
        {
            _variableName = variableName;
            _index = index;
        }

        #endregion

        #region Private members

        private readonly string _variableName;
        private readonly AtomNode _index;

        #endregion

        #region AtomNode members

        public override dynamic? Compute (Context context)
        {
            if (!context.TryGetVariable (_variableName, out var obj))
            {
                return null;
            }

            if (obj is null)
            {
                return null;
            }

            // TODO: в классе может быть больше одного индексера

            var index = _index.Compute (context);

            var type = ((object) obj).GetType();
            ParameterInfo[]? parameters;
            PropertyInfo? indexer = null;
            foreach (var property in type.GetProperties (BindingFlags.Instance | BindingFlags.Public))
            {
                parameters = property.GetIndexParameters();
                if (parameters.Length != 0)
                {
                    indexer = property;
                    break;
                }
            }

            if (indexer is null)
            {
                return null;
            }

            var method = indexer.GetGetMethod();
            if (method is null)
            {
                return null;
            }

            var result = method.Invoke (obj, new object? [] { index });

            return result;
        }

        #endregion
    }

    /// <summary>
    /// Вызов свободной функции.
    /// </summary>
    sealed class FreeCallNode : AtomNode
    {
        #region Construction

        public FreeCallNode (string name, IEnumerable<AtomNode>? arguments)
        {
            _name = name;
            _arguments = new ();
            if (arguments is not null)
            {
                _arguments.AddRange (arguments);
            }
        }

        #endregion

        #region Private members

        private readonly string _name;
        private readonly List<AtomNode> _arguments;
        private FunctionDescriptor? _function;

        #endregion

        #region AtomNode members

        public override dynamic? Compute (Context context)
        {
            _function ??= context.GetFunction (_name);

            var args = new List<dynamic?>();
            foreach (var node in _arguments)
            {
                var arg = node.Compute (context);
                args.Add (arg);
            }

            var result = _function.CallPoint (context, args.ToArray());

            return result;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            var builder = StringBuilderPool.Shared.Get();
            builder.Append ($"function '{_name}' (");
            var first = true;
            foreach (var node in _arguments)
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
    /// Возврат значения из функции.
    /// </summary>
    sealed class ReturnNode : StatementNode
    {
        public ReturnNode (AtomNode? value)
        {
            _value = value;
        }

        private readonly AtomNode? _value;

        public override void Execute (Context context)
        {
            var value = _value?.Compute (context) ?? "(null)";

            throw new ReturnException (value);
        }
    }

    /// <summary>
    /// Псевдо-узел, предназначенный для функций.
    /// </summary>
    class PseudoNode : StatementNode
    {
    }

    sealed class DefinitionNode : PseudoNode
    {
        public DefinitionNode
            (
                string theName,
                IEnumerable<string>? argumentNames,
                IEnumerable<StatementNode>? body
            )
        {
            this.theName = theName;
            theArguments = new ();
            theBody = new ();
            if (argumentNames is not null)
            {
                theArguments.AddRange (argumentNames);
            }

            if (body is not null)
            {
                theBody.AddRange (body);
            }
        }

        internal readonly string theName;
        public readonly List<string> theArguments;
        internal readonly List<StatementNode> theBody;

        // public override void Execute (Context context)
        // {
        //     context.Output.WriteLine ($"Function {theName} ({string.Join (',', theArguments)})");
        //     foreach (var statement in theBody)
        //     {
        //         context.Output.WriteLine (statement);
        //     }
        //
        // }
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
    /// Простой вызов функции, без присваивания.
    /// </summary>
    sealed class NotAssignmentNode : StatementNode
    {
        #region Construction

        public NotAssignmentNode (AtomNode expression)
        {
            _expression = expression;
        }

        #endregion

        #region Private members

        private readonly AtomNode _expression;

        #endregion

        #region AstNode members

        public override void Execute (Context context)
        {
            _expression.Compute (context);
        }

        #endregion
    }

    /// <summary>
    /// Распечатка (дамп) значений переменных.
    /// </summary>
    sealed class PrintNode : StatementNode
    {
        public PrintNode (IEnumerable<AtomNode>? nodes, bool newLine)
        {
            _nodes = new List<AtomNode> ();
            if (nodes is not null)
            {
                _nodes.AddRange (nodes);
            }
            _newLine = newLine;
        }

        private readonly List<AtomNode> _nodes;
        private readonly bool _newLine;

        private void Print (IEnumerable sequence, Context context)
        {
            var first = true;
            foreach (var item in sequence)
            {
                if (!first)
                {
                    context.Output.Write (", ");
                }

                context.Output.Write (item);
                first = false;
            }
        }

        private void Print (AtomNode node, Context context)
        {
            var value = node.Compute (context);
            if (value is null)
            {
                context.Output.Write ("(null)");
                return;
            }

            if (value is string)
            {
                context.Output.Write (value);
                return;
            }

            var type = ((object) value).GetType();
            if (type.IsPrimitive)
            {
                context.Output.Write (value);
                return;
            }

            switch (value)
            {
                case IEnumerable sequence:
                    Print (sequence, context);
                    break;

                default:
                    context.Output.Write (value);
                    break;
            }
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
    /// Цикл for.
    /// </summary>
    sealed class ForNode : StatementNode
    {
        public ForNode(StatementNode init, AtomNode condition,
            StatementNode step, IEnumerable<StatementNode>? body)
        {
            _init = init;
            _condition = condition;
            _step = step;
            _body = new ();
            if (body is not null)
            {
                _body.AddRange (body);
            }
        }

        private readonly StatementNode _init;
        private readonly AtomNode _condition;
        private readonly StatementNode _step;
        private readonly List<StatementNode> _body;

        public override void Execute (Context context)
        {
            _init.Execute (context);
            while (_condition.Compute (context))
            {
                foreach (var statement in _body)
                {
                    statement.Execute (context);
                }

                _step.Execute (context);
            }
        }
    }

    /// <summary>
    /// Цикл foreach.
    /// </summary>
    sealed class ForEachNode : StatementNode
    {
        public ForEachNode(string variableName, AtomNode enumerable,
            IEnumerable<StatementNode>? body)
        {
            _variableName = variableName;
            _enumerable = enumerable;
            _body = new ();
            if (body is not null)
            {
                _body.AddRange (body);
            }
        }

        private readonly string _variableName;
        private readonly AtomNode _enumerable;
        private readonly List<StatementNode> _body;

        public override void Execute (Context context)
        {
            var enumerable = _enumerable.Compute (context);
            if (enumerable is null || enumerable is not IEnumerable)
            {
                return;
            }

            foreach (var value in enumerable)
            {
                context.Variables[_variableName] = value;
                foreach (var statement in _body)
                {
                    statement.Execute (context);
                }
            }
        }
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
