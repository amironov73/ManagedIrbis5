// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    partial class DrawUtils
    {
#if CROSSPLATFORM
        static IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, DrawingOptions drawingOptions)
        {
            return IntPtr.Zero;
        }
#else
        [DllImport ("user32.dll")]
        static extern nint SendMessage (nint hWnd, int msg, nint wParam, DrawingOptions drawingOptions);
#endif

        const int WM_PRINT = 0x317;

        [Flags]
        enum DrawingOptions
        {
            PRF_CHECKVISIBLE = 0x01,
            PRF_NONCLIENT = 0x02,
            PRF_CLIENT = 0x04,
            PRF_ERASEBKGND = 0x08,
            PRF_CHILDREN = 0x10,
            PRF_OWNED = 0x20
        }

        /// <summary>
        /// Draws control to a bitmap.
        /// </summary>
        /// <param name="control">Control to draw.</param>
        /// <param name="children">Determines whether to draw control's children or not.</param>
        /// <returns>The bitmap.</returns>
        public static Bitmap DrawToBitmap (Control control, bool children)
        {
            var bitmap = new Bitmap (control.Width, control.Height);
            using (var gr = Graphics.FromImage (bitmap))
            {
                var hdc = gr.GetHdc();
                var options = DrawingOptions.PRF_ERASEBKGND |
                              DrawingOptions.PRF_CLIENT | DrawingOptions.PRF_NONCLIENT;
                if (children)
                {
                    options |= DrawingOptions.PRF_CHILDREN;
                }

                SendMessage (control.Handle, WM_PRINT, hdc, options);
                gr.ReleaseHdc (hdc);
            }

            return bitmap;
        }
    }
}
