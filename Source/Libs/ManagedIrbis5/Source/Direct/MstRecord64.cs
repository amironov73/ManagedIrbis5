// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MstRecord64.cs -- библиографическая запись в MST-файле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using AM.IO;
using AM.Text;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Direct;

/// <summary>
/// Библиографическая запись в MST-файле.
/// </summary>
[DebuggerDisplay ("Leader={" + nameof (Leader) + "}")]
public sealed class MstRecord64
{
    #region Properties

    /// <summary>
    /// MST file offset of the record.
    /// </summary>
    public long Offset { get; set; }

    /// <summary>
    /// Leader.
    /// </summary>
    public MstRecordLeader64 Leader { get; set; }

    /// <summary>
    /// Dictionary.
    /// </summary>
    public List<MstDictionaryEntry64> Dictionary { get; set; }

    /// <summary>
    /// Whether the record deleted.
    /// </summary>
    public bool Deleted
    {
        get
        {
            const int badStatus = (int)(RecordStatus.LogicallyDeleted
                                        | RecordStatus.PhysicallyDeleted);

            return (Leader.Status & badStatus) != 0;
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public MstRecord64()
    {
        Leader = new MstRecordLeader64();
        Dictionary = new List<MstDictionaryEntry64>();
    }

    #endregion

    #region Private members

    private string _DumpDictionary()
    {
        var builder = StringBuilderPool.Shared.Get();

        foreach (var entry in Dictionary)
        {
            builder.AppendLine (entry.ToString());
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Decode the field.
    /// </summary>
    public Field DecodeField
        (
            MstDictionaryEntry64 entry
        )
    {
        var result = new Field (entry.Tag);
        result.DecodeBody (entry.Text);

        return result;
    }

    /// <summary>
    /// Decode the record.
    /// </summary>
    public Record DecodeRecord()
    {
        var result = new Record
        {
            Mfn = Leader.Mfn,
            Status = (RecordStatus)Leader.Status,
            Version = Leader.Version
        };

        // result.Fields.BeginUpdate();
        // result.Fields.EnsureCapacity(Dictionary.Count);

        foreach (var entry in Dictionary)
        {
            var field = DecodeField (entry);
            result.Fields.Add (field);
        }

        // result.Fields.EndUpdate();

        return result;
    }

    /// <summary>
    /// Encode the field.
    /// </summary>
    public static MstDictionaryEntry64 EncodeField
        (
            Field field
        )
    {
        var result = new MstDictionaryEntry64
        {
            Tag = field.Tag,
            Text = field.ToText()
        };

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    public static MstRecord64 EncodeRecord
        (
            Record record
        )
    {
        var leader = new MstRecordLeader64
        {
            Mfn = record.Mfn,
            Status = (int)record.Status,
            Version = record.Version
        };
        var result = new MstRecord64
        {
            Leader = leader
        };

        if (result.Dictionary.Capacity < record.Fields.Count)
        {
            result.Dictionary.Capacity = record.Fields.Count;
        }

        foreach (var field in record.Fields)
        {
            var entry = EncodeField (field);
            result.Dictionary.Add (entry);
        }

        return result;
    }

    /// <summary>
    /// Prepare the record for serialization.
    /// </summary>
    public void Prepare()
    {
        MstRecordLeader64 leader = Leader;
        Encoding encoding = IrbisEncoding.Utf8;
        leader.Nvf = Dictionary.Count;
        int recordSize = MstRecordLeader64.LeaderSize
                         + Dictionary.Count * MstDictionaryEntry64.EntrySize;
        leader.Base = recordSize;
        int position = 0;
        for (int i = 0; i < Dictionary.Count; i++)
        {
            MstDictionaryEntry64 entry = Dictionary[i];
            entry.Position = position;
            entry.Bytes = encoding.GetBytes (entry.Text);
            int length = entry.Bytes.Length;
            entry.Length = length;
            Dictionary[i] = entry;
            recordSize += length;
            position += length;
        }

        if (recordSize % 2 != 0)
        {
            recordSize++;
        }

        leader.Length = recordSize;

        Leader = leader;
    }

    /// <summary>
    /// Write the record to specified stream.
    /// </summary>
    public void Write
        (
            Stream stream
        )
    {
        Leader.Write (stream);
        foreach (MstDictionaryEntry64 entry in Dictionary)
        {
            stream.WriteInt32Network (entry.Tag);
            stream.WriteInt32Network (entry.Position);
            stream.WriteInt32Network (entry.Length);
        }

        foreach (MstDictionaryEntry64 entry in Dictionary)
        {
            stream.Write (entry.Bytes, 0, entry.Bytes.Length);
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return $"Leader: {Leader}{Environment.NewLine}Dictionary: {_DumpDictionary()}";
    }

    #endregion
}
