// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfExtGStateTable.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Contains all used ExtGState objects of a document.
/// </summary>
public sealed class PdfExtGStateTable
    : PdfResourceTable
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of this class, which is a singleton for each document.
    /// </summary>
    public PdfExtGStateTable (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Gets a PdfExtGState with the key 'CA' set to the specified alpha value.
    /// </summary>
    public PdfExtGState GetExtGStateStroke (double alpha, bool overprint)
    {
        var key = PdfExtGState.MakeKey (alpha, overprint);
        if (!_strokeAlphaValues.TryGetValue (key, out var extGState))
        {
            extGState = new PdfExtGState (Owner)
            {
                StrokeAlpha = alpha
            };

            //extGState.Elements[PdfExtGState.Keys.CA] = new PdfReal(alpha);

            if (overprint)
            {
                extGState.StrokeOverprint = true;
                extGState.Elements.SetInteger (PdfExtGState.Keys.OPM, 1);
            }

            _strokeAlphaValues[key] = extGState;
        }

        return extGState;
    }

    /// <summary>
    /// Gets a PdfExtGState with the key 'ca' set to the specified alpha value.
    /// </summary>
    public PdfExtGState GetExtGStateNonStroke (double alpha, bool overprint)
    {
        var key = PdfExtGState.MakeKey (alpha, overprint);
        if (!_nonStrokeStates.TryGetValue (key, out var extGState))
        {
            extGState = new PdfExtGState (Owner)
            {
                NonStrokeAlpha = alpha
            };

            //extGState.Elements[PdfExtGState.Keys.ca] = new PdfReal(alpha);

            if (overprint)
            {
                extGState.NonStrokeOverprint = true;
                extGState.Elements.SetInteger (PdfExtGState.Keys.OPM, 1);
            }

            _nonStrokeStates[key] = extGState;
        }

        return extGState;
    }

    readonly Dictionary<string, PdfExtGState> _strokeAlphaValues = new ();
    readonly Dictionary<string, PdfExtGState> _nonStrokeStates = new ();
}
