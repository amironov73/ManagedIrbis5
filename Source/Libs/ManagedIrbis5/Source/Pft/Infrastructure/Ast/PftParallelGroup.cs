// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* PftParallelGroup.cs -- параллельная повторяющаяся группа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.Threading.Tasks;

using AM;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Параллельная повторяющаяся группа.
/// </summary>
public sealed class PftParallelGroup
    : PftNode
{
    #region Properties

    /// <summary>
    /// Throw an exception when an empty group is detected?
    /// </summary>
    public static bool ThrowOnEmpty { get; set; }

    /// <inheritdoc cref="PftNode.ComplexExpression" />
    public override bool ComplexExpression => true;

    /// <inheritdoc cref="PftNode.ExtendedSyntax"/>
    public override bool ExtendedSyntax => true;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftParallelGroup()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftParallelGroup
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.Parallel);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftParallelGroup
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

        if (context.CurrentGroup is not null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftParallelGroup) + "::" + nameof (Execute)
                    + ": nested group detected {Where}",
                    this
                );

            throw new PftSemanticException
                (
                    "Nested group detected "
                    + this
                );
        }

        if (Children.Count == 0)
        {
            Magna.Logger.LogWarning
                (
                    nameof (PftParallelGroup) + "::" + nameof (Execute)
                    + ": empty group {Where}",
                    this
                );

            if (ThrowOnEmpty)
            {
                throw new PftSemanticException
                    (
                        "Empty group: "
                        + this
                    );
            }
        }

        try
        {
            var group = new PftGroup();

            context.CurrentGroup = group;

            OnBeforeExecution (context);

            try
            {
                var affectedFields = GetAffectedFields();
                var repeatCount = PftUtility.GetFieldCount
                                      (
                                          context,
                                          affectedFields
                                      )
                                  + 1;

                var allIterations = new PftIteration[repeatCount];
                for (var index = 0; index < repeatCount; index++)
                {
                    var iteration = new PftIteration
                        (
                            context,
                            (PftNodeCollection)Children,
                            index,
                            (iter, _) => iter.Context.Execute (iter.Nodes),
                            this,
                            true
                        );
                    allIterations[index] = iteration;
                }

                var tasks = allIterations
                    .Select (iter => iter.Task)
                    .ToArray();
                Task.WaitAll (tasks);

                foreach (var iteration in allIterations)
                {
                    if (iteration.Exception is not null)
                    {
                        Magna.Logger.LogError
                            (
                                iteration.Exception,
                                nameof (PftParallelGroup) + "::" + nameof (Execute)
                                + ": iteration {Index}",
                                iteration.Index
                            );

                        throw new IrbisException
                            (
                                "Exception in parallel group, iteration: "
                                + iteration.Index,
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
                        "PftParallelGroup::Execute"
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
        var children = (PftNodeCollection) Children;
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
                .Write ("parallel(");
            printer.IncreaseLevel();
            printer.WriteLine();
            printer.WriteIndent();
        }
        else
        {
            printer
                .WriteIndentIfNeeded()
                .Write ("parallel( ");
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
        builder.Append ("parallel(");
        PftUtility.NodesToText (builder, Children);
        builder.Append (')');

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
