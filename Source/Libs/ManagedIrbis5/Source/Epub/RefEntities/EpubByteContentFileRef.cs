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

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

public class EpubByteContentFileRef : EpubContentFileRef
{
    public EpubByteContentFileRef(EpubBookRef epubBookRef, string fileName, EpubContentType contentType, string contentMimeType)
        : base(epubBookRef, fileName, contentType, contentMimeType)
    {
    }

    public byte[] ReadContent()
    {
        return ReadContentAsBytes();
    }

    public Task<byte[]> ReadContentAsync()
    {
        return ReadContentAsBytesAsync();
    }
}