// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* OrderManager.cs -- менеджер заказов для для книговыдачи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;

using Istu.OldModel.Interfaces;

using LinqToDB;
using LinqToDB.Data;

#endregion

#nullable enable

namespace Istu.OldModel.Implementation;

/// <summary>
/// Менеджер заказов для книговыдачи.
/// </summary>
public sealed class OrderManager
    : IOrderManager
{
    #region Properties

    /// <summary>
    /// База данных книговыдачи.
    /// </summary>
    public Storehouse Storehouse { get; }

    /// <summary>
    /// Таблица с заказами.
    /// </summary>
    public ITable<Order> Orders => _GetDb().GetOrders();

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public OrderManager
        (
            Storehouse storehouse
        )
    {
        Storehouse = storehouse;
    }

    #endregion

    #region Private members

    private DataConnection? _dataConnection;

    private DataConnection _GetDb() => _dataConnection ??= Storehouse.GetKladovka();

    #endregion

    #region IOrderManager members

    /// <inheritdoc cref="IOrderManager.ListAllOrders"/>
    public Order[] ListAllOrders() => Orders.ToArray();

    /// <inheritdoc cref="IOrderManager.ListOrdersByStatus"/>
    public Order[] ListOrdersByStatus
        (
            string status
        )
    {
        Sure.NotNullNorEmpty (status);

        return Orders.Where (order => order.Status == status).ToArray();
    }

    /// <inheritdoc cref="IOrderManager.ListNewOrders"/>
    public Order[] ListNewOrders() => ListOrdersByStatus (Order.NewOrder);

    /// <inheritdoc cref="IOrderManager.ListOrdersForReader"/>
    public Order[] ListOrdersForReader
        (
            string ticket
        )
    {
        Sure.NotNullNorEmpty (ticket);

        return Orders.Where (order => order.Ticket == ticket).ToArray();
    }

    /// <inheritdoc cref="IOrderManager.CreateOrder"/>
    public bool CreateOrder
        (
            Order order,
            bool throwOnVerify = true
        )
    {
        Sure.NotNull (order);

        if (order.Verify (throwOnVerify))
        {
            var db = _GetDb();
            db.Insert (order);

            return true;
        }

        return false;
    }

    /// <inheritdoc cref="IOrderManager.DeleteOrder"/>
    public int DeleteOrder
        (
            int id
        )
    {
        Sure.NonNegative (id);

        return Orders.Delete (order => order.Id == id);
    }

    /// <inheritdoc cref="IOrderManager.SetOrderStatus"/>
    public int SetOrderStatus
        (
            int id,
            string status,
            bool sendEmail = false
        )
    {
        // TODO: implement sendMail

        var db = _GetDb();
        var orders = db.GetOrders();
        var result = db.Execute
            (
                $"update [{orders.TableName}] set [status] = @status where [id] = @id",
                new DataParameter ("status", status),
                new DataParameter ("id", id)
            );

        return result;
    }

    /// <inheritdoc cref="IOrderManager.UpdateOrder"/>
    public int UpdateOrder
        (
            Order order,
            bool throwOnVerify = true
        )
    {
        Sure.NotNull (order);

        if (order.Verify (throwOnVerify))
        {
            var db = _GetDb();
            return db.Update (order);
        }

        return -1;
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
    }

    #endregion
}
