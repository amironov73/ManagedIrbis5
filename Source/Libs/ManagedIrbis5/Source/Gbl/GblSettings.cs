// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* GblSettings.cs -- settings for GBL
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// Settings for GBL execution.
    /// </summary>
    public sealed class GblSettings
        : IVerifiable,
        IHandmadeSerializable
    {
        #region Properties

        /// <summary>
        /// Actualize records after processing.
        /// </summary>
        [JsonPropertyName("actualize")]
        public bool Actualize { get; set; }

        /// <summary>
        /// Process 'autoin.gbl'.
        /// </summary>
        [JsonPropertyName("autoin")]
        public bool Autoin { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        [JsonPropertyName("database")]
        public string? Database { get; set; }

        /// <summary>
        /// File name.
        /// </summary>
        [JsonPropertyName("fileName")]
        public string? FileName { get; set; }

        /// <summary>
        /// First record MFN.
        /// </summary>
        [JsonPropertyName("firstRecord")]
        public int FirstRecord { get; set; }

        /// <summary>
        /// Provide formal control.
        /// </summary>
        [JsonPropertyName("formalControl")]
        public bool FormalControl { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        /// <remarks>0 means 'all records in the database'.
        /// </remarks>
        [JsonPropertyName("maxMfn")]
        public int MaxMfn { get; set; }

        /// <summary>
        /// List of MFN to process.
        /// </summary>
        [JsonPropertyName("mfnList")]
        public int[]? MfnList { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        /// <remarks>0 means 'all records in the database'.
        /// </remarks>
        [JsonPropertyName("minMfn")]
        public int MinMfn { get; set; }

        /// <summary>
        /// Number of records to process.
        /// </summary>
        [JsonPropertyName("numberOfRecords")]
        public int NumberOfRecords { get; set; }

        /// <summary>
        /// Search expression.
        /// </summary>
        [JsonPropertyName("searchExpression")]
        public string? SearchExpression { get; set; }

        /// <summary>
        /// Statements.
        /// </summary>
        [JsonPropertyName("statements")]
        public NonNullCollection<GblStatement> Statements
        {
            get;
            private set;
        }

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
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblSettings
            (
                IIrbisConnection connection
            )
            : this()
        {
            Database = connection.Database;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblSettings
            (
                IIrbisConnection connection,
                IEnumerable<GblStatement> statements
            )
            : this(connection)
        {
            Statements.AddRange(statements);
        }

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
        }

        */

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given interval of MFN.
        /// </summary>
        public static GblSettings ForInterval
            (
                IIrbisConnection connection,
                int minMfn,
                int maxMfn,
                IEnumerable<GblStatement> statements
            )
        {
            GblSettings result = new GblSettings(connection, statements)
            {
                MinMfn = minMfn,
                MaxMfn = maxMfn
            };

            return result;
        }

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given interval of MFN.
        /// </summary>
        public static GblSettings ForInterval
            (
                IIrbisConnection connection,
                string database,
                int minMfn,
                int maxMfn,
                IEnumerable<GblStatement> statements
            )
        {
            GblSettings result = new GblSettings(connection, statements)
            {
                Database = database,
                MinMfn = minMfn,
                MaxMfn = maxMfn
            };

            return result;
        }

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given list of MFN.
        /// </summary>
        public static GblSettings ForList
            (
                IIrbisConnection connection,
                IEnumerable<int> mfnList,
                IEnumerable<GblStatement> statements
            )
        {

            GblSettings result = new GblSettings(connection, statements)
            {
                MfnList = mfnList.ToArray()
            };

            return result;
        }

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given list of MFN.
        /// </summary>
        public static GblSettings ForList
            (
                IIrbisConnection connection,
                string database,
                IEnumerable<int> mfnList,
                IEnumerable<GblStatement> statements
            )
        {
            GblSettings result = new GblSettings(connection, statements)
            {
                Database = database,
                MfnList = mfnList.ToArray()
            };

            return result;
        }

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given list of MFN.
        /// </summary>
        public static GblSettings ForList
            (
                IIrbisConnection connection,
                string database,
                IEnumerable<int> mfnList
            )
        {
            GblSettings result = new GblSettings(connection)
            {
                Database = database,
                MfnList = mfnList.ToArray()
            };

            return result;
        }

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given searchExpression.
        /// </summary>
        public static GblSettings ForSearchExpression
            (
                IIrbisConnection connection,
                string searchExpression,
                IEnumerable<GblStatement> statements
            )
        {
            GblSettings result = new GblSettings(connection, statements)
            {
                SearchExpression = searchExpression
            };

            return result;
        }

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given searchExpression.
        /// </summary>
        public static GblSettings ForSearchExpression
            (
                IIrbisConnection connection,
                string database,
                string searchExpression,
                IEnumerable<GblStatement> statements
            )
        {
            GblSettings result = new GblSettings(connection, statements)
            {
                Database = database,
                SearchExpression = searchExpression
            };

            return result;
        }

        /// <summary>
        /// Set (server) file name.
        /// </summary>
        public GblSettings SetFileName
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            FileName = fileName;

            return this;
        }

        /// <summary>
        /// Set first record and number of records
        /// to process.
        /// </summary>
        public GblSettings SetRange
            (
                int firstRecord,
                int numberOfRecords
            )
        {
            Sure.NonNegative(firstRecord, nameof(firstRecord));
            Sure.NonNegative(numberOfRecords, nameof(numberOfRecords));

            FirstRecord = firstRecord;
            NumberOfRecords = numberOfRecords;

            return this;
        }

        /// <summary>
        /// Set search expression.
        /// </summary>
        public GblSettings SetSearchExpression
            (
                string searchExpression
            )
        {
            Sure.NotNullNorEmpty(searchExpression, nameof(searchExpression));

            SearchExpression = searchExpression;

            return this;
        }

        /*

        /// <summary>
        /// Convert settings to JSON string.
        /// </summary>
        public string ToJson()
        {
            string result = JObject.FromObject(this)
                .ToString(Formatting.None);

            return result;
        }

        */

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull(reader, nameof(reader));

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
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, nameof(writer));

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
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<GblSettings> verifier = new Verifier<GblSettings>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(Database, "Database")
                .Assert(Statements.Count != 0, "Statements");

            foreach (GblStatement statement in Statements)
            {
                statement.Verify(throwOnError);
            }

            return verifier.Result;
        }

        #endregion

    } // class GblSettings

} // namespace ManagedIrbis.Gbl
