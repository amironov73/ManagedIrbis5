// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Palette.cs -- палитра цветов для пользовательского интерфейсов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using AM.Text;

#endregion

#nullable enable

namespace AM.Drawing;

/// <summary>
/// Палитра цветов для пользовательского интерфейса.
/// </summary>
[XmlRoot ("palette")]
[System.ComponentModel.DesignerCategory ("Code")]
public class Palette
    : Collection<Tube>,
    IXmlSerializable,
    IDisposable
{
    #region Properties

    /// <summary>
    /// Доступ к цвету палитры по его имени.
    /// </summary>
    public Tube this [string name] => _dictionary[name];

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор,
    /// </summary>
    public Palette()
    {
        _dictionary = new Dictionary<string, Tube>();
    }

    #endregion

    #region Private members

    private readonly Dictionary<string, Tube> _dictionary;

    #endregion

    #region Public methods

    /// <summary>
    /// Добавляет указанный цвет с уникальным именем.
    /// </summary>
    public Tube Add
        (
            string name,
            Color color
        )
    {
        var result = new Tube (name, color);
        Add (result);

        return result;
    }

    /// <inheritdoc cref="Collection{T}.ClearItems"/>
    protected override void ClearItems()
    {
        Dispose();
        _dictionary.Clear();
        base.ClearItems();
    }

    /// <summary>
    /// Считывание палитры из атрибутов элементов пользовательского
    /// интерфейсов.
    /// </summary>
    public void InitializeFromAttributes()
    {
        var properties = GetType()
            .GetProperties (BindingFlags.Instance | BindingFlags.Public);
        foreach (var property in properties)
        {
            var attributes = property.GetCustomAttributes
                (
                    typeof (PaletteColorAttribute),
                    true
                );
            foreach (PaletteColorAttribute attribute in attributes)
            {
                var name = property.Name;
                var color = Color.FromName (attribute.Color);
                Add (name, color);
            }
        }
    }

    /// <summary>
    /// Получение цвета по имени свойства.
    /// </summary>
    public Tube GetTubeFromProperty
        (
            [CallerMemberName] string propertyName = null!
        )
    {
        return this[propertyName];
    }

    /// <inheritdoc cref="Collection{T}.InsertItem"/>
    protected override void InsertItem
        (
            int index,
            Tube item
        )
    {
        if (_dictionary.ContainsKey (item.Name))
        {
            _dictionary.Remove (item.Name);
        }

        _dictionary.Add (item.Name, item);
        base.InsertItem (index, item);
    }

    /// <inheritdoc cref="Collection{T}.RemoveItem"/>
    protected override void RemoveItem (int index)
    {
        var item = this[index];
        _dictionary.Remove (item.Name);
        item.Dispose();
        base.RemoveItem (index);
    }

    /// <inheritdoc cref="Collection{T}.SetItem"/>
    protected override void SetItem
        (
            int index,
            Tube item
        )
    {
        if (_dictionary.ContainsKey (item.Name))
        {
            _dictionary.Remove (item.Name);

            //Remove(item.Name);
        }

        _dictionary.Add (item.Name, item);
        base.SetItem (index, item);
    }

    /// <summary>
    /// Сохранение палитры в файл с указанным именем.
    /// </summary>
    public void Save
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var serializer = new XmlSerializer (typeof (Palette));
        using var writer = new StreamWriter (fileName);
        serializer.Serialize (writer, this);
    }

    /// <summary>
    /// Чтение палитры из указанного файла.
    /// </summary>
    public static Palette Read
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var serializer = new XmlSerializer (typeof (Palette));
        using var reader = new StreamReader (fileName);
        var result = (Palette) serializer.Deserialize (reader).ThrowIfNull ();

        return result;
    }

    /// <summary>
    /// Удаление из палитры цвета с указанным именем.
    /// Если такого цвета нет, ничего не происходит.
    /// </summary>
    public void Remove
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        int index;
        for (index = 0; index < Count; index++)
        {
            if (this[index].Name == name)
            {
                break;
            }
        }

        if (index < Count)
        {
            RemoveItem (index);
        }
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        foreach (var tube in Items)
        {
            tube.Dispose();
        }
    }

    #endregion

    #region IXmlSerializable members

    /// <inheritdoc cref="IXmlSerializable.GetSchema"/>
    XmlSchema? IXmlSerializable.GetSchema()
    {
        return null;
    }

    /// <inheritdoc cref="IXmlSerializable.ReadXml"/>
    void IXmlSerializable.ReadXml (XmlReader reader)
    {
        reader.Read();
        while (true)
        {
            if (reader.Name != "tube")
            {
                break;
            }

            var tube = new Tube();
            ((IXmlSerializable)tube).ReadXml (reader);
            Add (tube);
        }
    }

    /// <inheritdoc cref="IXmlSerializable.WriteXml"/>
    void IXmlSerializable.WriteXml
        (
            XmlWriter writer
        )
    {
        foreach (var tube in Items)
        {
            writer.WriteStartElement ("tube");
            ((IXmlSerializable)tube).WriteXml (writer);
            writer.WriteEndElement();
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();

        foreach (var item in Items)
        {
            builder.Append (item);
            builder.Append (Environment.NewLine);
        }

        return builder.ReturnShared();
    }

    #endregion
}
