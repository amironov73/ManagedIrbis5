// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CollectiveInfo.cs -- коллективный автор
 * Ars Magna project, http://arsmagna.ru
 * Status: poor
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.IO;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Коллективный (в т. ч. временный) автор.
    /// Раскладка полей 710, 711, 962, 972.
    /// </summary>
    [XmlRoot ("collective")]
    public sealed class CollectiveInfo
        : IHandmadeSerializable,
            IVerifiable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abcfnrsx7";

        #endregion

        #region Properties

        /// <summary>
        /// Известные метки полей.
        /// </summary>
        public static int[] KnownTags { get; } = { 710, 711, 962, 972 };

        /// <summary>
        /// Наименование. Подполе A.
        /// </summary>
        [SubField ('a')]
        [XmlElement ("title")]
        [JsonPropertyName ("title")]
        [Description ("Наименование")]
        [DisplayName ("Наименование")]
        public string? Title { get; set; }

        /// <summary>
        /// Страна. Подполе S.
        /// </summary>
        [SubField ('s')]
        [XmlElement ("country")]
        [JsonPropertyName ("country")]
        [Description ("Страна")]
        [DisplayName ("Страна")]
        public string? Country { get; set; }

        /// <summary>
        /// Аббревиатура. Подполе R.
        /// </summary>
        [SubField ('r')]
        [XmlElement ("abbreviation")]
        [JsonPropertyName ("abbreviation")]
        [Description ("Аббревиатура")]
        [DisplayName ("Аббревиатура")]
        public string? Abbreviation { get; set; }

        /// <summary>
        /// Номер. Подполе N.
        /// </summary>
        [SubField ('n')]
        [XmlElement ("number")]
        [JsonPropertyName ("number")]
        [Description ("Номер")]
        [DisplayName ("Номер")]
        public string? Number { get; set; }

        /// <summary>
        /// Дата проведения мероприятия. Подполе F.
        /// </summary>
        [SubField ('f')]
        [XmlElement ("date")]
        [JsonPropertyName ("date")]
        [Description ("Дата проведения мероприятия")]
        [DisplayName ("Дата проведения мероприятия")]
        public string? Date { get; set; }

        /// <summary>
        /// Город. Подполе C.
        /// </summary>
        [SubField ('c')]
        [XmlElement ("city")]
        [JsonPropertyName ("city")]
        [Description ("Город")]
        [DisplayName ("Город")]
        public string? City1 { get; set; }

        /// <summary>
        /// Подразделение. Подполе B.
        /// </summary>
        [SubField ('b')]
        [XmlElement ("department")]
        [JsonPropertyName ("department")]
        [Description ("Подразделение")]
        [DisplayName ("Подразделение")]
        public string? Department { get; set; }

        /// <summary>
        /// Характерное название подразделения. Подполе X.
        /// </summary>
        [SubField ('x')]
        [XmlElement ("characteristic")]
        [JsonPropertyName ("characteristic")]
        [Description ("Характерное название подразделения")]
        [DisplayName ("Характерное название подразделения")]
        public bool Characteristic { get; set; }

        /// <summary>
        /// Сокращение по ГОСТ. Подполе 7.
        /// </summary>
        [SubField ('7')]
        [XmlElement ("gost")]
        [JsonPropertyName ("gost")]
        [Description ("Сокращение по ГОСТ")]
        [DisplayName ("Сокращение по ГОСТ")]
        public string? Gost { get; set; }

        /// <summary>
        /// Неизвестные подполя.
        /// </summary>
        [XmlElement ("unknown")]
        [JsonPropertyName ("unknown")]
        [Browsable (false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Ассоциированное поле библиографической записи.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public Field? Field { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public CollectiveInfo()
        {
        } // constructor

        /// <summary>
        /// Конструктор с наименованием коллектива.
        /// </summary>
        public CollectiveInfo (string? title) => Title = title;

        #endregion

        #region Public methods

        /// <summary>
        /// Применение данных к полю библиографической записи.
        /// </summary>
        public Field ApplyToField (Field field) => field
            .ThrowIfNull ()
            .SetSubFieldValue ('a', Title)
            .SetSubFieldValue ('s', Country)
            .SetSubFieldValue ('r', Abbreviation)
            .SetSubFieldValue ('n', Number)
            .SetSubFieldValue ('f', Date)
            .SetSubFieldValue ('c', City1)
            .SetSubFieldValue ('b', Department)
            .SetSubFieldValue ('x', Characteristic, "1")
            .SetSubFieldValue ('7', Gost);

        /// <summary>
        /// Разбор библиографической записи.
        /// </summary>
        public static CollectiveInfo[] ParseRecord
            (
                Record record,
                IReadOnlyList<int> tags
            )
        {
            Sure.NotNull (record);
            Sure.NotNull (tags);

            var result = new List<CollectiveInfo>();
            foreach (var field in record.Fields)
            {
                if (field.Tag.IsOneOf (tags))
                {
                    var collective = Parse (field);
                    result.Add (collective);
                }
            }

            return result.ToArray();

        } // method ParseRecord

        /// <summary>
        /// Разбор поля библиографической записи.
        /// </summary>
        public static CollectiveInfo Parse
            (
                Field field
            )
        {
            Sure.NotNull (field);

            return new CollectiveInfo
            {
                Title = field.GetFirstSubFieldValue ('a'),
                Country = field.GetFirstSubFieldValue ('s'),
                Abbreviation = field.GetFirstSubFieldValue ('r'),
                Number = field.GetFirstSubFieldValue ('n'),
                Date = field.GetFirstSubFieldValue ('f'),
                City1 = field.GetFirstSubFieldValue ('c'),
                Department = field.GetFirstSubFieldValue ('b'),
                Characteristic = !field.GetFirstSubFieldValue ('x').IsEmpty(),
                Gost = field.GetFirstSubFieldValue ('7'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };

        } // method Parse

        /// <summary>
        /// Необходимо сериализовать свойство <see cref="Characteristic"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeCharacteristic() => Characteristic;

        /// <summary>
        /// Необходимо сериализовать свойство <see cref="UnknownSubFields"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeUnknownSubFields() => !ArrayUtility.IsNullOrEmpty (UnknownSubFields);

        /// <summary>
        /// Преобразование в поле библиографической записи.
        /// </summary>
        public Field ToField (int tag)
        {
            Sure.Positive (tag);

            return new Field (tag)
                .AddNonEmpty ('a', Title)
                .AddNonEmpty ('s', Country)
                .AddNonEmpty ('r', Abbreviation)
                .AddNonEmpty ('n', Number)
                .AddNonEmpty ('f', Date)
                .AddNonEmpty ('c', City1)
                .AddNonEmpty ('b', Department)
                .AddNonEmpty ('x', Characteristic ? "1" : null)
                .AddNonEmpty ('7', Gost)
                .AddRange (UnknownSubFields);

        } // method ToField

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Title = reader.ReadNullableString();
            Country = reader.ReadNullableString();
            Abbreviation = reader.ReadNullableString();
            Number = reader.ReadNullableString();
            Date = reader.ReadNullableString();
            City1 = reader.ReadNullableString();
            Department = reader.ReadNullableString();
            Gost = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();
            Characteristic = reader.ReadBoolean();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Title)
                .WriteNullable (Country)
                .WriteNullable (Abbreviation)
                .WriteNullable (Number)
                .WriteNullable (Date)
                .WriteNullable (City1)
                .WriteNullable (Department)
                .WriteNullable (Gost)
                .WriteNullableArray (UnknownSubFields)
                .Write (Characteristic);

        } // method CollectiveInfo

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<CollectiveInfo> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Title);

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Title.ToVisibleString();

        #endregion

    } // class CollectiveInfo

} // namespace ManagedIrbis.Fields
