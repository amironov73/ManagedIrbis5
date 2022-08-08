// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftComparison.cs -- операция сравнения двух операндов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;

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
/// Операция сравнения двух операндов
/// </summary>
public sealed class PftComparison
    : PftCondition
{
    #region Properties

    /// <summary>
    /// Левый операнд.
    /// </summary>
    public PftNode? LeftOperand { get; set; }

    /// <summary>
    /// Операция.
    /// </summary>
    public string? Operation { get; set; }

    /// <summary>
    /// Правый операнд.
    /// </summary>
    public PftNode? RightOperand { get; set; }

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax
    {
        get
        {
            return Operation switch
            {
                "::" or "~" or "~~" or "!~" or "!~~" or "==" or "===" => true,
                _ => base.ExtendedSyntax
            };
        }
    }

    /// <inheritdoc cref="PftNode.Children" />
    public override IList<PftNode> Children
    {
        get
        {
            if (_virtualChildren is null)
            {
                _virtualChildren = new VirtualChildren();
                var operationNode = new PftNode
                {
                    Text = Operation
                };
                var nodes = new List<PftNode>
                {
                    LeftOperand!, // TODO: исправить логику
                    operationNode,
                    RightOperand! // TODO: исправить логику
                };
                _virtualChildren.SetChildren (nodes);
            }

            return _virtualChildren;
        }

        [ExcludeFromCodeCoverage]
        protected set
        {
            // Nothing to do here
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftComparison()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftComparison
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
    public PftComparison
        (
            PftNode leftOperand,
            string operation,
            PftNode rightOperand
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
    /// Сравнение двух чисел.
    /// </summary>
    public static bool DoNumericOperation
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

        bool result;
        switch (operation)
        {
            case "<":
                result = leftValue < rightValue;
                break;

            case "<=":
                result = leftValue <= rightValue;
                break;

            case "=":
            case "==":
                // Original PFT behavior: exact number comparison
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                result = leftValue == rightValue; //-V3024
                break;

            case "!=":
            case "<>":
                // Original PFT behavior: exact number comparison
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                result = leftValue != rightValue; //-V3024
                break;

            case ">":
                result = leftValue > rightValue;
                break;

            case ">=":
                result = leftValue >= rightValue;
                break;

            default:
                Magna.Logger.LogError
                    (
                        nameof (PftComparison) + "::" + nameof (DoNumericOperation)
                        + ": unexpected operation {Operation}",
                        operation
                    );

                throw new PftSyntaxException();
        }

        Magna.Logger.LogTrace
            (
                nameof (PftComparison) + "::" + nameof (DoNumericOperation)
                + ": left={Left}, operation={Operation}, right={Right}, result={Result}",
                leftValue,
                operation,
                rightValue,
                result
            );

        return result;
    }

    /// <summary>
    /// Сравнение двух строк.
    /// </summary>
    public static bool DoStringOperation
        (
            PftContext context,
            string leftValue,
            string operation,
            string rightValue
        )
    {
        Sure.NotNull (context);
        Sure.NotNullNorEmpty (operation);

        operation = operation.ToLowerInvariant();

        bool result;
        switch (operation)
        {
            case ":":
                result = PftUtility.ContainsSubString
                    (
                        leftValue,
                        rightValue
                    );
                break;

            case "::":
                result = PftUtility.ContainsSubStringSensitive
                    (
                        leftValue,
                        rightValue
                    );
                break;

            case "=":
                result = leftValue.SameString (rightValue);
                break;

            case "==":
                result = leftValue.SameStringSensitive
                    (
                        rightValue
                    );
                break;

            case "!=":
            case "<>":
                result = !leftValue.SameString
                    (
                        rightValue
                    );
                break;

            case "!==":
                result = !leftValue.SameStringSensitive
                    (
                        rightValue
                    );
                break;

            case "<":
                result = PftUtility.CompareStrings
                             (
                                 leftValue,
                                 rightValue
                             )
                         < 0;
                break;

            case "<=":
                result = PftUtility.CompareStrings
                             (
                                 leftValue,
                                 rightValue
                             )
                         <= 0;
                break;

            case ">":
                result = PftUtility.CompareStrings
                             (
                                 leftValue,
                                 rightValue
                             )
                         > 0;
                break;

            case ">=":
                result = PftUtility.CompareStrings
                             (
                                 leftValue,
                                 rightValue
                             )
                         >= 0;
                break;

            case "~":
                result = Regex.IsMatch
                    (
                        leftValue,
                        rightValue,
                        RegexOptions.IgnoreCase
                    );
                break;

            case "~~":
                result = Regex.IsMatch
                    (
                        leftValue,
                        rightValue
                    );
                break;

            case "!~":
                result = !Regex.IsMatch
                    (
                        leftValue,
                        rightValue,
                        RegexOptions.IgnoreCase
                    );
                break;

            case "!~~":
                result = !Regex.IsMatch
                    (
                        leftValue,
                        rightValue
                    );
                break;

            default:
                Magna.Logger.LogError
                    (
                        nameof (PftComparison) + "::" + nameof (DoStringOperation)
                        + ": unexpected operation {Operation}",
                        operation
                    );

                throw new PftSyntaxException();
        }

        Magna.Logger.LogTrace
            (
                nameof (PftComparison) + "::" + nameof (DoStringOperation)
                + "left={Left}, operation={Operation}, right={Right}, result={Result}",
                leftValue,
                operation,
                rightValue,
                result
            );

        return result;
    }

    private double GetValue
        (
            PftContext context,
            PftNode node
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (node);

        var stringValue = context.Evaluate (node);

        if (node is not PftNumeric numeric)
        {
            double.TryParse (stringValue, out var result);

            return result;
        }

        return numeric.Value;
    }

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public override object Clone()
    {
        var result = (PftComparison)base.Clone();

        result._virtualChildren = null;

        if (!ReferenceEquals (LeftOperand, null))
        {
            result.LeftOperand = (PftNode)LeftOperand.Clone();
        }

        if (!ReferenceEquals (RightOperand, null))
        {
            result.RightOperand = (PftNode)RightOperand.Clone();
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

        var otherComparison = (PftComparison)otherNode;
        PftSerializationUtility.CompareNodes
            (
                LeftOperand,
                otherComparison.LeftOperand
            );
        if (Operation != otherComparison.Operation)
        {
            throw new IrbisException();
        }

        PftSerializationUtility.CompareNodes
            (
                RightOperand,
                otherComparison.RightOperand
            );
    }

    /// <inheritdoc cref="PftNode.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        Sure.NotNull (compiler);

        if (ReferenceEquals (LeftOperand, null)
            || ReferenceEquals (RightOperand, null)
            || string.IsNullOrEmpty (Operation))
        {
            throw new PftCompilerException();
        }

        LeftOperand.Compile (compiler);
        RightOperand.Compile (compiler);

        compiler.StartMethod (this);

        var context = new PftContext (null);
        if (PftUtility.IsNumeric (context, LeftOperand))
        {
            compiler
                .WriteIndent()
                .Write ("double left = ")
                .CallNodeMethod (LeftOperand);

            compiler
                .WriteIndent()
                .Write ("double right = ")
                .CallNodeMethod (RightOperand);

            compiler
                .WriteIndent()
                .WriteLine ("string operation = \"{0}\";", Operation);

            // TODO implement properly
            compiler
                .WriteIndent()
                .WriteLine
                    (
                        "bool result = PftComparison" +
                        ".DoNumericOperation(Context, left, operation, right);"
                    );
        }
        else
        {
            compiler
                .WriteIndent()
                .Write ("string left = Evaluate(")
                .RefNodeMethod (LeftOperand)
                .WriteLine (");");
            compiler
                .WriteIndent()
                .Write ("string right = Evaluate(")
                .RefNodeMethod (RightOperand)
                .WriteLine (");");
            compiler
                .WriteIndent()
                .WriteLine ("string operation = \"{0}\";", Operation);

            // TODO implement properly
            compiler
                .WriteIndent()
                .WriteLine
                    (
                        "bool result = PftComparison" +
                        ".DoStringOperation(Context, left, operation, right);"
                    );
        }

        compiler
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

        LeftOperand = PftSerializer.DeserializeNullable (reader);
        Operation = reader.ReadNullableString();
        RightOperand = PftSerializer.DeserializeNullable (reader);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        var operation = Operation.ThrowIfNull();

        if (LeftOperand is null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftComparison) + "::" + nameof (Execute)
                    + ": LeftOperand not set"
                );

            throw new PftSyntaxException (this);
        }

        if (RightOperand is null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftComparison) + "::" + nameof (Execute)
                    + ": RightOperand not set"
                );

            throw new PftSyntaxException (this);
        }

        var leftNumeric = PftUtility.IsNumeric
            (
                context,
                LeftOperand
            );
        var rightNumeric = PftUtility.IsNumeric
            (
                context,
                RightOperand
            );

        if (leftNumeric || rightNumeric)
        {
            var leftValue = GetValue (context, LeftOperand);
            var rightValue = GetValue (context, RightOperand);
            Value = DoNumericOperation
                (
                    context,
                    leftValue,
                    operation,
                    rightValue
                );
        }
        else
        {
            var leftValue = context.Evaluate (LeftOperand);
            var rightValue = context.Evaluate (RightOperand);
            Value = DoStringOperation
                (
                    context,
                    leftValue,
                    operation,
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

        if (!ReferenceEquals (LeftOperand, null))
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

        if (!ReferenceEquals (RightOperand, null))
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
        if (ReferenceEquals (LeftOperand, null)
            || ReferenceEquals (RightOperand, null)
            || string.IsNullOrEmpty (Operation))
        {
            throw new PftCompilerException();
        }

        LeftOperand = LeftOperand.Optimize();
        RightOperand = RightOperand.Optimize();

        return this;
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        if (!ReferenceEquals (LeftOperand, null))
        {
            LeftOperand.PrettyPrint (printer);
        }

        printer.Write (Operation);

        if (!ReferenceEquals (RightOperand, null))
        {
            RightOperand.PrettyPrint (printer);
        }
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

        if (LeftOperand is not null)
        {
            builder.Append (LeftOperand);
        }

        builder.Append (Operation);

        if (RightOperand is not null)
        {
            builder.Append (RightOperand);
        }

        return builder.ReturnShared();
    }

    #endregion
}
