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

using System.Threading.Tasks;

using ManagedIrbis.Epub.Environment;
using ManagedIrbis.Epub.Options;
using ManagedIrbis.Epub.Schema;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Internal;

internal static class SchemaReader
{
    public static async Task<EpubSchema> ReadSchemaAsync(IZipFile epubFile, EpubReaderOptions epubReaderOptions)
    {
        EpubSchema result = new EpubSchema();
        string rootFilePath = await RootFilePathReader.GetRootFilePathAsync(epubFile, epubReaderOptions).ConfigureAwait(false);
        string contentDirectoryPath = ZipPathUtils.GetDirectoryPath(rootFilePath);
        result.ContentDirectoryPath = contentDirectoryPath;
        EpubPackage package = await PackageReader.ReadPackageAsync(epubFile, rootFilePath, epubReaderOptions).ConfigureAwait(false);
        result.Package = package;
        result.Epub2Ncx = await Epub2NcxReader.ReadEpub2NcxAsync(epubFile, contentDirectoryPath, package, epubReaderOptions).ConfigureAwait(false);
        result.Epub3NavDocument = await Epub3NavDocumentReader.ReadEpub3NavDocumentAsync(epubFile, contentDirectoryPath, package, epubReaderOptions).ConfigureAwait(false);
        return result;
    }
}