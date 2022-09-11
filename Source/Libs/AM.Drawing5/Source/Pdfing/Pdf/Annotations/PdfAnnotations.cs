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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using AM;

using PdfSharpCore.Pdf.Advanced;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Annotations;

/// <summary>
/// Represents the annotations array of a page.
/// </summary>
public sealed class PdfAnnotations : PdfArray
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="document"></param>
    internal PdfAnnotations (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="array"></param>
    internal PdfAnnotations (PdfArray array)
        : base (array)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Adds the specified annotation.
    /// </summary>
    /// <param name="annotation">The annotation.</param>
    public void Add (PdfAnnotation annotation)
    {
        annotation.Document = Owner;
        Owner._irefTable.Add (annotation);
        Elements.Add (annotation.Reference.ThrowIfNull());
    }

    /// <summary>
    /// Removes an annotation from the document.
    /// </summary>
    public void Remove (PdfAnnotation annotation)
    {
        if (annotation.Owner != Owner)
        {
            throw new InvalidOperationException ("The annotation does not belong to this document.");
        }

        Owner.ThrowIfNull().Internals.RemoveObject (annotation);
        Elements.Remove (annotation.Reference.ThrowIfNull());
    }

    /// <summary>
    /// Removes all the annotations from the current page.
    /// </summary>
    public void Clear()
    {
        var page = Page.ThrowIfNull();
        for (var idx = Count - 1; idx >= 0; idx--)
        {
            page.Annotations.Remove (page.Annotations[idx]);
        }
    }

    //public void Insert(int index, PdfAnnotation annotation)
    //{
    //  annotation.Document = Document;
    //  annotations.Insert(index, annotation);
    //}

    /// <summary>
    /// Gets the number of annotations in this collection.
    /// </summary>
    public int Count => Elements.Count;

    /// <summary>
    /// Gets the <see cref="PdfSharpCore.Pdf.Annotations.PdfAnnotation"/> at the specified index.
    /// </summary>
    public PdfAnnotation this [int index]
    {
        get
        {
            PdfReference iref;
            PdfDictionary dict;
            var item = Elements[index];
            if ((iref = item as PdfReference) != null)
            {
                Debug.Assert (iref.Value is PdfDictionary, "Reference to dictionary expected.");
                dict = (PdfDictionary)iref.Value;
            }
            else
            {
                Debug.Assert (item is PdfDictionary, "Dictionary expected.");
                dict = (PdfDictionary)item;
            }

            var annotation = dict as PdfAnnotation;
            if (annotation == null)
            {
                annotation = new PdfGenericAnnotation (dict);
                if (iref == null)
                {
                    Elements[index] = annotation;
                }
            }

            return annotation;
        }
    }

    //public PdfAnnotation this[int index]
    //{
    //  get
    //  {
    //      //DMH 6/7/06
    //      //Broke this out to simplfy debugging
    //      //Use a generic annotation to access the Meta data
    //      //Assign this as the parent of the annotation
    //      PdfReference r = Elements[index] as PdfReference;
    //      PdfDictionary d = r.Value as PdfDictionary;
    //      PdfGenericAnnotation a = new PdfGenericAnnotation(d);
    //      a.Collection = this;
    //      return a;
    //  }
    //}

    /// <summary>
    /// Gets the page the annotations belongs to.
    /// </summary>
    internal PdfPage? Page { get; set; }

    /// <summary>
    /// Fixes the /P element in imported annotation.
    /// </summary>
    internal static void FixImportedAnnotation (PdfPage page)
    {
        var annots = page.Elements.GetArray (PdfPage.Keys.Annots);
        if (annots != null)
        {
            var count = annots.Elements.Count;
            for (var idx = 0; idx < count; idx++)
            {
                var annot = annots.Elements.GetDictionary (idx);
                if (annot != null && annot.Elements.ContainsKey ("/P"))
                {
                    annot.Elements["/P"] = page.Reference;
                }
            }
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    public override IEnumerator<PdfItem> GetEnumerator()
    {
        return new AnnotationsIterator (this);
    }

    // THHO4STLA: AnnotationsIterator: Implementation does not work http://forum.PdfSharpCore.net/viewtopic.php?p=3285#p3285
    // Code using the enumerator like this will crash:
    //foreach (var annotation in page.Annotations)
    //{
    //    annotation.GetType();
    //}

    //!!!new 2015-10-15: use PdfItem instead of PdfAnnotation.
    // TODO Should we change this to "public new IEnumerator<PdfAnnotation> GetEnumerator()"?

    class AnnotationsIterator
        : IEnumerator<PdfItem>
    {
        public AnnotationsIterator (PdfAnnotations annotations)
        {
            _annotations = annotations;
            _index = -1;
        }

        public PdfItem /*PdfAnnotation*/ Current => _annotations[_index];

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            return ++_index < _annotations.Count;
        }

        public void Reset()
        {
            _index = -1;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        readonly PdfAnnotations _annotations;
        int _index;
    }
}
