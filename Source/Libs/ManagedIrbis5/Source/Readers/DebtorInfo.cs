// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* DebtorInfo.cs -- информация о читателе-задолжнике
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
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
/// Информация о читателе-задолжнике.
/// </summary>
[DebuggerDisplay ("Name")]
public sealed class DebtorInfo
    : IHandmadeSerializable
{
    #region Properties

    /// <summary>
    /// Фамилия, имя, отчество.
    /// </summary>
    [XmlAttribute ("name")]
    [JsonPropertyName ("name")]
    [DisplayName ("ФИО")]
    [Description ("Фамилия, имя, отчество")]
    public string? Name { get; set; }

    /// <summary>
    /// Дата рождения.
    /// </summary>.
    [XmlAttribute ("dateOfBirth")]
    [JsonPropertyName ("dateOfBirth")]
    [DisplayName ("Дата рождения")]
    [Description ("Дата рождения")]
    public string? DateOfBirth { get; set; }

    /// <summary>
    /// Номер читательского билета.
    /// </summary>
    [XmlAttribute ("ticket")]
    [JsonPropertyName ("ticket")]
    [DisplayName ("Билет")]
    [Description ("Номер читательского билета")]
    public string? Ticket { get; set; }

    /// <summary>
    /// Пол.
    /// </summary>
    [XmlAttribute ("gender")]
    [JsonPropertyName ("gender")]
    [DisplayName ("Пол")]
    [Description ("Пол")]
    public string? Gender { get; set; }

    /// <summary>
    /// Категория.
    /// </summary>
    [XmlAttribute ("category")]
    [JsonPropertyName ("category")]
    [DisplayName ("Категория")]
    [Description ("Категория")]
    public string? Category { get; set; }

    /// <summary>
    /// Адрес.
    /// </summary>
    [XmlAttribute ("address")]
    [JsonPropertyName ("address")]
    [DisplayName ("Адрес")]
    [Description ("Адрес")]
    public string? Address { get; set; }

    /// <summary>
    /// Место работы.
    /// </summary>
    [XmlAttribute ("workPlace")]
    [JsonPropertyName ("workPlace")]
    [DisplayName ("Место работы")]
    [Description ("Место работы")]
    public string? WorkPlace { get; set; }

    /// <summary>
    /// Электронная почта.
    /// </summary>
    [XmlAttribute ("email")]
    [JsonPropertyName ("email")]
    [DisplayName ("E-mail")]
    [Description ("Электронная почта")]
    public string? Email { get; set; }

    /// <summary>
    /// Домашний телефон.
    /// </summary>
    [XmlAttribute ("homePhone")]
    [JsonPropertyName ("homePhone")]
    [DisplayName ("Домашний телефон")]
    [Description ("Домашний телефон")]
    public string? HomePhone { get; set; }

    /// <summary>
    /// Возраст.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public int Age { get; set; }

    /// <summary>
    /// Примечания в произвольной форме.
    /// </summary>
    [XmlAttribute ("remarks")]
    [JsonPropertyName ("remarks")]
    [DisplayName ("Примечания")]
    [Description ("Примечания в произвольной форме")]
    public string? Remarks { get; set; }

    /// <summary>
    /// MFN записи.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public int Mfn { get; set; }

    /// <summary>
    /// Расформатированное описание.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public string? Description { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public object? UserData { get; set; }

    /// <summary>
    /// Массив задолженных экземпляров.
    /// </summary>
    [XmlArrayItem ("exemplar")]
    [JsonPropertyName ("exemplars")]
    [DisplayName ("Задолженность")]
    [Description ("Массив задолженных экземпляров")]
    public VisitInfo[]? Debt { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Формирование задолжника из читателя.
    /// </summary>
    public static DebtorInfo FromReader
        (
            ReaderInfo reader,
            VisitInfo[] debt
        )
    {
        var result = new DebtorInfo
        {
            Name = reader.FullName,
            DateOfBirth = reader.DateOfBirth,
            Ticket = reader.Ticket,
            Gender = reader.Gender,
            Category = reader.Category,
            Address = reader.Address?.ToString(),
            WorkPlace = reader.WorkPlace,
            Email = reader.Email,
            HomePhone = reader.HomePhone,
            Age = reader.Age,
            Remarks = reader.Remarks,
            Mfn = reader.Mfn,
            Debt = debt
        };

        return result;
    }

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
        DateOfBirth = reader.ReadNullableString();
        Ticket = reader.ReadNullableString();
        Gender = reader.ReadNullableString();
        Category = reader.ReadNullableString();
        Address = reader.ReadNullableString();
        WorkPlace = reader.ReadNullableString();
        Email = reader.ReadNullableString();
        HomePhone = reader.ReadNullableString();
        Age = reader.ReadPackedInt32();
        Remarks = reader.ReadNullableString();
        Mfn = reader.ReadPackedInt32();
        Description = reader.ReadNullableString();
        Debt = reader.ReadNullableArray<VisitInfo>();
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
            .WriteNullable (DateOfBirth)
            .WriteNullable (Ticket)
            .WriteNullable (Gender)
            .WriteNullable (Category)
            .WriteNullable (Address)
            .WriteNullable (WorkPlace)
            .WriteNullable (Email)
            .WriteNullable (HomePhone)
            .WritePackedInt32 (Age)
            .WriteNullable (Remarks)
            .WritePackedInt32 (Mfn)
            .WriteNullable (Description)
            .WriteNullableArray (Debt);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Name.ToVisibleString();
    }

    #endregion
}
