// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* City.cs -- тестовый класс для опытов: сведения о городе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace SampleData;

/// <summary>
/// Тестовый класс для опытов: сведения о городе.
/// </summary>
[UsedImplicitly]
public sealed class City
{
    #region Properties

    /// <summary>
    /// Название города.
    /// </summary>
    [UsedImplicitly]
    public required string Name { get; init; }
    
    /// <summary>
    /// Страна.
    /// </summary>
    [UsedImplicitly]
    public required string Country { get; init; }

    /// <summary>
    /// В каком году основан.
    /// </summary>
    [UsedImplicitly]
    public int? Established { get; set; }
    
    /// <summary>
    /// Площадь.
    /// </summary>
    [UsedImplicitly]
    public float Area { get; set; }

    /// <summary>
    /// Численность населения.
    /// </summary>
    [UsedImplicitly]
    public int Population { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение списка городов.
    /// </summary>
    public static City[] Cities() => new City[]
    {
        new() { Name = "Алжир", Country = "Алжир", Established = 944, Area = 273, Population = 2_364_230 },
        new() { Name = "Альбервиль", Country = "Франция", Established = 1836, Area = 17.56f, Population = 19_214 },
        new() { Name = "Амман", Country = "Иордания", Established = null, Area = 1680, Population = 1_812_059 },
        new() { Name = "Амстердам", Country = "Нидерланды", Established = 1300, Area = 243.65f, Population = 872_757 },
        new() { Name = "Антананариву", Country = "Мадагаскар", Established = 1625, Area = 85.01f, Population = 1_275_207 },
        new() { Name = "Антверпен", Country = "Франция", Established = null, Area = 204.51f, Population = 529_247 },
        new() { Name = "Аньшань", Country = "Китай", Established = 1937, Area = 9255.36f, Population = 3_325_372 },
        new() { Name = "Атланта", Country = "США", Established = 1837, Area = 343, Population = 498_715 },
        new() { Name = "Афины", Country = "Греция", Established = null, Area = 412, Population = 3_168_846 },
        new() { Name = "Баку", Country = "Азербайджан", Established = null, Area = 2140, Population = 1_259_000 },
        new() { Name = "Бамако", Country = "Мали", Established = null, Area = 245, Population = 1_809_106 },
        new() { Name = "Бангкок", Country = "Таиланд", Established = 1782, Area = 1568.373f, Population = 5_676_648 },
        new() { Name = "Барнаул", Country = "Россия", Established = 1730, Area = 322.01f, Population = 630_877 },
        new() { Name = "Барселона", Country = "Испания", Established = null, Area = 101.3f, Population = 1_636_193 },
        new() { Name = "Бейрут", Country = "Ливан", Established = 1500, Area = 20, Population = 1_854_240 },
        new() { Name = "Берлин", Country = "Германия", Established = 1237, Area = 1213.5f, Population = 3_664_088 },
        new() { Name = "Бисмарк", Country = "США", Established = 1872, Area = 81, Population = 61_272 },
        new() { Name = "Болонья", Country = "Италия", Established = null, Area = 140.73f, Population = 388_367 },
        new() { Name = "Бостон", Country = "США", Established = 1630, Area = 232.1f, Population = 692_600 },
        new() { Name = "Будапешт", Country = "Венгрия", Established = 1873, Area = 525.14f, Population = 1_709_057 },
        new() { Name = "Бухарест", Country = "Румыния", Established = 1459, Area = 238, Population = 1_835_258 },
        new() { Name = "Ванкувер", Country = "Канада", Established = 1886, Area = 114.67f, Population = 631_486 },
        new() { Name = "Варшава", Country = "Польша", Established = 1300, Area = 517, Population = 1_792_692 },
        new() { Name = "Вашингтон", Country = "США", Established = 1790, Area = 177, Population = 689_545 },
        new() { Name = "Вена", Country = "Австрия", Established = 881, Area = 414.75f, Population = 1_897_491 },
        new() { Name = "Венеция", Country = "Италия", Established = 421, Area = 415.9f, Population = 259_939 },
        new() { Name = "Вильнюс", Country = "Литва", Established = 1200, Area = 401, Population = 581_475 },
        new() { Name = "Владивосток", Country = "Россия", Established = 1880, Area = 325.99f, Population = 603_519 },
        new() { Name = "Волгоград", Country = "Россия", Established = 1589, Area = 859.353f, Population = 1_028_036 },
        new() { Name = "Воронеж", Country = "Россия", Established = 1586, Area = 596.51f, Population = 1_057_681 },
        new() { Name = "Гавана", Country = "Куба", Established = 1517, Area = 728.25f, Population = 2_106_146 },
        new() { Name = "Гармиш-Партенкирхен", Country = "Германия", Established = 1935, Area = 205.66f, Population = 26_177 },
        new() { Name = "Гватемала", Country = "Гватемала", Established = 1524, Area = 692, Population = 1_010_253 },
        new() { Name = "Генуя", Country = "Италия", Established = null, Area = 243.56f, Population = 580_097 },
        new() { Name = "Гетеборг", Country = "Швеция", Established = 1621, Area = 447.88f, Population = 607_882 },
        new() { Name = "Гонолулу", Country = "США", Established = 1907, Area = 177.2f, Population = 390_738 },
        new() { Name = "Гренобль", Country = "Франция", Established = null, Area = 18.44f, Population = 160_779 },
        new() { Name = "Гуанджоу", Country = "Китай", Established = null, Area = 7248.86f, Population = 18_676_605 },
        new() { Name = "Даллас", Country = "США", Established = 1841, Area = 1000.26f, Population = 1_304_379 },
        new() { Name = "Далянь", Country = "Китай", Established = 1904, Area = 13_630.44f, Population = 7_450_785 },
        new() { Name = "Дар-эс-Салам", Country = "Танзания", Established = 1862, Area = 162.5f, Population = 6_400_000 },
        new() { Name = "Денвер", Country = "США", Established = 1858, Area = 401.3f, Population = 649_495 },
        new() { Name = "Джефферсон-Сити", Country = "США", Established = 1821, Area = 73.2f, Population = 43_079 },
        new() { Name = "Екатеринбург", Country = "Россия", Established = 1723, Area = 1111.702f, Population = 1_544_376 },
        new() { Name = "Ереван", Country = "Армения", Established = 782, Area = 223.28f, Population = 1_092_700 },
        new() { Name = "Ижевск", Country = "Россия", Established = 1760, Area = 315.15f, Population = 623_472 },
        new() { Name = "Индианаполис", Country = "США", Established = 1821, Area = 952.9f, Population = 882_039 },
        new() { Name = "Инсбрук", Country = "Австрия", Established = 1234, Area = 104.91f, Population = 131_961 },
        new() { Name = "Иокогама", Country = "Япония", Established = 1889, Area = 437.38f, Population = 3_709_686 },
        new() { Name = "Иркутск", Country = "Россия", Established = 1661, Area = 305, Population = 617_264 },
        new() { Name = "Кабул", Country = "Афганистан", Established = 1200, Area = 275, Population = 4_273_156 },
        new() { Name = "Кавасаки", Country = "Япония", Established = 1924, Area = 142.70f, Population = 1_459_796 },
        new() { Name = "Казань", Country = "Россия", Established = 1005, Area = 588.9844f, Population = 1_308_660 },
        new() { Name = "Калгари", Country = "Канада", Established = 1875, Area = 726.5f, Population = 1_235_171 },
        new() { Name = "Каракас", Country = "Венесуэла", Established = 1567, Area = 1567, Population = 1_945_901 },
        new() { Name = "Катманду", Country = "Непал", Established = null, Area = 50.67f, Population = 1_006_656 },
        new() { Name = "Киев", Country = "Украина", Established = null, Area = 839, Population = 2_950_702 },
        new() { Name = "Киото", Country = "Япония", Established = 796, Area = 827.83f, Population = 1_464_890 },
        new() { Name = "Кобе", Country = "Япония", Established = 1889, Area = 552.26f, Population = 1_538_267 },
        new() { Name = "Колумбия", Country = "США", Established = 1787, Area = 346.5f, Population = 133_358 },
        new() { Name = "Копенгаген", Country = "Дания", Established = 1167, Area = 86.40f, Population = 615_993 },
        new() { Name = "Краснодар", Country = "Россия", Established = 1793, Area = 294.91f, Population = 1_099_344 },
        new() { Name = "Красноярск", Country = "Россия", Established = 1628, Area = 379.5f, Population = 1_187_771 },
        new() { Name = "Лейк-Плэсид", Country = "США", Established = null, Area = 3.9f, Population = 2638 },
        new() { Name = "Лиллехаммер", Country = "Норвегия", Established = null, Area = 477.44f, Population = 27_476 },
        new() { Name = "Лондон", Country = "Великобритания", Established = 47, Area = 1602, Population = 8_961_989 },
        new() { Name = "Лос-Анжелес", Country = "США", Established = 1781, Area = 1299.01f, Population = 3_849_297 },
        new() { Name = "Луанда", Country = "Ангола", Established = 1575, Area = 113, Population = 2_825_311 },
        new() { Name = "Лусака", Country = "Замбия", Established = 1905, Area = 418, Population = 1_742_979 },
        new() { Name = "Мапуту", Country = "Мозамбик", Established = 1781, Area = 347, Population = 1_080_277 },
        new() { Name = "Махачкала", Country = "Россия", Established = 1844, Area = 468.13f, Population = 623_254 },
        new() { Name = "Мбомбела", Country = "ЮАР", Established = 1905, Area = 72.63f, Population = 58_670 },
        new() { Name = "Мельбурн", Country = "Австралия", Established = 1835, Area = 9990.5f, Population = 5_159_211 },
        new() { Name = "Мехико", Country = "Мексика", Established = 1325, Area = 1680, Population = 9_100_000 },
        new() { Name = "Милан", Country = "Италия", Established = 600, Area = 181.67f, Population = 1_378_689 },
        new() { Name = "Минск", Country = "Белоруссия", Established = 1067, Area = 348.84f, Population = 1_996_553 },
        new() { Name = "Могадишо", Country = "Сомали", Established = 1331, Area = 350, Population = 2_388_000 },
        new() { Name = "Монреаль", Country = "Канада", Established = 1642, Area = 363.13f, Population = 1_942_694 },
        new() { Name = "Монтевидео", Country = "Уругвай", Established = 1726, Area = 540, Population = 1_381_000 },
        new() { Name = "Москва", Country = "Россия", Established = 1147, Area = 2561.5f, Population = 13_015_126 },
        new() { Name = "Мюнхен", Country = "Германия", Established = 1158, Area = 310.71f, Population = 1_488_202 },
        new() { Name = "Нагано", Country = "Япония", Established = 1897, Area = 834.85f, Population = 377_741 },
        new() { Name = "Нагоя", Country = "Япония", Established = 1889, Area = 326.43f, Population = 2_275_171 },
        new() { Name = "Нанкин", Country = "Китай", Established = -495, Area = 6587.02f, Population = 9_314_685 },
        new() { Name = "Неаполь", Country = "Италия", Established = null, Area = 117.27f, Population = 956_183 },
        new() { Name = "Нижний Новгород", Country = "Россия", Established = 1221, Area = 410.68f, Population = 1_249_861 },
        new() { Name = "Новосибирск", Country = "Россия", Established = 1893, Area = 502.7f, Population = 1_633_595 },
        new() { Name = "Нью-Йорк", Country = "США", Established = 1621, Area = 1223.3f, Population = 8_467_513 },
        new() { Name = "Окленд", Country = "Новая Зеландия", Established = 1840, Area = 1086, Population = 1_534_700 },
        new() { Name = "Омск", Country = "Россия", Established = 1716, Area = 577.9f, Population = 1_112_625 },
        new() { Name = "Осака", Country = "Япония", Established = 1889, Area = 223, Population = 2_685_481 },
        new() { Name = "Осло", Country = "Норвегия", Established = 1048, Area = 454, Population = 699_827 },
        new() { Name = "Оттава", Country = "Канада", Established = 1850, Area = 2790.3f, Population = 934_243 },
        new() { Name = "Палермо", Country = "Италия", Established = null, Area = 160.59f, Population = 663_770 },
        new() { Name = "Париж", Country = "Франция", Established = null, Area = 105.4f, Population = 2_148_327 },
        new() { Name = "Пекин", Country = "Китай", Established = null, Area = 16_410.54f, Population = 21_893_095 },
        new() { Name = "Пермь", Country = "Россия", Established = 1723, Area = 799.68f, Population = 1_034_002 },
        new() { Name = "Пномпень", Country = "Камбоджа", Established = 1372, Area = 290, Population = 1_501_725 },
        new() { Name = "Прага", Country = "Чехия", Established = null, Area = 496, Population = 1_275_406 },
        new() { Name = "Пхеньян", Country = "КНДР", Established = null, Area = 315, Population = 4_138_187 },
        new() { Name = "Рим", Country = "Италия", Established = -753, Area = 1287.36f, Population = 2_758_454 },
        new() { Name = "Ростов-на-Дону", Country = "Россия", Established = 1749, Area = 348.5f, Population = 1_142_162 },
        new() { Name = "Сайтама", Country = "Япония", Established = 2001, Area = 217.49f, Population = 1_250_928 },
        new() { Name = "Самара", Country = "Россия", Established = 1586, Area = 541.4f, Population = 1_173_299 },
        new() { Name = "Сана", Country = "Йемен", Established = null, Area = 126, Population = 2_575_347 },
        new() { Name = "Санкт-Мориц", Country = "Швейцария", Established = null, Area = 28.69f, Population = 4882 },
        new() { Name = "Санкт-Петербург", Country = "Россия", Established = 1703, Area = 1439, Population = 5_607_916 },
        new() { Name = "Санто-Доминго", Country = "Доминикана", Established = 1496, Area = 104.44f, Population = 2_023_029 },
        new() { Name = "Саппоро", Country = "Япония", Established = 1922, Area = 1121.12f, Population = 1_933_787 },
        new() { Name = "Сараево", Country = "Босния и Герцеговина", Established = 1462, Area = 141.5f, Population = 343_000 },
        new() { Name = "Саратов", Country = "Россия", Established = 1590, Area = 1490, Population = 901_361 },
        new() { Name = "Сендай", Country = "Япония", Established = 1600, Area = 785.85f, Population = 1_052_147 },
        new() { Name = "Сент-Луис", Country = "США", Established = 1763, Area = 107.9f, Population = 293_310 },
        new() { Name = "Сеул", Country = "Корея", Established = 1394, Area = 605.24f, Population = 10_049_607 },
        new() { Name = "Сиань", Country = "Китай", Established = null, Area = 10_096.81f, Population = 12_952_907 },
        new() { Name = "Сидней", Country = "Австралия", Established = 1788, Area = 12_144.6f, Population = 5_367_206 },
        new() { Name = "Солт-Лейк-Сити", Country = "США", Established = 1847, Area = 285.0f, Population = 200_567 },
        new() { Name = "София", Country = "Болгария", Established = null, Area = 492, Population = 1_379_874 },
        new() { Name = "Сочи", Country = "Россия", Established = 1838, Area = 176.77f, Population = 466_078 },
        new() { Name = "Стокгольм", Country = "Швеция", Established = 1187, Area = 188, Population = 1_981_330 },
        new() { Name = "Ташкент", Country = "Узбекистан", Established = null, Area = 435, Population = 2_934_100 },
        new() { Name = "Тбилиси", Country = "Грузия", Established = 455, Area = 720, Population = 1_172_010 },
        new() { Name = "Токио", Country = "Япония", Established = null, Area = 2193.96f, Population = 14_043_239 },
        new() { Name = "Тольятти", Country = "Россия", Established = 1737, Area = 314.78f, Population = 684_709 },
        new() { Name = "Торонто", Country = "Канада", Established = 1793, Area = 630.21f, Population = 2_503_281 },
        new() { Name = "Триполи", Country = "Ливия", Established = null, Area = 400, Population = 1_780_000 },
        new() { Name = "Турин", Country = "Италия", Established = null, Area = 130.17f, Population = 878_735 },
        new() { Name = "Тюмень", Country = "Россия", Established = 1586, Area = 490.82f, Population = 847_488 },
        new() { Name = "Тяньцзинь", Country = "Китай", Established = null, Area = 11_920, Population = 13_866_009 },
        new() { Name = "Улан-Батор", Country = "Монголия", Established = 1639, Area = 4704.4f, Population = 1_405_000 },
        new() { Name = "Ульяновск", Country = "Россия", Established = 1648, Area = 316, Population = 617_352 },
        new() { Name = "Урумчи", Country = "Китай", Established = null, Area = 13_783.1f, Population = 4_054_369 },
        new() { Name = "Уфа", Country = "Россия", Established = 1574, Area = 708, Population = 1_144_809 },
        new() { Name = "Ухань", Country = "Китай", Established = null, Area = 8569.15f, Population = 12_326_518 },
        new() { Name = "Филадельфия", Country = "США", Established = 1682, Area = 369.59f, Population = 1_584_138 },
        new() { Name = "Флоренция", Country = "Италия", Established = null, Area = 102.32f, Population = 382_347 },
        new() { Name = "Фритаун", Country = "Сьерра-Леоне", Established = 1792, Area = 357, Population = 1_055_964 },
        new() { Name = "Фукуока", Country = "Япония", Established = 1889, Area = 341.70f, Population = 1_517_650 },
        new() { Name = "Хабаровск", Country = "Россия", Established = 1858, Area = 389, Population = 617_441 },
        new() { Name = "Хараре", Country = "Зимбабве", Established = 1890, Area = 960, Population = 2_150_000 },
        new() { Name = "Харбин", Country = "Китай", Established = 1898, Area = 53_076.48f, Population = 10_009_854 },
        new() { Name = "Хельсинки", Country = "Финляндия", Established = 1550, Area = 213.8f, Population = 656_611 },
        new() { Name = "Хиросима", Country = "Япония", Established = 1589, Area = 905.41f, Population = 1_202_000 },
        new() { Name = "Челябинск", Country = "Россия", Established = 1736, Area = 500.91f, Population = 1_189_525 },
        new() { Name = "Чикаго", Country = "США", Established = 1795, Area = 606.424f, Population = 2_746_388 },
        new() { Name = "Чуньцин", Country = "Китай", Established = 1929, Area = 82_403, Population = 32_054_159 },
        new() { Name = "Чэнду", Country = "Китай", Established = -311, Area = 14_334.78f, Population = 20_937_757 },
        new() { Name = "Шамони-Мон-Блан", Country = "Франция", Established = 1091, Area = 116.53f, Population = 9830 },
        new() { Name = "Шанхай", Country = "Китай", Established = 1949, Area = 6341, Population = 24_870_895 },
        new() { Name = "Шеньян", Country = "Китай", Established = null, Area = 12_859.89f, Population = 9_070_093 },
        new() { Name = "Шэньчжэнь", Country = "Китай", Established = 1979, Area = 1997.27f, Population = 17_494_398 },
        new() { Name = "Эдмонтон", Country = "Канада", Established = 1795, Area = 684, Population = 932_546 },
        new() { Name = "Янгон", Country = "Мьянма", Established = 1028, Area = 576, Population = 7_360_703 },
        new() { Name = "Яунде", Country = "Камерун", Established = 1888, Area = 180, Population = 3_412_000 },
    };

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals (object? obj) => 
        obj is City city && city.Name == Name && city.Country == Country;

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() => HashCode.Combine (Name, Country);

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name} ({Country})";

    #endregion
}
