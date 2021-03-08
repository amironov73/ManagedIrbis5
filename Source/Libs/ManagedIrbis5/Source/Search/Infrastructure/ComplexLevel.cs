// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ComplexLevel.cs -- комплексный уровень
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Комплексный уровень.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class ComplexLevel<T>
        : ISearchTree
        where T: class, ISearchTree
    {
        #region Properties

        /// <summary>
        /// Is complex expression?
        /// </summary>
        public bool IsComplex => Items.Count > 1;

        /// <summary>
        /// Item separator.
        /// </summary>
        public string? Separator { get; }

        /// <summary>
        /// Items.
        /// </summary>
        public NonNullCollection<T> Items { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ComplexLevel
            (
                string? separator
            )
        {
            Separator = separator;
            Items = new NonNullCollection<T>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add item.
        /// </summary>
        public ComplexLevel<T> AddItem
            (
                T item
            )
        {
            Items.Add(item);

            return this;
        }

        #endregion

        #region ISearchTree members

        /// <inheritdoc cref="ISearchTree.Parent"/>
        public ISearchTree? Parent { get; set; }

        /// <inheritdoc cref="ISearchTree.Children" />
        public ISearchTree[] Children
        {
            // ReSharper disable CoVariantArrayConversion
            get { return Items.ToArray(); }
            // ReSharper restore CoVariantArrayConversion
        }

        /// <inheritdoc cref="ISearchTree.Value" />
        public string? Value => null;

        /// <inheritdoc cref="ISearchTree.Find"/>
        public virtual TermLink[] Find ( SearchContext context ) =>
            Array.Empty<TermLink>();

        /// <inheritdoc cref="ISearchTree.ReplaceChild"/>
        public void ReplaceChild
            (
                ISearchTree fromChild,
                ISearchTree? toChild
            )
        {
            T item = (T) fromChild;

            int index = Items.IndexOf(item);
            if (index < 0)
            {
                Magna.Error
                    (
                        "ComplexLevel::ReplaceChild: "
                        + "child not found: "
                        + fromChild.ToVisibleString()
                    );

                throw new KeyNotFoundException();
            }

            if (ReferenceEquals(toChild, null))
            {
                Items.RemoveAt(index);
            }
            else
            {
                Items[index] = item;
                toChild.Parent = this;
            }

            fromChild.Parent = this;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            string result = string.Join(Separator, Items);

            if (IsComplex)
            {
                result = " ( " + result + " ) ";
            }

            return result;
        }

        #endregion

    } // class ComplexLevel

} // namespace ManagedIrbis.Infrastructure
