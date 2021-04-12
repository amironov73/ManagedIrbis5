// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridColumnCollection.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    [XmlRoot("columns")]
    public class TreeGridColumnCollection
        : Collection<TreeGridColumn>,
        IXmlSerializable
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeGridColumnCollection"/> class.
        /// </summary>
        public TreeGridColumnCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TreeGridColumnCollection"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        public TreeGridColumnCollection(TreeGrid grid)
        {
            _grid = grid;
        }

        #endregion

        #region Private members

        private readonly TreeGrid _grid;

        protected internal void Update()
        {
            if (_grid != null)
            {
                _grid.UpdateState();
            }
        }

        /// <summary>
        /// Removes all elements from the
        /// <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            Update();
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero.
        /// -or-
        /// <paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        protected override void InsertItem
            (
            int index,
            TreeGridColumn item
            )
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            base.InsertItem(index, item);
            item._grid = TreeGrid;
            Update();
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.Collection`1"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero.
        /// -or-
        /// <paramref name="index"/> is equal to or greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        protected override void RemoveItem(int index)
        {
            TreeGridColumn item = this[index];
            item._grid = null;
            base.RemoveItem(index);
            Update();
        }

        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="index"/> is less than zero.
        /// -or-
        /// <paramref name="index"/> is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"/>.
        /// </exception>
        protected override void SetItem(int index, TreeGridColumn item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            base.SetItem(index, item);
            item._grid = TreeGrid;
            Update();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the tree grid.
        /// </summary>
        /// <value>The tree grid.</value>
        [DesignerSerializationVisibility
            (DesignerSerializationVisibility.Hidden)]
        public TreeGrid TreeGrid
        {
            get
            {
                return _grid;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        public TreeGridColumn Add<T>(string title)
             where T : TreeGridColumn, new()
        {
            TreeGridColumn result = new T
                                        {
                                            Title = title
                                        };
            Add(result);
            return result;
        }

        /// <summary>
        /// Dumps this instance.
        /// </summary>
        public void Dump()
        {
            foreach (TreeGridColumn item in Items)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Saves the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void Save(string fileName)
        {
            XmlSerializer serializer
                = new XmlSerializer(typeof(TreeGridColumnCollection));
            using (Stream stream = File.OpenWrite(fileName))
            {
                serializer.Serialize(stream, this);
            }
        }

        /// <summary>
        /// Reads the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static TreeGridColumnCollection Read
            (
                string fileName
            )
        {
            XmlSerializer serializer
                = new XmlSerializer(typeof(TreeGridColumnCollection));
            using (Stream stream = File.OpenRead(fileName))
            {
                TreeGridColumnCollection result
                    = (TreeGridColumnCollection)serializer
                    .Deserialize(stream);
                return result;
            }
        }

        #endregion

        #region Implementation of IXmlSerializable

        /// <summary>
        /// This property is reserved, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class instead.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized. </param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.Read();
            while (reader.LocalName == "column")
            {
                string typeName = reader.GetAttribute("type");
                if (!string.IsNullOrEmpty(typeName))
                {
                    Type type = Type.GetType(typeName);
                    TreeGridColumn column = (TreeGridColumn)
                                            Activator.CreateInstance(type);
                    ((IXmlSerializable) column).ReadXml(reader);
                    Add(column);
                }
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized. </param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            foreach (TreeGridColumn item in Items)
            {
                writer.WriteStartElement("column");
                writer.WriteAttributeString
                    (
                        "type",
                        item.GetType().ToString()
                    );
                ((IXmlSerializable)item).WriteXml(writer);
                writer.WriteEndElement();
            }
        }

        #endregion
    }
}
