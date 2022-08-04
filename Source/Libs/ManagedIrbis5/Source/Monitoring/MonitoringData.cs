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

/* MonitoringData.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Monitoring;

/// <summary>
///
/// </summary>
[XmlRoot ("monitoring")]
public sealed class MonitoringData
    : IHandmadeSerializable
{
    #region Properties

    /// <summary>
    /// Moment of time.
    /// </summary>
    [XmlAttribute ("moment")]
    [JsonPropertyName ("moment")]
    public DateTime Moment { get; set; }

    /// <summary>
    /// Сообщение об ошибке (исключении).
    /// </summary>
    [XmlAttribute ("error")]
    [JsonPropertyName ("error")]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Number of running clients.
    /// </summary>
    [XmlAttribute ("clients")]
    [JsonPropertyName ("clients")]
    public int Clients { get; set; }

    /// <summary>
    /// Command count.
    /// </summary>
    [XmlAttribute ("commands")]
    [JsonPropertyName ("commands")]
    public int Commands { get; set; }

    /// <summary>
    /// Ping duration.
    /// </summary>
    [XmlAttribute ("ping")]
    [JsonPropertyName ("ping")]
    public int PingDuration { get; set; }

    /// <summary>
    /// Data for databases.
    /// </summary>
    [XmlElement ("database")]
    [JsonPropertyName ("databases")]
    public DatabaseData[]? Databases { get; set; }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        var ticks = reader.ReadInt64();
        Moment = new DateTime (ticks);
        ErrorMessage = reader.ReadNullableString();
        Clients = reader.ReadPackedInt32();
        Commands = reader.ReadPackedInt32();
        PingDuration = reader.ReadPackedInt32();
        Databases = reader.ReadNullableArray<DatabaseData>();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.Write (Moment.Ticks);
        writer
            .WriteNullable (ErrorMessage)
            .WritePackedInt32 (Clients)
            .WritePackedInt32 (Commands)
            .WritePackedInt32 (PingDuration)
            .WriteNullableArray (Databases);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append (Moment.ToLongUniformString());
        if (Databases is { Length: not 0 })
        {
            builder.Append (':');
            var first = true;
            foreach (var database in Databases)
            {
                if (!first)
                {
                    builder.Append (',');
                }

                builder.Append (database.Name);
                first = false;
            }
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
