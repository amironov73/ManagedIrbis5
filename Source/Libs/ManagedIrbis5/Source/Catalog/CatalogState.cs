// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* CatalogState.cs -- зафиксированное состояние каталога
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Catalog;

/// <summary>
/// Зафиксированное каталога.
/// </summary>
[XmlRoot ("database")]
[DebuggerDisplay ("{Database} {Date} {MaxMfn}")]
public sealed class CatalogState
    : IHandmadeSerializable
{
    #region Properties

    /// <summary>
    /// Идентификатор для LiteDB.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public int Id { get; set; }

    /// <summary>
    /// Дата фиксации состояния.
    /// </summary>
    [XmlAttribute ("date")]
    [JsonPropertyName ("date")]
    [Description ("Дата")]
    public DateTime Date { get; set; }

    /// <summary>
    /// Имя базы данных.
    /// </summary>
    [XmlAttribute ("database")]
    [JsonPropertyName ("database")]
    [Description ("Имя базы")]
    public string? Database { get; set; }

    /// <summary>
    /// Максимальный MFN.
    /// </summary>
    [XmlAttribute ("maxMfn")]
    [JsonPropertyName ("maxMfn")]
    [Description ("Максимальный MFN")]
    public int MaxMfn { get; set; }

    /// <summary>
    /// Состояние записей.
    /// </summary>
    [XmlArray ("records")]
    [XmlArrayItem ("record")]
    [JsonPropertyName ("records")]
    [Description ("Состояние записей")]
    public RecordState[]? Records { get; set; }

    /// <summary>
    /// Logically deleted records.
    /// </summary>
    [XmlArray ("logicallyDeleted")]
    [XmlArrayItem ("mfn")]
    [JsonPropertyName ("logicallyDeleted")]
    public int[]? LogicallyDeleted { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Should serialize the <see cref="Date"/> field?
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool ShouldSerializeDate()
    {
        return Date != DateTime.MinValue;
    }

    /// <summary>
    /// Should serialize the <see cref="MaxMfn"/> field?
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool ShouldSerializeMaxMfn()
    {
        return MaxMfn != 0;
    }

    #endregion

    #region IHandmadeSerializable

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Id = reader.ReadPackedInt32();
        Date = reader.ReadDateTime();
        Database = reader.ReadNullableString();
        MaxMfn = reader.ReadPackedInt32();
        Records = reader.ReadNullableArray<RecordState>();
        LogicallyDeleted = reader.ReadNullableInt32Array();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WritePackedInt32 (Id)
            .Write (Date)
            .WriteNullable (Database)
            .WritePackedInt32 (MaxMfn)
            .WriteNullableArray (Records)
            .WriteNullableArray (LogicallyDeleted);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Database.ToVisibleString()
               + " "
               + Date.ToLongUniformString()
               + " "
               + MaxMfn.ToInvariantString();
    }

    #endregion
}
