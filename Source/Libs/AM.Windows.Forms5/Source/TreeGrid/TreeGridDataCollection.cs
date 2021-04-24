// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* TreeGridDataCollection.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Generic collection of some data with events.
    /// </summary>
    [Serializable]
    [XmlRoot("data")]
    public sealed class TreeGridDataCollection
        : Collection<object?>,
        IXmlSerializable
    {
        #region Events

        /// <summary>
        /// Occurs when [data changed].
        /// </summary>
        public event EventHandler? DataChanged;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор
        /// </summary>
        public TreeGridDataCollection()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="node"></param>
        public TreeGridDataCollection
            (
                TreeGridNode node
            )
        {
            Node = node;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="range">The range.</param>
        public void AddRange(params object?[] range)
        {
            foreach (var item in range)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="range">The range.</param>
        public void AddRange(IEnumerable range)
        {
            foreach (var item in range)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Dumps this instance to the console.
        /// </summary>
        public void Dump ()
        {
            foreach (var item in Items)
            {
                if (item is null)
                {
                    Console.WriteLine("(null)");
                }
                else
                {
                    Console.WriteLine
                        (
                            "{0} {1}",
                            item.GetType(),
                            item
                        );
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Reads from XML.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static TreeGridDataCollection Read
            (
                string fileName,
                TreeGridNode node
            )
        {
            var serializer = new XmlSerializer(typeof(TreeGridDataCollection));
            using var stream = File.OpenRead(fileName);
            var result = (TreeGridDataCollection) serializer
                    .Deserialize(stream)
                    .ThrowIfNull("serializer.Deserialize");
            result.Node = node;
            return result;
        }

        /// <summary>
        /// Saves to XML.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void Save ( string fileName )
        {
            var serializer = new XmlSerializer(typeof(TreeGridDataCollection));
            using var stream = File.OpenWrite(fileName);
            serializer.Serialize(stream,this);
        }

        /// <summary>
        /// Безопасное получение элемента по его индексу.
        /// </summary>
        public object? SafeGet(int index) => index >= 0 && index < Count ? this[index] : null;

        /// <summary>
        /// Безопасное задание данных по указанному индексу.
        /// </summary>
        public void SafeSet
            (
                int index,
                object? data
            )
        {
            if (index >= 0)
            {
                while (Count <= index)
                {
                    Add(null);
                }

                this[index] = data;
            }
        }

        #endregion

        #region Private members

        internal TreeGridNode? Node;

        protected override void ClearItems()
        {
            base.ClearItems();
            Update();
        }

        protected override void InsertItem
            (
                int index,
                object? item
            )
        {
            base.InsertItem(index,item);
            Update();
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            Update();
        }

        protected override void SetItem
            (
                int index,
                object? item
            )
        {
            base.SetItem(index,item);
            Update();
        }

        internal void Update ()
        {
            OnDataChanged();
        }

        internal void OnDataChanged () => DataChanged?.Invoke(this, EventArgs.Empty);

        #endregion

        #region Implementation of IXmlSerializable

        /// <inheritdoc cref="IXmlSerializable.GetSchema"/>
        XmlSchema? IXmlSerializable.GetSchema() => null;

        /// <inheritdoc cref="IXmlSerializable.ReadXml"/>
        void IXmlSerializable.ReadXml
            (
                XmlReader reader
            )
        {
            reader.Read();
            while (reader.LocalName == "item")
            {
                var attribute = reader.GetAttribute("isnull");
                if (!string.IsNullOrEmpty(attribute))
                {
                    Add(null!);
                }
                else
                {
                    var typeName = reader.GetAttribute("type").ThrowIfNull("typeName");
                    var type = Type.GetType(typeName).ThrowIfNull("Type.GetType(typeName)");
                    var value = reader.ReadString();
                    var result = Convert.ChangeType(value, type);
                    Add(result);
                }
                reader.Read();
            }
        }

        /// <inheritdoc cref="IXmlSerializable.WriteXml"/>
        void IXmlSerializable.WriteXml
            (
                XmlWriter writer
            )
        {
            foreach (var item in Items)
            {
                if (item is null)
                {
                    writer.WriteStartElement("item");
                    writer.WriteAttributeString("isnull","true");
                    writer.WriteEndElement();
                }
                else
                {
                    writer.WriteStartElement("item");
                    writer.WriteAttributeString
                        (
                            "type",
                            item.GetType().ToString()
                        );
                    writer.WriteString(item.ToString());
                    writer.WriteEndElement();
                }
            }
        }

        #endregion
    }
}
