// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Xml.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Reporting.Utils;

/// <summary>
/// Represents a xml property.
/// </summary>
public readonly struct XmlProperty
{
    /// <summary>
    /// Represents a property key.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Represents a property value.
    /// </summary>
    public string? Value { get; }

    private XmlProperty (string key, string? value)
    {
        Key = key;
        Value = value;
    }

    /// <summary>
    /// Creates new property and assigns value
    /// </summary>
    /// <param name="key">Property key</param>
    /// <param name="value">Property value</param>
    public static XmlProperty Create (string key, string? value)
    {
        return new XmlProperty (key, value);
    }
}

/// <summary>
/// Represents a xml node.
/// </summary>
public class XmlItem : IDisposable
{
    private List<XmlItem>? _items;
    private XmlItem? _parent;
    private XmlProperty[]? _properties;

    /// <summary>
    /// Gets a number of children in this node.
    /// </summary>
    public int Count => _items?.Count ?? 0;

    /// <summary>
    /// Gets a list of children in this node.
    /// </summary>
    public List<XmlItem> Items => _items ??= new List<XmlItem>();

    /// <summary>
    /// Gets a child node with specified index.
    /// </summary>
    /// <param name="index">Index of node.</param>
    /// <returns>The node with specified index.</returns>
    public XmlItem this [int index] => Items[index];

    /// <summary>
    /// Gets or sets the node name.
    /// </summary>
    /// <remarks>
    /// This property will return "Node" for a node like <c>&lt;Node Text="" Left="0"/&gt;</c>
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a list of properties in this node.
    /// </summary>
    public XmlProperty[] Properties
    {
        get => _properties ??= Array.Empty<XmlProperty>();
        set => _properties = value;
    }

    /// <summary>
    /// Gets or sets the parent for this node.
    /// </summary>
    public XmlItem? Parent
    {
        get => _parent;
        set
        {
            if (_parent != value)
            {
                if (_parent != null)
                {
                    _parent.Items.Remove (this);
                }

                if (value != null)
                {
                    value.Items.Add (this);
                }
            }

            _parent = value;
        }
    }

    /// <summary>
    /// Gets or sets the node value.
    /// </summary>
    /// <remarks>
    /// This property will return "ABC" for a node like <c>&lt;Node&gt;ABC&lt;/Node&gt;</c>
    /// </remarks>
    public string Value { get; set; }

    /// <summary>
    /// Gets the root node which owns this node.
    /// </summary>
    public XmlItem Root
    {
        get
        {
            var result = this;
            while (result.Parent != null)
            {
                result = result.Parent;
            }

            return result;
        }
    }

    /// <summary>
    /// Clears the child nodes of this node.
    /// </summary>
    public void Clear()
    {
        if (_items != null)
        {
            _items.Clear();
            /*        while (Items.Count > 0)
                    {
                      Items[0].Dispose();
                    }  */
            _items = null;
        }
    }

    /// <summary>
    /// Adds a new child node to this node.
    /// </summary>
    /// <returns>The new child node.</returns>
    public XmlItem Add()
    {
        var result = new XmlItem();
        AddItem (result);
        return result;
    }

    /// <summary>
    /// Adds a specified node to this node.
    /// </summary>
    /// <param name="item">The node to add.</param>
    public void AddItem (XmlItem item)
    {
        item.Parent = this;
    }

    /// <summary>
    /// Inserts a specified node to this node.
    /// </summary>
    /// <param name="index">Position to insert.</param>
    /// <param name="item">Node to insert.</param>
    public void InsertItem (int index, XmlItem item)
    {
        AddItem (item);
        Items.RemoveAt (Count - 1);
        Items.Insert (index, item);
    }

    /// <summary>
    /// Finds the node with specified name.
    /// </summary>
    /// <param name="name">The name of node to find.</param>
    /// <returns>The node with specified name, if found; <b>null</b> otherwise.</returns>
    public int Find (string name)
    {
        for (var i = 0; i < Count; i++)
        {
            if (String.Compare (Items[i].Name, name, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Finds the node with specified name.
    /// </summary>
    /// <param name="name">The name of node to find.</param>
    /// <returns>The node with specified name, if found; the new node otherwise.</returns>
    /// <remarks>
    /// This method adds the node with specified name to the child nodes if it cannot find the node.
    /// Do not dispose items, which has been created by this method
    /// </remarks>
    public XmlItem FindItem (string name)
    {
        XmlItem? result;
        var i = Find (name);
        if (i == -1)
        {
            result = Add();
            result.Name = name;
        }
        else
        {
            result = Items[i];
        }

        return result;
    }

    /// <summary>
    /// Gets the index of specified node in the child nodes list.
    /// </summary>
    /// <param name="item">The node to find.</param>
    /// <returns>Zero-based index of node, if found; <b>-1</b> otherwise.</returns>
    public int IndexOf (XmlItem item)
    {
        return Items.IndexOf (item);
    }

    /// <summary>
    /// Gets a property with specified name.
    /// </summary>
    /// <param name="key">The property name.</param>
    /// <returns>The value of property, if found; empty string otherwise.</returns>
    /// <remarks>
    /// This property will return "0" when you request the "Left" property for a node
    /// like <c>&lt;Node Text="" Left="0"/&gt;</c>
    /// </remarks>
    public string? GetProp (string key)
    {
        return GetProp (key, true);
    }

    internal string? GetProp (string key, bool convertFromXml)
    {
        if (_properties == null || _properties.Length == 0)
        {
            return "";
        }

        // property key should be trimmed
        key = key.Trim();

        foreach (var kv in _properties)
        {
            if (kv.Key == key)
            {
                return kv.Value;
            }
        }

        return "";
    }

    internal void WriteProps (FastString sb)
    {
        if (_properties == null || _properties.Length == 0)
        {
            return;
        }

        sb.Append (" ");
        foreach (var kv in _properties)
        {
            //if (string.IsNullOrWhiteSpace(kv.Key))
            if (string.IsNullOrEmpty (kv.Key) || kv.Key.Trim().Length == 0)
            {
                continue;
            }

            sb.Append (kv.Key);
            sb.Append ("=\"");
            sb.Append (Converter.ToXml (kv.Value));
            sb.Append ("\" ");
        }

        sb.Length--;
    }

    /// <summary>
    /// Removes all properties.
    /// </summary>
    public void ClearProps()
    {
        _properties = null;
    }

    internal void CopyPropsTo (XmlItem item)
    {
        if (_properties == null)
        {
            item._properties = null;
            return;
        }

        item._properties = (XmlProperty[])_properties.Clone();
    }

    internal bool IsNullOrEmptyProps()
    {
        return _properties == null || _properties.Length == 0;
    }

    /// <summary>
    /// Sets the value for a specified property.
    /// </summary>
    /// <param name="key">The property name.</param>
    /// <param name="value">Value to set.</param>
    /// <remarks>
    /// For example, you have a node like <c>&lt;Node Text="" Left="0"/&gt;</c>. When you set the
    /// "Text" property to "test", the node will be <c>&lt;Node Text="test" Left="0"/&gt;</c>.
    /// If property with specified name is not exist, it will be added.
    /// </remarks>
    public void SetProp (string key, string? value)
    {
        // property key should be trimmed
        key = key.Trim();

        if (_properties == null)
        {
            _properties = new XmlProperty[1];
            _properties[0] = XmlProperty.Create (key, value);
            return;
        }

        for (var i = 0; i < _properties.Length; i++)
        {
            if (_properties[i].Key == key)
            {
                _properties[i] = XmlProperty.Create (key, value);
                return;
            }
        }

        Array.Resize<XmlProperty> (ref _properties, _properties.Length + 1);
        _properties[^1] = XmlProperty.Create (key, value);
    }

    /// <summary>
    /// Removes a property with specified name.
    /// </summary>
    /// <param name="key">The property name.</param>
    /// <returns>Returns true if property is removed, false otherwise.</returns>
    public bool RemoveProp (string key)
    {
        if (_properties == null || _properties.Length == 0)
        {
            return false;
        }

        // property key should be trimmed
        key = key.Trim();

        if (_properties.Length == 1 && _properties[0].Key == key)
        {
            _properties = null;
            return true;
        }

        if (_properties[^1].Key == key)
        {
            Array.Resize<XmlProperty> (ref _properties, _properties.Length - 1);
            return true;
        }

        var target = -1;

        for (var i = 0; i < _properties.Length; i++)
        {
            if (_properties[i].Key == key)
            {
                target = i;
                break;
            }
        }

        if (target != -1)
        {
            for (var i = target; i < _properties.Length - 1; i++)
            {
                _properties[i] = _properties[i + 1];
            }

            Array.Resize<XmlProperty> (ref _properties, _properties.Length - 1);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Disposes the node and all its children.
    /// </summary>
    public void Dispose()
    {
        Clear();
        Parent = null;
    }

    /// <summary>
    /// Initializes a new instance of the <b>XmlItem</b> class with default settings.
    /// </summary>
    public XmlItem()
    {
        Name = "";
        Value = "";
    }
}


/// <summary>
/// Represents a xml document that contains the root xml node.
/// </summary>
/// <remarks>
/// Use <b>Load</b> and <b>Save</b> methods to load/save the document. To access the root node
/// of the document, use the <see cref="Root"/> property.
/// </remarks>
public class XmlDocument : IDisposable
{
    /// <summary>
    /// Gets or sets a value indicating whether is necessary to indent the document
    /// when saving it to a file/stream.
    /// </summary>
    public bool AutoIndent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether is necessary to add xml header.
    /// </summary>
    public bool WriteHeader { get; set; }

    /// <summary>
    /// Gets the root node of the document.
    /// </summary>
    public XmlItem Root { get; }

    /// <summary>
    /// Clears the document.
    /// </summary>
    public void Clear()
    {
        Root.Clear();
    }

    /// <summary>
    /// Saves the document to a stream.
    /// </summary>
    /// <param name="stream">Stream to save to.</param>
    public void Save (Stream stream)
    {
        var wr = new XmlWriter (stream)
        {
            AutoIndent = AutoIndent,
            IsWriteHeader = WriteHeader
        };
        wr.Write (Root);
    }

    /// <summary>
    /// Saves the document to a string.
    /// </summary>
    /// <param name="textWriter">Writer to save to.</param>
    public void Save (TextWriter textWriter)
    {
        var wr = new XmlWriter (textWriter)
        {
            AutoIndent = AutoIndent,
            IsWriteHeader = WriteHeader
        };
        wr.Write (Root);
    }

    /// <summary>
    /// Loads the document from a stream.
    /// </summary>
    /// <param name="stream">Stream to load from.</param>
    public void Load (Stream stream)
    {
        var rd = new XmlReader (stream);
        Root.Clear();
        rd.Read (Root);
    }

    /// <summary>
    /// Saves the document to a file.
    /// </summary>
    /// <param name="fileName">The name of file to save to.</param>
    public void Save (string fileName)
    {
        var s = new FileStream (fileName, FileMode.Create);
        Save (s);
        s.Close();
    }

    /// <summary>
    /// Loads the document from a file.
    /// </summary>
    /// <param name="fileName">The name of file to load from.</param>
    public void Load (string fileName)
    {
        var s = new FileStream (fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        Load (s);
        s.Close();
    }

    /// <summary>
    /// Disposes resources used by the document.
    /// </summary>
    public void Dispose()
    {
        Root.Dispose();
    }

    /// <summary>
    /// Initializes a new instance of the <b>XmlDocument</b> class with default settings.
    /// </summary>
    public XmlDocument()
    {
        Root = new XmlItem();
        WriteHeader = true;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        using (TextWriter tw = new StringWriter())
        {
            Save (tw);
            tw.Flush();
            return tw.ToString();
        }
    }
}

internal class XmlReader
{
    private StreamReader reader;
    private Stream stream;
    private string lastName;

    private enum ReadState
    {
        FindLeft,
        FindRight,
        FindComment,
        FindCloseItem,
        Done
    }

    private enum ItemState
    {
        Begin,
        End,
        Complete
    }

    private int symbolInBuffer;
    Dictionary<string, string> stringPool;

    private ItemState ReadItem (XmlItem item)
    {
        FastString builder;
        if (Config.IsStringOptimization)
        {
            builder = new FastStringWithPool (stringPool);
        }
        else
        {
            builder = new FastString();
        }

        var state = ReadState.FindLeft;
        var comment = 0;
        var i = 0;

        //string tempAttrName = null;
        //int lc = -1;
        var c = -1;

        // find <
        c = readNextSymbol();
        while (c != -1)
        {
            if (c == '<')
            {
                break;
            }

            c = readNextSymbol();
        }

        //while not end
        while (state != ReadState.Done && c != -1)
        {
            // find name or comment;
            c = readNextSymbol();
            i = 0;
            while (c != -1)
            {
                if (i <= comment)
                {
                    switch (comment)
                    {
                        case 0:
                            if (c == '!')
                            {
                                comment++;
                            }

                            break;
                        case 1:
                            if (c == '-')
                            {
                                comment++;
                            }

                            break;
                        case 2:
                            if (c == '-')
                            {
                                state = ReadState.FindComment;
                            }

                            break;
                        default:
                            comment = -1;
                            break;
                    }

                    if (state == ReadState.FindComment)
                    {
                        break;
                    }
                }

                i++;
                switch (c)
                {
                    case '>':
                        state = ReadState.Done;
                        break; //Found name
                    case ' ':
                        state = ReadState.FindRight;
                        break; //Found name
                    case '<':
                        RaiseException();
                        break;
                    default:
                        builder.Append ((char)c);
                        break;
                }

                if (state != ReadState.FindLeft)
                {
                    break;
                }

                c = readNextSymbol();
            }

            switch (state)
            {
                case ReadState.FindComment:
                    comment = 0;
                    while (c != -1)
                    {
                        c = readNextSymbol();
                        if (comment > 1)
                        {
                            if (c == '>')
                            {
                                state = ReadState.FindLeft;
                                break;
                            }
                        }
                        else
                        {
                            if (c == '-')
                            {
                                comment++;
                            }
                            else
                            {
                                comment = 0;
                            }
                        }
                    }

                    comment = 0;
                    builder.Length = 0;
                    while (c != -1)
                    {
                        if (c == '<')
                        {
                            break;
                        }

                        c = readNextSymbol();
                    }

                    break;
                case ReadState.Done:
                    var result = builder.ToString();
                    if (result[0] == '/')
                    {
                        item.Name = result.Substring (1);
                        return ItemState.End;
                    }

                    if (result[result.Length - 1] == '/')
                    {
                        item.Name = result.Substring (0, result.Length - 1);
                        return ItemState.Complete;
                    }

                    item.Name = result;
                    return ItemState.Begin;
                case ReadState.FindRight:
                    if (builder[0] == '/')
                    {
                        builder.Remove (0, 1);
                        item.Name = builder.ToString();
                        return ItemState.End;
                    }

                    item.Name = builder.ToString();
                    builder.Length = 0;
                    while (c != -1 && c != '>')
                    {
                        c = readNextSymbol();
                        while (c != -1)
                        {
                            if (c == ' ')
                            {
                                builder.Length = 0;
                                c = readNextSymbol();
                                continue;
                            }

                            if (c is '=' or '>')
                            {
                                break;
                            }

                            builder.Append ((char)c);
                            c = readNextSymbol();
                        }

                        if (c == '>')
                        {
                            if (builder.Length > 0 && builder[builder.Length - 1] == '/')
                            {
                                return ItemState.Complete;
                            }

                            return ItemState.Begin;
                        }

                        c = readNextSymbol();
                        if (c != '"')
                        {
                            continue;
                        }

                        var attrName = builder.ToString();
                        builder.Length = 0;
                        while (c != -1)
                        {
                            c = readNextSymbol();
                            if (c == '"')
                            {
                                break;
                            }

                            builder.Append ((char)c);
                        }

                        item.SetProp (attrName, Converter.FromXml (builder.ToString()));
                        builder.Length = 0;
                    }

                    break;
            }
        }

        //just for errors
        return ItemState.Begin;
    }

    private int readNextSymbol()
    {
        if (symbolInBuffer != -1)
        {
            var temp = symbolInBuffer;
            symbolInBuffer = -1;
            return temp;
        }

        return reader.Read();
    }

    private bool ReadValue (XmlItem item)
    {
        FastString builder;
        if (Config.IsStringOptimization)
        {
            builder = new FastStringWithPool (stringPool);
        }
        else
        {
            builder = new FastString();
        }

        var state = ReadState.FindLeft;
        var lastName = "</" + this.lastName + ">";
        var lastNameLength = lastName.Length;

        do
        {
            var c = reader.Read();
            if (c == -1)
            {
                RaiseException();
            }

            builder.Append ((char)c);
            if (state == ReadState.FindLeft)
            {
                if (c == '<')
                {
                    symbolInBuffer = '<';
                    return false;
                }
                else if (c != ' ' && c != '\r' && c != '\n' && c != '\t')
                {
                    state = ReadState.FindCloseItem;
                }
            }
            else if (state == ReadState.FindCloseItem)
            {
                if (builder.Length >= lastNameLength)
                {
                    var match = true;
                    for (var j = 0; j < lastNameLength; j++)
                    {
                        if (builder[builder.Length - lastNameLength + j] != lastName[j])
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        builder.Length -= lastNameLength;
                        item.Value = Converter.FromXml (builder.ToString());
                        return true;
                    }
                }
            }
        } while (true);
    }

    private bool DoRead (XmlItem rootItem)
    {
        var itemState = ReadItem (rootItem);
        lastName = rootItem.Name;

        if (itemState == ItemState.End)
        {
            return true;
        }
        else if (itemState == ItemState.Complete)
        {
            return false;
        }

        if (ReadValue (rootItem))
        {
            return false;
        }

        var done = false;
        do
        {
            var childItem = new XmlItem();
            done = DoRead (childItem);
            if (!done)
            {
                rootItem.AddItem (childItem);
            }
            else
            {
                childItem.Dispose();
            }
        } while (!done);

        if (lastName != "" && string.Compare (lastName, rootItem.Name, StringComparison.OrdinalIgnoreCase) != 0)
        {
            RaiseException();
        }

        return false;
    }

    private void RaiseException()
    {
        throw new FileFormatException();
    }

    private void ReadHeader()
    {
        using (var item = new XmlItem())
        {
            ReadItem (item);
            if (item.Name.IndexOf ("?xml") != 0)
            {
                RaiseException();
            }
        }
    }

    public void Read (XmlItem item)
    {
        ReadHeader();
        DoRead (item);
    }

    public XmlReader (Stream stream)
    {
        this.stream = stream;
        reader = new StreamReader (this.stream, Encoding.UTF8);
        if (Config.IsStringOptimization)
        {
            stringPool = new Dictionary<string, string>();
        }

        lastName = "";
        symbolInBuffer = -1;
    }
}


internal class XmlWriter
{
    //private Stream FStream;
    private TextWriter writer;

    public bool AutoIndent { get; set; }

    public bool IsWriteHeader { get; set; }

    private void WriteLn (string s)
    {
        if (!AutoIndent)
        {
            writer.Write (s);
        }
        else
        {
            writer.Write (s + "\r\n");
        }
    }

    private string Dup (int num)
    {
        var s = "";
        return s.PadLeft (num);
    }

    private void WriteItem (XmlItem item, int level)
    {
        var sb = new FastString();


        // start
        if (AutoIndent)
        {
            sb.Append (Dup (level));
        }

        sb.Append ("<");
        sb.Append (item.Name);

        // text

        item.WriteProps (sb);

        // end
        if (item is { Count: 0, Value: "" })
        {
            sb.Append ("/>");
        }
        else
        {
            sb.Append (">");
        }

        // value
        if (item.Count == 0 && item.Value != "")
        {
            sb.Append (Converter.ToXml (item.Value, false));
            sb.Append ("</");
            sb.Append (item.Name);
            sb.Append (">");
        }

        WriteLn (sb.ToString());
    }

    private void DoWrite (XmlItem rootItem, int level)
    {
        if (!AutoIndent)
        {
            level = 0;
        }

        WriteItem (rootItem, level);
        for (var i = 0; i < rootItem.Count; i++)
        {
            DoWrite (rootItem[i], level + 2);
        }

        if (rootItem.Count > 0)
        {
            if (!AutoIndent)
            {
                WriteLn ("</" + rootItem.Name + ">");
            }
            else
            {
                WriteLn (Dup (level) + "</" + rootItem.Name + ">");
            }
        }
    }

    private void WriteHeader()
    {
        WriteLn ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
    }

    public void Write (XmlItem rootItem)
    {
        if (IsWriteHeader)
        {
            WriteHeader();
        }

        DoWrite (rootItem, 0);
        writer.Flush();
    }

    public XmlWriter (Stream stream)
    {
        //FStream = stream;
        writer = new StreamWriter (stream, Encoding.UTF8);
        IsWriteHeader = true;
    }

    public XmlWriter (TextWriter textWriter)
    {
        //FStream = null;
        writer = textWriter;
        IsWriteHeader = true;
    }
}
