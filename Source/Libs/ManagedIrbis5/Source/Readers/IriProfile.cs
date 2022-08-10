// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* IriProfile.cs -- профиль ИРИ
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Readers;

/// <summary>
/// Профиль ИРИ.
/// </summary>
[Serializable]
[XmlRoot ("iri-profile")]
public sealed class IriProfile
    : IHandmadeSerializable,
    IVerifiable
{
    #region Constants

    /// <summary>
    /// Тег поля ИРИ.
    /// </summary>
    public const int Tag = 140;

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "abcdefi";

    #endregion

    #region Properties

    /// <summary>
    /// Статус профиля (активен или нет). Подполе A.
    /// </summary>
    [SubField ('a')]
    [XmlAttribute ("active")]
    [JsonPropertyName ("active")]
    [DisplayName ("Статус")]
    [Description ("Статус профиля (активен или нет)")]
    public bool Active { get; set; }

    /// <summary>
    /// Код (порядковый номер). Подполе B.
    /// </summary>
    [SubField ('b')]
    [XmlAttribute ("id")]
    [JsonPropertyName ("id")]
    [DisplayName ("Код")]
    [Description ("Код (порядковый номер) профиля")]
    public string? ID { get; set; }

    /// <summary>
    /// Описание профиля на естественном языке. Подполе C.
    /// </summary>
    [SubField ('c')]
    [XmlAttribute ("title")]
    [JsonPropertyName ("title")]
    [DisplayName ("Описание")]
    [Description ("Описание профиля на естественном языке")]
    public string? Title { get; set; }

    /// <summary>
    /// Запрос на языке ИРБИС. Подполе D.
    /// </summary>
    [SubField ('d')]
    [XmlAttribute ("query")]
    [JsonPropertyName ("query")]
    [DisplayName ("Запрос")]
    [Description ("Запрос на языке ИРБИС")]
    public string? Query { get; set; }

    /// <summary>
    /// Периодичность в днях, целое число. Подполе E.
    /// </summary>
    [SubField ('e')]
    [XmlAttribute ("periodicity")]
    [JsonPropertyName ("periodicity")]
    [DisplayName ("Периодичность")]
    [Description ("Периодичность в днях, целое число")]
    public int Periodicity { get; set; }

    /// <summary>
    /// Дата последнего обслуживания. Подполе F.
    /// </summary>
    [SubField ('f')]
    [XmlAttribute ("lastServed")]
    [JsonPropertyName ("lastServed")]
    [DisplayName ("Дата последнего обслуживания")]
    [Description ("Дата последнего обслуживания")]
    public string? LastServed { get; set; }

    /// <summary>
    /// Список баз данных. Подполе I.
    /// </summary>
    [SubField ('i')]
    [XmlAttribute ("database")]
    [JsonPropertyName ("database")]
    [DisplayName ("Базы данных")]
    [Description ("Список баз данных")]
    public string? Database { get; set; }

    /// <summary>
    /// Поле, в котором хранится данный профиль.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public Field? Field { get; set; }

    /// <summary>
    /// Массив неизвестных подполей.
    /// </summary>
    [XmlElement ("unknown")]
    [JsonPropertyName ("unknown")]
    [Browsable (false)]
    public SubField[]? UnknownSubFields { get; set; }

    /// <summary>
    /// Ссылка на читателя, для которого заведен данный профиль.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public ReaderInfo? Reader { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные,
    /// ассоциированные с данным профилем.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public object? UserData { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор поля библиографической записи, получение профиля ИРИ.
    /// </summary>
    public static IriProfile ParseField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        var active = field.GetFirstSubFieldValue ('a') ?? string.Empty;
        var result = new IriProfile
        {
            Active = Utility.ToBoolean (active),
            ID = field.GetFirstSubFieldValue ('b'),
            Title = field.GetFirstSubFieldValue ('c'),
            Query = field.GetFirstSubFieldValue ('d'),
            Periodicity = field.GetFirstSubFieldValue ('e').SafeToInt32(),
            LastServed = field.GetFirstSubFieldValue ('f'),
            Database = field.GetFirstSubFieldValue ('i'),
            UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
            Field = field
        };

        return result;
    }

    /// <summary>
    /// Преобразование профиля ИРИ в поле библиографической записи.
    /// </summary>
    public Field ToField()
    {
        var result = new Field (Tag)
            .Add ('a', Active)
            .AddNonEmpty ('b', ID)
            .AddNonEmpty ('c', Title)
            .AddNonEmpty ('d', Query)
            .AddNonEmpty ('e', Periodicity.ToInvariantString())
            .AddNonEmpty ('f', LastServed)
            .AddNonEmpty ('i', Database)
            .AddRange (UnknownSubFields);

        return result;
    }

    /// <summary>
    /// Разбор библиографической записи, получение массива профилей ИРИ.
    /// </summary>
    public static IriProfile[] ParseRecord
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var result = new List<IriProfile>();
        foreach (var field in record.Fields.GetField (Tag))
        {
            var profile = ParseField (field);
            result.Add (profile);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Считывание массива профилей ИРИ из указанного файла.
    /// </summary>
    public static IriProfile[] LoadFromFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var result = SerializationUtility
            .RestoreArrayFromFile<IriProfile> (fileName)
            .ThrowIfNull();

        return result;
    }

    /// <summary>
    /// Сохранение массива профилей ИРИ в файл.
    /// </summary>
    public static void SaveToFile
        (
            string fileName,
            IriProfile[] profiles
        )
    {
        Sure.NotNullNorEmpty (fileName);
        Sure.NotNull (profiles);

        profiles.SaveToFile (fileName);
    }

    #endregion

    #region IHandmadeSerializable

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Active = reader.ReadBoolean();
        ID = reader.ReadNullableString();
        Title = reader.ReadNullableString();
        Query = reader.ReadNullableString();
        Periodicity = reader.ReadPackedInt32();
        LastServed = reader.ReadNullableString();
        Database = reader.ReadNullableString();
        UnknownSubFields = reader.ReadNullableArray<SubField>();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.Write (Active);
        writer.WriteNullable (ID);
        writer.WriteNullable (Title);
        writer.WriteNullable (Query);
        writer.WritePackedInt32 (Periodicity);
        writer.WriteNullable (LastServed);
        writer.WriteNullable (Database);
        writer.WriteNullableArray (UnknownSubFields);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<IriProfile> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Query);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return string.IsNullOrEmpty (Title)
            ? Query.ToVisibleString()
            : $"{Title}: {Query}";
    }

    #endregion
}
