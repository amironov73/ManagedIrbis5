// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TableDefinition.cs -- определение таблицы для команды Table
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Signature for Table command.
    /// </summary>
    [XmlRoot("table")]
    public sealed class TableDefinition
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        [XmlAttribute("database")]
        [JsonPropertyName("database")]
        public string? DatabaseName { get; set; }

        /// <summary>
        /// Table name.
        /// </summary>
        [XmlAttribute("table")]
        [JsonPropertyName("table")]
        public string? Table { get; set; }

        /// <summary>
        /// Table headers.
        /// </summary>
        [XmlElement("header")]
        [JsonPropertyName("headers")]
        public List<string> Headers { get; } = new ();

        /// <summary>
        /// Mode.
        /// </summary>
        [XmlAttribute("mode")]
        [JsonPropertyName("mode")]
        public string? Mode { get; set; }

        /// <summary>
        /// Search query.
        /// </summary>
        [XmlAttribute("search")]
        [JsonPropertyName("search")]
        public string? SearchQuery { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        [XmlAttribute("minMfn")]
        [JsonPropertyName("minMfn")]
        public int MinMfn { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        [XmlAttribute("maxMfn")]
        [JsonPropertyName("maxMfn")]
        public int MaxMfn { get; set; }

        /// <summary>
        /// Optional sequential query.
        /// </summary>
        [XmlAttribute("sequential")]
        [JsonPropertyName("sequential")]
        public string? SequentialQuery { get; set; }

        /// <summary>
        /// List of MFN.
        /// </summary>
        [XmlElement("mfn")]
        [JsonPropertyName("mfn")]
        public List<int> MfnList { get; } = new ();

        #endregion

        #region Public methods

        /// <summary>
        /// Кодирование запроса.
        /// </summary>
        public void Encode<T>
            (
                T query
            )
            where T: IQuery
        {
            // TODO: реализовать заголовки и список MFN

            query.AddAnsi(Table);
            query.NewLine(); // вместо заголовков
            query.AddAnsi(Mode);
            query.AddUtf(SearchQuery);
            query.Add(MinMfn);
            query.Add(MaxMfn);
            query.AddUtf(SequentialQuery);
            query.NewLine(); // вместо списка MFN

        } // method Encode

        /// <summary>
        /// Should serialize the <see cref="Headers"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeHeaders()
        {
            return Headers.Count != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="MaxMfn"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMaxMfn()
        {
            return MaxMfn != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="MinMfn"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMinMfn()
        {
            return MinMfn != 0;
        }

        /// <summary>
        /// Should serialize the <see cref="MfnList"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeMfnList()
        {
            return MfnList.Count != 0;
        }

        #endregion

    } // class TableDefinition

} // namespace ManagedIrbis
