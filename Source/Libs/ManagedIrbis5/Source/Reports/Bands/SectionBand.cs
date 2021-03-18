// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* SectionBand.cs -- полоса, формирующая секцию отчета
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Полоса, формирующая секцию отчета.
    /// </summary>
    public class SectionBand
        : ReportBand
    {
        #region Properties

        /// <summary>
        /// Body bands.
        /// </summary>[NotNull]
        [XmlElement("body")]
        [JsonPropertyName("body")]
        public BandCollection<ReportBand> Body { get; internal set; }

        /// <summary>
        /// Footer band.
        /// </summary>
        [XmlElement("footer")]
        [JsonPropertyName("footer")]
        public ReportBand? Footer { get; set; }

        /// <summary>
        /// Header band.
        /// </summary>
        [XmlElement("header")]
        [JsonPropertyName("header")]
        public ReportBand? Header { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SectionBand()
        {
            Body = new BandCollection<ReportBand>();
        } // constructor

        #endregion

        #region ReportBand members

        /// <inheritdoc cref="ReportBand.Render"/>
        public override void Render
            (
                ReportContext context
            )
        {
            OnBeforeRendering(context);

            // TODO mark section begin

            var header = Header;
            if (header is not null)
            {
                context.Index = -1;
                context.CurrentRecord = null;
                header.Render(context);
            }

            context.Index = -1;
            context.CurrentRecord = null;
            foreach (var band in Body)
            {
                band.Render(context);
            }

            var footer = Footer;
            if (footer is not null)
            {
                context.Index = -1;
                context.CurrentRecord = null;
                footer.Render(context);
            }

            // TODO mark section end

            OnAfterRendering(context);
        } // method Render

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ReportBand>(this, throwOnError);

            verifier.Assert(base.Verify(throwOnError));

            if (!ReferenceEquals(Header, null))
            {
                verifier.VerifySubObject(Header, "header");
            }

            if (!ReferenceEquals(Footer, null))
            {
                verifier.VerifySubObject(Footer, "footer");
            }

            verifier
                .Assert(Cells.Count == 0, "Cells.Count != 0")
                .VerifySubObject(Body, "body");

            return verifier.Result;
        } // method Verify

        #endregion

    } // class SectionBand

} // namespace ManagedIrbis.Reports
