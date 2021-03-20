// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TotalCell.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    ///
    /// </summary>
    public class TotalCell
        : ReportCell
    {
        #region Properties

        /// <summary>
        /// Band index.
        /// </summary>
        [JsonPropertyName("band")]
        [XmlAttribute("band")]
        public int BandIndex { get; set; }

        /// <summary>
        /// Cell index.
        /// </summary>
        [JsonPropertyName("cell")]
        [XmlAttribute("cell")]
        public int CellIndex { get; set; }

        /// <summary>
        /// Function.
        /// </summary>
        [JsonPropertyName("function")]
        [XmlAttribute("function")]
        public TotalFunction Function { get; set; }

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
        public TotalCell()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TotalCell
            (
                int bandIndex,
                int cellIndex,
                TotalFunction function
            )
        {
            BandIndex = bandIndex;
            CellIndex = cellIndex;
            Function = function;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TotalCell
            (
                int bandIndex,
                int cellIndex,
                TotalFunction function,
                string? format
            )
        {
            BandIndex = bandIndex;
            CellIndex = cellIndex;
            Function = function;
            Format = format;
        }

        #endregion

        #region Private members

        /// <inheritdoc cref="ReportCell.Compute"/>
        public override string? Compute
            (
                ReportContext context
            )
        {
            OnBeforeCompute(context);

            var band = Band.ThrowIfNull("Band not set");
            var parent = (CompositeBand) band.Parent.ThrowIfNull("Parent not set");
            band = parent.Body[BandIndex];
            var cell = band.Cells[CellIndex];
            var format = Format;

            string? result = null;
            var count = context.Records.Count;

            if (Function == TotalFunction.Count)
            {
                result = context.Records.Count.ToInvariantString();
            }
            else
            {
                var countNonEmpty = 0;
                double accumulator = 0;

                for (var i = 0; i < count; i++)
                {
                    context.Index = i;
                    context.CurrentRecord = context.Records[i];

                    var value = cell.Compute(context);

                    switch (Function)
                    {
                        case TotalFunction.CountNonEmpty:
                            if (!string.IsNullOrEmpty(value))
                            {
                                countNonEmpty++;
                            }
                            result
                                = countNonEmpty.ToInvariantString();
                            break;

                        case TotalFunction.Maximum:
                            if (string.IsNullOrEmpty(result))
                            {
                                result = value;
                            }
                            if (NumberText.Compare
                                (
                                    result,
                                    value
                                ) < 0)
                            {
                                result = value;
                            }
                            break;

                        case TotalFunction.Minimum:
                            if (string.IsNullOrEmpty(result))
                            {
                                result = value;
                            }
                            if (NumberText.Compare
                                (
                                    result,
                                    value
                                ) > 0)
                            {
                                result = value;
                            }
                            break;

                        case TotalFunction.Sum:
                            if (double.TryParse (value, out var number))
                            {
                                accumulator += number;
                                result = string.IsNullOrEmpty(format)
                                    ? accumulator.ToInvariantString()
                                    : accumulator.ToInvariantString(format);
                            }
                            break;
                    }
                }
            }

            context.CurrentRecord = null;
            context.Index = -1;

            OnAfterCompute(context);

            return result;
        }

        /// <inheritdoc cref="ReportCell.Render"/>
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
        }

        #endregion
    }
}
