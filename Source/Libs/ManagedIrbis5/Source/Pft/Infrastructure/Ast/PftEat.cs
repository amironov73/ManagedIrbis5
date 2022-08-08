// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PftEat.cs -- съедает результат выполнения операторов
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
/// Съедает результат выполнения операторов.
/// </summary>
/// <example>
/// <code>
/// 'Hello,', [[['again,']]] 'World'
/// </code>
/// </example>
public sealed class PftEat
    : PftNode
{
    #region Properties

    /// <inheritdoc cref="PftNode.ExtendedSyntax" />
    public override bool ExtendedSyntax => true;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftEat()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftEat
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.EatOpen);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftEat
        (
            params PftNode[] children
        )
        : base (children)
    {
        // пустое тело конструктора
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

        // съедаем результат вычисления
        var _ = context.Evaluate (Children);

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        printer.Write ("[[[");
        base.PrettyPrint (printer);
        printer.Write ("]]] ");
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("[[[");
        PftUtility.NodesToText (builder, Children);
        builder.Append ("]]]");

        return builder.ReturnShared();
    }

    #endregion
}
