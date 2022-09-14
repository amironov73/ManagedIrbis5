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

using AM;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Holds PDF specific information of the document.
/// </summary>
public sealed class PdfDocumentSettings
{
    #region Construction

    internal PdfDocumentSettings
        (
            PdfDocument document
        )
    {
        document.NotUsed();
    }

    #endregion

    /// <summary>
    /// Gets or sets the default trim margins.
    /// </summary>
    public TrimMargins TrimMargins
    {
        get
        {
            if (_trimMargins == null)
                _trimMargins = new TrimMargins();
            return _trimMargins;
        }
        set
        {
            if (_trimMargins == null)
                _trimMargins = new TrimMargins();
            if (value != null)
            {
                _trimMargins.Left = value.Left;
                _trimMargins.Right = value.Right;
                _trimMargins.Top = value.Top;
                _trimMargins.Bottom = value.Bottom;
            }
            else
                _trimMargins.All = 0;
        }
    }

    TrimMargins _trimMargins = new TrimMargins();
}
