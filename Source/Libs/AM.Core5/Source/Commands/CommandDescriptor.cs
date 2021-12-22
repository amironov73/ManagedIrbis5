// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* CommandDescriptor.cs -- описатель команды
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Commands;

/// <summary>
/// Описатель команды (для сохранения/восстановления из файла).
/// </summary>
[XmlRoot ("command")]
public sealed class CommandDescriptor
{
    #region Properties

    /// <summary>
    /// Вызывается при выполнении команды.
    /// </summary>
    [XmlElement ("execute")]
    [JsonPropertyName ("execute")]
    [Description ("Вызывается при выполнении команды")]
    public DelegateDescriptor? Execute;

    /// <summary>
    /// Вызывается, когда команда должна обновить свое состояние.
    /// </summary>
    [XmlElement ("update")]
    [JsonPropertyName ("update")]
    [Description ("Вызывается, когда команда должна обновить свое состояние")]
    public DelegateDescriptor? Update;

    /// <summary>
    /// Вызывается, когда в команде что-то поменялось
    /// (например, состояние <see cref="Enabled"/>).
    /// </summary>
    [XmlElement ("changed")]
    [JsonPropertyName ("changed")]
    [Description ("Вызывается при изменениях в команде")]
    public DelegateDescriptor? Changed;

    /// <summary>
    /// Вызывается при очистке команды.
    /// </summary>
    [XmlElement ("disposed")]
    [JsonPropertyName ("disposed")]
    [Description ("Вызывается при очистке команды")]
    public DelegateDescriptor? Disposed;

    /// <summary>
    /// Команда разрешена к выполнению?
    /// </summary>
    [XmlAttribute ("enabled")]
    [JsonPropertyName ("enabled")]
    [Description ("Разрешена")]
    public bool Enabled { get; set; }

    /// <summary>
    /// Заглавие команды (произвольное).
    /// </summary>
    [XmlAttribute ("title")]
    [JsonPropertyName ("title")]
    [Description ("Заглавие команды")]
    public string? Title { get; set; }

    /// <summary>
    /// Описание команды в произвольной форме.
    /// </summary>
    [JsonPropertyName ("description")]
    [XmlAttribute ("description")]
    [Description ("Описание команды")]
    public string? Description { get; set; }

    #endregion
}
