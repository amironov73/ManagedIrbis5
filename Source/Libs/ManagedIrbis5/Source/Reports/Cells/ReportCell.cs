// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ReportCell.cs -- базовый тип для ячеек отчета
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Базовый тип для ячеек отчета.
    /// </summary>
    public abstract class ReportCell
        : IAttributable,
        IVerifiable,
        IDisposable
    {
        #region Events

        /// <summary>
        /// Raised after <see cref="Compute"/>.
        /// </summary>
        public event EventHandler<ReportEventArgs> AfterCompute;

        /// <summary>
        /// Raised before <see cref="Compute"/>.
        /// </summary>
        public event EventHandler<ReportEventArgs> BeforeCompute;

        #endregion

        #region Properties

        /// <summary>
        /// Attributes.
        /// </summary>
        [XmlArray("attr")]
        [JsonPropertyName("attr")]
        public ReportAttributes Attributes { get; }

        /// <summary>
        /// Band.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public ReportBand? Band { get; internal set; }

        /// <summary>
        /// Report.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public IrbisReport? Report { get; internal set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ReportCell()
        {
            Magna.Trace("ReportCell::Constructor");

            Attributes = new ReportAttributes();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ReportCell
            (
                params ReportAttribute[] attributes
            )
            : this()
        {
            foreach (ReportAttribute attribute in attributes)
            {
                Attributes.Add(attribute.Name, attribute.Value);
            }
        }

        #endregion

        #region Private members

        /// <summary>
        /// Called after <see cref="Compute"/>.
        /// </summary>
        protected void OnAfterCompute
            (
                ReportContext context
            )
        {
            ReportEventArgs eventArgs
                = new ReportEventArgs(context);
            AfterCompute.Raise(eventArgs);
        }

        /// <summary>
        /// Called before <see cref="Compute"/>.
        /// </summary>
        protected void OnBeforeCompute
            (
                ReportContext context
            )
        {
            ReportEventArgs eventArgs
                = new ReportEventArgs(context);
            BeforeCompute.Raise(eventArgs);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the cell.
        /// </summary>
        public virtual ReportCell Clone()
        {
            return (ReportCell) MemberwiseClone();
        }

        /// <summary>
        /// Compute value of the cell.
        /// </summary>
        public virtual string? Compute
            (
                ReportContext context
            )
        {
            OnBeforeCompute(context);

            // Nothing to do here

            OnAfterCompute(context);

            return null;
        }

        /// <summary>
        /// Render the cell.
        /// </summary>
        public virtual void Render
            (
                ReportContext context
            )
        {
            // Nothing to do here
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ReportCell> verifier
                = new Verifier<ReportCell>(this, throwOnError);

            verifier
                .VerifySubObject(Attributes, "attributes");

            // TODO Add some verification

            return verifier.Result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public virtual void Dispose()
        {
            // Nothing to do here
            Magna.Trace("ReportCell::Dispose");
        }

        #endregion

    } // class ReportCell

} // namespace ManagedIrbis.Reports
