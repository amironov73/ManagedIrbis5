// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* YearVolumeNumbers.cs -- спецификация "год-том-номера журнала"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines;

/// <summary>
/// Спецификация "год-том-номера журнала".
/// </summary>
[XmlRoot ("issues")]
public sealed class YearVolumeNumbers
    : IVerifiable,
    IHandmadeSerializable
{
    #region Properties

    /// <summary>
    /// Год выхода. Обязательный элемент.
    /// </summary>
    [XmlAttribute ("year")]
    [JsonPropertyName ("year")]
    [DisplayName ("Год выхода")]
    [Description ("Год выхода (обязательный)")]
    public string? Year { get; set; }

    /// <summary>
    /// Номер тома. Необязательный элемент.
    /// </summary>
    [XmlAttribute ("volume")]
    [JsonPropertyName ("volume")]
    [DisplayName ("Том")]
    [Description ("Номер тома (необязательный)")]
    public string? Volume { get; set; }

    /// <summary>
    /// Номер выпуска. Обязательный элемент.
    /// </summary>
    [XmlAttribute ("numbers")]
    [JsonPropertyName ("numbers")]
    [DisplayName ("Номера")]
    [Description ("Номер выпусков (обязательные)")]
    public string[]? Numbers { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public YearVolumeNumbers()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public YearVolumeNumbers
        (
            string year,
            string[] numbers
        )
    {
        Sure.NotNullNorEmpty (year);
        Sure.NotNull (numbers);
        Sure.AssertState (numbers.Length != 0);

        Year = year;
        Numbers = numbers;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public YearVolumeNumbers
        (
            string year,
            string? volume,
            string[] numbers
        )
    {
        Sure.NotNullNorEmpty (year);
        Sure.NotNull (numbers);
        Sure.AssertState (numbers.Length != 0);

        Year = year;
        Volume = volume;
        Numbers = numbers;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор текстового представления выпуска.
    /// </summary>
    public void Parse
        (
            ReadOnlySpan<char> text
        )
    {
        const StringSplitOptions options = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
        var navigator = new ValueTextNavigator (text);

        var year = navigator.ReadUntil ('/');
        var part1 = navigator.ReadUntil ('/');
        var part2 = navigator.GetRemainingText();

        if (part1.IsEmpty)
        {
            throw new FormatException();
        }

        Year = year.ToString();
        if (part2.IsEmpty)
        {
            Volume = null;
            Numbers = part1.ToString().Split (',', options);
        }
        else
        {
            Volume = part1.ToString();
            Numbers = part2.ToString().Split (',', options);
        }
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Year = reader.ReadNullableString();
        Volume = reader.ReadNullableString();
        Numbers = reader.ReadNullableStringArray();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.WriteNullable (Year);
        writer.WriteNullable (Volume);
        writer.WriteNullableArray (Numbers);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    [Pure]
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<YearVolumeNumbers> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Year)
            .NotNull (Numbers)
            .Assert (Numbers!.Length != 0);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add (Year);
        hash.Add (Volume);

        if (Numbers is not null)
        {
            foreach (var number in Numbers)
            {
                hash.Add (number);
            }
        }

        return hash.ToHashCode();
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var numbers = Numbers is null
            ? string.Empty
            : string.Join (',', Numbers);

        return string.IsNullOrEmpty (Volume)
            ? $"{Year}/{numbers}"
            : $"{Year}/{Volume}/{numbers}";
    }

    #endregion
}
