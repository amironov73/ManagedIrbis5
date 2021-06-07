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

/* MediumType.cs -- тип средства, поле 182
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
    /// Поле 182 - тип средства.
    /// Введено в связи с ГОСТ 7.0.100-2018.
    /// </summary>
    public sealed class MediumType
    {
        #region Constants

        /// <summary>
        /// Tag number.
        /// </summary>
        public const int Tag = 182;

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "a";

        #endregion

        #region Properties

        /// <summary>
        /// Код типа средства. Подполе a.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("medium")]
        [JsonPropertyName("medium")]
        public string? MediumCode { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="ContentType"/>
        /// to the <see cref="Field"/>.
        /// </summary>
        public void ApplyToField
            (
                Field field
            )
        {
            field.ApplySubField('a', MediumCode);

        } // method ApplyToField

        /// <summary>
        /// Parse the field.
        /// </summary>
        public static MediumType Parse
            (
                Field field
            )
        {
            var result = new MediumType
            {
                MediumCode = field.GetFirstSubFieldValue('a')
            };

            return result;

        } // method Parse

        /// <summary>
        /// Convert <see cref="ContentType"/>
        /// back to <see cref="Field"/>.
        /// </summary>
        public Field ToField()
        {
            var result = new Field(Tag)
                .AddNonEmptySubField('a', MediumCode);

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
            MediumCode = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(MediumCode);

        } // method SaveToStream

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => $"MediumCode: {MediumCode.ToVisibleString()}";

        #endregion

    } // class MediumType

} // namespace ManagedIrbis.Fields
