// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftNested.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Временное переключение контекста
    /// на альтернативный (проще говоря,
    /// на другую запись).
    /// </summary>
    /// <example>
    /// <code>
    /// v200^a, " : "v200^e, " / "v200^f
    /// #
    /// {
    ///    /* Выводятся значения полей от другой записи
    ///    v200^a, " : "v200^e, " / "v200^f
    /// }
    /// </code>
    /// </example>
    public sealed class PftNested
        : PftNode
    {
        #region Properties

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax
        {
            get { return true; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNested()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNested
            (
                [NotNull] PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.LeftCurly);
        }

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            var mainRecord = context.Record;
            var alternativeRecord = context.AlternativeRecord;

            try
            {
                context.Record = alternativeRecord;
                context.AlternativeRecord = mainRecord;
                context.Execute(Children);
            }
            finally
            {
                context.Record = mainRecord;
                context.AlternativeRecord = alternativeRecord;
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.Write("{");
            base.PrettyPrint(printer);
            printer.Write("} ");
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        [DebuggerStepThrough]
        [ExcludeFromCodeCoverage]
        protected internal override bool ShouldSerializeText()
        {
            return false;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="PftNode.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append('{');
            PftUtility.NodesToText(result, Children);
            result.Append('}');

            return result.ToString();
        }

        #endregion
    }
}
