// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

/* KnownCountries.cs -- известные коды стран
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Fields;

/// <summary>
/// Известные коды стран согласно <c>str.mnu</c>.
/// </summary>
public static class KnownCountries
{
    #region Private members

    /// <summary>
    /// Коды стран согласно <c>str.mnu</c>.
    /// </summary>
    private static readonly CaseInsensitiveDictionary<string> _dictionary = new ()
    {
        ["AU"] = "Австралия",
        ["AT"] = "Австрия",
        ["AL"] = "Албания",
        ["AZ"] = "Азербайджан",
        ["AM"] = "Армения",
        ["DZ"] = "Алжир",
        ["AO"] = "Ангола",
        ["AR"] = "Аргентина",
        ["AF"] = "Афганистан",
        ["BS"] = "Багамы",
        ["BD"] = "Бангладеш",
        ["BE"] = "Бельгия",
        ["BY"] = "Беларусь",
        ["XX"] = "Бирма см. Мьянма",
        ["BG"] = "Болгария",
        ["BO"] = "Боливия",
        ["BA"] = "Босния и Герцеговина",
        ["BW"] = "Ботсвана",
        ["BR"] = "Бразилия",
        ["VA"] = "Ватикан",
        ["GB"] = "Великобритания",
        ["HU"] = "Венгрия",
        ["VE"] = "Венесуэла",
        ["VN"] = "Вьетнам",
        ["GA"] = "Габон",
        ["GH"] = "Гана",
        ["GT"] = "Гватемала",
        ["GN"] = "Гвинея",
        ["GW"] = "Гвинея-Бисау",
        ["DE"] = "Германия",
        ["XX"] = "Голландия см. Нидерланды",
        ["HN"] = "Гондурас",
        ["XX"] = "Гонконг см. Сянган",
        ["GR"] = "Греция",
        ["GE"] = "Грузия",
        ["DK"] = "Дания",
        ["DO"] = "Доминиканская Республика",
        ["EU"] = "Европейское сообщество",
        ["EG"] = "Египет",
        ["ZR"] = "Заир",
        ["ZM"] = "Замбия",
        ["IL"] = "Израиль",
        ["IN"] = "Индия",
        ["ID"] = "Индонезия",
        ["JO"] = "Иордания",
        ["IQ"] = "Ирак",
        ["IR"] = "Иран",
        ["IE"] = "Ирландия",
        ["IS"] = "Исландия",
        ["ES"] = "Испания",
        ["IT"] = "Италия",
        ["YE"] = "Йемен",
        ["KZ"] = "Казахстан",
        ["CM"] = "Камерун",
        ["KH"] = "Камбоджа",
        ["CA"] = "Канада",
        ["KE"] = "Кения",
        ["KG"] = "Киргизия",
        ["CN"] = "Китай",
        ["CO"] = "Колумбия",
        ["CG"] = "Конго",
        ["KP"] = "Корейская Народно-Демократическая Республика",
        ["KR"] = "Корея. Республика (Южная)",
        ["CR"] = "Коста-Рика",
        ["CU"] = "Куба",
        ["KW"] = "Кувейт",
        ["LV"] = "Латвия",
        ["LB"] = "Ливан",
        ["LY"] = "Ливия",
        ["LT"] = "Литва",
        ["LI"] = "Лихтенштейн",
        ["LU"] = "Люксембург",
        ["MU"] = "Маврикий",
        ["MG"] = "Мадагаскар",
        ["ME"] = "Черногория",
        ["MK"] = "Македония",
        ["MW"] = "Малави",
        ["MY"] = "Малайзия",
        ["ML"] = "Мали",
        ["MT"] = "Мальта",
        ["MA"] = "Марокко",
        ["MX"] = "Мексика",
        ["MZ"] = "Мозамбик",
        ["MD"] = "Молдавия",
        ["MC"] = "Монако",
        ["MN"] = "Монголия",
        ["MM"] = "Мьянма (Бирма)",
        ["NP"] = "Непал",
        ["NE"] = "Нигер",
        ["NG"] = "Нигерия",
        ["NL"] = "Нидерланды",
        ["NI"] = "Никарагуа",
        ["NZ"] = "Новая Зеландия",
        ["NO"] = "Норвегия",
        ["PK"] = "Пакистан",
        ["PA"] = "Панама",
        ["PY"] = "Парагвай",
        ["PE"] = "Перу",
        ["PL"] = "Польша",
        ["PT"] = "Португалия",
        ["PR"] = "Пуэрто-Рико",
        ["RU"] = "Российская Федерация",
        ["RO"] = "Румыния",
        ["RS"] = "Сербия",
        ["RW"] = "Руанда",
        ["SV"] = "Сальвадор",
        ["SA"] = "Саудовская Аравия",
        ["SC"] = "Сейшельские Острова",
        ["SN"] = "Сенегал",
        ["SG"] = "Сингапур",
        ["SY"] = "Сирия",
        ["SK"] = "Словакия",
        ["SI"] = "Словения",
        ["SU"] = "СССР",
        ["US"] = "Соединенные Штаты Америки",
        ["SO"] = "Сомали",
        ["SD"] = "Судан",
        ["SL"] = "Сьерра-Леоне",
        ["HK"] = "Сянган (Гонконг)",
        ["TJ"] = "Таджикистан",
        ["TH"] = "Таиланд",
        ["TZ"] = "Танзания",
        ["TG"] = "Того",
        ["TT"] = "Тринидад и Тобаго",
        ["TN"] = "Тунис",
        ["TM"] = "Туркменистан",
        ["TR"] = "Турция",
        ["UG"] = "Уганда",
        ["UZ"] = "Узбекистан",
        ["UA"] = "Украина",
        ["UY"] = "Уругвай",
        ["FJ"] = "Фиджи",
        ["PH"] = "Филиппины",
        ["FI"] = "Финляндия",
        ["FR"] = "Франция",
        ["HR"] = "Хорватия",
        ["CF"] = "Ценральноафриканская Республика",
        ["TD"] = "Чад",
        ["CZ"] = "Чешская Республика",
        ["CL"] = "Чили",
        ["CH"] = "Швейцария",
        ["SE"] = "Швеция",
        ["LK"] = "Шри-Ланка",
        ["EC"] = "Эквадор",
        ["EE"] = "Эстония",
        ["ET"] = "Эфиопия",
        ["YU"] = "Югославия",
        ["ZA"] = "Южно-Африканская Республика",
        ["JP"] = "Япония"
    };

    #endregion

    #region Public methods

    /// <summary>
    /// Получение списка всех известных кодов.
    /// </summary>
    public static string[] ListAllCodes()
    {
        var result = _dictionary.Keys.ToArray();
        Array.Sort (result);

        return result;
    }

    /// <summary>
    /// Получение списка всех известных названий.
    /// </summary>
    public static string[] ListAllTitles()
    {
        var result = _dictionary.Values.ToArray();
        Array.Sort (result);

        return result;
    }

    /// <summary>
    /// Раскрытие кода страны в ее название.
    /// </summary>
    /// <returns>Раскрытое название страны либо код,
    /// если раскрыть не удалось.</returns>
    public static string TranslateCode
        (
            string code
        )
    {
        if (string.IsNullOrEmpty (code))
        {
            return code;
        }

        if (_dictionary.TryGetValue (code, out var result))
        {
            return result.ThrowIfNull();
        }

        return code;
    }

    /// <summary>
    /// Сворачивание названия страны в ее код.
    /// </summary>
    /// <returns>Код страны либо исходное название,
    /// если подобрать код не удалось.</returns>
    public static string TranslateCountry
        (
            string title
        )
    {
        if (string.IsNullOrEmpty (title))
        {
            return title;
        }

        foreach (var pair in _dictionary)
        {
            if (pair.Value.SameString (title))
            {
                return pair.Key;
            }
        }

        return title;
    }

    #endregion
}
