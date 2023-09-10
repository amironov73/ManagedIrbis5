// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TermParameters.cs -- параметры запроса терминов
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis
{
    /// <summary>
    /// Параметры запроса терминов.
    /// </summary>
    public sealed class TermParameters
    {
        #region Properties

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// Количество терминов, которое необходимо вернуть.
        /// По умолчанию 0 - максимально возможное.
        /// Ограничение текущей реализации MAX_PACKET.
        /// </summary>
        public int NumberOfTerms { get; set; }

        /// <summary>
        /// Термины в обратном порядке?
        /// </summary>
        public bool ReverseOrder { get; set; }

        /// <summary>
        /// Стартовый термин.
        /// </summary>
        public string? StartTerm { get; set; }

        /// <summary>
        /// Опциональная спецификация формата.
        /// </summary>
        public string? Format { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование параметров.
        /// </summary>
        public TermParameters Clone()
        {
            return (TermParameters)MemberwiseClone();
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
            var database = Database.ThrowIfNull (nameof (Database));

            query.AddAnsi (database);
            query.AddUtf (StartTerm);
            query.Add (NumberOfTerms);
            query.AddFormat (Format);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => StartTerm.ToVisibleString();

        #endregion
    }
}
