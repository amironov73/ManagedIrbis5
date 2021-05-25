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
    [XmlRoot("collective")]
    public sealed class CollectiveInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcfnrsx7";

        #endregion

        #region Properties

        /// <summary>
        /// Known tags.
        /// </summary>
        public static int[] KnownTags { get; } = { 710, 711, 962, 972 };

        /// <summary>
        /// Наименование. Подполе a.
        /// </summary>
        [SubField('a')]
        [XmlElement("title")]
        [JsonPropertyName("title")]
        [Description("Наименование")]
        [DisplayName("Наименование")]
        public string? Title { get; set; }

        /// <summary>
        /// Страна. Подполе s.
        /// </summary>
        [SubField('s')]
        [XmlElement("country")]
        [JsonPropertyName("country")]
        [Description("Страна")]
        [DisplayName("Страна")]
        public string? Country { get; set; }

        /// <summary>
        /// Аббревиатура. Подполе r.
        /// </summary>
        [SubField('r')]
        [XmlElement("abbreviation")]
        [JsonPropertyName("abbreviation")]
        [Description("Аббревиатура")]
        [DisplayName("Аббревиатура")]
        public string? Abbreviation { get; set; }

        /// <summary>
        /// Номер. Подполе n.
        /// </summary>
        [SubField('n')]
        [XmlElement("number")]
        [JsonPropertyName("number")]
        [Description("Номер")]
        [DisplayName("Номер")]
        public string? Number { get; set; }

        /// <summary>
        /// Дата проведения мероприятия. Подполе f.
        /// </summary>
        [SubField('f')]
        [XmlElement("date")]
        [JsonPropertyName("date")]
        [Description("Дата проведения мероприятия")]
        [DisplayName("Дата проведения мероприятия")]
        public string? Date { get; set; }

        /// <summary>
        /// Город. Подполе c.
        /// </summary>
        [SubField('c')]
        [XmlElement("city")]
        [JsonPropertyName("city")]
        [Description("Город")]
        [DisplayName("Город")]
        public string? City1 { get; set; }

        /// <summary>
        /// Подразделение. Подполе b.
        /// </summary>
        [SubField('b')]
        [XmlElement("department")]
        [JsonPropertyName("department")]
        [Description("Подразделение")]
        [DisplayName("Подразделение")]
        public string? Department { get; set; }

        /// <summary>
        /// Характерное название подразделения. Подполе x.
        /// </summary>
        [SubField('x')]
        [XmlElement("characteristic")]
        [JsonPropertyName("characteristic")]
        [Description("Характерное название подразделения")]
        [DisplayName("Характерное название подразделения")]
        public bool Characteristic { get; set; }

        /// <summary>
        /// Сокращение по ГОСТ. Подполе 7.
        /// </summary>
        [SubField('7')]
        [XmlElement("gost")]
        [JsonPropertyName("gost")]
        [Description("Сокращение по ГОСТ")]
        [DisplayName("Сокращение по ГОСТ")]
        public string? Gost { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [XmlElement("unknown")]
        [JsonPropertyName("unknown")]
        [Browsable(false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public Field? Field { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectiveInfo()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public CollectiveInfo
            (
                string? title
            )
        {
            Title = title;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="CollectiveInfo"/>
        /// to the <see cref="Field"/>.
        /// </summary>
        public void ApplyToField
            (
                Field field
            )
        {
            field
                .ApplySubField('a', Title)
                .ApplySubField('s', Country)
                .ApplySubField('r', Abbreviation)
                .ApplySubField('n', Number)
                .ApplySubField('f', Date)
                .ApplySubField('c', City1)
                .ApplySubField('b', Department)
                .ApplySubField('x', Characteristic ? "1" : null)
                .ApplySubField('7', Gost);
        }

        /// <summary>
        /// Parse the <see cref="Record"/>.
        /// </summary>
        public static CollectiveInfo[] ParseRecord
            (
                Record record,
                int[] tags
            )
        {
            var result = new List<CollectiveInfo>();
            foreach (var field in record.Fields)
            {
                if (field.Tag.IsOneOf(tags))
                {
                    var collective = ParseField(field);
                    result.Add(collective);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        public static CollectiveInfo ParseField
            (
                Field field
            )
        {
            var result = new CollectiveInfo
            {
                Title = field.GetFirstSubFieldValue('a').ToString(),
                Country = field.GetFirstSubFieldValue('s').ToString(),
                Abbreviation = field.GetFirstSubFieldValue('r').ToString(),
                Number = field.GetFirstSubFieldValue('n').ToString(),
                Date = field.GetFirstSubFieldValue('f').ToString(),
                City1 = field.GetFirstSubFieldValue('c').ToString(),
                Department = field.GetFirstSubFieldValue('b').ToString(),
                Characteristic = !string.IsNullOrEmpty
                    (
                        field.GetFirstSubFieldValue('x').ToString()
                    ),
                Gost = field.GetFirstSubFieldValue('7').ToString(),
                UnknownSubFields = field.Subfields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Characteristic"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCharacteristic()
        {
            return Characteristic;
        }

        /// <summary>
        /// Should serialize the <see cref="UnknownSubFields"/> array?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeUnknownSubFields()
        {
            return !ArrayUtility.IsNullOrEmpty(UnknownSubFields);
        }

        /// <summary>
        /// Convert the <see cref="CollectiveInfo"/>
        /// back to <see cref="Field"/>.
        /// </summary>
        public Field ToField
            (
                int tag
            )
        {
            var result = new Field(tag);
            result
                .AddNonEmptySubField('a', Title)
                .AddNonEmptySubField('s', Country)
                .AddNonEmptySubField('r', Abbreviation)
                .AddNonEmptySubField('n', Number)
                .AddNonEmptySubField('f', Date)
                .AddNonEmptySubField('c', City1)
                .AddNonEmptySubField('b', Department)
                .AddNonEmptySubField('x', Characteristic ? "1" : null)
                .AddNonEmptySubField('7', Gost)
                .AddSubFields(UnknownSubFields);

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
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Title)
                .WriteNullable(Country)
                .WriteNullable(Abbreviation)
                .WriteNullable(Number)
                .WriteNullable(Date)
                .WriteNullable(City1)
                .WriteNullable(Department)
                .WriteNullable(Gost)
                .WriteNullableArray(UnknownSubFields)
                .Write(Characteristic);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<CollectiveInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Title, "Title");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Title.ToVisibleString();

        #endregion

    } // class CollectiveInfo

} // namespace ManagedIrbis.Fields
