// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.Text;

using PdfSharpCore.Pdf.Advanced;
using PdfSharpCore.Pdf.IO;

#endregion

namespace PdfSharpCore.Pdf;

/// <summary>
/// Represents a PDF array object.
/// </summary>
[DebuggerDisplay ("{DebuggerDisplay}")]
public class PdfArray
    : PdfObject, IEnumerable<PdfItem>
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfArray"/> class.
    /// </summary>
    public PdfArray()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfArray"/> class.
    /// </summary>
    /// <param name="document">The document.</param>
    public PdfArray (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfArray"/> class.
    /// </summary>
    /// <param name="document">The document.</param>
    /// <param name="items">The items.</param>
    public PdfArray (PdfDocument document, params PdfItem[] items)
        : base (document)
    {
        foreach (var item in items)
        {
            Elements.Add (item);
        }
    }

    /// <summary>
    /// Initializes a new instance from an existing dictionary. Used for object type transformation.
    /// </summary>
    /// <param name="array">The array.</param>
    protected PdfArray (PdfArray array)
        : base (array)
    {
        if (array._elements != null)
        {
            array._elements.ChangeOwner (this);
        }
    }

    #endregion

    /// <summary>
    /// Creates a copy of this array. Direct elements are deep copied.
    /// Indirect references are not modified.
    /// </summary>
    public new PdfArray Clone()
    {
        return (PdfArray)Copy();
    }

    /// <inheritdoc cref="PdfObject.Copy"/>
    protected override object Copy()
    {
        var result = (PdfArray) base.Copy();
        if (result._elements != null)
        {
            result._elements = result._elements.Clone();
            var count = result._elements.Count;
            for (var idx = 0; idx < count; idx++)
            {
                var item = result._elements[idx];
                if (item is PdfObject)
                {
                    result._elements[idx] = item.Clone();
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Gets the collection containing the elements of this object.
    /// </summary>
    public ArrayElements Elements => _elements ??= new ArrayElements (this);

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    public virtual IEnumerator<PdfItem> GetEnumerator()
    {
        return Elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Returns a string with the content of this object in a readable form. Useful for debugging purposes only.
    /// </summary>
    public override string ToString()
    {
        var pdf = new StringBuilder();
        pdf.Append ("[ ");
        var count = Elements.Count;
        for (var idx = 0; idx < count; idx++)
        {
            pdf.Append (Elements[idx] + " ");
        }

        pdf.Append ("]");
        return pdf.ToString();
    }

    internal override void WriteObject (PdfWriter writer)
    {
        writer.WriteBeginObject (this);
        var count = Elements.Count;
        for (var idx = 0; idx < count; idx++)
        {
            var value = Elements[idx];
            value.WriteObject (writer);
        }

        writer.WriteEndObject();
    }

    /// <summary>
    /// Represents the elements of an PdfArray.
    /// </summary>
    public sealed class ArrayElements : IList<PdfItem>, ICloneable
    {
        internal ArrayElements (PdfArray array)
        {
            _elements = new List<PdfItem>();
            _ownerArray = array;
        }

        object ICloneable.Clone()
        {
            var elements = (ArrayElements)MemberwiseClone();
            elements._elements = new List<PdfItem> (elements._elements);
            elements._ownerArray = null;
            return elements;
        }

        /// <summary>
        /// Creates a shallow copy of this object.
        /// </summary>
        public ArrayElements Clone()
        {
            return (ArrayElements)((ICloneable)this).Clone();
        }

        /// <summary>
        /// Moves this instance to another array during object type transformation.
        /// </summary>
        internal void ChangeOwner (PdfArray array)
        {
            if (_ownerArray != null)
            {
                // ???
            }

            // Set new owner.
            _ownerArray = array;

            // Set owners elements to this.
            array._elements = this;
        }

        /// <summary>
        /// Converts the specified value to boolean.
        /// If the value does not exist, the function returns false.
        /// If the value is not convertible, the function throws an InvalidCastException.
        /// If the index is out of range, the function throws an ArgumentOutOfRangeException.
        /// </summary>
        public bool GetBoolean (int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException ("index", index, PSSR.IndexOutOfRange);
            }

            object obj = this[index];
            if (obj == null || obj is PdfNull)
            {
                return false;
            }

            var boolean = obj as PdfBoolean;
            if (boolean != null)
            {
                return boolean.Value;
            }

            var booleanObject = obj as PdfBooleanObject;
            if (booleanObject != null)
            {
                return booleanObject.Value;
            }

            throw new InvalidCastException ("GetBoolean: Object is not a boolean.");
        }

        /// <summary>
        /// Converts the specified value to integer.
        /// If the value does not exist, the function returns 0.
        /// If the value is not convertible, the function throws an InvalidCastException.
        /// If the index is out of range, the function throws an ArgumentOutOfRangeException.
        /// </summary>
        public int GetInteger (int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException ("index", index, PSSR.IndexOutOfRange);
            }

            object obj = this[index];
            if (obj == null || obj is PdfNull)
            {
                return 0;
            }

            var integer = obj as PdfInteger;
            if (integer != null)
            {
                return integer.Value;
            }

            var integerObject = obj as PdfIntegerObject;
            if (integerObject != null)
            {
                return integerObject.Value;
            }

            throw new InvalidCastException ("GetInteger: Object is not an integer.");
        }

        /// <summary>
        /// Converts the specified value to double.
        /// If the value does not exist, the function returns 0.
        /// If the value is not convertible, the function throws an InvalidCastException.
        /// If the index is out of range, the function throws an ArgumentOutOfRangeException.
        /// </summary>
        public double GetReal (int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException ("index", index, PSSR.IndexOutOfRange);
            }

            object obj = this[index];
            if (obj == null || obj is PdfNull)
            {
                return 0;
            }

            var real = obj as PdfReal;
            if (real != null)
            {
                return real.Value;
            }

            var realObject = obj as PdfRealObject;
            if (realObject != null)
            {
                return realObject.Value;
            }

            var integer = obj as PdfInteger;
            if (integer != null)
            {
                return integer.Value;
            }

            var integerObject = obj as PdfIntegerObject;
            if (integerObject != null)
            {
                return integerObject.Value;
            }

            throw new InvalidCastException ("GetReal: Object is not a number.");
        }

        /// <summary>
        /// Converts the specified value to string.
        /// If the value does not exist, the function returns the empty string.
        /// If the value is not convertible, the function throws an InvalidCastException.
        /// If the index is out of range, the function throws an ArgumentOutOfRangeException.
        /// </summary>
        public string GetString (int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException ("index", index, PSSR.IndexOutOfRange);
            }

            object obj = this[index];
            if (obj == null || obj is PdfNull)
            {
                return String.Empty;
            }

            var str = obj as PdfString;
            if (str != null)
            {
                return str.Value;
            }

            var strObject = obj as PdfStringObject;
            if (strObject != null)
            {
                return strObject.Value;
            }

            throw new InvalidCastException ("GetString: Object is not a string.");
        }

        /// <summary>
        /// Converts the specified value to a name.
        /// If the value does not exist, the function returns the empty string.
        /// If the value is not convertible, the function throws an InvalidCastException.
        /// If the index is out of range, the function throws an ArgumentOutOfRangeException.
        /// </summary>
        public string GetName (int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException ("index", index, PSSR.IndexOutOfRange);
            }

            object obj = this[index];
            if (obj == null || obj is PdfNull)
            {
                return String.Empty;
            }

            var name = obj as PdfName;
            if (name != null)
            {
                return name.Value;
            }

            var nameObject = obj as PdfNameObject;
            if (nameObject != null)
            {
                return nameObject.Value;
            }

            throw new InvalidCastException ("GetName: Object is not a name.");
        }

        /// <summary>
        /// Gets the PdfObject with the specified index, or null, if no such object exists. If the index refers to
        /// a reference, the referenced PdfObject is returned.
        /// </summary>
        public PdfObject GetObject (int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException ("index", index, PSSR.IndexOutOfRange);
            }

            var item = this[index];
            var reference = item as PdfReference;
            if (reference != null)
            {
                return reference.Value;
            }

            return item as PdfObject;
        }

        /// <summary>
        /// Gets the PdfArray with the specified index, or null, if no such object exists. If the index refers to
        /// a reference, the referenced PdfArray is returned.
        /// </summary>
        public PdfDictionary GetDictionary (int index)
        {
            return GetObject (index) as PdfDictionary;
        }

        /// <summary>
        /// Gets the PdfArray with the specified index, or null, if no such object exists. If the index refers to
        /// a reference, the referenced PdfArray is returned.
        /// </summary>
        public PdfArray GetArray (int index)
        {
            return GetObject (index) as PdfArray;
        }

        /// <summary>
        /// Gets the PdfReference with the specified index, or null, if no such object exists.
        /// </summary>
        public PdfReference GetReference (int index)
        {
            var item = this[index];
            return item as PdfReference;
        }

        /// <summary>
        /// Gets all items of this array.
        /// </summary>
        public PdfItem[] Items
        {
            get { return _elements.ToArray(); }
        }

        #region IList Members

        /// <summary>
        /// Returns false.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets or sets an item at the specified index.
        /// </summary>
        /// <value></value>
        public PdfItem this [int index]
        {
            get { return _elements[index]; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException ("value");
                }

                _elements[index] = value;
            }
        }

        /// <summary>
        /// Removes the item at the specified index.
        /// </summary>
        public void RemoveAt (int index)
        {
            _elements.RemoveAt (index);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the array/>.
        /// </summary>
        public bool Remove (PdfItem item)
        {
            return _elements.Remove (item);
        }

        /// <summary>
        /// Inserts the item the specified index.
        /// </summary>
        public void Insert (int index, PdfItem value)
        {
            _elements.Insert (index, value);
        }

        /// <summary>
        /// Determines whether the specified value is in the array.
        /// </summary>
        public bool Contains (PdfItem value)
        {
            return _elements.Contains (value);
        }

        /// <summary>
        /// Removes all items from the array.
        /// </summary>
        public void Clear()
        {
            _elements.Clear();
        }

        /// <summary>
        /// Gets the index of the specified item.
        /// </summary>
        public int IndexOf (PdfItem value)
        {
            return _elements.IndexOf (value);
        }

        /// <summary>
        /// Appends the specified object to the array.
        /// </summary>
        public void Add (PdfItem value)
        {
            // TODO: ???
            //Debug.Assert((value is PdfObject && ((PdfObject)value).Reference == null) | !(value is PdfObject),
            //  "You try to set an indirect object directly into an array.");

            var obj = value as PdfObject;
            if (obj != null && obj.IsIndirect)
            {
                _elements.Add (obj.Reference);
            }
            else
            {
                _elements.Add (value);
            }
        }

        /// <summary>
        /// Returns false.
        /// </summary>
        public bool IsFixedSize
        {
            get { return false; }
        }

        #endregion

        #region ICollection Members

        /// <summary>
        /// Returns false.
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the number of elements in the array.
        /// </summary>
        public int Count
        {
            get { return _elements.Count; }
        }

        /// <summary>
        /// Copies the elements of the array to the specified array.
        /// </summary>
        public void CopyTo (PdfItem[] array, int index)
        {
            _elements.CopyTo (array, index);
        }

        /// <summary>
        /// The current implementation return null.
        /// </summary>
        public object SyncRoot
        {
            get { return null; }
        }

        #endregion

        /// <summary>
        /// Returns an enumerator that iterates through the array.
        /// </summary>
        public IEnumerator<PdfItem> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        /// <summary>
        /// The elements of the array.
        /// </summary>
        List<PdfItem> _elements;

        /// <summary>
        /// The array this objects belongs to.
        /// </summary>
        PdfArray _ownerArray;
    }

    private ArrayElements? _elements;

    /// <summary>
    /// Gets the DebuggerDisplayAttribute text.
    /// </summary>

    // ReSharper disable UnusedMember.Local
    string DebuggerDisplay

        // ReSharper restore UnusedMember.Local
    {
        get
        {
            return String.Format (CultureInfo.InvariantCulture, "array({0},[{1}])", ObjectID.DebuggerDisplay,
                _elements == null ? 0 : _elements.Count);
        }
    }
}
