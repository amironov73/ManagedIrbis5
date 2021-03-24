// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* ReportContext.cs -- контекст, в котором выполняется построение отчета
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;
using AM.Collections;

using ManagedIrbis.Client;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Контекст, в котором выполняется построение отчета.
    /// </summary>
    public class ReportContext
        : IDisposable,
        IVerifiable
    {
        #region Events

        /// <summary>
        /// Raised on rendering bands.
        /// </summary>
        public event EventHandler<ReportEventArgs>? Rendering;

        #endregion

        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        public IrbisProvider Provider { get; set; }

        /// <summary>
        /// Current record.
        /// </summary>
        public Record? CurrentRecord { get; internal set; }

        /// <summary>
        /// Text driver.
        /// </summary>
        public ReportDriver Driver { get; internal set; }

        /// <summary>
        /// Record index.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Records.
        /// </summary>
        public NonNullCollection<Record> Records { get; private set; }

        /// <summary>
        /// Output.
        /// </summary>
        public ReportOutput Output { get; private set; }

        /// <summary>
        /// Variables.
        /// </summary>
        public ReportVariableManager Variables { get; private set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportContext
            (
                IrbisProvider provider
            )
        {
            Variables = new ReportVariableManager();
            Records = new NonNullCollection<Record>();
            Output = new ReportOutput();
            Driver = new PlainTextDriver();
            Provider = provider;
        } // constructor

        #endregion

        #region Private members

        /// <summary>
        /// Raise <see cref="Rendering"/> event.
        /// </summary>
        internal void OnRendering()
        {
            var rendering = Rendering;
            if (rendering is not null)
            {
                var eventArgs = new ReportEventArgs(this);
                rendering(this, eventArgs);
            }
        } // method OnRendering

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the context.
        /// </summary>
        public ReportContext Clone()
        {
            ReportContext result = (ReportContext)MemberwiseClone();

            return result;
        } // method Clone

        /// <summary>
        /// Clone the context with new record list.
        /// </summary>
        public ReportContext Clone
            (
                IEnumerable<Record> records
            )
        {
            var result = (ReportContext) MemberwiseClone();
            result.Records = new NonNullCollection<Record>();
            result.Records.AddRange(records);

            return result;
        } // method Clone

        /// <summary>
        /// Push the context.
        /// </summary>
        public ReportContext Push()
        {
            var result = (ReportContext) MemberwiseClone();
            result.Output = new ReportOutput();

            return result;
        } // method Push

        /// <summary>
        /// Set text driver for the context.
        /// </summary>
        public ReportContext SetDriver
            (
                ReportDriver driver
            )
        {
            Driver = driver;

            return this;
        } // method SetDriver

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            // TODO Do something here?
        } // method Dispose

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ReportContext>(this, throwOnError);

            verifier.VerifySubObject(Variables, "variables");

            // TODO Add some verification here

            return verifier.Result;
        } // method Verify

        #endregion

    } // class ReportContext

} // namespace ManagedIrbis.Reports
