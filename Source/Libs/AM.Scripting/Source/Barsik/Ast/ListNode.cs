// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* ListNode.cs -- создание списка вида [1, 2, 3]
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Создание списка вида <c>[1, 2, 3]</c>.
    /// </summary>
    sealed class ListNode : AtomNode
    {
        public ListNode(IEnumerable<AtomNode>? items)
        {
            _items = new ();
            if (items is not null)
            {
                _items.AddRange (items);
            }
        }

        private readonly List<AtomNode> _items;

        public override dynamic? Compute (Context context)
        {
            var result = new BarsikList();
            foreach (var item in _items)
            {
                var value = item.Compute (context);
                result.Add (value);
            }

            return result;
        }
    }
}
