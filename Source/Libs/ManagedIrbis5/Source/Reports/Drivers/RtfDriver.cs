// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* RtfDriver.cs -- драйвер для вывода отчета в RTF
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Драйвер для вывода отчета в RTF.
    /// </summary>
    public sealed class RtfDriver
        : ReportDriver
    {
        #region ReportDriver members

        /// <inheritdoc cref="ReportDriver.BeginDocument"/>
        public override void BeginDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            var output = context.Output;

            var prologue = Prologue;
            if (string.IsNullOrEmpty(prologue))
            {
                prologue = RichText.CommonPrologue;
            }

            output.Write(prologue);
        } // method BeginDocument

        /// <inheritdoc cref="ReportDriver.BeginParagraph" />
        public override void BeginParagraph
            (
                ReportContext context,
                ReportBand band
            )
        {
            var output = context.Output;
            output.Write(@"\par\pard\plain ");
        } // method BeginParagraph

        /// <inheritdoc cref="ReportDriver.EndDocument" />
        public override void EndDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            var output = context.Output;
            output.Write(@"}");
        } // method EndDocument

        /// <inheritdoc cref="ReportDriver.BeginCell"/>
        public override void BeginCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            var output = context.Output;
            output.Write("\\cell ");
        } // method BeginCell

        /// <inheritdoc cref="ReportDriver.EndRow"/>
        public override void EndRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            var output = context.Output;
            output.Write("\\row ");
        } // method EndRow

        /// <inheritdoc cref="ReportDriver.NewPage" />
        public override void NewPage
            (
                ReportContext context,
                ReportBand band
            )
        {
            var output = context.Output;
            output.Write("\\page ");
        } // method NewPage

        /// <inheritdoc cref="ReportDriver.Write"/>
        public override void Write
            (
                ReportContext context,
                string? text
            )
        {
            var encoded = RichText.Encode(text, null);
            var output = context.Output;
            if (!string.IsNullOrEmpty(encoded))
            {
                output.Write (encoded);
            }

        } // method Write

        /// <inheritdoc cref="ReportDriver.WriteServiceText"/>
        public override void WriteServiceText
            (
                ReportContext context,
                string? text
            )
        {
            var output = context.Output;
            if (!string.IsNullOrEmpty(text))
            {
                output.Write(text);
            }

        } // method WriteServiceText

        #endregion

    } // class RtfDriver

} // namespace ManagedIrbis.Reports
