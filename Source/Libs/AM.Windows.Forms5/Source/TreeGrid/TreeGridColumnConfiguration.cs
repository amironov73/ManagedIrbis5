// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridColumnConfiguration.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    [XmlRoot("column")]
    public class TreeGridColumnConfiguration
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TreeGridColumnConfiguration"/> class.
        /// </summary>
        public TreeGridColumnConfiguration()
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TreeGridColumnConfiguration"/> class.
        /// </summary>
        /// <param name="column">The column.</param>
        public TreeGridColumnConfiguration
            (
                TreeGridColumn column
            )
        {
            Title = column.Title;
            Resizeable = column.Resizeable;
            FillFactor = column.FillFactor;
            Alignment = column.Alignment;
            ReadOnly = column.ReadOnly;
            Width = column.Width;
            DataIndex = column.DataIndex;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the type of the column.
        /// </summary>
        /// <value>The type of the column.</value>
        [XmlAttribute("type")]
        public string ColumnType { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [XmlAttribute("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="TreeGridColumnConfiguration"/>
        /// is resizeable.
        /// </summary>
        /// <value><c>true</c> if resizeable; otherwise,
        /// <c>false</c>.</value>
        [XmlAttribute("resizeable")]
        public bool Resizeable { get; set; }

        /// <summary>
        /// Gets or sets the fill factor.
        /// </summary>
        /// <value>The fill factor.</value>
        [XmlAttribute("fill-factor")]
        public int FillFactor { get; set; }

        /// <summary>
        /// Gets or sets the alignment.
        /// </summary>
        /// <value>The alignment.</value>
        [XmlAttribute("alignment")]
        public TreeGridAlignment Alignment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise,
        /// <c>false</c>.</value>
        [XmlAttribute("read-only")]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        [XmlAttribute("width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the index of the data.
        /// </summary>
        /// <value>The index of the data.</value>
        [XmlAttribute("data-index")]
        public int DataIndex { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Saves the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void Save ( string fileName )
        {
            XmlSerializer serializer
                = new XmlSerializer
                    (
                        typeof(TreeGridColumnConfiguration)
                    );
            using (Stream stream = File.Create(fileName))
            {
                serializer.Serialize(stream,this);
            }
        }

        /// <summary>
        /// Loads the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static TreeGridColumnConfiguration Load
            (
                string fileName
            )
        {
            XmlSerializer serializer
                = new XmlSerializer
                    (
                        typeof(TreeGridColumnConfiguration)
                    );
            using (Stream stream = File.OpenRead(fileName))
            {
                return (TreeGridColumnConfiguration) serializer
                    .Deserialize(stream);
            }
        }

        /// <summary>
        /// Creates the column.
        /// </summary>
        /// <returns></returns>
        public TreeGridColumn CreateColumn ()
        {
            Type columnType = Type.GetType(ColumnType);
            TreeGridColumn result = (TreeGridColumn)
                Activator.CreateInstance(columnType);

            result.Title = Title;
            result.Resizeable = Resizeable;
            result.FillFactor = FillFactor;
            result.Alignment = Alignment;
            result.ReadOnly = ReadOnly;
            result.Width = Width;
            result.DataIndex = DataIndex;

            return result;
        }

        #endregion
    }
}
