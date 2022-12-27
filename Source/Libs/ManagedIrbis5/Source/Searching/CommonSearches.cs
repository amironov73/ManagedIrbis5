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

using System.Threading.Tasks;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis;

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
    /// Шифр документа в базе.
    /// </summary>
    public const string IndexPrefix = "I=";

    /// <summary>
    /// Инвентарный номер, штрих-код или радиометка.
    /// </summary>
    public const string InventoryPrefix = "IN=";

    #endregion

    #region Public methods

    /// <summary>
    /// Поиск единственной записи с указанным шифром.
    /// Запись может отсуствовать, это не будет считаться ошибкой.
    /// </summary>
    public static Record? ByIndex
        (
            this ISyncProvider connection,
            string index
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (index);

        return SingleOrDefault (connection, IndexPrefix, index);
    }

    /// <summary>
    /// Поиск единственной записи с указанным шифром.
    /// Запись может отсуствовать, это не будет считаться ошибкой.
    /// </summary>
    public static Task<Record?> ByIndexAsync
        (
            this IAsyncProvider connection,
            string index
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (index);

        return SingleOrDefaultAsync (connection, IndexPrefix, index);
    }

    /// <summary>
    /// Поиск единственной записи, содержащей экземпляр с указанным номером
    /// (или штрих-кодом или радио-меткой).
    /// Запись может отсуствовать, это не будет считаться ошибкой.
    /// </summary>
    public static Record? ByInventory
        (
            this ISyncProvider connection,
            string inventory
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (inventory);

        return SingleOrDefault (connection, InventoryPrefix, inventory);
    }

    /// <summary>
    /// Поиск единственной записи, содержащей экземпляр с указанным номером
    /// (или штрих-кодом или радио-меткой).
    /// Запись может отсуствовать, это не будет считаться ошибкой.
    /// </summary>
    public static Task<Record?> ByInventoryAsync
        (
            this IAsyncProvider connection,
            string inventory
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (inventory);

        return SingleOrDefaultAsync (connection, InventoryPrefix, inventory);
    }

    /// <summary>
    /// Поиск первой попавшейся записи, удовлетворяющей указанному условию.
    /// Запись может отсуствовать, это не будет считаться ошибкой.
    /// </summary>
    /// <returns>Найденную запись либо <c>null</c>.</returns>
    public static Record? FirstOrDefault
        (
            this ISyncProvider connection,
            string prefix,
            string value
        )
    {
        Sure.NotNull (connection);

        var expression = $"\"{prefix}{value}\"";
        Record? result;
        if (connection is ISyncConnection syncConnection)
        {
            // протокол позволяет не только найти запись,
            // но и заодно прочитать её при помощи форматирования
            result = syncConnection.SearchReadOneRecord (expression);

            return result;
        }

        var searchParameters = new SearchParameters
        {
            Database = connection.EnsureDatabase(),
            Expression = expression,
            NumberOfRecords = 1
        };
        var found = connection.Search (searchParameters);
        if (found is null)
        {
            return null;
        }

        var readParameters = new ReadRecordParameters()
        {
            Database = connection.EnsureDatabase(),
            Mfn = found[0].Mfn
        };
        result = connection.ReadRecord<Record> (readParameters);

        return result;
    }

    /// <summary>
    /// Поиск единственной записи, удовлетворяющей указанному условию.
    /// Запись может отсуствовать, это не будет считаться ошибкой.
    /// </summary>
    /// <returns>Найденную запись либо <c>null</c>.</returns>
    /// <exception cref="IrbisException">Если найдено более одной записи.</exception>
    public static Record? SingleOrDefault
        (
            this ISyncProvider connection,
            string prefix,
            string value
        )
    {
        Sure.NotNull (connection);

        var expression = $"\"{prefix}{value}\"";
        Record? result;
        if (connection is ISyncConnection syncConnection)
        {
            // протокол позволяет не только найти запись,
            // но и заодно прочитать её при помощи форматирования
            result = syncConnection.SearchReadOneRecord (expression);

            return result;
        }

        var searchParameters = new SearchParameters
        {
            Database = connection.EnsureDatabase(),
            Expression = expression,
            NumberOfRecords = 2
        };
        var found = connection.Search (searchParameters);
        if (found is null || found.Length != 1)
        {
            return null;
        }

        var readParameters = new ReadRecordParameters()
        {
            Database = connection.EnsureDatabase(),
            Mfn = found[0].Mfn
        };
        result = connection.ReadRecord<Record> (readParameters);

        return result;
    }

    /// <summary>
    /// Поиск единственной записи, удовлетворяющей указанному условию.
    /// Запись может отсуствовать, это не будет считаться ошибкой.
    /// </summary>
    /// <returns>Найденную запись либо <c>null</c>.</returns>
    /// <exception cref="IrbisException">Если найдено более одной записи.</exception>
    public static async Task<Record?> SingleOrDefaultAsync
        (
            this IAsyncProvider connection,
            string prefix,
            string value
        )
    {
        Sure.NotNull (connection);

        var expression = $"\"{prefix}{value}\"";
        Record? result;
        if (connection is IAsyncConnection asyncConnection)
        {
            // протокол позволяет не только найти запись,
            // но и заодно прочитать её при помощи форматирования
            result = await asyncConnection.SearchReadOneRecordAsync (expression);

            return result;
        }

        var searchParameters = new SearchParameters
        {
            Database = connection.EnsureDatabase(),
            Expression = expression,
            NumberOfRecords = 2
        };
        var found = await connection.SearchAsync (searchParameters);
        if (found is null || found.Length != 1)
        {
            return null;
        }

        var readParameters = new ReadRecordParameters()
        {
            Database = connection.EnsureDatabase(),
            Mfn = found[0].Mfn
        };
        result = await connection.ReadRecordAsync<Record> (readParameters);

        return result;
    }

    /// <summary>
    /// Поиск единственной записи, удовлетворяющей данному условию.
    /// Запись должна существовать.
    /// </summary>
    /// <returns>Найденную запись.</returns>
    /// <exception cref="IrbisException">Найдено более одной записи,
    /// либо вообще ничего не найдено.</exception>
    public static Record Required
        (
            this ISyncProvider connection,
            string prefix,
            string value
        )
    {
        var result = SingleOrDefault (connection, prefix, value);
        if (result is null)
        {
            throw new IrbisException($"Not found: {prefix}{value}");
        }

        return result;
    }

    /// <summary>
    /// Поиск единственной записи, удовлетворяющей данному условию.
    /// Запись должна существовать.
    /// </summary>
    /// <returns>Найденную запись.</returns>
    /// <exception cref="IrbisException">Найдено более одной записи,
    /// либо вообще ничего не найдено.</exception>
    public static async Task<Record> RequiredAsync
        (
            this IAsyncProvider connection,
            string prefix,
            string value
        )
    {
        var result = await SingleOrDefaultAsync (connection, prefix, value);
        if (result is null)
        {
            throw new IrbisException($"Not found: {prefix}{value}");
        }

        return result;
    }

    #endregion
}
