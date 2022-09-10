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

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Represents a PDF page object.
/// </summary>
internal class PdfPageInheritableObjects
    : PdfDictionary
{
    // TODO Inheritable Resources not yet supported

    /// <summary>
    ///
    /// </summary>
    public PdfRectangle? MediaBox { get; set; }

    public PdfRectangle? CropBox { get; set; }

    public int Rotate
    {
        get => _rotate;
        set
        {
            if (value % 90 != 0)
                throw new ArgumentException ("Rotate", "The value must be a multiple of 90.");
            _rotate = value;
        }
    }

    private int _rotate;
}
