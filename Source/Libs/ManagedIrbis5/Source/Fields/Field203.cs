// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Field203.cs -- поддержка поля 203
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Linq;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Fields;

/// <summary>
/// В связи с ГОСТ 7.0.100-2018 введено 203 поле.
/// Это поле содержит подполя: вид содержания,
/// средства доступа, характеристика содержания.
/// </summary>
[XmlRoot ("field-203")]
public sealed class Field203
    : IHandmadeSerializable,
        IVerifiable
{
    #region Constants

    /// <summary>
    /// Метка поля.
    /// </summary>
    public const int Tag = 203;

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "12345678abcdefgikloptuvwxyz";

    #endregion

    #region Properties

    /// <summary>
    /// Вид содержания. A, B, D, E, F, G, I, K, L.
    /// </summary>
    [XmlAttribute ("content-type")]
    [JsonPropertyName ("contentType")]
    [Description ("Вид содержания")]
    [DisplayName ("Вид содержания")]
    public string[]? ContentType { get; set; }

    /// <summary>
    /// Средства доступа. Подполя C, 1, 2, 3, 4, 5, 6, 7, 8.
    /// </summary>
    [XmlAttribute ("access")]
    [JsonPropertyName ("access")]
    [Description ("Средства доступа")]
    [DisplayName ("Средства доступа")]
    public string[]? Access { get; set; }

    /// <summary>
    /// Характеристика содержания.
    /// Подполя O, P, U, Y, T, R, W, Q, X, V, Z.
    /// </summary>
    [XmlAttribute ("content-description")]
    [JsonPropertyName ("content-description")]
    [Description ("Характеристика содержания")]
    [DisplayName ("Характеристика содержания")]
    public string[]? ContentDescription { get; set; }

    /// <summary>
    /// Неизвестные подполя.
    /// </summary>
    [XmlElement ("unknown")]
    [JsonPropertyName ("unknown")]
    [Browsable (false)]
    public SubField[]? UnknownSubFields { get; set; }

    /// <summary>
    /// Ассоциированное поле библиографической записи.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public Field? Field { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public object? UserData { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Field203()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор со значениями.
    /// </summary>
    public Field203
        (
            string? contentType,
            string? access = "непосредственный",
            string? contentDescription = "визуальный"
        )
    {
        if (!string.IsNullOrEmpty (contentType))
        {
            ContentType = new[] { contentType };
        }

        if (!string.IsNullOrEmpty (access))
        {
            Access = new[] { access };
        }

        if (!string.IsNullOrEmpty (contentDescription))
        {
            ContentDescription = new[] { contentDescription };
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Применение данных к полю библиографической записи.
    /// </summary>
    public Field ApplyToField (Field field) => field
        .ThrowIfNull ()
        .SetSubFieldValue ("abdefgikl", ContentType)
        .SetSubFieldValue ("c12345678", Access)
        .SetSubFieldValue ("opuytrwqxvz", ContentDescription)
        .AddRange (UnknownSubFields);

    /// <summary>
    /// Разбор поля библиографической записи.
    /// </summary>
    public static Field203 ParseField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return new Field203
        {
            ContentType = Sequence.FromItems
                    (
                        field.GetFirstSubFieldValue ('a'),
                        field.GetFirstSubFieldValue ('b'),
                        field.GetFirstSubFieldValue ('d'),
                        field.GetFirstSubFieldValue ('e'),
                        field.GetFirstSubFieldValue ('f'),
                        field.GetFirstSubFieldValue ('g'),
                        field.GetFirstSubFieldValue ('i'),
                        field.GetFirstSubFieldValue ('k'),
                        field.GetFirstSubFieldValue ('l')
                    )
                .NonEmptyLines()
                .ToArray(),

            Access = Sequence.FromItems
                    (
                        field.GetFirstSubFieldValue ('c'),
                        field.GetFirstSubFieldValue ('1'),
                        field.GetFirstSubFieldValue ('2'),
                        field.GetFirstSubFieldValue ('3'),
                        field.GetFirstSubFieldValue ('4'),
                        field.GetFirstSubFieldValue ('5'),
                        field.GetFirstSubFieldValue ('6'),
                        field.GetFirstSubFieldValue ('7'),
                        field.GetFirstSubFieldValue ('8')
                    )
                .NonEmptyLines()
                .ToArray(),

            ContentDescription = Sequence.FromItems
                    (
                        field.GetFirstSubFieldValue ('o'),
                        field.GetFirstSubFieldValue ('p'),
                        field.GetFirstSubFieldValue ('u'),
                        field.GetFirstSubFieldValue ('y'),
                        field.GetFirstSubFieldValue ('t'),
                        field.GetFirstSubFieldValue ('r'),
                        field.GetFirstSubFieldValue ('w'),
                        field.GetFirstSubFieldValue ('q'),
                        field.GetFirstSubFieldValue ('x'),
                        field.GetFirstSubFieldValue ('v'),
                        field.GetFirstSubFieldValue ('z')
                    )
                .NonEmptyLines()
                .ToArray(),

            UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
            Field = field
        };
    }

    /// <summary>
    /// Разбор библиографической записи.
    /// </summary>
    public static Field203[] ParseRecord
        (
            Record record
        )
    {
        var result = new ValueList<Field203>();
        foreach (var field in record.Fields)
        {
            if (field.Tag is Tag)
            {
                var one = ParseField (field);
                result.Append (one);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Преобразование в поле библиографической записи.
    /// </summary>
    public Field ToField() => new Field (Tag)
        .AddRange ("abdefgikl", ContentType)
        .AddRange ("c12345678", Access)
        .AddRange ("opuytrwqxvz", ContentDescription)
        .AddRange (UnknownSubFields);

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        // TODO: check whether this code actually works
        ContentType = reader.ReadNullableStringArray();
        Access = reader.ReadNullableStringArray();
        ContentDescription = reader.ReadNullableStringArray();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullableArray (ContentType)
            .WriteNullableArray (Access)
            .WriteNullableArray (ContentDescription);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<Field203> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Utility.NonEmptyOrDefault (ContentType, string.Empty))
            .NotNullNorEmpty (Utility.NonEmptyOrDefault (ContentDescription, string.Empty));

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() =>
        Utility.NonEmptyOrDefault (ContentType) + ": " + Utility.NonEmptyOrDefault (Access);


    #endregion
}
