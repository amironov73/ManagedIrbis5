// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Storehouse.cs -- программный интерфейс книговыдачи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Data;
using System.Data.Common;
using System.Globalization;

using AM;

//using LinqToDB;
//using LinqToDB.Data;
//using LinqToDB.DataProvider.SqlServer;

using ManagedIrbis;
using ManagedIrbis.Providers;

using Microsoft.Data.SqlClient;

#endregion

namespace Opac2025;

/// <summary>
/// Программный интерфейс книговыдачи.
/// </summary>
internal sealed class Storehouse
    : IDisposable
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Storehouse
        (
            IServiceProvider serviceProvider,
            IConfiguration configuration
        )
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = serviceProvider.GetRequiredService<ILogger<Storehouse>>();

        _logger.LogTrace (nameof (Storehouse) + "::Constructor");

        _kladovkaConnectionString = _configuration["kladovka"]
            .ThrowIfNullOrEmpty();

        _irbisConnectionString = _configuration["irbis-connection-string"]
            .ThrowIfNullOrEmpty();
    }

    #endregion

    #region Private members

    private readonly IServiceProvider _serviceProvider;
    private readonly string _irbisConnectionString;
    private readonly IConfiguration _configuration;
    private readonly string _kladovkaConnectionString;
    private readonly ILogger _logger;

    private DbConnection? _dataConnection;
    private SyncConnection? _irbisConnection;

    private SyncConnection GetIrbis() => _irbisConnection
        ?? GetIrbisConnection (_irbisConnectionString);

    private static SyncConnection GetIrbisConnection
        (
            string connectionString
        )
    {
        Sure.NotNullNorEmpty (connectionString);

        try
        {
            var result = new SyncConnection();
            result.ParseConnectionString (connectionString);
            result.Connect();

            return result;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (Storehouse) + "::" + nameof (GetMsSqlConnection)
                );
            throw;
        }
    }

    /// <summary>
    /// Подключается к MSSQL.
    /// </summary>
    private static DbConnection GetMsSqlConnection
        (
            string connectionString
        )
    {
        Sure.NotNullNorEmpty (connectionString);

        try
        {
            var connection = new SqlConnection (connectionString);
            connection.Open();

            return connection;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (Storehouse) + "::" + nameof (GetMsSqlConnection)
                );
            throw;
        }
    }

    /// <summary>
    /// Подключается к базе <c>kladovka</c>.
    /// </summary>
    private DbConnection GetKladovka() => _dataConnection
        ??= GetMsSqlConnection (_kladovkaConnectionString);

    // /// <summary>
    // /// Получает таблицу <c>orders</c>.
    // /// </summary>
    // private ITable<Order> GetOrders() => GetKladovka().GetTable<Order>();

    private static string ConvertStatus (string? status) => status switch
    {
        "0" => "ok",
        "C" or "U" => "u",
        _ => "bad"
    };

    private static string? GetDataString
        (
            DbDataReader reader,
            int count
        )
    {
        for (var i = 0; i < count; i++)
        {
            if (!reader.IsDBNull (i))
            {
                return reader.GetString (i);
            }
        }

        return null;
    }

    private Exemplar ConvertExemplar
        (
            Field field
        )
    {
        var result = new Exemplar
        {
            Status = ConvertStatus (field.GetFirstSubFieldValue ('a')),
            Number = field.GetFirstSubFieldValue ('b'),
            Sigla = field.GetFirstSubFieldValue ('d'),
            Amount = field.GetFirstSubFieldValue ('1').SafeToInt32()

        };

        if (result.Status == "ok"
            && int.TryParse (result.Number, NumberStyles.Integer,
                CultureInfo.InvariantCulture, out var number))
        {
            var kladovka = GetKladovka();
            var command = kladovka.CreateCommand();
            command.CommandText = "select [chb], [onhand] from [podsob] where [invent] = @number";
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@number";
            parameter.DbType = DbType.Int32;
            parameter.Direction = ParameterDirection.Input;
            parameter.Value = number;
            command.Parameters.Add (parameter);
            using var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                var onhand = GetDataString (reader, 2);
                if (!string.IsNullOrEmpty (onhand))
                {
                    result.OnHand = onhand;
                    result.Status = "onhand";
                }
            }

        }

        return result;
    }

    private Exemplar[] ConvertExemplars
        (
            Record record
        )
    {
        var result = new List<Exemplar>();
        foreach (var field in record.EnumerateField (910))
        {
            result.Add (ConvertExemplar (field));
        }

        if (result.Count (x => x.Amount != 0) > 1)
        {
            var siglas = result
                .Where (x => x.Status == "u")
                .Select (x => x.Sigla).Distinct().ToArray();
            foreach (var sigla in siglas)
            {
                var one = new Exemplar
                {
                    Status = "u",
                    Sigla = sigla,
                    Amount = result
                        .Where (x => x.Status == "u" && x.Sigla == sigla)
                        .Sum (x => x.Amount)
                };
                result.RemoveAll (x => x.Sigla == sigla);
                result.Add (one);
            }
        }

        return result.ToArray();
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        if (_dataConnection is not null)
        {
            _dataConnection.Dispose();
            _dataConnection = null;
        }

        if (_irbisConnection is not null)
        {
            _irbisConnection.Dispose();
            _irbisConnection = null;
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение списка баз данных.
    /// </summary>
    public Database[] ListDatabases()
    {
        var oldFormat = GetIrbis().ListDatabases();
        var result = new Database[oldFormat.Length];

        for (var i = 0; i < oldFormat.Length; i++)
        {
            result[i] = new ()
            {
                Name = oldFormat[i].Name,
                Description = oldFormat[i].Description
            };
        }

        return result;
    }

    /// <summary>
    /// Получение списка сценариев поиска для указанной базы данных.
    /// </summary>
    public Scenario[] ListScenarios
        (
            string database
        )
    {
        Sure.NotNullNorEmpty (database);

        return
        [
            new ()
            {
                Prefix = "A=",
                Description = "Автор"
            },
            new ()
            {
                Prefix = "T=",
                Description = "Заглавие"
            },
            new ()
            {
                Prefix = "K=",
                Description = "Ключевое слово"
            },
        ];
    }

    /// <summary>
    /// Поиск книг.
    /// </summary>
    public Book[] SearchBooks
        (
            string database,
            string expression
        )
    {
        var irbis = GetIrbis();

        var parameters = new SearchParameters
        {
            Database = database,
            Expression = expression,
            NumberOfRecords = 100,
            Format = "@brief"
        };
        var found = irbis.Search(parameters);
        if (found is null or [])
        {
            return [];
        }

        var result = new Book[found.Length];
        for (var i = 0; i < found.Length; i++)
        {
            result[i] = new ()
            {
                Description = found[i].Text
            };
        }

        var mfns = found.Select (one => one.Mfn).ToArray();
        var records = irbis.ReadRecords (database, mfns);
        if (records is null or [])
        {
            return [];
        }

        for (var i = 0; i < records.Length; i++)
        {
            result[i].Exemplars = ConvertExemplars (records[i]);
        }

        return result;
    }

    /// <summary>
    /// Получение списка всех заказов.
    /// </summary>
    public Order[] ListAllOrders() => [];

    /// <summary>
    /// Создание нового заказа.
    /// </summary>
    public void CreateOrder
        (
            Order order
        )
    {
        Sure.NotNull (order);

        // GetKladovka().Insert (order);
    }

    /// <summary>
    /// Удаление указанного заказа.
    /// </summary>
    public void DeleteOrder
        (
            int orderId
        )
    {
        Sure.NonNegative (orderId);

        // GetOrders().Delete (order => order.Id == orderId);
    }

    /// <summary>
    /// Внесение изменений в заказ (например, обновление статуса).
    /// </summary>
    public void ModifyOrder
        (
            Order order
        )
    {
        Sure.NotNull (order);

        // GetKladovka().Update (order);
    }

    #endregion
}
