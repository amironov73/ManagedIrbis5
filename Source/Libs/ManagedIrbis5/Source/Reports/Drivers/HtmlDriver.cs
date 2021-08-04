// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* HtmlDriver.cs -- драйвер для вывода отчета в формате HTML
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Драйвер для вывода отчета в формате HTML.
    /// </summary>
    public sealed class HtmlDriver
        : ReportDriver
    {
        #region Properties

        /// <summary>
        /// Table borders visible?
        /// </summary>
        public bool Borders { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportDriver members

        /// <inheritdoc />
        public override void BeginCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            context.Output.Write("<td>");
        }

        /// <inheritdoc />
        public override void BeginDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            var table = $"<table border={(Borders ? "1" : "0")}>";

            context.Output.Write(table);
        }

        /// <inheritdoc />
        public override void BeginRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            context.Output.Write("<tr>");
        }

        /// <inheritdoc />
        public override void EndCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            context.Output.Write("</td>");
        }

        /// <inheritdoc />
        public override void EndDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            var output = context.Output;
            output.Write("</table>");
            output.Write(Environment.NewLine);
        }

        /// <inheritdoc />
        public override void EndRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            var output = context.Output;
            output.Write("</tr>");
            output.Write(Environment.NewLine);
        }

        /// <inheritdoc />
        public override void Write
            (
                ReportContext context,
                string? text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                var encoded = HtmlText.Encode(text);
                if (!string.IsNullOrEmpty(encoded))
                {
                    context.Output.Write(encoded);
                }
            }
        }

        #endregion

    } // class HtmlDriver

} // namespace ManagedIrbis.Reports
