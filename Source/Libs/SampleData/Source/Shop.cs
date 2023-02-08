// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Product.cs -- тестовый класс для опытов: сведения о магазине
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace SampleData;

/// <summary>
/// Тестовый класс для опытов: сведения о магазине.
/// </summary>
[UsedImplicitly]
public sealed class Shop
{
    #region Properties

    /// <summary>
    /// Идентификатор.
    /// </summary>
    [UsedImplicitly]
    public int Id { get; init; }
    
    /// <summary>
    /// Наименование.
    /// </summary>
    [UsedImplicitly]
    public string? Name { get; set; }
    
    /// <summary>
    /// Адрес магазина.
    /// </summary>
    [UsedImplicitly]
    public string? Address { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Иркутские магазины DNS.
    /// </summary>
    [UsedImplicitly]
    public static Shop[] DnsIrkutsk() => new Shop[]
    {
        new() { Id = 5916, Name = "МТЦ Новый", Address = "г. Иркутск, Советская, ул, дом 58/1" },
        new() { Id = 83476, Name = "Mega Home", Address = "г. Иркутск, ул. Сергеева 3/2А" },
        new() { Id = 1086, Name = "Базар", Address = "г. Иркутск, Трактовая, д. 35" },
        new() { Id = 12377, Name = "Версаль", Address = "г. Иркутск, Академическая, дом 31" },
        new() { Id = 2213, Name = "Торговый комплекс", Address = "г. Иркутск, ул. Литвинова, д. 17" },
        new() { Id = 4345, Name = "Электрон", Address = "г. Иркутск, ул. Рабочая, д. 18г" },
        new() { Id = 43, Name = "На Советской", Address = "г. Иркутск, Советская, д. 73" },
        new() { Id = 2126, Name = "Европарк", Address = "г. Иркутск, Розы Люксембург, д. 215В" },
        new() { Id = 2005, Name = "Оранж", Address = "г. Иркутск, Александра Невского, д. 80" },
        new() { Id = 5894, Name = "Смайл Молл", Address = "г. Иркутск, Баумана, ул, строение 233б/1" },
        new() { Id = 6175, Name = "Юбилейный", Address = "г. Иркутск, мкр. Юбилейный, дом 19/1" },
        new() { Id = 5111, Name = "На Чайке", Address = "г. Иркутск, ул. 2-я Железнодорожная, д. 76" },
        new() { Id = 4381, Name = "Слюдянка", Address = "г. Слюдянка, ул. Парижской Коммуны, д.5" },
        new() { Id = 22766, Name = "Усть-Ордынский", Address = "г. Усть-Ордынский, ул. Ербанова, д. 50Б/1" },
        new() { Id = 554401, Name = "Хомутово", Address = "г. Хомутово, ул. Трактовая, 1" },
        new() { Id = 952143, Name = "Шелехов", Address = "г. Шелехов, 6 квартал, д. 30, корп. а" },
        new() { Id = 5721, Name = "Маркова", Address = "Березовый (рп Маркова), мкр, дом 5" },
        new() { Id = 5875, Name = "Дом Торговли", Address = "г. Байкальск, Гагарина, мкр, дом 215" },
        new() { Id = 301812, Name = "Баррикад", Address = "г. Иркутск, ул. Баррикад, д. 51, корпус 8" },
        new() { Id = 1675, Name = "На Мельниковском", Address = "г. Иркутск, ул. Берёзовая роща, 1/2" },
        new() { Id = 5585, Name = "Сибирских Партизан", Address = "г. Иркутск, Сибирских Партизан, дом 18" },
    };

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals (object? obj) => obj is Shop shop && shop.Id == Id;

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() => HashCode.Combine (Id);

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Id}: {Name}: {Address}";

    #endregion
}
