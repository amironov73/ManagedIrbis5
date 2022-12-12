// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* TreeGridDataCollection.cs -- коллекция данных, хранящихся в строке грида
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

namespace AM.Windows.Forms;

/// <summary>
/// Коллекция данных, хранящихся в строке грида.
/// </summary>
[Serializable]
[XmlRoot ("data")]
public sealed class TreeGridDataCollection
    : Collection<object?>,
    IXmlSerializable
{
    #region Events

    /// <summary>
    /// Событие "данные изменились".
    /// </summary>
    public event EventHandler? DataChanged;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TreeGridDataCollection()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TreeGridDataCollection
        (
            TreeGridNode node
        )
    {
        Sure.NotNull (node);

        Node = node;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление произвольного числа элементов в коллекцию.
    /// </summary>
    public void AddRange
        (
            params object?[] range
        )
    {
        Sure.NotNull (range);

        foreach (var item in range)
        {
            Add (item);
        }
    }

    /// <summary>
    /// Добавление произвольного числа элементов в коллекцию.
    /// </summary>
    public void AddRange
        (
            IEnumerable range
        )
    {
        Sure.NotNull (range);

        foreach (var item in range)
        {
            Add (item);
        }
    }

    /// <summary>
    /// Отладочный вывод в консоль.
    /// </summary>
    public void Dump()
    {
        foreach (var item in Items)
        {
            Console.WriteLine
                (
                    item is null
                        ? "(null)"
                        : $"{item.GetType()} {item}"
                );
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Чтение данных из указанного файла.
    /// </summary>
    public static TreeGridDataCollection Read
        (
            string fileName,
            TreeGridNode node
        )
    {
        Sure.FileExists (fileName);
        Sure.NotNull (node);

        var serializer = new XmlSerializer (typeof (TreeGridDataCollection));
        using var stream = File.OpenRead (fileName);
        var result = (TreeGridDataCollection) serializer.Deserialize (stream)
            .ThrowIfNull();
        result.Node = node;
        return result;
    }

    /// <summary>
    /// Сохранение данных в указанный файл.
    /// </summary>
    public void Save
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var serializer = new XmlSerializer (typeof (TreeGridDataCollection));
        using var stream = File.OpenWrite (fileName);
        serializer.Serialize (stream, this);
    }

    /// <summary>
    /// Безопасное получение элемента по его индексу.
    /// </summary>
    public object? SafeGet
        (
            int index
        )
    {
        return index >= 0 && index < Count ? this[index] : null;
    }

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
                Add (null);
            }

            this[index] = data;
        }
    }

    #endregion

    #region Private members

    internal TreeGridNode? Node;

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
            object? item
        )
    {
        Sure.NonNegative (index);

        base.InsertItem (index, item);
        Update();
    }

    /// <inheritdoc cref="Collection{T}.RemoveItem"/>
    protected override void RemoveItem
        (
            int index
        )
    {
        Sure.InRange (index, this);

        base.RemoveItem (index);
        Update();
    }

    /// <inheritdoc cref="Collection{T}.SetItem"/>
    protected override void SetItem
        (
            int index,
            object? item
        )
    {
        base.SetItem (index, item);
        Update();
    }

    /// <summary>
    /// Инвалидация для отображения произошедших изменений.
    /// </summary>
    internal void Update()
    {
        OnDataChanged();
    }

    /// <summary>
    /// Вызов события "данные изменились".
    /// </summary>
    internal void OnDataChanged()
    {
        DataChanged?.Invoke (this, EventArgs.Empty);
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
        while (reader.LocalName == "item")
        {
            var attribute = reader.GetAttribute ("isnull");
            if (!string.IsNullOrEmpty (attribute))
            {
                Add (null!);
            }
            else
            {
                var typeName = reader.GetAttribute ("type").ThrowIfNull();
                var type = Type.GetType (typeName).ThrowIfNull();
                var value = reader.ReadString();
                var result = Convert.ChangeType (value, type);
                Add (result);
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
        Sure.NotNull (writer);

        foreach (var item in Items)
        {
            if (item is null)
            {
                writer.WriteStartElement ("item");
                writer.WriteAttributeString ("isnull", "true");
                writer.WriteEndElement();
            }
            else
            {
                writer.WriteStartElement ("item");
                writer.WriteAttributeString
                    (
                        "type",
                        item.GetType().ToString()
                    );
                writer.WriteString (item.ToString());
                writer.WriteEndElement();
            }
        }
    }

    #endregion
}
