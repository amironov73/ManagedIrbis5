// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    /// <summary>
    /// Represents a collection of AM.Reporting base objects.
    /// </summary>
    public class FRCollectionBase : CollectionBase
    {
        /// <summary>
        /// Gets an owner of this collection.
        /// </summary>
        public Base Owner { get; }

        /// <summary>
        /// Adds the specified elements to the end of this collection.
        /// </summary>
        /// <param name="range">Range of elements.</param>
        public void AddRange (Base[] range)
        {
            foreach (var c in range)
            {
                Add (c);
            }
        }

        /// <summary>
        /// Adds the specified elements to the end of this collection.
        /// </summary>
        /// <param name="range">Collection of elements.</param>
        public void AddRange (ObjectCollection range)
        {
            foreach (Base c in range)
            {
                Add (c);
            }
        }

        /// <summary>
        /// Adds an object to the end of this collection.
        /// </summary>
        /// <param name="value">Object to add.</param>
        /// <returns>Index of the added object.</returns>
        public int Add (Base value)
        {
            if (value == null)
            {
                return -1;
            }

            return List.Add (value);
        }

        /// <summary>
        /// Inserts an object into this collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The object to insert.</param>
        public void Insert (int index, Base value)
        {
            if (value != null)
            {
                List.Insert (index, value);
            }
        }

        /// <summary>
        /// Removes the specified object from the collection.
        /// </summary>
        /// <param name="value">Object to remove.</param>
        public void Remove (Base value)
        {
            if (Contains (value))
            {
                List.Remove (value);
            }
        }

        /// <summary>
        /// Returns the zero-based index of the first occurrence of an object.
        /// </summary>
        /// <param name="value">The object to locate in the collection.</param>
        /// <returns>The zero-based index of the first occurrence of value within the entire collection, if found;
        /// otherwise, -1.</returns>
        public int IndexOf (Base value)
        {
            return List.IndexOf (value);
        }

        /// <summary>
        /// Determines whether an element is in the collection.
        /// </summary>
        /// <param name="value">The object to locate in the collection.</param>
        /// <returns><b>true</b> if object is found in the collection; otherwise, <b>false</b>.</returns>
        public bool Contains (Base value)
        {
            return List.Contains (value);
        }

        /// <summary>
        /// Returns an array of collection items.
        /// </summary>
        /// <returns></returns>
        public object[] ToArray()
        {
            return InnerList.ToArray();
        }

        /// <summary>
        /// Determines whether two collections are equal.
        /// </summary>
        /// <param name="list">The collection to compare with.</param>
        /// <returns><b>true</b> if collections are equal; <b>false</b> otherwise.</returns>
        public bool Equals (FRCollectionBase list)
        {
            var result = Count == list.Count;
            if (result)
            {
                for (var i = 0; i < list.Count; i++)
                    if (List[i] != list.List[i])
                    {
                        result = false;
                        break;
                    }
            }

            return result;
        }

        /// <summary>
        /// Copies the content to another collection.
        /// </summary>
        /// <param name="list">The collection to copy to.</param>
        public void CopyTo (FRCollectionBase list)
        {
            list.Clear();
            for (var i = 0; i < Count; i++)
                list.Add (List[i] as Base);
        }

        /// <inheritdoc/>
        protected override void OnInsert (int index, object value)
        {
            if (Owner != null)
            {
                var c = value as Base;
                c.Parent = null;
                c.SetParent (Owner);
            }
        }

        /// <inheritdoc/>
        protected override void OnRemove (int index, object value)
        {
            if (Owner != null)
            {
                (value as Base).SetParent (null);
            }
        }

        /// <inheritdoc/>
        protected override void OnClear()
        {
            if (Owner != null)
            {
                while (Count > 0)
                {
                    (List[0] as Base).Dispose();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <b>FRCollectionBase</b> class with default settings.
        /// </summary>
        public FRCollectionBase() : this (null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <b>FRCollectionBase</b> class with specified owner.
        /// </summary>
        /// <param name="owner">The owner of this collection.</param>
        public FRCollectionBase (Base owner)
        {
            this.Owner = owner;
        }
    }
}
