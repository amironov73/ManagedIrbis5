// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* PlainTextDriver.cs -- драйвер для вывода отчета в плоский текст
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Драйвер для вывода отчета в плоский текст.
    /// </summary>
    public sealed class PlainTextDriver
        : ReportDriver
    {
        #region Properties

        /// <summary>
        /// Cell delimiter.
        /// </summary>
        public string? CellDelimiter { get; set; }

        /// <summary>
        /// Row delimiter.
        /// </summary>
        public string? RowDelimiter { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlainTextDriver()
        {
            CellDelimiter = "\t";
            RowDelimiter = Environment.NewLine;
        } // constructor

        #endregion

        #region ReportDriver members

        /// <inheritdoc cref="ReportDriver.BeginParagraph" />
        public override void BeginParagraph
            (
                ReportContext context,
                ReportBand band
            )
        {
            var output = context.Output;
            output.Write(Environment.NewLine);
        } // method BeginParagraph

        /// <inheritdoc cref="ReportDriver.EndRow" />
        public override void EndRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            var output = context.Output;
            output.TrimEnd();
            output.Write(RowDelimiter);
        } // method RowDelimiter

        /// <inheritdoc cref="ReportDriver.EndCell" />
        public override void EndCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            context.Output.Write(CellDelimiter);
        } // method EndCell

        /// <inheritdoc cref="ReportDriver.Write" />
        public override void Write
            (
                ReportContext context,
                string text
            )
        {
            context.Output.Write(text);
        } // method Write

        #endregion

    } // class PlainTextDriver

} // namespace ManagedIrbis.Reports
