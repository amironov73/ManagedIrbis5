// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EventTimes.cs -- время мероприятия, поле 31
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;
using System.Xml.Serialization;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.EventDb;

/// <summary>
/// Время мероприятия. Поле 31.
/// </summary>
public sealed class EventTimes
{
    #region Constants

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "ab";

    /// <summary>
    /// Метка поля.
    /// </summary>
    public const int Tag = 31;

    #endregion

    #region Properties

    /// <summary>
    /// Время проведения от... Подполе a.
    /// </summary>
    [SubField ('a')]
    [XmlAttribute ("from")]
    [JsonPropertyName ("from")]
    public string? From { get; set; }

    /// <summary>
    /// Время проведения до ... Подполе b.
    /// </summary>
    [SubField ('b')]
    [XmlAttribute ("till")]
    [JsonPropertyName ("till")]
    public string? Till { get; set; }

    /// <summary>
    /// Ассоциированное поле библиографической записи <see cref="Field"/>.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public Field? Field { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public object? UserData { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор поля библиографической записи на элементы.
    /// </summary>
    public static EventTimes? Parse
        (
            Field? field
        )
    {
        if (field is null)
        {
            return null;
        }

        var result = new EventTimes
        {
            From = field.GetFirstSubFieldValue ('a'),
            Till = field.GetFirstSubFieldValue ('b'),
            Field = field
        };

        return result;
    }

    #endregion
}
