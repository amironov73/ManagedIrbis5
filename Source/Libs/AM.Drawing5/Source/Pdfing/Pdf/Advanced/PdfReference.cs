// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* PdfReference.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#define UNIQUE_IREF_

// With this define each iref object gets a unique number (uid) to make them distinguishable in the debugger

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;

using PdfSharpCore.Pdf.IO;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Represents an indirect reference to a PdfObject.
/// </summary>
[DebuggerDisplay ("iref({ObjectNumber}, {GenerationNumber})")]
public sealed class PdfReference : PdfItem
{
    // About PdfReference
    //
    // * A PdfReference holds either the ObjectID or the PdfObject or both.
    //
    // * Each PdfObject has a PdfReference if and only if it is an indirect object. Direct objects have
    //   no PdfReference, because they are embedded in a parent objects.
    //
    // * PdfReference objects are used to reference PdfObject instances. A value in a PDF dictionary
    //   or array that is a PdfReference represents an indirect reference. A value in a PDF dictionary or
    //   or array that is a PdfObject represents a direct (or embeddded) object.
    //
    // * When a PDF file is imported, the PdfXRefTable is filled with PdfReference objects keeping the
    //   ObjectsIDs and file positions (offsets) of all indirect objects.
    //
    // * Indirect objects can easily be renumbered because they do not rely on their ObjectsIDs.
    //
    // * During modification of a document the ObjectID of an indirect object has no meaning,
    //   except that they must be different in pairs.

    /// <summary>
    /// Initializes a new PdfReference instance for the specified indirect object.
    /// </summary>
    public PdfReference (PdfObject pdfObject)
    {
        if (pdfObject.Reference != null)
        {
            throw new InvalidOperationException ("Must not create iref for an object that already has one.");
        }

        _value = pdfObject;
        pdfObject.Reference = this;
#if UNIQUE_IREF && DEBUG
            _uid = ++s_counter;
#endif
    }

    /// <summary>
    /// Initializes a new PdfReference instance from the specified object identifier and file position.
    /// </summary>
    public PdfReference (PdfObjectID objectID, long position)
    {
        _objectID = objectID;
        Position = position;
#if UNIQUE_IREF && DEBUG
            _uid = ++s_counter;
#endif
    }

    /// <summary>
    /// Writes the object in PDF iref table format.
    /// </summary>
    internal void WriteXRefEnty (PdfWriter writer)
    {
        // PDFsharp does not yet support PDF 1.5 object streams.

        // Each line must be exactly 20 bytes long, otherwise Acrobat repairs the file.
        var text = String.Format ("{0:0000000000} {1:00000} n\n",
            Position, _objectID.GenerationNumber); // InUse ? 'n' : 'f');
        writer.WriteRaw (text);
    }

    /// <summary>
    /// Writes an indirect reference.
    /// </summary>
    internal override void WriteObject (PdfWriter writer)
    {
        writer.Write (this);
    }

    /// <summary>
    /// Gets or sets the object identifier.
    /// </summary>
    public PdfObjectID ObjectID
    {
        get { return _objectID; }
        set
        {
            // Ignore redundant invokations.
            if (_objectID == value)
            {
                return;
            }

            _objectID = value;
            if (Document != null)
            {
                //PdfXRefTable table = Document.xrefTable;
                //table.Remove(this);
                //objectID = value;
                //table.Add(this);
            }
        }
    }

    private PdfObjectID _objectID;

    /// <summary>
    /// Gets the object number of the object identifier.
    /// </summary>
    public int ObjectNumber
    {
        get { return _objectID.ObjectNumber; }
    }

    /// <summary>
    /// Gets the generation number of the object identifier.
    /// </summary>
    public int GenerationNumber
    {
        get { return _objectID.GenerationNumber; }
    }

    /// <summary>
    /// Gets or sets the file position of the related PdfObject.
    /// </summary>
    public long Position { get; set; }

    //public bool InUse
    //{
    //  get {return inUse;}
    //  set {inUse = value;}
    //}
    //bool inUse;

    /// <summary>
    /// Gets or sets the referenced PdfObject.
    /// </summary>
    public PdfObject Value
    {
        get { return _value; }
        set
        {
            Debug.Assert (value != null, "The value of a PdfReference must never be null.");
            Debug.Assert (value.Reference == null || ReferenceEquals (value.Reference, this),
                "The reference of the value must be null or this.");
            _value = value;

            // value must never be null
            value.Reference = this;
        }
    }

    private PdfObject _value;

    /// <summary>
    /// Hack for dead objects.
    /// </summary>
    internal void SetObject (PdfObject value)
    {
        _value = value;
    }

    /// <summary>
    /// Gets or sets the document this object belongs to.
    /// </summary>
    public PdfDocument? Document { get; set; }

    /// <summary>
    /// Gets a string representing the object identifier.
    /// </summary>
    public override string ToString()
    {
        return _objectID + " R";
    }

    internal static PdfReferenceComparer Comparer => new ();

    /// <summary>
    /// Implements a comparer that compares PdfReference objects by their PdfObjectID.
    /// </summary>
    internal class PdfReferenceComparer
        : IComparer<PdfReference>
    {
        public int Compare (PdfReference? x, PdfReference? y)
        {
            var left = x;
            var right = y;
            if (left != null)
            {
                if (right != null)
                {
                    return left._objectID.CompareTo (right._objectID);
                }

                return -1;
            }

            if (right != null)
            {
                return 1;
            }

            return 0;
        }
    }

#if UNIQUE_IREF && DEBUG
        static int s_counter = 0;
        int _uid;
#endif
}
