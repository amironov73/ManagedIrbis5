// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IndexCell.cs -- ячейка, формирующая нумерацию строк в отчете
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
    /// Ячейка, формирующая нумерацию строк в отчете.
    /// </summary>
    public sealed class IndexCell
        : ReportCell
    {
        #region Properties

        /// <summary>
        /// Format.
        /// </summary>
        [JsonPropertyName("format")]
        [XmlAttribute("format")]
        public string? Format { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IndexCell()
        {
            Format = "{Index})";
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public IndexCell
            (
                string? format
            )
        {
            Format = format;
        } // constructor

        #endregion

        #region ReportCell members

        /// <inheritdoc cref="ReportCell.Compute"/>
        public override string Compute
            (
                ReportContext context
            )
        {
            OnBeforeCompute(context);

            string? result;
            var format = Format;
            if (string.IsNullOrEmpty(format))
            {
                result = (context.Index + 1).ToInvariantString();
            }
            else
            {
                var index = (context.Index + 1)
                    .ToInvariantString();

                var total = context.Records.Count
                    .ToInvariantString();

                result = format
                    .Replace("{Index}", index)
                    .Replace("{Total}", total);
            }

            OnAfterCompute(context);

            return result;
        } // method Compute

        /// <inheritdoc cref="ReportCell.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            var driver = context.Driver;

            driver.BeginCell(context, this);

            var text = Compute(context);
            driver.Write(context, text);

            driver.EndCell(context, this);
        } // method Render

        #endregion

    } // class IndexCell

} // namespace ManagedIrbis.Reports
