// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* FakeOrderSource.cs -- источник тестовых заказов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Istu.OldModel;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace FakeData;

/// <summary>
/// Источник тестовых заказов.
/// </summary>
[PublicAPI]
public static class FakeOrderSource
{
    #region Public methods

    /// <summary>
    /// Получение списка заказов.
    /// </summary>
    public static Order[] GetOrders()
    {
        return new Order[]
        {
            new()
            {
                Id = 1,
                Ticket = "123",
                Name = "Карл Маркс",
                Moment = new DateTime (2023, 3, 20),
                Status = Order.NewOrder,
                Mfn = "1234",
                Number = "12345",
                Description = "Книга, которую хочет прочитать Карл Маркс",
                Mailto = "karl@marx.org"
            },

            new()
            {
                Id = 2,
                Ticket = "321",
                Name = "Фридрих Энгельс",
                Moment = new DateTime (2023, 3, 20),
                Status = Order.NewOrder,
                Mfn = "4321",
                Number = "54321",
                Description = "Книга, которую хочет прочитать Фридрих Энгельс",
                Mailto = "engels@marx.org"
            },

            new()
            {
                Id = 3,
                Ticket = "333",
                Name = "В. И. Ленин",
                Moment = new DateTime (2023, 3, 20),
                Status = Order.NewOrder,
                Mfn = "4444",
                Number = "5555",
                Description = "Книга, которую хочет прочитать В. И. Ленин",
                Mailto = "lenin@marx.org"
            },

        };
    }

    #endregion
}
