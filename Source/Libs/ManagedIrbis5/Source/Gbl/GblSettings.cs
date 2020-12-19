// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* GblSettings.cs -- настройки для выполнения глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// Настройки для выполнения глобальной корректировки.
    /// </summary>
    [XmlRoot("gbl")]
    public sealed class GblSettings
        : IVerifiable,
        IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Actualize records after processing.
        /// </summary>
        [XmlAttribute("actualize")]
        [JsonPropertyName("actualize")]
        public bool Actualize { get; set; }

        /// <summary>
        /// Process 'autoin.gbl'.
        /// </summary>
        [XmlAttribute("autoin")]
        [JsonPropertyName("autoin")]
        public bool Autoin { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [XmlAttribute("database")]
        [JsonPropertyName("database")]
        public string? Database { get; set; }

        /// <summary>
        /// File name.
        /// </summary>
        [XmlAttribute("fileName")]
        [JsonPropertyName("fileName")]
        public string? FileName { get; set; }

        /// <summary>
        /// First record MFN.
        /// </summary>
        [XmlAttribute("firstRecord")]
        [JsonPropertyName("firstRecord")]
        public int FirstRecord { get; set; }

        /// <summary>
        /// Provide formal control.
        /// </summary>
        [XmlAttribute("formalControl")]
        [JsonPropertyName("formalControl")]
        public bool FormalControl { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        /// <remarks>0 means 'all records in the database'.
        /// </remarks>
        [XmlAttribute("maxMfn")]
        [JsonPropertyName("maxMfn")]
        public int MaxMfn { get; set; }

        /// <summary>
        /// List of MFN to process.
        /// </summary>
        [XmlAttribute("mfnList")]
        [JsonPropertyName("mfnList")]
        public int[]? MfnList { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        /// <remarks>0 means 'all records in the database'.
        /// </remarks>
        [XmlAttribute("minMfn")]
        [JsonPropertyName("minMfn")]
        public int MinMfn { get; set; }

        /// <summary>
        /// Number of records to process.
        /// </summary>
        [XmlAttribute("numberOfRecords")]
        [JsonPropertyName("numberOfRecords")]
        public int NumberOfRecords { get; set; }

        /// <summary>
        /// Search expression.
        /// </summary>
        [XmlAttribute("search")]
        [JsonPropertyName("search")]
        public string? SearchExpression { get; set; }

        /// <summary>
        /// Statements.
        /// </summary>
        [JsonPropertyName("statements")]
        public NonNullCollection<GblStatement> Statements { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblSettings()
        {
            Actualize = true;
            Autoin = false;
            FirstRecord = 1;
            FormalControl = false;
            MaxMfn = 0;
            MinMfn = 0;
            Statements = new NonNullCollection<GblStatement>();
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblSettings
            (
                Connection connection
            )
            : this()
        {
            Database = connection.Database;
        } // constructo

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblSettings
            (
                Connection connection,
                IEnumerable<GblStatement> statements
            )
            : this(connection)
        {
            Statements.AddRange(statements);
        } // constructor

        #endregion

        #region Public methods

        /*
        /// <summary>
        /// Restore settings from JSON string.
        /// </summary>
        public static GblSettings FromJson
            (
                string text
            )
        {
            Sure.NotNullNorEmpty(text, nameof(text));

            GblSettings result = JsonConvert
                .DeserializeObject<GblSettings>(text);

            return result;
        } // method FromJson

        /// <summary>
        /// Convert settings to JSON string.
        /// </summary>
        public string ToJson()
        {
            string result = JObject.FromObject(this)
                .ToString(Formatting.None);

            return result;
        } // method ToJson
        */

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Actualize = reader.ReadBoolean();
            Autoin = reader.ReadBoolean();
            Database = reader.ReadNullableString();
            FileName = reader.ReadNullableString();
            FirstRecord = reader.ReadPackedInt32();
            FormalControl = reader.ReadBoolean();
            MaxMfn = reader.ReadPackedInt32();
            MfnList = reader.ReadNullableInt32Array();
            MinMfn = reader.ReadPackedInt32();
            NumberOfRecords = reader.ReadPackedInt32();
            SearchExpression = reader.ReadNullableString();
            Statements = reader.ReadNonNullCollection<GblStatement>();
        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.Write(Actualize);
            writer.Write(Autoin);
            writer.WriteNullable(Database);
            writer.WriteNullable(FileName);
            writer.WritePackedInt32(FirstRecord);
            writer.Write(FormalControl);
            writer.WritePackedInt32(MaxMfn);
            writer.WriteNullableArray(MfnList);
            writer.WritePackedInt32(MinMfn);
            writer.WritePackedInt32(NumberOfRecords);
            writer.WriteNullable(SearchExpression);
            writer.Write(Statements);
        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<GblSettings>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(Database, nameof(Database))
                .Assert(Statements.Count != 0, nameof(Statements));

            foreach (GblStatement statement in Statements)
            {
                statement.Verify(throwOnError);
            }

            return verifier.Result;
        } // method Verify

        #endregion

    } // class GblSettings

} // namespace ManagedIrbis.Gbl
