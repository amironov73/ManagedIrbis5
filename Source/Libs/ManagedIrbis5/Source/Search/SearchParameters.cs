// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* SearchParameters.cs -- параметры поискового запроса
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
    /// Параметры поискового запроса.
    /// </summary>
    public sealed class SearchParameters
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
        /// Смещение первой записи, которую необходимо вернуть.
        /// Нумерация с 1.
        /// По умолчанию 1.
        /// </summary>
        [XmlAttribute("first")]
        [JsonPropertyName("first")]
        public int FirstRecord { get; set; } = 1;

        /// <summary>
        /// Опциональная спецификация формата.
        /// </summary>
        [XmlAttribute("format")]
        [JsonPropertyName("format")]
        public string? Format { get; set; }

        /// <summary>
        /// Максимальный MFN.
        /// По умолчанию 0 - без ограничения по MFN.
        /// </summary>
        [XmlAttribute("max")]
        [JsonPropertyName("max")]
        public int MaxMfn { get; set; }

        /// <summary>
        /// Минимальный MFN.
        /// По умолчанию 0 - без ограничения по MFN.
        /// </summary>
        [XmlAttribute("min")]
        [JsonPropertyName("min")]
        public int MinMfn { get; set; }

        /// <summary>
        /// Количество записей, которые необходимо вернуть.
        /// По умолчанию 0 - максимально возможное
        /// (ограничение текущей реализации MAX_PACKET).
        /// </summary>
        [XmlAttribute("records")]
        [JsonPropertyName("records")]
        public int NumberOfRecords { get; set; }

        /// <summary>
        /// Выражение для поиска по словарю.
        /// </summary>
        [XmlAttribute("expression")]
        [JsonPropertyName("expression")]
        public string? Expression { get; set; }

        /// <summary>
        /// Опциональное выражение для последовательного поиска.
        /// </summary>
        [XmlAttribute("sequential")]
        [JsonPropertyName("sequential.")]
        public string? Sequential { get; set; }

        /// <summary>
        /// Опциональная спецификация для фильтрации на клиенте.
        /// </summary>
        [XmlAttribute("filter")]
        [JsonPropertyName("filter")]
        public string? Filter { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование параметров поиска.
        /// </summary>
        public SearchParameters Clone()
        {
            return (SearchParameters) MemberwiseClone();
        }

        /// <summary>
        /// Кодирование параметров поиска для клиентского запроса.
        /// </summary>
        /// <param name="connection">Ссылка на подключение к серверу.</param>
        /// <param name="query">Клиентский запрос.</param>
        public void Encode
            (
                IIrbisConnection connection,
                IQuery query
            )
        {
            var database = (Database ?? connection.Database)
                .ThrowIfNull(nameof(Database));

            query.AddAnsi(database);
            query.AddUtf(Expression);
            query.Add(NumberOfRecords);
            query.Add(FirstRecord);
            query.AddFormat(Format);
            query.Add(MinMfn);
            query.Add(MaxMfn);
            query.AddAnsi(Sequential);
        } // method Encode

        /// <summary>
        /// Кодирование параметров поиска для клиентского запроса.
        /// </summary>
        /// <param name="connection">Ссылка на подключение к серверу.</param>
        /// <param name="query">Клиентский запрос.</param>
        public void Encode
            (
                IIrbisConnection connection,
                ref ValueQuery query
            )
        {
            var database = (Database ?? connection.Database)
                .ThrowIfNull(nameof(Database));

            query.AddAnsi(database);
            query.AddUtf(Expression);
            query.Add(NumberOfRecords);
            query.Add(FirstRecord);
            query.AddFormat(Format);
            query.Add(MinMfn);
            query.Add(MaxMfn);
            query.AddAnsi(Sequential);
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
            FirstRecord = reader.ReadPackedInt32();
            Format = reader.ReadNullableString();
            MaxMfn = reader.ReadPackedInt32();
            MinMfn = reader.ReadPackedInt32();
            NumberOfRecords = reader.ReadPackedInt32();
            Expression = reader.ReadNullableString();
            Sequential = reader.ReadNullableString();
            Filter = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull(writer, nameof(writer));

            writer
                .WriteNullable(Database)
                .WritePackedInt32(FirstRecord)
                .WriteNullable(Format)
                .WritePackedInt32(MaxMfn)
                .WritePackedInt32(MinMfn)
                .WritePackedInt32(NumberOfRecords)
                .WriteNullable(Expression)
                .WriteNullable(Sequential)
                .WriteNullable(Filter);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier
                = new Verifier<SearchParameters>(this, throwOnError);

            verifier.Assert
                (
                    !string.IsNullOrWhiteSpace(Expression)
                    || !string.IsNullOrWhiteSpace(Sequential),
                    "Expression and Sequential"
                );

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return (Expression ?? Sequential).ToVisibleString();
        }

        #endregion
    }
}
