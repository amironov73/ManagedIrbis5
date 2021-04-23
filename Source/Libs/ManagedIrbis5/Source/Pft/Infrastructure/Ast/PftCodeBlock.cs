// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftCodeBlock.cs -- выполнение скриптов на C#
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    //
    // Предоставляет возможность вставить в PFT-скрипт
    // фрагмент кода на C#. Выглядит это так:
    //
    // v100, v200,
    // {{{context.Write(node, "Hello, world!");}}}
    // v300, v400
    //
    // Внутри C#-фрагмента доступны следующие
    // глобальные переменные
    //
    // * context - контекст форматирования
    // * node - текущая нода с C#-кодом
    // * record - текущая MARC-запись
    //
    // В C# заранее импортированы пространства имен
    // System и ManagedIrbis
    //

    /// <summary>
    /// Выполнение скриптов на C#.
    /// </summary>
    public sealed class PftCodeBlock
        : PftNode
    {
        #region Nested classes

        /// <summary>
        /// Это специальный класс, для глобальных переменных,
        /// доступных из скрипта.
        /// </summary>
        // ReSharper disable MemberCanBePrivate.Global
        public class Globals
        {
            // ReSharper disable InconsistentNaming
            // ReSharper disable NotAccessedField.Global

            /// <summary>
            /// Указатель на текущую ноду,
            /// в которой сосредоточен C#-код.
            /// </summary>
            public PftNode node;

            /// <summary>
            /// Контекст форматирования.
            /// </summary>
            public PftContext context;

            /// <summary>
            /// Текущая MARC-запись.
            /// </summary>
            public Record record;

            // ReSharper restore NotAccessedField.Global
            // ReSharper restore InconsistentNaming
        }
        // ReSharper restore MemberCanBePrivate.Global

        #endregion

        #region Properties

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax => true;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCodeBlock()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCodeBlock
            (
                string text
            )
        {
            Text = text;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCodeBlock
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.TripleCurly);

            if (string.IsNullOrEmpty(token.Text))
            {
                Magna.Error
                    (
                        "PftCodeBlock::Constructor: "
                        + "token text not set"
                    );

                throw new PftSyntaxException(token);
            }

            Text = token.Text;
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

            Magna.Trace("PftCodeBlock::Execute: compile method");

            var text = Text;
            if (!string.IsNullOrEmpty(text))
            {
                var globals = new Globals
                {
                    node = this,
                    context = context,
                    record = context.Record
                };

                var scriptOptions = ScriptOptions.Default
                    .AddImports("System")
                    .AddReferences(typeof(PftCodeBlock).Assembly)
                    .AddImports("ManagedIrbis");

                CSharpScript.RunAsync
                    (
                        Text,
                        scriptOptions,
                        globals
                    );

            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.EatWhitespace();
            printer.EatNewLine();
            printer
                .WriteLine()
                .WriteIndentIfNeeded()
                .Write("{{{")
                .Write(Text)
                .WriteLine("}}}");
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString()" />
        public override string ToString() => "{{{" + Text + "}}}";

        #endregion

    } // class PftCodeBlock

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
