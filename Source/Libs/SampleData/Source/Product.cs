// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Product.cs -- тестовый класс для опытов: сведения о товаре
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace SampleData;

/// <summary>
/// Тестовый класс для опытов: сведения о товаре.
/// </summary>
[UsedImplicitly]
public sealed class Product
{
    #region Properties

    /// <summary>
    /// Артикул.
    /// </summary>
    [UsedImplicitly]
    public int Id { get; init; }
    
    /// <summary>
    /// Категория товара.
    /// </summary>
    [UsedImplicitly]
    public string? Category { get; init; }
    
    /// <summary>
    /// Наименование товара.
    /// </summary>
    [UsedImplicitly]
    public string? Name { get; set; }
    
    /// <summary>
    /// Цена.
    /// </summary>
    [UsedImplicitly]
    public decimal Price { get; set; }
    
    #endregion

    #region Public methods

    /// <summary>
    /// Ассортимент DNS.
    /// </summary>
    [UsedImplicitly]
    public static Product[] ComputerGoods() => new Product[]
    {
        new() { Id = 4793041, Category = "Смартфон", Name = "Смартфон BQ 5031G Fun 8 ГБ серый", Price = 3399m },
        new() { Id = 5013925, Category = "Смартфон", Name = "Смартфон INOI A22 Lite 8 ГБ черный", Price = 3499m },
        new() { Id = 4891224, Category = "Смартфон", Name = "Смартфон BQ 5060L Basic 8 ГБ красный", Price = 3799m },
        new() { Id = 4888314, Category = "Смартфон", Name = "Смартфон BQ 5560L Trend 8 ГБ зеленый", Price = 3999m },
        new() { Id = 5088334, Category = "Смарт-часы", Name = "Фитнес-браслет Maxvi SB-01", Price = 650m },
        new() { Id = 9908415, Category = "Смарт-часы", Name = "Смарт-часы ZDK Sport Fitpro", Price = 799m },
        new() { Id = 5063856, Category = "Смарт-часы", Name = "Фитнес-браслет DEXP M6", Price = 850m },
        new() { Id = 5042201, Category = "Телевизор", Name = "Телевизор LED DEXP H24H7000E черный", Price = 7399m },
        new() { Id = 1638645, Category = "Телевизор", Name = "Телевизор LED DEXP H24F7000E черный", Price = 7499m },
        new() { Id = 1331514, Category = "Телевизор", Name = "Телевизор LED Olto 24T20H черный", Price = 7599m },
        new() { Id = 5042232, Category = "Телевизор", Name = "Телевизор LED DEXP H32H7000E черный", Price = 8499m },
        new() { Id = 5354654, Category = "Телевизор", Name = "Телевизор LED Erisson 24LES80T2 черный", Price = 8999m },
        new() { Id = 1671536, Category = "Планшет", Name = "Планшет DEXP Ursus K17 3G 16 ГБ серый", Price = 3499m },
        new() { Id = 9905860, Category = "Планшет", Name = "Планшет BQ 7098G Armor Power 3G 8 ГБ разноцветный", Price = 3899m },
        new() { Id = 5035540, Category = "Планшет", Name = "Планшет Blackview Tab 6 Kids LTE 32 ГБ синий", Price = 7999m },
        new() { Id = 1607583, Category = "Планшет", Name = "Планшет Lenovo Tab M8 HD LTE 32 ГБ серебристый", Price = 8499m },
        new() { Id = 4863603, Category = "Планшет", Name = "Планшет TCL TAB 10L Wi-Fi 32 ГБ черный", Price = 8999m },
        new() { Id = 5056098, Category = "Планшет", Name = "Планшет HUAWEI Matepad T 8 (2022) Wi-Fi 32 ГБ синий", Price = 9999m },
        new() { Id = 5074554, Category = "Ноутбук", Name = "Ноутбук DEXP Aquilon серебристый", Price = 15_999m },
        new() { Id = 9932396, Category = "Ноутбук", Name = "Ноутбук Irbis NB283 серый", Price = 15_999m },
        new() { Id = 9907461, Category = "Ноутбук", Name = "Ноутбук Digma EVE 14 C414 серый", Price = 17_999m },
        new() { Id = 5094947, Category = "Ноутбук", Name = "Ноутбук Echips Envy серебристый", Price = 21_999m },
        new() { Id = 4876024, Category = "Ноутбук", Name = "Ноутбук ASUS Laptop 15 D543MA-DM1368 черный", Price = 22_999m },
        new() { Id = 5087848, Category = "Ноутбук", Name = "Ноутбук HP 255 G8 серебристый", Price = 22_999m },
        new() { Id = 5324829, Category = "Ноутбук", Name = "Ноутбук Acer Extensa 15 EX215-31-C3FF черный", Price = 25_999m },
        new() { Id = 1617203, Category = "Чехол", Name = "Чехол PortCase KNP-18 PN", Price = 250m },
        new() { Id = 1639846, Category = "Чехол", Name = "Чехол VLP PCBM-MB12", Price = 299m },
        new() { Id = 1387421, Category = "Чехол", Name = "Чехол DF MacCase-04", Price = 399m },
        new() { Id = 5007701, Category = "Чехол", Name = "Чехол Aceline SL1501NB", Price = 399m },
        new() { Id = 5066167, Category = "ПК", Name = "ПК DEXP Atlas H341", Price = 10_499m },
        new() { Id = 5046062, Category = "ПК", Name = "DEXP Aquilon O274", Price = 11_299m },
        new() { Id = 9939870, Category = "ПК", Name = "Мини ПК Lenovo ThinkCentre M75n IoT", Price = 16_499m },
        new() { Id = 9926393, Category = "ПК", Name = "Мини ПК HP t540 DM 34C62ES", Price = 18_899m },
        new() { Id = 5046210, Category = "ПК", Name = "Мини ПК MSI Cubi N JSL-041RU", Price = 18_999m },
        new() { Id = 9913386, Category = "ПК", Name = "Мини ПК Hiper M11", Price = 19_999m },
        new() { Id = 4873023, Category = "ПК", Name = "Мини ПК Acer Revo Box RN96", Price = 19_999m },
        new() { Id = 5094465, Category = "ПК", Name = "Мини ПК IRBIS IMFPC101", Price = 26_999m },
        new() { Id = 4888821, Category = "ОС", Name = "Операционная система Microsoft Windows 11 Home", Price = 15_999m },
        new() { Id = 1241341, Category = "ОС", Name = "Операционная система Microsoft Windows 10 Home", Price = 17_699m },
        new() { Id = 1023646, Category = "ОС", Name = "Операционная система Microsoft Windows 10 Pro", Price = 23_999m },
        new() { Id = 0197141, Category = "Монитор", Name = "Монитор AOC E970SWN черный", Price = 6499m },
        new() { Id = 8189155, Category = "Монитор", Name = "Монитор Acer EK220QAbi черный", Price = 6999m },
        new() { Id = 1000720, Category = "Монитор", Name = "Монитор Philips 203V5LSB26 черный", Price = 7699m },
        new() { Id = 5032352, Category = "Монитор", Name = "Монитор MSI Pro MP241X черный", Price = 8999m },
        new() { Id = 5370641, Category = "Монитор", Name = "Монитор ASUS VS197DE черный", Price = 9_099m },
        new() { Id = 5030046, Category = "Клавиатура", Name = "Клавиатура проводная Aceline K-1204BU", Price = 299m },
        new() { Id = 4787881, Category = "Клавиатура", Name = "Клавиатура проводная Oklick 180M Black PS/2", Price = 350m },
        new() { Id = 5010030, Category = "Клавиатура", Name = "Клавиатура проводная Qumo Office Element К65", Price = 350m },
        new() { Id = 1627135, Category = "Клавиатура", Name = "Клавиатура проводная DEXP K-909BU", Price = 399m },
        new() { Id = 8105968, Category = "Клавиатура", Name = "Клавиатура проводная Defender Accent SB-720 RU", Price = 399m },
        new() { Id = 1359448, Category = "Мышь", Name = "Мышь проводная Aceline CM-408BU черная", Price = 99m },
        new() { Id = 1399148, Category = "Мышь", Name = "Мышь проводная DEXP СM-801 черная", Price = 150m },
        new() { Id = 1108603, Category = "Мышь", Name = "Мышь проводная Defender MS-960 голубая", Price = 199m },
        new() { Id = 5354973, Category = "Мышь", Name = "Мышь проводная ExeGate SH-9025", Price = 199m },
        new() { Id = 1330834, Category = "Мышь", Name = "Мышь проводная Oklick 105S черная", Price = 199m },
        new() { Id = 9912783, Category = "Мышь", Name = "Мышь проводная Perfeo GLOW PF_4435 черная", Price = 199m },
        new() { Id = 1365054, Category = "Мышь", Name = "Мышь проводная QUMO Office M14 белый", Price = 199m },
        new() { Id = 4876362, Category = "Наушники", Name = "Проводная гарнитура Exployd EX-HP-1124", Price = 99m },
        new() { Id = 9938785, Category = "Наушники", Name = "Проводная гарнитура FORZA 410-054", Price = 99m },
        new() { Id = 4887696, Category = "Наушники", Name = "Проводные наушники OLTO VS-820", Price = 99m },
        new() { Id = 1112951, Category = "Колонки", Name = "Колонки 2.0 Aceline ASP100", Price = 199m },
        new() { Id = 5364310, Category = "Колонки", Name = "Колонки 2.0 ExeGate Disco 140", Price = 199m },
        new() { Id = 1040424, Category = "Колонки", Name = "Колонки 2.0 Defender SPK-22", Price = 250m },
        new() { Id = 4830071, Category = "Электронная книга", Name = "Электронная книга DEXP S4 Symbol ", Price = 4999m },
        new() { Id = 4844790, Category = "Электронная книга", Name = "Электронная книга Digma K1", Price = 5999m },
        new() { Id = 4844791, Category = "Электронная книга", Name = "Электронная книга Ritmix RBK-617", Price = 5999m },
        new() { Id = 4899465, Category = "Процессор", Name = "Процессор AMD PRO A6-8570 OEM", Price = 1299m },
        new() { Id = 1145121, Category = "Процессор", Name = "Процессор AMD A6-9500E OEM", Price = 1399m },
        new() { Id = 1145122, Category = "Процессор", Name = "Процессор AMD Athlon X4 970 OEM", Price = 2699m },
        new() { Id = 4716232, Category = "Процессор", Name = "Процессор Intel Celeron G5905 OEM", Price = 2799m },
        new() { Id = 4872432, Category = "Материнская плата", Name = "Материнская плата Esonic G31CHL3", Price = 2999m },
        new() { Id = 1279550, Category = "Материнская плата", Name = "Материнская плата ASRock H310CM-DVS", Price = 3499m },
        new() { Id = 1684185, Category = "Материнская плата", Name = "Материнская плата Biostar H310MHP", Price = 3499m },
    };

    #endregion

    #region Object members

    /// <inheritdoc cref="Equals"/>
    public override bool Equals (object? obj) =>
        obj is Product product && product.Id == Id;

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() => HashCode.Combine (Id);

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() =>
        $"{Id}: {Category}: {Name}: {Price}";

    #endregion
}
