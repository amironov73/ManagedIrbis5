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
using System.IO;
using System.Diagnostics;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    internal class BlobStore : IDisposable
    {
        private List<BlobItem> list;
        private Dictionary<string, BlobItem> items;
        private string tempFileName;

        public int Count => list.Count;

        internal FileStream TempFile { get; private set; }

        public int Add (byte[] stream)
        {
            var item = new BlobItem (stream, this);
            list.Add (item);
            return list.Count - 1;
        }

        public int AddOrUpdate (byte[] stream, string src)
        {
            var item = new BlobItem (stream, this);
            if (!string.IsNullOrEmpty (src))
            {
                if (items.ContainsKey (src))
                {
                    return list.IndexOf (items[src]);
                }
                else
                {
                    item.Source = src;
                    items[src] = item;
                }
            }

            list.Add (item);
            return list.Count - 1;
        }

        public byte[] Get (int index)
        {
            var stream = list[index].Stream;
            return stream;
        }

        public string GetSource (int index)
        {
            return list[index].Source;
        }

        /* public Dictionary<string, byte[]> GetCache()
         {
           Dictionary<string, byte[]> result = new Dictionary<string, byte[]>();
           foreach(BlobItem item in FList)
           {
             if (!String.IsNullOrEmpty(item.Source))
               result[item.Source] = item.Stream;
           }
           return result;
         }*/

        public void Clear()
        {
            foreach (var b in list)
            {
                b.Dispose();
            }

            items.Clear();
            list.Clear();
        }

        public void LoadDestructive (XmlItem rootItem)
        {
            Clear();

            // avoid memory fragmentation when loading large amount of big blobs
            for (var i = 0; i < rootItem.Count; i++)
            {
                AddOrUpdate (Convert.FromBase64String (rootItem[i].GetProp ("Stream", false)),
                    rootItem[i].GetProp ("Source"));
                rootItem[i].ClearProps();
            }
        }

        public void Save (XmlItem rootItem)
        {
            foreach (var item in list)
            {
                var xi = rootItem.Add();
                xi.Name = "item";
                xi.SetProp ("Stream", Converter.ToXml (item.Stream));
                if (!string.IsNullOrEmpty (item.Source))
                {
                    xi.SetProp ("Source", Converter.ToXml (item.Source));
                }

                if (TempFile != null)
                {
                    item.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Clear();
            if (TempFile != null)
            {
                TempFile.Dispose();
                TempFile = null;
                File.Delete (tempFileName);
            }
        }

        public BlobStore (bool useFileCache)
        {
            list = new List<BlobItem>();
            items = new Dictionary<string, BlobItem>();
            if (useFileCache)
            {
                tempFileName = Config.CreateTempFile ("");

                // delete temp file, when it will be disposed
                TempFile = new FileStream (tempFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                    FileShare.ReadWrite, 4096, FileOptions.DeleteOnClose);
            }
        }


        private class BlobItem : IDisposable
        {
            private byte[] stream;
            private BlobStore store;
            private long tempFilePosition;
            private long tempFileSize;

            public byte[] Stream
            {
                get
                {
                    if (TempFile != null)
                    {
                        lock (TempFile)
                        {
                            TempFile.Seek (tempFilePosition, SeekOrigin.Begin);
                            stream = new byte[tempFileSize];
                            TempFile.Read (stream, 0, (int)tempFileSize);
                        }
                    }

                    return stream;
                }
            }

            /// <summary>
            /// Source of image, only for inline img tag
            /// </summary>
            public string Source { get; set; }

            public Stream TempFile => store.TempFile;

            private void ClearStream()
            {
                stream = null;
            }

            public void Dispose()
            {
                ClearStream();
            }

            public BlobItem (byte[] stream, BlobStore store)
            {
                this.store = store;
                this.stream = stream;
                Source = null;
                if (TempFile != null)
                {
                    TempFile.Seek (0, SeekOrigin.End);
                    tempFilePosition = TempFile.Position;
                    tempFileSize = stream.Length;
                    TempFile.Write (stream, 0, (int)tempFileSize);
                    TempFile.Flush();
                    ClearStream();
                }
            }
        }
    }
}
