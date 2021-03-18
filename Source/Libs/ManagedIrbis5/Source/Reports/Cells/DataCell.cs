// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* DataCell.cs -- ячейка с данными
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
    /// Ячейка с данными.
    /// </summary>
    public class DataCell
        : ReportCell
    {
        #region Properties

        /// <summary>
        /// Индекс в массиве данных,
        /// связанных с текущей обрабатываемой
        /// библиографической записью.
        /// </summary>
        [JsonPropertyName("index")]
        [XmlAttribute("index")]
        public int Index { get; set; }

        #endregion

        #region ReportCell

        /// <inheritdoc cref="ReportCell.Compute"/>
        public override string? Compute
            (
                ReportContext context
            )
        {
            string? result = null;
            var record = context.CurrentRecord;
            if (record is not null)
            {
                object? obj = record.UserData switch
                {
                    object?[] array => array.SafeAt(Index),
                    IList<object?> list => list.SafeAt(Index),
                    _ => record.UserData
                };

                result = obj?.ToString();
            }

            return result;
        } // method Compute

        /// <inheritdoc cref="ReportCell.Render"/>
        public override void Render
            (
                ReportContext context
            )
        {
            var text = Compute(context);

            ReportDriver driver = context.Driver;
            driver.BeginCell(context, this);
            driver.Write(context, text);
            driver.EndCell(context, this);
        } // method Render

        #endregion

    } // class DataCell

} // namespace ManagedIrbis.Reports
