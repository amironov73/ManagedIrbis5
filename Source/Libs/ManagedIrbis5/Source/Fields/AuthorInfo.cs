// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AuthorInfo.cs -- информация об индивидуальном авторе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Информация об индивидуальном авторе, поле 70x.
    /// </summary>
    public sealed class AuthorInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Empty array of the <see cref="AuthorInfo"/>.
        /// </summary>
        public static readonly AuthorInfo[] EmptyArray = new AuthorInfo[0];

        /// <summary>
        /// Known tags.
        /// </summary>
        public static int[] AllKnownTags { get { return _allKnownTags; } }

        /// <summary>
        /// Known tags.
        /// </summary>
        public static int[] KnownTags1 { get { return _knownTags1; } }

        /// <summary>
        /// Known tags.
        /// </summary>
        public static int[] KnownTags2 { get { return _knownTags2; } }

        /// <summary>
        /// Known tags.
        /// </summary>
        public static int[] KnownTags3 { get { return _knownTags3; } }

        /// <summary>
        /// Known tags.
        /// </summary>
        public static int[] KnownTags4 { get { return _knownTags4; } }

        /// <summary>
        /// Known tags.
        /// </summary>
        public static int[] KnownTags5 { get { return _knownTags5; } }

        /// <summary>
        /// Фамилия. Подполе a.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("familyName")]
        [JsonPropertyName("familyName")]
        [Description("Фамилия. Подполе a.")]
        [DisplayName("Фамилия (без инициалов)")]
        public string? FamilyName { get; set; }

        /// <summary>
        /// Инициалы (сокращение). Подполе b.
        /// </summary>
        [SubField('b')]
        [XmlAttribute("initials")]
        [JsonPropertyName("initials")]
        [Description("Инициалы (сокращение). Подполе b.")]
        [DisplayName("Инициалы (сокращенные)")]
        public string? Initials { get; set; }

        /// <summary>
        /// Расширение инициалов (имя и отчество). Подполе g.
        /// </summary>
        [SubField('g')]
        [XmlAttribute("fullName")]
        [JsonPropertyName("fullName")]
        [Description("Расширение инициалов (имя и отчество). Подполе g.")]
        [DisplayName("Расширение инициалов (имя и отчество)")]
        public string? FullName { get; set; }

        /// <summary>
        /// Инвертирование имени недопустимо? Подполе 9.
        /// </summary>
        [SubField('9')]
        [XmlAttribute("cantBeInverted")]
        [JsonPropertyName("cantBeInverted")]
        [Description("Инвертирование имени недопустимо? Подполе 9.")]
        [DisplayName("Инвертирование имени недопустимо")]
        public bool CantBeInverted { get; set; }

        /// <summary>
        /// Неотъемлемая часть имени (отец, сын, младший, старший
        /// и т. п.). Подполе 1.
        /// </summary>
        [SubField('1')]
        [XmlAttribute("postfix")]
        [JsonPropertyName("postfix")]
        [Description("Неотъемлемая часть имени. Подполе 1.")]
        [DisplayName("Неотъемлемая часть имени")]
        public string? Postfix { get; set; }

        /// <summary>
        /// Дополнения к имени кроме дат (род деятельности, звание,
        /// титул и т. д.). Подполе c.
        /// </summary>
        [SubField('c')]
        [XmlAttribute("appendix")]
        [JsonPropertyName("appendix")]
        [Description("Дополнения к имени кроме дат. Подполе c.")]
        [DisplayName("Дополнения к имени кроме дат")]
        public string? Appendix { get; set; }

        /// <summary>
        /// Династический номер (римские цифры). Подполе d.
        /// </summary>
        [SubField('d')]
        [XmlAttribute("number")]
        [JsonPropertyName("number")]
        [Description("Династический номер (римские цифры). Подполе d.")]
        [DisplayName("Династический номер (римские цифры)")]
        public string? Number { get; set; }

        /// <summary>
        /// Даты жизни. Подполе f.
        /// </summary>
        [SubField('f')]
        [XmlAttribute("dates")]
        [JsonPropertyName("dates")]
        [Description("Даты жизни. Подполе f.")]
        [DisplayName("Даты жизни")]
        public string? Dates { get; set; }

        /// <summary>
        /// Разночтение фамилии. Подполе r.
        /// </summary>
        [SubField('r')]
        [XmlAttribute("variant")]
        [JsonPropertyName("variant")]
        [Description("Разночтение фамилии. Подполе r.")]
        [DisplayName("Разночтение фамилии")]
        public string? Variant { get; set; }

        /// <summary>
        /// Место работы автора. Подполе p.
        /// </summary>
        [SubField('p')]
        [XmlAttribute("workplace")]
        [JsonPropertyName("workplace")]
        [Description("Место работы автора. Подполе p.")]
        [DisplayName("Место работы автора")]
        public string? WorkPlace { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        [Description("Поле с подполями")]
        [DisplayName("Поле с подполями")]
        public Field? Field { get; private set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        [Description("Пользовательские данные")]
        [DisplayName("Пользовательские данные")]
        public object? UserData { get; private set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        private static readonly int[] _allKnownTags =
        {
            330, 391, 454, 470, 481, 488, 600,
            700, 701, 702, 922, 925, 926, 961, 970
        };

        private static readonly int[] _knownTags1 =
            { 391, 470, 700, 701, 702, 926, 961, 970 };

        private static readonly int[] _knownTags2 =
            { 330, 922, 925 };

        private static readonly int[] _knownTags3 =
            { 481, 488 };

        private static readonly int[] _knownTags4 =
            { 600 };

        private static readonly int[] _knownTags5 =
            { 454 };

        private static readonly char[] _first330 =
            { 'f', '?', 'x', '=' };

        private static readonly char[] _second330 =
            { '2', ',', '<', '+' };

        private static readonly char[] _third330 =
            { '3', ';', '>', '-' };

        private static readonly char[] _first481 =
            { 'x', '?', '9', '=' };

        private static readonly char[] _first454 =
            { 'd', '\0', 'x', '\0' };

        private static readonly char[] _second454 =
            { 'e', '\0', '<', '\0' };

        private static readonly char[] _third454 =
            { 'f', '\0', '>', '\0' };

        private static readonly char[] _delimiters =
            { ' ', ',' };

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the <see cref="AuthorInfo"/>
        /// to the <see cref="Field"/>.
        /// </summary>
        public void ApplyTo
            (
                Field field
            )
        {

            int tag = field.Tag;
            if (tag.IsOneOf(KnownTags1))
            {
                ApplyToField700(field);
            }
            else if (tag.IsOneOf(KnownTags2))
            {
                ApplyToField330(field);
            }
            else if (tag.IsOneOf(KnownTags3))
            {
                ApplyToField481(field);
            }
            else if (tag.IsOneOf(KnownTags4))
            {
                ApplyToField600(field);
            }
            else if (tag.IsOneOf(KnownTags5))
            {
                ApplyToField454(field);
            }
            else
            {
                throw new IrbisException("Don't know to handle the field");
            }
        }

        /// <summary>
        /// Apply the <see cref="AuthorInfo"/>
        /// to the <see cref="Field"/>.
        /// </summary>
        public void ApplyToField700
            (
                Field field
            )
        {
            field
                .ApplySubField('a', FamilyName)
                .ApplySubField('b', Initials)
                .ApplySubField('g', FullName)
                .ApplySubField('9', CantBeInverted ? "1" : null)
                .ApplySubField('1', Postfix)
                .ApplySubField('c', Appendix)
                .ApplySubField('d', Number)
                .ApplySubField('f', Dates)
                .ApplySubField('r', Variant)
                .ApplySubField('p', WorkPlace);
        }

        /// <summary>
        /// Apply the <see cref="AuthorInfo"/>
        /// to the <see cref="Field"/>.
        /// </summary>
        public void ApplyToField600
            (
                Field field
            )
        {
            string withInitials = FamilyName;
            if (!string.IsNullOrEmpty(Initials))
            {
                withInitials = withInitials + " " + Initials;
            }

            field
                .ApplySubField('a', withInitials)
                .ApplySubField('g', FullName)
                .ApplySubField('9', CantBeInverted ? "1" : null)
                .ApplySubField('1', Postfix)
                .ApplySubField('c', Appendix)
                .ApplySubField('d', Number)
                .ApplySubField('f', Dates)
                .ApplySubField('r', Variant)
                .ApplySubField('p', WorkPlace);
        }

        /// <summary>
        /// Apply the <see cref="AuthorInfo"/>
        /// to the <see cref="Field"/>.
        /// </summary>
        public void ApplyToField330
            (
                Field field
            )
        {
            if (!ApplyOneAuthor(field, _first330))
            {
                if (!ApplyOneAuthor(field, _second330))
                {
                    ApplyOneAuthor(field, _third330);
                }
            }
        }

        /// <summary>
        /// Apply the <see cref="AuthorInfo"/>
        /// to the <see cref="Field"/>.
        /// </summary>
        public void ApplyToField454
            (
                Field field
            )
        {
            if (!ApplyOneAuthor(field, _first454))
            {
                if (!ApplyOneAuthor(field, _second454))
                {
                    ApplyOneAuthor(field, _third454);
                }
            }
        }

        /// <summary>
        /// Apply the <see cref="AuthorInfo"/>
        /// to the <see cref="Field"/>.
        /// </summary>
        public void ApplyToField481
            (
                Field field
            )
        {
            if (!ApplyOneAuthor(field, _first481))
            {
                if (!ApplyOneAuthor(field, _second330))
                {
                    ApplyOneAuthor(field, _third330);
                }
            }
        }

        /// <summary>
        /// Apply the info to one of authors.
        /// </summary>
        public bool ApplyOneAuthor
            (
                Field field,
                char[] subFields
            )
        {
            if (subFields.Length != 4)
            {
                throw new IrbisException();
            }

            var withInitials = field.GetFirstSubFieldValue(subFields[0]);
            if (string.IsNullOrEmpty(withInitials))
            {
                return false;
            }

            TextNavigator navigator = new TextNavigator(withInitials);
            var familyName = navigator.ReadUntil(_delimiters).ToString();
            if (!familyName.SameString(FamilyName))
            {
                return false;
            }

            withInitials = FamilyName;
            if (!string.IsNullOrEmpty(Initials))
            {
                withInitials = withInitials + " " + Initials;
            }

            field
                .ApplySubField(subFields[0], withInitials)
                .ApplySubField(subFields[1], FullName)
                .ApplySubField(subFields[2], CantBeInverted ? "1" : null)
                .ApplySubField(subFields[3], WorkPlace);

            return true;
        }

        /// <summary>
        /// Extract family name from the text.
        /// </summary>
        public static string? ExtractFamilyName
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            var navigator = new TextNavigator(text);
            var result = navigator.ReadUntil(_delimiters);

            return result.ToString();
        }

        /// <summary>
        /// Parse the <see cref="Record"/>.
        /// </summary>
        public static AuthorInfo[] ParseRecord
            (
                Record record,
                int[] tags
            )
        {
            var result = new List<AuthorInfo>();
            foreach (var field in record.Fields)
            {
                if (field.Tag.IsOneOf(tags))
                {
                    AuthorInfo[] authors = ParseField(field);
                    result.AddRange(authors);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        public static AuthorInfo[] ParseField
            (
                Field field
            )
        {
            AuthorInfo? one;
            int tag = field.Tag;
            if (tag.IsOneOf(KnownTags1))
            {
                one = ParseField700(field);
                if (!ReferenceEquals(one, null))
                {
                    return new[] { one };
                }

                return EmptyArray;
            }

            if (tag.IsOneOf(KnownTags2))
            {
                return ParseField330(field);
            }

            if (tag.IsOneOf(KnownTags3))
            {
                return ParseField481(field);
            }

            if (tag.IsOneOf(KnownTags4))
            {
                one = ParseField600(field);
                if (!ReferenceEquals(one, null))
                {
                    return new[] {one};
                }

                return EmptyArray;
            }

            if (tag.IsOneOf(KnownTags5))
            {
                return ParseField454(field);
            }

            throw new IrbisException("Don't know how to handle field");
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        public static AuthorInfo? ParseField700
            (
                Field field
            )
        {
            string familyName = field.GetFirstSubFieldValue('a');
            if (string.IsNullOrEmpty(familyName))
            {
                return null;
            }

            AuthorInfo result = new AuthorInfo
            {
                FamilyName = familyName,
                Initials = field.GetFirstSubFieldValue('b'),
                FullName = field.GetFirstSubFieldValue('g'),
                CantBeInverted = !string.IsNullOrEmpty
                    (
                        field.GetFirstSubFieldValue('9')
                    ),
                Postfix = field.GetFirstSubFieldValue('1'),
                Appendix = field.GetFirstSubFieldValue('c'),
                Number = field.GetFirstSubFieldValue('d'),
                Dates = field.GetFirstSubFieldValue('f'),
                Variant = field.GetFirstSubFieldValue('r'),
                WorkPlace = field.GetFirstSubFieldValue('p'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        public static AuthorInfo? ParseField600
            (
                Field field
            )
        {
            string withInitials = field.GetFirstSubFieldValue('a');
            if (string.IsNullOrEmpty(withInitials))
            {
                return null;
            }

            var navigator = new TextNavigator(withInitials);
            var familyName = navigator.ReadUntil(_delimiters);
            navigator.SkipChar(_delimiters);
            var initials = navigator.GetRemainingText();

            var result = new AuthorInfo
            {
                FamilyName = familyName.ToString(),
                Initials = initials.ToString(),
                FullName = field.GetFirstSubFieldValue('g'),
                CantBeInverted = !string.IsNullOrEmpty
                    (
                        field.GetFirstSubFieldValue('9')
                    ),
                Postfix = field.GetFirstSubFieldValue('1'),
                Appendix = field.GetFirstSubFieldValue('c'),
                Number = field.GetFirstSubFieldValue('d'),
                Dates = field.GetFirstSubFieldValue('f'),
                Variant = field.GetFirstSubFieldValue('r'),
                WorkPlace = field.GetFirstSubFieldValue('p'),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        public static AuthorInfo[] ParseField330
            (
                Field field
            )
        {
            // TODO parse other authors

            var result = new List<AuthorInfo>();

            var first = ParseOneAuthor(field, _first330);
            if (!ReferenceEquals(first, null))
            {
                result.Add(first);
            }

            var second = ParseOneAuthor(field, _second330);
            if (!ReferenceEquals(second, null))
            {
                result.Add(second);
            }

            var third = ParseOneAuthor(field, _third330);
            if (!ReferenceEquals(third, null))
            {
                result.Add(third);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse the specified field.
        /// </summary>
        public static AuthorInfo[] ParseField454
            (
                Field field
            )
        {
            // TODO parse other authors

            List<AuthorInfo> result = new List<AuthorInfo>();

            var first = ParseOneAuthor(field, _first454);
            if (!ReferenceEquals(first, null))
            {
                result.Add(first);
            }

            // Second author layout same as for 330
            var second = ParseOneAuthor(field, _second454);
            if (!ReferenceEquals(second, null))
            {
                result.Add(second);
            }

            // Third author layout same as for 330
            var third = ParseOneAuthor(field, _third454);
            if (!ReferenceEquals(third, null))
            {
                result.Add(third);
            }

            return result.ToArray();
        }


        /// <summary>
        /// Parse the specified field.
        /// </summary>
        public static AuthorInfo[] ParseField481
            (
                Field field
            )
        {
            // TODO parse other authors

            var result = new List<AuthorInfo>();

            var first = ParseOneAuthor(field, _first481);
            if (!ReferenceEquals(first, null))
            {
                result.Add(first);
            }

            // Second author layout same as for 330
            var second = ParseOneAuthor(field, _second330);
            if (!ReferenceEquals(second, null))
            {
                result.Add(second);
            }

            // Third author layout same as for 330
            var third = ParseOneAuthor(field, _third330);
            if (!ReferenceEquals(third, null))
            {
                result.Add(third);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Parse one of the authors.
        /// </summary>
        public static AuthorInfo? ParseOneAuthor
            (
                Field field,
                char[] subFields
            )
        {
            if (subFields.Length != 4)
            {
                throw new IrbisException();
            }

            string withInitials = field.GetFirstSubFieldValue(subFields[0]);
            if (string.IsNullOrEmpty(withInitials))
            {
                return null;
            }

            var result = new AuthorInfo();
            TextNavigator navigator = new TextNavigator(withInitials);
            result.CantBeInverted = !string.IsNullOrEmpty
                (
                    field.GetFirstSubFieldValue(subFields[2])
                );
            result.FamilyName = navigator.ReadUntil(_delimiters).ToString();
            navigator.SkipChar(_delimiters);
            result.Initials = navigator.GetRemainingText().ToString();
            result.FullName = field.GetFirstSubFieldValue(subFields[1]);
            result.WorkPlace = field.GetFirstSubFieldValue(subFields[3]);
            result.Field = field;

            return result;
        }

        /// <summary>
        /// Transform back to field.
        /// </summary>
        public Field ToField700
            (
                int tag
            )
        {
            var result = new Field { Tag = tag }
                .AddNonEmptySubField('a', FamilyName)
                .AddNonEmptySubField('b', Initials)
                .AddNonEmptySubField('g', FullName)
                .AddNonEmptySubField('9', CantBeInverted ? "1" : null)
                .AddNonEmptySubField('1', Postfix)
                .AddNonEmptySubField('c', Appendix)
                .AddNonEmptySubField('d', Number)
                .AddNonEmptySubField('f', Dates)
                .AddNonEmptySubField('r', Variant)
                .AddNonEmptySubField('p', WorkPlace);

            return result;
        }

        /// <summary>
        /// Полное ФИО автора (насколько возможно).
        /// </summary>
        /// <returns></returns>
        public string ToFullName()
        {
            string result = FamilyName;

            if (!string.IsNullOrEmpty(FullName))
            {
                result = result + ", " + FullName;
            }
            else if (!string.IsNullOrEmpty(Initials))
            {
                result = result + " " + Initials;
            }

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
            FamilyName = reader.ReadNullableString();
            Initials = reader.ReadNullableString();
            FullName = reader.ReadNullableString();
            Postfix = reader.ReadNullableString();
            Appendix = reader.ReadNullableString();
            Number = reader.ReadNullableString();
            Dates = reader.ReadNullableString();
            Variant = reader.ReadNullableString();
            WorkPlace = reader.ReadNullableString();
            CantBeInverted = reader.ReadBoolean();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(FamilyName)
                .WriteNullable(Initials)
                .WriteNullable(FullName)
                .WriteNullable(Postfix)
                .WriteNullable(Appendix)
                .WriteNullable(Number)
                .WriteNullable(Dates)
                .WriteNullable(Variant)
                .WriteNullable(WorkPlace)
                .Write(CantBeInverted);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<AuthorInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(FamilyName, "FamilyName");

            return verifier.Result;
        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => FamilyName.ToVisibleString();

        #endregion

    } // class AuthorInfo

} // namespace ManagedIrbis.Fields
