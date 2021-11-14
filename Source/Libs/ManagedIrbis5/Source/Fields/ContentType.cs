// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UseStringInterpolation

/* ContentType.cs -- вид содержания, поле 181
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

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

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Поле 181 - вид содержания.
    /// Введено в связи с ГОСТ 7.0.100-2018.
    /// </summary>
    [XmlRoot ("content-type")]
    public sealed class ContentType
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 181;

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abcdef";

        #endregion

        #region Propertites

        /// <summary>
        /// Вид содержания. Подполе A.
        /// </summary>
        [SubField ('a')]
        [XmlAttribute ("content-kind")]
        [JsonPropertyName ("contentKind")]
        [Description ("Вид содержания")]
        [DisplayName ("Вид содержания")]
        public string? ContentKind { get; set; }

        /// <summary>
        /// Степень применимости. Подполе B.
        /// </summary>
        [SubField ('b')]
        [XmlAttribute ("degree-of-applicability")]
        [JsonPropertyName ("degreeOfApplicability")]
        [Description ("Степень применимости")]
        [DisplayName ("Степень применимости")]
        public string? DegreeOfApplicability { get; set; }

        /// <summary>
        /// Спецификация типа. Подполе C.
        /// </summary>
        [SubField ('c')]
        [XmlAttribute ("type-specification")]
        [JsonPropertyName ("typeSpecification")]
        [Description ("Спецификация типа")]
        [DisplayName ("Спецификация типа")]
        public string? TypeSpecification { get; set; }

        /// <summary>
        /// Спецификация движения. Подполе D.
        /// </summary>
        [SubField ('d')]
        [XmlAttribute ("movement-specification")]
        [JsonPropertyName ("movementSpecification")]
        [Description ("Спецификация движения")]
        [DisplayName ("Спецификация движения")]
        public string? MovementSpecification { get; set; }

        /// <summary>
        /// Спецификация размерности. Подполе E.
        /// </summary>
        [SubField ('e')]
        [XmlAttribute ("dimension-specification")]
        [JsonPropertyName ("dimensionSpecification")]
        [Description ("Спецификация резмерности")]
        [DisplayName ("Спецификация резмерности")]
        public string? DimensionSpecification { get; set; }

        /// <summary>
        /// Сенсорная спецификация. Подполе F.
        /// </summary>
        [SubField ('f')]
        [XmlAttribute ("sensory-specification")]
        [JsonPropertyName ("sensorySpecification")]
        [Description ("Сенсорная спецификация")]
        [DisplayName ("Сенсорная спецификация")]
        public string? SensorySpecification { get; set; }

        /// <summary>
        /// Неизвестные подполя.
        /// </summary>
        [XmlElement ("unknown")]
        [JsonPropertyName ("unknown")]
        [Browsable (false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Ассоциированное поле библиографической записи <see cref="Field"/>.
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

        #region Public methods

        /// <summary>
        /// Применение данных к указанному полю библиографической записи.
        /// </summary>
        public Field ApplyToField (Field field) => field
            .ThrowIfNull ()
            .SetSubFieldValue ('a', ContentKind)
            .SetSubFieldValue ('b', DegreeOfApplicability)
            .SetSubFieldValue ('c', TypeSpecification)
            .SetSubFieldValue ('d', MovementSpecification)
            .SetSubFieldValue ('e', DimensionSpecification)
            .SetSubFieldValue ('f', SensorySpecification);

        /// <summary>
        /// Разбор указанного поля библиографической записи.
        /// </summary>
        public static ContentType ParseField
            (
                Field field
            )
        {
            Sure.NotNull (field);

            return new ContentType
            {
                ContentKind = field.GetFirstSubFieldValue ('a'),
                DegreeOfApplicability = field.GetFirstSubFieldValue ('b'),
                TypeSpecification = field.GetFirstSubFieldValue ('c'),
                MovementSpecification = field.GetFirstSubFieldValue ('d'),
                DimensionSpecification = field.GetFirstSubFieldValue ('e'),
                SensorySpecification = field.GetFirstSubFieldValue ('f'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };

        } // method ParseField

        /// <summary>
        /// Преобразование в поле библиографической записи.
        /// </summary>
        public Field ToField() => new Field (Tag)
                .AddNonEmpty ('a', ContentKind)
                .AddNonEmpty ('b', DegreeOfApplicability)
                .AddNonEmpty ('c', TypeSpecification)
                .AddNonEmpty ('d', MovementSpecification)
                .AddNonEmpty ('e', DimensionSpecification)
                .AddNonEmpty ('f', SensorySpecification)
                .AddRange (UnknownSubFields);

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            ContentKind = reader.ReadNullableString();
            DegreeOfApplicability = reader.ReadNullableString();
            TypeSpecification = reader.ReadNullableString();
            MovementSpecification = reader.ReadNullableString();
            DimensionSpecification = reader.ReadNullableString();
            SensorySpecification = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (ContentKind)
                .WriteNullable (DegreeOfApplicability)
                .WriteNullable (TypeSpecification)
                .WriteNullable (MovementSpecification)
                .WriteNullable (DimensionSpecification)
                .WriteNullable (SensorySpecification)
                .WriteNullableArray (UnknownSubFields);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ContentType> (this, throwOnError);

            verifier
                .NotNullNorEmpty (ContentKind);

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => string.Format
                (
                    "ContentKind: {0}, DegreeOfApplicability: {1}, "
                    + "TypeSpecification: {2}, MovementSpecification: {3}, "
                    + "DimensionSpecification: {4}, SensorySpecification: {5}",
                    ContentKind.ToVisibleString(),
                    DegreeOfApplicability.ToVisibleString(),
                    TypeSpecification.ToVisibleString(),
                    MovementSpecification.ToVisibleString(),
                    DimensionSpecification.ToVisibleString(),
                    SensorySpecification.ToVisibleString()
                );

        #endregion

    } // class ContentType

} // namespace ManagedIrbis.Fields
