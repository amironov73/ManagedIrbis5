// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* RightRecord.cs -- запись в базе данных типовых прав доступа к полным текстам
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Drm;

//
// Структура БД типовых прав доступа к полным текстам
// Имя БД: RIGHT
// Каждая запись БД описывает типовое право доступа
// к полным текстам (электронным ресурсам).
// Структура записи включает в себя следующие элементы данных (поля):
//
// Идентификатор записи.
// Метка поля: 1
// Поле неповторяющееся, обязательное, содержит уникальное
// значение, инвертируется с префиксом I=
//
// Общий период действия права доступа.
// Метка поля: 2
// Поле неповторяющееся, необязательное, содержит два подполя:
// D – начальная дата периода в виде ГГГГММДД; может отсутствовать
// E – конечная дата периода в виде ГГГГММДД; может отсутствовать
//
// Право доступа.
// Метка поля: 3
// Поле повторяющееся, обязательное, содержит следующие подполя:
// A – элемент доступа; определяет данные, на основании которых
// решается вопрос о праве доступа; подполе обязательное;
// принимает значения в соответствии со справочником 3A.MNU:
// 01 – идентификатор читателя
// 02 – категория читателя
// 03 – IP-адрес клиента
// 04 – доменное имя клиента
// 05 - Факультет
// 06 - Семестр
// 07 - Специальность
//
// B – значение элемента доступа (в зависимости от значения
// подполя A); подполе обязательное; может содержать маскирующий
// символ *
// С – значение права доступа; подполе обязательное;
// принимает следующие значения (в соответствии со справочником 3C.MNU):
// 0 – доступ к полному тексту запрещен
// 1 – разрешен постраничный просмотр полного текста
// 2 – разрешен постраничный просмотр и скачивание полного текста
// F – количественное ограничение при просмотре/скачивании;
// содержит только ЧИСЛО; при отсутствии ограничений – остается пустым;
// G – единицы количественного ограничения; имеет смысл,
// если подполе F непустое; принимает значения в соответствии
// со справочником 3G.MNU:
// (пусто)  - Страницы
// 1 - Проценты
// В одной записи не может быть ограничений в разных единицах измерения!
// D – начальная дата периода доступа для данного права в виде ГГГГММДД;
// подполе необязательное; имеет смысл, если подполе С имеет
// значения 1 или 2
// E – конечная дата периода доступа для данного права в виде ГГГГММДД;
// подполе необязательное; имеет смысл, если подполе С имеет
// значения 1 или 2
//
// Описание/Название типовой записи права доступа
// Метка поля: 4
// Поле необязательное.

//
// Алгоритм формирования права доступа к полному тексту
// Право доступа к конкретному полному тексту для конкретного клиента
// решается на основе специального формата БД ЭК
// (по умолчанию - RIGHT_FT_G.PFT). Т.е., запись БД ЭК,
// соответствующая полному тексту форматируется по формату
// RIGHT_FT_G.PFT – при этом через глобальные переменные передаются
// следующие данные:
// идентификатор читателя – глобальная переменная 30
// IP-клиента – глобальная переменная 31
// доменное имя клиента – глобальная переменная 32
// Результат форматирования может принимать значения:
// 0 – доступ запрещен
// 1#NN – разрешен постраничный просмотр
// 2#NN – разрешен постраничный просмотр и скачивание
// где NN – ограничение на кол-во страниц; может отсутствовать
//
// Формат RIGHT_FT_G.PFT БД ЭК использует в качестве вложенных
// следующие форматы:
// - right2_ft_G.pft, right3_ft_G.pft, right4_ft_G.pft – БД ЭК
// - right0.pft – БД RIGHT
// - right_rid.pft, right_rkat, right_rfak, right_rsem, right_rspc – БД RDR
//

/// <summary>
/// Запись с правами доступа к ресурсам.
/// </summary>
public sealed class RightRecord
{
    #region Properties

    /// <summary>
    /// Идентификатор записи. Поле 1.
    /// </summary>
    /// <remarks>
    /// Типичное значение: "0001".
    /// </remarks>
    [Field (1)]
    [XmlAttribute ("id")]
    [JsonPropertyName ("id")]
    [DisplayName ("Идентификатор")]
    public string? Id { get; set; }

    /// <summary>
    /// Общий период действия права доступа. Поле 2.
    /// </summary>
    [Field (2)]
    [XmlElement ("period")]
    [JsonPropertyName ("period")]
    [DisplayName ("Период действия")]
    public ValidityPeriod? Period { get; set; }

    /// <summary>
    /// Права доступа. Поле 3 (повторяющееся).
    /// </summary>
    [Field (3)]
    [XmlElement ("right")]
    [JsonPropertyName ("rights")]
    [DisplayName ("Правад доступа")]
    public AccessRight[]? Rights { get; set; }

    /// <summary>
    /// Описание/название. Поле 4.
    /// </summary>
    [Field (4)]
    [XmlAttribute ("description")]
    [JsonPropertyName ("description")]
    [DisplayName ("Описание")]
    public string? Description { get; set; }

    /// <summary>
    /// Ассоциированная запись <see cref="Record"/>.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public Record? Record { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public object? UserData { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор библиографической записи.
    /// </summary>
    public static RightRecord ParseRecord
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var result = new RightRecord
        {
            Id = record.FM (1),
            Period = ValidityPeriod.ParseField (record.GetFirstField (2)),
            Rights = AccessRight.ParseRecord (record),
            Description = record.FM (4),
            Record = record
        };

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Id.ToVisibleString();
    }

    #endregion
}
