// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RecordReference.cs -- ссылка на запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using JetBrains.Annotations;

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
[PublicAPI]
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
    public required string Database
    {
        get => _database!;
        init => _database = value;
    }

    /// <summary>
    /// MFN записи.
    /// <c>0</c> означает "использовать свойство <see cref="Index"/>".
    /// </summary>
    [XmlAttribute ("mfn")]
    [JsonPropertyName ("mfn")]
    public required int Mfn
    {
        get => _mfn;
        init => _mfn = value;
    }

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
        // пустое тело метода
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    [SetsRequiredMembers]
    public RecordReference
        (
            Record record,
            RecordConfiguration? configuration = null
        )
    {
        Sure.NotNull (record);

        configuration ??= RecordConfiguration.GetDefault();

        Database = record.Database.ThrowIfNullOrEmpty();
        Mfn = record.Mfn;
        Index = configuration.GetIndex (record);
        Record = record;
    }

    #endregion

    #region Private members

    private string? _database;
    private int _mfn;

    #endregion

    #region Public methods

    /// <summary>
    /// Сравнение двух ссылок на равенство.
    /// </summary>
    public static bool AreEqual
        (
            RecordReference first,
            RecordReference second
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);

        if (ReferenceEquals (first, second))
        {
            return true;
        }

        if (first.Mfn != 0)
        {
            if (first.Mfn != second.Mfn)
            {
                return false;
            }
        }

        if (first.Index is not null)
        {
            if (string.CompareOrdinal (first.Index, second.Index) != 0)
            {
                return false;
            }
        }

        if (first is { Mfn: 0, Index: null })
        {
            return false;
        }

        return true;
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
            var parameters = new ReadRecordParameters
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
    /// Загрузка записей согласно ссылкам.
    /// </summary>
    public static List<Record> ReadRecords
        (
            ISyncProvider connection,
            IEnumerable<RecordReference> references,
            bool throwOnError
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull (references);

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

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        _database = reader.ReadNullableString();
        _mfn = reader.ReadPackedInt32();
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

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() => HashCode.Combine (Database, Mfn);

    /// <inheritdoc cref="object.Equals(object)"/>
    public override bool Equals (object? obj) =>
        obj is RecordReference other && AreEqual (this, other);

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
