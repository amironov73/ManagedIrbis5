﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftGroup.cs -- группа
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Diagnostics;
using System.Text;

using AM.Logging;
using AM.Text;

using CodeJam;

using JetBrains.Annotations;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Группа.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PftGroup
        : PftNode
    {
        #region Properties

        /// <summary>
        /// Throw an exception when empty group detected?
        /// </summary>
        public static bool ThrowOnEmpty { get; set; }

        /// <inheritdoc cref="PftNode.ComplexExpression"/>
        public override bool ComplexExpression
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static PftGroup()
        {
            ThrowOnEmpty = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftGroup()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftGroup
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            Code.NotNull(token, "token");
            token.MustBe(PftTokenKind.LeftParenthesis);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftGroup
            (
                params PftNode[] children
            )
            : base(children)
        {
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            compiler.CompileNodes(Children);

            string actionName = compiler.CompileAction(Children);

            compiler.StartMethod(this);

            if (!string.IsNullOrEmpty(actionName))
            {
                compiler
                    .WriteIndent()
                    .WriteLine("DoGroup({0});", actionName);
            }

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                Log.Error
                    (
                        "PftGroup::Execute: "
                        + "nested group detected: "
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
                Log.Error
                    (
                        "PftGroup::Execute: "
                        + "empty group detected: "
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
                context.CurrentGroup = this;

                OnBeforeExecution(context);

                try
                {
                    context.DoRepeatableAction
                        (
                            ctx =>
                            {
                                foreach (PftNode child in Children)
                                {
                                    child.Execute(ctx);
                                }
                            }
                        );
                }
                catch (PftBreakException exception)
                {
                    // It was break operator

                    Log.TraceException
                        (
                            "PftGroup::Execute",
                            exception
                        );
                }

                OnAfterExecution(context);
            }
            finally
            {
                context.CurrentGroup = null;
            }
        }

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode Optimize()
        {
            PftNodeCollection children = (PftNodeCollection) Children;
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
            bool isComplex = PftUtility.IsComplexExpression(Children);
            if (isComplex)
            {
                printer.EatWhitespace();
                printer.EatNewLine();
                printer.WriteLine();
                printer
                    .WriteIndent()
                    .Write('(');
                printer.IncreaseLevel();
                printer.WriteLine();
                printer.WriteIndent();
            }
            else
            {
                printer
                    .WriteIndentIfNeeded()
                    .Write("( ");
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
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        [DebuggerStepThrough]
        protected internal override bool ShouldSerializeText()
        {
            return false;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = StringBuilderCache.Acquire();
            result.Append('(');
            PftUtility.NodesToText(result, Children);
            result.Append(')');

            return StringBuilderCache.GetStringAndRelease(result);
        }

        #endregion
    }
}
