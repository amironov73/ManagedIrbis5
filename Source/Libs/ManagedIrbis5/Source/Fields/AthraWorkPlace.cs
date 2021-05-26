// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* AthraWorkplace.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Место работы в базе данных ATHRA.
    /// Поле 910.
    /// </summary>
    [XmlRoot("workplace")]
    public sealed class AthraWorkPlace
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "py";

        #endregion

        #region Properties

        /// <summary>
        /// Работает в данной организации.
        /// Подполе y.
        /// </summary>
        [SubField('y')]
        [XmlElement("here")]
        [JsonPropertyName("here")]
        public string? WorksHere { get; set; }

        /// <summary>
        /// Место работы автора.
        /// Подполе p.
        /// </summary>
        [SubField('p')]
        [XmlElement("place")]
        [JsonPropertyName("place")]
        public string? WorkPlace { get; set; }

        /// <summary>
        /// Associated <see cref="Field"/>.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Field? Field { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply to the <see cref="Field"/>.
        /// </summary>
        public Field ApplyTo
            (
                Field field
            )
        {
            field.ApplySubField('y', WorksHere)
                .ApplySubField('p', WorkPlace);

            return field;
        } // method ApplyTo

        /// <summary>
        /// Parse the field.
        /// </summary>
        public static AthraWorkPlace? Parse
            (
                Field? field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            // TODO: реализовать эффективно

            var result = new AthraWorkPlace
            {
                WorksHere = field.GetFirstSubFieldValue('y'),
                WorkPlace = field.GetFirstSubFieldValue('p'),
                Field = field
            };

            return result;
        } // method Parse

        /// <summary>
        /// Convert back to <see cref="Field"/>.
        /// </summary>
        public Field ToField() => new Field { Tag = 910 }
                .AddNonEmptySubField('p', WorkPlace)
                .AddNonEmptySubField('y', WorksHere);

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => WorkPlace.ToVisibleString();

        #endregion

    } // class AthraWorkPlace

} // namespace ManagedIrbis.Fieldss
