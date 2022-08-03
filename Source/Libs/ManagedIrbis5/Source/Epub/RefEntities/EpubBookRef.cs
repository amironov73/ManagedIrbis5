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

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ManagedIrbis.Epub.Environment;
using ManagedIrbis.Epub.Internal;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

public class EpubBookRef : IDisposable
{
    private bool isDisposed;

    public EpubBookRef(IZipFile epubFile)
    {
        EpubFile = epubFile;
        isDisposed = false;
    }

    ~EpubBookRef()
    {
        Dispose(false);
    }

    public string FilePath { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public List<string> AuthorList { get; set; }
    public string Description { get; set; }
    public EpubSchema Schema { get; set; }
    public EpubContentRef Content { get; set; }

    internal IZipFile EpubFile { get; private set; }

    public byte[] ReadCover()
    {
        return ReadCoverAsync().Result;
    }

    public async Task<byte[]> ReadCoverAsync()
    {
        if (Content.Cover == null)
        {
            return null;
        }
        return await Content.Cover.ReadContentAsBytesAsync().ConfigureAwait(false);
    }

    public List<EpubTextContentFileRef> GetReadingOrder()
    {
        return GetReadingOrderAsync().Result;
    }

    public async Task<List<EpubTextContentFileRef>> GetReadingOrderAsync()
    {
        return await Task.Run(() => SpineReader.GetReadingOrder(this)).ConfigureAwait(false);
    }

    public List<EpubNavigationItemRef> GetNavigation()
    {
        return GetNavigationAsync().Result;
    }

    public async Task<List<EpubNavigationItemRef>> GetNavigationAsync()
    {
        return await Task.Run(() => NavigationReader.GetNavigationItems(this)).ConfigureAwait(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
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