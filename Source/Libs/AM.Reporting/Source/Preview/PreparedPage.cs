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

using AM.Reporting.Engine;
using AM.Reporting.Utils;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Reporting.Preview
{
    internal class PreparedPage : IDisposable
    {
        #region Fields

        private XmlItem xmlItem;
        private PreparedPages preparedPages;
        private SizeF pageSize;
        private long tempFilePosition;
        private bool uploaded;
        private PreparedPagePosprocessor posprocessor;

        #endregion Fields

        #region Properties

        private Report Report => preparedPages.Report;

        private bool UseFileCache => Report.UseFileCache;

        public XmlItem Xml
        {
            get => xmlItem;
            set
            {
                xmlItem = value;
                value.Parent = null;
            }
        }

        public SizeF PageSize
        {
            get
            {
                if (pageSize.IsEmpty)
                {
                    ReCalcSizes();
                }

                return pageSize;
            }
            set => pageSize = value;
        }

        public int CurPosition => xmlItem.Count;

        #endregion Properties

        #region Private Methods

        private bool DoAdd (Base c, XmlItem item)
        {
            if (c == null)
            {
                return false;
            }

            var engine = Report.Engine;
            var isRunning = Report.IsRunning && engine != null;
            if (c is ReportComponentBase @base)
            {
                if (isRunning && !engine.CanPrint (@base))
                {
                    return false;
                }
            }

            item = item.Add();
            using (var writer = new FRWriter (item))
            {
                writer.SerializeTo = SerializeTo.Preview;
                writer.SaveChildren = false;
                writer.BlobStore = preparedPages.BlobStore;
                writer.Write (c);
            }

            if (isRunning)
            {
                engine.AddObjectToProcess (c, item);
            }

            if ((c.Flags & Flags.CanWriteChildren) == 0)
            {
                var childObjects = c.ChildObjects;
                foreach (Base obj in childObjects)
                {
                    DoAdd (obj, item);
                }
            }

            return true;
        }

        private Base ReadObject (Base parent, XmlItem item, bool readChildren, FRReader reader)
        {
            var objName = item.Name;

            // try to find the object in the dictionary
            var obj = preparedPages.Dictionary.GetObject (objName);

            // object not found, objName is type name
            if (obj == null)
            {
                var type = RegisteredObjects.FindType (objName);
                if (type == null)
                {
                    return null;
                }

                obj = Activator.CreateInstance (type) as Base;
            }

            obj.SetRunning (true);

            // read object's properties
            if (!item.IsNullOrEmptyProps())
            {
                // since the BlobStore is shared resource, lock it to avoid problems with multi-thread access.
                // this may happen in the html export that uses several threads to export one report.
                lock (reader.BlobStore)
                {
                    reader.Read (obj, item);
                }
            }

            if (readChildren)
            {
                for (var i = 0; i < item.Count; i++)
                {
                    ReadObject (obj, item[i], true, reader);
                }
            }

            obj.Parent = parent;
            return obj;
        }

        private void UpdateUnlimitedPage (Base obj, XmlItem item)
        {
            item.Clear();
            using (var writer = new FRWriter (item))
            {
                writer.SerializeTo = SerializeTo.Preview;
                writer.SaveChildren = false;
                writer.BlobStore = preparedPages.BlobStore;
                writer.Write (obj);
            }

            foreach (Base child in obj.ChildObjects)
            {
                UpdateUnlimitedPage (child, item.Add());
            }
        }

        #endregion Private Methods

        #region Internal Methods

        internal ReportPage ReadPage (Base parent, XmlItem item, bool readchild, FRReader reader)
        {
            var page = ReadObject (parent, item, false, reader) as ReportPage;
            if (readchild)
            {
                for (var i = 0; i < item.Count; i++)
                {
                    ReadObject (page, item[i], true, reader);
                }
            }

            return page;
        }

        internal ReportPage StartGetPage (int index)
        {
            Load();
            ReportPage page;
            using (var reader = new FRReader (null))
            {
                reader.DeserializeFrom = SerializeTo.Preview;
                reader.ReadChildren = false;
                reader.BlobStore = preparedPages.BlobStore;
                page = ReadPage (null, xmlItem, false, reader);
                if (!(page.UnlimitedHeight || page.UnlimitedWidth))
                {
                    page.Dispose();
                    page = ReadPage (null, xmlItem, true, reader);
                    page.SetReport (preparedPages.Report);
                    posprocessor = new PreparedPagePosprocessor();
                    posprocessor.Postprocess (page);
                    posprocessor = null;
                }
                else
                {
                    page.SetReport (preparedPages.Report);
                    posprocessor = new PreparedPagePosprocessor();
                    posprocessor.PostprocessUnlimited (this, page);
                }
            }

            if (page.MirrorMargins && (index + 1) % 2 == 0)
            {
                var f = page.LeftMargin;
                page.LeftMargin = page.RightMargin;
                page.RightMargin = f;
            }

            return page;
        }

        internal void EndGetPage (ReportPage page)
        {
            if (posprocessor != null)
            {
                posprocessor = null;
            }

            if (page != null)
            {
                page.Dispose();
            }

            ClearUploadedXml();
        }

        internal IEnumerable<Base> GetPageItems (ReportPage page, bool postprocess)
        {
            if (!(page.UnlimitedHeight || page.UnlimitedWidth))
            {
                foreach (Base child in page.ChildObjects)
                {
                    if (postprocess)
                    {
                        yield return child;
                    }
                    else
                    {
                        using (child)
                            yield return child;
                    }
                }
            }
            else
            {
                if (Export.ExportBase.HAVE_TO_WORK_WITH_OVERLAY)
#pragma warning disable CS0162 // Unreachable code detected
                {
                    foreach (Base child in page.ChildObjects)
#pragma warning restore CS0162 // Unreachable code detected
                    {
                        if (child is OverlayBand)
                        {
                            yield return child;
                        }
                    }
                }

                using (var reader = new FRReader (null))
                {
                    reader.DeserializeFrom = SerializeTo.Preview;
                    reader.ReadChildren = false;
                    reader.BlobStore = preparedPages.BlobStore;
                    for (var i = 0; i < xmlItem.Count; i++)
                    {
                        if (postprocess)
                        {
                            yield return ReadObject (page, xmlItem[i], true, reader);
                        }
                        else
                        {
                            using (var obj = ReadObject (page, xmlItem[i], true, reader))
                            {
                                using (var obj2 = posprocessor.PostProcessBandUnlimitedPage (obj))
                                    yield return obj2;
                            }
                        }
                    }
                }
            }
        }

        internal string GetName()
        {
            using (var reader = new FRReader (null))
            {
                reader.DeserializeFrom = SerializeTo.Preview;
                reader.ReadChildren = false;
                reader.BlobStore = preparedPages.BlobStore;
                var page = ReadObject (null, xmlItem, false, reader) as ReportPage;
                return page.Name;
            }
        }

        internal void ReCalcSizes()
        {
            var item = xmlItem;
            using (var reader = new FRReader (null, item))
            {
                reader.DeserializeFrom = SerializeTo.Preview;
                reader.BlobStore = preparedPages.BlobStore;
                reader.ReadChildren = false;

                using (var page = ReadPage (Report, item, false, reader))
                {
                    if (page.UnlimitedHeight | page.UnlimitedWidth)
                    {
                        var maxWidth = 0.0f;
                        var maxHeight = 0.0f;
                        for (var i = 0; i < item.Count; i++)
                        {
                            using (var obj = ReadObject (page, item[i], true, reader))
                            {
                                if (obj is BandBase band)
                                {
                                    var bandsHeight = band.Top + band.Height;
                                    if (maxHeight < bandsHeight)
                                    {
                                        maxHeight = bandsHeight;
                                    }

                                    var bandWidth = 0.0f;
                                    foreach (ComponentBase comp in band.Objects)
                                    {
                                        if ((comp.Anchor & AnchorStyles.Right) == 0 && comp.Dock == DockStyle.None)
                                        {
                                            bandWidth = Math.Max (bandWidth, comp.Left + comp.Width);
                                        }
                                    }

                                    if (maxWidth < bandWidth)
                                    {
                                        maxWidth = bandWidth;
                                    }
                                }
                            }
                        }

                        if (page.UnlimitedHeight)
                        {
                            page.UnlimitedHeightValue =
                                maxHeight + (page.TopMargin + page.BottomMargin) * Units.Millimeters;
                        }

                        if (page.UnlimitedWidth)
                        {
                            page.UnlimitedWidthValue =
                                maxWidth + (page.LeftMargin + page.RightMargin) * Units.Millimeters;
                        }
                    }

                    pageSize = new SizeF (page.WidthInPixels, page.HeightInPixels);

                    using (var writer = new FRWriter (item))
                    {
                        writer.SerializeTo = SerializeTo.Preview;
                        writer.SaveChildren = false;
                        writer.BlobStore = preparedPages.BlobStore;
                        writer.Write (page);
                    }
                }
            }
        }

        #endregion Internal Methods

        #region Public Methods

        public PreparedPage (ReportPage page, PreparedPages preparedPages)
        {
            this.preparedPages = preparedPages;
            xmlItem = new XmlItem();

            // page == null when we load prepared report from a file
            if (page != null)
            {
                using (var writer = new FRWriter (xmlItem))
                {
                    writer.SerializeTo = SerializeTo.Preview;
                    writer.SaveChildren = false;
                    writer.Write (page);
                }

                pageSize = new SizeF (page.WidthInPixels, page.HeightInPixels);
            }
        }

        public bool AddBand (BandBase band)
        {
            return DoAdd (band, xmlItem);
        }

        public ReportPage GetPage()
        {
            Load();
            ReportPage page;
            using (var reader = new FRReader (null))
            {
                reader.DeserializeFrom = SerializeTo.Preview;
                reader.ReadChildren = false;
                reader.BlobStore = preparedPages.BlobStore;
                page = ReadPage (null, xmlItem, true, reader);
            }

            page.SetReport (preparedPages.Report);
            new PreparedPagePosprocessor().Postprocess (page);
            ClearUploadedXml();
            return page;
        }

        public void Load()
        {
            if (UseFileCache && uploaded)
            {
                preparedPages.TempFile.Position = tempFilePosition;
                var reader = new XmlReader (preparedPages.TempFile);
                reader.Read (xmlItem);
            }
        }

        public void ClearUploadedXml()
        {
            if (UseFileCache && uploaded)
            {
                xmlItem.Clear();
            }
        }

        public void Upload()
        {
            if (UseFileCache && !uploaded)
            {
                preparedPages.TempFile.Seek (0, SeekOrigin.End);
                tempFilePosition = preparedPages.TempFile.Position;

                var writer = new XmlWriter (preparedPages.TempFile);
                writer.Write (xmlItem);

                xmlItem.Clear();
                uploaded = true;
            }
        }

        public XmlItem CutObjects (int index)
        {
            var result = new XmlItem();
            while (xmlItem.Count > index)
            {
                result.AddItem (xmlItem[index]);
            }

            return result;
        }

        public void PasteObjects (XmlItem objects, float deltaX, float deltaY)
        {
            if (objects.Count > 0)
            {
                while (objects.Count > 0)
                {
                    var obj = objects[0];

                    // shift the object's location
                    var objX = (obj.GetProp ("l") != "") ? Converter.StringToFloat (obj.GetProp ("l")) : 0;
                    var objY = (obj.GetProp ("t") != "") ? Converter.StringToFloat (obj.GetProp ("t")) : 0;
                    obj.SetProp ("l", Converter.ToString (objX + deltaX));
                    obj.SetProp ("t", Converter.ToString (objY + deltaY));

                    // add object to a page
                    xmlItem.AddItem (obj);
                }
            }
        }

        public float GetLastY()
        {
            float result = 0;

            for (var i = 0; i < xmlItem.Count; i++)
            {
                var xi = xmlItem[i];

                if (preparedPages.Dictionary.GetOriginalObject (xi.Name) is BandBase obj && !(obj is PageFooterBand) && !(obj is OverlayBand))
                {
                    var s = xi.GetProp ("t");
                    var top = (s != "") ? Converter.StringToFloat (s) : obj.Top;
                    s = xi.GetProp ("h");
                    var height = (s != "") ? Converter.StringToFloat (s) : obj.Height;

                    if (top + height > result)
                    {
                        result = top + height;
                    }
                }
            }

            return result;
        }

        public bool ContainsBand (Type bandType)
        {
            for (var i = 0; i < xmlItem.Count; i++)
            {
                var xi = xmlItem[i];

                if (preparedPages.Dictionary.GetOriginalObject (xi.Name) is BandBase obj && obj.GetType() == bandType)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsBand (string bandName)
        {
            for (var i = 0; i < xmlItem.Count; i++)
            {
                var xi = xmlItem[i];

                if (preparedPages.Dictionary.GetOriginalObject (xi.Name) is BandBase obj && obj.Name == bandName)
                {
                    return true;
                }
            }

            return false;
        }

        public void Dispose()
        {
            xmlItem.Dispose();
        }

        #endregion Public Methods
    }
}
