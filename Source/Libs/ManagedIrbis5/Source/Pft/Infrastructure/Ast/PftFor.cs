// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* PftFor.cs -- цикл "для"
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

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Цикл "для".
    /// </summary>
    /// <example>
    /// for $x=0; $x &lt; 10; $x = $x+1;
    /// do
    ///     $x, ') ',
    ///     'Прикольно же!'
    ///     #
    /// end
    /// </example>
    public sealed class PftFor
        : PftNode
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax => true;

        /// <inheritdoc cref="PftNode.ComplexExpression" />
        public override bool ComplexExpression => true;

        /// <summary>
        /// Initialization.
        /// </summary>
        public PftNodeCollection Initialization { get; private set; }

        /// <summary>
        /// Condition.
        /// </summary>
        public PftCondition? Condition { get; set; }

        /// <summary>
        /// Loop statements.
        /// </summary>
        public PftNodeCollection Loop { get; private set; }

        /// <summary>
        /// Body.
        /// </summary>
        public PftNodeCollection Body { get; private set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    var nodes = new List<PftNode>();
                    nodes.AddRange(Initialization);
                    if (Condition is { } condition)
                    {
                        nodes.Add(condition);
                    }

                    nodes.AddRange(Loop);
                    nodes.AddRange(Body);
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
                        "PftFor::Children: "
                        + "set value="
                        + value.ToVisibleString()
                    );
            }
        } // property Children

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFor()
        {
            Initialization = new PftNodeCollection(this);
            Loop = new PftNodeCollection(this);
            Body = new PftNodeCollection(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftFor
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.For);

            Initialization = new PftNodeCollection(this);
            Loop = new PftNodeCollection(this);
            Body = new PftNodeCollection(this);
        }

        #endregion

        #region Private members

        private VirtualChildren? _virtualChildren;

        private bool _EvaluateCondition
            (
                PftContext context
            )
        {
            if (ReferenceEquals(Condition, null))
            {
                return true;
            }

            Condition.Execute(context);

            return Condition.Value;
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            var result = (PftFor)base.Clone();

            result._virtualChildren = null;

            result.Initialization
                = Initialization.CloneNodes(result).ThrowIfNull();
            result.Loop = Loop.CloneNodes(result).ThrowIfNull();
            result.Body = Body.CloneNodes(result).ThrowIfNull();

            if (!ReferenceEquals(Condition, null))
            {
                result.Condition = (PftCondition)Condition.Clone();
            }

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

            var otherFor = (PftFor)otherNode;
            PftSerializationUtility.CompareLists
                (
                    Initialization,
                    otherFor.Initialization
                );
            PftSerializationUtility.CompareNodes
                (
                    Condition,
                    otherFor.Condition
                );
            PftSerializationUtility.CompareLists
                (
                    Loop,
                    otherFor.Loop
                );
            PftSerializationUtility.CompareLists
                (
                    Body,
                    otherFor.Body
                );
        }

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            PftSerializer.Deserialize(reader, Initialization);
            Condition = (PftCondition?)PftSerializer.DeserializeNullable(reader);
            PftSerializer.Deserialize(reader, Loop);
            PftSerializer.Deserialize(reader, Body);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            context.Execute(Initialization);

            try
            {
                while (_EvaluateCondition(context))
                {
                    context.Execute(Body);
                    context.Execute(Loop);
                }
            }
            catch (PftBreakException exception)
            {
                // It was break operator

                Magna.TraceException
                    (
                        "PftFor::Execute",
                        exception
                    );
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            var result = new PftNodeInfo
            {
                Node = this,
                Name = "For"
            };

            if (Initialization.Count != 0)
            {
                var init = new PftNodeInfo
                {
                    Name = "Init"
                };
                result.Children.Add(init);
                foreach (var node in Initialization)
                {
                    init.Children.Add(node.GetNodeInfo());
                }
            }

            if (!ReferenceEquals(Condition, null))
            {
                var condition = new PftNodeInfo
                {
                    Node = Condition,
                    Name = "Condition"
                };
                result.Children.Add(condition);
                condition.Children.Add(Condition.GetNodeInfo());
            }

            if (Loop.Count != 0)
            {
                var loop = new PftNodeInfo
                {
                    Name = "Loop"
                };
                result.Children.Add(loop);
                foreach (var node in Loop)
                {
                    loop.Children.Add(node.GetNodeInfo());
                }
            }

            var body = new PftNodeInfo
            {
                Name = "Body"
            };
            result.Children.Add(body);
            foreach (var node in Body)
            {
                body.Children.Add(node.GetNodeInfo());
            }

            return result;
        }

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
                .Write("for ");

            printer.WriteNodes(Initialization);
            printer.Write("; ");

            Condition?.PrettyPrint(printer);
            printer.EatWhitespace();
            printer.Write("; ");

            printer.WriteNodes(Loop);
            printer.WriteLine(";");
            printer
                .WriteIndent()
                .WriteLine("do");

            printer.IncreaseLevel();
            printer.WriteNodes(Body);
            printer.DecreaseLevel();
            printer.EatWhitespace();
            printer.EatNewLine();
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

            PftSerializer.Serialize(writer, Initialization);
            PftSerializer.SerializeNullable(writer, Condition);
            PftSerializer.Serialize(writer, Loop);
            PftSerializer.Serialize(writer, Body);
        } // method Serialize

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("for ");
            PftUtility.NodesToText(result, Initialization);
            result.Append(';');
            result.Append(Condition);
            result.Append(';');
            PftUtility.NodesToText(result, Loop);
            result.Append(" do ");
            PftUtility.NodesToText(result, Body);
            result.Append(" end");

            return result.ToString();
        } // method ToString

        #endregion

    } // class PftFor

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
