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

/* RecordSorter.cs -- сортировщик библиографических записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using ManagedIrbis.Pft;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    /// Сортировщик библиографических записей.
    /// </summary>
    public class RecordSorter
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
        ///
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
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordSorter()
        {
            Provider = new NullProvider();
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordSorter
            (
                ISyncProvider provider
            )
        {
            Provider = provider;
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordSorter
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
        /// Sort records.
        /// </summary>
        public List<Record> SortRecords
            (
                IEnumerable<Record> sourceRecords
            )
        {
            var expression = Expression;
            if (string.IsNullOrEmpty(expression))
            {
                return sourceRecords.ToList();
            }

            /*

            ConnectedClient connected
                = Provider as ConnectedClient;
            List<Pair<string, Record>> list
                = new List<Pair<string, Record>>();

            if (!ReferenceEquals(connected, null))
            {
                IIrbisConnection connection = connected.Connection;
                foreach (Record record in sourceRecords)
                {
                    string formatted = connection.FormatRecord
                        (
                            expression,
                            record
                        );
                    Pair<string, Record> pair
                        = new Pair<string, Record>
                        (
                            formatted,
                            record
                        );
                    list.Add(pair);
                }
            }
            else
            {
                if (ReferenceEquals(_formatter, null))
                {
                    _formatter = new PftFormatter();
                    _formatter.SetProvider(Provider);
                    _formatter.ParseProgram(expression);
                }

                foreach (Record record in sourceRecords)
                {
                    string formatted = _formatter.FormatRecord
                    (
                        record
                    );
                    Pair<string, Record> pair
                        = new Pair<string, Record>
                        (
                            formatted,
                            record
                        );
                    list.Add(pair);
                }
            }

            list.Sort
                (
                    (left, right) => NumberText.Compare
                    (
                        left.First,
                        right.First
                    )
                );

            var result = list
                .Select(pair => pair.Second)
                .ToList();

            return result;

            */

            throw new NotImplementedException();
        } // method SortRecords

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
        }

        #endregion

    } // class RecordSorter

} // namespace ManagedIrbis.Reports
