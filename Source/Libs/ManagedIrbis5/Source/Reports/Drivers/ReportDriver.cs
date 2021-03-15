﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ReportDriver.cs -- abstract report driver
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
using AM.Logging;
using AM.Runtime;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Abstract report driver.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public abstract class ReportDriver
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Driver capabilities.
        /// </summary>
        public virtual DriverCapability Capabilities
        {
            get { return DriverCapability.None; }
        }

        /// <summary>
        /// Prologue.
        /// </summary>
        [CanBeNull]
        public string Prologue { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ReportDriver()
        {
            Log.Trace("ReportDriver::Constructor");
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Begin cell.
        /// </summary>
        public virtual void BeginCell
            (
                [NotNull] ReportContext context,
                [NotNull] ReportCell cell
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Begin document.
        /// </summary>
        public virtual void BeginDocument
            (
                [NotNull] ReportContext context,
                [NotNull] IrbisReport report
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Begin row.
        /// </summary>
        public virtual void BeginParagraph
            (
                [NotNull] ReportContext context,
                [NotNull] ReportBand band
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Begin row.
        /// </summary>
        public virtual void BeginRow
            (
                [NotNull] ReportContext context,
                [NotNull] ReportBand band
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Begin section.
        /// </summary>
        public virtual void BeginSection
            (
                [NotNull] ReportContext context,
                [NotNull] ReportBand band
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Begin table.
        /// </summary>
        public virtual void BeginTable
            (
                [NotNull] ReportContext context,
                [NotNull] IrbisReport report
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// End cell.
        /// </summary>
        public virtual void EndCell
            (
                [NotNull] ReportContext context,
                [NotNull] ReportCell cell
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// End document.
        /// </summary>
        public virtual void EndDocument
            (
                [NotNull] ReportContext context,
                [NotNull] IrbisReport report
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// End row.
        /// </summary>
        public virtual void EndParagraph
            (
                [NotNull] ReportContext context,
                [NotNull] ReportBand band
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// End row.
        /// </summary>
        public virtual void EndRow
            (
                [NotNull] ReportContext context,
                [NotNull] ReportBand band
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// End section.
        /// </summary>
        public virtual void EndSection
            (
                [NotNull] ReportContext context,
                [NotNull] ReportBand band
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// End table.
        /// </summary>
        public virtual void EndTable
            (
                [NotNull] ReportContext context,
                [NotNull] IrbisReport report
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Start new page.
        /// </summary>
        public virtual void NewPage
            (
                [NotNull] ReportContext context,
                [NotNull] ReportBand band
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Set the document prologue.
        /// </summary>
        public virtual void SetPrologue
            (
                [CanBeNull] string prologue
            )
        {
            Prologue = prologue;
        }

        /// <summary>
        /// Write the text.
        /// </summary>
        public virtual void Write
            (
                [NotNull] ReportContext context,
                [CanBeNull] string text
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Write service (specific) text.
        /// </summary>
        public virtual void WriteServiceText
            (
                [NotNull] ReportContext context,
                [CanBeNull] string text
            )
        {
            // Nothing to do here
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public virtual void Dispose()
        {
            Log.Trace("ReportDriver::Dispose");
        }

        #endregion

        #region Object members

        #endregion
    }
}
