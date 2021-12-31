// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* PoolUtility.cs -- работа с пулом подключений.
 * Ars Magna project, http://arsmagna.ru
 */

using ManagedIrbis.Infrastructure;

#nullable enable

namespace ManagedIrbis.Pooling;

/// <summary>
/// Работа с пулом подключений.
/// </summary>
public static class PoolUtility
{
    #region Public methods

    /// <summary>
    /// Чтение записи с помощью пула.
    /// </summary>
    public static Record? ReadRecord
        (
            this ConnectionPool pool,
            int mfn
        )
    {
        var client = pool.AcquireConnection();
        var parameters = new ReadRecordParameters()
        {
            Mfn = mfn
        };
        var result = client.ReadRecord<Record> (parameters);
        pool.ReleaseConnection (client);

        return result;
    }

    /// <summary>
    /// Поиск в каталоге с помощью пула.
    /// </summary>
    public static int[] Search
        (
            this ConnectionPool pool,
            string expression
        )
    {
        var client = pool.AcquireConnection();
        var parameters = new SearchParameters()
        {
            Expression = expression
        };
        var result = client.Search (parameters);
        pool.ReleaseConnection (client);

        return FoundItem.ToMfn (result);
    }

    /// <summary>
    /// Сохранение записей с помощью пула.
    /// </summary>
    public static void WriteRecord
        (
            this ConnectionPool pool,
            Record record
        )
    {
        var client = pool.AcquireConnection();
        var parameters = new WriteRecordParameters()
        {
            Record = record,
            Actualize = true,
            Lock = false
        };
        client.WriteRecord (parameters);
        pool.ReleaseConnection (client);
    }

    #endregion
}
