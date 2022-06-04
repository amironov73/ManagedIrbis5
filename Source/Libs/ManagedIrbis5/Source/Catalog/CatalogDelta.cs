// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CatalogDelta.cs -- дельта (приращение) каталога
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
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Catalog;

/// <summary>
/// Дельта (приращение) каталога.
/// </summary>
[XmlRoot ("catalogDelta")]
public sealed class CatalogDelta
    : IHandmadeSerializable
{
    #region Properties

    /// <summary>
    /// Идентификатор для LiteDB.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// Первая дата.
    /// </summary>
    [XmlAttribute ("firstDate")]
    [JsonPropertyName ("firstDate")]
    public DateTime FirstDate { get; set; }

    /// <summary>
    /// Вторая дата.
    /// </summary>
    [XmlAttribute ("secondDate")]
    [JsonPropertyName ("secondDate")]
    public DateTime SecondDate { get; set; }

    /// <summary>
    /// Имя базы данных.
    /// </summary>
    [XmlAttribute ("database")]
    [JsonPropertyName ("database")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Database { get; set; }

    /// <summary>
    /// Перечень новых MFN.
    /// </summary>
    [XmlArray ("new")]
    [XmlArrayItem ("mfn")]
    [JsonPropertyName ("new")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? NewRecords { get; set; }

    /// <summary>
    /// Перечень удаленных записей.
    /// </summary>
    [XmlArray ("deleted")]
    [XmlArrayItem ("mfn")]
    [JsonPropertyName ("deleted")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int[]? DeletedRecords { get; set; }

    /// <summary>
    /// Перечень измененных записей.
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
            builder.Append (name);
            builder.Append (": ");
            builder.AppendJoin (", ", records);
            builder.AppendLine();
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Построение дельты для двух заданных состояний каталога.
    /// </summary>
    public static CatalogDelta Create
        (
            CatalogState first,
            CatalogState second
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);

        var firstRecords = first.Records.ThrowIfNull();
        var secondRecords = second.Records.ThrowIfNull();
        var firstDeleted = first.LogicallyDeleted.ThrowIfNull ();
        var secondDeleted = second.LogicallyDeleted.ThrowIfNull ();

        // TODO compare first.Database with second.Database?

        var result = new CatalogDelta
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
            .Except (result.NewRecords.ThrowIfNull())
            .Except (secondDeleted)
            .ToArray();

        result.DeletedRecords = secondDeleted.Except (firstDeleted)
                .Where (mfn => mfn != 0)
                .ToArray();

        return result;
    }

    /// <summary>
    /// Нужно ли сериализовать поле <see cref="FirstDate"/>?
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool ShouldSerializeFirstDate()
    {
        return FirstDate != DateTime.MinValue;
    }

    /// <summary>
    /// Нужно ли сериализовать поле <see cref="SecondDate"/>?
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
        var builder = StringBuilderPool.Shared.Get();

        _AppendRecords (builder, NewRecords, "New");
        _AppendRecords (builder, DeletedRecords, "Deleted");
        _AppendRecords (builder, AlteredRecords, "Altered");

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
