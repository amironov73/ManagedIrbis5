// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftConditionNot.cs -- составное условие - отрицание
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AM;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Составное условие - отрицание.
/// </summary>
public sealed class PftConditionNot
    : PftCondition
{
    #region Properties

    /// <summary>
    /// Внутренее условие.
    /// </summary>
    public PftCondition? InnerCondition { get; set; }

    /// <inheritdoc cref="PftNode.Children" />
    public override IList<PftNode> Children
    {
        get
        {
            if (_virtualChildren is null)
            {
                _virtualChildren = new VirtualChildren();
                var nodes = new List<PftNode>();
                if (InnerCondition is not null)
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
                nameof (PftConditionNot) + "::" + nameof (Children)
                + ": set value={Value}",
                value.ToVisibleString()
            );
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftConditionNot()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftConditionNot
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

    /// <inheritdoc cref="PftNode.Clone" />
    public override object Clone()
    {
        var result = (PftConditionNot) base.Clone();

        result._virtualChildren = null;
        result.InnerCondition = (PftCondition?) InnerCondition?.Clone();

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
                ((PftConditionNot) otherNode).InnerCondition
            );
    }

    /// <inheritdoc cref="PftNode.Compile"/>
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
            .WriteLine ("return !result;");

        compiler.EndMethod (this);
        compiler.MarkReady (this);
    }

    /// <inheritdoc cref="PftNode.Deserialize" />
    protected internal override void Deserialize
        (
            BinaryReader reader
        )
    {
        Sure. NotNull (reader);

        base.Deserialize (reader);

        InnerCondition = (PftCondition?) PftSerializer.DeserializeNullable (reader);
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
                    nameof (PftConditionNot) + "::" + nameof (Execute)
                    + ": inner condition is not set"
                );

            throw new PftSyntaxException();
        }

        InnerCondition.Execute (context);
        Value = !InnerCondition.Value;

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.Optimize" />
    public override PftNode Optimize()
    {
        InnerCondition = (PftCondition?) InnerCondition?.Optimize();

        return this;
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        printer
            .SingleSpace()
            .Write ("not")
            .SingleSpace();
        if (!ReferenceEquals (InnerCondition, null))
        {
            InnerCondition.PrettyPrint (printer);
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

        PftSerializer.SerializeNullable (writer, InnerCondition);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="PftNode.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append (" not ");
        if (!ReferenceEquals (InnerCondition, null))
        {
            builder.Append (InnerCondition);
        }

        return builder.ReturnShared();
    }

    #endregion
}
