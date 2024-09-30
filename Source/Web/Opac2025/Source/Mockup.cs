// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Mockup.cs -- заглушка для тестирования
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
/// Заглушка для тестирования.
/// </summary>
internal sealed class Mockup
{
    #region Private members

    internal readonly List<Order> _orders = new ()
    {
        new Order
        {
            Id = 1,
            Ticket = "123",
            Book = "Сказки Пушкина",
            Date = OpacUtility.LocalDate (2024, 1, 1),
            Status = Constants.New
        },
        new Order
        {
            Id = 2,
            Ticket = "124",
            Book = "Сказки Гоголя",
            Date = OpacUtility.LocalDate (2024, 1, 2),
            Status = Constants.Done
        },
        new Order
        {
            Id = 3,
            Ticket = "125",
            Book = "Сказки Толстого",
            Date = OpacUtility.LocalDate (2024, 1, 3),
            Status = Constants.Ready
        },
        new Order
        {
            Id = 4,
            Ticket = "126",
            Book = "Сказки Аксакова",
            Date = OpacUtility.LocalDate (2024, 1, 4),
            Status = Constants.Cancelled
        },
        new Order
        {
            Id = 5,
            Ticket = "127",
            Book = "Сказки Еще Чьи-то",
            Date = OpacUtility.LocalDate (2024, 1, 5),
            Status = Constants.Error
        }
    };

    private int _nextOrderId = 6;

    #endregion

    #region Public methods

    public int GetNextOrderId() => _nextOrderId++;

    public Order CreateOrder() => new() { Id = GetNextOrderId() };

    #endregion
}
