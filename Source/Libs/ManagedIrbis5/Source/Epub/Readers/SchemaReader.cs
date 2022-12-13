// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SchemaReader.cs --
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
    public static async Task<EpubSchema> ReadSchemaAsync
        (IZipFile epubFile, EpubReaderOptions epubReaderOptions)
    {
        var result = new EpubSchema();
        var rootFilePath = await RootFilePathReader.GetRootFilePathAsync (epubFile, epubReaderOptions)
            .ConfigureAwait (false);
        var contentDirectoryPath = ZipPathUtils.GetDirectoryPath (rootFilePath);
        result.ContentDirectoryPath = contentDirectoryPath;
        var package = await PackageReader.ReadPackageAsync (epubFile, rootFilePath, epubReaderOptions)
            .ConfigureAwait (false);
        result.Package = package;
        result.Epub2Ncx = await Epub2NcxReader
            .ReadEpub2NcxAsync (epubFile, contentDirectoryPath, package, epubReaderOptions)
            .ConfigureAwait (false);
        result.Epub3NavDocument = await Epub3NavDocumentReader
            .ReadEpub3NavDocumentAsync (epubFile, contentDirectoryPath, package, epubReaderOptions)
            .ConfigureAwait (false);

        return result;
    }
}
