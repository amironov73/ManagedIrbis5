// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* DataCollection.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Generic collection of some data with events.
    /// </summary>
    [Serializable]
    public sealed class DataCollection
        : Collection<object>
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DataCollection"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        public DataCollection(TreeGridNode node)
        {
            _node = node;
        }

        #endregion

        #region Private members

        internal TreeGridNode _node;

        /// <summary>
        /// Removes all elements from the
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            _UpdateNode();
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero.
        /// -or-
        /// <paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        protected override void InsertItem(int index, object item)
        {
            base.InsertItem(index,item);
            _UpdateNode();
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
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
            _UpdateNode();
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
        protected override void SetItem(int index, object item)
        {
            base.SetItem(index,item);
            _UpdateNode();
        }

        internal void _UpdateNode ()
        {
            if (Node != null)
            {
                Node._UpdateGrid();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>The node.</value>
        public TreeGridNode Node
        {
            get
            {
                return _node;
            }
        }

        /// <summary>
        /// Gets the tree grid.
        /// </summary>
        /// <value>The tree grid.</value>
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public TreeGrid TreeGrid
        {
            get
            {
                return ( _node == null )
                           ? null
                           : _node.TreeGrid;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="range">The range.</param>
        public void AddRange ( params object [] range )
        {
            foreach (object item in range)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="range">The range.</param>
        public void AddRange ( IEnumerable range )
        {
            foreach (object item in range)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Safes the get.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public object SafeGet ( int index )
        {
            return (index >= 0) && (index < Count)
                       ? this[index]
                       : null;
        }

        /// <summary>
        /// Safes the set.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="data">The data.</param>
        public void SafeSet ( int index, object data )
        {
            while (Count <= index)
            {
                Add(null);
            }
            this[index] = data;
        }

        #endregion
    }
}
