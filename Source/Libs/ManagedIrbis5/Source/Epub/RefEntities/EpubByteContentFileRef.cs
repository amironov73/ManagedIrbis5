// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* EpubByteContentRef.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public class EpubByteContentFileRef
    : EpubContentFileRef
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="epubBookRef"></param>
    /// <param name="fileName"></param>
    /// <param name="contentType"></param>
    /// <param name="contentMimeType"></param>
    public EpubByteContentFileRef
        (
            EpubBookRef epubBookRef,
            string fileName,
            EpubContentType contentType,
            string contentMimeType
        )
        : base (epubBookRef, fileName, contentType, contentMimeType)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public byte[] ReadContent()
    {
        return ReadContentAsBytes();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Task<byte[]> ReadContentAsync()
    {
        return ReadContentAsBytesAsync();
    }
}
