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
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* NumberOfTitlesInfo.cs -- число наименований, впервые поступивших в библиотеку
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

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
    /// Число наименований, впервые поступивших в библиотеку,
    /// поле 18 в БД CMPL.
    /// </summary>
    [XmlRoot("numberOfTitles")]
    public sealed class NumberOfTitlesInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Tag number.
        /// </summary>
        public const int Tag = 18;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "1234567";

        #endregion

        #region Properties

        /// <summary>
        /// Вновь поступившие. Подполе 1.
        /// </summary>
        [SubField('1')]
        [XmlAttribute("arrivals")]
        [JsonPropertyName("arrivals")]
        [Description("Вновь поступившие")]
        [DisplayName("Вновь поступившие")]
        public string? NewArrivals { get; set; }

        /// <summary>
        /// Книги. Подполе 2.
        /// </summary>
        [SubField('2')]
        [XmlAttribute("books")]
        [JsonPropertyName("books")]
        [Description("Книги")]
        [DisplayName("Книги")]
        public string? Books { get; set; }

        /// <summary>
        /// Монографические издания. Подполе 3.
        /// </summary>
        [SubField('3')]
        [XmlAttribute("monographic")]
        [JsonPropertyName("monographic")]
        [Description("Монографические издания")]
        [DisplayName("Монографические издания")]
        public string? Monographic { get; set; }

        /// <summary>
        /// Брошюры. Подполе 4.
        /// </summary>
        [SubField('4')]
        [XmlAttribute("brochures")]
        [JsonPropertyName("brochures")]
        [Description("Брошюры")]
        [DisplayName("Брошюры")]
        public string? Brochures { get; set; }

        /// <summary>
        /// Число томов. Подполе 5.
        /// </summary>
        [SubField('5')]
        [XmlAttribute("volumes")]
        [JsonPropertyName("volumes")]
        [Description("Число томов")]
        [DisplayName("Число томов")]
        public string? Volumes { get; set; }

        /// <summary>
        /// Отечественные издания. Подполе 6.
        /// </summary>
        [SubField('6')]
        [XmlAttribute("domestic")]
        [JsonPropertyName("domestic")]
        [Description("Отечественные издания")]
        [DisplayName("Отечественные издания")]
        public string? Domestic { get; set; }

        /// <summary>
        /// Иностранные книги. Подполе 7.
        /// </summary>
        [SubField('7')]
        [XmlAttribute("foreign")]
        [JsonPropertyName("foreign")]
        [Description("Иностранные книги")]
        [DisplayName("Иностранные книги")]
        public string? Foreign { get; set; }

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

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="NumberOfTitlesInfo"/>
        /// to the <see cref="Field"/>.
        /// </summary>
        public void ApplyToField
            (
                Field field
            )
        {
            field
                .ApplySubField('1', NewArrivals)
                .ApplySubField('2', Books)
                .ApplySubField('3', Monographic)
                .ApplySubField('4', Brochures)
                .ApplySubField('5', Volumes)
                .ApplySubField('6', Domestic)
                .ApplySubField('7', Foreign);

        } // method ApplyToField

        /// <summary>
        /// Parse the <see cref="Field"/>.
        /// </summary>
        public static NumberOfTitlesInfo Parse
            (
                Field field
            )
        {
            var result = new NumberOfTitlesInfo
            {
                NewArrivals = field.GetFirstSubFieldValue('1'),
                Books = field.GetFirstSubFieldValue('2'),
                Monographic = field.GetFirstSubFieldValue('3'),
                Brochures = field.GetFirstSubFieldValue('4'),
                Volumes = field.GetFirstSubFieldValue('5'),
                Domestic = field.GetFirstSubFieldValue('6'),
                Foreign = field.GetFirstSubFieldValue('7'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields(KnownCodes),
                Field = field
            };

            return result;

        } // method Parse

        /// <summary>
        /// Should serialize the <see cref="UnknownSubFields"/> array?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeUnknownSubFields() => !ArrayUtility.IsNullOrEmpty(UnknownSubFields);

        /// <summary>
        /// Convert the <see cref="NumberOfTitlesInfo"/>
        /// back to <see cref="Field"/>.
        /// </summary>
        public Field ToField()
        {
            var result = new Field(Tag)
                .AddNonEmptySubField('1', NewArrivals)
                .AddNonEmptySubField('2', Books)
                .AddNonEmptySubField('3', Monographic)
                .AddNonEmptySubField('4', Brochures)
                .AddNonEmptySubField('5', Volumes)
                .AddNonEmptySubField('6', Domestic)
                .AddNonEmptySubField('7', Foreign)
                .AddSubFields(UnknownSubFields);

            return result;

        } // method ToField

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            NewArrivals = reader.ReadNullableString();
            Books = reader.ReadNullableString();
            Monographic = reader.ReadNullableString();
            Brochures = reader.ReadNullableString();
            Volumes = reader.ReadNullableString();
            Domestic = reader.ReadNullableString();
            Foreign = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(NewArrivals)
                .WriteNullable(Books)
                .WriteNullable(Monographic)
                .WriteNullable(Brochures)
                .WriteNullable(Volumes)
                .WriteNullable(Domestic)
                .WriteNullable(Foreign)
                .WriteNullableArray(UnknownSubFields);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
           var verifier = new Verifier<NumberOfTitlesInfo>(this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrEmpty(NewArrivals)
                    || !string.IsNullOrEmpty(Books)
                    || !string.IsNullOrEmpty(Monographic)
                    || !string.IsNullOrEmpty(Brochures)
                    || !string.IsNullOrEmpty(Volumes)
                    || !string.IsNullOrEmpty(Domestic)
                    || !string.IsNullOrEmpty(Foreign)
                );

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            // ReSharper disable UseStringInterpolation

            return string.Format
                (
                    "NewArrivals: {0}, Books: {1}, Monographic: {2}, "
                    + "Brochures: {3}, Volumes: {4}, Domestic: {5}, "
                    + "Foreign: {6}",
                    NewArrivals.ToVisibleString(),
                    Books.ToVisibleString(),
                    Monographic.ToVisibleString(),
                    Brochures.ToVisibleString(),
                    Volumes.ToVisibleString(),
                    Domestic.ToVisibleString(),
                    Foreign.ToVisibleString()
                );

            // ReSharper restore UseStringInterpolation

        } // method ToString

        #endregion

    } // class NumberOfTitlesInfo

} // namespace ManagedIrbis.Fields
