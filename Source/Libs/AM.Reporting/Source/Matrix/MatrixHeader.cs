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
using System.Text;
using System.Collections;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Matrix
{
    /// <summary>
    /// Represents a collection of matrix header descriptors used in the <see cref="MatrixObject"/>.
    /// </summary>
    public class MatrixHeader : CollectionBase, IFRSerializable
    {
        private int nextIndex;

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">Index of an element.</param>
        /// <returns>The element at the specified index.</returns>
        public MatrixHeaderDescriptor this [int index]
        {
            get => List[index] as MatrixHeaderDescriptor;
            set => List[index] = value;
        }

        internal MatrixHeaderItem RootItem { get; }

        internal string Name { get; set; }

        /// <summary>
        /// Adds the specified descriptors to the end of this collection.
        /// </summary>
        /// <param name="range">Array of descriptors to add.</param>
        public void AddRange (MatrixHeaderDescriptor[] range)
        {
            foreach (var s in range)
            {
                Add (s);
            }
        }

        /// <summary>
        /// Adds a descriptor to the end of this collection.
        /// </summary>
        /// <param name="value">Descriptor to add.</param>
        /// <returns>Index of the added descriptor.</returns>
        public int Add (MatrixHeaderDescriptor value)
        {
            return List.Add (value);
        }

        /// <summary>
        /// Inserts a descriptor into this collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The descriptor to insert.</param>
        public void Insert (int index, MatrixHeaderDescriptor value)
        {
            List.Insert (index, value);
        }

        /// <summary>
        /// Removes the specified descriptor from the collection.
        /// </summary>
        /// <param name="value">Descriptor to remove.</param>
        public void Remove (MatrixHeaderDescriptor value)
        {
            var i = IndexOf (value);
            if (i != -1)
            {
                List.RemoveAt (i);
            }
        }

        /// <summary>
        /// Returns the zero-based index of the first occurrence of a descriptor.
        /// </summary>
        /// <param name="value">The descriptor to locate in the collection.</param>
        /// <returns>The zero-based index of the first occurrence of descriptor within
        /// the entire collection, if found; otherwise, -1.</returns>
        public int IndexOf (MatrixHeaderDescriptor value)
        {
            return List.IndexOf (value);
        }

        /// <summary>
        /// Determines whether a descriptor is in the collection.
        /// </summary>
        /// <param name="value">The descriptor to locate in the collection.</param>
        /// <returns><b>true</b> if descriptor is found in the collection; otherwise, <b>false</b>.</returns>
        public bool Contains (MatrixHeaderDescriptor value)
        {
            return List.Contains (value);
        }

        /// <summary>
        /// Copies the elements of this collection to a new array.
        /// </summary>
        /// <returns>An array containing copies of this collection elements. </returns>
        public MatrixHeaderDescriptor[] ToArray()
        {
            MatrixHeaderDescriptor[] result = new MatrixHeaderDescriptor[Count];
            for (var i = 0; i < Count; i++)
            {
                result[i] = this[i];
            }

            return result;
        }

        /// <summary>
        /// Gets the list of indices of terminal items of this header.
        /// </summary>
        /// <returns>The list of indices.</returns>
        public int[] GetTerminalIndices()
        {
            return GetTerminalIndices (RootItem);
        }

        /// <summary>
        /// Gets the list of indices of terminal items of the header with specified address.
        /// </summary>
        /// <param name="address">The address of a header.</param>
        /// <returns>The list of indices.</returns>
        public int[] GetTerminalIndices (object[] address)
        {
            var rootItem = Find (address, false, 0);
            return GetTerminalIndices (rootItem);
        }

        private int[] GetTerminalIndices (MatrixHeaderItem rootItem)
        {
            List<MatrixHeaderItem> terminalItems = rootItem.GetTerminalItems();
            var result = new int[terminalItems.Count];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = terminalItems[i].Index;
            }

            return result;
        }

        /// <summary>
        /// Removes a header item with the address specified.
        /// </summary>
        /// <param name="address">The address of a header.</param>
        public void RemoveItem (object[] address)
        {
            var item = Find (address, false, 0);
            if (item != null)
            {
                item.Parent.Items.Remove (item);
            }
        }

        /// <summary>
        /// Gets an index of header with the address specified.
        /// </summary>
        /// <param name="address">The address of a header.</param>
        /// <returns>The index of header.</returns>
        public int Find (object[] address)
        {
            var item = Find (address, false, 0);
            if (item != null)
            {
                return item.Index;
            }

            return -1;
        }

        /// <summary>
        /// Gets an index of header with the address specified. If there is no such header item, it will be created.
        /// </summary>
        /// <param name="address">The address of a header.</param>
        /// <returns>The index of header.</returns>
        public int FindOrCreate (object[] address)
        {
            var item = Find (address, true, 0);
            if (item != null)
            {
                return item.Index;
            }

            return -1;
        }

        internal MatrixHeaderItem Find (object[] address, bool create, int dataRowNo)
        {
            // Note that the top header itself does not contain a value.
            // It is used as a list of first-level headers only.
            var rootItem = RootItem;

            for (var i = 0; i < address.Length; i++)
            {
                var index = rootItem.Find (address[i], this[i].Sort);

                if (index >= 0)
                {
                    rootItem = rootItem.Items[index];
                }
                else if (create)
                {
                    // create new item if necessary.
                    var newItem = new MatrixHeaderItem (rootItem)
                    {
                        Value = address[i],
                        TemplateColumn = this[i].TemplateColumn,
                        TemplateRow = this[i].TemplateRow,
                        TemplateCell = this[i].TemplateCell,
                        DataRowNo = dataRowNo,
                        PageBreak = this[i].PageBreak
                    };

                    // index is used as a cell address in a matrix
                    if (i == address.Length - 1)
                    {
                        // create index for bottom-level header
                        newItem.Index = nextIndex;
                        nextIndex++;
                    }

                    rootItem.Items.Insert (~index, newItem);
                    rootItem = newItem;
                }
                else
                {
                    return null;
                }
            }

            return rootItem;
        }


        private void AddTotalItems (MatrixHeaderItem rootItem, int descriptorIndex, bool isTemplate)
        {
            if (descriptorIndex >= Count)
            {
                return;
            }

            foreach (var item in rootItem.Items)
            {
                AddTotalItems (item, descriptorIndex + 1, isTemplate);
            }

            if (this[descriptorIndex].Totals &&
                (isTemplate || !this[descriptorIndex].SuppressTotals || rootItem.Items.Count > 1))
            {
                var totalItem = new MatrixHeaderItem (rootItem)
                {
                    IsTotal = true,
                    Value = rootItem.Value,
                    DataRowNo = rootItem.DataRowNo,
                    TemplateColumn = this[descriptorIndex].TemplateTotalColumn,
                    TemplateRow = this[descriptorIndex].TemplateTotalRow,
                    TemplateCell = this[descriptorIndex].TemplateTotalCell,
                    Index = nextIndex
                };
                nextIndex++;

                if (this[descriptorIndex].TotalsFirst && !isTemplate)
                {
                    rootItem.Items.Insert (0, totalItem);
                }
                else
                {
                    rootItem.Items.Add (totalItem);
                }
            }
        }

        internal void AddTotalItems (bool isTemplate)
        {
            AddTotalItems (RootItem, 0, isTemplate);
        }

        internal void Reset()
        {
            RootItem.Clear();
            nextIndex = 0;
        }

        /// <inheritdoc/>
        public void Serialize (FRWriter writer)
        {
            writer.ItemName = Name;
            foreach (MatrixHeaderDescriptor d in this)
            {
                writer.Write (d);
            }
        }

        /// <inheritdoc/>
        public void Deserialize (FRReader reader)
        {
            Clear();
            while (reader.NextItem())
            {
                var d = new MatrixHeaderDescriptor();
                reader.Read (d);
                Add (d);
            }
        }

        internal MatrixHeader()
        {
            RootItem = new MatrixHeaderItem (null);
        }
    }
}
