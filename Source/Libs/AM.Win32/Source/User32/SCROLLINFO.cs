// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SCROLLINFO.cs -- scroll bar parameters
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// The SCROLLINFO structure contains scroll bar parameters to be set by
    /// the SetScrollInfo function (or SBM_SETSCROLLINFO message), or retrieved
    /// by the GetScrollInfo function (or SBM_GETSCROLLINFO message).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = Size)]
    public struct SCROLLINFO
    {
        /// <summary>
        /// Size of structure, in bytes.
        /// </summary>
        public const int Size = 28;

        /// <summary>
        /// Specifies the size, in bytes, of this structure.
        /// </summary>
        public int cbSize;

        /// <summary>
        /// Specifies the scroll bar parameters to set or retrieve.
        /// </summary>
        public int fMask;

        /// <summary>
        /// Specifies the minimum scrolling position.
        /// </summary>
        public int nMin;

        /// <summary>
        /// Specifies the maximum scrolling position.
        /// </summary>
        public int nMax;

        /// <summary>
        /// Specifies the page size. A scroll bar uses this value to
        /// determine the appropriate size of the proportional scroll box.
        /// </summary>
        public int nPage;

        /// <summary>
        /// Specifies the position of the scroll box.
        /// </summary>
        public int nPos;

        /// <summary>
        /// Specifies the immediate position of a scroll box that the user
        /// is dragging. An application can retrieve this value while processing
        /// the SB_THUMBTRACK request code. An application cannot set the
        /// immediate scroll position; the SetScrollInfo function ignores
        /// this member.
        /// </summary>
        public int nTrackPos;

    } // struct SCROLLINFO

} // namespace AM.Win32
