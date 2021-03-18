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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    ///
    /// </summary>
    public sealed class CsvDriver
        : ReportDriver
    {
        #region Properties

        /// <summary>
        /// Field separator.
        /// </summary>
        public string? Separator = ";";

        /// <summary>
        /// Quotes.
        /// </summary>
        public string? Quotes = "\"";

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
            if (!string.IsNullOrEmpty(Quotes))
            {
                context.Output.Write(Quotes);
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override void EndRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            context.Output.Write(Environment.NewLine);
        }

        /// <inheritdoc />
        public override void Write
            (
                ReportContext context,
                string text
            )
        {
            context.Output.Write(text);
        }

        #endregion

    }
}
