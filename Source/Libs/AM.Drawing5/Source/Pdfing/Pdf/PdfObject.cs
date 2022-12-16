// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfObject.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using PdfSharpCore.Pdf.Advanced;
using PdfSharpCore.Pdf.IO;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Base class of all composite PDF objects.
/// </summary>
public abstract class PdfObject
    : PdfItem
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfObject"/> class.
    /// </summary>
    protected PdfObject()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfObject"/> class.
    /// </summary>
    protected PdfObject (PdfDocument? document)
    {
        // Calling a virtual member in a constructor is dangerous.
        // In PDFsharp Document is overridden in PdfPage and the code is checked to be save
        // when called for a not completely initialized object.
        Document = document;
    }

    /// <summary>
    /// Initializes a new instance from an existing object. Used for object type transformation.
    /// </summary>
    protected PdfObject (PdfObject obj)
        : this (obj.Owner)
    {
        // If the object that was transformed to an instance of a derived class was an indirect object
        // set the value of the reference to this.
        if (obj.Reference != null)
        {
            obj.Reference.Value = this;
        }
#if DEBUG_ // BUG
            else
            {
                // If this occurs it is an internal error
                Debug.Assert(false, "Object type transformation must not be done with direct objects");
            }
#endif
    }

    #endregion

    /// <summary>
    /// Creates a copy of this object. The clone does not belong to a document, i.e. its owner and its iref are null.
    /// </summary>
    public new PdfObject Clone()
    {
        return (PdfObject)Copy();
    }

    /// <summary>
    /// Implements the copy mechanism. Must be overridden in derived classes.
    /// </summary>
    protected override object Copy()
    {
        var obj = (PdfObject)base.Copy();
        obj._document = null;
        obj.Reference = null;
        return obj;
    }

#if true_ // works, but may lead to other problems that I cannot assess
        /// <summary>
        /// Determines whether the specified object is equal to the current PdfObject.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is PdfObject)
            {
                PdfObject other = (PdfObject)obj;
                // Take object type transformation into account
                if (_iref != null && other._iref != null)
                {
                    Debug.Assert(_iref.Value != null, "iref without value.");
                    Debug.Assert(other.iref.Value != null, "iref without value.");
                    return Object.ReferenceEquals(_iref.Value, other.iref.Value);
                }
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            if (_iref != null)
            {
                Debug.Assert(_iref.Value != null, "iref without value.");
                return _iref.GetHashCode();
            }
            return base.GetHashCode();
        }
#endif

    /// <summary>
    /// Sets the object and generation number.
    /// Setting the object identifier makes this object an indirect object, i.e. the object gets
    /// a PdfReference entry in the PdfReferenceTable.
    /// </summary>
    internal void SetObjectID (int objectNumber, int generationNumber)
    {
        var objectID = new PdfObjectID (objectNumber, generationNumber);

        // TODO: check imported
        if (Reference == null)
        {
            Reference = _document!._irefTable[objectID];
        }

        if (Reference == null)
        {
            // ReSharper disable once ObjectCreationAsStatement because the new object is set to this object
            // in the constructor of PdfReference.
            new PdfReference (this);
            Debug.Assert (Reference != null);
            Reference.ObjectID = objectID;
        }

        Reference.Value = this;
        Reference.Document = _document;
    }

    /// <summary>
    /// Gets the PdfDocument this object belongs to.
    /// </summary>
    public virtual PdfDocument? Owner => _document;

    /// <summary>
    /// Sets the PdfDocument this object belongs to.
    /// </summary>
    internal virtual PdfDocument? Document
    {
        set
        {
            if (!ReferenceEquals (_document, value))
            {
                if (_document != null)
                {
                    throw new InvalidOperationException ("Cannot change document.");
                }

                _document = value;
                if (Reference != null)
                {
                    Reference.Document = value;
                }
            }
        }
    }

    internal PdfDocument? _document;

    /// <summary>
    /// Indicates whether the object is an indirect object.
    /// </summary>
    public bool IsIndirect =>

        // An object is an indirect object if and only if is has an indirect reference value.
        Reference != null;

    /// <summary>
    /// Gets the PdfInternals object of this document, that grants access to some internal structures
    /// which are not part of the public interface of PdfDocument.
    /// </summary>
    public PdfObjectInternals Internals
    {
        get { return _internals ??= new PdfObjectInternals (this); }
    }

    private PdfObjectInternals? _internals;

    /// <summary>
    /// When overridden in a derived class, prepares the object to get saved.
    /// </summary>
    internal virtual void PrepareForSave()
    {
    }

    /// <summary>
    /// Saves the stream position. 2nd Edition.
    /// </summary>
    internal override void WriteObject (PdfWriter writer)
    {
        Debug.Assert (false, "Must not come here!");

        //Debug.Assert(_inStreamOffset <= 0);
        //if (_inStreamOffset == 0)
        //{
        //    //_InStreamOffset = stream.Position;
        //    _document.xrefTable.AddObject(this);
        //    return Format("{0} {1} obj\n", _objectID, _generation);
        //}
        //else if (_inStreamOffset == -1)
        //{
        //}
        //return null;
    }

    /// <summary>
    /// Gets the object identifier. Returns PdfObjectID.Empty for direct objects,
    /// i.e. never returns null.
    /// </summary>
    internal PdfObjectID ObjectID => Reference?.ObjectID ?? PdfObjectID.Empty;

    /// <summary>
    /// Gets the object number.
    /// </summary>
    internal int ObjectNumber => ObjectID.ObjectNumber;

    /// <summary>
    /// Gets the generation number.
    /// </summary>
    internal int GenerationNumber => ObjectID.GenerationNumber;

    ///// <summary>
    ///// Creates a deep copy of the specified value and its transitive closure and adds the
    ///// new objects to the specified owner document.
    ///// </summary>
    /// <param name="owner">The document that owns the cloned objects.</param>
    /// <param name="externalObject">The root object to be cloned.</param>
    /// <returns>The clone of the root object</returns>
    internal static PdfObject DeepCopyClosure (PdfDocument owner, PdfObject externalObject)
    {
        // Get transitive closure.
        var elements = externalObject.Owner!.Internals.GetClosure (externalObject);
        var count = elements.Length;
#if DEBUG_
            for (int idx = 0; idx < count; idx++)
            {
                Debug.Assert(elements[idx].XRef != null);
                Debug.Assert(elements[idx].XRef.Document != null);
                Debug.Assert(elements[idx].Document != null);
                if (elements[idx].ObjectID.ObjectNumber == 12)
                    GetType();
            }
#endif

        // 1st loop. Replace all objects by their clones.
        var iot = new PdfImportedObjectTable (owner, externalObject.Owner);
        for (var idx = 0; idx < count; idx++)
        {
            var obj = elements[idx];
            var clone = obj.Clone();
            Debug.Assert (clone.Reference == null);
            clone.Document = owner;
            if (obj.Reference != null)
            {
                // Case: The cloned object was an indirect object.
                // Add clone to new owner document.
                owner._irefTable.Add (clone);

                // The clone gets an iref by adding it to its new owner.
                Debug.Assert (clone.Reference != null);

                // Save an association from old object identifier to new iref.
                iot.Add (obj.ObjectID, clone.Reference);
            }
            else
            {
                // Case: The cloned object was an direct object.
                // Only the root object can be a direct object.
                Debug.Assert (idx == 0);
            }

            // Replace external object by its clone.
            elements[idx] = clone;
        }
#if DEBUG_
            for (int idx = 0; idx < count; idx++)
            {
                Debug.Assert(elements[idx]._iref != null);
                Debug.Assert(elements[idx]._iref.Document != null);
                Debug.Assert(resources[idx].Document != null);
                if (elements[idx].ObjectID.ObjectNumber == 12)
                    GetType();
            }
#endif

        // 2nd loop. Fix up all indirect references that still refers to the import document.
        for (var idx = 0; idx < count; idx++)
        {
            var obj = elements[idx];
            Debug.Assert (obj.Owner == owner);
            FixUpObject (iot, owner, obj);
        }

        // Return the clone of the former root object.
        return elements[0];
    }

    ///// <summary>
    ///// Imports an object and its transitive closure to the specified document.
    ///// </summary>
    /// <param name="importedObjectTable">The imported object table of the owner for the external document.</param>
    /// <param name="owner">The document that owns the cloned objects.</param>
    /// <param name="externalObject">The root object to be cloned.</param>
    /// <returns>The clone of the root object</returns>
    internal static PdfObject ImportClosure (PdfImportedObjectTable importedObjectTable, PdfDocument owner,
        PdfObject externalObject)
    {
        Debug.Assert (ReferenceEquals (importedObjectTable.Owner, owner),
            "importedObjectTable does not belong to the owner.");
        Debug.Assert (ReferenceEquals (importedObjectTable.ExternalDocument, externalObject.Owner),
            "The ExternalDocument of the importedObjectTable does not belong to the owner of object to be imported.");

        // Get transitive closure of external object.
        var elements = externalObject.Owner!.Internals.GetClosure (externalObject);
        var count = elements.Length;
#if DEBUG_
            for (int idx = 0; idx < count; idx++)
            {
                Debug.Assert(elements[idx].XRef != null);
                Debug.Assert(elements[idx].XRef.Document != null);
                Debug.Assert(elements[idx].Document != null);
                if (elements[idx].ObjectID.ObjectNumber == 12)
                    GetType();
            }
#endif

        // 1st loop. Already imported objects are reused and new ones are cloned.
        for (var idx = 0; idx < count; idx++)
        {
            var obj = elements[idx];
            Debug.Assert (!ReferenceEquals (obj.Owner, owner));

            if (importedObjectTable.Contains (obj.ObjectID))
            {
#if DEBUG_
                    if (obj.ObjectID.ObjectNumber == 5894)
                        obj.GetType();
#endif

                // Case: External object was already imported.
                var iref = importedObjectTable[obj.ObjectID];
                Debug.Assert (iref != null);
                Debug.Assert (iref.Value != null);
                Debug.Assert (iref.Document == owner);

                // Replace external object by the already cloned counterpart.
                elements[idx] = iref.Value;
            }
            else
            {
                // Case: External object was not yet imported ealier and must be cloned.
                var clone = obj.Clone();
                Debug.Assert (clone.Reference == null);
                clone.Document = owner;
                if (obj.Reference != null)
                {
                    // Case: The cloned object was an indirect object.
                    // Add clone to new owner document.
                    owner._irefTable.Add (clone);
                    Debug.Assert (clone.Reference != null);

                    // Save an association from old object identifier to new iref.
                    importedObjectTable.Add (obj.ObjectID, clone.Reference);
                }
                else
                {
                    // Case: The cloned object was a direct object.
                    // Only the root object can be a direct object.
                    Debug.Assert (idx == 0);
                }

                // Replace external object by its clone.
                elements[idx] = clone;
            }
        }
#if DEBUG_
            for (int idx = 0; idx < count; idx++)
            {
                //Debug.Assert(elements[idx].Reference != null);
                //Debug.Assert(elements[idx].Reference.Document != null);
                Debug.Assert(elements[idx].IsIndirect == false);
                Debug.Assert(elements[idx].Owner != null);
                //if (elements[idx].ObjectID.ObjectNumber == 12)
                //    GetType();
            }
#endif

        // 2nd loop. Fix up indirect references that still refers to the external document.
        for (var idx = 0; idx < count; idx++)
        {
            var obj = elements[idx];
            Debug.Assert (owner != null);
            FixUpObject (importedObjectTable, importedObjectTable.Owner, obj);
        }

        // Return the imported root object.
        return elements[0];
    }

    /// <summary>
    /// Replace all indirect references to external objects by their cloned counterparts
    /// owned by the importer document.
    /// </summary>
    static void FixUpObject (PdfImportedObjectTable iot, PdfDocument owner, PdfObject value)
    {
        Debug.Assert (ReferenceEquals (iot.Owner, owner));

        if (value is PdfDictionary dict)
        {
            // Case: The object is a dictionary.
            // Set document for cloned direct objects.
            if (dict.Owner == null)
            {
                // If the dictionary has not yet an owner set the owner to the importing document.
                dict.Document = owner;
            }
            else
            {
                // If the dictionary already has an owner it must be the importing document.
                Debug.Assert (dict.Owner == owner);
            }

            // Search for indirect references in all dictionary elements.
            var names = dict.Elements.KeyNames;
            foreach (var name in names)
            {
                var item = dict.Elements[name];
                Debug.Assert (item != null, "A dictionary element cannot be null.");

                // Is item an iref?
                var iref = item as PdfReference;
                if (iref != null)
                {
                    // Case: The item is a reference.
                    // Does the iref already belongs to the new owner?
                    if (iref.Document == owner)
                    {
                        // Yes: fine. Happens when an already cloned object is reused.
                        continue;
                    }

                    //Debug.Assert(iref.Document == iot.Document);
                    // No: Replace with iref of cloned object.
                    var newXRef = iot[iref.ObjectID]; // TODO: Explain this line of code in all details.
                    Debug.Assert (newXRef != null);
                    Debug.Assert (newXRef.Document == owner);
                    dict.Elements[name] = newXRef;
                }
                else
                {
                    // Case: The item is not a reference.
                    // If item is an object recursively fix its inner items.
                    var pdfObject = item as PdfObject;
                    if (pdfObject != null)
                    {
                        // Fix up inner objects, i.e. recursively walk down the object tree.
                        FixUpObject (iot, owner, pdfObject);
                    }
                    else
                    {
                        // The item is something else, e.g. a name.
                        // Nothing to do.

                        // ...but let's double check this case in DEBUG build.
                        DebugCheckNonObjects (item);
                    }
                }
            }
        }
        else if (value is PdfArray array)
        {
            // Case: The object is an array.
            // Set document for cloned direct objects.
            if (array.Owner == null)
            {
                // If the array has not yet an owner set the owner to the importing document.
                array.Document = owner;
            }
            else
            {
                // If the array already has an owner it must be the importing document.
                Debug.Assert (array.Owner == owner);
            }

            // Search for indirect references in all array elements.
            var count = array.Elements.Count;
            for (var idx = 0; idx < count; idx++)
            {
                var item = array.Elements[idx];
                Debug.Assert (item != null, "An array element cannot be null.");

                // Is item an iref?
                if (item is PdfReference iref)
                {
                    // Case: The item is a reference.
                    // Does the iref already belongs to the owner?
                    if (iref.Document == owner)
                    {
                        // Yes: fine. Happens when an already cloned object is reused.
                        continue;
                    }

                    // No: replace with iref of cloned object.
                    Debug.Assert (iref.Document == iot.ExternalDocument);
                    var newXRef = iot[iref.ObjectID];
                    Debug.Assert (newXRef != null);
                    Debug.Assert (newXRef.Document == owner);
                    array.Elements[idx] = newXRef;
                }
                else
                {
                    // Case: The item is not a reference.
                    // If item is an object recursively fix its inner items.
                    if (item is PdfObject pdfObject)
                    {
                        // Fix up inner objects, i.e. recursively walk down the object tree.
                        FixUpObject (iot, owner, pdfObject);
                    }
                    else
                    {
                        // The item is something else, e.g. a name.
                        // Nothing to do.

                        // ...but let's double check this case in DEBUG build.
                        DebugCheckNonObjects (item);
                    }
                }
            }
        }
        else
        {
            // Case: The item is some other indirect object.
            // Indirect integers, booleans, etc. are allowed, but PDFsharp do not create them.
            // If such objects occur in imported PDF files from other producers, nothing more is to do.
            // The owner was already set, which is double checked by the assertions below.
            if (value is PdfNameObject || value is PdfStringObject || value is PdfBooleanObject ||
                value is PdfIntegerObject || value is PdfNumberObject)
            {
                Debug.Assert (value.IsIndirect);
                Debug.Assert (value.Owner == owner);
            }
            else
            {
                Debug.Assert (false, "Should not come here. Object is neither a dictionary nor an array.");
            }
        }
    }

    /// <summary>
    /// Ensure for future versions of PDFsharp not to forget code for a new kind of PdfItem.
    /// </summary>
    /// <param name="item">The item.</param>
    [Conditional ("DEBUG")]
    static void DebugCheckNonObjects (PdfItem item)
    {
        if (item is PdfName)
        {
            return;
        }

        if (item is PdfBoolean)
        {
            return;
        }

        if (item is PdfInteger)
        {
            return;
        }

        if (item is PdfNumber)
        {
            return;
        }

        if (item is PdfString)
        {
            return;
        }

        if (item is PdfRectangle)
        {
            return;
        }

        if (item is PdfNull)
        {
            return;
        }

        var type = item.GetType();
        Debug.Assert (type != null, $"CheckNonObjects: Add {type.Name} to the list.");
    }

    /// <summary>
    /// Gets the indirect reference of this object. If the value is null, this object is a direct object.
    /// </summary>
    public PdfReference? Reference
    {
        get;

        // Setting the reference outside PDFsharp is not considered as a valid operation.
        internal set;
    }
}
