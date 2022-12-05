// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* PdfPages.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;

using AM;

using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.Advanced;
using PdfSharpCore.Pdf.Annotations;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Represents the pages of the document.
/// </summary>
[DebuggerDisplay ("(PageCount={Count})")]
public sealed class PdfPages
    : PdfDictionary, IEnumerable<PdfPage>
{
    #region Construction

    internal PdfPages (PdfDocument document)
        : base (document)
    {
        Elements.SetName (Keys.Type, "/Pages");
        Elements[Keys.Count] = new PdfInteger (0);
    }

    internal PdfPages (PdfDictionary dictionary)
        : base (dictionary)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Gets the number of pages.
    /// </summary>
    public int Count => PagesArray.Elements.Count;

    /// <summary>
    /// Gets the page with the specified index.
    /// </summary>
    public PdfPage this [int index]
    {
        get
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException (nameof (index), index, PSSR.PageIndexOutOfRange);
            }

            var dict = (PdfDictionary)((PdfReference)PagesArray.Elements[index]).Value;
            if (!(dict is PdfPage))
            {
                dict = new PdfPage (dict);
            }

            return (PdfPage)dict;
        }
    }

    /// <summary>
    /// Finds a page by its id. Transforms it to PdfPage if necessary.
    /// </summary>
    internal PdfPage? FindPage (PdfObjectID id) // TODO: public?
    {
        PdfPage? page = null;
        foreach (var item in PagesArray)
        {
            if (item is PdfReference reference)
            {
                if (reference.Value is PdfDictionary dictionary && dictionary.ObjectID == id)
                {
                    page = dictionary as PdfPage ?? new PdfPage (dictionary);
                    break;
                }
            }
        }

        return page;
    }

    /// <summary>
    /// Creates a new PdfPage, adds it to the end of this document, and returns it.
    /// </summary>
    public PdfPage Add()
    {
        var page = new PdfPage();
        Insert (Count, page);
        return page;
    }

    /// <summary>
    /// Adds the specified PdfPage to the end of this document and maybe returns a new PdfPage object.
    /// The value returned is a new object if the added page comes from a foreign document.
    /// </summary>
    public PdfPage Add (PdfPage page, AnnotationCopyingType annotationCopying = AnnotationCopyingType.DoNotCopy)
    {
        return Insert (Count, page, annotationCopying);
    }

    /// <summary>
    /// Creates a new PdfPage, inserts it at the specified position into this document, and returns it.
    /// </summary>
    public PdfPage Insert (int index)
    {
        var page = new PdfPage();
        Insert (index, page);
        return page;
    }

    /// <summary>
    /// Inserts the specified PdfPage at the specified position to this document and maybe returns a new PdfPage object.
    /// The value returned is a new object if the inserted page comes from a foreign document.
    /// </summary>
    public PdfPage Insert (int index, PdfPage page,
        AnnotationCopyingType annotationCopying = AnnotationCopyingType.DoNotCopy)
    {
        if (page == null)
        {
            throw new ArgumentNullException (nameof (page));
        }

        // Is the page already owned by this document?
        if (page.Owner == Owner)
        {
            // Case: Page is first removed and than inserted again, maybe at another position.
            var count = Count;

            // Check if page is not already part of the document.
            for (var idx = 0; idx < count; idx++)
            {
                if (ReferenceEquals (this[idx], page))
                {
                    throw new InvalidOperationException (PSSR.MultiplePageInsert);
                }
            }

            // TODO: check this case
            // Because the owner of the inserted page is this document we assume that the page was former part of it
            // and it is therefore well-defined.
            Owner!._irefTable.Add (page);
            Debug.Assert (page.Owner == Owner);

            // Insert page in array.
            PagesArray.Elements.Insert (index, page.Reference!);

            // Update page count.
            Elements.SetInteger (Keys.Count, PagesArray.Elements.Count);

            return page;
        }

        // All new page insertions come here.
        if (page.Owner == null)
        {
            // Case: New page was newly created and inserted now.
            page.Document = Owner;

            Owner!._irefTable.Add (page);
            Debug.Assert (page.Owner == Owner);
            PagesArray.Elements.Insert (index, page.Reference!);
            Elements.SetInteger (Keys.Count, PagesArray.Elements.Count);
        }
        else
        {
            // Case: Page is from an external document -> import it.
            var importPage = page;
            page = ImportExternalPage (importPage, annotationCopying);
            Owner!._irefTable.Add (page);

            // Add page substitute to importedObjectTable.
            var importedObjectTable = Owner.FormTable.GetImportedObjectTable (importPage);
            importedObjectTable.Add (importPage.ObjectID, page.Reference!);

            PagesArray.Elements.Insert (index, page.Reference!);
            Elements.SetInteger (Keys.Count, PagesArray.Elements.Count);
            PdfAnnotations.FixImportedAnnotation (page);
        }

        if (Owner.Settings.TrimMargins.AreSet)
        {
            page.TrimMargins = Owner.Settings.TrimMargins;
        }

        return page;
    }

    /// <summary>
    /// Inserts  pages of the specified document into this document.
    /// </summary>
    /// <param name="index">The index in this document where to insert the page .</param>
    /// <param name="document">The document to be inserted.</param>
    /// <param name="startIndex">The index of the first page to be inserted.</param>
    /// <param name="pageCount">The number of pages to be inserted.</param>
    /// <param name="annotationCopying">Annotation copying action, by default annotations are not copied.</param>
    public void InsertRange (int index, PdfDocument document, int startIndex, int pageCount,
        AnnotationCopyingType annotationCopying = AnnotationCopyingType.DoNotCopy)
    {
        if (document == null)
        {
            throw new ArgumentNullException (nameof (document));
        }

        if (index < 0 || index > Count)
        {
            throw new ArgumentOutOfRangeException (nameof (index), "Argument 'index' out of range.");
        }

        var importDocumentPageCount = document.PageCount;

        if (startIndex < 0 || startIndex + pageCount > importDocumentPageCount)
        {
            throw new ArgumentOutOfRangeException (nameof (startIndex), "Argument 'startIndex' out of range.");
        }

        if (pageCount > importDocumentPageCount)
        {
            throw new ArgumentOutOfRangeException (nameof (pageCount), "Argument 'pageCount' out of range.");
        }

        var insertPages = new PdfPage[pageCount];
        var importPages = new PdfPage[pageCount];

        // 1st create all new pages.
        for (int idx = 0, insertIndex = index, importIndex = startIndex;
             importIndex < startIndex + pageCount;
             idx++, insertIndex++, importIndex++)
        {
            var importPage = document.Pages[importIndex];
            var page = ImportExternalPage (importPage, annotationCopying);
            insertPages[idx] = page;
            importPages[idx] = importPage;

            Owner!._irefTable.Add (page);

            // Add page substitute to importedObjectTable.
            var importedObjectTable = Owner.FormTable.GetImportedObjectTable (importPage);
            importedObjectTable.Add (importPage.ObjectID, page.Reference!);

            PagesArray.Elements.Insert (insertIndex, page.Reference!);

            if (Owner.Settings.TrimMargins.AreSet)
            {
                page.TrimMargins = Owner.Settings.TrimMargins;
            }
        }

        Elements.SetInteger (Keys.Count, PagesArray.Elements.Count);

        // 2nd copy link annotations that are in the range of the imported pages.
        for (int idx = 0, importIndex = startIndex;
             importIndex < startIndex + pageCount;
             idx++, importIndex++)
        {
            var importPage = document.Pages[importIndex];
            var page = insertPages[idx];

            // Get annotations.
            var annots = importPage.Elements.GetArray (PdfPage.Keys.Annots);
            if (annots != null)
            {
                var annotations = new PdfAnnotations (Owner!);

                // Loop through annotations.
                var count = annots.Elements.Count;
                for (var idxAnnotation = 0; idxAnnotation < count; idxAnnotation++)
                {
                    var annot = annots.Elements.GetDictionary (idxAnnotation);
                    if (annot != null!)
                    {
                        var subtype = annot.Elements.GetString (PdfAnnotation.Keys.Subtype);
                        if (subtype == "/Link")
                        {
                            var addAnnotation = false;
                            var newAnnotation = new PdfLinkAnnotation (Owner!);

                            var importAnnotationKeyNames = annot.Elements.KeyNames;
                            foreach (var pdfItem in importAnnotationKeyNames)
                            {
                                PdfItem impItem;
                                switch (pdfItem.Value)
                                {
                                    case "/BS":
                                        newAnnotation.Elements.Add ("/BS", new PdfLiteral ("<</W 0>>"));
                                        break;

                                    case "/F": // /F 4
                                        impItem = annot.Elements.GetValue ("/F")!;
                                        Debug.Assert (impItem is PdfInteger);
                                        newAnnotation.Elements.Add ("/F", impItem.Clone());
                                        break;

                                    case "/Rect": // /Rect [68.6 681.08 145.71 702.53]
                                        impItem = annot.Elements.GetValue ("/Rect")!;
                                        Debug.Assert (impItem is PdfArray);
                                        newAnnotation.Elements.Add ("/Rect", impItem.Clone());
                                        break;

                                    case "/StructParent": // /StructParent 3
                                        impItem = annot.Elements.GetValue ("/StructParent")!;
                                        Debug.Assert (impItem is PdfInteger);
                                        newAnnotation.Elements.Add ("/StructParent", impItem.Clone());
                                        break;

                                    case "/Subtype": // Already set.
                                        break;

                                    case "/Dest": // /Dest [30 0 R /XYZ 68 771 0]
                                        impItem = annot.Elements.GetValue ("/Dest")!;
                                        impItem = impItem.Clone();

                                        // Is value an array with 5 elements where the first one is an iref?
                                        if (impItem is PdfArray { Elements: [PdfReference iref, _, _, _, _] } destArray)
                                        {
                                            iref = RemapReference (insertPages, importPages, iref)!;
                                            if (iref != null!)
                                            {
                                                destArray.Elements[0] = iref;
                                                newAnnotation.Elements.Add ("/Dest", destArray);
                                                addAnnotation = true;
                                            }
                                        }

                                        break;

                                    default:
#if DEBUG_
                                            Debug-Break.Break(true);
#endif
                                        break;
                                }
                            }

                            // Add newAnnotations only it points to an imported page.
                            if (addAnnotation)
                            {
                                annotations.Add (newAnnotation);
                            }
                        }
                    }
                }

                // At least one link annotation found?
                if (annotations.Count > 0)
                {
                    //Owner._irefTable.Add(annotations);
                    page.Elements.Add (PdfPage.Keys.Annots, annotations);
                }
            }
        }
    }

    /// <summary>
    /// Inserts all pages of the specified document into this document.
    /// </summary>
    /// <param name="index">The index in this document where to insert the page .</param>
    /// <param name="document">The document to be inserted.</param>
    /// <param name="annotationCopying">Annotation copying action, by default annotations are not copied.</param>
    public void InsertRange (int index, PdfDocument document,
        AnnotationCopyingType annotationCopying = AnnotationCopyingType.DoNotCopy)
    {
        if (document == null)
        {
            throw new ArgumentNullException (nameof (document));
        }

        InsertRange (index, document, 0, document.PageCount, annotationCopying);
    }

    /// <summary>
    /// Inserts all pages of the specified document into this document.
    /// </summary>
    /// <param name="index">The index in this document where to insert the page .</param>
    /// <param name="document">The document to be inserted.</param>
    /// <param name="startIndex">The index of the first page to be inserted.</param>
    /// <param name="annotationCopying">Annotation copying action, by default annotations are not copied.</param>
    public void InsertRange (int index, PdfDocument document, int startIndex,
        AnnotationCopyingType annotationCopying = AnnotationCopyingType.DoNotCopy)
    {
        if (document == null)
        {
            throw new ArgumentNullException (nameof (document));
        }

        InsertRange (index, document, startIndex, document.PageCount - startIndex, annotationCopying);
    }

    /// <summary>
    /// Removes the specified page from the document.
    /// </summary>
    public void Remove (PdfPage page)
    {
        PagesArray.Elements.Remove (page.Reference!);
        Elements.SetInteger (Keys.Count, PagesArray.Elements.Count);
    }

    /// <summary>
    /// Removes the specified page from the document.
    /// </summary>
    public void RemoveAt (int index)
    {
        PagesArray.Elements.RemoveAt (index);
        Elements.SetInteger (Keys.Count, PagesArray.Elements.Count);
    }

    /// <summary>
    /// Moves a page within the page sequence.
    /// </summary>
    /// <param name="oldIndex">The page index before this operation.</param>
    /// <param name="newIndex">The page index after this operation.</param>
    public void MovePage (int oldIndex, int newIndex)
    {
        Sure.InRange (oldIndex, 0, Count);
        Sure.InRange (newIndex, 0, Count);

        if (oldIndex == newIndex)
        {
            return;
        }

        //PdfPage page = (PdfPage)pagesArray.Elements[oldIndex];
        var page = (PdfReference) _pagesArray!.Elements[oldIndex];
        _pagesArray.Elements.RemoveAt (oldIndex);
        _pagesArray.Elements.Insert (newIndex, page);
    }

    /// <summary>
    /// Imports an external page. The elements of the imported page are cloned and added to this document.
    /// Important: In contrast to PdfFormXObject adding an external page always make a deep copy
    /// of their transitive closure. Any reuse of already imported objects is not intended because
    /// any modification of an imported page must not change another page.
    /// </summary>
    private PdfPage ImportExternalPage
        (
            PdfPage importPage,
            AnnotationCopyingType annotationCopying = AnnotationCopyingType.DoNotCopy
        )
    {
        if (importPage.Owner!._openMode != PdfDocumentOpenMode.Import)
        {
            throw new InvalidOperationException (
                "A PDF document must be opened with PdfDocumentOpenMode.Import to import pages from it.");
        }

        var page = new PdfPage (_document!);

        // ReSharper disable AccessToStaticMemberViaDerivedType for a better code readability.
        CloneElement (page, importPage, PdfPage.Keys.Resources, false);
        CloneElement (page, importPage, PdfPage.Keys.Contents, false);
        CloneElement (page, importPage, PdfPage.Keys.MediaBox, true);
        CloneElement (page, importPage, PdfPage.Keys.CropBox, true);
        CloneElement (page, importPage, PdfPage.Keys.Rotate, true);
        CloneElement (page, importPage, PdfPage.Keys.BleedBox, true);
        CloneElement (page, importPage, PdfPage.Keys.TrimBox, true);
        CloneElement (page, importPage, PdfPage.Keys.ArtBox, true);

        if (annotationCopying == AnnotationCopyingType.ShallowCopy)
        {
            CloneElement (page, importPage, PdfPage.Keys.Annots, false);
        }
        else if (annotationCopying == AnnotationCopyingType.DeepCopy)
        {
            CloneElement (page, importPage, PdfPage.Keys.Annots, true);
        }

        // ReSharper restore AccessToStaticMemberViaDerivedType
        // TODO more elements?
        return page;
    }

    /// <summary>
    /// Helper function for ImportExternalPage.
    /// </summary>
    private void CloneElement
        (
            PdfPage page,
            PdfPage importPage,
            string key,
            bool deepcopy
        )
    {
        Debug.Assert (page != null);
        Debug.Assert (page.Owner == _document);
        Debug.Assert (importPage.Owner != null);
        Debug.Assert (importPage.Owner != _document);

        var item = importPage.Elements[key];
        if (item != null)
        {
            PdfImportedObjectTable? importedObjectTable = null;
            if (!deepcopy)
            {
                importedObjectTable = Owner!.FormTable.GetImportedObjectTable (importPage);
            }

            // The item can be indirect. If so, replace it by its value.
            if (item is PdfReference reference)
            {
                item = reference.Value;
            }

            if (item is PdfObject root)
            {
                if (deepcopy)
                {
                    Debug.Assert (root.Owner != null, "See 'else' case for details");
                    root = DeepCopyClosure (_document!, root);
                }
                else
                {
                    // The owner can be null if the item is not a reference.
                    if (root.Owner == null)
                    {
                        root.Document = importPage.Owner;
                    }

                    root = ImportClosure (importedObjectTable!, page.Owner!, root);
                }

                if (root.Reference == null)
                {
                    page.Elements[key] = root;
                }
                else
                {
                    page.Elements[key] = root.Reference;
                }
            }
            else
            {
                // Simple items are just cloned.
                page.Elements[key] = item.Clone();
            }
        }
    }

    static PdfReference? RemapReference
        (
            PdfPage[] newPages,
            PdfPage[] impPages,
            PdfReference iref
        )
    {
        // Directs the iref to a one of the imported pages?
        for (var idx = 0; idx < newPages.Length; idx++)
        {
            if (impPages[idx].Reference == iref)
            {
                return newPages[idx].Reference;
            }
        }

        return null;
    }

    /// <summary>
    /// Gets a PdfArray containing all pages of this document. The array must not be modified.
    /// </summary>
    public PdfArray PagesArray =>
        _pagesArray ??= (PdfArray) Elements.GetValue (Keys.Kids, VCF.Create)
            .ThrowIfNull();

    private PdfArray? _pagesArray;

    /// <summary>
    /// Replaces the page tree by a flat array of indirect references to the pages objects.
    /// </summary>
    internal void FlattenPageTree()
    {
        // Acrobat creates a balanced tree if the number of pages is rougly more than ten. This is
        // not difficult but obviously also not necessary. I created a document with 50000 pages with
        // PDF4NET and Acrobat opened it in less than 2 seconds.

        //PdfReference xrefRoot = Document.Catalog.Elements[PdfCatalog.Keys.Pages] as PdfReference;
        //PdfDictionary[] pages = GetKids(xrefRoot, null);

        // Promote inheritable values down the page tree
        var values = new PdfPage.InheritedValues();
        PdfPage.InheritValues (this, ref values);
        var pages = GetKids (Reference!, values, null);

        // Replace /Pages in catalog by this object
        // xrefRoot.Value = this;

        var array = new PdfArray (Owner!);
        foreach (var page in pages)
        {
            // Fix the parent
            page.Elements[PdfPage.Keys.Parent] = Reference;
            array.Elements.Add (page.Reference!);
        }

        Elements.SetName (Keys.Type, "/Pages");
#if true

        // direct array
        Elements.SetValue (Keys.Kids, array);
#else
            // incdirect array
            Document.xrefTable.Add(array);
            Elements.SetValue(Keys.Kids, array.XRef);
#endif
        Elements.SetInteger (Keys.Count, array.Elements.Count);
    }

    /// <summary>
    /// Recursively converts the page tree into a flat array.
    /// </summary>
    PdfDictionary[] GetKids
        (
            PdfReference iref,
            PdfPage.InheritedValues values,
            PdfDictionary? parent
        )
    {
        // TODO: inherit inheritable keys...
        var kid = (PdfDictionary)iref.Value;

#if true
        var type = kid.Elements.GetName (Keys.Type);
        if (type == "/Page")
        {
            PdfPage.InheritValues (kid, values);
            return new PdfDictionary[] { kid };
        }

        if (string.IsNullOrEmpty (type))
        {
            // Type is required. If type is missing, assume it is "/Page" and hope it will work.
            // TODO Implement a "Strict" mode in PDFsharp and don't do this in "Strict" mode.
            PdfPage.InheritValues (kid, values);
            return new PdfDictionary[] { kid };
        }

#else
            if (kid.Elements.GetName(Keys.Type) == "/Page")
            {
                PdfPage.InheritValues(kid, values);
                return new PdfDictionary[] { kid };
            }
#endif

        Debug.Assert (kid.Elements.GetName (Keys.Type) == "/Pages");
        PdfPage.InheritValues (kid, ref values);
        var list = new List<PdfDictionary>();

        if (kid.Elements["/Kids"] is not PdfArray kids)
        {
            var xref3 = kid.Elements["/Kids"] as PdfReference;
            kids = (xref3!.Value as PdfArray)!;
        }

        foreach (PdfReference xref2 in kids)
        {
            list.AddRange (GetKids (xref2, values, kid));
        }

        var count = list.Count;
        Debug.Assert (count == kid.Elements.GetInteger ("/Count"));
        return list.ToArray();
    }

    /// <summary>
    /// Prepares the document for saving.
    /// </summary>
    internal override void PrepareForSave()
    {
        // TODO: Close all open content streams

        // TODO: Create the page tree.
        // Arrays have a limit of 8192 entries, but I successfully tested documents
        // with 50000 pages and no page tree.
        // ==> wait for bug report.
        var count = _pagesArray!.Elements.Count;
        for (var idx = 0; idx < count; idx++)
        {
            var page = this[idx];
            page.PrepareForSave();
        }
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    public new IEnumerator<PdfPage> GetEnumerator()
    {
        return new PdfPagesEnumerator (this);
    }

    class PdfPagesEnumerator : IEnumerator<PdfPage>
    {
        internal PdfPagesEnumerator (PdfPages list)
        {
            _list = list;
            _index = -1;
        }

        public bool MoveNext()
        {
            if (_index < _list.Count - 1)
            {
                _index++;
                _currentElement = _list[_index];
                return true;
            }

            _index = _list.Count;
            return false;
        }

        public void Reset()
        {
            _currentElement = null;
            _index = -1;
        }

        object IEnumerator.Current => Current;

        public PdfPage Current
        {
            get
            {
                if (_index == -1 || _index >= _list.Count)
                {
                    throw new InvalidOperationException (PSSR.ListEnumCurrentOutOfRange);
                }

                return _currentElement!;
            }
        }

        public void Dispose()
        {
            // Nothing to do.
        }

        private PdfPage? _currentElement;
        private int _index;
        private readonly PdfPages _list;
    }

    /// <summary>
    /// Predefined keys of this dictionary.
    /// </summary>
    internal sealed class Keys : PdfPage.InheritablePageKeys
    {
        /// <summary>
        /// (Required) The type of PDF object that this dictionary describes;
        /// must be Pages for a page tree node.
        /// </summary>
        [KeyInfo (KeyType.Name | KeyType.Required, FixedValue = "Pages")]
        public const string Type = "/Type";

        /// <summary>
        /// (Required except in root node; must be an indirect reference)
        /// The page tree node that is the immediate parent of this one.
        /// </summary>
        [KeyInfo (KeyType.Dictionary | KeyType.Required)]
        public const string Parent = "/Parent";

        /// <summary>
        /// (Required) An array of indirect references to the immediate children of this node.
        /// The children may be page objects or other page tree nodes.
        /// </summary>
        [KeyInfo (KeyType.Array | KeyType.Required)]
        public const string Kids = "/Kids";

        /// <summary>
        /// (Required) The number of leaf nodes (page objects) that are descendants of this node
        /// within the page tree.
        /// </summary>
        [KeyInfo (KeyType.Integer | KeyType.Required)]
        public const string Count = "/Count";

        /// <summary>
        /// Gets the KeysMeta for these keys.
        /// </summary>
        public static DictionaryMeta Meta => _meta ??= CreateMeta (typeof (Keys));

        private static DictionaryMeta? _meta;
    }

    /// <summary>
    /// Gets the KeysMeta of this dictionary type.
    /// </summary>
    internal override DictionaryMeta Meta => Keys.Meta;
}
