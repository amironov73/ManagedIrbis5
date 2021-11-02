// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

/* ImprintInfo.cs --выходные данные, поле 210
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
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
    /// Выходные данные, поле 210.
    /// </summary>
    [XmlRoot("imprint")]
    public sealed class ImprintInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "1acdltxy";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const int Tag = 210;

        #endregion

        #region Properties

        /// <summary>
        /// Издательство (издающая организация), подполе c.
        /// </summary>
        [SubField('c')]
        [XmlAttribute("publisher")]
        [JsonPropertyName("publisher")]
        public string? Publisher { get; set; }

        /// <summary>
        /// Издательство на издании, подполе l.
        /// </summary>
        [SubField('l')]
        [XmlAttribute("printedPublisher")]
        [JsonPropertyName("printedPublisher")]
        public string? PrintedPublisher { get; set; }

        /// <summary>
        /// Город1, подполе a.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("city1")]
        [JsonPropertyName("city1")]
        public string? City1 { get; set; }

        /// <summary>
        /// Город2, подполе x.
        /// </summary>
        [SubField('x')]
        [XmlAttribute("city2")]
        [JsonPropertyName("city2")]
        public string? City2 { get; set; }

        /// <summary>
        /// Город3, подполе y.
        /// </summary>
        [SubField('y')]
        [XmlAttribute("city3")]
        [JsonPropertyName("city3")]
        public string? City3 { get; set; }

        /// <summary>
        /// Год издания, подполе d.
        /// </summary>
        [SubField('d')]
        [XmlAttribute("year")]
        [JsonPropertyName("year")]
        public string? Year { get; set; }

        /// <summary>
        /// Место печати, подполе 1.
        /// </summary>
        [SubField('1')]
        [XmlAttribute("place")]
        [JsonPropertyName("place")]
        public string? Place { get; set; }

        /// <summary>
        /// Типография, подполе t.
        /// </summary>
        [SubField('t')]
        [XmlAttribute("printingHouse")]
        [JsonPropertyName("printingHouse")]
        public string? PrintingHouse { get; set; }

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
        public ImprintInfo()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImprintInfo
            (
                string? publisher,
                string? city1,
                string? year
            )
        {
            Publisher = publisher;
            City1 = city1;
            Year = year;

        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="ImprintInfo"/>
        /// to the <see cref="Field"/>.
        /// </summary>
        public Field ApplyToField (Field field) => field
            .SetSubFieldValue ('c', Publisher)
            .SetSubFieldValue ('l', PrintedPublisher)
            .SetSubFieldValue ('a', City1)
            .SetSubFieldValue ('x', City2)
            .SetSubFieldValue ('y', City3)
            .SetSubFieldValue ('d', Year)
            .SetSubFieldValue ('1', Place)
            .SetSubFieldValue ('t', PrintingHouse);

        /// <summary>
        /// Parse the field.
        /// </summary>
        public static ImprintInfo ParseField
            (
                Field field
            )
        {
            var result = new ImprintInfo
            {
                Publisher = field.GetFirstSubFieldValue('c'),
                PrintedPublisher = field.GetFirstSubFieldValue('l'),
                City1 = field.GetFirstSubFieldValue('a'),
                City2 = field.GetFirstSubFieldValue('x'),
                City3 = field.GetFirstSubFieldValue('y'),
                Year = field.GetFirstSubFieldValue('d'),
                Place = field.GetFirstSubFieldValue('1'),
                PrintingHouse = field.GetFirstSubFieldValue('t'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;

        } // method ParseField

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static ImprintInfo[] ParseRecord
            (
                Record record
            )
        {
            var result = new List<ImprintInfo>();
            foreach (var field in record.Fields)
            {
                if (field.Tag == Tag)
                {
                    var print = ParseField(field);
                    result.Add(print);
                }
            }

            return result.ToArray();

        } // method ParseRecord

        /// <summary>
        /// Should serialize <see cref="Publisher"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePublisher() => Publisher is not null;

        /// <summary>
        /// Should serialize <see cref="PrintedPublisher"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePrintedPublisher() => PrintedPublisher is not null;

        /// <summary>
        /// Should serialize <see cref="City1"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCity1() => City1 is not null;

        /// <summary>
        /// Should serialize <see cref="City2"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCity2() => City2 is not null;

        /// <summary>
        /// Should serialize <see cref="City3"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeCity3() => City3 is not null;

        /// <summary>
        /// Should serialize <see cref="Year"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeYear() => Year is not null;

        /// <summary>
        /// Should serialize <see cref="Place"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePlace() => Place is not null;

        /// <summary>
        /// Should serialize <see cref="PrintingHouse"/> field.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializePrintingHouse() =>   PrintingHouse is not null;

        /// <summary>
        /// Should serialize the <see cref="UnknownSubFields"/> array?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeUnknownSubFields() => UnknownSubFields is { Length: not 0 };

        /// <summary>
        /// Превращение обратно в поле.
        /// </summary>
        public Field ToField()
        {
            var result = new Field (Tag)
                .AddNonEmpty ('c', Publisher)
                .AddNonEmpty ('l', PrintedPublisher)
                .AddNonEmpty ('a', City1)
                .AddNonEmpty ('x', City2)
                .AddNonEmpty ('y', City3)
                .AddNonEmpty ('d', Year)
                .AddNonEmpty ('1', Place)
                .AddNonEmpty ('t', PrintingHouse)
                .AddRange (UnknownSubFields);

            return result;

        } // method ToField

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        void IHandmadeSerializable.RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Publisher = reader.ReadNullableString();
            PrintedPublisher = reader.ReadNullableString();
            City1 = reader.ReadNullableString();
            City2 = reader.ReadNullableString();
            City3 = reader.ReadNullableString();
            Year = reader.ReadNullableString();
            Place = reader.ReadNullableString();
            PrintingHouse = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Publisher)
                .WriteNullable(PrintedPublisher)
                .WriteNullable(City1)
                .WriteNullable(City2)
                .WriteNullable(City3)
                .WriteNullable(Year)
                .WriteNullable(Place)
                .WriteNullable(PrintingHouse);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ImprintInfo>(this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty(Publisher)
                    || !string.IsNullOrEmpty(City1)
                    || !string.IsNullOrEmpty(Year)
                );

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            $"{City1.ToVisibleString()}: {Publisher.ToVisibleString()}, {Year.ToVisibleString()}";

        #endregion

    } // class ImprintInfo

} // namespace ManagedIrbis.Fields
