// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ReaderConfiguration.cs -- конфигурация базы данных читателей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Конфигурация базы данных читателей.
    /// </summary>
    [XmlRoot ("reader-configuration")]
    public sealed class ReaderConfiguration
    {
        #region Constants

        private const string IrbisDatabase = "ИРБИС64: база данных";

        #endregion

        #region Properties

        /// <summary>
        /// База данных читателей в ИРБИС. Как правило, <c>RDR</c>.
        /// </summary>
        [JsonPropertyName ("database")]
        [XmlAttribute ("database")]
        [Category (IrbisDatabase)]
        [DefaultValue ("RDR")]
        [DisplayName ("Имя базы данных")]
        [Description ("База данных читателей в ИРБИС. Как правило, RDR.")]
        public string Database { get; set; } = "RDR";

        /// <summary>
        /// Префикс инверсии в поисковом словаре для идентификатора читателя.
        /// Как правило, <c>RI=</c>.
        /// </summary>
        [JsonPropertyName ("prefix")]
        [XmlAttribute ("prefix")]
        [Category (IrbisDatabase)]
        [DefaultValue ("RI=")]
        [DisplayName ("Префикс для идентификатора читателя")]
        [Description ("Префикс инверсии в поисковом словаре для идентификатора читателя. Как правило, RI=")]
        public string Prefix { get; set; } = "RI=";

        /// <summary>
        /// Поле записи в БД RDR, используемое как идентификатор читателя.
        /// В дистрибутиве это поле 30.
        /// </summary>
        [JsonPropertyName ("readerID")]
        [XmlAttribute ("readerID")]
        [Category (IrbisDatabase)]
        [DefaultValue (30)]
        [DisplayName ("Поле с идентификатором читателя")]
        [Description ("Поле записи в БД RDR, используемое в качестве "
                      + "идентификатора читателя. Как правило, это поле 30.")]
        public int ReaderId { get; set; } = 30;

        /// <summary>
        /// Поле записи в БД RDR, используемое для хранения
        /// номера пропуска в библиотеку (например, RFID-метка).
        /// В дистрибутиве это поле 22.
        /// </summary>
        [JsonPropertyName ("ticket")]
        [XmlAttribute ("ticket")]
        [Category (IrbisDatabase)]
        [DefaultValue (22)]
        [DisplayName ("Поле с номером пропуска")]
        [Description ("Поле записи в БД RDR, используемое для "
                      + "хранения номера пропуска в библиотеку (например, "
                      + "RFID-метка). Как правило, это поле 22.")]
        public int Ticket { get; set; } = 22;

        #endregion

        #region Public methods

        /// <summary>
        /// Получение идентификатора читателя из библиографической записи.
        /// </summary>
        public string? GetReaderId (Record record) => record.FM (ReaderId);

        /// <summary>
        /// Получение идентификатора читателя из библиографической записи.
        /// </summary>
        public string? GetTicket (Record record) => record.FM (Ticket);

        #endregion
    }
}
