// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* YearVolumeNumber.cs -- спецификация номера журнала "год-том-номер"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Text;

#endregion

#nullable enable
namespace ManagedIrbis.Magazines;

/// <summary>
/// Спецификация номера журнала "год-том-номер"
/// </summary>
[XmlRoot ("issue")]
public sealed class YearVolumeNumber
    : IVerifiable,
    IEquatable<YearVolumeNumber>,
    IComparable<YearVolumeNumber>
{
    #region Properties

    /// <summary>
    /// Год выхода. Обязательный элемент.
    /// </summary>
    [XmlAttribute ("year")]
    [JsonPropertyName ("year")]
    [Description ("Год выхода")]
    public string? Year { get; set; }

    /// <summary>
    /// Номер тома. Необязательный элемент.
    /// </summary>
    [XmlAttribute ("volume")]
    [JsonPropertyName ("volume")]
    [Description ("Номер тома (необязательный)")]
    public string? Volume { get; set; }

    /// <summary>
    /// Номер выпуска. Обязательный элемент.
    /// </summary>
    [XmlAttribute ("number")]
    [JsonPropertyName ("number")]
    [Description ("Номер выпуска (обязательный)")]
    public string? Number { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public YearVolumeNumber()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public YearVolumeNumber
        (
            string year,
            string number
        )
    {
        Sure.NotNullNorEmpty (year);
        Sure.NotNullNorEmpty (number);

        Year = year;
        Number = number;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public YearVolumeNumber
        (
            string year,
            string? volume,
            string number
        )
    {
        Sure.NotNullNorEmpty (year);
        Sure.NotNullNorEmpty (number);

        Year = year;
        Volume = volume;
        Number = number;
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
            Number = part1.ToString();
        }
        else
        {
            Volume = part1.ToString();
            Number = part2.ToString();
        }
    }

    #endregion

    #region IEquatable members

    /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
    public bool Equals
        (
            YearVolumeNumber? other
        )
    {
        if (other is null)
        {
            return false;
        }

        var result = Year.SameCompare (other.Year);
        if (result is 0)
        {
            result = Volume.SameCompare (other.Volume);
            if (result is 0)
            {
                result = Number.SameCompare (other.Number);
            }
        }

        return result is 0;
    }

    #endregion

    #region IComparable members

    /// <inheritdoc cref="IComparable{T}.CompareTo"/>
    public int CompareTo
        (
            YearVolumeNumber? other
        )
    {
        if (other is null)
        {
            return 1;
        }

        var result = Year.SameCompare (other.Year);
        if (result is 0)
        {
            result = Volume.SameCompare (other.Volume);
            if (result is 0)
            {
                result = Number.SameCompare (other.Number);
            }
        }

        return result;
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<YearVolumeNumber> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Year)
            .NotNullNorEmpty (Number);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return string.IsNullOrEmpty (Volume)
            ? $"{Year}/{Number}"
            : $"{Year}/{Volume}/{Number}";
    }

    #endregion
}
