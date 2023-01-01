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

#endregion

#nullable enable

namespace AM.Reporting.Preview
{
    internal partial class SourcePages : IDisposable
    {
        #region Fields

        private readonly List<ReportPage> pages;
        private readonly PreparedPages preparedPages;

        #endregion

        #region Properties

        public int Count => pages.Count;

        public ReportPage this [int index] => pages[index];

        #endregion

        #region Private Methods

        private Base CloneObjects (Base source, Base parent)
        {
            if (source is ReportComponentBase { FlagPreviewVisible: false })
            {
                return null;
            }

            // create clone object and assign all properties from source
            var baseName = "";
            string objName;
            var clone = Activator.CreateInstance (source.GetType()) as Base;
            using (var xml = new XmlItem())
            using (var writer = new ReportWriter (xml))
            using (var reader = new FRReader (null, xml))
            {
                reader.DeserializeFrom = SerializeTo.SourcePages;
                writer.SaveChildren = false;
                writer.SerializeTo = SerializeTo.SourcePages;
                writer.Write (source, clone);
                reader.Read (clone);
            }

            clone.Name = source.Name;
            clone.OriginalComponent = source;
            source.OriginalComponent = clone;
            if (clone is ReportComponentBase componentBase)
            {
                componentBase.AssignPreviewEvents (source);
            }

            // create alias
            objName = "Page" + pages.Count.ToString() + "." + clone.Name;
            if (clone is BandBase)
            {
                baseName = "b";
            }
            else if (clone is PageBase)
            {
                baseName = "page";
                objName = "Page" + pages.Count.ToString();
            }
            else
            {
                baseName = clone.BaseName[0].ToString().ToLower();
            }

            clone.Alias = preparedPages.Dictionary.AddUnique (baseName, objName, clone);
            source.Alias = clone.Alias;

            var childObjects = source.ChildObjects;
            foreach (Base c in childObjects)
            {
                CloneObjects (c, clone);
            }

            clone.Parent = parent;
            return clone;
        }

        #endregion

        #region Public Methods

        public void Add (ReportPage page)
        {
            pages.Add (CloneObjects (page, null) as ReportPage);
        }

        public void RemoveLast()
        {
            pages.RemoveAt (pages.Count - 1);
        }

        public void Clear()
        {
            while (pages.Count > 0)
            {
                pages[0].Dispose();
                pages.RemoveAt (0);
            }
        }

        public int IndexOf (ReportPage page)
        {
            return pages.IndexOf (page);
        }

        public void ApplyWatermark (Watermark watermark)
        {
            foreach (var page in pages)
            {
                page.Watermark = watermark.Clone();
            }
        }

        public void ApplyPageSize()
        {
        }

        public void Load (XmlItem rootItem)
        {
            Clear();
            for (var i = 0; i < rootItem.Count; i++)
            {
                using (var reader = new FRReader (null, rootItem[i]))
                {
                    pages.Add (reader.Read() as ReportPage);
                }
            }
        }

        public void Save (XmlItem rootItem)
        {
            rootItem.Clear();
            for (var i = 0; i < pages.Count; i++)
            {
                using (var writer = new ReportWriter (rootItem.Add()))
                {
                    writer.Write (pages[i]);
                }
            }
        }

        public void Dispose()
        {
            Clear();
        }

        #endregion

        public SourcePages (PreparedPages preparedPages)
        {
            this.preparedPages = preparedPages;
            pages = new List<ReportPage>();
        }
    }
}
