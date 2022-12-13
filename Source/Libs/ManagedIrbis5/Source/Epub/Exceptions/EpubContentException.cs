// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubContentException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public class EpubContentException
    : EpubReaderException
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="contentFilePath"></param>
    public EpubContentException
        (
            string contentFilePath
        )
    {
        Sure.NotNullNorEmpty (contentFilePath);

        ContentFilePath = contentFilePath;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="contentFilePath"></param>
    public EpubContentException
        (
            string message,
            string contentFilePath
        )
        : base (message)
    {
        Sure.NotNullNorEmpty (contentFilePath);

        ContentFilePath = contentFilePath;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="message"></param>
    /// <param name="innerException"></param>
    /// <param name="contentFilePath"></param>
    public EpubContentException
        (
            string message,
            Exception innerException,
            string contentFilePath
        )
        : base (message, innerException)
    {
        Sure.NotNullNorEmpty (contentFilePath);

        ContentFilePath = contentFilePath;
    }

    /// <summary>
    ///
    /// </summary>
    public string ContentFilePath { get; }
}
