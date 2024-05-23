// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* HtmlRow.cs -- строка в HTML-таблице
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using HtmlAgilityPack;

using JetBrains.Annotations;

#endregion

namespace AM.Web;

/// <summary>
/// Строка в HTML-таблице.
/// </summary>
[PublicAPI]
public sealed class HtmlRow
{
    #region Properties

    /// <summary>
    /// Узел, соответствующий строке.
    /// </summary>
    public HtmlNode Node { get; init; } = null!;

    /// <summary>
    /// Ячейки, образующие строку.
    /// </summary>
    public List<HtmlCell> Cells { get; } = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Дамп строки.
    /// </summary>
    public void Dump
        (
            TextWriter writer,
            string separator = "\t"
        )
    {
        Sure.NotNull (writer);

        var isFirst = true;
        foreach (var cell in Cells)
        {
            if (!isFirst)
            {
                writer.Write (separator);
            }

            writer.Write (cell.InnerText);
            isFirst = false;
        }

        writer.WriteLine();
    }

    /// <summary>
    /// Разбор узла, соответствующего строке.
    /// </summary>
    public static HtmlRow Parse
        (
            HtmlNode node
        )
    {
        Sure.NotNull (node);

        var cells = node.Descendants ("td");
        var result = new HtmlRow();
        foreach (var cellNode in cells)
        {
            var cell = HtmlCell.Parse (cellNode);
            result.Cells.Add (cell);
        }

        return result;
    }

    /// <summary>
    /// Безопасный доступ к ячейке по ее индексу.
    /// </summary>
    public string? SafeGet (int index) =>
        index >= 0 && index < Cells.Count
            ? Cells[index].InnerText
            : null;

    #endregion
}
