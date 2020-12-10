// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PostingParameters.cs -- параметры запроса постингов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Параметры запроса постингов.
    /// </summary>
    [XmlRoot("postings")]
    public sealed class PostingParameters
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
        /// Номер первого постинга, который необходимо вернуть.
        /// Нумерация с 1.
        /// По умолчанию 1.
        /// </summary>
        [XmlAttribute("first")]
        [JsonPropertyName("first")]
        public int FirstPosting { get; set; } = 1;

        /// <summary>
        /// Опциональный формат.
        /// </summary>
        [XmlAttribute("format")]
        [JsonPropertyName("format")]
        public string? Format { get; set; }

        /// <summary>
        /// Количество постингов, которые необходимо вернуть.
        /// По умолчанию 0 - все.
        /// </summary>
        [XmlAttribute("number")]
        [JsonPropertyName("number")]
        public int NumberOfPostings { get; set; }

        /// <summary>
        /// Массив терминов, для которых нужны постинги.
        /// </summary>
        [XmlAttribute("term")]
        [JsonPropertyName("terms")]
        public string[]? Terms { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the parameters.
        /// </summary>
        public PostingParameters Clone()
        {
            var result = (PostingParameters) MemberwiseClone();
            if (Terms is not null)
            {
                result.Terms = (string[]?) Terms.Clone();
            }

            return result;
        }

        /// <summary>
        /// Кодирование параметров постингов для клиентского запроса.
        /// </summary>
        /// <param name="connection">Ссылка на подключение к серверу.</param>
        /// <param name="query">Клиентский запрос.</param>
        public void Encode
            (
                Connection connection,
                IQuery query
            )
        {
            var database = (Database ?? connection.Database)
                .ThrowIfNull(nameof(Database));

            query.AddAnsi(database);
            query.Add(NumberOfPostings);
            query.Add(FirstPosting);
            query.AddFormat(Format);

            foreach (var term in Terms.ThrowIfNull())
            {
                query.AddUtf(term);
            }
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
            FirstPosting = reader.ReadPackedInt32();
            Format = reader.ReadNullableString();
            NumberOfPostings = reader.ReadPackedInt32();
            Terms = reader.ReadNullableStringArray();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Database)
                .WritePackedInt32(FirstPosting)
                .WriteNullable(Format)
                .WritePackedInt32(NumberOfPostings)
                .WriteNullableArray(Terms);
        }

        #endregion


        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<PostingParameters>
                (
                    this,
                    throwOnError
                );

            verifier
                .Assert(FirstPosting >= 0, "FirstPosting")
                .Assert(NumberOfPostings >= 0, "NumberOfPostings");

            verifier
                .Assert
                (
                    !Terms.IsNullOrEmpty(),
                    "Terms"
                );

            return verifier.Result;
        }

        #endregion

    }
}
