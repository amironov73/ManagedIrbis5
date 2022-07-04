// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XGraphicsState.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Represents the internal state of an XGraphics object.
    /// This class is used as a handle for restoring the context.
    /// </summary>
    public sealed class XGraphicsState
    {
        // This class is simply a wrapper of InternalGraphicsState.
        internal InternalGraphicsState InternalState;
    }
}
