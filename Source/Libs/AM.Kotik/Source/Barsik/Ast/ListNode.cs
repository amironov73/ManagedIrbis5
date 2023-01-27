// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ListNode.cs -- создание списка вида [1, 2, 3]
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Создание списка вида `[1, 2, 3]`.
/// </summary>
internal sealed class ListNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ListNode
        (
            IList<AtomNode> items
        )
    {
        Sure.NotNull (items);

        _items = items;
    }

    #endregion

    #region Private members

    private readonly IList<AtomNode> _items;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic Compute
        (
            Context context
        )
    {
        var result = new BarsikList();
        foreach (var item in _items)
        {
            var value = item.Compute (context);
            result.Add (value);
        }

        return result;
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer);

        for (var i = 0; i < _items.Count; i++)
        {
            _items[i].DumpHierarchyItem (i.ToInvariantString(), level + 1, writer);
        }
    }

    #endregion
}
