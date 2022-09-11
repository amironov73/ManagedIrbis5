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

#nullable enable

namespace PdfSharpCore.Pdf.Content;

/// <summary>
/// Exception thrown by ContentReader.
/// </summary>
public class ContentReaderException
    : PdfSharpException
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentReaderException"/> class.
    /// </summary>
    public ContentReaderException()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentReaderException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public ContentReaderException
        (
            string message
        )
        : base (message)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentReaderException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ContentReaderException
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
