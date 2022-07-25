// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* PftParallelForEach.cs -- параллельная версия цикла "для каждого"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AM;
using AM.Linq;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Параллельная версия цикла "для каждого".
/// <example>
/// parallel foreach $x in (v692^g,/)
/// do
///     $x, #
///     if $x:'2010' then break fi
/// end
/// </example>
/// </summary>
public sealed class PftParallelForEach
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

    /// <inheritdoc cref="PftNode.ComplexExpression" />
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
        protected set
        {
            Magna.Logger.LogError
                (
                    nameof (PftParallelForEach) + "::" + nameof (Children)
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
    public PftParallelForEach()
    {
        Sequence = new PftNodeCollection (this);
        Body = new PftNodeCollection (this);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftParallelForEach
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.Parallel);

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
        var result = (PftParallelForEach) base.Clone();

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
            // TODO: implement properly
            foreach (var item in items)
            {
                context.Variables.SetVariable (name, item);

                context.Execute (Body);
            }
        }
        catch (PftBreakException exception)
        {
            // It was break operator

            Magna.Logger.LogError
                (
                    exception,
                    nameof (PftParallelForEach) + "::" + nameof (Execute)
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
            Name = "ParallelForEach"
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
    } // method GetNodeInfo

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        printer.EatWhitespace();
        printer.EatNewLine();

        printer
            .WriteLine()
            .WriteIndent()
            .Write ("parallel foreach ");

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

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var result = new StringBuilder();
        result.Append ("parallel foreach ");
        result.Append (Variable);
        result.Append (" in ");
        PftUtility.NodesToText (result, Sequence);
        result.Append (" do ");
        PftUtility.NodesToText (result, Body);
        result.Append (" end");

        return result.ToString();
    } // method ToString

    #endregion
} // class ParallelForEach

// namespace ManagedIrbis.Pft.Infrastructure.Ast
