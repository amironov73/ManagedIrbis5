// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XMatrixOrder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Specifies the order for matrix transform operations.
    /// </summary>
    public enum XMatrixOrder
    {
        /// <summary>
        /// The new operation is applied before the old operation.
        /// </summary>
        Prepend = 0,

        /// <summary>
        /// The new operation is applied after the old operation.
        /// </summary>
        Append = 1,
    }
}
