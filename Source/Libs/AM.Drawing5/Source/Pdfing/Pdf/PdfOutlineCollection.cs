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


using AM;

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections;

#endregion

#nullable enable
using PdfSharpCore.Drawing;

// Review: CountOpen does not work. - StL/14-10-05

namespace PdfSharpCore.Pdf;

/// <summary>
/// Represents a collection of outlines.
/// </summary>
public class PdfOutlineCollection
    : PdfObject, IList<PdfOutline>
{
    #region Construction

    /// <summary>
    /// Can only be created as part of PdfOutline.
    /// </summary>
    internal PdfOutlineCollection
        (
            PdfDocument document,
            PdfOutline parent
        )
        : base (document)
    {
        _parent = parent;
    }

    #endregion

    /// <summary>
    /// Removes the first occurrence of a specific item from the collection.
    /// </summary>
    public bool Remove (PdfOutline item)
    {
        if (_outlines.Remove (item))
        {
            RemoveFromOutlinesTree (item);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the number of entries in this collection.
    /// </summary>
    public int Count => _outlines.Count;

    /// <summary>
    /// Returns false.
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// Adds the specified outline.
    /// </summary>
    public void Add (PdfOutline outline)
    {
        if (outline == null)
        {
            throw new ArgumentNullException (nameof (outline));
        }

        // DestinationPage is optional. PDFsharp does not yet support outlines with action ("/A") instead of destination page ("/DEST")
        if (outline.DestinationPage != null && !ReferenceEquals (Owner, outline.DestinationPage.Owner))
        {
            throw new ArgumentException ("Destination page must belong to this document.");
        }

        //// TODO check the parent problems...
        ////outline.Document = Owner;
        ////outline.Parent = _parent;
        ////Owner._irefTable.Add(outline);

        AddToOutlinesTree (outline);
        _outlines.Add (outline);

        if (outline.Opened)
        {
            outline = _parent;
            while (outline != null)
            {
                outline.OpenCount++;
                outline = outline.Parent;
            }
        }
    }

    /// <summary>
    /// Removes all elements form the collection.
    /// </summary>
    public void Clear()
    {
        if (Count > 0)
        {
            var array = new PdfOutline[Count];
            _outlines.CopyTo (array);
            _outlines.Clear();
            foreach (var item in array)
            {
                RemoveFromOutlinesTree (item);
            }
        }
    }

    /// <summary>
    /// Determines whether the specified element is in the collection.
    /// </summary>
    public bool Contains (PdfOutline item)
    {
        return _outlines.Contains (item);
    }

    /// <summary>
    /// Copies the collection to an array, starting at the specified index of the target array.
    /// </summary>
    public void CopyTo (PdfOutline[] array, int arrayIndex)
    {
        _outlines.CopyTo (array, arrayIndex);
    }

    /// <summary>
    /// Adds the specified outline entry.
    /// </summary>
    /// <param name="title">The outline text.</param>
    /// <param name="destinationPage">The destination page.</param>
    /// <param name="opened">Specifies whether the node is displayed expanded (opened) or collapsed.</param>
    /// <param name="style">The font style used to draw the outline text.</param>
    /// <param name="textColor">The color used to draw the outline text.</param>
    public PdfOutline Add (string title, PdfPage destinationPage, bool opened, PdfOutlineStyle style, XColor textColor)
    {
        var outline = new PdfOutline (title, destinationPage, opened, style, textColor);
        Add (outline);
        return outline;
    }

    /// <summary>
    /// Adds the specified outline entry.
    /// </summary>
    /// <param name="title">The outline text.</param>
    /// <param name="destinationPage">The destination page.</param>
    /// <param name="opened">Specifies whether the node is displayed expanded (opened) or collapsed.</param>
    /// <param name="style">The font style used to draw the outline text.</param>
    public PdfOutline Add (string title, PdfPage destinationPage, bool opened, PdfOutlineStyle style)
    {
        var outline = new PdfOutline (title, destinationPage, opened, style);
        Add (outline);
        return outline;
    }

    /// <summary>
    /// Adds the specified outline entry.
    /// </summary>
    /// <param name="title">The outline text.</param>
    /// <param name="destinationPage">The destination page.</param>
    /// <param name="opened">Specifies whether the node is displayed expanded (opened) or collapsed.</param>
    public PdfOutline Add (string title, PdfPage destinationPage, bool opened)
    {
        var outline = new PdfOutline (title, destinationPage, opened);
        Add (outline);
        return outline;
    }

    /// <summary>
    /// Creates a PdfOutline and adds it into the outline collection.
    /// </summary>
    public PdfOutline Add (string title, PdfPage destinationPage)
    {
        var outline = new PdfOutline (title, destinationPage);
        Add (outline);
        return outline;
    }

    /// <summary>
    /// Gets the index of the specified item.
    /// </summary>
    public int IndexOf (PdfOutline item)
    {
        return _outlines.IndexOf (item);
    }

    /// <summary>
    /// Inserts the item at the specified index.
    /// </summary>
    public void Insert (int index, PdfOutline outline)
    {
        if (outline == null)
        {
            throw new ArgumentNullException (nameof (outline));
        }

        if (index < 0 || index >= _outlines.Count)
        {
            throw new ArgumentOutOfRangeException (nameof (index), index, PSSR.OutlineIndexOutOfRange);
        }

        AddToOutlinesTree (outline);
        _outlines.Insert (index, outline);
    }

    /// <summary>
    /// Removes the outline item at the specified index.
    /// </summary>
    public void RemoveAt (int index)
    {
        var outline = _outlines[index];
        _outlines.RemoveAt (index);
        RemoveFromOutlinesTree (outline);
    }

    /// <summary>
    /// Gets the <see cref="PdfSharpCore.Pdf.PdfOutline"/> at the specified index.
    /// </summary>
    public PdfOutline this [int index]
    {
        get
        {
            if (index < 0 || index >= _outlines.Count)
            {
                throw new ArgumentOutOfRangeException (nameof (index), index, PSSR.OutlineIndexOutOfRange);
            }

            return _outlines[index];
        }
        set
        {
            if (index < 0 || index >= _outlines.Count)
            {
                throw new ArgumentOutOfRangeException (nameof (index), index, PSSR.OutlineIndexOutOfRange);
            }

            if (value == null)
            {
                throw new ArgumentOutOfRangeException (nameof (value), null, PSSR.SetValueMustNotBeNull);
            }

            AddToOutlinesTree (value);
            _outlines[index] = value;
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the outline collection.
    /// </summary>
    public IEnumerator<PdfOutline> GetEnumerator()
    {
        return _outlines.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal int CountOpen()
    {
        var count = 0;

        //foreach (PdfOutline outline in _outlines)
        //    count += outline.CountOpen();
        return count;
    }

    void AddToOutlinesTree (PdfOutline outline)
    {
        Sure.NotNull (outline);

        // DestinationPage is optional. PDFsharp does not yet support outlines with action ("/A") instead of destination page ("/DEST")
        if (outline.DestinationPage != null && !ReferenceEquals (Owner, outline.DestinationPage.Owner))
        {
            throw new ArgumentException ("Destination page must belong to this document.");
        }

        // TODO check the parent problems...
        outline.Document = Owner;
        outline.Parent = _parent;

        //_outlines.Add(outline);
        if (!Owner._irefTable.Contains (outline.ObjectID))
        {
            Owner._irefTable.Add (outline);
        }

        // else
        // {
        //     outline.GetType();
        // }

        //if (outline.Opened)
        //{
        //    outline = _parent;
        //    while (outline != null)
        //    {
        //        outline.OpenCount++;
        //        outline = outline.Parent;
        //    }
        //}
    }

    void RemoveFromOutlinesTree (PdfOutline outline)
    {
        Sure.NotNull (outline);

        // TODO check the parent problems...
        //outline.Document = Owner;
        outline.Parent = null;

        Owner._irefTable.Remove (outline.Reference);
    }

    /// <summary>
    /// The parent outline of this collection.
    /// </summary>
    readonly PdfOutline _parent;

    readonly List<PdfOutline> _outlines = new ();
}
