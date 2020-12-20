// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* HELPINFO.cs -- информация об элементе, для которого затребована подсказка
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32
{
    /// <summary>
    /// Информация об элементе, для которого была затребована контекстно-
    /// чувствительная подсказка.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HELPINFO
    {
        /// <summary>
        /// The structure size, in bytes.
        /// </summary>
        public int cbSize;

        /// <summary>
        /// The type of context for which Help is requested.
        /// This member can be one of the following values.
        ///
        /// HELPINFO_MENUITEM
        /// Help requested for a menu item.
        ///
        /// HELPINFO_WINDOW
        /// Help requested for a control or window.
        /// </summary>
        public int iContextType;

        /// <summary>
        /// The identifier of the window or control
        /// if iContextType is HELPINFO_WINDOW,
        /// or identifier of the menu item
        /// if iContextType is HELPINFO_MENUITEM.
        /// </summary>
        public int iCtrlId;

        /// <summary>
        /// The identifier of the child window or control
        /// if iContextType is HELPINFO_WINDOW,
        /// or identifier of the associated menu
        /// if iContextType is HELPINFO_MENUITEM.
        /// </summary>
        public IntPtr hItemHandle;

        /// <summary>
        /// The help context identifier of the window or control.
        /// </summary>
        public int dwContextId;

        /// <summary>
        /// The POINT structure that contains the screen coordinates
        /// of the mouse cursor. This is useful for providing Help
        /// based on the position of the mouse cursor.
        /// </summary>
        public Point MousePos;

    } // struct HELPINFO

} // namespace AM.Win32
