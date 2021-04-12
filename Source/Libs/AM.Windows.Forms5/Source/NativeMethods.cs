// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/*
 * NativeMethods.cs
 */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    static class NativeMethods
    {
        [Flags]
        public enum ScrollInfoFlags : uint
        {
            SIF_RANGE = 0x0001,
            SIF_PAGE = 0x0002,
            SIF_POS = 0x0004,
            SIF_DISABLENOSCROLL = 0x0008,
            SIF_TRACKPOS = 0x0010,
            SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS
        }

        public enum ScrollBarKind
        {
            SB_HORZ = 0,
            SB_VERT = 1,
            SB_CTL = 2,
            SB_BOTH = 3
        }

        [Flags]
        public enum EnableScrollBarFlags
        {
            ESB_ENABLE_BOTH = 0x0000,
            ESB_DISABLE_BOTH = 0x0003,

            ESB_DISABLE_LEFT = 0x0001,
            ESB_DISABLE_RIGHT = 0x0002,

            ESB_DISABLE_UP = 0x0001,
            ESB_DISABLE_DOWN = 0x0002
        }

        public enum ObjectId : uint
        {
            OBJID_WINDOW = 0x00000000,
            OBJID_SYSMENU = 0xFFFFFFFF,
            OBJID_TITLEBAR = 0xFFFFFFFE,
            OBJID_MENU = 0xFFFFFFFD,
            OBJID_CLIENT = 0xFFFFFFFC,
            OBJID_VSCROLL = 0xFFFFFFFB,
            OBJID_HSCROLL = 0xFFFFFFFA,
            OBJID_SIZEGRIP = 0xFFFFFFF9,
            OBJID_CARET = 0xFFFFFFF8,
            OBJID_CURSOR = 0xFFFFFFF7,
            OBJID_ALERT = 0xFFFFFFF6,
            OBJID_SOUND = 0xFFFFFFF5,
            OBJID_QUERYCLASSNAMEIDX = 0xFFFFFFF4,
            OBJID_NATIVEOM = 0xFFFFFFF0
        }

        [Flags]
        public enum ScrollWindowFlags : uint
        {
            SW_SCROLLCHILDREN = 0x0001,  /* Scroll children within *lprcScroll. */
            SW_INVALIDATE = 0x0002,  /* Invalidate after scrolling */
            SW_ERASE = 0x0004,  /* If SW_INVALIDATE, don't send WM_ERASEBACKGROUND */
            SW_SMOOTHSCROLL = 0x0010  /* Use smooth scrolling */
        }

        public const int CCHILDREN_SCROLLBAR = 5;

        [StructLayout(LayoutKind.Sequential)]
        public struct SCROLLBARINFO
        {
            public const int StructureSize = 60;

            public uint cbSize;
            public Rectangle rcScrollBar;
            public int dxyLineButton;
            public int xyThumbTop;
            public int xyThumbBottom;
            public int reserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = CCHILDREN_SCROLLBAR + 1)]
            public uint[] rgstate;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct SCROLLINFO
        {
            public const int StructureSize = 7 * 4;

            public uint cbSize;
            public ScrollInfoFlags fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        };

        public const int WS_HSCROLL = 0x00100000;
        public const int WS_VSCROLL = 0x00200000;

        public const int WM_WININICHANGE = 0x001A;
        public const int WM_HSCROLL = 0x0114;
        public const int WM_VSCROLL = 0x0115;

        public const int SB_LINEUP = 0;
        public const int SB_LINELEFT = 0;
        public const int SB_LINEDOWN = 1;
        public const int SB_LINERIGHT = 1;
        public const int SB_PAGEUP = 2;
        public const int SB_PAGELEFT = 2;
        public const int SB_PAGEDOWN = 3;
        public const int SB_PAGERIGHT = 3;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_THUMBTRACK = 5;
        public const int SB_TOP = 6;
        public const int SB_LEFT = 6;
        public const int SB_BOTTOM = 7;
        public const int SB_RIGHT = 7;
        public const int SB_ENDSCROLL = 8;

        public const string User32Dll = "User32.dll";

        [DllImport(User32Dll)]
        public static extern bool EnableScrollBar
            (
                IntPtr hWnd,
                ScrollBarKind wSBflags,
                EnableScrollBarFlags wArrows
            );

        [DllImport(User32Dll)]
        public static extern bool GetScrollBarInfo
            (
                IntPtr hwnd,
                ObjectId idObject,
                ref SCROLLBARINFO psbi
            );

        [DllImport(User32Dll)]
        public static extern bool GetScrollInfo
            (
                IntPtr hwnd,
                ScrollBarKind fnBar,
                ref SCROLLINFO lpsi
            );

        [DllImport(User32Dll)]
        public static extern int GetScrollPos
            (
                IntPtr hWnd,
                ScrollBarKind nBar
            );

        [DllImport(User32Dll)]
        public static extern bool GetScrollRange
            (
                IntPtr hWnd,
                ScrollBarKind nBar,
                out int lpMinPos,
                out int lpMaxPos
            );

        [DllImport(User32Dll)]
        public static extern bool ScrollDC
            (
                IntPtr hDC,
                int dx,
                int dy,
                ref Rectangle lprcScroll,
                ref Rectangle lprcClip,
                IntPtr hrgnUpdate,
                ref Rectangle lprcUpdate
            );

        [DllImport(User32Dll)]
        public static extern bool ScrollWindow
            (
                IntPtr hWnd,
                int XAmount,
                int YAmount,
                ref Rectangle lpRect,
                ref Rectangle lpClipRect
            );

        [DllImport(User32Dll)]
        public static extern int ScrollWindowEx
            (
                IntPtr hWnd,
                int dx,
                int dy,
                ref Rectangle prcScroll,
                ref Rectangle prcClip,
                IntPtr hrgnUpdate,
                ref Rectangle prcUpdate,
                ScrollWindowFlags flags
            );


        [DllImport(User32Dll)]
        public static extern int SetScrollInfo
            (
                IntPtr hwnd,
                ScrollBarKind fnBar,
                ref SCROLLINFO lpsi,
                bool fRedraw
            );

        [DllImport(User32Dll)]
        public static extern int SetScrollPos
            (
                IntPtr hWnd,
                ScrollBarKind nBar,
                int nPos,
                bool bRedraw
            );

        [DllImport(User32Dll)]
        public static extern bool SetScrollRange
            (
                IntPtr hWnd,
                ScrollBarKind nBar,
                int nMinPos,
                int nMaxPos,
                bool bRedraw
            );

        [DllImport(User32Dll)]
        public static extern bool ShowScrollBar
            (
                IntPtr hWnd,
                ScrollBarKind wBar,
                bool bShow
            );


        public static int LOWORD(int n)
        {
            return (n & 0xffff);
        }

        public static int LOWORD(IntPtr n)
        {
            return LOWORD((int)((long)n));
        }

    }
}
