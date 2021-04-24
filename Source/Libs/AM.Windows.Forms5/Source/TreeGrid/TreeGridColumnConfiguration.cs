// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridColumnConfiguration.cs -- настройки колонок грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Настройки колонок грида.
    /// </summary>
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
        /// Тип колонки.
        /// </summary>
        [XmlAttribute("type")]
        [JsonPropertyName("type")]
        public string? ColumnType { get; set; }

        /// <summary>
        /// Заголовок колонки.
        /// </summary>
        [XmlAttribute("title")]
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Пользователь может менять ширину колонки?
        /// </summary>
        [XmlAttribute("resizeable")]
        [JsonPropertyName("resizeable")]
        public bool Resizeable { get; set; }

        /// <summary>
        /// Фактор заполнения.
        /// </summary>
        [XmlAttribute("fill-factor")]
        [JsonPropertyName("fillFactor")]
        public int FillFactor { get; set; }

        /// <summary>
        /// Выравнивание данных в колонке.
        /// </summary>
        [XmlAttribute("alignment")]
        [JsonPropertyName("alignment")]
        public TreeGridAlignment Alignment { get; set; }

        /// <summary>
        /// Колонка только для чтения?
        /// </summary>
        [XmlAttribute("read-only")]
        [JsonPropertyName("readOnly")]
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Ширина колонки.
        /// </summary>
        [XmlAttribute("width")]
        [JsonPropertyName("width")]
        public int Width { get; set; }

        /// <summary>
        /// Индекс данных в массиве, образующем строку грида.
        /// </summary>
        [XmlAttribute("data-index")]
        [JsonPropertyName("dataIndex")]
        public int DataIndex { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Сохраняет настройки колонки в указанный файл.
        /// </summary>
        public void Save
            (
                string fileName
            )
        {
            var serializer = new XmlSerializer (typeof(TreeGridColumnConfiguration));
            using var stream = File.Create(fileName);
            serializer.Serialize(stream,this);
        }

        /// <summary>
        /// Загружает настройки колонки в указанный файл.
        /// </summary>
        public static TreeGridColumnConfiguration Load
            (
                string fileName
            )
        {
            var serializer = new XmlSerializer (typeof(TreeGridColumnConfiguration));
            using var stream = File.OpenRead(fileName);
            return (TreeGridColumnConfiguration) serializer
                .Deserialize(stream)
                .ThrowIfNull("serializer.Deserialize");
        }

        /// <summary>
        /// Создает колонку с текущими настройками.
        /// </summary>
        public TreeGridColumn CreateColumn ()
        {
            var columnType = Type.GetType(ColumnType.ThrowIfNull("ColumnType"))
                .ThrowIfNull("Type.GetType");
            var result = (TreeGridColumn) Activator.CreateInstance(columnType)
                .ThrowIfNull("Activator.CreateInstance");

            result.Title = Title;
            result.Resizeable = Resizeable;
            result.FillFactor = FillFactor;
            result.Alignment = Alignment;
            result.ReadOnly = ReadOnly;
            result.Width = Width;
            result.DataIndex = DataIndex;

            return result;
        }

        /// <summary>
        /// Создает колонку с текущими настройками.
        /// </summary>
        public TreeGridColumn CreateColumn<T> ()
            where  T: TreeGridColumn, new()
        {
            var result = Activator.CreateInstance<T>();
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

    } // class TreeGridColumnConfiguration

} // namespace AM.Windows.Forms
