// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Storehouse.cs -- программный интерфейс книговыдачи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using AM;

using JetBrains.Annotations;

using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;

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
            IConfiguration configuration,
            string? connectionString = null
        )
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = serviceProvider.GetRequiredService<ILogger<Storehouse>>();

        _logger.LogTrace (nameof (Storehouse) + "::Constructor");

        _kladovkaConnectionString = (connectionString ?? _configuration["kladovka"])
            .ThrowIfNullOrEmpty();
    }

    #endregion

    #region Private members

    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly string _kladovkaConnectionString;
    private readonly ILogger _logger;

    private DataConnection? _dataConnection;

    /// <summary>
    /// Подключается к MSSQL.
    /// </summary>
    private static DataConnection GetMsSqlConnection
        (
            string connectionString
        )
    {
        Sure.NotNullNorEmpty (connectionString);

        try
        {
            var result = SqlServerTools.CreateDataConnection (connectionString);

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
    /// Подключается к базе <c>kladovka</c>.
    /// </summary>
    private DataConnection GetKladovka() => _dataConnection ??= GetMsSqlConnection (_kladovkaConnectionString);

    /// <summary>
    /// Получает таблицу <c>orders</c>.
    /// </summary>
    private ITable<Order> GetOrders() => GetKladovka().GetTable<Order>();

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
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение списка всех заказов.
    /// </summary>
    public Order[] ListAllOrders() => GetOrders().ToArray();

    /// <summary>
    /// Создание нового заказа.
    /// </summary>
    public void CreateOrder
        (
            Order order
        )
    {
        Sure.NotNull (order);

        GetKladovka().Insert (order);
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

        GetOrders().Delete (order => order.Id == orderId);
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

        GetKladovka().Update (order);
    }

    #endregion
}
