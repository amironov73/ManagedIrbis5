// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftFrom.cs -- жалкое подобие LINQ
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

using AM;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using static ManagedIrbis.Pft.Infrastructure.Serialization.PftSerializationUtility;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Жалкое подобие LINQ.
    /// </summary>
    /// <example>
    /// <code>
    /// from $x in (v692^b/)
    /// where $x:'2008'
    /// select 'Item: ', $x,
    /// order $x,
    /// end
    /// </code>
    /// </example>
    public sealed class PftFrom
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Variable reference.
        /// </summary>
        public PftVariableReference? Variable { get; set; }

        /// <summary>
        /// Source.
        /// </summary>
        public PftNodeCollection Source { get; private set; }

            /// <summary>
        /// Where clause.
        /// </summary>
        public PftCondition? Where { get; set; }

        /// <summary>
        /// Select clause.
        /// </summary>
        public PftNodeCollection Select { get; private set; }

        /// <summary>
        /// Order clause.
        /// </summary>
        public PftNodeCollection Order { get; private set; }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax => true;

        /// <inheritdoc cref="PftNode.ComplexExpression" />
        public override bool ComplexExpression => true;

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    var nodes = new List<PftNode>();
                    if (!ReferenceEquals(Variable, null))
                    {
                        nodes.Add(Variable);
                    }
                    nodes.AddRange(Source);
                    if (!ReferenceEquals(Where, null))
                    {
                        nodes.Add(Where);
                    }
                    nodes.AddRange(Select);
                    nodes.AddRange(Order);
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            [ExcludeFromCodeCoverage]
            protected set
            {
                // Nothing to do here

                Magna.Error
                    (
                        "PftFrom::Children: "
                        + "set value="
                        + value.ToVisibleString()
                    );
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFrom()
        {
            Source = new PftNodeCollection(this);
            Select = new PftNodeCollection(this);
            Order = new PftNodeCollection(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFrom
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.From);

            Source = new PftNodeCollection(this);
            Select = new PftNodeCollection(this);
            Order = new PftNodeCollection(this);
        }

        #endregion

        #region Private members

        private VirtualChildren? _virtualChildren;

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            var result = (PftFrom)base.Clone();

            result._virtualChildren = null;

            if (!ReferenceEquals(Variable, null))
            {
                result.Variable = (PftVariableReference) Variable.Clone();
            }

            result.Source = Source.CloneNodes(result).ThrowIfNull();

            if (!ReferenceEquals(Where, null))
            {
                result.Where = (PftCondition) Where.Clone();
            }

            result.Select = Select.CloneNodes(result).ThrowIfNull();
            result.Order = Order.CloneNodes(result).ThrowIfNull();

            return result;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.CompareNode" />
        internal override void CompareNode
            (
                PftNode otherNode
            )
        {
            base.CompareNode(otherNode);

            var otherFrom = (PftFrom) otherNode;
            CompareNodes
                (
                    Variable,
                    otherFrom.Variable
                );
            CompareLists
                (
                    Source,
                    otherFrom.Source
                );
            CompareNodes
                (
                    Where,
                    otherFrom.Where
                );
            CompareLists
                (
                    Select,
                    otherFrom.Select
                );
            CompareLists
                (
                    Order,
                    otherFrom.Order
                );
        } // method CompareNode

        ///// <inheritdoc cref="PftNode.Compile" />
        //public override void Compile
        //    (
        //        PftCompiler compiler
        //    )
        //{
        //    if (ReferenceEquals(Variable, null)
        //        || Source.Count == 0
        //        || Select.Count == 0)
        //    {
        //        throw new PftCompilerException();
        //    }

        //    // TODO implement

        //    compiler.CompileNodes(Source);
        //    if (!ReferenceEquals(Where, null))
        //    {
        //        Where.Compile(compiler);
        //    }
        //    compiler.CompileNodes(Select);
        //    compiler.CompileNodes(Order);

        //    compiler.StartMethod(this);

        //    compiler.EndMethod(this);
        //    compiler.MarkReady(this);
        //}

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Variable = (PftVariableReference?) PftSerializer
                .DeserializeNullable(reader);
            PftSerializer.Deserialize(reader, Source);
            Where = (PftCondition?) PftSerializer.DeserializeNullable(reader);
            PftSerializer.Deserialize(reader, Select);
            PftSerializer.Deserialize(reader, Order);
        } // method Deserialize

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            var manager = context.Variables;
            var variable = Variable;
            if (variable is not null)
            {
                var name = variable.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    var buffer = new List<string>();

                    // In clause
                    var sourceText = context.Evaluate(Source);
                    var lines = sourceText.SplitLines();

                    // Where clause
                    if (!ReferenceEquals(Where, null))
                    {
                        foreach (var line in lines)
                        {
                            manager.SetVariable(name, line);
                            Where.Execute(context);
                            if (Where.Value)
                            {
                                buffer.Add(line);
                            }
                        }

                        lines = buffer.ToArray();
                    }

                    // Select clause
                    buffer.Clear();
                    foreach (var line in lines)
                    {
                        manager.SetVariable(name, line);
                        var value = context.Evaluate(Select);
                        buffer.Add(value);
                    }

                    lines = buffer.ToArray();

                    // Order clause
                    buffer.Clear();
                    if (Order.Count != 0)
                    {
                        foreach (var line in lines)
                        {
                            manager.SetVariable(name, line);
                            var value = context.Evaluate(Order);
                            buffer.Add(value);
                        }

                        Array.Sort
                            (
                                lines,
                                buffer.ToArray()
                            );
                    }

                    var output = string.Join
                        (
                            Environment.NewLine,
                            lines
                        );
                    if (!string.IsNullOrEmpty(output))
                    {
                        context.Write(this, output);
                    }
                }
            }

            OnAfterExecution(context);
        } // method Execute

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            var result = new PftNodeInfo
            {
                Node = this,
                Name = "From"
            };

            if (!ReferenceEquals(Variable, null))
            {
                var variable = new PftNodeInfo
                {
                    Name = "Variable"
                };
                result.Children.Add(variable);
                variable.Children.Add(Variable.GetNodeInfo());
            }

            var sourceClause = new PftNodeInfo
            {
                Name = "Source"
            };
            result.Children.Add(sourceClause);
            foreach (var node in Source)
            {
                sourceClause.Children.Add(node.GetNodeInfo());
            }

            if (!ReferenceEquals(Where, null))
            {
                var whereClause = new PftNodeInfo
                {
                    Name = "Where"
                };
                result.Children.Add(whereClause);
                whereClause.Children.Add(Where.GetNodeInfo());
            }

            var selectClause = new PftNodeInfo
            {
                Name = "Select"
            };
            result.Children.Add(selectClause);
            foreach (var node in Select)
            {
                selectClause.Children.Add(node.GetNodeInfo());
            }

            if (Order.Count != 0)
            {
                var orderClause = new PftNodeInfo
                {
                    Name = "Order"
                };
                result.Children.Add(orderClause);
                foreach (var node in Order)
                {
                    orderClause.Children.Add(node.GetNodeInfo());
                }
            }

            return result;
        } // method GetNodeInfo

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.EatWhitespace();
            printer.EatNewLine();

            printer
                .WriteLine()
                .WriteIndent()
                .Write("from ");

            Variable?.PrettyPrint(printer);
            printer.Write(" in ");

            var first = true;
            foreach (var node in Source)
            {
                if (!first)
                {
                    printer.Write(", ");
                }
                node.PrettyPrint(printer);
                first = false;
            }
            printer.WriteLine();

            printer
                .WriteIndent()
                .Write("where ");
            Where?.PrettyPrint(printer);
            printer.WriteLine();

            printer
                .WriteIndentIfNeeded()
                .Write("select ");
            first = true;
            foreach (var node in Select)
            {
                if (!first)
                {
                    printer.Write(", ");
                }
                node.PrettyPrint(printer);
                first = false;
            }
            printer.WriteLine();

            if (Order.Count != 0)
            {
                printer
                    .WriteIndent()
                    .Write("order ");
                first = true;
                foreach (var node in Order)
                {
                    if (!first)
                    {
                        printer.Write(", ");
                    }
                    node.PrettyPrint(printer);
                    first = false;
                }
                printer.WriteLine();
            }

            printer.WriteLine();
            printer
                .WriteIndent()
                .WriteLine("end");
        } // method PrettyPrint

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.SerializeNullable(writer, Variable);
            PftSerializer.Serialize(writer, Source);
            PftSerializer.SerializeNullable(writer, Where);
            PftSerializer.Serialize(writer, Select);
            PftSerializer.Serialize(writer, Order);
        } // method Serialize

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("from ");
            result.Append(Variable);
            result.Append(" in ");
            PftUtility.NodesToText(result, Source);
            if (Where is { } whereClause)
            {
                result.Append(" where ");
                result.Append(whereClause);
            }
            result.Append(" select ");
            PftUtility.NodesToText(result, Select);
            if (Order.Count != 0)
            {
                result.Append(" order ");
                PftUtility.NodesToText(result, Order);
            }
            result.Append(" end");

            return result.ToString();
        } // method ToString

        #endregion

    } // class PftFrom

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
