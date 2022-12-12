// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

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

namespace ManagedIrbis.Fields;

/// <summary>
/// Информация об индивидуальном авторе, поле 70x.
/// </summary>
public sealed class AuthorInfo
    : IHandmadeSerializable,
        IVerifiable
{
    #region Properties

    /// <summary>
    /// Известные метки полей (вообще все).
    /// </summary>
    public static int[] AllKnownTags { get; } =
    {
        330, 391, 454, 470, 481, 488, 600,
        700, 701, 702, 922, 925, 926, 961, 970
    };

    /// <summary>
    /// Первый набор известных меток полей.
    /// </summary>
    public static int[] KnownTags1 { get; } = { 391, 470, 700, 701, 702, 926, 961, 970 };

    /// <summary>
    /// Второй набор известных меток полей.
    /// </summary>
    public static int[] KnownTags2 { get; } = { 330, 922, 925 };

    /// <summary>
    /// Третий набор известных меток полей.
    /// </summary>
    public static int[] KnownTags3 { get; } = { 481, 488 };

    /// <summary>
    /// Четвертый набор известных меток полей.
    /// </summary>
    public static int[] KnownTags4 { get; } = { 600 };

    /// <summary>
    /// Пятый набор известных меток полей.
    /// </summary>
    public static int[] KnownTags5 { get; } = { 454 };

    /// <summary>
    /// Фамилия. Подполе a.
    /// </summary>
    [SubField ('a')]
    [XmlAttribute ("familyName")]
    [JsonPropertyName ("familyName")]
    [Description ("Фамилия. Подполе a.")]
    [DisplayName ("Фамилия (без инициалов)")]
    public string? FamilyName { get; set; }

    /// <summary>
    /// Инициалы (сокращение). Подполе b.
    /// </summary>
    [SubField ('b')]
    [XmlAttribute ("initials")]
    [JsonPropertyName ("initials")]
    [Description ("Инициалы (сокращение). Подполе b.")]
    [DisplayName ("Инициалы (сокращенные)")]
    public string? Initials { get; set; }

    /// <summary>
    /// Расширение инициалов (имя и отчество). Подполе g.
    /// </summary>
    [SubField ('g')]
    [XmlAttribute ("fullName")]
    [JsonPropertyName ("fullName")]
    [Description ("Расширение инициалов (имя и отчество). Подполе g.")]
    [DisplayName ("Расширение инициалов (имя и отчество)")]
    public string? FullName { get; set; }

    /// <summary>
    /// Инвертирование имени недопустимо? Подполе 9.
    /// </summary>
    [SubField ('9')]
    [XmlAttribute ("cantBeInverted")]
    [JsonPropertyName ("cantBeInverted")]
    [Description ("Инвертирование имени недопустимо? Подполе 9.")]
    [DisplayName ("Инвертирование имени недопустимо")]
    public bool CantBeInverted { get; set; }

    /// <summary>
    /// Неотъемлемая часть имени (отец, сын, младший, старший
    /// и т. п.). Подполе 1.
    /// </summary>
    [SubField ('1')]
    [XmlAttribute ("postfix")]
    [JsonPropertyName ("postfix")]
    [Description ("Неотъемлемая часть имени. Подполе 1.")]
    [DisplayName ("Неотъемлемая часть имени")]
    public string? Postfix { get; set; }

    /// <summary>
    /// Дополнения к имени кроме дат (род деятельности, звание,
    /// титул и т. д.). Подполе c.
    /// </summary>
    [SubField ('c')]
    [XmlAttribute ("appendix")]
    [JsonPropertyName ("appendix")]
    [Description ("Дополнения к имени кроме дат. Подполе c.")]
    [DisplayName ("Дополнения к имени кроме дат")]
    public string? Appendix { get; set; }

    /// <summary>
    /// Династический номер (римские цифры). Подполе d.
    /// </summary>
    [SubField ('d')]
    [XmlAttribute ("number")]
    [JsonPropertyName ("number")]
    [Description ("Династический номер (римские цифры). Подполе d.")]
    [DisplayName ("Династический номер (римские цифры)")]
    public string? Number { get; set; }

    /// <summary>
    /// Даты жизни. Подполе f.
    /// </summary>
    [SubField ('f')]
    [XmlAttribute ("dates")]
    [JsonPropertyName ("dates")]
    [Description ("Даты жизни. Подполе f.")]
    [DisplayName ("Даты жизни")]
    public string? Dates { get; set; }

    /// <summary>
    /// Разночтение фамилии. Подполе r.
    /// </summary>
    [SubField ('r')]
    [XmlAttribute ("variant")]
    [JsonPropertyName ("variant")]
    [Description ("Разночтение фамилии. Подполе r.")]
    [DisplayName ("Разночтение фамилии")]
    public string? Variant { get; set; }

    /// <summary>
    /// Место работы автора. Подполе p.
    /// </summary>
    [SubField ('p')]
    [XmlAttribute ("workplace")]
    [JsonPropertyName ("workplace")]
    [Description ("Место работы автора. Подполе p.")]
    [DisplayName ("Место работы автора")]
    public string? WorkPlace { get; set; }

    /// <summary>
    /// Ассоциированное поле библиографической записи.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    [Description ("Поле с подполями")]
    [DisplayName ("Поле с подполями")]
    public Field? Field { get; private set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    [Description ("Пользовательские данные")]
    [DisplayName ("Пользовательские данные")]
    public object? UserData { get; set; }

    #endregion

    #region Construction

    #endregion

    #region Private members

    private static readonly char[] _first330 = { 'f', '?', 'x', '=' };
    private static readonly char[] _second330 = { '2', ',', '<', '+' };
    private static readonly char[] _third330 = { '3', ';', '>', '-' };
    private static readonly char[] _first481 = { 'x', '?', '9', '=' };
    private static readonly char[] _first454 = { 'd', '\0', 'x', '\0' };
    private static readonly char[] _second454 = { 'e', '\0', '<', '\0' };
    private static readonly char[] _third454 = { 'f', '\0', '>', '\0' };
    private static readonly char[] _delimiters = { ' ', ',' };

    #endregion

    #region Public methods

    /// <summary>
    /// Применение данных класса <see cref="AuthorInfo"/>
    /// к полю библиографической записи <see cref="Field"/>.
    /// </summary>
    public void ApplyTo
        (
            Field field
        )
    {
        Sure.NotNull (field);

        var tag = field.Tag;
        if (tag.IsOneOf (KnownTags1))
        {
            ApplyToField700 (field);
        }
        else if (tag.IsOneOf (KnownTags2))
        {
            ApplyToField330 (field);
        }
        else if (tag.IsOneOf (KnownTags3))
        {
            ApplyToField481 (field);
        }
        else if (tag.IsOneOf (KnownTags4))
        {
            ApplyToField600 (field);
        }
        else if (tag.IsOneOf (KnownTags5))
        {
            ApplyToField454 (field);
        }
        else
        {
            throw new IrbisException ("Don't know to handle the field");
        }
    }

    /// <summary>
    /// Применение данных из класса <see cref="AuthorInfo"/>
    /// к полю библиографической записи <see cref="Field"/>.
    /// </summary>
    public Field ApplyToField700
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return field
            .SetSubFieldValue ('a', FamilyName)
            .SetSubFieldValue ('b', Initials)
            .SetSubFieldValue ('g', FullName)
            .SetSubFieldValue ('9', CantBeInverted, "1")
            .SetSubFieldValue ('1', Postfix)
            .SetSubFieldValue ('c', Appendix)
            .SetSubFieldValue ('d', Number)
            .SetSubFieldValue ('f', Dates)
            .SetSubFieldValue ('r', Variant)
            .SetSubFieldValue ('p', WorkPlace);
    }

    /// <summary>
    /// Применение данных класса <see cref="AuthorInfo"/>
    /// к полю библиографической записи <see cref="Field"/>.
    /// </summary>
    public void ApplyToField600
        (
            Field field
        )
    {
        Sure.NotNull (field);

        var withInitials = FamilyName;
        if (!string.IsNullOrEmpty (Initials))
        {
            withInitials = withInitials + " " + Initials;
        }

        field
            .SetSubFieldValue ('a', withInitials)
            .SetSubFieldValue ('g', FullName)
            .SetSubFieldValue ('9', CantBeInverted ? "1" : null)
            .SetSubFieldValue ('1', Postfix)
            .SetSubFieldValue ('c', Appendix)
            .SetSubFieldValue ('d', Number)
            .SetSubFieldValue ('f', Dates)
            .SetSubFieldValue ('r', Variant)
            .SetSubFieldValue ('p', WorkPlace);
    }

    /// <summary>
    /// Применение данных класса <see cref="AuthorInfo"/>
    /// к полю библиографической записи <see cref="Field"/>.
    /// </summary>
    public void ApplyToField330
        (
            Field field
        )
    {
        Sure.NotNull (field);

        if (!ApplyOneAuthor (field, _first330)
            && !ApplyOneAuthor (field, _second330))
        {
            ApplyOneAuthor (field, _third330);
        }
    }

    /// <summary>
    /// Применение данных класса <see cref="AuthorInfo"/>
    /// к полю библиографической записи <see cref="Field"/>.
    /// </summary>
    public void ApplyToField454
        (
            Field field
        )
    {
        Sure.NotNull (field);

        if (!ApplyOneAuthor (field, _first454)
            && !ApplyOneAuthor (field, _second454))
        {
            ApplyOneAuthor (field, _third454);
        }
    }

    /// <summary>
    /// Применение данных класса <see cref="AuthorInfo"/>
    /// к полю библиографической записи <see cref="Field"/>.
    /// </summary>
    public void ApplyToField481
        (
            Field field
        )
    {
        Sure.NotNull (field);

        if (!ApplyOneAuthor (field, _first481)
            && !ApplyOneAuthor (field, _second330))
        {
            ApplyOneAuthor (field, _third330);
        }
    }

    /// <summary>
    /// Применение данных класса <see cref="AuthorInfo"/>
    /// к одному полю библиографической записи в соответствии
    /// с заданной раскладкой по подполям.
    /// </summary>
    public bool ApplyOneAuthor
        (
            Field field,
            char[] subFields
        )
    {
        Sure.NotNull (field);
        Sure.NotNull (subFields);

        if (subFields.Length != 4)
        {
            throw new IrbisException();
        }

        var withInitials = field.GetFirstSubFieldValue (subFields[0]);
        if (withInitials.IsEmpty())
        {
            return false;
        }

        var navigator = new TextNavigator (withInitials);
        var familyName = navigator.ReadUntil (_delimiters).ToString();
        if (!familyName.SameString (FamilyName))
        {
            return false;
        }

        withInitials = FamilyName;
        if (!string.IsNullOrEmpty (Initials))
        {
            withInitials = withInitials + " " + Initials;
        }

        field
            .SetSubFieldValue (subFields[0], withInitials)
            .SetSubFieldValue (subFields[1], FullName)
            .SetSubFieldValue (subFields[2], CantBeInverted, "1")
            .SetSubFieldValue (subFields[3], WorkPlace);

        return true;
    }

    /// <summary>
    /// Извлечение фамилии из текста.
    /// </summary>
    public static string? ExtractFamilyName
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return null;
        }

        var navigator = new TextNavigator (text);
        var result = navigator.ReadUntil (_delimiters);

        return result.ToString();
    }

    /// <summary>
    /// Разбор сведений об авторах, хранящихся в библиографической
    /// записи <see cref="Record"/>.
    /// </summary>
    public static AuthorInfo[] ParseRecord
        (
            Record record,
            int[] tags
        )
    {
        Sure.NotNull (record);
        Sure.NotNull (tags);

        var result = new List<AuthorInfo>();
        foreach (var field in record.Fields)
        {
            if (field.Tag.IsOneOf (tags))
            {
                var authors = ParseField (field);
                result.AddRange (authors);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Разбор сведений об авторе, хранящихся в поле библиографической записи.
    /// </summary>
    public static AuthorInfo[] ParseField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        AuthorInfo? one;
        var tag = field.Tag;
        if (tag.IsOneOf (KnownTags1))
        {
            one = ParseField700 (field);
            if (!ReferenceEquals (one, null))
            {
                return new[] { one };
            }

            return Array.Empty<AuthorInfo>();
        }

        if (tag.IsOneOf (KnownTags2))
        {
            return ParseField330 (field);
        }

        if (tag.IsOneOf (KnownTags3))
        {
            return ParseField481 (field);
        }

        if (tag.IsOneOf (KnownTags4))
        {
            one = ParseField600 (field);
            if (!ReferenceEquals (one, null))
            {
                return new[] { one };
            }

            return Array.Empty<AuthorInfo>();
        }

        if (tag.IsOneOf (KnownTags5))
        {
            return ParseField454 (field);
        }

        throw new IrbisException ("Don't know how to handle field");
    }

    /// <summary>
    /// Разбор сведений об авторе, хранящихся в поле библиографической записи.
    /// </summary>
    public static AuthorInfo? ParseField700
        (
            Field field
        )
    {
        Sure.NotNull (field);

        var familyName = field.GetFirstSubFieldValue ('a');
        if (familyName.IsEmpty())
        {
            return null;
        }

        // TODO: реализовать эффективно

        var result = new AuthorInfo
        {
            FamilyName = familyName,
            Initials = field.GetFirstSubFieldValue ('b'),
            FullName = field.GetFirstSubFieldValue ('g'),
            CantBeInverted = !field.GetFirstSubFieldValue ('9').IsEmpty(),
            Postfix = field.GetFirstSubFieldValue ('1'),
            Appendix = field.GetFirstSubFieldValue ('c'),
            Number = field.GetFirstSubFieldValue ('d'),
            Dates = field.GetFirstSubFieldValue ('f'),
            Variant = field.GetFirstSubFieldValue ('r'),
            WorkPlace = field.GetFirstSubFieldValue ('p'),
            Field = field
        };

        return result;
    }

    /// <summary>
    /// Разбор сведений об авторе, хранящихся в поле библиографической записи.
    /// </summary>
    public static AuthorInfo? ParseField600
        (
            Field field
        )
    {
        Sure.NotNull (field);

        var withInitials = field.GetFirstSubFieldValue ('a');
        if (withInitials.IsEmpty())
        {
            return null;
        }

        var navigator = new TextNavigator (withInitials);
        var familyName = navigator.ReadUntil (_delimiters);
        navigator.SkipChar (_delimiters);
        var initials = navigator.GetRemainingText();

        var result = new AuthorInfo
        {
            FamilyName = familyName.ToString(),
            Initials = initials.ToString(),
            FullName = field.GetFirstSubFieldValue ('g'),
            CantBeInverted = !field.GetFirstSubFieldValue ('9').IsEmpty(),
            Postfix = field.GetFirstSubFieldValue ('1'),
            Appendix = field.GetFirstSubFieldValue ('c'),
            Number = field.GetFirstSubFieldValue ('d'),
            Dates = field.GetFirstSubFieldValue ('f'),
            Variant = field.GetFirstSubFieldValue ('r'),
            WorkPlace = field.GetFirstSubFieldValue ('p'),
            Field = field
        };

        return result;
    }

    /// <summary>
    /// Разбор сведений об авторе, хранящихся в поле библиографической записи.
    /// </summary>
    public static AuthorInfo[] ParseField330
        (
            Field field
        )
    {
        Sure.NotNull (field);

        // TODO parse other authors

        var result = new List<AuthorInfo>();

        var first = ParseOneAuthor (field, _first330);
        if (!ReferenceEquals (first, null))
        {
            result.Add (first);
        }

        var second = ParseOneAuthor (field, _second330);
        if (!ReferenceEquals (second, null))
        {
            result.Add (second);
        }

        var third = ParseOneAuthor (field, _third330);
        if (!ReferenceEquals (third, null))
        {
            result.Add (third);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Разбор сведений об авторе, хранящихся в поле библиографической записи.
    /// </summary>
    public static AuthorInfo[] ParseField454
        (
            Field field
        )
    {
        Sure.NotNull (field);

        // TODO parse other authors

        var result = new List<AuthorInfo>();

        var first = ParseOneAuthor (field, _first454);
        if (!ReferenceEquals (first, null))
        {
            result.Add (first);
        }

        // Second author layout same as for 330
        var second = ParseOneAuthor (field, _second454);
        if (!ReferenceEquals (second, null))
        {
            result.Add (second);
        }

        // Third author layout same as for 330
        var third = ParseOneAuthor (field, _third454);
        if (!ReferenceEquals (third, null))
        {
            result.Add (third);
        }

        return result.ToArray();
    }


    /// <summary>
    /// Разбор сведений об авторе, хранящихся в заданном
    /// поле библиографической записи.
    /// </summary>
    public static AuthorInfo[] ParseField481
        (
            Field field
        )
    {
        Sure.NotNull (field);

        // TODO parse other authors

        var result = new List<AuthorInfo>();

        var first = ParseOneAuthor (field, _first481);
        if (!ReferenceEquals (first, null))
        {
            result.Add (first);
        }

        // Second author layout same as for 330
        var second = ParseOneAuthor (field, _second330);
        if (!ReferenceEquals (second, null))
        {
            result.Add (second);
        }

        // Third author layout same as for 330
        var third = ParseOneAuthor (field, _third330);
        if (!ReferenceEquals (third, null))
        {
            result.Add (third);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Разбор сведений об авторе, хранящихся в поле библиографической записи
    /// в соответствии с раскладкой по подполям.
    /// </summary>
    public static AuthorInfo? ParseOneAuthor
        (
            Field field,
            char[] subFields
        )
    {
        Sure.NotNull (field);
        Sure.NotNull (subFields);

        if (subFields.Length != 4)
        {
            throw new IrbisException();
        }

        var withInitials = field.GetFirstSubFieldValue (subFields[0]);
        if (withInitials.IsEmpty())
        {
            return null;
        }

        var result = new AuthorInfo();
        var navigator = new TextNavigator (withInitials);
        result.CantBeInverted = !field.GetFirstSubFieldValue (subFields[2]).IsEmpty();
        result.FamilyName = navigator.ReadUntil (_delimiters).ToString();
        navigator.SkipChar (_delimiters);
        result.Initials = navigator.GetRemainingText().ToString();
        result.FullName = field.GetFirstSubFieldValue (subFields[1]);
        result.WorkPlace = field.GetFirstSubFieldValue (subFields[3]);
        result.Field = field;

        return result;
    }

    /// <summary>
    /// Преобразоване данных об авторе в поле 70x или аналогичное.
    /// </summary>
    public Field ToField700
        (
            int tag
        )
    {
        Sure.Positive (tag);

        var result = new Field { Tag = tag }
            .AddNonEmpty ('a', FamilyName)
            .AddNonEmpty ('b', Initials)
            .AddNonEmpty ('g', FullName)
            .AddNonEmpty ('9', CantBeInverted ? "1" : null)
            .AddNonEmpty ('1', Postfix)
            .AddNonEmpty ('c', Appendix)
            .AddNonEmpty ('d', Number)
            .AddNonEmpty ('f', Dates)
            .AddNonEmpty ('r', Variant)
            .AddNonEmpty ('p', WorkPlace);

        return result;
    }

    /// <summary>
    /// Полное ФИО автора (насколько возможно).
    /// </summary>
    /// <returns></returns>
    public string? ToFullName()
    {
        var result = FamilyName;

        if (!string.IsNullOrEmpty (FullName))
        {
            result = result + ", " + FullName;
        }
        else if (!string.IsNullOrEmpty (Initials))
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
        Sure.NotNull (reader);

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
        Sure.NotNull (writer);

        writer
            .WriteNullable (FamilyName)
            .WriteNullable (Initials)
            .WriteNullable (FullName)
            .WriteNullable (Postfix)
            .WriteNullable (Appendix)
            .WriteNullable (Number)
            .WriteNullable (Dates)
            .WriteNullable (Variant)
            .WriteNullable (WorkPlace)
            .Write (CantBeInverted);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<AuthorInfo> (this, throwOnError);

        verifier
            .NotNullNorEmpty (FamilyName);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() => FamilyName.ToVisibleString();

    #endregion
}
