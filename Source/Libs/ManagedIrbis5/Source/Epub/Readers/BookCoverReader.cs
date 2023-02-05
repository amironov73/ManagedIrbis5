// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BookCoverReader.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;

using ManagedIrbis.Epub.Schema;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Internal;

internal static class BookCoverReader
{
    public static EpubByteContentFileRef? ReadBookCover
        (
            EpubSchema epubSchema,
            Dictionary<string, EpubByteContentFileRef> imageContentRefs
        )
    {
        EpubByteContentFileRef? result;
        if (epubSchema.Package!.EpubVersion == EpubVersion.EPUB_3 ||
            epubSchema.Package.EpubVersion == EpubVersion.EPUB_3_1)
        {
            result = ReadEpub3Cover (epubSchema, imageContentRefs)
                     ?? ReadEpub2Cover (epubSchema, imageContentRefs);
        }
        else
        {
            result = ReadEpub2Cover (epubSchema, imageContentRefs);
        }

        return result;
    }

    private static EpubByteContentFileRef? ReadEpub2Cover
        (
            EpubSchema epubSchema,
            IReadOnlyDictionary<string, EpubByteContentFileRef> imageContentRefs
        )
    {
        var result = ReadEpub2CoverFromMetadata (epubSchema, imageContentRefs)
                     ?? ReadEpub2CoverFromGuide (epubSchema, imageContentRefs);

        return result;
    }

    private static EpubByteContentFileRef? ReadEpub2CoverFromMetadata
        (
            EpubSchema epubSchema,
            IReadOnlyDictionary<string, EpubByteContentFileRef> imageContentRefs
        )
    {
        var metaItems = epubSchema.Package!.Metadata!.MetaItems;
        if (metaItems == null! || !metaItems.Any())
        {
            return null;
        }

        var coverMetaItem = metaItems.FirstOrDefault (metaItem => metaItem.Name.SameString ("cover"));
        if (coverMetaItem == null)
        {
            return null;
        }

        if (string.IsNullOrEmpty (coverMetaItem.Content))
        {
            throw new EpubPackageException ("Incorrect EPUB metadata: cover item content is missing.");
        }

        var coverManifestItem = epubSchema.Package.Manifest!.FirstOrDefault (manifestItem =>
            manifestItem.Id.SameString (coverMetaItem.Content));
        if (coverManifestItem == null)
        {
            throw new EpubPackageException (
                $"Incorrect EPUB manifest: item with ID = \"{coverMetaItem.Content}\" is missing.");
        }

        if (coverManifestItem.Href == null!)
        {
            return null;
        }

        if (!imageContentRefs.TryGetValue (coverManifestItem.Href, out var coverImageContentFileRef))
        {
            throw new EpubPackageException ($"Incorrect EPUB manifest: item with href = \"{coverManifestItem.Href}\" is missing.");
        }

        return coverImageContentFileRef;
    }

    private static EpubByteContentFileRef? ReadEpub2CoverFromGuide
        (
            EpubSchema epubSchema,
            IReadOnlyDictionary<string, EpubByteContentFileRef> imageContentRefs
        )
    {
        foreach (var guideReference in epubSchema.Package!.Guide!)
        {
            if (guideReference.Type!.ToLowerInvariant() == "cover" && imageContentRefs.TryGetValue (guideReference.Href!,
                    out var coverImageContentFileRef))
            {
                return coverImageContentFileRef;
            }
        }

        return null;
    }

    private static EpubByteContentFileRef? ReadEpub3Cover
        (
            EpubSchema epubSchema,
            IReadOnlyDictionary<string, EpubByteContentFileRef> imageContentRefs
        )
    {
        var coverManifestItem = epubSchema.Package!.Manifest!.FirstOrDefault (manifestItem =>
                manifestItem.Properties != null!
                && manifestItem.Properties.Contains (ManifestProperty.COVER_IMAGE));
        if (coverManifestItem == null || coverManifestItem.Href == null!)
        {
            return null;
        }

        if (!imageContentRefs.TryGetValue (coverManifestItem.Href, out var coverImageContentFileRef))
        {
            throw new EpubPackageException (
                $"Incorrect EPUB manifest: item with href = \"{coverManifestItem.Href}\" is missing.");
        }

        return coverImageContentFileRef;
    }
}
