// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* ConditionalBand.cs -- условная полоса отчета
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Условная полоса отчета. Ее рендеринг определяется неким условием,
    /// сформулированным вне этой полосы.
    /// </summary>
    public class ConditionalBand
        : CompositeBand
    {
        #region Properties

        /// <summary>
        /// Conditional expression (PFT).
        /// </summary>
        [XmlAttribute("condition")]
        [JsonPropertyName("condition")]
        public string? ConditionalExpression { get; set; }

        #endregion

        #region ReportBand members

        /// <inheritdoc cref="CompositeBand.Render"/>
        public override void Render
            (
                ReportContext context
            )
        {
            var expression = ConditionalExpression;

            if (string.IsNullOrEmpty(expression))
            {
                base.Render(context);
            }
            else
            {
                /*

                using (PftFormatter formatter
                    = context.GetFormatter(expression))
                {
                    string text = formatter.FormatRecord(null);
                    if (RecordFilter.CheckResult(text))
                    {
                        base.Render(context);
                    }
                }

                */
            }
        } // method Render

        #endregion

    } // class ConditionalBand

} // namespace ManagedIrbis.Reports
