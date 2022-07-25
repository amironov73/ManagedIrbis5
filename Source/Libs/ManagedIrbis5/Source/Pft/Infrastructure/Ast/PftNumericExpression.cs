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

using AM;
using AM.IO;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Математическое выражение.
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
            if (ReferenceEquals (_virtualChildren, null))
            {
                _virtualChildren = new VirtualChildren();
                var nodes = new List<PftNode>();
                if (!ReferenceEquals (LeftOperand, null))
                {
                    nodes.Add (LeftOperand);
                }

                if (!ReferenceEquals (RightOperand, null))
                {
                    nodes.Add (RightOperand);
                }

                _virtualChildren.SetChildren (nodes);
            }

            return _virtualChildren;
        }

        [ExcludeFromCodeCoverage]
        protected set
        {
            Magna.Logger.LogError
                (
                    nameof (PftNumericExpression) + "::" + nameof (Children)
                    + ": set value={Value}",
                    value.ToVisibleString()
                );
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftNumericExpression()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftNumericExpression
        (
            PftToken token
        )
        : base (token)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftNumericExpression
        (
            PftNumeric leftOperand,
            string operation,
            PftNumeric rightOperand
        )
    {
        Sure.NotNull (leftOperand);
        Sure.NotNullNorEmpty (operation);
        Sure.NotNull (rightOperand);

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
            PftContext context,
            double leftValue,
            string operation,
            double rightValue
        )
    {
        Sure.NotNull (context);
        Sure.NotNullNorEmpty (operation);

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
                result = (int) leftValue % (int) rightValue;

                // ReSharper enable PossibleLossOfFraction
                break;

            case "div":
                // ReSharper disable PossibleLossOfFraction
                result = (int) leftValue / (int) rightValue;

                // ReSharper enable PossibleLossOfFraction
                break;

            default:
                Magna.Logger.LogError
                    (
                        nameof (PftNumericExpression) + "::" + nameof (DoOperation)
                        + ": unexpected operation={Operation}",
                        operation
                    );

                throw new PftSyntaxException ("Unexpected operation " + operation);
        }

        return result;
    }

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public override object Clone()
    {
        var result = (PftNumericExpression) base.Clone();

        if (LeftOperand is not null)
        {
            result.LeftOperand = (PftNumeric) LeftOperand.Clone();
        }

        if (RightOperand is not null)
        {
            result.RightOperand = (PftNumeric) RightOperand.Clone();
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
        Sure.NotNull (otherNode);

        base.CompareNode (otherNode);

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
    }

    /// <inheritdoc cref="PftNode.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        Sure.NotNull (compiler);

        if (LeftOperand is null
            || RightOperand is null
            || string.IsNullOrEmpty (Operation))
        {
            throw new PftCompilerException();
        }

        LeftOperand.Compile (compiler);
        RightOperand.Compile (compiler);

        compiler.StartMethod (this);

        compiler
            .WriteIndent()
            .WriteLine ("PftContext previousContext = StartEvaluate();");

        compiler
            .WriteIndent()
            .Write ("double left = ")
            .CallNodeMethod (LeftOperand)
            .WriteLine();
        compiler
            .WriteIndent()
            .Write ("double right = ")
            .CallNodeMethod (RightOperand)
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
            .WriteLine ("EndEvaluate(previousContext);")
            .WriteIndent()
            .WriteLine ("return result;");

        compiler.EndMethod (this);
        compiler.MarkReady (this);
    }

    /// <inheritdoc cref="PftNode.Deserialize" />
    protected internal override void Deserialize
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        base.Deserialize (reader);

        LeftOperand = (PftNumeric?) PftSerializer.DeserializeNullable (reader);
        Operation = reader.ReadNullableString();
        RightOperand = (PftNumeric?) PftSerializer.DeserializeNullable (reader);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        if (LeftOperand is null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftNumericExpression) + "::" + nameof (Execute)
                    + ": left operand is not specified"
                );

            throw new PftSyntaxException (this);
        }

        if (string.IsNullOrEmpty (Operation))
        {
            Magna.Logger.LogError
                (
                    nameof (PftNumericExpression) + "::" + nameof (Execute)
                    + ": operation is not specified"
                );

            throw new PftSyntaxException (this);
        }

        if (RightOperand is null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftNumericExpression) + "::" + nameof (Execute)
                    + ": right operand is not specified"
                );

            throw new PftSyntaxException (this);
        }

        using (var guard = new PftContextGuard (context))
        {
            var clone = guard.ChildContext;
            clone.Evaluate (LeftOperand);
            var leftValue = LeftOperand.Value;
            clone.Evaluate (RightOperand);
            var rightValue = RightOperand.Value;
            Value = DoOperation
                (
                    context,
                    leftValue,
                    Operation.ThrowIfNull(),
                    rightValue
                );
        }

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.GetNodeInfo" />
    public override PftNodeInfo GetNodeInfo()
    {
        var result = new PftNodeInfo
        {
            Node = this,
            Name = SimplifyTypeName (GetType().Name)
        };

        if (LeftOperand is not null)
        {
            var leftNode = new PftNodeInfo
            {
                Node = LeftOperand,
                Name = "Left"
            };
            result.Children.Add (leftNode);
            leftNode.Children.Add (LeftOperand.GetNodeInfo());
        }

        if (!string.IsNullOrEmpty (Operation))
        {
            var operationNode = new PftNodeInfo
            {
                Name = "Operation",
                Value = Operation
            };
            result.Children.Add (operationNode);
        }

        if (RightOperand is not null)
        {
            var rightNode = new PftNodeInfo
            {
                Node = RightOperand,
                Name = "Right"
            };
            result.Children.Add (rightNode);
            rightNode.Children.Add (RightOperand.GetNodeInfo());
        }

        return result;
    }

    /// <inheritdoc cref="PftNode.Optimize" />
    public override PftNode Optimize()
    {
        LeftOperand = (PftNumeric?)LeftOperand?.Optimize();
        RightOperand = (PftNumeric?)RightOperand?.Optimize();

        return this;
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        LeftOperand?.PrettyPrint (printer);
        printer.Write (" {0} ", Operation ?? string.Empty);
        RightOperand?.PrettyPrint (printer);
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        PftSerializer.SerializeNullable (writer, LeftOperand);
        writer.WriteNullable (Operation);
        PftSerializer.SerializeNullable (writer, RightOperand);
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append (LeftOperand);
        builder.Append (Operation);
        builder.Append (RightOperand);

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
