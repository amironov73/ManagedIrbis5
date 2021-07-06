// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TermParameters.cs -- параметры запроса терминов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Параметры запроса терминов.
    /// </summary>
    [XmlRoot("terms")]
    public sealed class TermParameters
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        [XmlAttribute("database")]
        [JsonPropertyName("database")]
        public string? Database { get; set; }

        /// <summary>
        /// Количество терминов, которое необходимо вернуть.
        /// По умолчанию 0 - максимально возможное.
        /// Ограничение текущей реализации MAX_PACKET.
        /// </summary>
        [XmlAttribute("number")]
        [JsonPropertyName("number")]
        public int NumberOfTerms { get; set; }

        /// <summary>
        /// Термины в обратном порядке?
        /// </summary>
        [XmlAttribute("reverse")]
        [JsonPropertyName("reverse")]
        public bool ReverseOrder { get; set; }

        /// <summary>
        /// Стартовый термин.
        /// </summary>
        [XmlAttribute("start")]
        [JsonPropertyName("start")]
        public string? StartTerm { get; set; }

        /// <summary>
        /// Опциональная спецификация формата.
        /// </summary>
        [XmlAttribute("format")]
        [JsonPropertyName("format")]
        public string? Format { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование параметров.
        /// </summary>
        public TermParameters Clone()
        {
            return (TermParameters) MemberwiseClone();
        }

        /// <summary>
        /// Кодирование параметров постингов для клиентского запроса.
        /// </summary>
        /// <param name="connection">Ссылка на подключение к серверу.</param>
        /// <param name="query">Клиентский запрос.</param>
        public void Encode<TQuery>
            (
                IConnectionSettings connection,
                TQuery query
            )
            where TQuery: IQuery
        {
            var database = Database.ThrowIfNull(nameof(Database));

            query.AddAnsi(database);
            query.AddUtf(StartTerm);
            query.Add(NumberOfTerms);
            query.AddFormat(Format);

        } // method Encode

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Database = reader.ReadNullableString();
            NumberOfTerms = reader.ReadPackedInt32();
            StartTerm = reader.ReadNullableString();
            Format = reader.ReadNullableString();
            ReverseOrder = reader.ReadBoolean();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Database)
                .WritePackedInt32(NumberOfTerms)
                .WriteNullable(StartTerm)
                .WriteNullable(Format)
                .Write(ReverseOrder);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<TermParameters>
                (
                    this,
                    throwOnError
                );

            /* Тут что-то надо делать? */

            return verifier.Result;
        }

        #endregion


        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => StartTerm.ToVisibleString();

        #endregion

    } // class TermParameters

} // namespace ManagedIrbis
