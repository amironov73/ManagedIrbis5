// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridNodeCollection.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.ObjectModel;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public sealed class TreeGridNodeCollection
        : Collection<TreeGridNode>
    {
        #region Private members

        internal TreeGrid? _grid;
        internal TreeGridNode? _parent;

        #endregion

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TreeGridNodeCollection"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="parent">The parent.</param>
        public TreeGridNodeCollection
            (
                TreeGrid? grid,
                TreeGridNode? parent
            )
        {
            _grid = grid;
            _parent = parent;
        }

        public TreeGridNode Add ( string text, params object [] data )
        {
            TreeGridNode result = new TreeGridNode
                                      {
                                          Title = text,
                                      };
            result.Data.AddRange(data);
            Add(result);
            return result;
        }

        internal void _UpdateGrid ()
        {
            if (_grid != null)
            {
                _grid.UpdateState();
            }
        }

        /// <summary>
        /// Removes all elements from the
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            _UpdateGrid();
        }

        /// <summary>
        /// Inserts an element into the
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero.
        /// -or-
        /// <paramref name="index"/> is greater than
        /// <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        protected override void InsertItem
            (
            int index,
            TreeGridNode item
            )
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            item._parent = _parent;
            item._SetTreeGrid ( _grid );
            base.InsertItem(index,item);
            _UpdateGrid();
        }

        /// <summary>
        /// Removes the element at the specified index of the
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero.
        /// -or-
        /// <paramref name="index"/> is equal to or greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            _UpdateGrid();
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero.
        /// -or-
        /// <paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        protected override void SetItem(int index, TreeGridNode item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            item._parent = _parent;
            item._SetTreeGrid ( _grid );
            base.SetItem(index,item);
            _UpdateGrid();
        }
    }
}
