// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Country.cs -- тестовый класс для опытов: сведения о стране
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace SampleData;

/// <summary>
/// Тестовый класс для опытов: сведения о стране.
/// </summary>
[UsedImplicitly]
public sealed class Country
{
    #region Properties

    /// <summary>
    /// Название страны.
    /// </summary>
    [UsedImplicitly]
    public required string Name { get; init; }
    
    /// <summary>
    /// Столица.
    /// </summary>
    [UsedImplicitly]
    public required string Capital { get; set; }
    
    /// <summary>
    /// Площадь территории.
    /// </summary>
    [UsedImplicitly]
    public float Area { get; set; }
    
    /// <summary>
    /// Население.
    /// </summary>
    [UsedImplicitly]
    public int Population { get; set; }
    
    /// <summary>
    /// Язык.
    /// </summary>
    [UsedImplicitly]
    public string? Language { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение списка стран.
    /// </summary>
    public static Country[] Countries() => new Country[]
    {
        new() { Name = "Австралия", Capital = "Канберра", Area = 7_692_024, Population = 25_766_605, Language = "английский" },
        new() { Name = "Австрия", Capital = "Вена", Area = 83_879, Population = 9_923_507, Language = "немецкий" },
        new() { Name = "Азербайджан", Capital = "Баку", Area = 86_600, Population = 10_179_147, Language = "азербайджанский" },
        new() { Name = "Албания", Capital = "Тирана", Area = 28_748, Population = 2_793_592, Language = "албанский" },
        new() { Name = "Алжир", Capital = "Алжир", Area = 2_381_740, Population = 38_087_812, Language = "арабский" },
        new() { Name = "Ангола", Capital = "Луанда", Area = 1_246_700, Population = 34_795_287, Language = "португальский" },
        new() { Name = "Андорра", Capital = "Андорра-ла-Велья", Area = 467.63f, Population = 82_887, Language = "каталанский" },
        new() { Name = "Антигуа и Барбуда", Capital = "Сент-Джонс", Area = 440, Population = 93_581, Language = "английский" },
        new() { Name = "Аргентина", Capital = "Буэнос-Айрес", Area = 2_780_400, Population = 46_403_544, Language = "испанский" },
        new() { Name = "Армения", Capital = "Ереван", Area = 29_743, Population = 2_986_100, Language = "армянский" },
        new() { Name = "Афганистан", Capital = "Кабул", Area = 652_864, Population = 40_719_234, Language = "дари" },
        new() { Name = "Багамы", Capital = "Нассау", Area = 13_878, Population = 321_834, Language = "английский" },
        new() { Name = "Бангладеш", Capital = "Дакка", Area = 148_460, Population = 171_674_893, Language = "бенгальский" },
        new() { Name = "Барбадос", Capital = "Бриджтаун", Area = 430, Population = 290_604, Language = "английский" },
        new() { Name = "Бахрейн", Capital = "Манама", Area = 766, Population = 1_505_003, Language = "арабский" },
        new() { Name = "Белоруссия", Capital = "Минск", Area = 207_600, Population = 9_255_524, Language = "белорусский" },
        new() { Name = "Белиз", Capital = "Бельмопан", Area = 22_966, Population = 430_131, Language = "английский" },
        new() { Name = "Бельгия", Capital = "Брюссель", Area = 30_528, Population = 11_584_008, Language = "нидерландский" },
        new() { Name = "Бенин", Capital = "Порто-Ново", Area = 112_622, Population = 12_864_634, Language = "французский" },
        new() { Name = "Болгария", Capital = "София", Area = 110_993.6f, Population = 6_847_402, Language = "болгарский" },
        new() { Name = "Боливия", Capital = "Сукре", Area = 1_098_581, Population = 11_639_909, Language = "испанский" },
        new() { Name = "Босния и Герцеговина", Capital = "Сараево", Area = 51_197, Population = 3_531_159, Language = "боснийский" },
        new() { Name = "Ботсвана", Capital = "Габороне", Area = 581_730, Population = 2_380_250, Language = "английский" },
        new() { Name = "Бразилия", Capital = "Бразилиа", Area = 8_515_767, Population = 207_353_391, Language = "португальский" },
        new() { Name = "Бруней", Capital = "Бандар-Сери-Бегаван", Area = 5765, Population = 464_478, Language = "малайский" },
        new() { Name = "Буркина-Фасо", Capital = "Уагадугу", Area = 274_200, Population = 20_835_401, Language = "французский" },
        new() { Name = "Бурунди", Capital = "Гитега", Area = 27_830, Population = 11_099_298, Language = "рунди" },
        new() { Name = "Бутан", Capital = "Тхимпху", Area = 38_394, Population = 758_288, Language = "дзонг-кэ" },
        new() { Name = "Вануату", Capital = "Порт-Вила", Area = 12_190, Population = 277_554, Language = "английский" },
        new() { Name = "Ватикан", Capital = "Ватикан", Area = 0.49f, Population = 825, Language = "итальянский" },
        new() { Name = "Великобритания", Capital = "Лондон", Area = 242_495, Population = 67_081_000, Language = "анлийский" },
        new() { Name = "Венгрия", Capital = "Будапешт", Area = 93_036, Population = 9_689_010, Language = "венгерский" },
        new() { Name = "Венесуэла", Capital = "Каракас", Area = 916_445, Population = 28_887_118, Language = "испанский" },
        new() { Name = "Восточный Тимор", Capital = "Дили", Area = 15_007, Population = 1_291_358, Language = "тетум" },
        new() { Name = "Вьетнам", Capital = "Ханой", Area = 331_210, Population = 99_495_753, Language = "вьетнамский" },
        new() { Name = "Габон", Capital = "Либревиль", Area = 267_667, Population = 2_230_908, Language = "французский" },
        new() { Name = "Гаити", Capital = "Порт-о-Пренс", Area = 27_750, Population = 11_508_242, Language = "гаитянский" },
        new() { Name = "Гайана", Capital = "Джорджтаун", Area = 214_970, Population = 773_303, Language = "английский" },
        new() { Name = "Гамбия", Capital = "Банжул", Area = 11_300, Population = 2_173_999, Language = "английский" },
        new() { Name = "Гана", Capital = "Аккра", Area = 238_533, Population = 33_107_275, Language = "аглийский" },
        new() { Name = "Гватемала", Capital = "Гватемала", Area = 108_889, Population = 18_697_234, Language = "испанский" },
        new() { Name = "Гвинея", Capital = "Конакри", Area = 245_857, Population = 12_771_000, Language = "французский" },
        new() { Name = "Гвинея-Бисау", Capital = "Бисау", Area = 36_120, Population = 1_647_000, Language = "португальский" },
        new() { Name = "Германия", Capital = "Берлин", Area = 357_385, Population = 83_019_200, Language = "немецкий" },
        new() { Name = "Гондурас", Capital = "Тегусигальпа", Area = 112_090, Population = 8_448_465, Language = "испанский" },
        new() { Name = "Гренада", Capital = "Сент-Джорджес", Area = 344, Population = 113_648, Language = "английский" },
        new() { Name = "Греция", Capital = "Афины", Area = 131_957, Population = 10_340_064, Language = "греческий" },
        new() { Name = "Грузия", Capital = "Тбилиси", Area = 69_789, Population = 3_723_536, Language = "грузинский" },
        new() { Name = "Дания", Capital = "Копенгаген", Area = 43_094, Population = 5_848_669, Language = "датский" },
        new() { Name = "Джибути", Capital = "Джибути", Area = 23_200, Population = 921_804, Language = "арабский" },
        new() { Name = "Доминика", Capital = "Розо", Area = 751, Population = 74_243, Language = "английский" },
        new() { Name = "Доминикана", Capital = "Санто-Доминго", Area = 48_670, Population = 10_499_707, Language = "испанский" },
        new() { Name = "Египет", Capital = "Каир", Area = 1_001_450, Population = 106_646_200, Language = "арабский" },
        new() { Name = "Замбия", Capital = "Лусака", Area = 752_614, Population = 19_610_769, Language = "английский" },
        new() { Name = "Зимбабве", Capital = "Хараре", Area = 390_757, Population = 14_862_927, Language = "английский" },
        new() { Name = "Израиль", Capital = "Иерусалим", Area = 20_770, Population = 9_449_000, Language = "иврит" },
        new() { Name = "Индия", Capital = "Дели", Area = 3_287_263, Population = 1_400_000_000, Language = "хинди" },
        new() { Name = "Индонезия", Capital = "Джакарта", Area = 1_919_440, Population = 270_203_917, Language = "индонезийский" },
        new() { Name = "Иордания", Capital = "Амман", Area = 92_300, Population = 9_856_034, Language = "арабский" },
        new() { Name = "Ирак", Capital = "Багдад", Area = 437_072, Population = 43_839_634, Language = "арабский" },
        new() { Name = "Иран", Capital = "Тегеран", Area = 1_648_195, Population = 81_000_000, Language = "персидский" },
        new() { Name = "Ирландия", Capital = "Дублин", Area = 70_273, Population = 5_123_536, Language = "ирландский" },
        new() { Name = "Исландия", Capital = "Рейкьявик", Area = 103_125, Population = 368_590, Language = "исландский" },
        new() { Name = "Испания", Capital = "Мадрид", Area = 505_990, Population = 47_163_418, Language = "испанский" },
        new() { Name = "Италия", Capital = "Рим", Area = 302_073, Population = 58_983_122, Language = "итальянский" },
        new() { Name = "Йемен", Capital = "Сана", Area = 527_970, Population = 30_553_054, Language = "арабский" },
        new() { Name = "Кабо-Верде", Capital = "Прая", Area = 4033, Population = 583_255, Language = "португальский" },
        new() { Name = "Казахстан", Capital = "Астана", Area = 2_724_902, Population = 19_765_004, Language = "казахский" },
        new() { Name = "Камбоджа", Capital = "Пномпень", Area = 181_035, Population = 16_926_984, Language = "кхмерский" },
        new() { Name = "Камерун", Capital = "Яунде", Area = 475_440, Population = 27_744_989, Language = "французский" },
        new() { Name = "Канада", Capital = "Оттава", Area = 9_984_670, Population = 39_432_111, Language = "английский" },
        new() { Name = "Катар", Capital = "Доха", Area = 11_586, Population = 2_846_118, Language = "арабский" },
        new() { Name = "Кения", Capital = "Найроби", Area = 582_650, Population = 47_564_296, Language = "английский" },
        new() { Name = "Кипр", Capital = "Никосия", Area = 9250, Population = 1_229_087, Language = "греческий" },
        new() { Name = "Киргизия", Capital = "Бишкек", Area = 199_951, Population = 6_700_000, Language = "киргизский" },
        new() { Name = "Кирибати", Capital = "Южная Тарава", Area = 812.34f, Population = 115_300, Language = "кирибати" },
        new() { Name = "Китай", Capital = "Пекин", Area = 9_598_962, Population = 1_411_778_724, Language = "китайский" },
        new() { Name = "Колумбия", Capital = "Богота", Area = 1_141_748, Population = 52_156_254, Language = "испанский" },
        new() { Name = "Коморы", Capital = "Морони", Area = 2235, Population = 846_281, Language = "коморский" },
        new() { Name = "Конго", Capital = "Браззавиль", Area = 342_000, Population = 5_643_646, Language = "французский" },
        new() { Name = "ДР Конго", Capital = "Киншаса", Area = 2_345_409, Population = 108_407_721, Language = "французский" },
        new() { Name = "Корейская Народно-Демократическая Республика", Capital = "Пхеньян", Area = 120_540, Population = 26_111_648, Language = "корейский" },
        new() { Name = "Корея", Capital = "Сеул", Area = 100_210, Population = 51_744_876, Language = "корейский" },
        new() { Name = "Коста-Рика", Capital = "Сан-Хосе", Area = 51_100, Population = 5_097_988, Language = "испанский" },
        new() { Name = "Кот-д'Ивуар", Capital = "Ямусукро", Area = 322_463, Population = 27_970_139, Language = "французский" },
        new() { Name = "Куба", Capital = "Гавана", Area = 110_860, Population = 11_061_886, Language = "испанский" },
        new() { Name = "Кувейт", Capital = "Эль-Кувейт", Area = 17_818, Population = 4_464_521, Language = "арабский" },
        new() { Name = "Лаос", Capital = "Вьентьян", Area = 237_055, Population = 7_123_205, Language = "лаосский" },
        new() { Name = "Латвия", Capital = "Рига", Area = 64_589, Population = 1_873_100, Language = "латвийский" },
        new() { Name = "Лесото", Capital = "Масеру", Area = 30_355, Population = 2_031_000, Language = "сесото" },
        new() { Name = "Либерия", Capital = "Монровия", Area = 111_369, Population = 4_937_000, Language = "английский" },
        new() { Name = "Ливан", Capital = "Бейрут", Area = 10_452, Population = 8_133_770, Language = "арабский" },
        new() { Name = "Ливия", Capital = "Триполи", Area = 1_759_541, Population = 7_200_000, Language = "арабский" },
        new() { Name = "Литва", Capital = "Вильнюс", Area = 65_302, Population = 2_797_945, Language = "литовский" },
        new() { Name = "Лихтенштейн", Capital = "Вадуц", Area = 160, Population = 38_829, Language = "немецкий" },
        new() { Name = "Люксембург", Capital = "Люксембург", Area = 2586.4f, Population = 626_108, Language = "люксембургский" },
        new() { Name = "Маврикий", Capital = "Порт-Луи", Area = 2040, Population = 1_295_789, Language = "английский" },
        new() { Name = "Мавритания", Capital = "Нуакшот", Area = 1_030_700, Population = 4_649_658, Language = "арабский" },
        new() { Name = "Мадагаскар", Capital = "Антананариву", Area = 587_041, Population = 24_823_539, Language = "малагасийский" },
        new() { Name = "Малави", Capital = "Лилонгве", Area = 118_494, Population = 16_777_547, Language = "английский" },
        new() { Name = "Малайзия", Capital = "Куала-Лумпур", Area = 329_758, Population = 32_763_726, Language = "малайский" },
        new() { Name = "Мали", Capital = "Бамако", Area = 1_240_192, Population = 22_106_606, Language = "французский" },
        new() { Name = "Мальдивы", Capital = "Мале", Area = 298, Population = 402_071, Language = "мальдивский" },
        new() { Name = "Мальта", Capital = "Валлетта", Area = 316, Population = 514_564, Language = "мальтийский" },
        new() { Name = "Марокко", Capital = "Рабат", Area = 710_851, Population = 37_112_080, Language = "арабский" },
        new() { Name = "Маршалловы Острова", Capital = "Маджуро", Area = 181.3f, Population = 55_000, Language = "маршалльский" },
        new() { Name = "Мексика", Capital = "Мехико", Area = 1_972_550, Population = 129_150_971, Language = "испанский" },
        new() { Name = "Микронезия", Capital = "Папикир", Area = 702, Population =104_600, Language = "английский" },
        new() { Name = "Мозамбик", Capital = "Мапуту", Area = 801_590, Population = 31_693_239, Language = "португальский" },
        new() { Name = "Молдавия", Capital = "Кишинев", Area = 33_846, Population = 2_603_813, Language = "молдавский" },
        new() { Name = "Монако", Capital = "Монако", Area = 2.027f, Population = 38_100, Language = "французский" },
        new() { Name = "Монголия", Capital = "Улан-Батор", Area = 1_564_116, Population = 3_400_948, Language = "монгольский" },
        new() { Name = "Мьянма", Capital = "Нейпьидо", Area = 678_500, Population = 52_885_223, Language = "бирманский" },
        new() { Name = "Намибия", Capital = "Виндхук", Area = 825_418, Population = 2_533_794, Language = "английский" },
        new() { Name = "Науру", Capital = "нет", Area = 21.3f, Population = 11_065, Language = "науранский" },
        new() { Name = "Непал", Capital = "Катманду", Area = 147_516, Population = 29_640_448, Language = "непали" },
        new() { Name = "Нигер", Capital = "Ниамей", Area = 1_267_000, Population = 23_470_530, Language = "французский" },
        new() { Name = "Нигерия", Capital = "Абуджа", Area = 923_768, Population = 213_000_000, Language = "английский" },
        new() { Name = "Нидерланды", Capital = "Амстердам", Area = 41_543, Population = 17_722_000, Language = "нидерландский" },
        new() { Name = "Никарагуа", Capital = "Манагуа", Area = 129_494, Population = 6_751_191, Language = "испанский" },
        new() { Name = "Новая Зеландия", Capital = "Веллингтон", Area = 268_680, Population = 4_848_477, Language = "английский" },
        new() { Name = "Норвегия", Capital = "Осло", Area = 385_207, Population = 5_425_270, Language = "норвежский" },
        new() { Name = "Объединенные Арабские Эмираты", Capital = "Абу-Даби", Area = 83_600, Population = 10_207_863, Language = "арабский" },
        new() { Name = "Оман", Capital = "Маскат", Area = 309_500, Population = 5_635_601, Language = "арабский" },
        new() { Name = "Пакистан", Capital = "Исламабад", Area = 803_940, Population = 221_180_890, Language = "урду" },
        new() { Name = "Палау", Capital = "Нгерулмуд", Area = 459, Population = 17_907, Language = "английский" },
        new() { Name = "Палестина", Capital = "Рамалла", Area = 6_020, Population = 5_227_193, Language = "арабский" },
        new() { Name = "Панама", Capital = "Панама", Area = 78_200, Population = 4_252_620, Language = "испанский" },
        new() { Name = "Папуа-Новая Гвинеия", Capital = "Порт-Норсби", Area = 462_840, Population = 9_134_073, Language = "английский" },
        new() { Name = "Парагвай", Capital = "Асунсьон", Area = 406_752, Population = 7_252_672, Language = "испанский" },
        new() { Name = "Перу", Capital = "Лима", Area = 1_285_216, Population = 32_162_184, Language = "испанский" },
        new() { Name = "Польша", Capital = "Варшава", Area = 312_679, Population = 38_244_000, Language = "польский" },
        new() { Name = "Португалия", Capital = "Лиссабон", Area = 92_255.61f, Population = 10_347_892, Language = "португальский" },
        new() { Name = "Россия", Capital = "Москва", Area = 17_125_191, Population = 146_424_729, Language = "русский" },
        new() { Name = "Руанда", Capital = "Кигали", Area = 26_338, Population = 12_943_132, Language = "руанда" },
        new() { Name = "Румыния", Capital = "Бухарест", Area = 238_391, Population = 19_038_098, Language = "румынский" },
        new() { Name = "Сальвадор", Capital = "Сан-Сальвадор", Area = 21_400, Population = 6_460_000, Language = "испанский" },
        new() { Name = "Самоа", Capital = "Алиа", Area = 2832, Population = 187_820, Language = "самоанский" },
        new() { Name = "Сан-Марино", Capital = "Сан-Марино", Area = 61, Population = 33_627, Language = "итальянский" },
        new() { Name = "Сан-Томе и Принсипи", Capital = "Сан-ТОме", Area = 1001, Population = 163_000, Language = "португальский" },
        new() { Name = "Саудовская Аравия", Capital = "Эр-Рияд", Area = 2_149_690, Population = 34_218_169, Language = "арабский" },
        new() { Name = "Северная Македония", Capital = "Скопье", Area = 25_333, Population = 2_073_702, Language = "македонский" },
        new() { Name = "Сейшелы", Capital = "Виктория", Area = 455, Population = 90_024, Language = "французский" },
        new() { Name = "Сенегал", Capital = "Дакар", Area = 196_722, Population = 18_103_345, Language = "французский" },
        new() { Name = "Сент-Винсент и Гренадины", Capital = "Кингстаун", Area = 389, Population = 104_332, Language = "английский" },
        new() { Name = "Сент-Китс и Невис", Capital = "Бастер", Area = 261, Population = 54_341, Language = "английский" },
        new() { Name = "Сент-Люсия", Capital = "Кастри", Area = 616, Population = 165_600, Language = "английский" },
        new() { Name = "Сербия", Capital = "Белград", Area = 88_499, Population = 6_926_705, Language = "сербский" },
        new() { Name = "Сингапур", Capital = "Сингапур", Area = 734.3f, Population = 5_866_139, Language = "английский" },
        new() { Name = "Сирия", Capital = "Дамаск", Area = 185_180, Population = 16_139_840, Language = "арабский" },
        new() { Name = "Словакия", Capital = "Братислава", Area = 49_034, Population = 5_466_965, Language = "словацкий" },
        new() { Name = "Словения", Capital = "Любляна", Area = 20_273, Population = 2_085_556, Language = "словенский" },
        new() { Name = "Соединенные Штаты Америки", Capital = "Вашингтон", Area = 9_519_431, Population = 333_449_281, Language = "английский" },
        new() { Name = "Соломоновы Острова", Capital = "Хониара", Area = 28_450, Population = 515_870, Language = "английский" },
        new() { Name = "Сомали", Capital = "Могадишо", Area = 637_657, Population = 15_443_000, Language = "сомалийский" },
        new() { Name = "Судан", Capital = "Хартум", Area = 1_886_068, Population = 39_578_828, Language = "арабский" },
        new() { Name = "Суринам", Capital = "Парамарибо", Area = 163_821, Population = 575_990, Language = "нидерландский" },
        new() { Name = "Сьерра-Леоне", Capital = "Фритаун", Area = 71_740, Population = 8_093_425, Language = "английский" },
        new() { Name = "Таджикистан", Capital = "Душанбе", Area = 141_400, Population = 10_000_000, Language = "таджикский" },
        new() { Name = "Таиланд", Capital = "Бангкок", Area = 513_120, Population = 66_171_493, Language = "тайский" },
        new() { Name = "Танзания", Capital = "Додома", Area = 945_203, Population = 60_979_238, Language = "суахили" },
        new() { Name = "Того", Capital = "Ломе", Area = 56_785, Population = 7_154_237, Language = "французский" },
        new() { Name = "Тонга", Capital = "Нукуалофа", Area = 748, Population = 100_651, Language = "тонганский" },
        new() { Name = "Тринидад и Тобаго", Capital = "Порт-оф-Спейн", Area = 5128, Population = 1_218_208, Language = "английский" },
        new() { Name = "Тувалу", Capital = "Фунафути", Area = 26, Population = 11_448, Language = "тувалу" },
        new() { Name = "Тунис", Capital = "Тунис", Area = 163_610, Population = 11_722_038, Language = "арабский" },
        new() { Name = "Туркменистан", Capital = "Ашхабад", Area = 488_100, Population = 6_031_000, Language = "туркменский" },
        new() { Name = "Турция", Capital = "Анкара", Area = 783_562, Population = 84_680_273, Language = "турецкий" },
        new() { Name = "Уганда", Capital = "Кампала", Area = 236_040, Population = 44_758_809, Language = "английский" },
        new() { Name = "Узбекистан", Capital = "Ташкент", Area = 448_924, Population = 36_000_000, Language = "узбекский" },
        new() { Name = "Украина", Capital = "Киев", Area = 603_549, Population = 40_997_699, Language = "украинский" },
        new() { Name = "Уругвай", Capital = "Монтевидео", Area = 176_215, Population = 3_480_883, Language = "уругвайский" },
        new() { Name = "Фиджи", Capital = "Сува", Area = 18_274, Population = 935_974, Language = "английский" },
        new() { Name = "Филиппины", Capital = "Манила", Area = 300_000, Population = 114_597_229, Language = "английский" },
        new() { Name = "Финляндия", Capital = "Хельсинки", Area = 338_145, Population = 5_550_066, Language = "финский" },
        new() { Name = "Франция", Capital = "Париж", Area = 643_801, Population = 68_084_217, Language = "французский" },
        new() { Name = "Хорватия", Capital = "Загреб", Area = 56_594, Population = 4_188_853, Language = "хорватский" },
        new() { Name = "Центральноафриканская Республика", Capital = "Банги", Area = 622_984, Population = 4_892_749, Language = "французский" },
        new() { Name = "Чад", Capital = "Нджамена", Area = 1_284_000, Population = 17_414_108, Language = "французский" },
        new() { Name = "Черногория", Capital = "Подгорица", Area = 13_812, Population = 621_873, Language = "черногорский" },
        new() { Name = "Чехия", Capital = "Прага", Area = 78_866, Population = 10_701_777, Language = "чешский" },
        new() { Name = "Чили", Capital = "Сантьяго", Area = 756_102, Population = 18_186_770, Language = "испанский" },
        new() { Name = "Швейцария", Capital = "Берн", Area = 41_284, Population = 8_558_758, Language = "немецкий" },
        new() { Name = "Швеция", Capital = "Стокгольм", Area = 447_435, Population = 10_514_692, Language = "шведский" },
        new() { Name = "Шри-Ланка", Capital = "Шри-Джаяварденепура-Котте", Area = 65_610, Population = 21_675_648, Language = "сингальский" },
        new() { Name = "Эквадор", Capital = "Кито", Area = 283_561, Population = 18_267_203, Language = "испанский" },
        new() { Name = "Экваториальная Гвинея", Capital = "Малабо", Area = 28_051, Population = 1_403_000, Language = "испанский" },
        new() { Name = "Эритрея", Capital = "Асмэра", Area = 117_600, Population = 6_081_196, Language = "тигринья" },
        new() { Name = "Эсватини", Capital = "Мбабане", Area = 17_364, Population = 1_104_479, Language = "английский" },
        new() { Name = "Эстония", Capital = "Таллин", Area = 43_466, Population = 1_357_739, Language = "эстонский" },
        new() { Name = "Эфиопия", Capital = "Аддис-Абеба", Area = 1_104_300, Population = 120_811_390, Language = "амхарский" },
        new() { Name = "Южно-Африканская Республика", Capital = "Претория", Area = 1_219_912, Population = 54_956_900, Language = "английский" },
        new() { Name = "Южный Судан", Capital = "Джуба", Area = 644_329, Population = 12_340_000, Language = "английский" },
        new() { Name = "Ямайка", Capital = "Кингстон", Area = 10_991, Population = 2_930_050, Language = "анлийский" },
        new() { Name = "Япония", Capital = "Токио", Area = 377_976, Population = 125_309_000, Language = "японский" }
    };

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals (object? obj) => 
        obj is Country country && country.Name == Name;

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() => HashCode.Combine (Name);

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}";

    #endregion
}
