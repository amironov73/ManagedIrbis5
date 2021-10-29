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
        /// Конструктор
        /// </summary>
        /// <param name="grid">Грид, которому принадледат колонки</param>
        public TreeGridColumnCollection (TreeGrid grid) => _grid = grid;

        #endregion

        #region Private members

        private readonly TreeGrid _grid;

        /// <summary>
        ///
        /// </summary>
        protected internal void Update()
        {
            _grid.UpdateState();
        }

        /// <inheritdoc cref="Collection{T}.ClearItems"/>
        protected override void ClearItems()
        {
            base.ClearItems();
            Update();
        }

        /// <inheritdoc cref="Collection{T}.InsertItem"/>
        protected override void InsertItem
            (
                int index,
                TreeGridColumn item
            )
        {
            base.InsertItem(index, item);
            item._grid = Grid;
            Update();
        }

        /// <inheritdoc cref="Collection{T}.RemoveItem"/>
        protected override void RemoveItem
            (
                int index
            )
        {
            var item = this[index];
            item._grid = null;
            base.RemoveItem(index);
            Update();
        }

        /// <inheritdoc cref="Collection{T}.SetItem"/>
        protected override void SetItem
            (
                int index,
                TreeGridColumn item
            )
        {
            base.SetItem(index, item);
            item._grid = Grid;
            Update();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Грид, которому принадлежат колонки.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TreeGrid Grid => _grid;

        #endregion

        #region Public methods

        /// <summary>
        /// Добавляет колонку указанного типа с нужным заголовком.
        /// </summary>
        public TreeGridColumn Add<T>
            (
                string title
            )
             where T : TreeGridColumn, new()
        {
            var result = new T { Title = title };
            Add(result);

            return result;
        }

        /// <summary>
        /// Dumps this instance.
        /// </summary>
        public void Dump()
        {
            foreach (var item in Items)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Saves the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void Save
            (
                string fileName
            )
        {
            var serializer = new XmlSerializer(typeof(TreeGridColumnCollection));
            using var stream = File.OpenWrite(fileName);
            serializer.Serialize(stream, this);
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
            var serializer = new XmlSerializer(typeof(TreeGridColumnCollection));
            using var stream = File.OpenRead(fileName);
            var result = (TreeGridColumnCollection)serializer
                    .Deserialize(stream).ThrowIfNull();

            return result;
        }

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
            while (reader.LocalName == "column")
            {
                var typeName = reader.GetAttribute("type");
                if (!string.IsNullOrEmpty(typeName))
                {
                    var type = Type.GetType (typeName).ThrowIfNull();
                    var column = (TreeGridColumn) Activator.CreateInstance (type).ThrowIfNull();
                    ((IXmlSerializable) column).ReadXml (reader);
                    Add(column);
                }
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

    } // class TreeGridColumnCollection

} // namespace AM.Windows.Forms
