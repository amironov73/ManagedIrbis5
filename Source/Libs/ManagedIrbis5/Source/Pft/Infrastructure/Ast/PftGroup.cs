// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* PftGroup.cs -- повторяющаяся группа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Повторяющаяся группа.
/// </summary>
public sealed class PftGroup
    : PftNode
{
    #region Properties

    /// <summary>
    /// Throw an exception when empty group detected?
    /// </summary>
    public static bool ThrowOnEmpty { get; set; }

    /// <inheritdoc cref="PftNode.ComplexExpression"/>
    public override bool ComplexExpression => true;

    #endregion

    #region Construction

    /// <summary>
    /// Статический конструктор.
    /// </summary>
    static PftGroup()
    {
        ThrowOnEmpty = false;
    }

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftGroup()
    {
        // пустое тело конструктора.
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftGroup
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.LeftParenthesis);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftGroup
        (
            params PftNode[] children
        )
        : base (children)
    {
        // пустое тело конструктора
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

        compiler.CompileNodes (Children);

        var actionName = compiler.CompileAction (Children);

        compiler.StartMethod (this);

        if (!string.IsNullOrEmpty (actionName))
        {
            compiler
                .WriteIndent()
                .WriteLine ("DoGroup({0});", actionName);
        }

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

        if (context.CurrentGroup is not null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftGroup) + "::" + nameof (Execute)
                    + ": nested group detected at {Token}",
                    this
                );

            throw new PftSemanticException
                (
                    "Nested group detected: "
                    + this
                );
        }

        if (Children.Count == 0)
        {
            Magna.Logger.LogTrace
                (
                    nameof (PftGroup) + "::" + nameof (Execute)
                    + ": empty group detected at {Token}",
                    this
                );

            if (ThrowOnEmpty)
            {
                throw new PftSemanticException
                    (
                        "Empty group detected: "
                        + this
                    );
            }
        }

        try
        {
            context.CurrentGroup = this;

            OnBeforeExecution (context);

            try
            {
                context.DoRepeatableAction
                    (
                        ctx =>
                        {
                            foreach (var child in Children)
                            {
                                child.Execute (ctx);
                            }
                        }
                    );
            }
            catch (PftBreakException exception)
            {
                // It was break operator

                Magna.Logger.LogWarning
                    (
                        exception,
                        nameof (PftGroup) + "::" + nameof (Execute)
                    );
            }

            OnAfterExecution (context);
        }
        finally
        {
            context.CurrentGroup = null;
        }
    }

    /// <inheritdoc cref="PftNode.Optimize" />
    public override PftNode? Optimize()
    {
        var children = (PftNodeCollection)Children;
        children.Optimize();

        if (children.Count == 0)
        {
            // Take the node away from the AST

            return null;
        }

        return this;
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        var isComplex = PftUtility.IsComplexExpression (Children);
        if (isComplex)
        {
            printer.EatWhitespace();
            printer.EatNewLine();
            printer.WriteLine();
            printer
                .WriteIndent()
                .Write ('(');
            printer.IncreaseLevel();
            printer.WriteLine();
            printer.WriteIndent();
        }
        else
        {
            printer
                .WriteIndentIfNeeded()
                .Write ("( ");
        }

        base.PrettyPrint (printer);
        if (isComplex)
        {
            printer.EatWhitespace();
            printer.EatNewLine();
            printer.WriteLine()
                .DecreaseLevel()
                .WriteIndent()
                .Write (')')
                .WriteLine();
        }
        else
        {
            printer
                .WriteIndentIfNeeded()
                .Write (')');
        }
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ('(');
        PftUtility.NodesToText (builder, Children);
        builder.Append (')');

        return builder.ReturnShared();
    }

    #endregion
}
