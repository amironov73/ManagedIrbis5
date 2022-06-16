// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* EventLocation.cs -- место проведения мероприятия, поле 210
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
/// Место проведения мероприятия. Поле 210.
/// </summary>
public sealed class EventLocation
{
    #region Constants

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "eghts";

    /// <summary>
    /// Метка поля.
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
    [DisplayName ("Место проведения")]
    public string? Location { get; set; }

    /// <summary>
    /// Адрес. Подполе h.
    /// </summary>
    [SubField ('h')]
    [XmlAttribute ("address")]
    [JsonPropertyName ("address")]
    [DisplayName ("Адрес")]
    public string? Address { get; set; }

    /// <summary>
    /// Телефон. Подполе t.
    /// </summary>
    [SubField ('t')]
    [XmlAttribute ("phone")]
    [JsonPropertyName ("phone")]
    [DisplayName ("Телефон")]
    public string? Phone { get; set; }

    /// <summary>
    /// Страна. Подполе s.
    /// </summary>
    [SubField ('s')]
    [XmlAttribute ("country")]
    [JsonPropertyName ("country")]
    [DisplayName ("Страна")]
    public string? Country { get; set; }

    /// <summary>
    /// Город. подполе g.
    /// </summary>
    [SubField ('g')]
    [XmlAttribute ("city")]
    [JsonPropertyName ("city")]
    [DisplayName ("Город")]
    public string? City { get; set; }

    /// <summary>
    /// Associated <see cref="Field"/>.
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
    }

    #endregion
}
