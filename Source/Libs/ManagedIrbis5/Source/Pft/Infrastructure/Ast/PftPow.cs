// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftPow.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using AM;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PftPow
        : PftNumeric
    {
        #region Properties

        /// <summary>
        /// X.
        /// </summary>
        public PftNumeric? X { get; set; }

        /// <summary>
        /// Y.
        /// </summary>
        public PftNumeric? Y { get; set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    List<PftNode> nodes = new List<PftNode>();
                    if (!ReferenceEquals(X, null))
                    {
                        nodes.Add(X);
                    }
                    if (!ReferenceEquals(Y, null))
                    {
                        nodes.Add(Y);
                    }
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
                        "PftPow::Children: "
                        + "set value="
                        + value.ToVisibleString()
                    );
            }
        }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax => true;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftPow()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftPow
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.Pow);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftPow
            (
                [NotNull] PftNumeric x,
                [NotNull] PftNumeric y
            )
        {
            X = x;
            Y = y;
        }

        #endregion

        #region Private members

        private VirtualChildren? _virtualChildren;

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            PftPow result = (PftPow)base.Clone();

            if (!ReferenceEquals(X, null))
            {
                result.X = (PftNumeric) X.Clone();
            }

            if (!ReferenceEquals(Y, null))
            {
                result.Y = (PftNumeric)Y.Clone();
            }

            return result;
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (ReferenceEquals(X, null)
                || ReferenceEquals(Y, null))
            {
                throw new PftCompilerException();
            }

            X.Compile(compiler);
            Y.Compile(compiler);

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine("double x = ")
                .CallNodeMethod(X);

            compiler
                .WriteIndent()
                .WriteLine("double y = ")
                .CallNodeMethod(Y);

            compiler
                .WriteIndent()
                .WriteLine("double result = Math.Pow(x, y);");

            compiler
                .WriteIndent()
                .WriteLine("return result;");

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

            X = (PftNumeric?) PftSerializer.DeserializeNullable(reader);
            Y = (PftNumeric?) PftSerializer.DeserializeNullable(reader);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (!ReferenceEquals(X, null)
                && !ReferenceEquals(Y, null))
            {
                X.Execute(context);
                Y.Execute(context);
                Value = Math.Pow(X.Value, Y.Value);
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = SimplifyTypeName(GetType().Name)
            };

            if (!ReferenceEquals(X, null))
            {
                PftNodeInfo x = new PftNodeInfo
                {
                    Node = X,
                    Name = "X"
                };
                result.Children.Add(x);
                x.Children.Add(X.GetNodeInfo());
            }

            if (!ReferenceEquals(Y, null))
            {
                PftNodeInfo y = new PftNodeInfo
                {
                    Node = Y,
                    Name = "Y"
                };
                result.Children.Add(y);
                y.Children.Add(Y.GetNodeInfo());
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
            printer
                .SingleSpace()
                .Write("pow")
                .Write('(')
                .Write(X)
                .Write(", ")
                .Write(Y)
                .Write(')');
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.SerializeNullable(writer, X);
            PftSerializer.SerializeNullable(writer, Y);
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("pow(");
            result.Append(X);
            result.Append(',');
            result.Append(Y);
            result.Append(')');

            return result.ToString();
        }

        #endregion
    }
}
