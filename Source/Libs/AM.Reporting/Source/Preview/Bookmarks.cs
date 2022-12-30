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

using AM.Reporting.Utils;

using System.Collections;

#endregion

#nullable enable

namespace AM.Reporting.Preview
{
    internal class Bookmarks
    {
        private List<BookmarkItem> items;
        private List<BookmarkItem> firstPassItems;

        internal int CurPosition => items.Count;

        internal void Shift (int index, float newY)
        {
            if (index < 0 || index >= items.Count)
            {
                return;
            }

            var topY = items[index].offsetY;
            var shift = newY - topY;

            for (var i = index; i < items.Count; i++)
            {
                items[i].pageNo++;
                items[i].offsetY += shift;
            }
        }

        public void Add (string name, int pageNo, float offsetY)
        {
            var item = new BookmarkItem
            {
                name = name,
                pageNo = pageNo,
                offsetY = offsetY
            };

            items.Add (item);
        }

        public int GetPageNo (string name)
        {
            var item = Find (name);
            if (item == null)
            {
                item = Find (name, firstPassItems);
            }

            return item == null ? 0 : item.pageNo + 1;
        }

        public BookmarkItem Find (string name)
        {
            return Find (name, items);
        }

        private BookmarkItem Find (string name, List<BookmarkItem> items)
        {
            if (items == null)
            {
                return null;
            }

            foreach (var item in items)
            {
                if (item.name == name)
                {
                    return item;
                }
            }

            return null;
        }

        public void Clear()
        {
            items.Clear();
        }

        public void ClearFirstPass()
        {
            firstPassItems = items;
            items = new List<BookmarkItem>();
        }

        public void Save (XmlItem rootItem)
        {
            rootItem.Clear();
            foreach (var item in items)
            {
                var xi = rootItem.Add();
                xi.Name = "item";
                xi.SetProp ("Name", item.name);
                xi.SetProp ("Page", item.pageNo.ToString());
                xi.SetProp ("Offset", Converter.ToString (item.offsetY));
            }
        }

        public void Load (XmlItem rootItem)
        {
            Clear();
            for (var i = 0; i < rootItem.Count; i++)
            {
                var item = rootItem[i];
                Add (item.GetProp ("Name"), int.Parse (item.GetProp ("Page")),
                    (float)Converter.FromString (typeof (float), item.GetProp ("Offset")));
            }
        }

        public Bookmarks()
        {
            items = new List<BookmarkItem>();
        }


        internal class BookmarkItem
        {
            public string name;
            public int pageNo;
            public float offsetY;
        }
    }
}
