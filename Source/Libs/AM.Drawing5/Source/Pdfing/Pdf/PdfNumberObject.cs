// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Base class for indirect number values (not yet used, maybe superfluous).
/// </summary>
public abstract class PdfNumberObject
    : PdfObject
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfNumberObject"/> class.
    /// </summary>
    protected PdfNumberObject()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfNumberObject"/> class.
    /// </summary>
    /// <param name="document">The document.</param>
    protected PdfNumberObject
        (
            PdfDocument document
        )
        : base (document)
    {
        // пустое тело конструктора
    }

    #endregion
}
