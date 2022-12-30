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

using AM.Reporting.Table;

#endregion

#nullable enable

namespace AM.Reporting.Matrix
{
    internal class MatrixHeaderItem : MatrixDescriptor
    {
        public MatrixHeaderItem Parent { get; }

        public List<MatrixHeaderItem> Items { get; }

        public object Value { get; set; }

        public int Index { get; set; }

        public int Span
        {
            get
            {
                List<MatrixHeaderItem> terminalItems = new List<MatrixHeaderItem>();
                GetTerminalItems (terminalItems);
                return terminalItems.Count;
            }
        }

        public bool IsTotal { get; set; }

        public object[] Values
        {
            get
            {
                var count = 0;
                var item = this;

                while (item.Parent != null)
                {
                    count++;
                    item = item.Parent;
                }

                object[] values = new object[count];
                item = this;
                var index = count - 1;

                while (item.Parent != null)
                {
                    values[index] = item.Value;
                    index--;
                    item = item.Parent;
                }

                return values;
            }
        }

        public int DataRowNo { get; set; }

        public bool PageBreak { get; set; }

        internal bool IsSplitted { get; set; }

        public int Find (object value, SortOrder sort)
        {
            if (Items.Count == 0)
            {
                return -1;
            }

            if (sort == SortOrder.None)
            {
                for (var i = 0; i < Items.Count; i++)
                {
                    var i1 = Items[i].Value as IComparable;

                    var result = 0;
                    if (i1 != null)
                    {
                        result = i1.CompareTo (value);
                    }
                    else if (value != null)
                    {
                        result = -1;
                    }

                    if (result == 0)
                    {
                        return i;
                    }
                }

                return ~Items.Count;
            }
            else
            {
                var header = new MatrixHeaderItem (null)
                {
                    Value = value
                };
                return Items.BinarySearch (header, new HeaderComparer (sort));
            }
        }

        public void Clear()
        {
            Items.Clear();
        }

        private void GetTerminalItems (List<MatrixHeaderItem> list)
        {
            if (Items.Count == 0 && !IsSplitted)
            {
                list.Add (this);
            }
            else
            {
                foreach (var item in Items)
                {
                    item.GetTerminalItems (list);
                }
            }
        }

        public List<MatrixHeaderItem> GetTerminalItems()
        {
            List<MatrixHeaderItem> result = new List<MatrixHeaderItem>();
            GetTerminalItems (result);
            return result;
        }

        public MatrixHeaderItem (MatrixHeaderItem parent)
        {
            this.Parent = parent;
            Items = new List<MatrixHeaderItem>();
            IsSplitted = false;
        }


        private class HeaderComparer : IComparer<MatrixHeaderItem>
        {
            private SortOrder sort;

            public int Compare (MatrixHeaderItem x, MatrixHeaderItem y)
            {
                var result = 0;
                var i2 = y.Value as IComparable;

                if (x.Value is IComparable i1)
                {
                    result = i1.CompareTo (i2);
                }
                else if (i2 != null)
                {
                    result = -1;
                }

                if (sort == SortOrder.Descending)
                {
                    result = -result;
                }

                return result;
            }

            public HeaderComparer (SortOrder sort)
            {
                this.sort = sort;
            }
        }
    }
}
