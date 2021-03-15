﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* RtfDriver.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using AM.Text;

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
    public sealed class RtfDriver
        : ReportDriver
    {
        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region ReportDriver members

        /// <inheritdoc cref="ReportDriver.BeginDocument"/>
        public override void BeginDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(report, "report");

            ReportOutput output = context.Output;

            string prologue = Prologue;
            if (string.IsNullOrEmpty(prologue))
            {
                prologue = RichText.CommonPrologue;
            }

            output.Write(prologue);
        }

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
            output.Write(@"\par\pard\plain ");
        }

        /// <inheritdoc cref="ReportDriver.EndDocument" />
        public override void EndDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(report, "report");

            ReportOutput output = context.Output;
            output.Write(@"}");
        }

        /// <inheritdoc cref="ReportDriver.BeginCell"/>
        public override void BeginCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(cell, "cell");

            ReportOutput output = context.Output;
            output.Write("\\cell ");
        }

        /// <inheritdoc cref="ReportDriver.EndRow"/>
        public override void EndRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            Code.NotNull(context, "context");
            Code.NotNull(band, "band");

            ReportOutput output = context.Output;
            output.Write("\\row ");
        }

        /// <inheritdoc cref="ReportDriver.NewPage" />
        public override void NewPage
            (
                ReportContext context,
                ReportBand band
            )
        {
            Code.NotNull(context, "context");

            ReportOutput output = context.Output;
            output.Write("\\page ");
        }

        /// <inheritdoc cref="ReportDriver.Write"/>
        public override void Write
            (
                ReportContext context,
                string text
            )
        {
            Code.NotNull(context, "text");

            string encoded = RichText.Encode(text, null);
            ReportOutput output = context.Output;
            output.Write(encoded);
        }

        /// <inheritdoc cref="ReportDriver.WriteServiceText"/>
        public override void WriteServiceText
            (
                ReportContext context,
                string text
            )
        {
            Code.NotNull(context, "text");

            ReportOutput output = context.Output;
            output.Write(text);
        }

        #endregion

        #region Public methods

        #endregion
    }
}
