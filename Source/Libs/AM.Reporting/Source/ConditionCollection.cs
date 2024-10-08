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
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Represents a collection of highlight conditions used in the <see cref="TextObject.Highlight"/> property
    /// of the <see cref="TextObject"/>.
    /// </summary>
    public class ConditionCollection : CollectionBase, IReportSerializable
    {
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">Index of an element.</param>
        /// <returns>The element at the specified index.</returns>
        public HighlightCondition this [int index]
        {
            get => List[index] as HighlightCondition;
            set => List[index] = value;
        }

        /// <summary>
        /// Adds the specified elements to the end of this collection.
        /// </summary>
        /// <param name="range">Array of elements to add.</param>
        public void AddRange (HighlightCondition[] range)
        {
            foreach (var t in range)
            {
                Add (t);
            }
        }

        /// <summary>
        /// Adds an object to the end of this collection.
        /// </summary>
        /// <param name="value">Object to add.</param>
        /// <returns>Index of the added object.</returns>
        public int Add (HighlightCondition value)
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
        public void Insert (int index, HighlightCondition value)
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
        public void Remove (HighlightCondition value)
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
        public int IndexOf (HighlightCondition value)
        {
            return List.IndexOf (value);
        }

        /// <summary>
        /// Determines whether an element is in the collection.
        /// </summary>
        /// <param name="value">The object to locate in the collection.</param>
        /// <returns><b>true</b> if object is found in the collection; otherwise, <b>false</b>.</returns>
        public bool Contains (HighlightCondition value)
        {
            return List.Contains (value);
        }

        /// <inheritdoc/>
        public void Serialize (ReportWriter writer)
        {
            writer.ItemName = "Highlight";
            foreach (HighlightCondition c in this)
            {
                writer.Write (c);
            }
        }

        /// <inheritdoc/>
        public void Deserialize (ReportReader reader)
        {
            Clear();
            while (reader.NextItem())
            {
                var c = new HighlightCondition();
                reader.Read (c);
                Add (c);
            }
        }

        /// <summary>
        /// Copies conditions from another collection.
        /// </summary>
        /// <param name="collection">Collection to copy from.</param>
        public void Assign (ConditionCollection collection)
        {
            Clear();
            foreach (HighlightCondition condition in collection)
            {
                Add (condition.Clone());
            }
        }

        /// <inheritdoc/>
        public override bool Equals (object obj)
        {
            if (obj is not ConditionCollection collection || Count != collection.Count)
            {
                return false;
            }

            for (var i = 0; i < Count; i++)
            {
                if (!this[i].Equals (collection[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
