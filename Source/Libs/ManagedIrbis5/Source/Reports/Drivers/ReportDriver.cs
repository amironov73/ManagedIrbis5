// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* ReportDriver.cs -- abstract report driver
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

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Abstract report driver.
    /// </summary>
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
        public string? Prologue { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ReportDriver()
        {
            Magna.Trace
                (
                    nameof(ReportDriver) + "::Constructor"
                );
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
                ReportContext context,
                ReportCell cell
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Begin document.
        /// </summary>
        public virtual void BeginDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Begin row.
        /// </summary>
        public virtual void BeginParagraph
            (
                ReportContext context,
                ReportBand band
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Begin row.
        /// </summary>
        public virtual void BeginRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Begin section.
        /// </summary>
        public virtual void BeginSection
            (
                ReportContext context,
                ReportBand band
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Begin table.
        /// </summary>
        public virtual void BeginTable
            (
                ReportContext context,
                IrbisReport report
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// End cell.
        /// </summary>
        public virtual void EndCell
            (
                ReportContext context,
                ReportCell cell
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// End document.
        /// </summary>
        public virtual void EndDocument
            (
                ReportContext context,
                IrbisReport report
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// End row.
        /// </summary>
        public virtual void EndParagraph
            (
                ReportContext context,
                ReportBand band
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// End row.
        /// </summary>
        public virtual void EndRow
            (
                ReportContext context,
                ReportBand band
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// End section.
        /// </summary>
        public virtual void EndSection
            (
                ReportContext context,
                ReportBand band
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// End table.
        /// </summary>
        public virtual void EndTable
            (
                ReportContext context,
                IrbisReport report
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Start new page.
        /// </summary>
        public virtual void NewPage
            (
                ReportContext context,
                ReportBand band
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Set the document prologue.
        /// </summary>
        public virtual void SetPrologue
            (
                string? prologue
            )
        {
            Prologue = prologue;
        }

        /// <summary>
        /// Write the text.
        /// </summary>
        public virtual void Write
            (
                ReportContext context,
                string? text
            )
        {
            // Nothing to do here
        }

        /// <summary>
        /// Write service (specific) text.
        /// </summary>
        public virtual void WriteServiceText
            (
                ReportContext context,
                string? text
            )
        {
            // Nothing to do here
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public virtual void Dispose()
        {
            Magna.Trace
                (
                    nameof(ReportDriver) + "::" + nameof(Dispose)
                );
        }

        #endregion

        #region Object members

        #endregion
    }
}
