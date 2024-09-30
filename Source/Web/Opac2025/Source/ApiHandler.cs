// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ApiHandler.cs -- обработчик API-запросов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using Swashbuckle.AspNetCore.Annotations;

#endregion

namespace Opac2025;

/// <summary>
/// Обработчик API-запросов.
/// </summary>
internal sealed class ApiHandler
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ApiHandler
        (
            WebApplication application
        )
    {
        _serviceProvider = application.Services;
        _configuration = application.Configuration;
        _logger = _serviceProvider.GetRequiredService<ILogger<Storehouse>>();

        _logger.LogTrace (nameof (Storehouse) + "::Constructor");
    }

    #endregion

    #region Private members

    private readonly Mockup _mockup = new ();
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;


    // ReSharper disable UnusedMember.Local

    private Storehouse GetStorehouse() =>
        new (_serviceProvider, _configuration);

    // ReSharper restore UnusedMember.Local

    /// <summary>
    /// Перечисление всех заказов.
    /// </summary>
    [SwaggerOperation ("List all orders")]
    private IResult ListOrders()
    {
        // using var storehouse = GetStorehouse();
        _logger.LogTrace (nameof (ListOrders) + ": OK");

        return Results.Json (_mockup._orders);
    }

    /// <summary>
    /// Перечисление заказов с указанным статусом.
    /// </summary>
    [SwaggerOperation ("Find orders with given status")]
    private IResult ListOrdersWithStatus
        (
            [SwaggerParameter ("Status to retrieve")]
            string status
        )
    {
        Sure.NotNullNorEmpty (status);

        var orders = _mockup._orders
            .Where (o => o.Status == status)
            .ToArray();

        _logger.LogTrace (nameof (ListOrdersWithStatus) + ": {Status}: OK", status);

        return Results.Json (orders);
    }

    /// <summary>
    /// Создание нового заказа.
    /// </summary>
    [SwaggerOperation ("Create an order")]
    private IResult CreateOrder
        (
            [SwaggerParameter ("Order for creation")]
            Order newOrder
        )
    {
        Sure.NotNull (newOrder);

        newOrder.Id = _mockup.GetNextOrderId();
        _mockup._orders.Add (newOrder);

        _logger.LogTrace (nameof (CreateOrder) + ": {Order}: OK", newOrder);

        return Results.Ok();
    }

    /// <summary>
    /// Обновление данных о заказе.
    /// </summary>
    [SwaggerOperation ("Update the order")]
    private IResult UpdateOrder
        (
            [SwaggerParameter ("New values for the order")]
            Order updatedOrder
        )
    {
        Sure.NotNull (updatedOrder);

        var existingOrder = _mockup._orders.SingleOrDefault
            (
                o => o.Id == updatedOrder.Id
            );
        if (existingOrder is null)
        {
            _logger.LogTrace (nameof (UpdateOrder)
                              + ": {Order}: not found", updatedOrder);
            return Results.NotFound ();
        }

        existingOrder.Status = updatedOrder.Status;
        existingOrder.Book = updatedOrder.Book;
        existingOrder.Ticket = updatedOrder.Ticket;
        existingOrder.Date = updatedOrder.Date;

        _logger.LogTrace (nameof (UpdateOrder)
                          + ": {Order}: OK", updatedOrder);

        return Results.Ok();
    }

    /// <summary>
    /// Обновление данных о заказе.
    /// </summary>
    [SwaggerOperation ("Update an order status")]
    private IResult UpdateOrderStatus
        (
            [SwaggerParameter ("Order identifier")]
            int orderId,

            [SwaggerParameter ("New status value")]
            string newStatus
        )
    {
        Sure.NonNegative (orderId);
        Sure.NotNullNorEmpty (newStatus);

        var existingOrder = _mockup._orders.SingleOrDefault
            (
                o => o.Id == orderId
            );
        if (existingOrder is null)
        {
            _logger.LogTrace (nameof (UpdateOrderStatus) +
                              ": {OrderId} not found", orderId);
            return Results.NotFound();
        }

        existingOrder.Status = newStatus;
        _logger.LogTrace (nameof (UpdateOrderStatus) +
                          ": {OrderId}, {Status}: OK", newStatus, newStatus);

        return Results.Json (existingOrder);
    }

    /// <summary>
    /// Удаление указанного заказа.
    /// </summary>
    [SwaggerOperation ("Delete the order")]
    private IResult DeleteOrder
        (
            [SwaggerParameter ("Identifier of the order to delete")]
            int orderId
        )
    {
        Sure.NonNegative (orderId);

        var existingOrder = _mockup._orders.SingleOrDefault
            (
                o => o.Id == orderId
            );
        if (existingOrder is null)
        {
            _logger.LogTrace (nameof (DeleteOrder) + ": {OrderId}: not found", orderId);
            return Results.NotFound();
        }

        _mockup._orders.Remove (existingOrder);
        _logger.LogTrace (nameof (DeleteOrder) + ": {Order}: OK", existingOrder);

        return Results.Ok();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Регистрация обработчика.
    /// </summary>
    public void Register
        (
            RouteGroupBuilder api
        )
    {
        Sure.NotNull (api);

        api.MapGet ("/orders", ListOrders);
        api.MapGet ("/orders/{status}", ListOrdersWithStatus);
        api.MapPost ("/orders", CreateOrder);
        api.MapPut ("/orders", UpdateOrder);
        api.MapPut ("/orders/{status}", UpdateOrderStatus);
        api.MapDelete ("/orders/{id}", DeleteOrder);

    }

    #endregion
}
