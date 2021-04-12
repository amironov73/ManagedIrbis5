// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridColumn.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using AM.Xml;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    [XmlRoot("column")]
    public abstract class TreeGridColumn
        : Component,
        IXmlSerializable
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TreeGridColumn"/> class.
        /// </summary>
        protected TreeGridColumn()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeGridColumn"/> class.
        /// </summary>
        /// <param name="grid">The grid.</param>
        protected TreeGridColumn(TreeGrid grid)
        {
            _width = DefaultWidth;
            _grid = grid;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [draw cell].
        /// </summary>
        public event EventHandler<TreeGridDrawCellEventArgs> DrawCell;

        #endregion

        #region Properties

        private const int DefaultWidth = 100;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [DefaultValue(null)]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                Update();
            }
        }

        /// <summary>
        /// Gets or sets the fill factor.
        /// </summary>
        /// <value>The fill factor.</value>
        [DefaultValue(0)]
        public int FillFactor
        {
            get
            {
                return _fillFactor;
            }
            set
            {
                _fillFactor = value;
                Update();
            }
        }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        [DefaultValue(null)]
        public Icon Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                Update();
            }
        }

        /// <summary>
        /// Gets or sets the alignment.
        /// </summary>
        /// <value>The alignment.</value>
        [DefaultValue(TreeGridAlignment.Near)]
        public TreeGridAlignment Alignment
        {
            get
            {
                return _alignment;
            }
            set
            {
                _alignment = value;
                Update();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
                Update();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TreeGridColumn"/> is resizeable.
        /// </summary>
        /// <value><c>true</c> if resizeable; otherwise, <c>false</c>.</value>
        [DefaultValue(false)]
        public bool Resizeable
        {
            get
            {
                return _resizeable;
            }
            set
            {
                _resizeable = value;
                Update();
            }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        [DefaultValue(DefaultWidth)]
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                _width = value;
                Update();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="TreeGridColumn"/> is editable.
        /// </summary>
        /// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
        [Browsable(false)]
        [DefaultValue(false)]
        [DesignerSerializationVisibility
            (
            DesignerSerializationVisibility.Hidden
            )]
        public virtual bool Editable
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the type of the editor.
        /// </summary>
        /// <value>The type of the editor.</value>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility
            (
            DesignerSerializationVisibility.Hidden
            )]
        public virtual Type EditorType
        {
            get { return null; }
        }


        /// <summary>
        /// Gets the left.
        /// </summary>
        /// <value>The left.</value>
        [Browsable(false)]
        [DefaultValue(0)]
        [DesignerSerializationVisibility
            (
            DesignerSerializationVisibility.Hidden
            )]
        public int Left { get { return _left; } }

        /// <summary>
        /// Gets the right.
        /// </summary>
        /// <value>The right.</value>
        [Browsable(false)]
        [DefaultValue(0)]
        [DesignerSerializationVisibility
            (
            DesignerSerializationVisibility.Hidden
            )]
        public int Right { get { return _right; } }

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>The index.</value>
        [Browsable(false)]
        [DefaultValue(0)]
        [DesignerSerializationVisibility
            (
            DesignerSerializationVisibility.Hidden
            )]
        public int Index { get { return _index; } }

        public const int DefaultDataIndex = -1;

        private int _dataIndex = DefaultDataIndex;

        /// <summary>
        /// Gets or sets the index of the data.
        /// </summary>
        /// <value>The index of the data.</value>
        [DefaultValue(DefaultDataIndex)]
        public int DataIndex
        {
            get { return _dataIndex; }
            set { _dataIndex = value; }
        }

        /// <summary>
        /// Gets the grid.
        /// </summary>
        /// <value>The grid.</value>
        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>The index.</value>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility
            (
            DesignerSerializationVisibility.Hidden
            )]
        public TreeGrid Grid { get { return _grid; } }

        #endregion

        #region Private members

        private TreeGridAlignment _alignment;
        private Icon _icon;
        private string _title;
        private int _fillFactor;
        internal int _left;
        internal int _width = DefaultWidth;
        private bool _readOnly;
        private bool _resizeable;
        internal int _right;
        internal int _index;
        internal TreeGrid _grid;


        protected internal virtual void OnDrawHeader
            (
                TreeGridDrawColumnHeaderEventArgs args
            )
        {
            args.DrawBackground();
            args.DrawText();
        }

        /// <summary>
        /// Raises the <see cref="DrawCell"/> event.
        /// </summary>
        /// <param name="args">The <see cref="TreeGridDrawCellEventArgs"/> instance containing the event data.</param>
        protected internal virtual void OnDrawCell
            (
                TreeGridDrawCellEventArgs args
            )
        {
            args.Node.OnDrawCell(args);

            EventHandler<TreeGridDrawCellEventArgs> handler = DrawCell;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        protected internal void Update()
        {
            if (Grid != null)
            {
                Grid.Update();
            }
        }

        protected internal virtual void OnMouseClick
            (
                TreeGridMouseEventArgs args
            )
        {
        }

        protected internal virtual void OnMouseDoubleClick
            (
                TreeGridMouseEventArgs args
            )
        {
            TreeGridNode node = args.Node;
            if (node.Enabled)
            {
                if (Editable && !ReadOnly)
                {
                    string initialValue = Grid.GetInitialValue
                        (
                            node,
                            this
                        );
                    if (!Grid.BeginEdit(Index, initialValue))
                    {
                        node.Expanded = !node.Expanded;
                    }
                }
                else
                {
                    node.Expanded = !node.Expanded;
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Begins the edit.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        public virtual void BeginEdit(string initialValue)
        {
            Grid.BeginEdit(Index, initialValue);
        }

        /// <summary>
        /// Ends the edit.
        /// </summary>
        /// <param name="accept">if set to <c>true</c> [accept].</param>
        public virtual void EndEdit(bool accept)
        {
            Grid.EndEdit(accept);
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents
        /// this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this
        /// instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format
                (
                    "{0} ({1})",
                    _title,
                    _width
                );
        }

        #endregion

        #region Implementation of IXmlSerializable

        /// <summary>
        /// This property is reserved, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class instead.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized. </param>
        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();

            /*

            Title = reader.GetAttribute("title");
            FillFactor = reader.GetInt32("fill-factor");
            ReadOnly = reader.GetBoolean("read-only");
            Resizeable = reader.GetBoolean("resizeable");
            Alignment = reader
                .GetEnum<TreeGridAlignment>("alignment");
            Width = reader.GetInt32("width");
            reader.Read();
            if (reader.LocalName == "icon")
            {
                string base64 = reader.ReadString();
                byte[] bytes = Convert.FromBase64String(base64);
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    Icon = new Icon(stream);
                }
                reader.Read();
                reader.Read();
            }

            */
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized. </param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("title", Title);
            writer.WriteAttributeString
                (
                    "fill-factor",
                    FillFactor.ToString()
                );
            writer.WriteAttributeString
                (
                    "read-only",
                    ReadOnly.ToString()
                );
            writer.WriteAttributeString
                (
                    "resizeable",
                    Resizeable.ToString()
                );
            writer.WriteAttributeString
                (
                    "alignment",
                    Alignment.ToString()
                );
            writer.WriteAttributeString
                (
                    "width",
                    Width.ToString()
                );
            if (Icon != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    Icon.Save(stream);
                    writer.WriteStartElement("icon");
                    byte[] bytes = stream.ToArray();
                    writer.WriteBase64(bytes, 0, bytes.Length);
                    writer.WriteEndElement();
                }
            }
        }

        #endregion
    }
}
