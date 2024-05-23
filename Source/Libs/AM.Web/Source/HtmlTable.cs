// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* HtmlTable.cs -- таблица в HTML-документе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Linq;

using HtmlAgilityPack;

using JetBrains.Annotations;

#endregion

namespace AM.Web;

/// <summary>
/// Таблица в HTML-документе.
/// </summary>
[PublicAPI]
public sealed class HtmlTable
{
    #region Properties

    /// <summary>
    /// Узел, соответствующий таблице.
    /// </summary>
    public HtmlNode Node { get; init; } = null!;

    /// <summary>
    /// Идентификатор таблицы.
    /// </summary>
    public string? Id { get; private set; }

    /// <summary>
    /// Класс таблицы.
    /// </summary>
    public string? Class { get; private set; }

    /// <summary>
    /// Строки, образующие заголовок таблицы.
    /// </summary>
    public List<HtmlRow> Header { get; } = new ();

    /// <summary>
    /// Строки, образующие тело таблицы.
    /// </summary>
    public List<HtmlRow> Body { get; } = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Дамп таблицы.
    /// </summary>
    public void Dump
        (
            TextWriter writer,
            bool withHeader = true,
            string separator = "\t"
        )
    {
        Sure.NotNull (writer);

        if (withHeader)
        {
            foreach (var header in Header)
            {
                header.Dump (writer, separator);
            }
        }

        foreach (var row in Body)
        {
            row.Dump (writer, separator);
        }
    }

    /// <summary>
    /// Поиск и разбор таблиц, имеющихся в документе.
    /// </summary>
    public static IList<HtmlTable> FindTables
        (
            HtmlNode node
        )
    {
        Sure.NotNull (node);

        return node.Descendants ("table")
            .Select (Parse)
            .ToList();
    }

    /// <summary>
    /// Разбор узла, соответствующего таблице.
    /// </summary>
    public static HtmlTable Parse
        (
            HtmlNode node
        )
    {
        Sure.NotNull (node);

        var result = new HtmlTable
        {
            Node = node,
            Id = node.GetAttributeValue ("id", null),
            Class = node.GetAttributeValue ("class", null)
        };

        var rows = node.Descendants ("th");
        foreach (var rowNode in rows)
        {
            var row = HtmlRow.Parse (rowNode);
            result.Header.Add (row);
        }

        rows = node.Descendants ("tr");
        foreach (var rowNode in rows)
        {
            var row = HtmlRow.Parse (rowNode);
            result.Body.Add (row);
        }

        return result;
    }

    #endregion
}
