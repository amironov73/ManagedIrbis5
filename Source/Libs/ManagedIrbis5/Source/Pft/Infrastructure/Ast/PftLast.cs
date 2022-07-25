// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftLast.cs -- выдает номер последнего повторения поля, для которого выполняется условие
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
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
/// Выдаёт номер последнего повторения поля,
/// для которого выполняется заданное условие.
/// Если условие не выполняется, выдаёт 0.
/// </summary>
/// <example>
/// f(last(v910^d='ЧЗ'),0,0)
/// </example>
public sealed class PftLast
    : PftNumeric
{
    #region Properties

    /// <summary>
    /// Condition
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
            // Nothing to do here

            Magna.Logger.LogError
                (
                    nameof (PftLast) + "::" + nameof (Children)
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
    public PftLast()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftLast
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.Last);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftLast
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
        var result = (PftLast) base.Clone();

        if (InnerCondition is not null)
        {
            result.InnerCondition = (PftCondition) InnerCondition.Clone();
        }

        return result;
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Deserialize" />
    protected internal override void Deserialize
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

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

        if (context.CurrentGroup is not null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftLast) + "::" + nameof (Execute)
                    + ": nested group detected at {Token}",
                    this
                );

            throw new PftSemanticException ("Nested group detected: " + this);
        }

        var condition = InnerCondition.ThrowIfNull();

        var group = new PftGroup();
        try
        {
            context.CurrentGroup = group;
            context.VMonitor = false;

            OnBeforeExecution (context);

            Value = 0;

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

                if (condition.Value)
                {
                    Value = context.Index + 1;
                }
            }

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
        printer.EatWhitespace();
        printer
            .SingleSpace()
            .Write ("last(");
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
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("last(");
        PftUtility.NodesToText (builder, Children);
        builder.Append (')');

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
