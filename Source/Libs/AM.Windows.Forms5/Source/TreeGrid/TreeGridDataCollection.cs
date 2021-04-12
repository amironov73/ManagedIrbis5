// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
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
        : Collection<object>,
        IXmlSerializable
    {
        #region Events

        /// <summary>
        /// Occurs when [data changed].
        /// </summary>
        public event EventHandler DataChanged;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TreeGridDataCollection"/> class.
        /// </summary>
        /// <remarks>This constructor is called during
        /// the XML-deserialization.</remarks>
        public TreeGridDataCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TreeGridDataCollection"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        public TreeGridDataCollection(TreeGridNode node)
        {
            Node = node;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="range">The range.</param>
        public void AddRange(params object[] range)
        {
            foreach (object item in range)
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
            foreach (object item in range)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Dumps this instance to the console.
        /// </summary>
        public void Dump ()
        {
            foreach (object item in Items)
            {
                if (item == null)
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
            XmlSerializer serializer
                = new XmlSerializer(typeof(TreeGridDataCollection));
            using (Stream stream = File.OpenRead(fileName))
            {
                TreeGridDataCollection result =
                    (TreeGridDataCollection) serializer
                                                 .Deserialize(stream);
                result.Node = node;
                return result;
            }
        }

        /// <summary>
        /// Saves to XML.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void Save ( string fileName )
        {
            XmlSerializer serializer
                = new XmlSerializer(typeof(TreeGridDataCollection));
            using (Stream stream = File.OpenWrite(fileName))
            {
                serializer.Serialize(stream,this);
            }
        }

        /// <summary>
        /// Safes the get.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public object SafeGet(int index)
        {
            return (index >= 0) && (index < Count)
                       ? this[index]
                       : null;
        }

        /// <summary>
        /// Safes the set.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="data">The data.</param>
        public void SafeSet(int index, object data)
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

        internal TreeGridNode Node;

        protected override void ClearItems()
        {
            base.ClearItems();
            Update();
        }

        protected override void InsertItem(int index, object item)
        {
            base.InsertItem(index,item);
            Update();
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            Update();
        }

        protected override void SetItem(int index, object item)
        {
            base.SetItem(index,item);
            Update();
        }

        internal void Update ()
        {
            OnDataChanged();
        }

        internal void OnDataChanged ()
        {
            EventHandler handler = DataChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
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
        /// <param name="reader">The
        /// <see cref="T:System.Xml.XmlReader"/> stream from
        /// which the object is deserialized. </param>
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.Read();
            while (reader.LocalName == "item")
            {
                string attribute = reader.GetAttribute("isnull");
                if (!string.IsNullOrEmpty(attribute))
                {
                    Add(null);
                }
                else
                {
                    string typeName = reader.GetAttribute("type");
                    if (typeName == null)
                    {
                        throw new ArgumentNullException();
                    }
                    Type type = Type.GetType(typeName);
                    string value = reader.ReadString();
                    object result = Convert.ChangeType(value, type);
                    Add(result);
                }
                reader.Read();
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The
        ///  <see cref="T:System.Xml.XmlWriter"/> stream
        /// to which the object is serialized. </param>
        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            foreach (object item in Items)
            {
                if (item == null)
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
