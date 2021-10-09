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

/* EventLocation.cs -- место проведения мероприятия, поле 210
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
    /// Место проведения мероприятия. Поле 210.
    /// </summary>
    public sealed class EventLocation
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "eghts";

        /// <summary>
        /// Field tag.
        /// </summary>
        public const int Tag = 210;

        #endregion

        #region Properties

        /// <summary>
        /// Место проведения. Подполе e.
        /// </summary>
        [SubField ('e')]
        [XmlAttribute ("location")]
        [JsonPropertyName ("location")]
        public string? Location { get; set; }

        /// <summary>
        /// Адрес. Подполе h.
        /// </summary>
        [SubField ('h')]
        [XmlAttribute ("address")]
        [JsonPropertyName ("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Телефон. Подполе t.
        /// </summary>
        [SubField ('t')]
        [XmlAttribute ("phone")]
        [JsonPropertyName ("phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// Страна. Подполе s.
        /// </summary>
        [SubField ('s')]
        [XmlAttribute ("country")]
        [JsonPropertyName ("country")]
        public string? Country { get; set; }

        /// <summary>
        /// Город. подполе g.
        /// </summary>
        [SubField ('g')]
        [XmlAttribute ("city")]
        [JsonPropertyName ("city")]
        public string? City { get; set; }

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
        /// Parse the field.
        /// </summary>
        public static EventLocation? Parse
            (
                Field? field
            )
        {
            if (field is null)
            {
                return null;
            }

            var result = new EventLocation
            {
                Location = field.GetFirstSubFieldValue ('e'),
                Address = field.GetFirstSubFieldValue ('h'),
                Phone = field.GetFirstSubFieldValue ('t'),
                Country = field.GetFirstSubFieldValue ('s'),
                City = field.GetFirstSubFieldValue ('g'),
                Field = field
            };

            return result;

        } // method Parse

        #endregion

    } // class EventLocation

} // namespace ManagedIrbis.EventDb
