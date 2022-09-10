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
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.IO;

using AM.Text;

using PdfSharpCore.Pdf.IO;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Provides access to the internal document data structures. This class prevents the public
/// interfaces from pollution with to much internal functions.
/// </summary>
public class PdfInternals // TODO: PdfDocumentInternals... PdfPageInterals etc.
{
    internal PdfInternals (PdfDocument document)
    {
        _document = document;
    }

    readonly PdfDocument _document;

    /// <summary>
    /// Gets or sets the first document identifier.
    /// </summary>
    public string FirstDocumentID
    {
        get => _document._trailer.GetDocumentID (0);
        set => _document._trailer.SetDocumentID (0, value);
    }

    /// <summary>
    /// Gets the first document identifier as GUID.
    /// </summary>
    public Guid FirstDocumentGuid => GuidFromString (_document._trailer.GetDocumentID (0));

    /// <summary>
    /// Gets or sets the second document identifier.
    /// </summary>
    public string SecondDocumentID
    {
        get => _document._trailer.GetDocumentID (1);
        set => _document._trailer.SetDocumentID (1, value);
    }

    /// <summary>
    /// Gets the first document identifier as GUID.
    /// </summary>
    public Guid SecondDocumentGuid => GuidFromString (_document._trailer.GetDocumentID (0));

    Guid GuidFromString (string? id)
    {
        if (id is not { Length: 16 })
        {
            return Guid.Empty;
        }

        var builder = StringBuilderPool.Shared.Get();
        for (var idx = 0; idx < 16; idx++)
        {
            builder.Append ($"{(byte)id[idx]:X2}");
        }

        return new Guid (builder.ReturnShared());
    }

    /// <summary>
    /// Gets the catalog dictionary.
    /// </summary>
    public PdfCatalog Catalog => _document.Catalog;

    /// <summary>
    /// Gets the ExtGStateTable object.
    /// </summary>
    public PdfExtGStateTable ExtGStateTable => _document.ExtGStateTable;

    /// <summary>
    /// Returns the object with the specified Identifier, or null, if no such object exists.
    /// </summary>
    public PdfObject GetObject (PdfObjectID objectID)
    {
        return _document._irefTable[objectID].Value;
    }

    /// <summary>
    /// Maps the specified external object to the substitute object in this document.
    /// Returns null if no such object exists.
    /// </summary>
    public PdfObject MapExternalObject (PdfObject externalObject)
    {
        var table = _document.FormTable;
        var iot = table.GetImportedObjectTable (externalObject.Owner);
        var reference = iot[externalObject.ObjectID];

        return reference == null ? null : reference.Value;
    }

    /// <summary>
    /// Returns the PdfReference of the specified object, or null, if the object is not in the
    /// document's object table.
    /// </summary>
    public static PdfReference GetReference (PdfObject obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException ("obj");
        }

        return obj.Reference;
    }

    /// <summary>
    /// Gets the object identifier of the specified object.
    /// </summary>
    public static PdfObjectID GetObjectID (PdfObject obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException ("obj");
        }

        return obj.ObjectID;
    }

    /// <summary>
    /// Gets the object number of the specified object.
    /// </summary>
    public static int GetObjectNumber (PdfObject obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException ("obj");
        }

        return obj.ObjectNumber;
    }

    /// <summary>
    /// Gets the generation number of the specified object.
    /// </summary>
    public static int GenerationNumber (PdfObject obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException ("obj");
        }

        return obj.GenerationNumber;
    }

    /// <summary>
    /// Gets all indirect objects ordered by their object identifier.
    /// </summary>
    public PdfObject[] GetAllObjects()
    {
        var irefs = _document._irefTable.AllReferences;
        var count = irefs.Length;
        var objects = new PdfObject[count];
        for (var idx = 0; idx < count; idx++)
        {
            objects[idx] = irefs[idx].Value;
        }

        return objects;
    }

    /// <summary>
    /// Creates the indirect object of the specified type, adds it to the document, and
    /// returns the object.
    /// </summary>
    public T CreateIndirectObject<T>() where T : PdfObject
    {
        T result = null;
        ConstructorInfo ctorInfo = null; // TODO
        if (ctorInfo != null)
        {
            result = (T)ctorInfo.Invoke (new object[] { _document });
            Debug.Assert (result != null);
            AddObject (result);
        }

        Debug.Assert (result != null, "CreateIndirectObject failed with type " + typeof (T).FullName);
        return result;
    }

    /// <summary>
    /// Adds an object to the PDF document. This operation and only this operation makes the object
    /// an indirect object owned by this document.
    /// </summary>
    public void AddObject (PdfObject obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException ("obj");
        }

        if (obj.Owner == null)
        {
            obj.Document = _document;
        }
        else if (obj.Owner != _document)
        {
            throw new InvalidOperationException ("Object does not belong to this document.");
        }

        _document._irefTable.Add (obj);
    }

    /// <summary>
    /// Removes an object from the PDF document.
    /// </summary>
    public void RemoveObject (PdfObject obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException ("obj");
        }

        if (obj.Reference == null)
        {
            throw new InvalidOperationException ("Only indirect objects can be removed.");
        }

        if (obj.Owner != _document)
        {
            throw new InvalidOperationException ("Object does not belong to this document.");
        }

        _document._irefTable.Remove (obj.Reference);
    }

    /// <summary>
    /// Returns an array containing the specified object as first element follows by its transitive
    /// closure. The closure of an object are all objects that can be reached by indirect references.
    /// The transitive closure is the result of applying the calculation of the closure to a closure
    /// as long as no new objects came along. This is e.g. useful for getting all objects belonging
    /// to the resources of a page.
    /// </summary>
    public PdfObject[] GetClosure (PdfObject obj)
    {
        return GetClosure (obj, Int32.MaxValue);
    }

    /// <summary>
    /// Returns an array containing the specified object as first element follows by its transitive
    /// closure limited by the specified number of iterations.
    /// </summary>
    public PdfObject[] GetClosure (PdfObject obj, int depth)
    {
        var references = _document._irefTable.TransitiveClosure (obj, depth);
        var count = references.Length + 1;
        var objects = new PdfObject[count];
        objects[0] = obj;
        for (var idx = 1; idx < count; idx++)
        {
            objects[idx] = references[idx - 1].Value;
        }

        return objects;
    }

    /// <summary>
    /// Writes a PdfItem into the specified stream.
    /// </summary>

    // This function exists to keep PdfWriter and PdfItem.WriteObject internal.
    public void WriteObject (Stream stream, PdfItem item)
    {
        // Never write an encrypted object
        var writer = new PdfWriter (stream, null);
        writer.Options = PdfWriterOptions.OmitStream;
        item.WriteObject (writer);
    }

    /// <summary>
    /// The name of the custom value key.
    /// </summary>
    public string CustomValueKey = "/PdfSharpCore.CustomValue";
}
