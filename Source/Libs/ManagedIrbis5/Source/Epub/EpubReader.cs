// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* EpubReader.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using ManagedIrbis.Epub.Environment;
using ManagedIrbis.Epub.Internal;
using ManagedIrbis.Epub.Options;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public static class EpubReader
{
    private static IFileSystem FileSystem => EnvironmentDependencies.FileSystem;

    /// <summary>
    /// Opens the book synchronously without reading its whole content. Holds the handle to the EPUB file.
    /// </summary>
    /// <param name="filePath">Path to the EPUB file.</param>
    /// <param name="epubReaderOptions">Various options to configure the behavior of the EPUB reader.</param>
    /// <returns></returns>
    public static EpubBookRef OpenBook
        (
            string filePath,
            EpubReaderOptions? epubReaderOptions = null
        )
    {
        return OpenBookAsync (filePath, epubReaderOptions).Result;
    }

    /// <summary>
    /// Opens the book synchronously without reading its whole content.
    /// </summary>
    /// <param name="stream">Seekable stream containing the EPUB file.</param>
    /// <param name="epubReaderOptions">Various options to configure the behavior of the EPUB reader.</param>
    /// <returns></returns>
    public static EpubBookRef OpenBook
        (
            Stream stream,
            EpubReaderOptions? epubReaderOptions = null
        )
    {
        return OpenBookAsync (stream, epubReaderOptions).Result;
    }

    /// <summary>
    /// Opens the book asynchronously without reading its whole content. Holds the handle to the EPUB file.
    /// </summary>
    /// <param name="filePath">Path to the EPUB file.</param>
    /// <param name="epubReaderOptions">Various options to configure the behavior of the EPUB reader.</param>
    /// <returns></returns>
    public static Task<EpubBookRef> OpenBookAsync
        (
            string filePath,
            EpubReaderOptions? epubReaderOptions = null
        )
    {
        var fileSystem = EnvironmentDependencies.FileSystem;
        if (!fileSystem.FileExists (filePath))
        {
            throw new FileNotFoundException ("Specified EPUB file not found.", filePath);
        }

        return OpenBookAsync (GetZipFile (filePath), filePath, epubReaderOptions);
    }

    /// <summary>
    /// Opens the book asynchronously without reading its whole content.
    /// </summary>
    /// <param name="stream">Seekable stream containing the EPUB file.</param>
    /// <param name="epubReaderOptions">Various options to configure the behavior of the EPUB reader.</param>
    /// <returns></returns>
    public static Task<EpubBookRef> OpenBookAsync
        (
            Stream stream,
            EpubReaderOptions? epubReaderOptions = null
        )
    {
        return OpenBookAsync (GetZipFile (stream), null, epubReaderOptions);
    }

    /// <summary>
    /// Opens the book synchronously and reads all of its content into the memory. Does not hold the handle to the EPUB file.
    /// </summary>
    /// <param name="filePath">Path to the EPUB file.</param>
    /// <param name="epubReaderOptions">Various options to configure the behavior of the EPUB reader.</param>
    /// <returns></returns>
    public static EpubBook ReadBook
        (
            string filePath,
            EpubReaderOptions? epubReaderOptions = null
        )
    {
        return ReadBookAsync (filePath, epubReaderOptions).Result;
    }

    /// <summary>
    /// Opens the book synchronously and reads all of its content into the memory.
    /// </summary>
    /// <param name="stream">Seekable stream containing the EPUB file.</param>
    /// <param name="epubReaderOptions">Various options to configure the behavior of the EPUB reader.</param>
    /// <returns></returns>
    public static EpubBook ReadBook
        (
            Stream stream,
            EpubReaderOptions? epubReaderOptions = null
        )
    {
        return ReadBookAsync (stream, epubReaderOptions).Result;
    }

    /// <summary>
    /// Opens the book asynchronously and reads all of its content into the memory. Does not hold the handle to the EPUB file.
    /// </summary>
    /// <param name="filePath">Path to the EPUB file.</param>
    /// <param name="epubReaderOptions">Various options to configure the behavior of the EPUB reader.</param>
    /// <returns></returns>
    public static async Task<EpubBook> ReadBookAsync
        (
            string filePath,
            EpubReaderOptions? epubReaderOptions = null
        )
    {
        var epubBookRef = await OpenBookAsync (filePath, epubReaderOptions).ConfigureAwait (false);
        return await ReadBookAsync (epubBookRef).ConfigureAwait (false);
    }

    /// <summary>
    /// Opens the book asynchronously and reads all of its content into the memory.
    /// </summary>
    /// <param name="stream">Seekable stream containing the EPUB file.</param>
    /// <param name="epubReaderOptions">Various options to configure the behavior of the EPUB reader.</param>
    /// <returns></returns>
    public static async Task<EpubBook> ReadBookAsync
        (
            Stream stream,
            EpubReaderOptions? epubReaderOptions = null
        )
    {
        var epubBookRef = await OpenBookAsync (stream, epubReaderOptions).ConfigureAwait (false);

        return await ReadBookAsync (epubBookRef).ConfigureAwait (false);
    }

    private static async Task<EpubBookRef> OpenBookAsync
        (
            IZipFile zipFile,
            string filePath,
            EpubReaderOptions? epubReaderOptions
        )
    {
        EpubBookRef? result = null;
        try
        {
            result = new EpubBookRef (zipFile);
            result.FilePath = filePath;
            result.Schema = await SchemaReader.ReadSchemaAsync (zipFile, epubReaderOptions ?? new EpubReaderOptions())
                .ConfigureAwait (false);
            result.Title = result.Schema.Package?.Metadata.Titles.FirstOrDefault() ?? string.Empty;
            result.AuthorList = result.Schema.Package?.Metadata.Creators.Select (creator => creator.Creator).ToList();
            result.Author = String.Join (", ", result.AuthorList);
            result.Description = result.Schema.Package.Metadata.Description;
            result.Content = await Task.Run (() => ContentReader.ParseContentMap (result)).ConfigureAwait (false);
            return result;
        }
        catch
        {
            result?.Dispose();
            throw;
        }
    }

    private static async Task<EpubBook> ReadBookAsync
        (
            EpubBookRef epubBookRef
        )
    {
        var result = new EpubBook();
        using (epubBookRef)
        {
            result.FilePath = epubBookRef.FilePath;
            result.Schema = epubBookRef.Schema;
            result.Title = epubBookRef.Title;
            result.AuthorList = epubBookRef.AuthorList;
            result.Author = epubBookRef.Author;
            result.Content = await ReadContent (epubBookRef.Content).ConfigureAwait (false);
            result.CoverImage = await epubBookRef.ReadCoverAsync().ConfigureAwait (false);
            result.Description = epubBookRef.Description;
            var htmlContentFileRefs =
                await epubBookRef.GetReadingOrderAsync().ConfigureAwait (false);
            result.ReadingOrder = ReadReadingOrder (result, htmlContentFileRefs);
            var navigationItemRefs =
                await epubBookRef.GetNavigationAsync().ConfigureAwait (false);
            result.Navigation = ReadNavigation (result, navigationItemRefs);
        }

        return result;
    }

    private static IZipFile GetZipFile (string filePath)
    {
        return FileSystem.OpenZipFile (filePath);
    }

    private static IZipFile GetZipFile (Stream stream)
    {
        return FileSystem.OpenZipFile (stream);
    }

    private static async Task<EpubContent> ReadContent
        (
            EpubContentRef contentRef
        )
    {
        var result = new EpubContent
        {
            Html = await ReadTextContentFiles (contentRef.Html).ConfigureAwait (false),
            Css = await ReadTextContentFiles (contentRef.Css).ConfigureAwait (false),
            Images = await ReadByteContentFiles (contentRef.Images).ConfigureAwait (false),
            Fonts = await ReadByteContentFiles (contentRef.Fonts).ConfigureAwait (false),
            AllFiles = new Dictionary<string, EpubContentFile>()
        };
        foreach (var textContentFile in result.Html.Concat (result.Css))
        {
            result.AllFiles.Add (textContentFile.Key, textContentFile.Value);
        }

        foreach (var byteContentFile in result.Images.Concat (result.Fonts))
        {
            result.AllFiles.Add (byteContentFile.Key, byteContentFile.Value);
        }

        foreach (var contentFileRef in contentRef.AllFiles)
        {
            if (!result.AllFiles.ContainsKey (contentFileRef.Key))
            {
                result.AllFiles.Add (contentFileRef.Key,
                    await ReadByteContentFile (contentFileRef.Value).ConfigureAwait (false));
            }
        }

        if (contentRef.Cover != null)
        {
            result.Cover = result.Images[contentRef.Cover.FileName];
        }

        if (contentRef.NavigationHtmlFile != null)
        {
            result.NavigationHtmlFile = result.Html[contentRef.NavigationHtmlFile.FileName];
        }

        return result;
    }

    private static async Task<Dictionary<string, EpubTextContentFile>> ReadTextContentFiles
        (
            Dictionary<string, EpubTextContentFileRef> textContentFileRefs
        )
    {
        var result = new Dictionary<string, EpubTextContentFile>();
        foreach (var textContentFileRef in textContentFileRefs)
        {
            var textContentFile = new EpubTextContentFile
            {
                FileName = textContentFileRef.Value.FileName,
                FilePathInEpubArchive = textContentFileRef.Value.FilePathInEpubArchive,
                ContentType = textContentFileRef.Value.ContentType,
                ContentMimeType = textContentFileRef.Value.ContentMimeType
            };
            textContentFile.Content = await textContentFileRef.Value.ReadContentAsTextAsync().ConfigureAwait (false);
            result.Add (textContentFileRef.Key, textContentFile);
        }

        return result;
    }

    private static async Task<Dictionary<string, EpubByteContentFile>> ReadByteContentFiles
        (
            Dictionary<string, EpubByteContentFileRef> byteContentFileRefs
        )
    {
        var result = new Dictionary<string, EpubByteContentFile>();
        foreach (var byteContentFileRef in byteContentFileRefs)
        {
            result.Add (byteContentFileRef.Key,
                await ReadByteContentFile (byteContentFileRef.Value).ConfigureAwait (false));
        }

        return result;
    }

    private static async Task<EpubByteContentFile> ReadByteContentFile
        (
            EpubContentFileRef contentFileRef
        )
    {
        var result = new EpubByteContentFile
        {
            FileName = contentFileRef.FileName,
            FilePathInEpubArchive = contentFileRef.FilePathInEpubArchive,
            ContentType = contentFileRef.ContentType,
            ContentMimeType = contentFileRef.ContentMimeType
        };
        result.Content = await contentFileRef.ReadContentAsBytesAsync().ConfigureAwait (false);
        return result;
    }

    private static List<EpubTextContentFile> ReadReadingOrder
        (
            EpubBook epubBook,
            List<EpubTextContentFileRef> htmlContentFileRefs
        )
    {
        return htmlContentFileRefs.Select (htmlContentFileRef => epubBook.Content.Html[htmlContentFileRef.FileName])
            .ToList();
    }

    private static List<EpubNavigationItem> ReadNavigation
        (
            EpubBook epubBook,
            List<EpubNavigationItemRef> navigationItemRefs
        )
    {
        var result = new List<EpubNavigationItem>();
        foreach (var navigationItemRef in navigationItemRefs)
        {
            var navigationItem = new EpubNavigationItem (navigationItemRef.Type)
            {
                Title = navigationItemRef.Title,
                Link = navigationItemRef.Link,
            };
            if (navigationItemRef.HtmlContentFileRef != null)
            {
                navigationItem.HtmlContentFile = epubBook.Content.Html[navigationItemRef.HtmlContentFileRef.FileName];
            }

            navigationItem.NestedItems = ReadNavigation (epubBook, navigationItemRef.NestedItems);
            result.Add (navigationItem);
        }

        return result;
    }
}
