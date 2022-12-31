// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    /// <summary>
    /// The reader used to deserialize object's properties from a report file.
    /// </summary>
    public class FRReader : IDisposable
    {
        #region Fields

        private XmlDocument doc;
        private XmlItem root;
        private XmlItem curItem;
        private XmlItem curRoot;
        private XmlProperty[] props;
        private List<FixupInfo> fixups;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a string that contains errors occured during the load.
        /// </summary>
        public string Errors { get; private set; }

        internal BlobStore BlobStore { get; set; }

        /// <summary>
        /// Gets the current item name.
        /// </summary>
        public string ItemName => curItem.Name;

        /// <summary>
        /// Gets or sets a value indicating whther is necessary to read the object's children.
        /// </summary>
        public bool ReadChildren { get; set; }

        /// <summary>
        /// Returns Root element for this reader
        /// </summary>
        public XmlItem Root
        {
            get
            {
                if (curItem != null)
                {
                    return GetRoot (curItem, 11);
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets target of serialization.
        /// </summary>
        public SerializeTo DeserializeFrom { get; set; }

        public Report Report { get; private set; }

        private XmlItem GetRoot (XmlItem item, int count)
        {
            if (count < 0 || item.Parent == null)
            {
                return item;
            }

            return GetRoot (item.Parent, count - 1);
        }

        #endregion

        #region Private Methods

        private void GetProps()
        {
            if (curRoot != null)
            {
                props = curRoot.Properties;
            }
        }

        private string PropName (string name)
        {
            return ShortProperties.GetFullName (name);
        }

        private string PropValue (string name)
        {
            var i = IndexOf (name);
            if (i != -1)
            {
                return props[i].Value;
            }

            return "";
        }

        private int IndexOf (string name)
        {
            if (props == null)
            {
                return -1;
            }

            // property key should be trimmed
            name = name.Trim();

            var result = -1;
            for (var i = 0; i < props.Length; i++)
            {
                if (string.Compare (props[i].Key, name, true) == 0)
                {
                    result = i;
                    break;
                }
            }

            return result;
        }

        private void FixupReferences()
        {
            if (Report == null)
            {
                return;
            }

            foreach (var fixup in fixups)
            {
                var pi = fixup.obj.GetType().GetProperty (fixup.name);
                if (pi != null)
                {
                    pi.SetValue (fixup.obj, Report.FindObject (fixup.value), null);
                }
            }

            fixups.Clear();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reads the specified object.
        /// </summary>
        /// <param name="obj">The object to read.</param>
        /// <remarks>
        /// The object must implement the <see cref="IFRSerializable"/> interface. This method
        /// invokes the <b>Deserialize</b> method of the object.
        /// </remarks>
        /// <example>This example demonstrates the use of <b>ReadProperties</b>, <b>ReadChildren</b>,
        /// <b>NextItem</b>, <b>Read</b> methods.
        /// <code>
        /// public void Deserialize(FRReader reader)
        /// {
        ///   // read simple properties like "Text", complex properties like "Border.Lines"
        ///   reader.ReadProperties(this);
        ///
        ///   // moves the current reader item
        ///   while (reader.NextItem())
        ///   {
        ///     // read the "Styles" collection
        ///     if (String.Compare(reader.ItemName, "Styles", true) == 0)
        ///       reader.Read(Styles);
        ///     else if (reader.ReadChildren)
        ///     {
        ///       // if read of children is enabled, read them
        ///       Base obj = reader.Read();
        ///       if (obj != null)
        ///          obj.Parent = this;
        ///     }
        ///   }
        /// }
        /// </code>
        /// </example>
        public void Read (IFRSerializable obj)
        {
            var saveCurItem = curItem;
            var saveCurRoot = curRoot;
            var saveProps = props;
            try
            {
                if (curItem == null)
                {
                    curItem = root;
                }

                curRoot = curItem;
                GetProps();
                obj.Deserialize (this);
            }
            finally
            {
                curItem = saveCurItem;
                curRoot = saveCurRoot;
                props = saveProps;
            }
        }

        internal void Read (IFRSerializable obj, XmlItem Root)
        {
            curItem = Root;
            Read (obj);
        }

        /// <summary>
        /// Reads an object from current xml node.
        /// </summary>
        /// <returns>The object.</returns>
        /// <remarks>
        /// This method creates an instance of object described by the current xml node, then invokes
        /// its <b>Deserialize</b> method.
        /// </remarks>
        /// <example>This example demonstrates the use of <b>ReadProperties</b>, <b>ReadChildren</b>,
        /// <b>NextItem</b>, <b>Read</b> methods.
        /// <code>
        /// public void Deserialize(FRReader reader)
        /// {
        ///   // read simple properties like "Text", complex properties like "Border.Lines"
        ///   reader.ReadProperties(this);
        ///
        ///   // moves the current reader item
        ///   while (reader.NextItem())
        ///   {
        ///     // read the "Styles" collection
        ///     if (String.Compare(reader.ItemName, "Styles", true) == 0)
        ///       reader.Read(Styles);
        ///     else if (reader.ReadChildren)
        ///     {
        ///       // if read of children is enabled, read them
        ///       Base obj = reader.Read();
        ///       if (obj != null)
        ///          obj.Parent = this;
        ///     }
        ///   }
        /// }
        /// </code>
        /// </example>
        public IFRSerializable Read()
        {
            var saveCurItem = curItem;
            var saveCurRoot = curRoot;
            var saveProps = props;
            IFRSerializable result = null;

            try
            {
                if (curItem == null)
                {
                    curItem = root;
                }

                curRoot = curItem;
                GetProps();

                if (Report is { IsAncestor: true })
                {
                    result = Report.FindObject (ReadStr ("Name"));
                }

                if (result == null && curItem.Name != "inherited")
                {
                    var type = RegisteredObjects.FindType (curItem.Name);
                    if (type != null)
                    {
                        result = Activator.CreateInstance (type) as IFRSerializable;
                        if (result is Report report1)
                        {
                            Report = report1;
                        }
                    }
                    else
                    {
                        if (!Config.WebMode)
                        {
                            MessageBox.Show (Res.Get ("Messages,CantFindObject") + " " + curItem.Name);
                        }
                        else
                        {
                            throw new ClassException (curItem.Name);
                        }
                    }
                }

                if (result != null)
                {
                    result.Deserialize (this);
                }
            }
            finally
            {
                curItem = saveCurItem;
                curRoot = saveCurRoot;
                props = saveProps;
            }

            return result;
        }

        private int getDotCount (string s)
        {
            if (string.IsNullOrEmpty (s))
            {
                return 0;
            }

            var i = 0;
            var count = 0;
            while ((i = s.IndexOf ('.', i)) != -1)
            {
                ++i;
                ++count;
            }

            return count;
        }

        private void DoReadProperties (object obj, XmlProperty[] properties)
        {
            for (var i = 0; i < properties.Length; i++)
            {
                var name = properties[i].Key;
                var value = properties[i].Value;

                // check multiple properties like Frame.LeftLine.Typ
                var obj1 = obj;
                var len = name.Length;
                var start = 0;
                var j = 0;

                // find '.'
                while (j < len && name[j] != '.')
                    j++;
                if (j < len)
                {
                    while (j < len)
                    {
                        // get subproperty
                        var pi = obj1.GetType().GetProperty (name.Substring (start, j - start));
                        if (pi == null)
                        {
                            break;
                        }

                        obj1 = pi.GetValue (obj1, null);

                        // find next '.'
                        start = j + 1;
                        j++;
                        while (j < len && name[j] != '.')
                            j++;
                    }

                    name = name.Substring (start);
                }

                try
                {
                    name = PropName (name);
                    var pi = obj1.GetType().GetProperty (name);
                    if (pi == null)
                    {
                        continue;
                    }

                    if (value == "null")
                    {
                        if (pi.PropertyType == typeof (string))
                        {
                            pi.SetValue (obj1, "null", null);
                        }
                        else
                        {
                            pi.SetValue (obj1, null, null);
                        }
                    }
                    else
                    {
                        if (pi.PropertyType == typeof (string))
                        {
                            pi.SetValue (obj1, value, null);
                        }
                        else if (pi.PropertyType.IsClass && pi.PropertyType.IsSubclassOf (typeof (Base)))
                        {
                            // it's a reference
                            fixups.Add (new FixupInfo (obj1, name, value));
                        }
                        else
                        {
                            pi.SetValue (obj1, Converter.FromString (pi.PropertyType, value), null);
                        }
                    }
                }
                catch (Exception e)
                {
                    Errors += e.Message + "\r\n";
                }
            }
        }

        /// <summary>
        /// Reads properties of specified object.
        /// </summary>
        /// <param name="obj">The object to read.</param>
        /// <remarks>
        /// This method reads simple properties like "Text", "Border.Lines" etc. for specified object.
        /// To read nested properties like collections, you should override the <see cref="Base.DeserializeSubItems"/>
        /// method of an object.
        /// </remarks>
        /// <example>This example demonstrates the use of <b>ReadProperties</b>, <b>ReadChildren</b>,
        /// <b>NextItem</b>, <b>Read</b> methods.
        /// <code>
        /// public void Deserialize(FRReader reader)
        /// {
        ///   // read simple properties like "Text", complex properties like "Border.Lines"
        ///   reader.ReadProperties(this);
        ///
        ///   // moves the current reader item
        ///   while (reader.NextItem())
        ///   {
        ///     // read the "Styles" collection
        ///     if (String.Compare(reader.ItemName, "Styles", true) == 0)
        ///       reader.Read(Styles);
        ///     else if (reader.ReadChildren)
        ///     {
        ///       // if read of children is enabled, read them
        ///       Base obj = reader.Read();
        ///       if (obj != null)
        ///          obj.Parent = this;
        ///     }
        ///   }
        /// }
        /// </code>
        /// </example>
        public void ReadProperties (object obj)
        {
            if (props == null)
            {
                return;
            }

            // speed optimization, for use in the preview mode
            if (obj is TextObject textObject && props is [{ Key: "x" } _])
            {
                textObject.Text = props[0].Value;
                return;
            }

            // Fix for multilevel properties with dots such Barcode.CalcCheckSum
            // Reported wrong working with saving from On-line Designer
            var FProps0 = new XmlProperty[0];
            var FProps1 = new XmlProperty[0];
            var FProps2 = new XmlProperty[0];

            for (var i = 0; i < props.Length; i++)
            {
                var dotCount = getDotCount (props[i].Key);
                switch (dotCount)
                {
                    case 0:
                        AppendProperty (ref FProps0, props[i]);
                        break;
                    case 1:
                        AppendProperty (ref FProps1, props[i]);
                        break;
                    default:
                        AppendProperty (ref FProps2, props[i]);
                        break;
                }
            }

            // without dots
            if (FProps0.Length > 0)
            {
                DoReadProperties (obj, FProps0);
            }

            // with one dot
            if (FProps1.Length > 0)
            {
                DoReadProperties (obj, FProps1);
            }

            // with two dots
            if (FProps2.Length > 0)
            {
                DoReadProperties (obj, FProps2);
            }
        }

        private void AppendProperty (ref XmlProperty[] fProps, XmlProperty xmlProperty)
        {
            Array.Resize<XmlProperty> (ref fProps, fProps.Length + 1);
            fProps[fProps.Length - 1] = xmlProperty;
        }

        /// <summary>
        /// Moves the current xml item.
        /// </summary>
        /// <returns><b>false</b> if there is no more items to move on; <b>true</b> otherwise.</returns>
        /// <remarks>
        /// This method is used to read child objects.
        /// </remarks>
        /// <example>This example demonstrates the use of <b>ReadProperties</b>, <b>ReadChildren</b>,
        /// <b>NextItem</b>, <b>Read</b> methods.
        /// <code>
        /// public void Deserialize(FRReader reader)
        /// {
        ///   // read simple properties like "Text", complex properties like "Border.Lines"
        ///   reader.ReadProperties(this);
        ///
        ///   // moves the current reader item
        ///   while (reader.NextItem())
        ///   {
        ///     // read the "Styles" collection
        ///     if (String.Compare(reader.ItemName, "Styles", true) == 0)
        ///       reader.Read(Styles);
        ///     else if (reader.ReadChildren)
        ///     {
        ///       // if read of children is enabled, read them
        ///       Base obj = reader.Read();
        ///       if (obj != null)
        ///          obj.Parent = this;
        ///     }
        ///   }
        /// }
        /// </code>
        /// </example>
        public bool NextItem()
        {
            if (curItem == curRoot)
            {
                if (curRoot.Count > 0)
                {
                    curItem = curRoot[0];
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var i = curRoot.IndexOf (curItem);
                if (i < curRoot.Count - 1)
                {
                    curItem = curRoot[i + 1];
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Checks if current item has specified property.
        /// </summary>
        /// <param name="name">The property name to check.</param>
        /// <returns><b>true</b> if current item has specified property.</returns>
        public bool HasProperty (string name)
        {
            return IndexOf (name) != -1;
        }

        /// <summary>
        /// Reads the string property.
        /// </summary>
        /// <param name="name">Name of property.</param>
        /// <returns>Property value.</returns>
        public string ReadStr (string name)
        {
            return PropValue (name);
        }

        /// <summary>
        /// Reads the boolean property.
        /// </summary>
        /// <param name="name">Name of property.</param>
        /// <returns>Property value.</returns>
        public bool ReadBool (string name)
        {
            var prop = PropValue (name);
            return (prop == "1" || prop.ToLower() == "true") ? true : false;
        }

        /// <summary>
        /// Reads the integer property.
        /// </summary>
        /// <param name="name">Name of property.</param>
        /// <returns>Property value.</returns>
        public int ReadInt (string name)
        {
            return int.Parse (PropValue (name));
        }

        /// <summary>
        /// Reads the float property.
        /// </summary>
        /// <param name="name">Name of property.</param>
        /// <returns>Property value.</returns>
        public float ReadFloat (string name)
        {
            return (float)Converter.FromString (typeof (float), PropValue (name));
        }

        /// <summary>
        /// Reads the double property.
        /// </summary>
        /// <param name="name">Name of property.</param>
        /// <returns>Property value.</returns>
        public double ReadDouble (string name)
        {
            return (double)Converter.FromString (typeof (double), PropValue (name));
        }

        /// <summary>
        /// Reads the enum property.
        /// </summary>
        /// <param name="name">Name of property.</param>
        /// <param name="typ">Type of property.</param>
        /// <returns>Property value.</returns>
        public object ReadValue (string name, Type typ)
        {
            var propValue = PropValue (name);
            if (propValue == "null")
            {
                if (typ == typeof (string))
                {
                    return "null";
                }

                return null;
            }

            return Converter.FromString (typ, propValue);
        }

        /// <summary>
        /// Reads the standalone property value.
        /// </summary>
        /// <returns>Property value.</returns>
        public string ReadPropertyValue()
        {
            return curItem.Value;
        }

        /// <summary>
        /// Disposes the reader, fixups the property references.
        /// </summary>
        public void Dispose()
        {
            FixupReferences();
            doc.Dispose();
        }

        /// <summary>
        /// Loads the xml items from a stream.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        public void Load (Stream stream)
        {
            doc.Load (stream);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <b>FRReader</b> class with specified report.
        /// </summary>
        /// <param name="report">Reference to a report.</param>
        public FRReader (Report report)
        {
            doc = new XmlDocument();
            root = doc.Root;
            fixups = new List<FixupInfo>();
            Errors = "";
            this.Report = report;
            ReadChildren = true;
        }

        /// <summary>
        /// Initializes a new instance of the <b>FRReader</b> class with specified report and xml item with
        /// contents to read.
        /// </summary>
        /// <param name="report">Reference to a report.</param>
        /// <param name="root">Xml item with contents to read.</param>
        public FRReader (Report report, XmlItem root) : this (report)
        {
            this.root = root;
        }


        private class FixupInfo
        {
            public object obj;
            public string name;
            public string value;

            public FixupInfo (object obj, string name, string value)
            {
                this.obj = obj;
                this.name = name;
                this.value = value;
            }
        }
    }
}
