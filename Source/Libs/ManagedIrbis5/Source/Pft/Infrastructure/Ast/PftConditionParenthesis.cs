// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftConditionParenthesis.cs -- составное условие - в скобках
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AM;
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
/// Составное условие - в скобках.
/// </summary>
public sealed class PftConditionParenthesis
    : PftCondition
{
    #region Properties

    /// <summary>
    /// Внутрненее условие.
    /// </summary>
    public PftCondition? InnerCondition { get; set; }

    /// <inheritdoc cref="PftNode.Children" />
    public override IList<PftNode> Children
    {
        get
        {
            if (ReferenceEquals (_virtualChildren, null))
            {
                _virtualChildren = new VirtualChildren();
                var nodes = new List<PftNode>();
                if (!ReferenceEquals (InnerCondition, null))
                {
                    nodes.Add (InnerCondition);
                }

                _virtualChildren.SetChildren (nodes);
            }

            return _virtualChildren;
        }

        [ExcludeFromCodeCoverage]
        protected set => Magna.Logger.LogError
            (
                nameof (PftConditionParenthesis) + "::" + nameof (Children)
                + ":set value={Value}",
                value.ToVisibleString()
            );
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftConditionParenthesis()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftConditionParenthesis
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
        var result = (PftConditionParenthesis)base.Clone();

        result._virtualChildren = null;

        if (InnerCondition is not null)
        {
            result.InnerCondition = (PftCondition)InnerCondition.Clone();
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

        PftSerializationUtility.CompareNodes
            (
                InnerCondition,
                ((PftConditionParenthesis)otherNode).InnerCondition
            );
    }

    /// <inheritdoc cref="PftNode.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        Sure.NotNull (compiler);

        if (InnerCondition is null)
        {
            throw new PftCompilerException();
        }

        InnerCondition.Compile (compiler);

        compiler.StartMethod (this);

        compiler
            .WriteIndent()
            .Write ("bool result = ")
            .CallNodeMethod (InnerCondition)
            .WriteLine (";")
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

        InnerCondition = (PftCondition?)PftSerializer.DeserializeNullable (reader);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        if (InnerCondition is null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftConditionParenthesis) + "::" + nameof (Execute)
                    + ": inner condition is not set"
                );

            throw new PftSyntaxException ("Inner condition is not set");
        }

        InnerCondition.Execute (context);
        Value = InnerCondition.Value;

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

        if (!ReferenceEquals (InnerCondition, null))
        {
            result.Children.Add (InnerCondition.GetNodeInfo());
        }

        return result;
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        printer.Write ('(');
        InnerCondition?.PrettyPrint (printer);
        printer.EatWhitespace();
        printer.Write (')');
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        PftSerializer.SerializeNullable (writer, InnerCondition);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ('(');
        if (!ReferenceEquals (InnerCondition, null))
        {
            builder.Append (InnerCondition);
        }

        builder.Append (')');

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
