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
using ManagedIrbis.Providers;
using ManagedIrbis.Records;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Ссылка на запись (например, для сохранения в "кармане").
/// </summary>
[XmlRoot ("record")]
[DebuggerDisplay ("MFN={" + nameof (Mfn) + "}, Index={" + nameof (Index) + "}")]
public sealed class RecordReference
    : IHandmadeSerializable,
        IVerifiable
{
    #region Properties

    /// <summary>
    /// Имя базы данных, например, "IBIS".
    /// </summary>
    [XmlAttribute ("db")]
    [JsonPropertyName ("db")]
    public string? Database { get; set; }

    /// <summary>
    /// MFN записи.
    /// <c>0</c> означает "использовать свойство <see cref="Index"/>".
    /// </summary>
    [XmlAttribute ("mfn")]
    [JsonPropertyName ("mfn")]
    public int Mfn { get; set; }

    /// <summary>
    /// Шифр записи в базе данных, например, "81.432.1-42/P41-012833".
    /// </summary>
    [XmlAttribute ("index")]
    [JsonPropertyName ("index")]
    public string? Index { get; set; }

    /// <summary>
    /// Собственно запись. Не сохраняется в XML или JSON.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public Record? Record { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public RecordReference()
    {
    } // constructor

    /// <summary>
    /// Constructor.
    /// </summary>
    public RecordReference
        (
            Record record,
            RecordConfiguration? configuration = null
        )
    {
        configuration ??= RecordConfiguration.GetDefault();

        Database = record.Database;
        Mfn = record.Mfn;
        Index = configuration.GetIndex (record).ThrowIfNullOrEmpty();
        Record = record;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Сравнение двух ссылок.
    /// </summary>
    public static bool Compare
        (
            RecordReference first,
            RecordReference second
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);

        if (first.Mfn != 0)
        {
            if (first.Mfn != second.Mfn)
            {
                return false;
            }
        }

        if (!ReferenceEquals (first.Index, null))
        {
            if (first.Index != second.Index)
            {
                return false;
            }
        }

        if (first.Mfn == 0 && ReferenceEquals (first.Index, null))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Загрузка ссылок из архивного файла.
    /// </summary>
    public static RecordReference[] LoadFromZipFile
        (
            string fileName
        )
    {
        Sure.NotNull (fileName);

        var result = SerializationUtility
            .RestoreArrayFromFile<RecordReference> (fileName);

        return result;
    }

    /// <summary>
    /// Загрузка записи по ссылке на нее.
    /// </summary>
    public Record? ReadRecord
        (
            ISyncProvider connection
        )
    {
        Sure.NotNull (connection);

        Verify (true);

        if (Mfn != 0)
        {
            var parameters = new ReadRecordParameters()
            {
                Database = Database.ThrowIfNullOrEmpty(),
                Mfn = Mfn
            };
            Record = connection.ReadRecord<Record> (parameters);
        }
        else
        {
            Record = connection.SearchReadOneRecord
                (
                    $"\"{CommonSearches.IndexPrefix}{Index}\""
                );
        }

        return Record;
    }

    /// <summary>
    /// Load records according to the references.
    /// </summary>
    public static List<Record> ReadRecords
        (
            ISyncProvider connection,
            IEnumerable<RecordReference> references,
            bool throwOnError
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull ((object?) references);

        var result = new List<Record>();
        foreach (var reference in references)
        {
            var record = reference.ReadRecord (connection);
            if (record is null)
            {
                Magna.Logger.LogError
                    (
                        nameof (RecordReference) + "::" + nameof (ReadRecords)
                        + "record not found: {Reference}",
                        reference
                    );

                if (throwOnError)
                {
                    throw new IrbisException ("record not found: " + reference);
                }
            }
            else
            {
                result.Add (record);
            }
        }

        return result;
    }


    /// <summary>
    /// Save references to the archive file.
    /// </summary>
    public static void SaveToZipFile
        (
            RecordReference[] references,
            string fileName
        )
    {
        Sure.NotNull (references);
        Sure.NotNullNorEmpty (fileName);

        references.SaveToFile (fileName);
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Database = reader.ReadNullableString();
        Mfn = reader.ReadPackedInt32();
        Index = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Database)
            .WritePackedInt32 (Mfn)
            .WriteNullable (Index);
    }

    #endregion

    #region IVerifiable<T> members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<RecordReference> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Database, nameof (Database))
            .Assert
                (
                    Mfn != 0
                    || !string.IsNullOrEmpty (Index),
                    "Mfn or Index"
                );

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Record is null
            ? Database.ToVisibleString()
              + "#"
              + Mfn.ToInvariantString()
              + "#"
              + Index.ToVisibleString()
            : Database.ToVisibleString()
              + IrbisText.IrbisDelimiter
              + Record.ToProtocolText();
    }

    #endregion
}
