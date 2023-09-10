// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* SearchParameters.cs -- параметры поискового запроса
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis
{
    /// <summary>
    /// Параметры поискового запроса.
    /// </summary>
    public sealed class SearchParameters
    {
        #region Properties

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// Смещение первой записи, которую необходимо вернуть.
        /// Нумерация с 1.
        /// По умолчанию 1.
        /// </summary>
        public int FirstRecord { get; set; } = 1;

        /// <summary>
        /// Опциональная спецификация формата.
        /// </summary>
        public string? Format { get; set; }

        /// <summary>
        /// Максимальный MFN.
        /// По умолчанию 0 - без ограничения по MFN.
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Минимальный MFN.
        /// По умолчанию 0 - без ограничения по MFN.
        /// </summary>
        public int MinMfn { get; set; }

        /// <summary>
        /// Количество записей, которые необходимо вернуть.
        /// По умолчанию 0 - максимально возможное
        /// (ограничение текущей реализации MAX_PACKET).
        /// </summary>
        public int NumberOfRecords { get; set; }

        /// <summary>
        /// Выражение для поиска по словарю.
        /// </summary>
        public string? Expression { get; set; }

        /// <summary>
        /// Опциональное выражение для последовательного поиска.
        /// </summary>
        public string? Sequential { get; set; }

        /// <summary>
        /// Опциональная спецификация для фильтрации на клиенте.
        /// </summary>
        public string? Filter { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование параметров поиска.
        /// </summary>
        public SearchParameters Clone()
        {
            return (SearchParameters)MemberwiseClone();
        }

        /// <summary>
        /// Кодирование параметров поиска для клиентского запроса.
        /// </summary>
        /// <param name="connection">Ссылка на подключение к серверу.</param>
        /// <param name="query">Клиентский запрос.</param>
        public void Encode
            (
                SyncConnection connection,
                SyncQuery query
            )
        {
            var database = Database.ThrowIfNull (nameof (Database));

            query.AddAnsi (database);
            query.AddUtf (Expression);
            query.Add (NumberOfRecords);
            query.Add (FirstRecord);
            query.AddFormat (Format);
            query.Add (MinMfn);
            query.Add (MaxMfn);
            query.AddAnsi (Sequential);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() =>
            (Expression ?? Sequential).ToVisibleString();

        #endregion
    }
}
