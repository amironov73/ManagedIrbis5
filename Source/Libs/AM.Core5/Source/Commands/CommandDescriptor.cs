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

using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Commands
{
    /// <summary>
    /// Описатель команды (для сохранения/восстановления из файла).
    /// </summary>
    [XmlRoot("command")]
    public sealed class CommandDescriptor
    {
        #region Properties

        /// <summary>
        /// Вызывается при выполнении команды.
        /// </summary>
        [JsonPropertyName("execute")]
        [XmlElement("execute")]
        public DelegateDescriptor? Execute;

        /// <summary>
        /// Вызывается, когда команда должна обновить свое состояние.
        /// </summary>
        [JsonPropertyName("update")]
        [XmlElement("update")]
        public DelegateDescriptor? Update;

        /// <summary>
        /// Вызывается, когда в команде что-то поменялось
        /// (например, состояние <see cref="Enabled"/>).
        /// </summary>
        [JsonPropertyName("changed")]
        [XmlElement("changed")]
        public DelegateDescriptor? Changed;

        /// <summary>
        /// Вызывается при очистке команды.
        /// </summary>
        [JsonPropertyName("disposed")]
        [XmlElement("disposed")]
        public DelegateDescriptor? Disposed;

        /// <summary>
        /// Команда разрешена к выполнению?
        /// </summary>
        [JsonPropertyName("enabled")]
        [XmlAttribute("enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Заглавие команды (произвольное).
        /// </summary>
        [JsonPropertyName("title")]
        [XmlAttribute("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Описание команды в произвольной форме.
        /// </summary>
        [JsonPropertyName("description")]
        [XmlAttribute("description")]
        public string? Description { get; set; }

        #endregion

    } // class CommandDescriptor

} // namespace AM.Commands
