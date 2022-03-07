// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IrbisData.cs -- маппинг для таблицы в базе данных ИРНИТУ
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Irbis2istu;

/// <summary>
/// Маппинг для таблицы в базе данных ИРНИТУ.
/// </summary>
[Table ("irbisdata")]
public class IrbisData
{
    #region Properties

    /// <summary>
    /// Уникальный идентификатор - первичный ключ.
    /// </summary>
    [Identity]
    [PrimaryKey]
    [Column ("id")]
    public int Id { get; set; }

    /// <summary>
    /// Шифр в базе данных ИРБИС.
    /// </summary>
    [Column ("irbisid")]
    public string? Index { get; set; }

    /// <summary>
    /// Библиографическое описание.
    /// </summary>
    [Column ("bib_disc")]
    public string? Description { get; set; }

    /// <summary>
    /// Предметная рубрика.
    /// </summary>
    [Column ("rubrica")]
    public string? Heading { get; set; }

    /// <summary>
    /// Заглавие.
    /// </summary>
    [Column ("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Индивидуальные авторы.
    /// </summary>
    [Column ("avtors")]
    public string? Author { get; set; }

    /// <summary>
    /// Количество экземпляров.
    /// </summary>
    [Column ("cnt")]
    public int Count { get; set; }

    /// <summary>
    /// Год выхода из печати.
    /// </summary>
    [Column ("year_izd")]
    public int Year { get; set; }

    /// <summary>
    /// Ссылка на электронный экземпляр.
    /// </summary>
    [Column ("http_link")]
    public string? Link { get; set; }

    /// <summary>
    /// Тип издания: электронное или обычное.
    /// </summary>
    [Column ("izd_type")]
    public string? Type { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Column ("place")]
    public string? Place { get; set; }

    #endregion
}
