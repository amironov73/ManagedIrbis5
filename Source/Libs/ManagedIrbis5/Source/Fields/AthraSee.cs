// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* AthraSee.cs --
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
    /// "СМ." в базе данных ATHRA.
    /// Поле 410.
    /// </summary>
    /// <remarks>
    /// Также "СМ. ТАКЖЕ" - поле 510.
    /// </remarks>
    [XmlRoot("see")]
    public sealed class AthraSee
    {
        #region Constants

        /// <summary>
        /// Known subfield codes.
        /// </summary>
        public const string KnownCodes = "0145789abcdfg<";

        #endregion

        #region Properties

        /// <summary>
        /// Начальный элемент ввода (фамилия или имя).
        /// Подполе a.
        /// </summary>
        [SubField('a')]
        [XmlElement("surname")]
        [JsonPropertyName("surname")]
        public string? Surname { get; set; }

        /// <summary>
        /// Инициалы.
        /// Подполе b.
        /// </summary>
        [SubField('b')]
        [XmlElement("initials")]
        [JsonPropertyName("intials")]
        public string? Initials { get; set; }

        /// <summary>
        /// Расширение инициалов.
        /// Подполе g.
        /// </summary>
        [SubField('g')]
        [XmlElement("extension")]
        [JsonPropertyName("extension")]
        public string? Extension { get; set; }

        /// <summary>
        /// Роль (инвертирование ФИО допустимо?).
        /// Подполе &lt;.
        /// </summary>
        [SubField('<')]
        [XmlElement("role")]
        [JsonPropertyName("role")]
        public string? Role { get; set; }

        /// <summary>
        /// Неотъемлемая часть имени (выводится в скобках).
        /// Подполе 1.
        /// </summary>
        [SubField('1')]
        [XmlElement("integral")]
        [JsonPropertyName("integral")]
        public string? IntegralPart { get; set; }

        /// <summary>
        /// Идентифицирующие признаки имени.
        /// Подполе c.
        /// </summary>
        [SubField('c')]
        [XmlElement("identifying")]
        [JsonPropertyName("identifying")]
        public string? IdentifyingSigns { get; set; }

        /// <summary>
        /// Римские цифры.
        /// Подполе d.
        /// </summary>
        [SubField('d')]
        [XmlElement("roman")]
        [JsonPropertyName("roman")]
        public string? RomanNumerals { get; set; }

        /// <summary>
        /// Даты.
        /// Подполе f.
        /// </summary>
        [SubField('f')]
        [XmlElement("dates")]
        [JsonPropertyName("dates")]
        public string? Dates { get; set; }

        /// <summary>
        /// Инструктивный текст.
        /// Подполе 0.
        /// </summary>
        [SubField('0')]
        [XmlElement("instruction")]
        [JsonPropertyName("instruction")]
        public string? Instruction { get; set; }

        /// <summary>
        /// Графика.
        /// Подполе 7.
        /// </summary>
        [SubField('7')]
        [XmlElement("graphics")]
        [JsonPropertyName("graphics")]
        public string? Graphics { get; set; }

        /// <summary>
        /// Язык заголовка.
        /// Подполе 8.
        /// </summary>
        [SubField('8')]
        [XmlElement("language")]
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        /// <summary>
        /// Код формирования ссылки.
        /// Подполе 5.
        /// </summary>
        [SubField('5')]
        [XmlElement("link")]
        [JsonPropertyName("link")]
        public string? Link { get; set; }

        /// <summary>
        /// Признак ввода имени лица.
        /// Подполе 9.
        /// </summary>
        [SubField('9')]
        [XmlElement("mark")]
        [JsonPropertyName("mark")]
        public string? Mark { get; set; }

        /// <summary>
        /// Код отношения.
        /// Подполе 4.
        /// </summary>
        [SubField('4')]
        [XmlElement("relation")]
        [JsonPropertyName("relation")]
        public string? RelationCode { get; set; }

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
            field.ApplySubField('a', Surname)
                .ApplySubField('b', Initials)
                .ApplySubField('g', Extension)
                .ApplySubField('<', Role)
                .ApplySubField('1', IntegralPart)
                .ApplySubField('c', IdentifyingSigns)
                .ApplySubField('d', RomanNumerals)
                .ApplySubField('f', Dates)
                .ApplySubField('0', Instruction)
                .ApplySubField('7', Graphics)
                .ApplySubField('8', Language)
                .ApplySubField('5', Link)
                .ApplySubField('9', Mark)
                .ApplySubField('4', RelationCode);

            return field;
        } // method ApplyTo

        /// <summary>
        /// Parse the field.
        /// </summary>
        public static AthraSee? Parse
            (
                Field? field
            )
        {
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            var result = new AthraSee
            {
                Surname = field.GetFirstSubFieldValue('a'),
                Initials = field.GetFirstSubFieldValue('b'),
                Extension = field.GetFirstSubFieldValue('g'),
                Role = field.GetFirstSubFieldValue('<'),
                IntegralPart = field.GetFirstSubFieldValue('1'),
                IdentifyingSigns = field.GetFirstSubFieldValue('c'),
                RomanNumerals = field.GetFirstSubFieldValue('d'),
                Dates = field.GetFirstSubFieldValue('f'),
                Instruction = field.GetFirstSubFieldValue('0'),
                Graphics = field.GetFirstSubFieldValue('7'),
                Language = field.GetFirstSubFieldValue('8'),
                Link = field.GetFirstSubFieldValue('5'),
                Mark = field.GetFirstSubFieldValue('9'),
                RelationCode = field.GetFirstSubFieldValue('4'),
                Field = field
            };

            return result;
        } // method Parse

        /// <summary>
        /// Convert back to <see cref="Field"/>.
        /// </summary>
        public Field ToField() => new Field { Tag = 410 }
                .AddNonEmptySubField('a', Surname)
                .AddNonEmptySubField('b', Initials)
                .AddNonEmptySubField('g', Extension)
                .AddNonEmptySubField('<', Role)
                .AddNonEmptySubField('1', IntegralPart)
                .AddNonEmptySubField('c', IdentifyingSigns)
                .AddNonEmptySubField('d', RomanNumerals)
                .AddNonEmptySubField('f', Dates)
                .AddNonEmptySubField('0', Instruction)
                .AddNonEmptySubField('7', Graphics)
                .AddNonEmptySubField('8', Language)
                .AddNonEmptySubField('5', Link)
                .AddNonEmptySubField('9', Mark)
                .AddNonEmptySubField('4', RelationCode);

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Surname.ToVisibleString();

        #endregion

    } // class AthraSee

} // namespace ManagedIrbis.Fields
