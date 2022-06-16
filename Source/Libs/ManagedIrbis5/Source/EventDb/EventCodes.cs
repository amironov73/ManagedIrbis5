// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* EventCodes.cs -- коды мероприятия, поле 900
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.EventDb;

/// <summary>
/// Коды мероприятия. Поле 900.
/// </summary>
public sealed class EventCodes
{
    #region Constants

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "abcfpz";

    /// <summary>
    /// Метка поля.
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
    [DisplayName ("Категория")]
    public string? Category { get; set; }

    /// <summary>
    /// Вид мероприятия. Подполе b.
    /// </summary>
    [SubField ('b')]
    [XmlAttribute ("kind")]
    [JsonPropertyName ("kind")]
    [DisplayName ("Вид")]
    public string? Kind { get; set; }

    /// <summary>
    /// Характер мероприятия. Подполе c.
    /// </summary>
    [SubField ('c')]
    [XmlAttribute ("character")]
    [JsonPropertyName ("character")]
    [DisplayName ("Характер")]
    public string? Character { get; set; }

    /// <summary>
    /// Степень доступности. Подполе p.
    /// </summary>
    [SubField ('p')]
    [XmlAttribute ("accessibility")]
    [JsonPropertyName ("accessibility")]
    [DisplayName ("Доступность")]
    public string? Accessibility { get; set; }

    /// <summary>
    /// Финансирование мероприятия. Подполе f.
    /// </summary>
    [SubField ('f')]
    [XmlAttribute ("financing")]
    [JsonPropertyName ("financing")]
    [DisplayName ("Финансирование")]
    public string? Financing { get; set; }

    /// <summary>
    /// Возрастные ограничения. Подполе z.
    /// </summary>
    [SubField ('z')]
    [XmlAttribute ("age")]
    [JsonPropertyName ("age")]
    [DisplayName ("Возрастные ограничения")]
    public string? AgeRestrictions { get; set; }

    /// <summary>
    /// Ассоциированное поле <see cref="Field"/>.
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

    #region Public methods

    /// <summary>
    /// Разбор поля библиографической записи <see cref="Field"/>.
    /// </summary>
    public static EventCodes? ParseField
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
    }

    #endregion
}
