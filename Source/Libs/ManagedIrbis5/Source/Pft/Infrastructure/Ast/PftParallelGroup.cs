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
using System.Text;
using System.Threading.Tasks;

using AM;

using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
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
        /// Constructor.
        /// </summary>
        public PftParallelGroup()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftParallelGroup
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.Parallel);
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftParallelGroup
            (
                params PftNode[] children
            )
            : base(children)
        {
        } // constructor

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            if (context.CurrentGroup is not null)
            {
                Magna.Error
                    (
                        nameof(PftParallelGroup) + "::" + nameof(Execute)
                        + ": nested group detected: "
                        + this
                    );

                throw new PftSemanticException
                    (
                        "Nested group: "
                        + this
                    );
            }

            if (Children.Count == 0)
            {
                Magna.Error
                    (
                        nameof(PftParallelGroup) + "::" + nameof(Execute)
                        + ": empty group: "
                        + this
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

                OnBeforeExecution(context);

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
                                (iter,_) => iter.Context.Execute(iter.Nodes),
                                this,
                                true
                            );
                        allIterations[index] = iteration;
                    }

                    var tasks = allIterations
                        .Select(iter => iter.Task)
                        .ToArray();
                    Task.WaitAll(tasks);

                    foreach (var iteration in allIterations)
                    {
                        if (!ReferenceEquals(iteration.Exception, null))
                        {
                            Magna.TraceException
                                (
                                    nameof(PftParallelGroup) + "::" + nameof(Execute),
                                    iteration.Exception
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

                    Magna.TraceException
                        (
                            "PftParallelGroup::Execute",
                            exception
                        );
                }

                OnAfterExecution(context);
            }
            finally
            {
                context.CurrentGroup = null;
            }
        } // method Execute

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
        } // method Optimize

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            var isComplex = PftUtility.IsComplexExpression(Children);
            if (isComplex)
            {
                printer.EatWhitespace();
                printer.EatNewLine();
                printer.WriteLine();
                printer
                    .WriteIndent()
                    .Write("parallel(");
                printer.IncreaseLevel();
                printer.WriteLine();
                printer.WriteIndent();
            }
            else
            {
                printer
                    .WriteIndentIfNeeded()
                    .Write("parallel( ");
            }
            base.PrettyPrint(printer);
            if (isComplex)
            {
                printer.EatWhitespace();
                printer.EatNewLine();
                printer.WriteLine()
                    .DecreaseLevel()
                    .WriteIndent()
                    .Write(')')
                    .WriteLine();
            }
            else
            {
                printer
                    .WriteIndentIfNeeded()
                    .Write(')');
            }
        } // method PrettyPrint

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("parallel(");
            PftUtility.NodesToText(result, Children);
            result.Append(')');

            return result.ToString();
        } // method ToString

        #endregion

    } // class PftParallelGroup

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
