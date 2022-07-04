// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* BcgSR.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Drawing.BarCodes
{
    // TODO: Mere with PDFsharp strings table
    /// <summary>
    /// String resources for the empira barcode renderer.
    /// </summary>
    internal class BcgSR
    {
        internal static string Invalid2Of5Code(string code)
        {
            return string.Format("'{0}' is not a valid code for an interleave 2 of 5 bar code. It can only represent an even number of digits.", code);
        }

        internal static string Invalid3Of9Code(string code)
        {
            return string.Format("'{0}' is not a valid code for a 3 of 9 standard bar code.", code);
        }

        internal static string BarCodeNotSet
        {
            get { return "A text must be set before rendering the bar code."; }
        }

        internal static string EmptyBarCodeSize
        {
            get { return "A non-empty size must be set before rendering the bar code."; }
        }

        internal static string Invalid2of5Relation
        {
            get { return "Value of relation between thick and thin lines on the interleaved 2 of 5 code must be between 2 and 3."; }
        }

        internal static string InvalidMarkName(string name)
        {
            return string.Format("'{0}' is not a valid mark name for this OMR representation.", name);
        }

        internal static string OmrAlreadyInitialized
        {
            get { return "Mark descriptions cannot be set when marks have already been set on OMR."; }
        }

        internal static string DataMatrixTooBig
        {
            get { return "The given data and encoding combination is too big for the matrix size."; }
        }

        internal static string DataMatrixNotSupported
        {
            get { return "Zero sizes, odd sizes and other than ecc200 coded DataMatrix is not supported."; }
        }

        internal static string DataMatrixNull
        {
            get { return "No DataMatrix code is produced."; }
        }

        internal static string DataMatrixInvalid(int columns, int rows)
        {
            return string.Format("'{1}'x'{0}' is an invalid ecc200 DataMatrix size.", columns, rows);
        }
    }
}
