﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftWhile.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

using AM;
using AM.Logging;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// While loop.
    /// </summary>
    /// <example>
    /// $x=0;
    /// while $x &lt; 10
    /// do
    ///     $x, ') ',
    ///     'Прикольно же!'
    ///     #
    ///     $x=$x+1;
    /// end
    /// </example>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftWhile
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Condition.
        /// </summary>
        [CanBeNull]
        public PftCondition Condition { get; set; }

        /// <summary>
        /// Body.
        /// </summary>
        [NotNull]
        public PftNodeCollection Body { get; private set; }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {

                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(Condition, null))
                    {
                        nodes.Add(Condition);
                    }
                    nodes.AddRange(Body);
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            [ExcludeFromCodeCoverage]
            protected set
            {
                // Nothing to do here

                Log.Error
                    (
                        "PftWhile::Children: "
                        + "set value="
                        + value.ToVisibleString()
                    );
            }
        }

        /// <inheritdoc cref="PftNode.ComplexExpression" />
        public override bool ComplexExpression
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftWhile()
        {
            Body = new PftNodeCollection(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftWhile
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.While);

            Body = new PftNodeCollection(this);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftWhile
            (
                [NotNull] PftCondition condition,
                params PftNode[] body
            )
            : this()
        {
            Code.NotNull(condition, "condition");

            Condition = condition;
            foreach (PftNode node in body)
            {
                Body.Add(node);
            }
        }

        #endregion

        #region Private members

        private VirtualChildren _virtualChildren;

        private bool EvaluateCondition
            (
                [NotNull] PftContext context
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
            PftWhile result = (PftWhile)base.Clone();

            result._virtualChildren = null;

            if (!ReferenceEquals(Condition, null))
            {
                result.Condition = (PftCondition)Condition.Clone();
            }

            result.Body = Body.CloneNodes(result).ThrowIfNull();

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

            PftWhile otherWhile = (PftWhile)otherNode;
            PftSerializationUtility.CompareNodes
                (
                    Condition,
                    otherWhile.Condition
                );
            PftSerializationUtility.CompareLists
                (
                    Body,
                    otherWhile.Body
                );
        }

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (ReferenceEquals(Condition, null))
            {
                throw new PftCompilerException();
            }

            Condition.Compile(compiler);
            compiler.CompileNodes(Body);

            compiler.StartMethod(this);

            // TODO handle break

            compiler
                .Write("while (")
                .RefNodeMethod(Condition)
                .WriteLine("())")
                .WriteIndent()
                .WriteLine("{")
                .IncreaseIndent();

            compiler.CallNodes(Body);

            compiler
                .DecreaseIndent()
                .WriteIndent()
                .WriteLine("}");

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        }

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Condition
                = (PftCondition)PftSerializer.DeserializeNullable(reader);
            PftSerializer.Deserialize(reader, Body);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            try
            {
                while (EvaluateCondition(context))
                {
                    context.Execute(Body);
                }
            }
            catch (PftBreakException exception)
            {
                // It was break operator

                Log.TraceException
                    (
                        "PftWhile::Execute",
                        exception
                    );
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = "While"
            };

            if (!ReferenceEquals(Condition, null))
            {
                PftNodeInfo condition = new PftNodeInfo
                {
                    Node = Condition,
                    Name = "Condition"
                };
                result.Children.Add(condition);
                condition.Children.Add(Condition.GetNodeInfo());
            }

            PftNodeInfo body = new PftNodeInfo
            {
                Name = "Body"
            };
            result.Children.Add(body);
            foreach (PftNode node in Body)
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
                .Write("while ");

            if (!ReferenceEquals(Condition, null))
            {
                Condition.PrettyPrint(printer);
            }

            printer
                .WriteLine()
                .WriteLine("do");

            printer
                .IncreaseLevel()
                .WriteIndent()
                .WriteNodes(Body);
            printer.EatWhitespace();
            printer.EatNewLine();
            printer
                .DecreaseLevel()
                .WriteLine()
                .WriteIndent()
                .Write("end");
            if (!ReferenceEquals(Condition, null))
            {
                printer.Write(" /* while ");
                Condition.PrettyPrint(printer);
            }
            printer.WriteLine();
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.SerializeNullable(writer, Condition);
            PftSerializer.Serialize(writer, Body);
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        [DebuggerStepThrough]
        protected internal override bool ShouldSerializeText()
        {
            return false;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("while ");
            result.Append(Condition);
            result.Append(" do");
            foreach (PftNode node in Body)
            {
                result.Append(' ');
                result.Append(node);
            }
            result.Append(" end");

            return result.ToString();
        }

        #endregion
    }
}
