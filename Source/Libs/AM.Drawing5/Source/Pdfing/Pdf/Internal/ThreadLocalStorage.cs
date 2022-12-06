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
using System.IO;

using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.IO.enums;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Internal;

/// <summary>
/// Provides a thread-local cache for large objects.
/// </summary>
internal class ThreadLocalStorage // #???
{
    public ThreadLocalStorage()
    {
        _importedDocuments = new Dictionary<string, PdfDocument.DocumentHandle> (StringComparer.OrdinalIgnoreCase);
    }

    public void AddDocument (string path, PdfDocument document)
    {
        _importedDocuments.Add (path, document.Handle);
    }

    public void RemoveDocument (string path)
    {
        _importedDocuments.Remove (path);
    }

    public PdfDocument GetDocument (string path, PdfReadAccuracy accuracy)
    {
        Debug.Assert (path.StartsWith ("*") || Path.IsPathRooted (path), "Path must be full qualified.");

        PdfDocument? document = null;
        if (_importedDocuments.TryGetValue (path, out var handle))
        {
            document = handle.Target;
            if (document == null)
            {
                RemoveDocument (path);
            }
        }

        if (document == null)
        {
            document = PdfReader.Open (path, PdfDocumentOpenMode.Import, accuracy);
            _importedDocuments.Add (path, document!.Handle);
        }

        return document;
    }

    public PdfDocument[] Documents
    {
        get
        {
            List<PdfDocument> list = new List<PdfDocument>();
            foreach (PdfDocument.DocumentHandle handle in _importedDocuments.Values)
            {
                if (handle.IsAlive)
                {
                    list.Add (handle.Target!);
                }
            }

            return list.ToArray();
        }
    }

    public void DetachDocument (PdfDocument.DocumentHandle handle)
    {
        if (handle.IsAlive)
        {
            foreach (string path in _importedDocuments.Keys)
            {
                if (_importedDocuments[path] == handle)
                {
                    _importedDocuments.Remove (path);
                    break;
                }
            }
        }

        // Clean table
        bool itemRemoved = true;
        while (itemRemoved)
        {
            itemRemoved = false;
            foreach (string path in _importedDocuments.Keys)
            {
                if (!_importedDocuments[path].IsAlive)
                {
                    _importedDocuments.Remove (path);
                    itemRemoved = true;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Maps path to document handle.
    /// </summary>
    readonly Dictionary<string, PdfDocument.DocumentHandle> _importedDocuments;
}
