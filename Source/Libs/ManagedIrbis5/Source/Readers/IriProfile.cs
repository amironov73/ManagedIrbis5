// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IriProfile.cs -- профиль ИРИ
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Профиль ИРИ
    /// </summary>
    [XmlRoot("iri-profile")]
    [DebuggerDisplay("{Title} {Query}")]
    public sealed class IriProfile
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Тег поля ИРИ.
        /// </summary>
        public const int Tag = 140;

        /// <summary>
        /// Известные коды.
        /// </summary>
        public const string KnownCodes = "abcdefi";

        #endregion

        #region Properties

        /// <summary>
        /// Статус профиля (активен или нет). Подполе A.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("active")]
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        /// <summary>
        /// Код (порядковый номер). Подполе B.
        /// </summary>
        [SubField('b')]
        [XmlAttribute("id")]
        [JsonPropertyName("id")]
        // ReSharper disable once InconsistentNaming
        public string? ID { get; set; }

        /// <summary>
        /// Описание профиля на естественном языке. Подполе C.
        /// </summary>
        [SubField('c')]
        [XmlAttribute("title")]
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Запрос на языке ИРБИС. Подполе D.
        /// </summary>
        [SubField('d')]
        [XmlAttribute("query")]
        [JsonPropertyName("query")]
        public string? Query { get; set; }

        /// <summary>
        /// Периодичность в днях, целое число. Подполе E.
        /// </summary>
        [SubField('e')]
        [XmlAttribute("periodicity")]
        [JsonPropertyName("periodicity")]
        public int Periodicity { get; set; }

        /// <summary>
        /// Дата последнего обслуживания. Подполе F.
        /// </summary>
        [SubField('f')]
        [XmlAttribute("lastServed")]
        [JsonPropertyName("lastServed")]
        public string? LastServed { get; set; }

        /// <summary>
        /// Список баз данных. Подполе I.
        /// </summary>
        [SubField('i')]
        [XmlAttribute("database")]
        [JsonPropertyName("database")]
        public string? Database { get; set; }

        /// <summary>
        /// Поле, в котором хранится профиль.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public Field? Field { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [XmlElement("unknown")]
        [JsonPropertyName("unknown")]
        [Browsable(false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Ссылка на читателя.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public ReaderInfo? Reader { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор поля.
        /// </summary>
        public static IriProfile ParseField
            (
                Field field
            )
        {
            var result = new IriProfile
            {
                Active = field.GetFirstSubFieldValue('a') == "1",
                ID = field.GetFirstSubFieldValue('b'),
                Title = field.GetFirstSubFieldValue('c'),
                Query = field.GetFirstSubFieldValue('d'),
                Periodicity = int.Parse(field.GetFirstSubFieldValue('e') ?? "0"),
                LastServed = field.GetFirstSubFieldValue('f'),
                Database = field.GetFirstSubFieldValue('i'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Конверсия обратно в поле.
        /// </summary>
        public Field ToField()
        {
            var result = new Field { Tag = Tag }
                .Add('a', Active ? "1" : "0")
                .AddNonEmptySubField('b', ID)
                .AddNonEmptySubField('c', Title)
                .AddNonEmptySubField('d', Query)
                .AddNonEmptySubField('e', Periodicity.ToInvariantString())
                .AddNonEmptySubField('f', LastServed)
                .AddNonEmptySubField('i', Database);

            if (!ReferenceEquals(UnknownSubFields, null))
            {
                result.AddSubFields(UnknownSubFields);
            }

            return result;
        } // method ToField

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static IriProfile[] ParseRecord
            (
                Record record
            )
        {
            var result = new List<IriProfile>();
            foreach (var field in record.Fields.GetField(Tag))
            {
                var profile = ParseField(field);
                result.Add(profile);
            }

            return result.ToArray();
        }


        /// <summary>
        /// Считывание из файла.
        /// </summary>
        public static IriProfile[] LoadFromFile
            (
                string fileName
            )
        {
            var result = SerializationUtility
                .RestoreArrayFromFile<IriProfile>
                    (
                        fileName
                    )
                .ThrowIfNull("RestoreArrayFromFile");

            return result;
        }

        /// <summary>
        /// Сохранение в файл.
        /// </summary>
        public static void SaveToFile
            (
                string fileName,
                IriProfile[] profiles
            )
        {
            profiles.SaveToFile(fileName);
        } // method SaveToFile

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Active = reader.ReadBoolean();
            ID = reader.ReadNullableString();
            Title = reader.ReadNullableString();
            Query = reader.ReadNullableString();
            Periodicity = reader.ReadPackedInt32();
            LastServed = reader.ReadNullableString();
            Database = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();
        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.Write(Active);
            writer.WriteNullable(ID);
            writer.WriteNullable(Title);
            writer.WriteNullable(Query);
            writer.WritePackedInt32(Periodicity);
            writer.WriteNullable(LastServed);
            writer.WriteNullable(Database);
            writer.WriteNullableArray(UnknownSubFields);
        } // method SaveToStream

        #endregion

    } // class IriProfile

} // namespace ManagedIrbis.Readers

