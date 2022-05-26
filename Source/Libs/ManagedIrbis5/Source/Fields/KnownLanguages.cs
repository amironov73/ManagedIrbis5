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

/* KnownLanguages.cs -- известные коды языков
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
/// Известные коды языков согласно <c>jz.mnu</c>.
/// </summary>
public static class KnownLanguages
{
    #region Private members

    /// <summary>
    /// Коды языков согласно <c>jz.mnu</c>.
    /// </summary>
    private static readonly CaseInsensitiveDictionary<string> _dictionary = new ()
    {
        ["abk"] = "Абхазский",
        ["ava"] = "Аварский",
        ["ady"] = "Адыгейский",
        ["aze"] = "Азербайджанский",
        ["alb"] = "Албанский",
        ["ale"] = "Алеутский",
        ["ali"] = "Алюторский",
        ["amu"] = "Амурский",
        ["eng"] = "Английский",
        ["ara"] = "Арабский",
        ["arm"] = "Армянский",
        ["afr"] = "Африкаанс",
        ["bak"] = "Башкирский",
        ["bel"] = "Белорусский",
        ["bua"] = "Бурятский",
        ["bok"] = "Бухарский",
        ["bul"] = "Болгарский",
        ["hun"] = "Венгерский",
        ["vie"] = "Вьетнамский",
        ["dut"] = "Голландский",
        ["gre"] = "Греческий",
        ["geo"] = "Грузинский",
        ["dog"] = "Дагестанский",
        ["dan"] = "Датский",
        ["dog"] = "Долганский",
        ["grc"] = "Древнегреческий",
        ["heb"] = "Иврит",
        ["yid"] = "Идиш",
        ["ing"] = "Ингушский",
        ["ind"] = "Индонезийский",
        ["iri"] = "Ирландский",
        ["ice"] = "Исландский",
        ["spa"] = "Испанский",
        ["ita"] = "Итальянский",
        ["kad"] = "Кабардино-черкесcкий",
        ["cau"] = "Кавказский-другие",
        ["kaz"] = "Казахский",
        ["kal"] = "Калмыцкий",
        ["kah"] = "Карачаево-балкарский",
        ["kar"] = "Карельский",
        ["ket"] = "Кетский",
        ["chi"] = "Китайский",
        ["kor"] = "Корейский",
        ["koy"] = "Корякский",
        ["kir"] = "Киргизский",
        ["lat"] = "Латинский",
        ["lav"] = "Латышский",
        ["lez"] = "Лезгинский",
        ["lit"] = "Литовский",
        ["mac"] = "Македонский",
        ["may"] = "Малайский",
        ["vog"] = "Мансийский",
        ["chm"] = "Марийский (Черемисский) ",
        ["mau"] = "Маньчжерский",
        ["mol"] = "Молдавский",
        ["mon"] = "Монгольский",
        ["mok"] = "Мордовский",
        ["mul"] = "мультиязычное издание",
        ["und"] = "Не определено",
        ["gol"] = "Нанайский",
        ["jur"] = "Ненецкий",
        ["ger"] = "Немецкий",
        ["nga"] = "Нганасанский",
        ["nor"] = "Норвежский",
        ["oss"] = "Осетинский",
        ["oro"] = "Орочский",
        ["per"] = "Персидский",
        ["pol"] = "Польский",
        ["por"] = "Португальский",
        ["rum"] = "Румынский",
        ["rus"] = "Русский",
        ["scc"] = "Сербский",
        ["sla"] = "Словацкий",
        ["slo"] = "Словенский",
        ["taj"] = "Таджикский",
        ["tar"] = "Татарский",
        ["tyv"] = "Тувинский",
        ["tuk"] = "Туркменский",
        ["tur"] = "Турецкий",
        ["uzb"] = "Узбекский",
        ["ude"] = "Удэгейский",
        ["ukr"] = "Украинский",
        ["udm"] = "Удмуртский",
        ["vot"] = "Удмуртский (вотяцкий)",
        ["fin"] = "Финский",
        ["fiu"] = "Финно-угорский",
        ["fle"] = "Фламандский",
        ["fre"] = "Французский",
        ["khk"] = "Хакасский",
        ["ost"] = "Хантыйский (Остяцкий)",
        ["hin"] = "Хинди",
        ["scr"] = "Хорватский",
        ["cze"] = "Чешский",
        ["chv"] = "Чувашский",
        ["chk"] = "Чукотский",
        ["swe"] = "Шведский",
        ["ewe"] = "Эве",
        ["tum"] = "Эвенкийский (Тунгусский)",
        ["lau"] = "Эвенский",
        ["ene"] = "Энецкий",
        ["esk"] = "Эскимосские (другие)",
        ["kal"] = "Эскимосский",
        ["esp"] = "Эсперанто",
        ["est"] = "Эстонский",
        ["sah"] = "Якутский",
        ["jpn"] = "Японский"
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
    /// Раскрытие кода языка в его название.
    /// </summary>
    /// <returns>Раскрытое название языка либо код,
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
    /// Сворачивание названия языка в его код.
    /// </summary>
    /// <returns>Код языка либо исходное название,
    /// если подобрать код не удалось.</returns>
    public static string TranslateLanguage
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
