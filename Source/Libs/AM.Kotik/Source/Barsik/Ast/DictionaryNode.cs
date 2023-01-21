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

using System.IO;

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
            KeyValueNode[] items
        )
    {
        Sure.NotNull (items);
        
        _items = items;
    }

    #endregion

    #region Private members

    private readonly KeyValueNode[] _items;

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

        for (var i = 0; i < _items.Length; i++)
        {
            _items[i].DumpHierarchyItem (i.ToInvariantString(), level + 1, writer);
        }
    }

    #endregion
}
