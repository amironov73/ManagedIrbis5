// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* PftMfn.cs -- вывод номера записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;

using AM;
using AM.IO;

using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Для вывода номера записи в файле документов служит
    /// команда MFN, формат которой:
    ///
    /// MFN или MFN(d),
    ///
    /// где d - количество выводимых на экран цифр.
    /// Если параметр(d) опущен, то по умолчанию
    /// предполагается 6 цифр.
    /// </summary>
    public sealed class PftMfn
        : PftNumeric
    {
        #region Constants

        /// <summary>
        /// Default width.
        /// </summary>
        public const int DefaultWidth = 10;

        #endregion

        #region Properties

        /// <summary>
        /// Width of the output.
        /// </summary>
        public int Width { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMfn()
        {
            Width = DefaultWidth;
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMfn
            (
                int width
            )
        {
            Sure.Positive(width, nameof(width));

            Width = width;
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftMfn
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.Mfn);

            Width = DefaultWidth;

            try
            {
                var text = token.Text;
                if (!string.IsNullOrEmpty(text))
                {
                    if (text.Length > 3)
                    {
                        text = text.Substring(3);
                        text = text.TrimStart('(').TrimEnd(')');

                        Width = int.Parse(text);
                        if (Width <= 0)
                        {
                            Magna.Error
                                (
                                    "PftMfn::Constructor: "
                                    + "Width="
                                    + Width
                                );

                            throw new PftSyntaxException(token);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        "PftMfn::Constructor",
                        exception
                    );

                throw new PftSyntaxException(token, exception);
            }

        } // constructor

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Width = reader.ReadPackedInt32();
        } // method Deserialize

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            Value = 0.0;

            if (context.Record is { } record)
            {
                Value = record.Mfn;

                var text = Width == 0
                    ? record.Mfn.ToInvariantString()
                    : record.Mfn.ToString
                        (
                            new string('0', Width),
                            CultureInfo.InvariantCulture
                        );

                context.Write
                    (
                        this,
                        text
                    );
            }

            OnAfterExecution(context);
        } // method Execute

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.WritePackedInt32(Width);
        } // method Serialize

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer.Write("mfn");
            if (Width > 0
                && Width != DefaultWidth)
            {
                printer.Write('(')
                    .Write(Width)
                    .Write(')');
            }
        } // method PrettyPrint

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

    } // class PftMfn

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
