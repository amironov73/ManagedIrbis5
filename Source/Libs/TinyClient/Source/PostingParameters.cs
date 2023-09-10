// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* PostingParameters.cs -- параметры запроса постингов
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis
{
    /// <summary>
    /// Параметры запроса постингов.
    /// </summary>
    public sealed class PostingParameters
    {
        #region Properties

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// Номер первого постинга, который необходимо вернуть.
        /// Нумерация с 1.
        /// По умолчанию 1.
        /// </summary>
        public int FirstPosting { get; set; } = 1;

        /// <summary>
        /// Опциональный формат.
        /// </summary>
        public string? Format { get; set; }

        /// <summary>
        /// Количество постингов, которые необходимо вернуть.
        /// По умолчанию 0 - все.
        /// </summary>
        public int NumberOfPostings { get; set; }

        /// <summary>
        /// Массив терминов, для которых нужны постинги.
        /// </summary>
        public string[]? Terms { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование параметров <see cref="PostingParameters"/>.
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
                SyncConnection connection,
                SyncQuery query
            )
        {
            var database = connection.EnsureDatabase (Database);
            query.AddAnsi (database);
            query.Add (NumberOfPostings);
            query.Add (FirstPosting);
            query.AddFormat (Format);

            foreach (var term in Terms.ThrowIfNull())
            {
                query.AddUtf (term);
            }
        }

        #endregion
    }
}
