// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* EpubContentFileRef.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Threading.Tasks;

using ManagedIrbis.Epub.Environment;
using ManagedIrbis.Epub.Internal;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public abstract class EpubContentFileRef
{
    private readonly EpubBookRef _epubBookRef;

    /// <summary>
    ///
    /// </summary>
    /// <param name="epubBookRef"></param>
    /// <param name="fileName"></param>
    /// <param name="contentType"></param>
    /// <param name="contentMimeType"></param>
    protected EpubContentFileRef
        (
            EpubBookRef epubBookRef,
            string fileName,
            EpubContentType contentType,
            string contentMimeType
        )
    {
        _epubBookRef = epubBookRef;
        FileName = fileName;
        FilePathInEpubArchive = ZipPathUtils.Combine
            (
                epubBookRef.Schema!.ContentDirectoryPath!,
                FileName
            );
        ContentType = contentType;
        ContentMimeType = contentMimeType;
    }

    /// <summary>
    ///
    /// </summary>
    public string FileName { get; }

    /// <summary>
    ///
    /// </summary>
    public string FilePathInEpubArchive { get; }

    /// <summary>
    ///
    /// </summary>
    public EpubContentType ContentType { get; }

    /// <summary>
    ///
    /// </summary>
    public string ContentMimeType { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public byte[] ReadContentAsBytes()
    {
        return ReadContentAsBytesAsync().Result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<byte[]> ReadContentAsBytesAsync()
    {
        var contentFileEntry = GetContentFileEntry();
        var content = new byte[(int)contentFileEntry.Length];
        using (var contentStream = OpenContentStream (contentFileEntry))
        using (var memoryStream = new MemoryStream (content))
        {
            await contentStream.CopyToAsync (memoryStream).ConfigureAwait (false);
        }

        return content;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string ReadContentAsText()
    {
        return ReadContentAsTextAsync().Result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<string> ReadContentAsTextAsync()
    {
        using (var contentStream = GetContentStream())
        using (var streamReader = new StreamReader (contentStream))
        {
            return await streamReader.ReadToEndAsync().ConfigureAwait (false);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Stream GetContentStream()
    {
        return OpenContentStream (GetContentFileEntry());
    }

    private IZipFileEntry GetContentFileEntry()
    {
        if (string.IsNullOrEmpty (FileName))
        {
            throw new EpubPackageException ("EPUB parsing error: file name of the specified content file is empty.");
        }

        var contentFilePath = FilePathInEpubArchive;
        var contentFileEntry = _epubBookRef.EpubFile!.GetEntry (contentFilePath);
        if (contentFileEntry == null)
        {
            throw new EpubContentException
                (
                    $"EPUB parsing error: file \"{contentFilePath}\" was not found in the EPUB file.",
                contentFilePath
                );
        }

        if (contentFileEntry.Length > Int32.MaxValue)
        {
            throw new EpubContentException ($"EPUB parsing error: file \"{contentFilePath}\" is larger than 2 GB.",
                contentFilePath);
        }

        return contentFileEntry;
    }

    private Stream OpenContentStream
        (
            IZipFileEntry contentFileEntry
        )
    {
        var contentStream = contentFileEntry.Open();
        if (contentStream == null)
        {
            throw new EpubContentException
                (
                    $"Incorrect EPUB file: content file \"{FileName}\" specified in the manifest was not found in the EPUB file.",
                    FilePathInEpubArchive
                );
        }

        return contentStream;
    }
}
