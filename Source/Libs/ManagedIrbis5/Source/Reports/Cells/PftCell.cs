// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftCell.cs -- ячейка с PFT-форматированием
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Pft;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Ячейка с PFT-форматированием.
    /// </summary>
    public class PftCell
        : ReportCell
    {
        #region Properties

        /// <summary>
        /// PFT-скрипт.
        /// </summary>
        [JsonPropertyName("text")]
        [XmlAttribute("text")]
        public string? Text { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCell()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftCell
            (
                string format
            )
        {
            Text = format;
        }

        #endregion

        #region Private members

        private PftFormatter? _formatter;

        #endregion

        #region Public methods

        #endregion

        #region ReportCell members

        /// <inheritdoc cref="ReportCell.Compute"/>
        public override string? Compute
            (
                ReportContext context
            )
        {
            Magna.Trace("PftCell::Compute");

            OnBeforeCompute(context);

            var text = Text;

            if (string.IsNullOrEmpty(text))
            {
                // TODO: Skip or not on empty format?

                return null;
            }

            string? result = null;

            /*

            ConnectedClient connected
                = context.Provider as ConnectedClient;
            if (!ReferenceEquals(connected, null))
            {
                var record = context.CurrentRecord;
                if (!ReferenceEquals(record, null))
                {
                    result = connected.FormatRecord
                    (
                        record,
                        text
                    );
                }
            }
            else
            {
                if (ReferenceEquals(_formatter, null))
                {
                    _formatter = context.GetFormatter(text);
                }

                context.SetVariables(_formatter);

                result
                    = _formatter.FormatRecord(context.CurrentRecord);

                OnAfterCompute(context);
            }

            */

            return result;
        } // method Compute

        /// <inheritdoc cref="ReportCell.Render" />
        public override void Render
            (
                ReportContext context
            )
        {
            Magna.Trace("PftCell::Render");

            var text = Text;

            if (string.IsNullOrEmpty(text))
            {
                // TODO: Skip or not on empty format?

                return;
            }

            var driver = context.Driver;
            var formatted = Compute(context);

            driver.BeginCell(context, this);
            driver.Write(context, formatted);
            driver.EndCell(context, this);
        } // method Render

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public override void Dispose()
        {
            base.Dispose();

            _formatter?.Dispose();
            _formatter = null;
        } // method Dispose

        #endregion

    } // class PftCell

} // namespace ManagedIrbis.Reports
