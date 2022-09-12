// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace PdfSharpCore.Pdf.IO;

/// <summary>
/// Exception thrown by PdfReader.
/// </summary>
public class PdfReaderException
    : PdfSharpException
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfReaderException"/> class.
    /// </summary>
    public PdfReaderException()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfReaderException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public PdfReaderException
        (
            string message
        )
        : base (message)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfReaderException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public PdfReaderException
        (
            string message,
            Exception innerException
        )
        : base (message, innerException)
    {
        // пустое тело конструктора
    }

    #endregion
}
