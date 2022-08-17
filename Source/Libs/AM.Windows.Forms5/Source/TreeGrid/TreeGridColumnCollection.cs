// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TreeGridColumnCollection.cs -- коллекция колонок грида
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

namespace AM.Windows.Forms;

/// <summary>
/// Коллекция колонок грида.
/// </summary>
[XmlRoot ("columns")]
public class TreeGridColumnCollection
    : Collection<TreeGridColumn>,
    IXmlSerializable
{
    #region Properties

    /// <summary>
    /// Грид, которому принадлежат колонки.
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public TreeGrid Grid { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="grid">Грид, которому принадлежат колонки</param>
    public TreeGridColumnCollection
        (
            TreeGrid grid
        )
    {
        Sure.NotNull (grid);

        Grid = grid;
    }

    #endregion

    #region Private members

    /// <summary>
    /// Инвалидация грида для отрисовки внесенных изменений.
    /// </summary>
    protected internal void Update()
    {
        Grid.UpdateState();
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
        Sure.NonNegative (index);
        Sure.NotNull (item);

        base.InsertItem (index, item);
        item._grid = Grid;
        Update();
    }

    /// <inheritdoc cref="Collection{T}.RemoveItem"/>
    protected override void RemoveItem
        (
            int index
        )
    {
        Sure.InRange (index, this);

        var item = this[index];
        item._grid = null;
        base.RemoveItem (index);
        Update();
    }

    /// <inheritdoc cref="Collection{T}.SetItem"/>
    protected override void SetItem
        (
            int index,
            TreeGridColumn item
        )
    {
        Sure.InRange (index, this);
        Sure.NotNull (item);

        base.SetItem (index, item);
        item._grid = Grid;
        Update();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавляет колонку указанного типа с нужным заголовком.
    /// </summary>
    public TreeGridColumn Add<T>
        (
            string? title
        )
        where T : TreeGridColumn, new()
    {
        var result = new T { Title = title };
        Add (result);

        return result;
    }

    /// <summary>
    /// Отладочный дамп.
    /// </summary>
    public void Dump()
    {
        foreach (var item in Items)
        {
            Console.WriteLine (item);
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Сохранение описания колонок в указанный файл.
    /// </summary>
    public void Save
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var serializer = new XmlSerializer (typeof (TreeGridColumnCollection));
        using var stream = File.OpenWrite (fileName);
        serializer.Serialize (stream, this);
    }

    /// <summary>
    /// Загрузка описания колонок из указанного файла.
    /// </summary>
    public static TreeGridColumnCollection Read
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var serializer = new XmlSerializer (typeof (TreeGridColumnCollection));
        using var stream = File.OpenRead (fileName);
        var result = (TreeGridColumnCollection)serializer
            .Deserialize (stream).ThrowIfNull();

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
        Sure.NotNull (reader);

        reader.Read();
        while (reader.LocalName == "column")
        {
            var typeName = reader.GetAttribute ("type");
            if (!string.IsNullOrEmpty (typeName))
            {
                var type = Type.GetType (typeName).ThrowIfNull();
                var column = (TreeGridColumn)Activator.CreateInstance (type).ThrowIfNull();
                ((IXmlSerializable)column).ReadXml (reader);
                Add (column);
            }
        }
    }

    /// <inheritdoc cref="IXmlSerializable.WriteXml"/>
    void IXmlSerializable.WriteXml
        (
            XmlWriter writer
        )
    {
        Sure.NotNull (writer);

        foreach (var item in Items)
        {
            writer.WriteStartElement ("column");
            writer.WriteAttributeString
                (
                    "type",
                    item.GetType().ToString()
                );
            ((IXmlSerializable)item).WriteXml (writer);
            writer.WriteEndElement();
        }
    }

    #endregion
}
