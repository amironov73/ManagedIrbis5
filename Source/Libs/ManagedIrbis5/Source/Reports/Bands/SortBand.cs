// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* SortBand.cs -- полоса отчета с сортировкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Полоса отчета с сортировкой.
    /// </summary>
    public class SortBand
        : CompositeBand
    {
        #region Properties

        /// <summary>
        /// Sort expression.
        /// </summary>
        [XmlAttribute("sort")]
        [JsonPropertyName("sort")]
        public string? SortExpression { get; set; }

        #endregion

        #region ReportBand members

        /// <inheritdoc />
        public override void Render
            (
                ReportContext context
            )
        {
            OnBeforeRendering(context);

            var expression = SortExpression;
            if (string.IsNullOrEmpty(expression))
            {
                RenderOnce(context);
            }
            else
            {
                List<Record> list;

                using (var sorter = new RecordSorter(context.Provider, expression))
                {
                    list = sorter.SortRecords(context.Records);
                }

                var cloneContext = context.Clone(list);

                RenderOnce(cloneContext);
            }

            OnAfterRendering(context);
        } // method Render

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="CompositeBand.Verify"/>
        public override bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ReportBand>(this, throwOnError);

            verifier.Assert(base.Verify(throwOnError));

            verifier.NotNullNorEmpty
                (
                    SortExpression,
                    nameof(SortExpression)
                );

            return verifier.Result;
        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => SortExpression.ToVisibleString();

        #endregion

    } // class SortBand

} // namespace ManagedIrbis.Reports
