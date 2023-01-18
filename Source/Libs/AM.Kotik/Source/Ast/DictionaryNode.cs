// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DictionaryNode.cs -- создание словаря вида {1: "one", 2: "two", 3: "three"}
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Создание словаря вида `{1: "one", 2: "two", 3: "three"}`.
/// </summary>
public sealed class DictionaryNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DictionaryNode
        (
            IEnumerable<KeyValueNode>? items
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

    private readonly List<KeyValueNode> _items;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic Compute
        (
            Context context
        )
    {
        var result = new BarsikDictionary();
        foreach (var item in _items)
        {
            var key = item.Key.Compute (context);
            var value = item.Value.Compute (context);
            result.Add (key!, value);
        }

        return result;
    }

    #endregion
}
