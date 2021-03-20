// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftS.cs -- конкатенация строк
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Linq;
using System.Text;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Функция S возвращает текст, полученный
    /// в результате вычисления ее аргумента.
    /// </summary>
    public sealed class PftS
        : PftNode
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftS()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftS
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.S);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftS
            (
                params PftNode[] children
            )
        {
            foreach (PftNode child in children)
            {
                Children.Add(child);
            }
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

            compiler.StartMethod(this);

            compiler.CallNodes(Children);

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            context.Execute(Children);

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.Optimize" />
        public override PftNode? Optimize()
        {
            PftNodeCollection children = (PftNodeCollection)Children;
            children.Optimize();

            if (children.Count == 0)
            {
                // Take the node away from the AST

                return null;
            }

            if (children.Count == 1)
            {
                return children.First();
            }

            return this;
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.EatWhitespace();
            printer
                .SingleSpace()
                .Write("s(");
            base.PrettyPrint(printer);
            printer.EatWhitespace();
            printer.Write(')');
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
            var result = new StringBuilder();
            result.Append("s(");
            PftUtility.NodesToText(result, Children);
            result.Append(')');

            return result.ToString();
        }

        #endregion
    }
}
