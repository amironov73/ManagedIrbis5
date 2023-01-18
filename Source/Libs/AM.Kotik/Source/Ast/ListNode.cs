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

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Создание списка вида `[1, 2, 3]`.
/// </summary>
public sealed class ListNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ListNode
        (
            IEnumerable<ExpressionNode>? items
        )
    {
        _items = new ();
        if (items is not null)
        {
            _items.AddRange (items);
        }
    }

    #endregion

    #region Private members

    private readonly List<ExpressionNode> _items;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
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
}
