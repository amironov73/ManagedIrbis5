// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SearchParameters.cs -- параметры поискового запроса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Параметры поискового запроса.
    /// </summary>
    public sealed class SearchParameters
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// First record offset.
        /// </summary>
        public int FirstRecord { get; set; } = 1;

        /// <summary>
        /// Format specification.
        /// </summary>
        public string? Format { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        public int MinMfn { get; set; }

        /// <summary>
        /// Number of records.
        /// </summary>
        public int NumberOfRecords { get; set; }

        /// <summary>
        /// Search query expression.
        /// </summary>
        public string? Expression { get; set; }

        /// <summary>
        /// Specification of sequential search.
        /// </summary>
        public string? Sequential { get; set; }

        /// <summary>
        /// Specification for local filter.
        /// </summary>
        public string? Filter { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Кодирование параметров поиска для клиентского запроса.
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
                .AddUtf(Expression)
                .Add(NumberOfRecords)
                .Add(FirstRecord)
                .AddFormat(Format)
                .Add(MinMfn)
                .Add(MaxMfn)
                .AddAnsi(Sequential);
        }

        #endregion
    }
}
