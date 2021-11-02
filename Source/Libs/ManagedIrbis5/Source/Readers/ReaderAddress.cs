// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ReaderAddress.cs -- адрес читателя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Linq;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Адрес читателя: поле 13 в базе RDR.
    /// </summary>
    [XmlRoot("address")]
    public sealed class ReaderAddress
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const int Tag = 13;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcdefgh";

        #endregion

        #region Properties

        /// <summary>
        /// Почтовый индекс. Подполе A.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("postcode")]
        [JsonPropertyName("postcode")]
        public string? Postcode { get; set; }

        /// <summary>
        /// Страна/республика. Подполе B.
        /// </summary>
        [SubField('b')]
        [XmlAttribute("country")]
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        /// <summary>
        /// Город. Подполе C.
        /// </summary>
        [SubField('c')]
        [XmlAttribute("city")]
        [JsonPropertyName("city")]
        public string? City { get; set; }

        /// <summary>
        /// Улица. Подполе D.
        /// </summary>
        [SubField('d')]
        [XmlAttribute("street")]
        [JsonPropertyName("street")]
        public string? Street { get; set; }

        /// <summary>
        /// Номер дома. Подполе E.
        /// </summary>
        [SubField('e')]
        [XmlAttribute("building")]
        [JsonPropertyName("building")]
        public string? Building { get; set; }

        /// <summary>
        /// Номер подъезда. Подполе G.
        /// </summary>
        [SubField('g')]
        [XmlAttribute("entrance")]
        [JsonPropertyName("entrance")]
        public string? Entrance { get; set; }

        /// <summary>
        /// Номер квартиры. Подполе H.
        /// </summary>
        [SubField('h')]
        [XmlAttribute("apartment")]
        [JsonPropertyName("apartment")]
        public string? Apartment { get; set; }

        /// <summary>
        /// Дополнительные данные. Подполе F.
        /// </summary>
        [SubField('f')]
        [XmlAttribute("additionalData")]
        [JsonPropertyName("additionalData")]
        public string? AdditionalData { get; set; }

        /// <summary>
        /// Поле, в котором хранится адрес.
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
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply to the field.
        /// </summary>
        public Field ApplyToField (Field field) => field
            .SetSubFieldValue ('a', Postcode)
            .SetSubFieldValue ('b', Country)
            .SetSubFieldValue ('c', City)
            .SetSubFieldValue ('d', Street)
            .SetSubFieldValue ('e', Building)
            .SetSubFieldValue ('g', Entrance)
            .SetSubFieldValue ('h', Apartment)
            .SetSubFieldValue ('f', AdditionalData);

        /// <summary>
        /// Разбор поля 13.
        /// </summary>
        public static ReaderAddress? Parse
            (
                Field? field
            )
        {
            if (field is null)
            {
                return null;
            }

            return new ReaderAddress
            {
                Postcode = field.GetFirstSubFieldValue('A'),
                Country = field.GetFirstSubFieldValue('B'),
                City = field.GetFirstSubFieldValue('C'),
                Street = field.GetFirstSubFieldValue('D'),
                Building = field.GetFirstSubFieldValue('E'),
                Entrance = field.GetFirstSubFieldValue('G'),
                Apartment = field.GetFirstSubFieldValue('H'),
                AdditionalData = field.GetFirstSubFieldValue('F'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields(KnownCodes),
                Field = field
            };
        } // method Parse

        /// <summary>
        /// Разбор поля 13.
        /// </summary>
        public static ReaderAddress? Parse
            (
                Record record,
                int tag = Tag
            )
        {
            var field = record.Fields.GetFirstField(tag);

            return field is null
                ? null
                : Parse(field);
        } // method Parse

        /// <summary>
        /// Преобразование обратно в поле.
        /// </summary>
        public Field ToField()
        {
            var result = new Field (Tag)
                .AddNonEmpty ('a', Postcode)
                .AddNonEmpty ('b', Country)
                .AddNonEmpty ('c', City)
                .AddNonEmpty ('d', Street)
                .AddNonEmpty ('e', Building)
                .AddNonEmpty ('g', Entrance)
                .AddNonEmpty ('h', Apartment)
                .AddNonEmpty ('f', AdditionalData)
                .AddRange (UnknownSubFields);

            return result;

        } // method ToField

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Postcode = reader.ReadNullableString();
            Country = reader.ReadNullableString();
            City = reader.ReadNullableString();
            Street = reader.ReadNullableString();
            Building = reader.ReadNullableString();
            Entrance = reader.ReadNullableString();
            Apartment = reader.ReadNullableString();
            AdditionalData = reader.ReadNullableString();
        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Postcode);
            writer.WriteNullable(Country);
            writer.WriteNullable(City);
            writer.WriteNullable(Street);
            writer.WriteNullable(Building);
            writer.WriteNullable(Entrance);
            writer.WriteNullable(Apartment);
            writer.WriteNullable(AdditionalData);
        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify(bool throwOnError)
        {
            var verifier
                = new Verifier<ReaderAddress>(this, throwOnError);

            var haveAnyNonNull = new []
                {
                    Postcode,
                    Country,
                    City,
                    Street,
                    Building,
                    Entrance,
                    Apartment,
                    AdditionalData
                }
                .NonNullItems()
                .Count() != 0;

            verifier.Assert(haveAnyNonNull, "address is empty");

            return verifier.Result;
        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var list = new[]
                {
                    Postcode,
                    Country,
                    City,
                    Street,
                    Building,
                    Entrance,
                    Apartment,
                    AdditionalData
                }
                .NonNullItems();

            return string.Join
                (
                    ", ",
                    list
                );
        }

        #endregion

    } // class ReaderAddress

} // namespace ManagedIrbis.Readers
