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

/* EventCodes.cs -- коды мероприятия, поле 900
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
    /// Коды мероприятия. Поле 900.
    /// </summary>
    public sealed class EventCodes
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcfpz";

        /// <summary>
        /// Field tag.
        /// </summary>
        public const int Tag = 900;

        #endregion

        #region Properties

        /// <summary>
        /// Категория мероприятия. Подполе a.
        /// </summary>
        [SubField ('a')]
        [XmlAttribute ("category")]
        [JsonPropertyName ("category")]
        public string? Category { get; set; }

        /// <summary>
        /// Вид мероприятия. Подполе b.
        /// </summary>
        [SubField ('b')]
        [XmlAttribute ("kind")]
        [JsonPropertyName ("kind")]
        public string? Kind { get; set; }

        /// <summary>
        /// Характер мероприятия. Подполе c.
        /// </summary>
        [SubField ('c')]
        [XmlAttribute ("character")]
        [JsonPropertyName ("character")]
        public string? Character { get; set; }

        /// <summary>
        /// Степень доступности. Подполе p.
        /// </summary>
        [SubField ('p')]
        [XmlAttribute ("accessibility")]
        [JsonPropertyName ("accessibility")]
        public string? Accessibility { get; set; }

        /// <summary>
        /// Финансирование мероприятия. Подполе f.
        /// </summary>
        [SubField ('f')]
        [XmlAttribute ("financing")]
        [JsonPropertyName ("financing")]
        public string? Financing { get; set; }

        /// <summary>
        /// Возрастные ограничения. Подполе z.
        /// </summary>
        [SubField ('z')]
        [XmlAttribute ("age")]
        [JsonPropertyName ("age")]
        public string? AgeRestrictions { get; set; }

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
        public static EventCodes? Parse
            (
                Field? field
            )
        {
            if (field is null)
            {
                return null;
            }

            var result = new EventCodes
            {
                Category = field.GetFirstSubFieldValue ('a'),
                Kind = field.GetFirstSubFieldValue ('b'),
                Character = field.GetFirstSubFieldValue ('c'),
                Accessibility = field.GetFirstSubFieldValue ('p'),
                Financing = field.GetFirstSubFieldValue ('f'),
                AgeRestrictions = field.GetFirstSubFieldValue ('z'),
                Field = field
            };

            return result;

        } // method Parse

        #endregion

    } // method EventCodes

} // namespace ManagedIrbis.EventDb
