// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using ManagedIrbis.Epub.Schema;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Internal;

internal static class SpineReader
{
    public static List<EpubTextContentFileRef> GetReadingOrder(EpubBookRef bookRef)
    {
        List<EpubTextContentFileRef> result = new List<EpubTextContentFileRef>();
        foreach (EpubSpineItemRef spineItemRef in bookRef.Schema.Package.Spine)
        {
            EpubManifestItem manifestItem = bookRef.Schema.Package.Manifest.FirstOrDefault(item => item.Id == spineItemRef.IdRef);
            if (manifestItem == null)
            {
                throw new EpubPackageException($"Incorrect EPUB spine: item with IdRef = \"{spineItemRef.IdRef}\" is missing in the manifest.");
            }
            if (!bookRef.Content.Html.TryGetValue(manifestItem.Href, out EpubTextContentFileRef htmlContentFileRef))
            {
                throw new EpubPackageException($"Incorrect EPUB manifest: item with href = \"{spineItemRef.IdRef}\" is missing in the book.");
            }
            result.Add(htmlContentFileRef);
        }
        return result;
    }
}