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

/* EventDates.cs -- даты мероприятия, поле 30
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.EventDb
{
    /// <summary>
    /// Даты мероприятия. Поле 30.
    /// </summary>
    public sealed class EventDates
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abc";

        /// <summary>
        /// Field tag.
        /// </summary>
        public const int Tag = 30;

        #endregion

        #region Properties

        /// <summary>
        /// Начальная дата. Подполе a.
        /// </summary>
        [SubField ('a')]
        [XmlAttribute ("from")]
        [JsonPropertyName ("from")]
        public IrbisDate? From { get; set; }

        /// <summary>
        /// Конечная дата. Подполе b.
        /// </summary>
        [SubField ('b')]
        [XmlAttribute ("till")]
        [JsonPropertyName ("till")]
        public IrbisDate? Till { get; set; }

        /// <summary>
        /// Исключения дат. Подполе c.
        /// </summary>
        [SubField ('c')]
        [XmlAttribute ("exclude")]
        [JsonPropertyName ("exclude")]
        public string? Exclude { get; set; }

        /// <summary>
        /// Associated <see cref="Field"/>.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
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
        /// Parse the field.
        /// </summary>
        public static EventDates? Parse
            (
                Field? field
            )
        {
            if (field is null)
            {
                return null;
            }

            var result = new EventDates
            {
                From = IrbisDate.ConvertStringToDate (field.GetFirstSubFieldValue ('a')),
                Till = IrbisDate.ConvertStringToDate (field.GetFirstSubFieldValue ('b')),
                Exclude = field.GetFirstSubFieldValue ('c'),
                Field = field
            };

            return result;
        }

        #endregion
    }
}
