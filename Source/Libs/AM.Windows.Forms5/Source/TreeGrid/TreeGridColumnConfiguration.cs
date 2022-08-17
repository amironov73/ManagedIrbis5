// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TreeGridColumnConfiguration.cs -- настройки колонок грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Настройки колонок грида.
/// </summary>
[Serializable]
[XmlRoot ("column")]
public class TreeGridColumnConfiguration
{
    #region Properties

    /// <summary>
    /// Тип колонки.
    /// </summary>
    [XmlAttribute ("type")]
    [JsonPropertyName ("type")]
    [DisplayName ("Тип")]
    [Description ("Тип колонки")]
    public string? ColumnType { get; set; }

    /// <summary>
    /// Заголовок колонки.
    /// </summary>
    [XmlAttribute ("title")]
    [JsonPropertyName ("title")]
    [DisplayName ("Заголовок")]
    [Description ("Заголовок колонки")]
    public string? Title { get; set; }

    /// <summary>
    /// Пользователь может менять ширину колонки?
    /// </summary>
    [XmlAttribute ("resizeable")]
    [JsonPropertyName ("resizeable")]
    [DisplayName ("Изменяемый размер")]
    [Description ("Пользователь может менять ширину колонки")]
    public bool Resizeable { get; set; }

    /// <summary>
    /// Фактор заполнения.
    /// </summary>
    [XmlAttribute ("fill-factor")]
    [JsonPropertyName ("fillFactor")]
    [DisplayName ("Фактор заполнения")]
    [Description ("Фактор заполнения")]
    public int FillFactor { get; set; }

    /// <summary>
    /// Выравнивание данных в колонке.
    /// </summary>
    [XmlAttribute ("alignment")]
    [JsonPropertyName ("alignment")]
    [DisplayName ("Выравнивание")]
    [Description ("Выравнивание данных в колонке")]
    public TreeGridAlignment Alignment { get; set; }

    /// <summary>
    /// Колонка только для чтения?
    /// </summary>
    [XmlAttribute ("read-only")]
    [JsonPropertyName ("readOnly")]
    [DisplayName ("Только для чтения")]
    [Description ("Колонка только для чтения")]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Ширина колонки.
    /// </summary>
    [XmlAttribute ("width")]
    [JsonPropertyName ("width")]
    [DisplayName ("Ширина")]
    [Description ("Ширина колонки")]
    public int Width { get; set; }

    /// <summary>
    /// Индекс данных в массиве, образующем строку грида.
    /// </summary>
    [XmlAttribute ("data-index")]
    [JsonPropertyName ("dataIndex")]
    [DisplayName ("Индекс")]
    [Description ("Индекс в массиве")]
    public int DataIndex { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TreeGridColumnConfiguration()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TreeGridColumnConfiguration
        (
            TreeGridColumn column
        )
    {
        Sure.NotNull (column);

        Title = column.Title;
        Resizeable = column.Resizeable;
        FillFactor = column.FillFactor;
        Alignment = column.Alignment;
        ReadOnly = column.ReadOnly;
        Width = column.Width;
        DataIndex = column.DataIndex;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Сохранение настроек колонки в указанный файл.
    /// </summary>
    public void Save
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var serializer = new XmlSerializer (typeof (TreeGridColumnConfiguration));
        using var stream = File.Create (fileName);
        serializer.Serialize (stream, this);
    }

    /// <summary>
    /// Загрузка настроек колонки из указанного файла.
    /// </summary>
    public static TreeGridColumnConfiguration Load
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var serializer = new XmlSerializer (typeof (TreeGridColumnConfiguration));
        using var stream = File.OpenRead (fileName);
        return (TreeGridColumnConfiguration) serializer
            .Deserialize (stream).ThrowIfNull();
    }

    /// <summary>
    /// Создает колонку с текущими настройками.
    /// </summary>
    public TreeGridColumn CreateColumn()
    {
        var columnType = Type.GetType (ColumnType.ThrowIfNull()).ThrowIfNull();
        var result = (TreeGridColumn)Activator.CreateInstance (columnType).ThrowIfNull();

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
    public TreeGridColumn CreateColumn<T>()
        where T : TreeGridColumn, new()
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
}
