// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* PdfFormXObjectTable.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Collections.Generic;

using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Contains all external PDF files from which PdfFormXObjects are imported into the current document.
/// </summary>
internal sealed class PdfFormXObjectTable
    : PdfResourceTable
{
    // The name PdfFormXObjectTable is technically not correct, because in contrast to PdfFontTable
    // or PdfImageTable this class holds no PdfFormXObject objects. Actually it holds instances of
    // the class ImportedObjectTable, one for each external document. The PdfFormXObject instances
    // are not cached, because they hold a transformation matrix that make them unique. If the user
    // wants to use a particual page of a PdfFormXObject more than once, he must reuse the object
    // before he changes the PageNumber or the transformation matrix. In other words this class
    // caches the indirect objects of an external form, not the form itself.

    #region Construction

    /// <summary>
    /// Initializes a new instance of this class, which is a singleton for each document.
    /// </summary>
    public PdfFormXObjectTable (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Gets a PdfFormXObject from an XPdfForm. Because the returned objects must be unique, always
    /// a new instance of PdfFormXObject is created if none exists for the specified form.
    /// </summary>
    public PdfFormXObject GetForm (XForm form)
    {
        // If the form already has a PdfFormXObject, return it.
        if (form._pdfForm != null)
        {
            Debug.Assert (form.IsTemplate, "An XPdfForm must not have a PdfFormXObject.");
            if (ReferenceEquals (form._pdfForm.Owner, Owner))
            {
                return form._pdfForm;
            }

            //throw new InvalidOperationException("Because of a current limitation of PDFsharp an XPdfForm object can be used only within one single PdfDocument.");

            // Dispose PdfFromXObject when document has changed
            form._pdfForm = null;
        }

        if (form is XPdfForm pdfForm)
        {
            // Is the external PDF file from which is imported already known for the current document?
            var selector = new Selector (form);
            if (!_forms.TryGetValue (selector, out var importedObjectTable))
            {
                // No: Get the external document from the form and create ImportedObjectTable.
                var doc = pdfForm.ExternalDocument;
                importedObjectTable = new PdfImportedObjectTable (Owner, doc);
                _forms[selector] = importedObjectTable;
            }

            var xObject = importedObjectTable.GetXObject (pdfForm.PageNumber);
            if (xObject == null)
            {
                xObject = new PdfFormXObject (Owner, importedObjectTable, pdfForm);
                importedObjectTable.SetXObject (pdfForm.PageNumber, xObject);
            }

            return xObject;
        }

        Debug.Assert (form.GetType() == typeof (XForm));
        form._pdfForm = new PdfFormXObject (Owner, form);
        return form._pdfForm;
    }

    /// <summary>
    /// Gets the imported object table.
    /// </summary>
    public PdfImportedObjectTable GetImportedObjectTable (PdfPage page)
    {
        // Is the external PDF file from which is imported already known for the current document?
        var selector = new Selector (page);
        if (!_forms.TryGetValue (selector, out var importedObjectTable))
        {
            importedObjectTable = new PdfImportedObjectTable (Owner, page.Owner);
            _forms[selector] = importedObjectTable;
        }

        return importedObjectTable;
    }

    /// <summary>
    /// Gets the imported object table.
    /// </summary>
    public PdfImportedObjectTable GetImportedObjectTable (PdfDocument document)
    {
        if (document == null)
        {
            throw new ArgumentNullException ("document");
        }

        // Is the external PDF file from which is imported already known for the current document?
        var selector = new Selector (document);
        if (!_forms.TryGetValue (selector, out var importedObjectTable))
        {
            // Create new table for document.
            importedObjectTable = new PdfImportedObjectTable (Owner, document);
            _forms[selector] = importedObjectTable;
        }

        return importedObjectTable;
    }

    public void DetachDocument (PdfDocument.DocumentHandle handle)
    {
        if (handle.IsAlive)
        {
            foreach (var selector in _forms.Keys)
            {
                var table = _forms[selector];
                if (table.ExternalDocument != null && table.ExternalDocument.Handle == handle)
                {
                    _forms.Remove (selector);
                    break;
                }
            }
        }

        // Clean table
        var itemRemoved = true;
        while (itemRemoved)
        {
            itemRemoved = false;
            foreach (var selector in _forms.Keys)
            {
                var table = _forms[selector];
                if (table.ExternalDocument == null)
                {
                    _forms.Remove (selector);
                    itemRemoved = true;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Map from Selector to PdfImportedObjectTable.
    /// </summary>
    readonly Dictionary<Selector, PdfImportedObjectTable> _forms = new ();

    /// <summary>
    /// A collection of information that uniquely identifies a particular ImportedObjectTable.
    /// </summary>
    public class Selector
    {
        /// <summary>
        /// Initializes a new instance of FormSelector from an XPdfForm.
        /// </summary>
        public Selector (XForm form)
        {
            // HACK: just use full path to identify
            _path = form._path.ToLowerInvariant();
        }

        /// <summary>
        /// Initializes a new instance of FormSelector from a PdfPage.
        /// </summary>
        public Selector (PdfPage page)
        {
            var owner = page.Owner;
            _path = "*" + owner.Guid.ToString ("B");
            _path = _path.ToLowerInvariant();
        }

        public Selector (PdfDocument document)
        {
            _path = "*" + document.Guid.ToString ("B");
            _path = _path.ToLowerInvariant();
        }

        /// <summary>
        ///
        /// </summary>
        public string? Path
        {
            get => _path;
            set => _path = value;
        }

        private string? _path;

        /// <inheritdoc cref="object.Equals(object?)"/>
        public override bool Equals (object? obj)
        {
            return obj is Selector selector && _path == selector._path;
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return _path?.GetHashCode() ?? 0;
        }
    }
}
