// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* RecordFilter.cs -- фильтр для библиографических записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using ManagedIrbis.Pft;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Фильтр для библиографических записей.
    /// </summary>
    public sealed class RecordFilter
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public ISyncProvider Provider { get; internal set; }

        /// <summary>
        /// Булево выражение для фильтрации записей.
        /// </summary>
        [XmlElement("expression")]
        [JsonPropertyName("expression")]
        public string? Expression
        {
            get => _expression;
            set
            {
                _formatter?.Dispose();
                _formatter = null;
                _expression = value;
            }
        } // property Expression

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordFilter()
        {
            Provider = new NullProvider();
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordFilter
            (
                ISyncProvider provider
            )
        {
            Provider = provider;
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordFilter
            (
                ISyncProvider provider,
                string expression
            )
        {
            Provider = provider;
            _expression = expression;
        } // constructor

        #endregion

        #region Private members

        private string? _expression;

        private IPftFormatter? _formatter;

        #endregion

        #region Public methods

        /// <summary>
        /// Check the record.
        /// </summary>
        public bool CheckRecord
            (
                Record record
            )
        {
            var expression = Expression;
            if (string.IsNullOrEmpty(expression))
            {
                return true;
            }

            string? text = null;

            /*

            мфк connected
                = Provider as ConnectedClient;
            if (!ReferenceEquals(connected, null))
            {
                text = connected.FormatRecord
                    (
                        record,
                        expression
                    );
            }
            else
            {
                if (ReferenceEquals(_formatter, null))
                {
                    _formatter = new PftFormatter();
                    _formatter.SetProvider(Provider);
                    _formatter.ParseProgram(expression);
                }

                text = _formatter.FormatRecord(record);
            }
            */

            var result = CheckResult(text);

            return result;
        } // method CheckRecord

        /// <summary>
        /// Check text result.
        /// </summary>
        public static bool CheckResult
            (
                string? text
            )
        {
            if (!int.TryParse(text, out var value))
            {
                return false;
            }

            return value != 0;
        } // method checkResult

        /// <summary>
        /// Filter records.
        /// </summary>
        public IEnumerable<Record> FilterRecords
            (
                IEnumerable<Record> sourceRecords
            )
        {
            foreach (var record in sourceRecords)
            {
                if (CheckRecord(record))
                {
                    yield return record;
                }
            }
        } // method FilterRecords

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (!ReferenceEquals(_formatter, null))
            {
                _formatter.Dispose();
                _formatter = null;
            }
        } // method Dispose

        #endregion

    } // class RecordFilter

} // namespace ManagedIrbis.Reports
