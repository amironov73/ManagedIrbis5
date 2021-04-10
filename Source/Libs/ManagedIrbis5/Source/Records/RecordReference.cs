// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* RecordReference.cs -- ссылка на запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Ссылка на запись (например, для сохранения в "кармане").
    /// </summary>
    [XmlRoot("record")]
    [DebuggerDisplay("MFN={" + nameof(Mfn) + "}, Index={" + nameof(Index) +"}")]
    public sealed class RecordReference
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Database name, e. g. "IBIS".
        /// </summary>
        [XmlAttribute("db")]
        [JsonPropertyName("db")]
        public string? Database { get; set; }

        /// <summary>
        /// MFN of the record.
        /// <c>0</c> means "use <see cref="Index"/> field".
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonPropertyName("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Index of the record, e. g. "81.432.1-42/P41-012833".
        /// </summary>
        [XmlAttribute("index")]
        [JsonPropertyName("index")]
        public string? Index { get; set; }

        /// <summary>
        /// Record itself. Not written.
        /// Can be <c>null</c>.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Record? Record { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RecordReference()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public RecordReference
            (
                Record record
            )
        {
            Database = record.Database;
            Mfn = record.Mfn;
            Index = record.FM(903).ThrowIfEmpty("record.FM(903)").ToString();
            Record = record;
        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Compare two references.
        /// </summary>
        public static bool Compare
            (
                RecordReference first,
                RecordReference second
            )
        {
            if (first.Mfn != 0)
            {
                if (first.Mfn != second.Mfn)
                {
                    return false;
                }
            }

            if (!ReferenceEquals(first.Index, null))
            {
                if (first.Index != second.Index)
                {
                    return false;
                }
            }

            if (first.Mfn == 0 && ReferenceEquals(first.Index, null))
            {
                return false;
            }

            return true;
        } // method Compare

        /// <summary>
        /// Load references from archive file.
        /// </summary>
        public static RecordReference[] LoadFromZipFile
            (
                string fileName
            )
        {
            Sure.NotNull(fileName, nameof(fileName));

            var result = SerializationUtility
                .RestoreArrayFromFile<RecordReference>(fileName);

            return result;
        } // method LoadfromZipFile

        /// <summary>
        /// Load record according to the reference.
        /// </summary>
        public Record? ReadRecord
            (
                ISyncIrbisProvider connection
            )
        {
            Verify(true);

            /*
            if (Mfn != 0)
            {
                Record = connection.ReadRecordAsync
                    (
                        Database.ThrowIfNull("Database"),
                        Mfn
                    )
                    .Result;
            }
            else
            {
                Record = connection.SearchReadOneRecord
                    (
                        "\"I={0}\"",
                        Index
                    );
            }
            */

            return Record;
        } // method ReadRecord

        /// <summary>
        /// Load records according to the references.
        /// </summary>
        public static List<Record> ReadRecords
            (
                ISyncIrbisProvider connection,
                IEnumerable<RecordReference> references,
                bool throwOnError
            )
        {
            var result = new List<Record>();
            foreach (RecordReference reference in references)
            {
                var record = reference.ReadRecord(connection);
                if (ReferenceEquals(record, null))
                {
                    Magna.Error
                        (
                            nameof(RecordReference) + "::" + nameof(ReadRecords)
                            + "record not found: "
                            + reference
                        );

                    if (throwOnError)
                    {
                        throw new IrbisException("record not found");
                    }
                }
                else
                {
                    result.Add(record);
                }
            }

            return result;

            // ReSharper restore PossibleMultipleEnumeration
        } // method ReadRecords


        /// <summary>
        /// Save references to the archive file.
        /// </summary>
        public static void SaveToZipFile
            (
                RecordReference[] references,
                string fileName
            )
        {
            references.SaveToFile(fileName);
        } // method SaveToZipFile

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Database = reader.ReadNullableString();
            Mfn = reader.ReadPackedInt32();
            Index = reader.ReadNullableString();
        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Database)
                .WritePackedInt32(Mfn)
                .WriteNullable(Index);
        } // method SaveToStream

        #endregion

        #region IVerifiable<T> members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<RecordReference>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Database, nameof(Database))
                .Assert
                    (
                        Mfn != 0
                        || !string.IsNullOrEmpty(Index),
                        "Mfn or Index"
                    );

            return verifier.Result;
        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => ReferenceEquals(Record, null)
            ? Database.ToVisibleString()
              + "#"
              + Mfn.ToInvariantString()
              + "#"
              + Index.ToVisibleString()

            : Database.ToVisibleString()
              + IrbisText.IrbisDelimiter
              + Record.ToProtocolText();

        #endregion

    } // class RecordReference

} // namespace ManagedIrbis
