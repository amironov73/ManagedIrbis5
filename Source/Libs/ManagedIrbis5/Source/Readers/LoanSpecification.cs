// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* LoanSpecification.cs -- спецификация выдачи документа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Readers;

/// <summary>
/// Спецификация выдачи документа.
/// </summary>
[Serializable]
[XmlRoot ("loan")]
public sealed class LoanSpecification
    : IHandmadeSerializable,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Имя базы данных (опционально).
    /// </summary>
    [XmlAttribute ("database")]
    [JsonPropertyName ("database")]
    [DisplayName ("База данных")]
    [Description ("Имя базы данных (опционально)")]
    public string? Database { get; set; }

    /// <summary>
    /// Шифр документа в базе.
    /// </summary>
    [XmlAttribute ("index")]
    [JsonPropertyName ("index")]
    [DisplayName ("Шифр")]
    [Description ("Шифр документа в базе")]
    public string? Index { get; set; }

    /// <summary>
    /// Инвентарный номер выдаваемого экземпляра.
    /// </summary>
    [XmlAttribute ("inventory")]
    [JsonPropertyName ("inventory")]
    [DisplayName ("Инвентарный номер")]
    [Description ("Инвентарный номер выдаваемого экземпляра")]
    public string? InventoryNumber { get; set; }

    /// <summary>
    /// Штрих-код или радиометка выдаваемого экземпляра.
    /// </summary>
    [XmlAttribute ("barcode")]
    [JsonPropertyName ("barcode")]
    [DisplayName ("Штрих-код/радиометка")]
    [Description ("Штрих-код или радиометка выдаваемого экземпляра")]
    public string? Barcode { get; set; }

    /// <summary>
    /// Предполагаемая дата возврата (опционально).
    /// </summary>
    [XmlAttribute ("estimated")]
    [JsonPropertyName ("estimated")]
    [DisplayName ("Дата возврата")]
    [Description ("Предполагаемая дата возврата (опционально)")]
    public string? EstimatedReturnDate { get; set; }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Database = reader.ReadNullableString();
        Index = reader.ReadNullableString();
        InventoryNumber = reader.ReadNullableString();
        Barcode = reader.ReadNullableString();
        EstimatedReturnDate = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Database)
            .WriteNullable (Index)
            .WriteNullable (InventoryNumber)
            .WriteNullable (Barcode)
            .WriteNullable (EstimatedReturnDate);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<LoanSpecification> (this, throwOnError);

        if (string.IsNullOrEmpty (Database) && string.IsNullOrEmpty (Index))
        {
            verifier.AnyNotNullNorEmpty
                (
                    InventoryNumber,
                    Barcode
                );
        }
        else
        {
            verifier.NotNullNorEmpty (Index);
        }

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return (InventoryNumber ?? Barcode).ToVisibleString();
    }

    #endregion
}
