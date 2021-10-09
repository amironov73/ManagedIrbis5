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

/* EventStatus.cs -- статус мероприятя, поле 997
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.EventDb
{
    /// <summary>
    /// Статус мероприятия. Поле 997.
    /// </summary>
    public sealed class EventStatus
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abc";

        /// <summary>
        /// Field tag.
        /// </summary>
        public const int Tag = 997;

        #endregion

        #region Properties

        /// <summary>
        /// Статус мероприятия. Подполе a.
        /// </summary>
        [SubField ('a')]
        [XmlAttribute ("status")]
        [JsonPropertyName ("status")]
        public string? Status { get; set; }

        /// <summary>
        /// Текст. Подполе b.
        /// </summary>
        [SubField ('b')]
        [XmlAttribute ("text")]
        [JsonPropertyName ("text")]
        public string? Text { get; set; }

        /// <summary>
        /// Первоначальная дата (для перенесенных мероприятий). Подполе c.
        /// </summary>
        [SubField ('c')]
        [XmlAttribute ("initial-date")]
        [JsonPropertyName ("initialDate")]
        public IrbisDate? InitialDate { get; set; }

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
        public static EventStatus? Parse
            (
                Field? field
            )
        {
            if (field is null)
            {
                return null;
            }

            var result = new EventStatus
            {
                Status = field.GetFirstSubFieldValue ('a'),
                Text = field.GetFirstSubFieldValue ('b'),
                InitialDate = IrbisDate.ConvertStringToDate (field.GetFirstSubFieldValue ('c')),
                Field = field
            };

            return result;

        } // method Parse

        #endregion

    } // class EventStatus

} // namespace ManagedIrbis.EventDb
