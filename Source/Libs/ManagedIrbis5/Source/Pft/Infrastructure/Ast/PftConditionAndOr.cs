// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftConditionAndOr.cs -- составное условие И/ИЛИ
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

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Составное условие И/ИЛИ.
/// </summary>
public sealed class PftConditionAndOr
    : PftCondition
{
    #region Properties

    /// <summary>
    /// Левый операнд.
    /// </summary>
    public PftCondition? LeftOperand { get; set; }

    /// <summary>
    /// Операция: И или ИЛИ.
    /// </summary>
    public string? Operation { get; set; }

    /// <summary>
    /// Правый операнд.
    /// </summary>
    public PftCondition? RightOperand { get; set; }

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
        protected set => Magna.Logger.LogError
            (
                nameof (PftConditionAndOr) + "::" + nameof (Children)
                + ": set value={Value}",
                value.ToVisibleString()
            );
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftConditionAndOr()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftConditionAndOr
        (
            PftToken token
        )
        : base (token)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Private members

    private VirtualChildren? _virtualChildren;

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public override object Clone()
    {
        var result = (PftConditionAndOr)base.Clone();

        result._virtualChildren = null;

        if (LeftOperand is not null)
        {
            result.LeftOperand = (PftCondition)LeftOperand.Clone();
        }

        if (RightOperand is not null)
        {
            result.RightOperand = (PftCondition)RightOperand.Clone();
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

        var otherCondition
            = (PftConditionAndOr)otherNode;
        PftSerializationUtility.CompareNodes
            (
                LeftOperand,
                otherCondition.LeftOperand
            );
        if (Operation != otherCondition.Operation)
        {
            throw new IrbisException();
        }

        PftSerializationUtility.CompareNodes
            (
                RightOperand,
                otherCondition.RightOperand
            );
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
            .Write ("bool result = ")
            .RefNodeMethod (LeftOperand)
            .Write ("() ");
        if (Operation.SameString ("and"))
        {
            compiler.Write ("&&");
        }
        else if (Operation.SameString ("or"))
        {
            compiler.Write ("||");
        }
        else
        {
            throw new PftCompilerException();
        }

        compiler
            .Write (' ')
            .RefNodeMethod (RightOperand)
            .WriteLine ("();")
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

        LeftOperand = (PftCondition?)PftSerializer.DeserializeNullable (reader);
        Operation = reader.ReadNullableString();
        RightOperand = (PftCondition?)PftSerializer.DeserializeNullable (reader);
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
                    nameof (PftConditionAndOr) + "::" + nameof (Execute)
                    + ": LeftOperand not set"
                );

            throw new PftSyntaxException();
        }

        if (RightOperand is null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftConditionAndOr) + "::" + nameof (Execute)
                    + ": RightOperand not set"
                );

            throw new PftSyntaxException();
        }

        if (string.IsNullOrEmpty (Operation))
        {
            Magna.Logger.LogError
                (
                    nameof (PftConditionAndOr) + "::" + nameof (Execute)
                    + ": operation not set"
                );

            throw new PftSyntaxException();
        }


        LeftOperand.Execute (context);
        var left = LeftOperand.Value;

        // TODO оптимизация: не вычислять правую часть, если не нужно
        RightOperand.Execute (context);
        var right = RightOperand.Value;

        if (Operation.SameString ("and"))
        {
            left = left && right;
        }
        else if (Operation.SameString ("or"))
        {
            left = left || right;
        }
        else
        {
            Magna.Logger.LogError
                (
                    nameof (PftConditionAndOr) + "::" + nameof (Execute)
                    + ": unexpected operation {Operation}",
                    Operation.ToVisibleString()
                );

            throw new PftSyntaxException();
        }

        Value = left;

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.GetNodeInfo" />
    public override PftNodeInfo GetNodeInfo()
    {
        var result = new PftNodeInfo
        {
            Node = this,
            Name = "ConditionAndOr"
        };

        if (!ReferenceEquals (LeftOperand, null))
        {
            var left = new PftNodeInfo
            {
                Node = LeftOperand,
                Name = "LeftOperand"
            };
            result.Children.Add (left);
            left.Children.Add (LeftOperand.GetNodeInfo());
        }

        var operation = new PftNodeInfo
        {
            Name = "Operation",
            Value = Operation
        };
        result.Children.Add (operation);

        if (!ReferenceEquals (RightOperand, null))
        {
            var right = new PftNodeInfo
            {
                Node = RightOperand,
                Name = "RightOperand"
            };
            result.Children.Add (right);
            right.Children.Add (RightOperand.GetNodeInfo());
        }

        return result;
    }

    /// <inheritdoc cref="PftNode.Optimize" />
    public override PftNode Optimize()
    {
        if (LeftOperand is not null)
        {
            LeftOperand = (PftCondition?)LeftOperand.Optimize();
        }

        if (RightOperand is not null)
        {
            RightOperand = (PftCondition?)RightOperand.Optimize();
        }

        return this;
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        if (!ReferenceEquals (LeftOperand, null))
        {
            LeftOperand.PrettyPrint (printer);
        }

        printer
            .SingleSpace()
            .Write (Operation)
            .SingleSpace();

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
        base.Serialize (writer);

        PftSerializer.SerializeNullable (writer, LeftOperand);
        writer.WriteNullable (Operation);
        PftSerializer.SerializeNullable (writer, RightOperand);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var result = new StringBuilder();
        if (!ReferenceEquals (LeftOperand, null))
        {
            result.Append (LeftOperand);
        }

        result.Append (' ');
        result.Append (Operation);
        result.Append (' ');
        if (!ReferenceEquals (RightOperand, null))
        {
            result.Append (RightOperand);
        }

        return result.ToString();
    }

    #endregion
}
