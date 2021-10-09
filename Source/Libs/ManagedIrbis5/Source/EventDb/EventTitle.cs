// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* EventTitle.cs -- название мероприятия
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.EventDb
{
    /// <summary>
    /// Название мероприятия.
    /// </summary>
    public sealed class EventTitle
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "0abr";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const int Tag = 972;

        #endregion

        #region Properties

        /// <summary>
        /// Название мероприятия. Подполе a.
        /// </summary>
        [SubField ('a')]
        [XmlAttribute ("title")]
        [JsonPropertyName ("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Подзаголовок. Подполе b.
        /// </summary>
        [SubField ('b')]
        [XmlAttribute ("subtitle")]
        [JsonPropertyName ("subtitle")]
        public string? Subtitle { get; set; }

        /// <summary>
        /// Аббревиатура названия. Подполе r.
        /// </summary>
        [SubField ('r')]
        [XmlAttribute ("abbreviation")]
        [JsonPropertyName ("abbreviation")]
        public string? Abbreviation { get; set; }

        /// <summary>
        /// Номер пункта мероприятия в плане.
        /// </summary>
        [SubField ('0')]
        [XmlAttribute ("number")]
        [JsonPropertyName ("number")]
        public string? Number { get; set; }

        /// <summary>
        /// Associated <see cref="Field"/>.
        /// </summary>
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
        /// Parse the field.
        /// </summary>
        public static EventTitle? Parse
            (
                Field? field
            )
        {
            if (field is null)
            {
                return null;
            }

            var result = new EventTitle
            {
                Title = field.GetFirstSubFieldValue ('a'),
                Subtitle = field.GetFirstSubFieldValue ('b'),
                Abbreviation = field.GetFirstSubFieldValue ('r'),
                Number = field.GetFirstSubFieldValue ('0'),
                Field = field
            };

            return result;

        } // method Parse

        #endregion

    } // class EventTitle

} // namespace ManagedIrbis.EventDb
