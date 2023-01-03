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
using System.Collections;
using System.Diagnostics;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    /// <summary>
    /// Specifies the target for the serialize operation.
    /// </summary>
    public enum SerializeTo
    {
        /// <summary>
        /// Serialize to the report file.
        /// </summary>
        Report,

        /// <summary>
        /// Serialize to the preview pages.
        /// </summary>
        Preview,

        /// <summary>
        /// Serialize to the source pages of a preview.
        /// </summary>
        SourcePages,

        /// <summary>
        /// Serialize to the designer's clipboard.
        /// </summary>
        Clipboard,

        /// <summary>
        /// Serialize to the designer's undo/redo buffer.
        /// </summary>
        Undo
    }

    internal class DiffEventArgs
    {
        public object? Object { get; set; }

        public object? DiffObject { get; set; }
    }

    internal delegate void DiffEventHandler (object sender, DiffEventArgs e);


    /// <summary>
    /// The writer used to serialize object's properties to a report file.
    /// </summary>
    public class ReportWriter
        : IDisposable
    {
        #region Fields

        private readonly XmlDocument _document;
        private readonly XmlItem _root;
        private XmlItem? _currentItem;

        private XmlItem curRoot;

        //private StringBuilder FText;
        private readonly Hashtable _diffObjects;

        #endregion

        #region Properties

        internal event DiffEventHandler? GetDiff;

        internal BlobStore? BlobStore { get; set; }

        /// <summary>
        /// Gets or sets current xml item name.
        /// </summary>
        public string ItemName
        {
            get => _currentItem!.Name;
            set => _currentItem!.Name = value;
        }

        /// <summary>
        /// Gets or sets target of serialization.
        /// </summary>
        public SerializeTo SerializeTo { get; set; }

        /// <summary>
        /// Gets the ethalon object to compare with.
        /// </summary>
        public object? DiffObject { get; private set; }

        /// <summary>
        /// Gets or sets a value that determines whether is necessary to serialize child objects.
        /// </summary>
        public bool SaveChildren { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether is necessary to add xml header.
        /// </summary>
        public bool WriteHeader { get; set; }

        #endregion

        #region Private Methods

        private string PropName (string name)
        {
            return SerializeTo == SerializeTo.Preview ? ShortProperties.GetShortName (name) : name;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <remarks>
        /// The object must implement the <see cref="IReportSerializable"/> interface. This method
        /// invokes the <b>Serialize</b> method of the object.
        /// </remarks>
        /// <example>This example demonstrates the use of writer.
        /// <code>
        /// public void Serialize(FRWriter writer)
        /// {
        ///   // get the etalon object. It will be used to write changed properties only.
        ///   Base c = writer.DiffObject as Base;
        ///
        ///   // write the type name
        ///   writer.ItemName = ClassName;
        ///
        ///   // write properties
        ///   if (Name != "")
        ///     writer.WriteStr("Name", Name);
        ///   if (Restrictions != c.Restrictions)
        ///     writer.WriteValue("Restrictions", Restrictions);
        ///
        ///   // write child objects if allowed
        ///   if (writer.SaveChildren)
        ///   {
        ///     foreach (Base child in ChildObjects)
        ///     {
        ///       writer.Write(child);
        ///     }
        ///   }
        /// }
        /// </code>
        /// </example>
        public void Write (IReportSerializable obj)
        {
            Write (obj, null);
        }

        /// <summary>
        /// Serializes the object using specified etalon.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="diff">The etalon object.</param>
        public void Write (IReportSerializable? obj, object? diff)
        {
            if (obj == null)
            {
                return;
            }

            var saveCurItem = _currentItem;
            var saveCurRoot = curRoot;

            //StringBuilder saveText = FText;
            var saveDiffObject = DiffObject;
            try
            {
                //FText = new StringBuilder();
                _currentItem = _currentItem == null ? _root : _currentItem.Add();
                curRoot = _currentItem;
                DiffObject = diff;
                if (obj is Base @base && SerializeTo == SerializeTo.Preview)
                {
                    DiffObject = @base.OriginalComponent;
                    _currentItem.Name = DiffObject != null ? @base.Alias : @base.ClassName;
                }

                if (GetDiff != null)
                {
                    var e = new DiffEventArgs
                    {
                        Object = obj
                    };
                    GetDiff (this, e);
                    DiffObject = e.DiffObject;
                }

                if (DiffObject == null)
                {
                    try
                    {
                        var objType = obj.GetType();
                        if (!_diffObjects.Contains (objType))
                        {
                            _diffObjects[objType] = Activator.CreateInstance (objType);
                        }

                        DiffObject = _diffObjects[objType];
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine (exception.Message);
                    }
                }

                obj.Serialize (this);
            }
            finally
            {
                //if (FText.Length > 0)
                //          FText.Remove(FText.Length - 1, 1);
                //FCurRoot.Text = FText.ToString();
                //FText = saveText;
                _currentItem = saveCurItem;
                curRoot = saveCurRoot;
                DiffObject = saveDiffObject;
            }
        }

        /// <summary>
        /// Writes a string property.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        public void WriteStr (string name, string? value)
        {
            curRoot.SetProp (PropName (name), value);

            //FText.Append(PropName(name));
            //FText.Append("=\"");
            //FText.Append(Converter.ToXml(value));
            //FText.Append("\" ");
        }

        /// <summary>
        /// Writes a boolean property.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        public void WriteBool (string name, bool value)
        {
            curRoot.SetProp (PropName (name), value ? "true" : "false");

            //      FText.Append(PropName(name));
            //FText.Append("=\"");
            //FText.Append(value ? "true" : "false");
            //FText.Append("\" ");
        }

        /// <summary>
        /// Writes an integer property.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        public void WriteInt (string name, int value)
        {
            curRoot.SetProp (PropName (name), value.ToString());

            //FText.Append(PropName(name));
            //FText.Append("=\"");
            //FText.Append(value.ToString());
            //FText.Append("\" ");
        }

        /// <summary>
        /// Writes a float property.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        public void WriteFloat (string name, float value)
        {
            curRoot.SetProp (PropName (name), value.ToString (CultureInfo.InvariantCulture.NumberFormat));

            //FText.Append(PropName(name));
            //FText.Append("=\"");
            //FText.Append(value.ToString(CultureInfo.InvariantCulture.NumberFormat));
            //FText.Append("\" ");
        }

        /// <summary>
        /// Writes a double property.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        public void WriteDouble (string name, double value)
        {
            curRoot.SetProp (PropName (name), value.ToString (CultureInfo.InvariantCulture.NumberFormat));

            //FText.Append(PropName(name));
            //FText.Append("=\"");
            //FText.Append(value.ToString(CultureInfo.InvariantCulture.NumberFormat));
            //FText.Append("\" ");
        }

        /// <summary>
        /// Writes an enumeration property.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        public void WriteValue (string name, object? value)
        {
            curRoot.SetProp (PropName (name), value != null ? Converter.ToString (value) : "null");

            //FText.Append(PropName(name));
            //FText.Append("=\"");
            //FText.Append(value != null ? Converter.ToXml(value) : "null");
            //FText.Append("\" ");
        }

        /// <summary>
        /// Writes an object reference property.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <param name="value">Property value.</param>
        public void WriteRef (string name, Base? value)
        {
            curRoot.SetProp (PropName (name), value != null ? value.Name : "null");

            //FText.Append(PropName(name));
            //FText.Append("=\"");
            //FText.Append(value != null ? value.Name : "null");
            //FText.Append("\" ");
        }

        /// <summary>
        /// Writes a standalone property value.
        /// </summary>
        /// <param name="name">Name of property.</param>
        /// <param name="value">Property value.</param>
        /// <remarks>
        /// This method produces the following output:
        /// &lt;PropertyName&gt;PropertyValue&lt;/PropertyName&gt;
        /// </remarks>
        public void WritePropertyValue (string name, string value)
        {
            var item = _currentItem!.Add();
            item.Name = name;
            item.Value = value;
        }

        /// <summary>
        /// Determines if two objects are equal.
        /// </summary>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <returns><b>true</b> if objects will be serialized to the same value.</returns>
        public bool AreEqual (object? obj1, object? obj2)
        {
            if (obj1 == obj2)
            {
                return true;
            }

            if (obj1 == null || obj2 == null)
            {
                return false;
            }

            var s1 = Converter.ToString (obj1);
            var s2 = Converter.ToString (obj2);
            return s1 == s2;
        }

        /// <summary>
        /// Disposes the writer.
        /// </summary>
        public void Dispose()
        {
            _document.Dispose();
            foreach (var obj in _diffObjects.Values)
            {
                if (obj is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        /// <summary>
        /// Saves the writer output to a stream.
        /// </summary>
        /// <param name="stream">Stream to save to.</param>
        public void Save (Stream stream)
        {
            _document.AutoIndent = SerializeTo == SerializeTo.Report;
            _document.WriteHeader = WriteHeader;
            _document.Save (stream);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <b>FRWriter</b> class with default settings.
        /// </summary>
        public ReportWriter()
        {
            _document = new XmlDocument();
            _root = _document.Root;

            //FText = new StringBuilder();
            SaveChildren = true;
            WriteHeader = true;
            _diffObjects = new Hashtable();
        }

        /// <summary>
        /// Initializes a new instance of the <b>FRWriter</b> class with specified xml item that will
        /// receive writer's output.
        /// </summary>
        /// <param name="root">The xml item that will receive writer's output.</param>
        public ReportWriter (XmlItem root) : this()
        {
            this._root = root;
        }
    }
}
