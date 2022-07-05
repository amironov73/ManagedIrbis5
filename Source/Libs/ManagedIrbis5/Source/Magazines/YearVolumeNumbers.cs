// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* YearVolumeNumbers.cs -- спецификация "год-том-номера журнала"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines;

/// <summary>
/// Спецификация "год-том-номера журнала".
/// </summary>
[XmlRoot ("issues")]
public sealed class YearVolumeNumbers
{
    #region Properties

    /// <summary>
    /// Год выхода. Обязательный элемент.
    /// </summary>
    [XmlAttribute ("year")]
    [JsonPropertyName ("year")]
    [DisplayName ("Год выхода")]
    [Description ("Год выхода (обязательный)")]
    public string Year { get; }

    /// <summary>
    /// Номер тома. Необязательный элемент.
    /// </summary>
    [XmlAttribute ("volume")]
    [JsonPropertyName ("volume")]
    [DisplayName ("Том")]
    [Description ("Номер тома (необязательный)")]
    public string? Volume { get; }

    /// <summary>
    /// Номер выпуска. Обязательный элемент.
    /// </summary>
    [XmlAttribute ("numbers")]
    [JsonPropertyName ("numbers")]
    [DisplayName ("Номера")]
    [Description ("Номер выпусков (обязательные)")]
    public string[] Numbers { get; }

    #endregion

    #region Construction

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

    #region Object members

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        var hash = new HashCode();
        hash.Add (Year);
        hash.Add (Volume);
        foreach (var number in Numbers)
        {
            hash.Add (number);
        }

        return hash.ToHashCode();
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var numbers = string.Join (',', Numbers);

        return string.IsNullOrEmpty (Volume)
            ? $"{Year}/{numbers}"
            : $"{Year}/{Volume}/{numbers}";
    }

    #endregion
}
