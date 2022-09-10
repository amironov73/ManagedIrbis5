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

using System.Collections.Generic;

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Base class for all dictionaries that map resource names to objects.
/// </summary>
internal class PdfResourceMap
    : PdfDictionary //, IEnumerable
{
    public PdfResourceMap()
    {
        // пустое тело конструктора
    }

    public PdfResourceMap (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    protected PdfResourceMap (PdfDictionary dict)
        : base (dict)
    {
        // пустое тело конструктора
    }

    //    public int Count
    //    {
    //      get {return resources.Count;}
    //    }
    //
    //    public PdfObject this[string key]
    //    {
    //      get {return resources[key] as PdfObject;}
    //      set {resources[key] = value;}
    //    }

    /// <summary>
    /// Adds all imported resource names to the specified hashtable.
    /// </summary>
    internal void CollectResourceNames
        (
            Dictionary<string, object> usedResourceNames
        )
    {
        // ?TODO: Imported resources (e.g. fonts) can be reused, but I think this is rather difficult. Will be an issue in PDFsharp 2.0.
        var names = Elements.KeyNames;
        foreach (var name in names)
        {
            usedResourceNames.Add (name.ToString(), null);
        }
    }
}
