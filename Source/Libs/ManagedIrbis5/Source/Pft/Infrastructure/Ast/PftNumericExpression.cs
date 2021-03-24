// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* PftNumericExpression.cs -- математическое выражение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

using AM;
using AM.IO;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Математическое выражение
    /// </summary>
    public sealed class PftNumericExpression
        : PftNumeric
    {
        #region Properties

        /// <summary>
        /// Left operand.
        /// </summary>
        public PftNumeric? LeftOperand { get; set; }

        /// <summary>
        /// Operation.
        /// </summary>
        public string? Operation { get; set; }

        /// <summary>
        /// Right operand.
        /// </summary>
        public PftNumeric? RightOperand { get; set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    var nodes = new List<PftNode>();
                    if (!ReferenceEquals(LeftOperand, null))
                    {
                        nodes.Add(LeftOperand);
                    }
                    if (!ReferenceEquals(RightOperand, null))
                    {
                        nodes.Add(RightOperand);
                    }
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }
            protected set => Magna.Error
                (
                    "PftNumericExpression::Children: "
                    + "set value="
                    + value.ToVisibleString()
                );
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNumericExpression()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNumericExpression
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNumericExpression
            (
                [NotNull] PftNumeric leftOperand,
                [NotNull] string operation,
                [NotNull] PftNumeric rightOperand
            )
        {
            LeftOperand = leftOperand;
            Operation = operation;
            RightOperand = rightOperand;
        }

        #endregion

        #region Private members

        private VirtualChildren? _virtualChildren;

        #endregion

        #region Public methods

        /// <summary>
        /// Do the operation.
        /// </summary>
        public static double DoOperation
            (
                [NotNull] PftContext context,
                double leftValue,
                [NotNull] string operation,
                double rightValue
            )
        {
            operation = operation.ToLowerInvariant();

            double result;
            switch (operation)
            {
                case "+":
                    result = leftValue + rightValue;
                    break;

                case "-":
                    result = leftValue - rightValue;
                    break;

                case "*":
                    result = leftValue * rightValue;
                    break;

                case "/":
                    result = leftValue / rightValue;
                    break;

                case "%":
                    // ReSharper disable PossibleLossOfFraction
                    result = (int)leftValue % (int)rightValue;
                    // ReSharper enable PossibleLossOfFraction
                    break;

                case "div":
                    // ReSharper disable PossibleLossOfFraction
                    result = (int)leftValue / (int)rightValue;
                    // ReSharper enable PossibleLossOfFraction
                    break;

                default:
                    Magna.Error
                        (
                            nameof(PftNumericExpression) + "::" + nameof(DoOperation)
                            + ": unexpected operation="
                            + operation
                        );

                    throw new PftSyntaxException();
            }

            return result;
        } // method DoOperation

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            var result = (PftNumericExpression) base.Clone();

            if (LeftOperand is { } left)
            {
                result.LeftOperand = (PftNumeric) left.Clone();
            }

            if (RightOperand is { } right)
            {
                result.RightOperand = (PftNumeric) right.Clone();
            }

            return result;
        } // method Clone

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.CompareNode" />
        internal override void CompareNode
            (
                PftNode otherNode
            )
        {
            base.CompareNode(otherNode);

            var otherExpression = (PftNumericExpression) otherNode;
            PftSerializationUtility.CompareNodes
                (
                    LeftOperand,
                    otherExpression.LeftOperand
                );
            PftSerializationUtility.CompareNodes
                (
                    RightOperand,
                    otherExpression.RightOperand
                );
            if (Operation != otherExpression.Operation)
            {
                throw new IrbisException();
            }
        } // method CompareNode

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (ReferenceEquals(LeftOperand, null)
                || ReferenceEquals(RightOperand, null)
                || string.IsNullOrEmpty(Operation))
            {
                throw new PftCompilerException();
            }

            LeftOperand.Compile(compiler);
            RightOperand.Compile(compiler);

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine("PftContext previousContext = StartEvaluate();");

            compiler
                .WriteIndent()
                .Write("double left = ")
                .CallNodeMethod(LeftOperand)
                .WriteLine();
            compiler
                .WriteIndent()
                .Write("double right = ")
                .CallNodeMethod(RightOperand)
                .WriteLine();
            compiler
                .WriteIndent()
                .WriteLine
                    (
                        "double result = left {0} right;",
                        Operation
                    );
            compiler
                .WriteIndent()
                .WriteLine("EndEvaluate(previousContext);")
                .WriteIndent()
                .WriteLine("return result;");

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        } // method Compile

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            LeftOperand = (PftNumeric?) PftSerializer.DeserializeNullable(reader);
            Operation = reader.ReadNullableString();
            RightOperand = (PftNumeric?) PftSerializer.DeserializeNullable(reader);
        } // method Deserialize

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (LeftOperand is null)
            {
                Magna.Error
                    (
                        nameof(PftNumericExpression) + "::" + nameof(Execute)
                        + ": LeftOperand not specified"
                    );

                throw new PftSyntaxException(this);
            }

            if (string.IsNullOrEmpty(Operation))
            {
                Magna.Error
                    (
                        nameof(PftNumericExpression) + "::" + nameof(Execute)
                        + ": Operation not specified"
                    );

                throw new PftSyntaxException(this);
            }

            if (RightOperand is null)
            {
                Magna.Error
                    (
                        nameof(PftNumericExpression) + "::" + nameof(Execute)
                        + ": RightOperand not specified"
                    );

                throw new PftSyntaxException(this);
            }

            using (var guard = new PftContextGuard(context))
            {
                var clone = guard.ChildContext;
                clone.Evaluate(LeftOperand);
                var leftValue = LeftOperand.Value;
                clone.Evaluate(RightOperand);
                var rightValue = RightOperand.Value;
                Value = DoOperation
                    (
                        context,
                        leftValue,
                        Operation.ThrowIfNull(),
                        rightValue
                    );
            }

            OnAfterExecution(context);
        } // method Execute

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            var result = new PftNodeInfo
            {
                Node = this,
                Name = SimplifyTypeName(GetType().Name)
            };

            if (LeftOperand is { } left)
            {
                var leftNode = new PftNodeInfo
                {
                    Node = left,
                    Name = "Left"
                };
                result.Children.Add(leftNode);
                leftNode.Children.Add(LeftOperand.GetNodeInfo());
            }

            if (!string.IsNullOrEmpty(Operation))
            {
                var operationNode = new PftNodeInfo
                {
                    Name = "Operation",
                    Value = Operation
                };
                result.Children.Add(operationNode);
            }

            if (RightOperand is { } right)
            {
                var rightNode = new PftNodeInfo
                {
                    Node = right,
                    Name = "Right"
                };
                result.Children.Add(rightNode);
                rightNode.Children.Add(RightOperand.GetNodeInfo());
            }

            return result;
        } // method GetNodeInfo

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode Optimize()
        {
            if (LeftOperand is { } left)
            {
                LeftOperand = (PftNumeric?) left.Optimize();
            }

            if (RightOperand is { } right)
            {
                RightOperand = (PftNumeric?) right.Optimize();
            }

            return this;
        } // method Optimize

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            LeftOperand?.PrettyPrint(printer);
            printer.Write(" {0} ", Operation ?? String.Empty);
            RightOperand?.PrettyPrint(printer);
        } // method PrettyPrint

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.SerializeNullable(writer, LeftOperand);
            writer.WriteNullable(Operation);
            PftSerializer.SerializeNullable(writer, RightOperand);
        } // method Serialize

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append(LeftOperand);
            result.Append(Operation);
            result.Append(RightOperand);

            return result.ToString();
        } // method ToString

        #endregion

    } // class PftNumericExpression

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
