// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* PftNumericLiteral.cs -- числовой литерал
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Числовой литерал.
    /// </summary>
    public sealed class PftNumericLiteral
        : PftNumeric
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ConstantExpression" />
        public override bool ConstantExpression => true;

        /// <inheritdoc cref="PftNode.RequiresConnection" />
        public override bool RequiresConnection => false;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNumericLiteral()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNumericLiteral
            (
                double value
            )
            : base(value)
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNumericLiteral
            (
                PftToken token
            )
            : base(token)
        {
            Value = token.Text.ThrowIfNull("token.Text").ParseDouble();
        } // constructor

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine("double result = {0};", Value.ToInvariantString())
                .WriteIndent()
                .WriteLine("return result;");

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        } // method Compile

        /// <inheritdoc cref="PftNode.CompareNode"/>
        internal override void CompareNode
            (
                PftNode otherNode
            )
        {
            base.CompareNode(otherNode);

            var otherLiteral = (PftNumericLiteral) otherNode;
            if (Value != otherLiteral.Value)
            {
                throw new IrbisException();
            }
        }

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);
            Value = reader.ReadDouble();
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            // Nothing to do here

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.Write(Value.ToInvariantString());
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);
            writer.Write(Value);
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="PftNode.ToString" />
        public override string ToString() => Value.ToInvariantString();

        #endregion

    } // class PftNumericLiteral

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
