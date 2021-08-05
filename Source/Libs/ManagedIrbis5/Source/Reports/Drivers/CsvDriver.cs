// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* CsvDriver.cs -- драйвер для вывода отчета в формате CSV
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Драйвер для CSV-формата.
    /// </summary>
    public sealed class CsvDriver
        : ReportDriver
    {
        #region Properties

        /// <summary>
        /// Разделитель полей. По умолчанию ';'.
        /// </summary>
        public string? Separator = ";";

        /// <summary>
        /// Символ кавычек. По умолчанию '"'.
        /// </summary>
        public string? Quotes = "\"";

        #endregion

        #region ReportDriver members

        /// <inheritdoc cref="ReportDriver.BeginCell" />
        public override void BeginCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            if (!string.IsNullOrEmpty(Quotes))
            {
                context.Output.Write(Quotes);
            }
        }

        /// <inheritdoc cref="ReportDriver.EndCell" />
        public override void EndCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            ReportOutput output = context.Output;

            if (!string.IsNullOrEmpty(Quotes))
            {
                output.Write(Quotes);
            }

            if (!string.IsNullOrEmpty(Separator))
            {
                output.Write(Separator);
            }
        }

        /// <inheritdoc cref="ReportDriver.EndRow" />
        public override void EndRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            context.Output.Write(Environment.NewLine);
        }

        /// <inheritdoc cref="ReportDriver.Write" />
        public override void Write
            (
                ReportContext context,
                string? text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                context.Output.Write(text);
            }
        } // method Write

        #endregion

    } // class ReportDriver

} // namespace ManagedIrbis.Reports
