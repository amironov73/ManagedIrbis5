// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* LvalueNode.cs -- lvalue
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Pidgin;
using Pidgin.Expression;

using static Pidgin.Parser;

using static AM.Scripting.Barsik.Grammar;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Lvalue, т. е. то, что может появиться слева от знака присваивания.
/// </summary>
internal sealed class Lvalue
{
    #region Nested classes

    /// <summary>
    /// Временный узел с вызовом метода.
    /// </summary>
    private class CallNode
    {
        public string Name { get; }
        public IEnumerable<int> Arguments { get; }

        public CallNode
            (
                string name,
                IEnumerable<int> arguments
            )
        {
            Name = name;
            Arguments = arguments;
        }
    }

    /// <summary>
    /// Абстрактная база для узлов lvalue.
    /// </summary>
    internal abstract class SubNode
    {
        #region Public methods

        /// <summary>
        /// Вычисление значения.
        /// </summary>
        public abstract dynamic? Compute
            (
                Context context
            );

        /// <summary>
        /// Присвоение значения.
        /// </summary>
        public abstract void Assign
            (
                Context context,
                string operation,
                dynamic? value
            );

        #endregion
    }

    /// <summary>
    /// Обращение к свойству/полю.
    /// </summary>
    private sealed class PropertySubNode
        : SubNode
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public PropertySubNode
            (
                SubNode leftNode,
                string propertyName
            )
        {
            _leftNode = leftNode;
            _propertyName = propertyName;
        }

        #endregion

        #region Private members

        private readonly SubNode _leftNode;

        private readonly string _propertyName;

        #endregion

        #region SubNode members

        /// <inheritdoc cref="SubNode.Compute"/>
        public override dynamic? Compute
            (
                Context context
            )
        {
            var obj = _leftNode.Compute (context);
            if (obj is null)
            {
                return null;
            }
            var type = ((object) obj).GetType();
            var name = _propertyName;
            var property = type.GetProperty (name);
            if (property is not null)
            {
                return property.GetValue (obj);
            }

            var field = type.GetField (name);
            if (field is not null)
            {
                return field.GetValue (obj);
            }

            return null;
        }

        /// <inheritdoc cref="SubNode.Assign"/>
        public override void Assign
            (
                Context context,
                string operation,
                dynamic? value
            )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"({_leftNode}).{_propertyName}";
        }

        #endregion
    }

    /// <summary>
    /// Доступ по индексу
    /// </summary>
    private sealed class IndexSubNode
        : SubNode
    {
        #region Construction

        public IndexSubNode
            (
                SubNode leftNode,
                int index
            )
        {
            // TODO сделать index нецелым
            _leftNode = leftNode;
            _index = index;
        }

        #endregion

        #region Private members

        private readonly SubNode _leftNode;

        private readonly int _index;

        #endregion

        #region SubNode members

        /// <inheritdoc cref="SubNode.Compute"/>
        public override dynamic? Compute
            (
                Context context
            )
        {
            var obj = _leftNode.Compute (context);
            if (obj is null)
            {
                return null;
            }

            // TODO: в классе может быть больше одного индексера

            if (obj is Array array)
            {
                return array.GetValue (_index);
            }

            var type = ((object) obj).GetType();
            PropertyInfo? indexer = null;
            foreach (var property in type.GetProperties (BindingFlags.Instance | BindingFlags.Public))
            {
                var parameters = property.GetIndexParameters();
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

            var result = method.Invoke (obj, new object? [] { _index });

            return result;
        }

        /// <inheritdoc cref="SubNode.Assign"/>
        public override void Assign
            (
                Context context,
                string operation,
                dynamic? value
            )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"({_leftNode}) [{_index}]";
        }

        #endregion
    }

    /// <summary>
    /// Вызов метода
    /// </summary>
    private sealed class MethodSubNode
        : SubNode
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MethodSubNode
            (
                SubNode leftNode,
                string methodName,
                IEnumerable<int> arguments
            )
        {
            Sure.NotNullNorEmpty (methodName);

            _leftNode = leftNode;
            _methodName = methodName;
            _arguments = arguments.ToArray();
        }

        #endregion

        #region Private members

        private readonly SubNode _leftNode;

        private readonly string _methodName;

        private readonly int[] _arguments;

        #endregion

        #region SubNode members

        /// <inheritdoc cref="SubNode.Compute"/>
        public override dynamic? Compute
            (
                Context context
            )
        {
            var thisValue = _leftNode.Compute (context);
            if (thisValue is null)
            {
                return null;
            }

            var argumentValues = new List<dynamic?>();
            var argumentTypes = new List<Type>();
            foreach (var argument in _arguments)
            {
                //var value = argument.Compute (context);
                var value = (dynamic?) argument;
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

        /// <inheritdoc cref="SubNode.Assign"/>
        public override void Assign
            (
                Context context,
                string operation,
                dynamic? value
            )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            var args = string.Join (", ", _arguments);
            return $"{_methodName} (({_leftNode}) ({args}))";
        }

        #endregion
    }

    /// <summary>
    /// Самый левый (начальный) узел, представляющий собой
    /// ссылку на переменную или на класс.
    /// </summary>
    private sealed class VariableSubNode
        : SubNode
    {
        #region Properties

        /// <summary>
        /// Имя переменной или класса.
        /// </summary>
        // ReSharper disable MemberCanBePrivate.Local
        public string Name { get; }
        // ReSharper restore MemberCanBePrivate.Local

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя переменной/класса.</param>
        public VariableSubNode
            (
                string name
            )
        {
            Name = name;
        }

        #endregion

        #region SubNode members

        /// <inheritdoc cref="SubNode.Compute"/>
        public override dynamic? Compute
            (
                Context context
            )
        {
            if (!context.TryGetVariable (Name, out var value))
            {
                context.Error.WriteLine ($"Variable {Name} not found");

                return null;
            }

            return value;
        }

        /// <inheritdoc cref="SubNode.Assign"/>
        public override void Assign
            (
                Context context,
                string operation,
                dynamic? value
            )
        {
            // TODO обрабатывать operation

            context.SetVariable (Name, value);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"variable {Name}";
        }

        #endregion
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="topNode">Топовый, т. е. самый правый узел. </param>
    public Lvalue
        (
            SubNode topNode
        )
    {
        _topNode = topNode;
    }

    #endregion

    #region Private members

    private readonly SubNode _topNode;

    private static readonly Parser<char, SubNode> _Variable = Identifier
        .Select<SubNode> (v => new VariableSubNode (v));

    private static readonly Parser<char, string> _Property =
        Tok ('.').Then (Identifier);

    private static readonly Parser<char, int> _Index =
        Tok (Num).Between (Tok ('['), Tok (']'));

    private static readonly Parser<char, CallNode> _Call = Map
        (
            (_, name, _, args, _) => new CallNode (name, args),
            Tok ('.'),
            Tok (Identifier),
            Tok ('('),
            Try (Num).Separated (Tok (',')),
            Tok (')')
        );

    private static Parser<char, Func<SubNode, SubNode>> Property (Parser<char, string> op) =>
        op.Select<Func<SubNode, SubNode>> (name => node => new PropertySubNode (node, name));

    private static Parser<char, Func<SubNode, SubNode>> Index (Parser<char, int> op) =>
        op.Select<Func<SubNode, SubNode>> (index => node => new IndexSubNode (node, index));

    private static Parser<char, Func<SubNode, SubNode>> Call (Parser<char, CallNode> op) =>
        op.Select<Func<SubNode, SubNode>> (call => node => new MethodSubNode (node, call.Name, call.Arguments));

    // цель присваивания
    internal static readonly Parser<char, SubNode> Target = ExpressionParser.Build
        (
            _Variable,
            new []
            {
                Operator.PostfixChainable
                    (
                        Try (Call (_Call)),
                        Try (Property (_Property)),
                        Try (Index (_Index))
                    )
            }
        );

    /// <summary>
    /// Присвоение значения.
    /// </summary>
    internal void Execute
        (
            Context context,
            string operation,
            dynamic? value
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (operation);

        _topNode.Assign (context, operation, value);
    }

    #endregion
}
