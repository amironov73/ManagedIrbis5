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

/* ContentType.cs -- вид содержания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

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
    public sealed class ContentType
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Tag number.
        /// </summary>
        public const int Tag = 181;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcdef";

        #endregion

        #region Propertites

        /// <summary>
        /// Вид содержания. Подполе a.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("content-kind")]
        [JsonPropertyName("contentKind")]
        public string? ContentKind { get; set; }

        /// <summary>
        /// Степень применимости. Подполе b.
        /// </summary>
        [SubField('b')]
        [XmlAttribute("degree-of-applicability")]
        [JsonPropertyName("degreeOfApplicability")]
        public string? DegreeOfApplicability { get; set; }

        /// <summary>
        /// Спецификация типа. Подполе c.
        /// </summary>
        [SubField('c')]
        [XmlAttribute("type-specification")]
        [JsonPropertyName("typeSpecification")]
        public string? TypeSpecification { get; set; }

        /// <summary>
        /// Спецификация движения. Подполе d.
        /// </summary>
        [SubField('d')]
        [XmlAttribute("movement-specification")]
        [JsonPropertyName("movementSpecification")]
        public string? MovementSpecification { get; set; }

        /// <summary>
        /// Спецификация размерности. Подполе e.
        /// </summary>
        [SubField('e')]
        [XmlAttribute("dimension-specification")]
        [JsonPropertyName("dimensionSpecification")]
        public string? DimensionSpecification { get; set; }

        /// <summary>
        /// Сенсорная спецификация. Подполе f.
        /// </summary>
        [SubField('f')]
        [XmlAttribute("sensory-specification")]
        [JsonPropertyName("sensorySpecification")]
        public string? SensorySpecification { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="ContentType"/>
        /// to the <see cref="Field"/>.
        /// </summary>
        public Field ApplyToField (Field field) => field
            .SetSubFieldValue ('a', ContentKind)
            .SetSubFieldValue ('b', DegreeOfApplicability)
            .SetSubFieldValue ('c', TypeSpecification)
            .SetSubFieldValue ('d', MovementSpecification)
            .SetSubFieldValue ('e', DimensionSpecification)
            .SetSubFieldValue ('f', SensorySpecification);

        /// <summary>
        /// Parse the field.
        /// </summary>
        public static ContentType Parse
            (
                Field field
            )
        {
            ContentType result = new ContentType
            {
                ContentKind = field.GetFirstSubFieldValue('a'),
                DegreeOfApplicability = field.GetFirstSubFieldValue('b'),
                TypeSpecification = field.GetFirstSubFieldValue('c'),
                MovementSpecification = field.GetFirstSubFieldValue('d'),
                DimensionSpecification = field.GetFirstSubFieldValue('e'),
                SensorySpecification = field.GetFirstSubFieldValue('f')
            };

            return result;
        }

        /// <summary>
        /// Convert <see cref="ContentType"/>
        /// back to <see cref="Field"/>.
        /// </summary>
        public Field ToField()
        {
            var result = new Field(Tag)
                .AddNonEmpty ('a', ContentKind)
                .AddNonEmpty ('b', DegreeOfApplicability)
                .AddNonEmpty ('c', TypeSpecification)
                .AddNonEmpty ('d', MovementSpecification)
                .AddNonEmpty ('e', DimensionSpecification)
                .AddNonEmpty ('f', SensorySpecification);

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
            ContentKind = reader.ReadNullableString();
            DegreeOfApplicability = reader.ReadNullableString();
            TypeSpecification = reader.ReadNullableString();
            MovementSpecification = reader.ReadNullableString();
            DimensionSpecification = reader.ReadNullableString();
            SensorySpecification = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(ContentKind)
                .WriteNullable(DegreeOfApplicability)
                .WriteNullable(TypeSpecification)
                .WriteNullable(MovementSpecification)
                .WriteNullable(DimensionSpecification)
                .WriteNullable(SensorySpecification);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
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
        }

        #endregion

    } // class ContentType

} // namespace ManagedIrbis.Fields
