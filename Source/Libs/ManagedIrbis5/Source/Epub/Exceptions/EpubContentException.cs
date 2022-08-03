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

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

public class EpubContentException : EpubReaderException
{
    public EpubContentException(string contentFilePath)
    {
        ContentFilePath = contentFilePath;
    }

    public EpubContentException(string message, string contentFilePath)
        : base(message)
    {
        ContentFilePath = contentFilePath;
    }

    public EpubContentException(string message, Exception innerException, string contentFilePath)
        : base(message, innerException)
    {
        ContentFilePath = contentFilePath;
    }

    public string ContentFilePath { get; }
}