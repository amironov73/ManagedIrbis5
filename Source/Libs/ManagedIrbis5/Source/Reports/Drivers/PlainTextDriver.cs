﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PlainTextDriver.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
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

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class PlainTextDriver
        : ReportDriver
    {
        #region Properties

        /// <summary>
        /// Cell delimiter.
        /// </summary>
        [CanBeNull]
        public string CellDelimiter { get; set; }

        /// <summary>
        /// Row delimiter.
        /// </summary>
        [CanBeNull]
        public string RowDelimiter { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlainTextDriver()
        {
            CellDelimiter = "\t";
            RowDelimiter = Environment.NewLine;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region ReportDriver members

        /// <inheritdoc cref="ReportDriver.BeginParagraph" />
        public override void BeginParagraph
            (
                ReportContext context,
                ReportBand band
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(band, "band");

            ReportOutput output = context.Output;
            output.Write(Environment.NewLine);
        }

        /// <inheritdoc cref="ReportDriver.EndRow" />
        public override void EndRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(band, "band");

            ReportOutput output = context.Output;
            output.TrimEnd();
            output.Write(RowDelimiter);
        }

        /// <inheritdoc cref="ReportDriver.EndCell" />
        public override void EndCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(cell, "cell");

            context.Output.Write(CellDelimiter);
        }

        /// <inheritdoc cref="ReportDriver.Write" />
        public override void Write
            (
                ReportContext context,
                string text
            )
        {
            Code.NotNull(context, "context");

            context.Output.Write(text);
        }

        #endregion

        #region Object members

        #endregion
    }
}
