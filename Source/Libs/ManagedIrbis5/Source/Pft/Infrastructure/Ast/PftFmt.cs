// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* PftFmt.cs -- форматирование чисел по принципам .NET
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
/// Форматирование чисел с плавающей точкой по принципам .NET.
/// </summary>
/// <example>
/// <code>
/// fmt(3+0.14,'E4')
/// </code>
/// </example>
public sealed class PftFmt
    : PftNode
{
    #region Properties

    /// <summary>
    /// Спецификация формата для числа.
    /// Если спецификация пуста, то применяется форматирование,
    /// принятое в .NET по умолчанию ("G").
    /// </summary>
    public PftNodeCollection Format { get; private set; }

    /// <summary>
    /// Собственно число, подлежащее форматированию.
    /// </summary>
    public PftNumeric? Number { get; set; }

    /// <inheritdoc cref="PftNode.Children" />
    public override IList<PftNode> Children
    {
        get
        {
            if (ReferenceEquals (_virtualChildren, null))
            {
                _virtualChildren = new VirtualChildren();
                var nodes = new List<PftNode>();
                if (Number is { } number)
                {
                    nodes.Add (number);
                }

                nodes.AddRange (Format);
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
                    nameof (PftFmt) + "::" + nameof (Children)
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
    public PftFmt()
    {
        Format = new PftNodeCollection (this);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftFmt
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.Fmt);

        Format = new PftNodeCollection (this);
    }

    #endregion

    #region Private members

    private VirtualChildren? _virtualChildren;

    #endregion

    #region Public methods

    /// <summary>
    /// Format the number according specified format.
    /// </summary>
    public static void FormatNumber
        (
            PftContext context,
            PftNode? node,
            double number,
            string? format
        )
    {
        Sure.NotNull (context);

        var output = string.IsNullOrEmpty (format)
            ? number.ToString (CultureInfo.InvariantCulture)
            : number.ToString (format, CultureInfo.InvariantCulture);

        context.Write (node, output);
    }

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public override object Clone()
    {
        var result = (PftFmt) base.Clone();
        result._virtualChildren = null;
        result.Format = Format.CloneNodes (result).ThrowIfNull();

        if (Number is not null)
        {
            result.Number = (PftNumeric)Number.Clone();
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

        var otherFmt = (PftFmt) otherNode;
        PftSerializationUtility.CompareLists
            (
                Format,
                otherFmt.Format
            );
        PftSerializationUtility.CompareNodes
            (
                Number,
                otherFmt.Number
            );
    }

    /// <inheritdoc cref="PftNode.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        Sure.NotNull (compiler);

        if (Number is null)
        {
            return;
        }

        var defaultFormat = Format.Count == 0
            ? "G"
            : null;

        Number.Compile (compiler);
        compiler.CompileNodes (Format);

        var actionName = compiler.CompileAction (Format);

        compiler.StartMethod (this);

        compiler
            .WriteIndent()
            .Write ("double value = ")
            .CallNodeMethod (Number);

        if (defaultFormat is null)
        {
            compiler
                .WriteIndent()
                .WriteLine ("string format = Evaluate({0});", actionName);
        }
        else
        {
            compiler
                .WriteIndent()
                .WriteLine ("string format = \"{0}\";", defaultFormat);
        }

        compiler
            .WriteIndent()
            .WriteLine ("string text = value.ToString(format, CultureInfo.InvariantCulture);")
            .WriteIndent()
            .WriteLine ("Context.Write(null, text);");

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

        PftSerializer.Deserialize (reader, Format);
        Number = (PftNumeric?) PftSerializer.DeserializeNullable (reader);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        if (Number is { } number)
        {
            number.Execute (context);
            var value = number.Value;
            var format = context.Evaluate (Format);

            FormatNumber
                (
                    context,
                    this,
                    value,
                    format
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
            Name = "Fmt"
        };

        if (Number is { } number)
        {
            result.Children.Add (number.GetNodeInfo());
        }

        if (Format.Count != 0)
        {
            var format = new PftNodeInfo
            {
                Name = "Format"
            };

            result.Children.Add (format);
            foreach (var node in Format)
            {
                format.Children.Add (node.GetNodeInfo());
            }
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
            .Write ("fmt(");
        Number?.PrettyPrint (printer);
        printer.EatWhitespace();
        printer
            .Write (", ")
            .Write (Format)
            .Write (')');
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        PftSerializer.Serialize (writer, Format);
        PftSerializer.SerializeNullable (writer, Number);
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("fmt(");
        if (Number is not null)
        {
            builder.Append (Number);
        }

        builder.Append (',');
        var first = true;
        foreach (var node in Format)
        {
            if (!first)
            {
                builder.Append (' ');
            }

            builder.Append (node);
            first = false;
        }

        builder.Append (')');

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
