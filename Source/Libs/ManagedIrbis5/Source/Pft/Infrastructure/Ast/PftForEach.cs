// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PftForEach.cs -- цикл "для каждого"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.Linq;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// foreach $x in (v692^g,/)
/// do
///     $x, #
///     if $x:'2010' then break fi
/// end
/// </summary>
public sealed class PftForEach
    : PftNode
{
    #region Properties

    /// <summary>
    /// Variable reference.
    /// </summary>
    public PftVariableReference? Variable { get; set; }

    /// <summary>
    /// Sequence.
    /// </summary>
    public PftNodeCollection Sequence { get; private set; }

    /// <summary>
    /// Body.
    /// </summary>
    public PftNodeCollection Body { get; private set; }

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax => true;

    /// <inheritdoc cref="PftNode.ComplexExpression"/>
    public override bool ComplexExpression => true;

    /// <inheritdoc cref="PftNode.Children" />
    public override IList<PftNode> Children
    {
        get
        {
            if (ReferenceEquals (_virtualChildren, null))
            {
                _virtualChildren = new VirtualChildren();
                var nodes = new List<PftNode>();
                nodes.AddRange (Sequence);
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
                    nameof (PftForEach) + "::" + nameof (Children)
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
    public PftForEach()
    {
        Sequence = new PftNodeCollection (this);
        Body = new PftNodeCollection (this);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftForEach
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.ForEach);

        Sequence = new PftNodeCollection (this);
        Body = new PftNodeCollection (this);
    }

    #endregion

    #region Private members

    private VirtualChildren? _virtualChildren;

    private string[] GetSequence
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        var result = new List<string>();
        foreach (var node in Sequence)
        {
            var text = context.Evaluate (node);
            if (!string.IsNullOrEmpty (text))
            {
                var lines = text.SplitLines()
                    .NonEmptyLines()
                    .ToArray();
                result.AddRange (lines);
            }
        }

        return result.ToArray();
    }

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public override object Clone()
    {
        var result = (PftForEach )base.Clone();
        result._virtualChildren = null;
        result.Sequence = Sequence.CloneNodes (result).ThrowIfNull();
        result.Body = Body.CloneNodes (result).ThrowIfNull();

        if (Variable is not null)
        {
            result.Variable = (PftVariableReference) Variable.Clone();
        }

        return result;
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.CompareNode"/>
    internal override void CompareNode
        (
            PftNode otherNode
        )
    {
        Sure.NotNull (otherNode);

        base.CompareNode (otherNode);

        var otherForEach = (PftForEach) otherNode;
        PftSerializationUtility.CompareNodes
            (
                Variable,
                otherForEach.Variable
            );
        PftSerializationUtility.CompareLists
            (
                Sequence,
                otherForEach.Sequence
            );
        PftSerializationUtility.CompareLists
            (
                Body,
                otherForEach.Body
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

        Variable = (PftVariableReference?) PftSerializer.DeserializeNullable (reader);
        PftSerializer.Deserialize (reader, Sequence);
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

        var variable = Variable.ThrowIfNull();
        var name = variable.Name.ThrowIfNull();

        var items = GetSequence (context);
        try
        {
            foreach (var item in items)
            {
                context.Variables.SetVariable (name, item);

                context.Execute (Body);
            }
        }
        catch (PftBreakException exception)
        {
            // It was break operator

            Magna.Logger.LogTrace
                (
                    exception,
                    nameof (PftForEach) + "::" + nameof (Execute)
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
            Name = "ForEach"
        };

        if (!ReferenceEquals (Variable, null))
        {
            result.Children.Add (Variable.GetNodeInfo());
        }

        var sequence = new PftNodeInfo
        {
            Name = "Sequence"
        };

        result.Children.Add (sequence);

        foreach (var node in Sequence)
        {
            sequence.Children.Add (node.GetNodeInfo());
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
            .Write ("foreach ");

        Variable?.PrettyPrint (printer);
        printer.Write (" in ");

        var first = true;
        foreach (var node in Sequence)
        {
            if (!first)
            {
                printer.Write (", ");
            }

            node.PrettyPrint (printer);
            first = false;
        }

        printer
            .WriteIndent()
            .WriteLine ("do");

        printer.IncreaseLevel();
        printer.WriteNodes (Body);
        printer.DecreaseLevel();
        printer.EatWhitespace();
        printer.EatNewLine();
        printer.WriteLine();
        printer
            .WriteIndent()
            .WriteLine ("end");
    } // method PrettyPrint

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        PftSerializer.SerializeNullable (writer, Variable);
        PftSerializer.Serialize (writer, Sequence);
        PftSerializer.Serialize (writer, Body);
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("foreach ");
        builder.Append (Variable);
        builder.Append (" in ");
        PftUtility.NodesToText (builder, Sequence);
        builder.Append (" do ");
        PftUtility.NodesToText (builder, Body);
        builder.Append (" end");

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
