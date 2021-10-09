// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* AccessRight.cs -- право доступа к ресурсу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Drm
{
    /// <summary>
    /// Право доступа к ресурсу. Поле 3.
    /// </summary>
    public class AccessRight
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "abcdefg";

        #endregion

        #region Properties

        /// <summary>
        /// Элемент доступа. Подполе a.
        /// </summary>
        /// <remarks>
        /// Типичное значение: "02".
        /// </remarks>
        [SubField ('a')]
        [JsonPropertyName ("elementKind")]
        public string? ElementKind { get; set; }

        /// <summary>
        /// Значение элемента доступа. Подполе b.
        /// </summary>
        /// <remarks>
        /// Типичное значние: "В01".
        /// </remarks>
        [SubField ('b')]
        [JsonPropertyName ("elementValue")]
        public string? ElementValue { get; set; }

        /// <summary>
        /// Значение права доступа. Подполе c.
        /// </summary>
        /// <remarks>
        /// Типичное значение: "2".
        /// </remarks>
        [SubField ('c')]
        [JsonPropertyName ("accessKind")]
        public string? AccessKind { get; set; }

        /// <summary>
        /// Количественное ограничение. Подполе f.
        /// </summary>
        [SubField ('f')]
        [JsonPropertyName ("limitValue")]
        public int LimitValue { get; set; }

        /// <summary>
        /// Единицы ограничения. Подполе g.
        /// </summary>
        [SubField ('g')]
        [JsonPropertyName ("limitKind")]
        public string? LimitKind { get; set; }

        /// <summary>
        /// Начальная дата периода доступа. Подполе d.
        /// </summary>
        [SubField ('d')]
        [XmlAttribute ("from")]
        [JsonPropertyName ("from")]
        public IrbisDate? FromDate { get; set; }

        /// <summary>
        /// Конечная дата периода доступа. Подполе e.
        /// </summary>
        [SubField ('e')]
        [XmlAttribute ("till")]
        [JsonPropertyName ("till")]
        public IrbisDate? TillDate { get; set; }

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
        public static AccessRight Parse
            (
                Field field
            )
        {
            Sure.NotNull (field, nameof(field));

            var result = new AccessRight
            {
                ElementKind = field.GetFirstSubFieldValue ('a'),
                ElementValue = field.GetFirstSubFieldValue ('b'),
                AccessKind = field.GetFirstSubFieldValue ('c'),
                LimitValue = field.GetFirstSubFieldValue ('f').SafeToInt32(),
                LimitKind = field.GetFirstSubFieldValue ('g'),
                FromDate = IrbisDate.ConvertStringToDate (field.GetFirstSubFieldValue ('d')),
                TillDate = IrbisDate.ConvertStringToDate (field.GetFirstSubFieldValue ('e')),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse the record.
        /// </summary>
        public static AccessRight[] Parse
            (
                Record record
            )
        {
            Sure.NotNull (record, nameof (record));

            return record.Fields
                .GetField(3)
                .Select(field => Parse (field))
                .ToArray();
        }

        #endregion

    } // class AccessRight

} // namespace ManagedIrbis.Drm
