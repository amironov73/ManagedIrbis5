// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ForeignAgent.cs -- данные об иностранном агенте (из реестра иноагентов)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using HtmlAgilityPack;

using JetBrains.Annotations;

#endregion

namespace AM.Web;

/// <summary>
/// Данные об иностранном агенте (из реестра иноагентов).
/// </summary>
[PublicAPI]
public sealed class ForeignAgent
{
    #region Properties

    /// <summary>
    /// Категория (например, организация-СМИ-иноагент).
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// ФИО или наименование организации.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Род деятельности (для физических лиц).
    /// </summary>
    public string? Occupation { get; set; }

    /// <summary>
    /// Сведения об иностранных источниках.
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Информация об осуществлении политической деятельности
    /// и/или целенаправленном сборе сведений.
    /// </summary>
    public string? Activity { get; set; }

    /// <summary>
    /// Адрес организации.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// ИНН организации/лица.
    /// </summary>
    public string? Inn { get; set; }

    /// <summary>
    /// Номер в реестре.
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Дата включения в реестр.
    /// </summary>
    public string? DateAdded { get; set; }

    /// <summary>
    /// Дата исключения из реестра.
    /// </summary>
    public string? DateRemoved { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Некоммерческие организации, признанные иностранными агентами.
    /// </summary>
    public static ForeignAgent ParseNko (HtmlRow row) => new ()
        {
            Category = "Некоммерческая организация, признанная иностранным агентом",
            Name = row.SafeGet (1),
            Address = row.SafeGet (2),
            Inn = row.SafeGet (3),
            Number = row.SafeGet (4),
            DateAdded = row.SafeGet (5),
            DateRemoved = row.SafeGet (6)
        };

    /// <summary>
    /// Средства массовой информации, признанные иностранными агентами.
    /// </summary>
    public static ForeignAgent ParseSmi (HtmlRow row) => new ()
        {
            Category = "Средство массовой информации, признанное иностранным агентом",
            Name = row.SafeGet (1),
            DateAdded = row.SafeGet (2),
            DateRemoved = row.SafeGet (3)
        };

    /// <summary>
    /// Физические лица, признанные СМИ - иностранными агентами.
    /// </summary>
    public static ForeignAgent ParsePhysicalSmi (HtmlRow row) => new ()
        {
            Category = "Физическое лицо, признанное СМИ - иностранным агентом",
            Name = row.SafeGet (1),
            Occupation = row.SafeGet (2),
            DateAdded = row.SafeGet (3),
            DateRemoved = row.SafeGet (4)
        };

    /// <summary>
    /// Физические лица, признанные иностранными агентами.
    /// </summary>
    public static ForeignAgent ParseIndividual (HtmlRow row) => new ()
        {
            Category = "Физическое лицо, признанное иностранным агентом",
            Name = row.SafeGet (1),
            Occupation = row.SafeGet (2),
            Country = row.SafeGet (3),
            Activity = row.SafeGet (4),
            DateAdded = row.SafeGet (5)
        };

    /// <summary>
    /// Незарегистрированные общественные объединения, признанные иностранными агентами.
    /// </summary>
    public static ForeignAgent ParseNonregistered (HtmlRow row) => new ()
        {
            Category = "Незарегистрированное общественное объединение, признанное иностранным агентом",
            Name = row.SafeGet (1),
            DateAdded = row.SafeGet (2),
            Activity = row.SafeGet (3),
            Occupation = row.SafeGet (4)
        };

    /// <summary>
    /// Разбор страницы Wikipedia.
    /// </summary>
    public static IList<ForeignAgent> ParseWikiPage
        (
            string? url = null
        )
    {
        url = url?? "https://ru.wikipedia.org/w/index.php?title=%D0%A1%D0%BF%D0%B8%D1%81%D0%BE%D0%BA_%D0%B8%D0%BD%D0%BE%D1%81%D1%82%D1%80%D0%B0%D0%BD%D0%BD%D1%8B%D1%85_%D0%B0%D0%B3%D0%B5%D0%BD%D1%82%D0%BE%D0%B2_(%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D1%8F)";
        var client = new HttpClient();
        var content = client.GetStringAsync (url).GetAwaiter().GetResult();
        var document = new HtmlDocument();
        document.LoadHtml (content);
        return ParseWikiPage (document);
    }

    /// <summary>
    /// Разбор страницы Wikipedia.
    /// </summary>
    public static IList<ForeignAgent> ParseWikiPage
        (
            HtmlDocument document
        )
    {
        Sure.NotNull (document);

        var result = new List<ForeignAgent> ();
        var tables = HtmlTable.FindTables (document.DocumentNode);
        if (tables.Count > 0)
        {
            result.AddRange (tables[0].Body.Select (ParseNko));
        }

        if (tables.Count > 1)
        {
            result.AddRange (tables[1].Body.Select (ParseSmi));
        }

        if (tables.Count > 2)
        {
            result.AddRange (tables[2].Body.Select (ParsePhysicalSmi));
        }

        if (tables.Count > 3)
        {
            result.AddRange (tables[3].Body.Select (ParseIndividual));
        }

        if (tables.Count > 4)
        {
            result.AddRange (tables[4].Body.Select (ParseNonregistered));
        }

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => Name.ToVisibleString();

    #endregion
}
