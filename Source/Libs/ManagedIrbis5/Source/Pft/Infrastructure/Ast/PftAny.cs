// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftAny.cs -- проверяет, выполняется ли указанное условие хотя бы для одного повторения в группе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AM;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Проверяет, выполняется ли указанное условие хотя бы для одного повторения в группе.
/// </summary>
public sealed class PftAny
    : PftCondition
{
    #region Properties

    /// <summary>
    /// Проверяемое условие.
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
                if (!ReferenceEquals (InnerCondition, null))
                {
                    var nodes = new List<PftNode>
                    {
                        InnerCondition
                    };
                    _virtualChildren.SetChildren (nodes);
                }
            }

            return _virtualChildren;
        }

        [ExcludeFromCodeCoverage]
        protected set
        {
            Magna.Logger.LogError
                (
                    nameof (PftAny) + "::" + nameof (Children)
                    + ": set value={Value}",
                    value.ToVisibleString()
                );
        }
    }

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax => true;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftAny()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftAny
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.Any);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftAny
        (
            PftCondition condition
        )
    {
        Sure.NotNull (condition);

        InnerCondition = condition;
    }

    #endregion

    #region Private members

    private VirtualChildren? _virtualChildren;

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public override object Clone()
    {
        var result = (PftAny) base.Clone();

        if (!ReferenceEquals (InnerCondition, null))
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
                ((PftAny)otherNode).InnerCondition
            );
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

        if (context.CurrentGroup is not null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftAny) + "::" + nameof (Execute)
                    + ": nested group detected"
                );

            throw new PftSemanticException ("Nested group");
        }

        var condition = InnerCondition.ThrowIfNull();

        var group = new PftGroup();

        try
        {
            context.CurrentGroup = group;
            context.VMonitor = false;

            OnBeforeExecution (context);

            var value = false;

            for (
                    context.Index = 0;
                    context.Index < PftConfig.MaxRepeat;
                    context.Index++
                )
            {
                context.VMonitor = false;

                condition.Execute (context);

                if (!context.VMonitor || context.BreakFlag)
                {
                    break;
                }

                value = condition.Value;
                if (value)
                {
                    break;
                }
            }

            Value = value;

            OnAfterExecution (context);
        }
        finally
        {
            context.CurrentGroup = null;
        }
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

        printer
            .SingleSpace()
            .Write ("any(");
        InnerCondition?.PrettyPrint (printer);
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

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    [DebuggerStepThrough]
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("any(");
        PftUtility.NodesToText (builder, Children);
        builder.Append (')');

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
