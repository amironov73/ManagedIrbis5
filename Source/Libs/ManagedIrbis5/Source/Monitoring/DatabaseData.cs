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

/* DatabaseData.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Monitoring;

/// <summary>
///
/// </summary>
[XmlRoot ("database")]
public sealed class DatabaseData
    : IHandmadeSerializable
{
    #region Properties

    /// <summary>
    /// Database name.
    /// </summary>
    [XmlAttribute ("name")]
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Number of deleted records.
    /// </summary>
    [XmlAttribute ("deletedRecords")]
    [JsonPropertyName ("deletedRecords")]
    public int DeletedRecords { get; set; }

    /// <summary>
    /// Array of locked records.
    /// </summary>
    [XmlArray ("locked")]
    [XmlArrayItem ("mfn")]
    [JsonPropertyName ("lockedRecords")]
    public int[]? LockedRecords { get; set; }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Name = reader.ReadNullableString();
        DeletedRecords = reader.ReadPackedInt32();
        LockedRecords = reader.ReadNullableInt32Array();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Name)
            .WritePackedInt32 (DeletedRecords)
            .WriteNullableArray (LockedRecords);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Name.ToVisibleString();
    }

    #endregion
}
