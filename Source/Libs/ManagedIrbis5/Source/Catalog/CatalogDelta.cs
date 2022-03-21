// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CatalogDelta.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Catalog;

/// <summary>
///
/// </summary>
[XmlRoot ("catalogDelta")]
public sealed class CatalogDelta
    : IHandmadeSerializable
{
    #region Properties

    /// <summary>
    /// Identifier for LiteDB.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// First date.
    /// </summary>
    [XmlAttribute ("firstDate")]
    [JsonPropertyName ("firstDate")]
    public DateTime FirstDate { get; set; }

    /// <summary>
    /// Second date.
    /// </summary>
    [XmlAttribute ("secondDate")]
    [JsonPropertyName ("secondDate")]
    public DateTime SecondDate { get; set; }

    /// <summary>
    /// Database name.
    /// </summary>
    [XmlAttribute ("database")]
    [JsonPropertyName ("database")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Database { get; set; }

    /// <summary>
    /// New records.
    /// </summary>
    [XmlArray ("new")]
    [XmlArrayItem ("mfn")]
    [JsonPropertyName ("new")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? NewRecords { get; set; }

    /// <summary>
    /// Deleted records.
    /// </summary>
    [XmlArray ("deleted")]
    [XmlArrayItem ("mfn")]
    [JsonPropertyName ("deleted")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? DeletedRecords { get; set; }

    /// <summary>
    /// Altered records.
    /// </summary>
    [XmlArray ("altered")]
    [XmlArrayItem ("mfn")]
    [JsonPropertyName ("altered")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? AlteredRecords { get; set; }

    #endregion

    #region Private members

    private static void _AppendRecords
        (
            StringBuilder builder,
            int[]? records,
            string name
        )
    {
        if (!ReferenceEquals (records, null)
            && records.Length != 0)
        {
            builder.AppendLine
                (
                    string.Format
                        (
                            "{0}: {1}",
                            name,
                            string.Join
                                (
                                    ", ",
                                    records
                                )
                        )
                );
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Create delta for two catalog states.
    /// </summary>
    public static CatalogDelta Create
        (
            CatalogState first,
            CatalogState second
        )
    {
        RecordState[] firstRecords = first.Records
            .ThrowIfNull ("first.Records");
        RecordState[] secondRecords = second.Records
            .ThrowIfNull ("second.Records");

        int[] firstDeleted = first.LogicallyDeleted
            .ThrowIfNull ("first.LogicallyDeleted");
        int[] secondDeleted = second.LogicallyDeleted
            .ThrowIfNull ("second.LogicallyDeleted");

        // TODO compare first.Database with second.Database?

        CatalogDelta result = new CatalogDelta
        {
            FirstDate = first.Date,
            SecondDate = second.Date,
            Database = first.Database,
            NewRecords = secondRecords.Except
                    (
                        firstRecords,
                        new RecordStateComparer.ByMfn()
                    )
                .Select (state => state.Mfn)
                .Where (mfn => mfn != 0)
                .ToArray()
        };


        result.AlteredRecords = secondRecords.Except
                (
                    firstRecords,
                    new RecordStateComparer.ByVersion()
                )
            .Select (state => state.Mfn)
            .Where (mfn => mfn != 0)
            .Except (result.NewRecords.ThrowIfNull ("result.NewRecords"))
            .Except (secondDeleted)
            .ToArray();

        result.DeletedRecords
            = secondDeleted.Except (firstDeleted)
                .Where (mfn => mfn != 0)
                .ToArray();

        return result;
    }

    /// <summary>
    /// Should serialize the <see cref="FirstDate"/> field?
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool ShouldSerializeFirstDate()
    {
        return FirstDate != DateTime.MinValue;
    }

    /// <summary>
    /// Should serialize the <see cref="SecondDate"/> field?
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool ShouldSerializeSecondDate()
    {
        return SecondDate != DateTime.MinValue;
    }

    #endregion

    #region IHandmadeSerializable

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Id = reader.ReadPackedInt32();
        FirstDate = reader.ReadDateTime();
        SecondDate = reader.ReadDateTime();
        Database = reader.ReadNullableString();
        NewRecords = reader.ReadNullableInt32Array();
        DeletedRecords = reader.ReadNullableInt32Array();
        AlteredRecords = reader.ReadNullableInt32Array();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        writer
            .WritePackedInt32 (Id)
            .Write (FirstDate)
            .Write (SecondDate)
            .WriteNullable (Database)
            .WriteNullableArray (NewRecords)
            .WriteNullableArray (DeletedRecords)
            .WriteNullableArray (AlteredRecords);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();

        _AppendRecords (result, NewRecords, "New");
        _AppendRecords (result, DeletedRecords, "Deleted");
        _AppendRecords (result, AlteredRecords, "Altered");

        return result.ToString();
    }

    #endregion
}
