// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PostingParameters.cs -- параметры запроса постингов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Collections;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Параметры запроса постингов.
    /// </summary>
    public sealed class PostingParameters
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// First posting to return.
        /// </summary>
        public int FirstPosting { get; set; } = 1;

        /// <summary>
        /// Format.
        /// </summary>
        public string? Format { get; set; }

        /// <summary>
        /// Number of postings to return.
        /// </summary>
        public int NumberOfPostings { get; set; }

        /// <summary>
        /// List of terms.
        /// </summary>
        public string[]? Terms { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Кодирование параметров постингов для клиентского запроса.
        /// </summary>
        /// <param name="connection">Ссылка на подключение к серверу.</param>
        /// <param name="query">Клиентский запрос.</param>
        public void Encode
            (
                Connection connection,
                Query query
            )
        {
            var database = (Database ?? connection.Database)
                .ThrowIfNull(nameof(Database));

            query.AddAnsi(database)
                .Add(NumberOfPostings)
                .Add(FirstPosting)
                .AddFormat(Format);

            foreach (var term in Terms.ThrowIfNull())
            {
                query.AddUtf(term);
            }
        }

        #endregion
    }
}
