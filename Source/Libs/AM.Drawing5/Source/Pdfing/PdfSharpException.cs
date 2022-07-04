// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfSharpException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace PdfSharpCore
{
    /// <summary>
    /// Base class of all exceptions in the PDFsharp frame work.
    /// </summary>
    public class PdfSharpException : Exception
    {
        // The class is not yet used

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfSharpException"/> class.
        /// </summary>
        public PdfSharpException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfSharpException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public PdfSharpException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfSharpException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PdfSharpException(string message, Exception innerException) :
            base(message, innerException)
        { }
    }
}
