// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* EpubBookRef.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ManagedIrbis.Epub.Environment;
using ManagedIrbis.Epub.Internal;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public class EpubBookRef
    : IDisposable
{
    private bool isDisposed;

    /// <summary>
    ///
    /// </summary>
    /// <param name="epubFile"></param>
    public EpubBookRef
        (
            IZipFile epubFile
        )
    {
        EpubFile = epubFile;
        isDisposed = false;
    }

    /// <summary>
    /// Деструктор.
    /// </summary>
    ~EpubBookRef()
    {
        Dispose (false);
    }

    /// <summary>
    ///
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<string>? AuthorList { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubSchema? Schema { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubContentRef? Content { get; set; }

    /// <summary>
    ///
    /// </summary>
    internal IZipFile? EpubFile { get; private set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public byte[]? ReadCover()
    {
        return ReadCoverAsync().Result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<byte[]?> ReadCoverAsync()
    {
        if (Content?.Cover is null)
        {
            return null;
        }

        return await Content.Cover.ReadContentAsBytesAsync().ConfigureAwait (false);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public List<EpubTextContentFileRef> GetReadingOrder()
    {
        return GetReadingOrderAsync().Result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<List<EpubTextContentFileRef>> GetReadingOrderAsync()
    {
        return await Task.Run (() => SpineReader.GetReadingOrder (this)).ConfigureAwait (false);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public List<EpubNavigationItemRef> GetNavigation()
    {
        return GetNavigationAsync().Result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<List<EpubNavigationItemRef>> GetNavigationAsync()
    {
        return await Task.Run
            (
                () => NavigationReader.GetNavigationItems (this)!
            )
            .ConfigureAwait (false);
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Dispose (true);
        GC.SuppressFinalize (this);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose
        (
            bool disposing
        )
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                EpubFile?.Dispose();
            }

            isDisposed = true;
        }
    }
}
