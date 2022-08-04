// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftEval.cs -- исполнение динамического PFT-формата
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Исполнение динамического PFT-формата.
/// </summary>
public sealed class PftEval
    : PftNode
{
    #region Properties

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax => true;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftEval()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftEval
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.Eval);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftEval
        (
            params PftNode[] children
        )
    {
        Sure.NotNull (children);

        foreach (var child in children)
        {
            Children.Add (child);
        }
    }

    #endregion

    #region PftNode members

    /// <inheritdoc />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        var expression = context.Evaluate (Children);
        if (!string.IsNullOrEmpty (expression))
        {
            var program = PftUtility.CompileProgram (expression);
            program.Execute (context);
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
            .WriteIndentIfNeeded()
            .Write ("eval(")
            .WriteNodes (Children)
            .Write (")");
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString()" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("eval(");
        var first = true;
        foreach (var child in Children)
        {
            if (!first)
            {
                builder.Append (' ');
            }

            builder.Append (child);
            first = false;
        }

        builder.Append (')');

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
