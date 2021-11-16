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
    [XmlRoot ("imprint")]
    public sealed class ImprintInfo
        : IHandmadeSerializable,
            IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 210;

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "1acdltxy";

        #endregion

        #region Properties

        /// <summary>
        /// Издательство (издающая организация), подполе C.
        /// </summary>
        [SubField ('c')]
        [XmlAttribute ("publisher")]
        [JsonPropertyName ("publisher")]
        [Description ("Издательство (издающая организация)")]
        [DisplayName ("Издательство (издающая организация)")]
        public string? Publisher { get; set; }

        /// <summary>
        /// Издательство на издании, подполе L.
        /// </summary>
        [SubField ('l')]
        [XmlAttribute ("printedPublisher")]
        [JsonPropertyName ("printedPublisher")]
        [Description ("Издательство на издании")]
        [DisplayName ("Издательство на издании")]
        public string? PrintedPublisher { get; set; }

        /// <summary>
        /// Город1, подполе A.
        /// </summary>
        [SubField ('a')]
        [XmlAttribute ("city1")]
        [JsonPropertyName ("city1")]
        [Description ("Город1")]
        [DisplayName ("Город1")]
        public string? City1 { get; set; }

        /// <summary>
        /// Город2, подполе X.
        /// </summary>
        [SubField ('x')]
        [XmlAttribute ("city2")]
        [JsonPropertyName ("city2")]
        [Description ("Город2")]
        [DisplayName ("Город2")]
        public string? City2 { get; set; }

        /// <summary>
        /// Город3, подполе Y.
        /// </summary>
        [SubField ('y')]
        [XmlAttribute ("city3")]
        [JsonPropertyName ("city3")]
        [Description ("Город3")]
        [DisplayName ("Город3")]
        public string? City3 { get; set; }

        /// <summary>
        /// Год издания, подполе D.
        /// </summary>
        [SubField ('d')]
        [XmlAttribute ("year")]
        [JsonPropertyName ("year")]
        [Description ("Год издания")]
        [DisplayName ("Год издания")]
        public string? Year { get; set; }

        /// <summary>
        /// Место печати, подполе 1.
        /// </summary>
        [SubField ('1')]
        [XmlAttribute ("place")]
        [JsonPropertyName ("place")]
        [Description ("Место печати")]
        [DisplayName ("Место печати")]
        public string? Place { get; set; }

        /// <summary>
        /// Типография, подполе T.
        /// </summary>
        [SubField ('t')]
        [XmlAttribute ("printingHouse")]
        [JsonPropertyName ("printingHouse")]
        [Description ("Типография")]
        [DisplayName ("Типография")]
        public string? PrintingHouse { get; set; }

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
        public ImprintInfo()
        {
        }

        /// <summary>
        /// Конструктор с городом, издательством и годом.
        /// </summary>
        public ImprintInfo
            (
                string? city,
                string? publisher = null,
                string? year = null
            )
        {
            Publisher = publisher;
            City1 = city;
            Year = year;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Примемение данных к указанному полю библиографической записи <see cref="Field"/>.
        /// </summary>
        public Field ApplyTo
            (
                Field field
            )
        {
            Sure.NotNull (field);

            return field
                .SetSubFieldValue ('c', Publisher)
                .SetSubFieldValue ('l', PrintedPublisher)
                .SetSubFieldValue ('a', City1)
                .SetSubFieldValue ('x', City2)
                .SetSubFieldValue ('y', City3)
                .SetSubFieldValue ('d', Year)
                .SetSubFieldValue ('1', Place)
                .SetSubFieldValue ('t', PrintingHouse);
        }

        /// <summary>
        /// Разбор указанного поля библиографической записи.
        /// </summary>
        public static ImprintInfo ParseField
            (
                Field field
            )
        {
            Sure.NotNull (field);

            return new ImprintInfo
            {
                Publisher = field.GetFirstSubFieldValue ('c'),
                PrintedPublisher = field.GetFirstSubFieldValue ('l'),
                City1 = field.GetFirstSubFieldValue ('a'),
                City2 = field.GetFirstSubFieldValue ('x'),
                City3 = field.GetFirstSubFieldValue ('y'),
                Year = field.GetFirstSubFieldValue ('d'),
                Place = field.GetFirstSubFieldValue ('1'),
                PrintingHouse = field.GetFirstSubFieldValue ('t'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };
        }

        /// <summary>
        /// Разбор библиографической записи.
        /// </summary>
        public static ImprintInfo[] ParseRecord
            (
                Record record,
                int tag = Tag
            )
        {
            Sure.NotNull (record);
            Sure.Positive (tag);

            var result = new List<ImprintInfo>();
            foreach (var field in record.Fields)
            {
                if (field.Tag == tag)
                {
                    var print = ParseField (field);
                    result.Add (print);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="Publisher"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializePublisher() => Publisher is not null;

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="PrintedPublisher"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializePrintedPublisher() => PrintedPublisher is not null;

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="City1"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeCity1() => City1 is not null;

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="City2"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeCity2() => City2 is not null;

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="City3"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeCity3() => City3 is not null;

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="Year"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeYear() => Year is not null;

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="Place"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializePlace() => Place is not null;

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="PrintingHouse"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializePrintingHouse() => PrintingHouse is not null;

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="UnknownSubFields"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeUnknownSubFields() => UnknownSubFields is { Length: not 0 };

        /// <summary>
        /// Превращение обратно в поле.
        /// </summary>
        public Field ToField()
        {
            return new Field (Tag)
                .AddNonEmpty ('c', Publisher)
                .AddNonEmpty ('l', PrintedPublisher)
                .AddNonEmpty ('a', City1)
                .AddNonEmpty ('x', City2)
                .AddNonEmpty ('y', City3)
                .AddNonEmpty ('d', Year)
                .AddNonEmpty ('1', Place)
                .AddNonEmpty ('t', PrintingHouse)
                .AddRange (UnknownSubFields);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        void IHandmadeSerializable.RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Publisher = reader.ReadNullableString();
            PrintedPublisher = reader.ReadNullableString();
            City1 = reader.ReadNullableString();
            City2 = reader.ReadNullableString();
            City3 = reader.ReadNullableString();
            Year = reader.ReadNullableString();
            Place = reader.ReadNullableString();
            PrintingHouse = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Publisher)
                .WriteNullable (PrintedPublisher)
                .WriteNullable (City1)
                .WriteNullable (City2)
                .WriteNullable (City3)
                .WriteNullable (Year)
                .WriteNullable (Place)
                .WriteNullable (PrintingHouse)
                .WriteNullableArray (UnknownSubFields);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ImprintInfo> (this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty (Publisher)
                    || !string.IsNullOrEmpty (City1)
                    || !string.IsNullOrEmpty (Year)
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            $"{City1.ToVisibleString()}: {Publisher.ToVisibleString()}, {Year.ToVisibleString()}";

        #endregion

    } // class ImprintInfo

} // namespace ManagedIrbis.Fields
