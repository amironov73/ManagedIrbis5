// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* FilterBand.cs -- полоса отчета, фильтрующая библиографические записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Полоса отчета, фильтрующая библиографические записи.
    /// </summary>
    public class FilterBand
        : CompositeBand
    {
        #region Properties

        /// <summary>
        /// Filter expression.
        /// </summary>
        [XmlAttribute("filter")]
        [JsonPropertyName("filter")]
        public string? FilterExpression { get; set; }

        #endregion

        #region ReportBand members

        /// <inheritdoc cref="ReportBand.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            OnBeforeRendering(context);

            var expression = FilterExpression;
            if (string.IsNullOrEmpty(expression))
            {
                RenderOnce(context);
            }
            else
            {
                List<Record> list;

                using (var filter = new RecordFilter
                    (
                        context.Provider,
                        expression
                    ))
                {
                    list = filter
                        .FilterRecords(context.Records)
                        .ToList();
                }

                var cloneContext
                    = context.Clone(list);

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
            var verifier
                = new Verifier<ReportBand>(this, throwOnError);

            verifier.Assert(base.Verify(throwOnError));

            verifier.NotNullNorEmpty
                (
                    FilterExpression,
                    "FilterExpression"
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        #endregion

    } // class FilterBand

} // namespace ManagedIrbis.Reports
