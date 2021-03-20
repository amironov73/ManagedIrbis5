// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftNodeCollection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.ObjectModel;
using System.Text;

using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PftNodeCollection
        : NonNullCollection<PftNode>
    {
        #region Properties

        /// <summary>
        /// Parent node.
        /// </summary>
        public PftNode? Parent { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftNodeCollection
            (
                PftNode? parent
            )
        {
            Parent = parent;
        }

        #endregion

        #region Private members

        internal void EnsureParent()
        {
            foreach (PftNode node in this)
            {
                node.Parent = Parent;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Optimize the collection.
        /// </summary>
        public void Optimize()
        {
            var array = ToArray();

            foreach (PftNode node in array)
            {
                var optimized = node.Optimize();

                if (!ReferenceEquals(node, optimized))
                {
                    if (ReferenceEquals(optimized, null))
                    {
                        Remove(node);
                    }
                    else
                    {
                        int index = IndexOf(node);
                        optimized.Parent = null;
                        SetItem(index, optimized);
                    }
                }
            }
        }

        #endregion

        #region Collection<T> members

        /// <inheritdoc cref="Collection{T}.ClearItems" />
        protected override void ClearItems()
        {
            foreach (PftNode node in this)
            {
                node.Parent = null;
            }

            base.ClearItems();
        }

        /// <inheritdoc cref="NonNullCollection{T}.InsertItem" />
        protected override void InsertItem
            (
                int index,
                PftNode item
            )
        {
            if (!ReferenceEquals(item.Parent, null))
            {
                if (!ReferenceEquals(item.Parent, Parent))
                {
                    throw new IrbisException();
                }
            }

            item.Parent = Parent;
            base.InsertItem(index, item);
        }

        /// <inheritdoc cref="NonNullCollection{T}.SetItem" />
        protected override void SetItem
            (
                int index,
                PftNode item
            )
        {
            if (!ReferenceEquals(item.Parent, null))
            {
                if (!ReferenceEquals(item.Parent, Parent))
                {
                    throw new IrbisException();
                }
            }

            if (index < Count)
            {
                PftNode previousItem = this[index];
                if (!ReferenceEquals(previousItem, item))
                {
                    previousItem.Parent = null;
                }
            }

            item.Parent = Parent;
            base.SetItem(index, item);
        }

        /// <inheritdoc cref="Collection{T}.RemoveItem" />
        protected override void RemoveItem
            (
                int index
            )
        {
            PftNode node = this[index];
            node.Parent = null;

            base.RemoveItem(index);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();
            PftUtility.NodesToText(result, this);

            return result.ToString();
        }

        #endregion
    }
}
