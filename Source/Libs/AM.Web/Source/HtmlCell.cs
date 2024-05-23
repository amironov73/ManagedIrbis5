// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* HtmlCell.cs -- ячейка в HTML-таблице
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using HtmlAgilityPack;

using JetBrains.Annotations;

#endregion

namespace AM.Web;

/// <summary>
/// Ячейка в HTML-таблице.
/// </summary>
[PublicAPI]
public sealed class HtmlCell
{
    #region Properties

    /// <summary>
    /// Узел, соответствующий ячейке.
    /// </summary>
    public HtmlNode Node { get; init; } = null!;

    /// <summary>
    /// Текст, хранящийся в ячейке.
    /// </summary>
    public string? InnerText { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор узла, соответствующего ячейке.
    /// </summary>
    public static HtmlCell Parse (HtmlNode node) => new()
        {
            Node = node,
            InnerText = node.InnerText
        };

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString () => InnerText.ToVisibleString();

    #endregion
}
