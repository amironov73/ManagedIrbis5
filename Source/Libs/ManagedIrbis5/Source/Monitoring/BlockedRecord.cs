// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* BlockedRecord.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Monitoring;

/// <summary>
/// Информация о заблокированной записи.
/// </summary>
[XmlRoot ("blocked")]
public sealed class BlockedRecord
    : IHandmadeSerializable,
    IVerifiable,
    IEquatable<BlockedRecord>
{
    #region Properties

    /// <summary>
    /// Database.
    /// </summary>
    [XmlAttribute ("database")]
    [JsonPropertyName ("database")]
    public string? Database { get; set; }

    /// <summary>
    /// MFN.
    /// </summary>
    [XmlAttribute ("mfn")]
    [JsonPropertyName ("mfn")]
    public int Mfn { get; set; }

    /// <summary>
    /// Сколько раз запись обнаруживалась заблокированной
    /// системой мониторинга.
    /// </summary>
    [XmlAttribute ("count")]
    [JsonPropertyName ("count")]
    public int Count { get; set; }

    /// <summary>
    /// Since the moment.
    /// </summary>
    [XmlAttribute ("since")]
    [JsonPropertyName ("since")]
    public DateTime Since { get; set; }

    /// <summary>
    /// Пометка.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public bool Marked { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Merge the database info.
    /// </summary>
    public static List<BlockedRecord> Merge
        (
            List<BlockedRecord> list,
            DatabaseInfo[] databases
        )
    {
        Sure.NotNull (list);
        Sure.NotNull (databases);

        foreach (var record in list)
        {
            record.Marked = false;
        }

        foreach (var database in databases)
        {
            var lockedRecords = database.LockedRecords;
            if (ReferenceEquals (lockedRecords, null))
            {
                continue;
            }

            foreach (var mfn in lockedRecords)
            {
                BlockedRecord? found = null;
                for (var i = 0; i < list.Count; i++)
                {
                    var record = list[i];
                    if (record.Database == database.Name
                        && record.Mfn == mfn)
                    {
                        found = record;
                        found.Marked = true;
                        found.Count++;
                    }
                }

                if (ReferenceEquals (found, null))
                {
                    var record = new BlockedRecord
                    {
                        Database = database.Name,
                        Mfn = mfn,
                        Count = 1,
                        Since = DateTime.Now,
                        Marked = true
                    };
                    list.Add (record);
                }
            }
        }

        var records = list.Where (r => !r.Marked).ToArray();
        foreach (var blocked in records)
        {
            list.Remove (blocked);
        }

        return list;
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
        Count = reader.ReadPackedInt32();
        var ticks = reader.ReadInt64();
        Since = new DateTime (ticks);
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
            .WritePackedInt32 (Count)
            .Write (Since.Ticks);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<BlockedRecord> (this, throwOnError);

        verifier
            .Assert (Mfn != 0);

        return verifier.Result;
    }

    #endregion

    #region IEquatable<T> members

    /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
    public bool Equals
        (
            BlockedRecord? other
        )
    {
        if (ReferenceEquals (other, null))
        {
            return false;
        }

        return Database == other.Database
               && Mfn == other.Mfn;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object)" />
    public override bool Equals (object? obj)
    {
        if (ReferenceEquals (obj, null))
        {
            return false;
        }

        if (ReferenceEquals (obj, this))
        {
            return true;
        }

        var record = obj as BlockedRecord;
        if (ReferenceEquals (record, null))
        {
            return false;
        }

        return Equals (record);
    }

    /// <inheritdoc cref="object.GetHashCode" />
    public override int GetHashCode()
    {
        var result = Mfn;
        if (!string.IsNullOrEmpty (Database))
        {
            result = result * 17 + Database.GetHashCode();
        }

        return result;
    }

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Database.ToVisibleString()
               + ":"
               + Mfn.ToInvariantString()
               + ":"
               + Count.ToInvariantString();
    }

    #endregion
}
