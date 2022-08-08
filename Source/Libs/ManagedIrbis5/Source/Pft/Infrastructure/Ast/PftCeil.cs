// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftCeil.cs -- округление вверх
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AM;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Округление вверх.
/// </summary>
public sealed class PftCeil
    : PftNumeric
{
    #region Properties

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax => true;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftCeil()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftCeil
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.Ceil);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftCeil
        (
            PftNumeric value
        )
    {
        Sure.NotNull (value);

        Children.Add (value);
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        Sure.NotNull (compiler);

        var child = Children.FirstOrDefault() as PftNumeric
                    ?? throw new PftCompilerException();

        child.Compile (compiler);

        compiler.StartMethod (this);

        compiler
            .WriteIndent()
            .WriteLine ("double value = ")
            .CallNodeMethod (child);

        compiler
            .WriteIndent()
            .WriteLine ("double result = Math.Ceiling (value);"); //-V3010

        compiler
            .WriteIndent()
            .WriteLine ("return result;");

        compiler.EndMethod (this);
        compiler.MarkReady (this);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        if (Children.FirstOrDefault() is PftNumeric child)
        {
            child.Execute (context);
            Value = Math.Ceiling (child.Value);
        }

        OnAfterExecution (context);
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
            .Write ("ceil(");
        base.PrettyPrint (printer);
        printer.Write (')');
    } // method PrettyPrint

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    [ExcludeFromCodeCoverage]
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="PftNode.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("ceil(");
        var child = Children.FirstOrDefault();
        if (!ReferenceEquals (child, null))
        {
            builder.Append (child);
        }

        builder.Append (')');

        return builder.ReturnShared();
    }

    #endregion
}
