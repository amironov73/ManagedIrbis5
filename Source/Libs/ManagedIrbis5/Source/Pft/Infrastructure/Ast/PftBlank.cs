// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftBlank.cs -- определяет, пуста ли указанная строка
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Определяет, пуста ли указанная строка.
    /// Пустой считается строка: 1) null, 2) "",
    /// 3) состоящая только из пробельных символов.
    /// </summary>
    public sealed class PftBlank
        : PftCondition
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax => true;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftBlank()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftBlank
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.Blank);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftBlank
            (
                params PftNode[] children
            )
            : base(children)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Пуста ли указанная строка?
        /// Пустой считается строка: 1) null, 2) "",
        /// 3) состоящая только из пробельных символов.
        /// </summary>
        public static bool IsBlank ( string? text ) => string.IsNullOrWhiteSpace(text);

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            compiler.CompileNodes(Children);

            var actionName = compiler.CompileAction(Children);

            compiler.StartMethod(this);

            if (string.IsNullOrEmpty(actionName))
            {
                compiler
                    .WriteIndent()
                    .WriteLine("bool result = true;"); //-V3010
            }
            else
            {
                compiler
                    .WriteIndent()
                    .WriteLine("string text = Evaluate({0});", actionName); //-V3010

                compiler
                    .WriteIndent()
                    .WriteLine("bool result = PftBlank.IsBlank(text);"); //-V3010
            }

            compiler
                .WriteIndent()
                .WriteLine("return result;");

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

            var text = context.Evaluate(Children);

            Value = IsBlank(text);

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .SingleSpace()
                .Write("blank(");
            base.PrettyPrint(printer);
            printer.Write(')');
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        [DebuggerStepThrough]
        [ExcludeFromCodeCoverage]
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("blank(");
            var first = true;
            foreach (var child in Children)
            {
                if (!first)
                {
                    result.Append(' ');
                }
                result.Append(child);
                first = false;
            }
            result.Append(')');

            return result.ToString();
        }

        #endregion

    } // class PftBlank

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
