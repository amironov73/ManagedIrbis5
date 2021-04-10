﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftNode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.Collections;
using AM.IO;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;
using ManagedIrbis.Pft.Infrastructure.Walking;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    using Diagnostics;

    /// <summary>
    /// AST item
    /// </summary>

    public class PftNode
        : IVerifiable,
        ICloneable
    {
        #region Events

        /// <summary>
        /// Вызывается непосредственно перед выполнением.
        /// </summary>
        public event EventHandler<PftDebugEventArgs>? BeforeExecution;

        /// <summary>
        /// Вызывается непосредственно после выполнения.
        /// </summary>
        public event EventHandler<PftDebugEventArgs>? AfterExecution;

        #endregion

        #region Properties

        /// <summary>
        /// Parent node.
        /// </summary>
        public PftNode? Parent { get; internal set; }

        /// <summary>
        /// Breakpoint.
        /// </summary>
        public bool Breakpoint { get; set; }

        /// <summary>
        /// Список потомков. Может быть пустым.
        /// </summary>
        public virtual IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_children, null))
                {
                    _children = new PftNodeCollection(this);
                }

                return _children;
            }
            protected set
            {
                PftNodeCollection collection = (PftNodeCollection)value;
                collection.Parent = this;
                collection.EnsureParent();
                _children = collection;
            }
        }

        /// <summary>
        /// Column number.
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Номер строки, на которой в скрипте расположена
        /// соответствующая конструкция языка.
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        public virtual string? Text { get; set; }

        /// <summary>
        /// Whether the node is complex expression?
        /// </summary>
        public virtual bool ComplexExpression { get { return false; } }

        /// <summary>
        /// Whether the node is constant expression?
        /// </summary>
        public virtual bool ConstantExpression { get { return false; } }

        /// <summary>
        /// Node uses extended syntax?
        /// </summary>
        public virtual bool ExtendedSyntax { get { return false; } }

        /// <summary>
        /// Help for the node.
        /// </summary>
        public virtual string? Help { get { return null; } }

        /// <summary>
        /// Whether the node requires server connection to evaluate.
        /// </summary>
        public virtual bool RequiresConnection { get { return true; } }

        /// <summary>
        /// Kind of the node.
        /// </summary>
        public virtual PftNodeKind Kind { get { return PftNodeKind.None; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNode()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNode
            (
                PftToken token
            )
        {
            LineNumber = token.Line;
            Column = token.Column;
            Text = token.Text;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNode
            (
                params PftNode[] children
            )
        {
            foreach (PftNode child in children)
            {
                Children.Add(child);
            }
        }

        #endregion

        #region Private members

        private PftNodeCollection _children;

        /// <summary>
        /// Check deserialization result.
        /// </summary>
        internal virtual void CompareNode
            (
                PftNode otherNode
            )
        {
            var result = ReferenceEquals
                (
                    GetType(),
                    otherNode.GetType()
                );

            //if (result)
            //{
            //    result = Column == otherNode.Column
            //              && LineNumber == otherNode.LineNumber;
            //}

            if (result && ShouldSerializeText())
            {
                result = Text == otherNode.Text;
            }

            if (result && ShouldSerializeChildren())
            {
                result = Children.Count == otherNode.Children.Count;
                if (result)
                {
                    for (var i = 0; i < Children.Count; i++)
                    {
                        PftNode our = Children[i];
                        PftNode their = otherNode.Children[i];

                        if (!ReferenceEquals(our.GetType(), their.GetType()))
                        {
                            throw new PftSerializationException
                                (
                                    "Expecting child " + our.GetType()
                                    + ", got " + their.GetType()
                                );
                        }

                        our.CompareNode(their);
                    }
                }
            }

            if (!result)
            {
                throw new PftSerializationException
                    (
                        "CompareNode failed with " + GetType()
                    );
            }
        }

        /// <summary>
        /// Deserialize AST.
        /// </summary>
        protected internal virtual void Deserialize
            (
                BinaryReader reader
            )
        {
            Column = reader.ReadPackedInt32();
            LineNumber = reader.ReadPackedInt32();
            if (ShouldSerializeText())
            {
                Text = reader.ReadNullableString();
            }

            if (ShouldSerializeChildren())
            {
                PftSerializer.Deserialize(reader, Children);
            }
        }

        /// <summary>
        /// After execution.
        /// </summary>
        protected void OnAfterExecution
            (
                PftContext? context
            )
        {
            var handler = AfterExecution;
            if (handler is not null)
            {
                var eventArgs = new PftDebugEventArgs
                {
                    Context = context,
                    Node = this
                };
                handler(this, eventArgs);
            }

            if (Breakpoint
                && !ReferenceEquals(context, null)
                && !ReferenceEquals(context.Debugger, null))
            {
                var eventArgs = new PftDebugEventArgs
                    {
                        Context = context,
                        Node = this
                    };
                context.Debugger.Activate(eventArgs);
            }
        }

        /// <summary>
        /// Before execution.
        /// </summary>
        protected void OnBeforeExecution
            (
                PftContext? context
            )
        {
            var handler = BeforeExecution;
            if (handler is not null)
            {
                var eventArgs = new PftDebugEventArgs
                    {
                        Context = context,
                        Node = this
                    };
                handler(this, eventArgs);
            }

            if (Breakpoint)
            {
                if (!ReferenceEquals(context, null))
                {
                    context.ActivateDebugger(this);
                }
            }
        }

        /// <summary>
        /// Serialize AST.
        /// </summary>
        protected internal virtual void Serialize
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Column)
                .WritePackedInt32(LineNumber);
            if (ShouldSerializeText())
            {
                writer.WriteNullable(Text);
            }

            if (ShouldSerializeChildren())
            {
                PftSerializer.Serialize(writer, Children);
            }
        }

        /// <summary>
        /// Should serialize <see cref="Children"/> property?
        /// </summary>
        protected internal virtual bool ShouldSerializeChildren()
        {
            var children = Children as PftNodeCollection;

            return !ReferenceEquals(children, null);
        }

        /// <summary>
        /// Should serialize <see cref="Text"/> property?
        /// </summary>
        protected internal virtual bool ShouldSerializeText()
        {
            return true;
        }

        /// <summary>
        /// Simplify type name.
        /// </summary>
        protected internal static string SimplifyTypeName
            (
                string typeName
            )
        {
            return typeName.StartsWith("Pft")
                ? typeName.Substring(3)
                : typeName;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Accept the visitor.
        /// </summary>
        /// <returns>
        /// <c>true</c> means "continue".
        /// </returns>
        public virtual bool AcceptVisitor
            (
                PftVisitor visitor
            )
        {
            if (!visitor.VisitNode(this))
            {
                return false;
            }

            foreach (PftNode child in Children)
            {
                if (!child.AcceptVisitor(visitor))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compiles the node.
        /// </summary>
        public virtual void Compile
            (
                PftCompiler compiler
            )
        {
            var flag = ShouldSerializeChildren();
            if (flag)
            {
                compiler.CompileNodes(Children);
            }

            compiler.StartMethod(this);
            if (flag)
            {
                compiler.CallNodes(Children);
            }
            compiler.EndMethod(this);

            compiler.MarkReady(this);
        }

        /// <summary>
        /// Собственно форматирование.
        /// Включает в себя результат
        /// форматирования всеми потомками.
        /// </summary>
        public virtual void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            foreach (PftNode child in Children)
            {
                child.Execute(context);
            }

            OnAfterExecution(context);
        }

        /// <summary>
        /// Find parent node of specified type.
        /// </summary>
        /// <typeparam name="T">Type of parent to find.</typeparam>
        /// <returns>Found parent node or <c>null</c>.</returns>
        public PftNode? FindParent<T>()
        {
            PftNode candidate = Parent;

            while (!ReferenceEquals(candidate, null))
            {
                if (candidate is T)
                {
                    return candidate;
                }

                candidate = candidate.Parent;
            }

            return null;
        }

        /// <summary>
        /// Список полей, задействованных в форматировании
        /// данным элементом и всеми его потомками, включая
        /// косвенных.
        /// </summary>
        public virtual int[] GetAffectedFields()
        {
            int[] result = new int[0];

            foreach (PftNode child in Children)
            {
                int[] sub = child.GetAffectedFields();
                if (sub.Length != 0)
                {
                    result = result
                        .Union(sub)
                        .Distinct()
                        .ToArray();
                }
            }

            return result;
        }

        /// <summary>
        /// Получение списка потомков (как прямых,
        /// так и косвенных) определенного типа.
        /// </summary>
        public NonNullCollection<T> GetDescendants<T>()
            where T : PftNode
        {
            NonNullCollection<T> result = new NonNullCollection<T>();

            foreach (PftNode child in Children)
            {
                T item = child as T;
                if (item != null)
                {
                    result.Add(item);
                }
                result.AddRange(child.GetDescendants<T>());
            }

            return result;
        }

        /// <summary>
        /// Построение массива потомков-листьев
        /// (т. е. не имеющих собственных потомков).
        /// </summary>
        /// <remarks>Если у узла нет потомков,
        /// он возвращает массив из одного элемента:
        /// самого себя.</remarks>
        public PftNode[] GetLeafs()
        {
            if (Children.Count == 0)
            {
                return new[] { this };
            }

            PftNode[] result = Children
                .SelectMany(child => child.GetLeafs())
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get node info for debugger.
        /// </summary>
        public virtual PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Name = SimplifyTypeName(GetType().Name),
                Node = this,
                Value = Text
            };

            foreach (PftNode child in Children)
            {
                PftNodeInfo info = child.GetNodeInfo();
                result.Children.Add(info);
            }

            return result;
        }

        /// <summary>
        /// Получение родительского узла указанного уровня.
        /// </summary>
        /// <param name="level">Требуемый уровень (число без знака).
        /// Считается, что непосредственный родитель имеет уровень 0.
        /// </param>
        /// <returns>Найденный родительский узел либо <c>null</c>.</returns>
        public PftNode? GetParent
            (
                int level
            )
        {
            var node = this;

            while (!ReferenceEquals(node, null))
            {
                node = node.Parent;
                if (level == 0)
                {
                    break;
                }

                level--;
            }

            return node;
        }

        /// <summary>
        /// Оптимизация дерева потомков.
        /// На данный момент не реализована.
        /// </summary>
        /// <remarks>Если возвращает <c>null</c>,
        /// это означает, что данный узел и всех
        /// его потомков можно безболезненно удалить.
        /// </remarks>
        public virtual PftNode? Optimize()
        {
            var children = Children as PftNodeCollection;
            if (!ReferenceEquals(children, null))
            {
                children.Optimize();
            }

            return this;
        }

        /// <summary>
        /// Поддерживает ли многопоточность,
        /// т. е. может ли быть запущен одновременно
        /// с соседними элементами.
        /// </summary>
        /// <remarks>
        /// На данный момент многопоточность
        /// не поддерживается.
        /// </remarks>
        public virtual bool SupportsMultithreading()
        {
            return Children.Count != 0
                && Children.All
                (
                    child => child.SupportsMultithreading()
                );
        }

        /// <summary>
        /// Формирование исходного текста по AST.
        /// Применяется, например, для красивой
        /// распечатки программы на языке PFT.
        /// </summary>
        public virtual void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            if (ShouldSerializeChildren())
            {
                printer.WriteNodes(Children);
            }
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public virtual object Clone()
        {
            PftNode result = (PftNode)MemberwiseClone();
            result.Parent = null;

            PftNodeCollection children
                = Children as PftNodeCollection;
            if (!ReferenceEquals(children, null))
            {
                result.Children = children.CloneNodes(result);
            }

            return result;
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public virtual bool Verify
            (
                bool throwOnError
            )
        {
            var result = Children.All
                (
                    child => child.Verify(throwOnError)
                );

            //if (!result)
            //{
            //    Log.Error
            //        (
            //            "PftNode::Verify: "
            //            + "verification failed"
            //        );

            //    if (throwOnError)
            //    {
            //        throw new VerificationException();
            //    }
            //}

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals ( object? other ) =>
            ReferenceEquals(this, other);

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Column;
                hashCode = (hashCode * 397) ^ LineNumber;
                hashCode = (hashCode * 397) ^
                    (
                        Text != null
                        ? Text.GetHashCode()
                        : 0
                    );

                return hashCode;
            }
        }

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();

            foreach (PftNode child in Children)
            {
                result.Append(child);
            }

            return result.ToString();
        }

        #endregion
    }
}
