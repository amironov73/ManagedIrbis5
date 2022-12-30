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
using System.Collections;
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    /// <summary>
    /// Represents a collection of float values.
    /// </summary>
    [TypeConverter (typeof (TypeConverters.FloatCollectionConverter))]
    public class FloatCollection : CollectionBase
    {
        /// <summary>
        /// Gets or sets the value at the specified index.
        /// </summary>
        /// <param name="index">Index of a value.</param>
        /// <returns>The value at the specified index.</returns>
        public float this [int index]
        {
            get => (float)List[index];
            set => List[index] = value;
        }

        /// <summary>
        /// Adds the specified values to the end of this collection.
        /// </summary>
        /// <param name="range"></param>
        public void AddRange (float[] range)
        {
            foreach (var t in range)
            {
                Add (t);
            }
        }

        /// <summary>
        /// Adds a value to the end of this collection.
        /// </summary>
        /// <param name="value">Value to add.</param>
        /// <returns>Index of the added value.</returns>
        public int Add (float value)
        {
            return List.Add (value);
        }

        /// <summary>
        /// Inserts a value into this collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The value to insert.</param>
        public void Insert (int index, float value)
        {
            List.Insert (index, value);
        }

        /// <summary>
        /// Removes the specified value from the collection.
        /// </summary>
        /// <param name="value">Value to remove.</param>
        public void Remove (float value)
        {
            var i = IndexOf (value);
            if (i != -1)
            {
                List.RemoveAt (i);
            }
        }

        /// <summary>
        /// Returns the zero-based index of the first occurrence of a value.
        /// </summary>
        /// <param name="value">The value to locate in the collection.</param>
        /// <returns>The zero-based index of the first occurrence of value within the entire collection, if found;
        /// otherwise, -1.</returns>
        public int IndexOf (float value)
        {
            for (var i = 0; i < Count; i++)
            {
                if (Math.Abs (this[i] - value) < 0.01)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Determines whether a value is in the collection.
        /// </summary>
        /// <param name="value">The value to locate in the collection.</param>
        /// <returns><b>true</b> if value is found in the collection; otherwise, <b>false</b>.</returns>
        public bool Contains (float value)
        {
            return IndexOf (value) != -1;
        }

        /// <summary>
        /// Copies values from another collection.
        /// </summary>
        /// <param name="source">Collection to copy from.</param>
        public void Assign (FloatCollection source)
        {
            Clear();

            foreach (float f in source)
            {
                Add (f);
            }
        }
    }
}
