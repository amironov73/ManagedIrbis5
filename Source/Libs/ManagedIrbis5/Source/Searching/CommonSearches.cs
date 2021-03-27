// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* CommonSearches.cs -- наиболее распространенные поиски
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Наиболее распространенные поиски.
    /// </summary>
    public static class CommonSearches
    {
        #region Constants

        /// <summary>
        /// Ключевые слова.
        /// </summary>
        public const string KeywordPrefix = "K=";

        /// <summary>
        /// Индивидуальный автор, редактор, составитель.
        /// </summary>
        public const string AuthorPrefix = "A=";

        /// <summary>
        /// Коллектив или мероприятие.
        /// </summary>
        public const string CollectivePrefix = "M=";

        /// <summary>
        /// Заглавие.
        /// </summary>
        public const string TitlePrefix = "T=";

        /// <summary>
        /// Инвентарный номер, штрих-код или радиометка.
        /// </summary>
        public const string InventoryPrefix = "IN=";

        #endregion

        #region Public methods

        /// <summary>
        /// Поиск единственной записи, содержащей экземпляр с указанным номером
        /// (или штрих-кодом или радио-меткой).
        /// Запись может отсуствовать, это не будет считаться ошибкой.
        /// </summary>
        /// <returns></returns>
        public static Record? ByInventory
            (
                this ISyncIrbisProvider connection,
                string inventory
            )
        {
            // Sure.NotNull(connection, nameof(connection));
            // Sure.NotNullNorEmpty(inventory, nameof(inventory));

            return SingleOrDefault(connection, InventoryPrefix, inventory);
        }


        /// <summary>
        /// Поиск первой попавшейся записи, удовлетворяющей указанному условию.
        /// Запись может отсуствовать, это не будет считаться ошибкой.
        /// </summary>
        /// <returns>Найденную запись либо <c>null</c>.</returns>
        public static Record? FirstOrDefault
            (
                this ISyncIrbisProvider connection,
                string prefix,
                string value
            )
        {
            /*

            Record result = connection.SearchReadOneRecord("\"{0}{1}\"", prefix, value);

            return result;

            */

            return null;
        } // method FirstOrDefault

        /// <summary>
        /// Поиск единственной записи, удовлетворяющей указанному условию.
        /// Запись может отсуствовать, это не будет считаться ошибкой.
        /// </summary>
        /// <returns>Найденную запись либо <c>null</c>.</returns>
        /// <exception cref="IrbisException">Если найдено более одной записи.</exception>
        public static Record? SingleOrDefault
            (
                this ISyncIrbisProvider connection,
                string prefix,
                string value
            )
        {
            /*

            string expression = $"\"{prefix}{value}\"";
            SearchReadCommand command = new SearchReadCommand
            {
                Database = connection.Database,
                SearchExpression = expression,
                NumberOfRecords = 2
            };

            connection.ExecuteCommand(command);

            Record[] found = command.Records
                .ThrowIfNull(nameof(command.Records));
            if (found.Length > 1)
            {
                throw new IrbisException($"Too many records found: {expression}");
            }

            Record result = found.GetItem(0);

            return result;

            */

            return null;
        } // method SingleOrDefault

        /// <summary>
        /// Поиск единственной записи, удовлетворяющей данному условию.
        /// Запись должна существовать.
        /// </summary>
        /// <returns>Найденную запись.</returns>
        /// <exception cref="IrbisException">Найдено более одной записи,
        /// либо вообще ничего не найдено.</exception>
        public static Record Required
            (
                this ISyncIrbisProvider connection,
                string prefix,
                string value
            )
        {
            var result = SingleOrDefault(connection, prefix, value);
            if (ReferenceEquals(result, null))
            {
                throw new IrbisException($"Not found: {prefix}{value}");
            }

            return result;
        } // method Required

        #endregion

    } // class CommonSearches

} // namespace ManagedIrbis
