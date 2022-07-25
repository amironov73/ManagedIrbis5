// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* PftParallelFor.cs -- параллельная версия цикла "для"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
/// Параллельная версия цикла "для".
/// </summary>
/// <example>
/// parallel for $x=0; $x &lt; 10; $x = $x+1;
/// do
///     $x, ') ',
///     'Прикольно же!'
///     #
/// end
/// </example>
public sealed class PftParallelFor
    : PftNode
{
    #region Properties

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax => true;

    /// <inheritdoc cref="PftNode.ComplexExpression" />
    public override bool ComplexExpression => true;

    /// <summary>
    /// Initialization.
    /// </summary>
    public PftNodeCollection Initialization { get; private set; }

    /// <summary>
    /// Condition.
    /// </summary>
    public PftCondition? Condition { get; set; }

    /// <summary>
    /// Loop statements.
    /// </summary>
    public PftNodeCollection Loop { get; private set; }

    /// <summary>
    /// Body.
    /// </summary>
    public PftNodeCollection Body { get; private set; }

    /// <inheritdoc cref="PftNode.Children" />
    public override IList<PftNode> Children
    {
        get
        {
            if (ReferenceEquals (_virtualChildren, null))
            {
                _virtualChildren = new VirtualChildren();
                var nodes = new List<PftNode>();
                nodes.AddRange (Initialization);
                if (!ReferenceEquals (Condition, null))
                {
                    nodes.Add (Condition);
                }

                nodes.AddRange (Loop);
                nodes.AddRange (Body);
                _virtualChildren.SetChildren (nodes);
            }

            return _virtualChildren;
        }
        protected set
        {
            Magna.Logger.LogError
                (
                    nameof (PftParallelFor) + "::" + nameof (Children)
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
    public PftParallelFor()
    {
        Initialization = new PftNodeCollection (this);
        Loop = new PftNodeCollection (this);
        Body = new PftNodeCollection (this);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftParallelFor
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.Parallel);

        Initialization = new PftNodeCollection (this);
        Loop = new PftNodeCollection (this);
        Body = new PftNodeCollection (this);
    }

    #endregion

    #region Private members

    private VirtualChildren? _virtualChildren;

    private bool _EvaluateCondition
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
        var result = (PftParallelFor) base.Clone();

        result._virtualChildren = null;

        result.Initialization = Initialization.CloneNodes (result).ThrowIfNull();
        result.Loop = Loop.CloneNodes (result).ThrowIfNull();
        result.Body = Body.CloneNodes (result).ThrowIfNull();

        if (Condition is not null)
        {
            result.Condition = (PftCondition) Condition.Clone();
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

        PftSerializer.Deserialize (reader, Initialization);
        Condition = (PftCondition?) PftSerializer.DeserializeNullable (reader);
        PftSerializer.Deserialize (reader, Loop);
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

        context.Execute (Initialization);

        try
        {
            var allIterations = new List<PftIteration>();
            while (_EvaluateCondition (context))
            {
                var iteration = new PftIteration
                    (
                        context,
                        Body,
                        0,
                        (iter, _) => iter.Context.Execute (iter.Nodes),
                        this,
                        false
                    );
                allIterations.Add (iteration);

                // TODO : fix this!
                context.Execute (Loop);
            }

            var tasks = allIterations
                .Select (iter => iter.Task)
                .ToArray();
            Task.WaitAll (tasks);

            foreach (var iteration in allIterations)
            {
                if (!ReferenceEquals (iteration.Exception, null))
                {
                    Magna.Logger.LogError
                        (
                            iteration.Exception,
                            nameof (PftParallelFor) + "::" + nameof (Execute)
                        );

                    throw new IrbisException
                        (
                            "Exception in parallel FOR",
                            iteration.Exception
                        );
                }

                context.Write
                    (
                        this,
                        iteration.Result
                    );
            }
        }
        catch (PftBreakException exception)
        {
            // It was break operator

            Magna.Logger.LogTrace
                (
                    exception,
                    nameof (PftParallelFor) + "::" + nameof (Execute)
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
            Name = "ParallelFor"
        };

        if (Initialization.Count != 0)
        {
            var init = new PftNodeInfo
            {
                Name = "Init"
            };
            result.Children.Add (init);
            foreach (var node in Initialization)
            {
                init.Children.Add (node.GetNodeInfo());
            }
        }

        if (Condition is not null)
        {
            var condition = new PftNodeInfo
            {
                Node = Condition,
                Name = "Condition"
            };
            result.Children.Add (condition);
            condition.Children.Add (Condition.GetNodeInfo());
        }

        if (Loop.Count != 0)
        {
            var loop = new PftNodeInfo
            {
                Name = "Loop"
            };
            result.Children.Add (loop);
            foreach (var node in Loop)
            {
                loop.Children.Add (node.GetNodeInfo());
            }
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
            .Write ("parallel for ");

        printer.WriteNodes (Initialization);
        printer.Write ("; ");

        Condition?.PrettyPrint (printer);
        printer.Write ("; ");

        printer.WriteNodes (Loop);
        printer.WriteLine (';');
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
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        PftSerializer.Serialize (writer, Initialization);
        PftSerializer.SerializeNullable (writer, Condition);
        PftSerializer.Serialize (writer, Loop);
        PftSerializer.Serialize (writer, Body);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("parallel for ");
        PftUtility.NodesToText (builder, Initialization);
        builder.Append (';');
        builder.Append (Condition);
        builder.Append (';');
        PftUtility.NodesToText (builder, Loop);
        builder.Append (" do ");
        PftUtility.NodesToText (builder, Body);
        builder.Append (" end");

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
