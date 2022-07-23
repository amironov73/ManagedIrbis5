// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftWhile.cs --
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

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// While loop.
/// </summary>
/// <example>
/// $x=0;
/// while $x &lt; 10
/// do
///     $x, ') ',
///     'Прикольно же!'
///     #
///     $x=$x+1;
/// end
/// </example>
public sealed class PftWhile
    : PftNode
{
    #region Properties

    /// <summary>
    /// Условие.
    /// </summary>
    public PftCondition? Condition { get; set; }

    /// <summary>
    /// Тело цикла.
    /// </summary>
    public PftNodeCollection Body { get; private set; }

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax => true;

    /// <inheritdoc cref="PftNode.Children" />
    public override IList<PftNode> Children
    {
        get
        {
            if (_virtualChildren is null)
            {
                _virtualChildren = new VirtualChildren();
                var nodes = new List<PftNode>();
                if (!ReferenceEquals (Condition, null))
                {
                    nodes.Add (Condition);
                }

                nodes.AddRange (Body);
                _virtualChildren.SetChildren (nodes);
            }

            return _virtualChildren;
        }

        [ExcludeFromCodeCoverage]
        protected set
        {
            // Nothing to do here

            Magna.Logger.LogError
                (
                    nameof (PftWhile) + "::" + nameof (Children)
                    + ": set value={Value}",
                    value.ToVisibleString()
                );
        }
    }

    /// <inheritdoc cref="PftNode.ComplexExpression" />
    public override bool ComplexExpression
    {
        get { return true; }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftWhile()
    {
        Body = new PftNodeCollection (this);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftWhile
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.While);

        Body = new PftNodeCollection (this);
    }

    /// <summary>
    /// Конструуктор.
    /// </summary>
    public PftWhile
        (
            PftCondition condition,
            params PftNode[] body
        )
        : this()
    {
        Condition = condition;
        foreach (var node in body)
        {
            Body.Add (node);
        }
    }

    #endregion

    #region Private members

    private VirtualChildren? _virtualChildren;

    private bool EvaluateCondition
        (
            PftContext context
        )
    {
        if (Condition is null)
        {
            return true;
        }

        Condition.Execute (context);

        return Condition.Value;
    }

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public override object Clone()
    {
        var result = (PftWhile) base.Clone();

        result._virtualChildren = null;

        if (Condition is not null)
        {
            result.Condition = (PftCondition) Condition.Clone();
        }

        result.Body = Body.CloneNodes (result).ThrowIfNull();

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

        var otherWhile = (PftWhile) otherNode;
        PftSerializationUtility.CompareNodes
            (
                Condition,
                otherWhile.Condition
            );
        PftSerializationUtility.CompareLists
            (
                Body,
                otherWhile.Body
            );
    }

    /// <inheritdoc cref="PftNode.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        Sure.NotNull (compiler);

        if (Condition is null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftWhile) + "::" + nameof (Compile)
                    + ": condition not set"
                );
            throw new PftCompilerException ("Condition not set");
        }

        Condition.Compile (compiler);
        compiler.CompileNodes (Body);

        compiler.StartMethod (this);

        // TODO handle break

        compiler
            .Write ("while (")
            .RefNodeMethod (Condition)
            .WriteLine ("())")
            .WriteIndent()
            .WriteLine ("{")
            .IncreaseIndent();

        compiler.CallNodes (Body);

        compiler
            .DecreaseIndent()
            .WriteIndent()
            .WriteLine ("}");

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

        Condition = (PftCondition?) PftSerializer.DeserializeNullable (reader);
        PftSerializer.Deserialize (reader, Body);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        try
        {
            while (EvaluateCondition (context))
            {
                context.Execute (Body);
            }
        }
        catch (PftBreakException exception)
        {
            // It was break operator

            Magna.Logger.LogError
                (
                    exception,
                    nameof (PftWhile) + "::" + nameof (Execute)
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
            Name = "While"
        };

        if (!ReferenceEquals (Condition, null))
        {
            var condition = new PftNodeInfo
            {
                Node = Condition,
                Name = "Condition"
            };
            result.Children.Add (condition);
            condition.Children.Add (Condition.GetNodeInfo());
        }

        var body = new PftNodeInfo
        {
            Name = "Body"
        };
        result.Children.Add (body);
        foreach (var node in Body)
        {
            body.Children.Add (node.GetNodeInfo());
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

        printer.EatWhitespace();
        printer.EatNewLine();

        printer
            .WriteLine()
            .WriteIndent()
            .Write ("while ");

        Condition?.PrettyPrint (printer);

        printer
            .WriteLine()
            .WriteLine ("do");

        printer
            .IncreaseLevel()
            .WriteIndent()
            .WriteNodes (Body);
        printer.EatWhitespace();
        printer.EatNewLine();
        printer
            .DecreaseLevel()
            .WriteLine()
            .WriteIndent()
            .Write ("end");
        if (!ReferenceEquals (Condition, null))
        {
            printer.Write (" /* while ");
            Condition.PrettyPrint (printer);
        }

        printer.WriteLine();
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        PftSerializer.SerializeNullable (writer, Condition);
        PftSerializer.Serialize (writer, Body);
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    [DebuggerStepThrough]
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="Object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("while ");
        builder.Append (Condition);
        builder.Append (" do");
        foreach (var node in Body)
        {
            builder.Append (' ');
            builder.Append (node);
        }

        builder.Append (" end");

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
