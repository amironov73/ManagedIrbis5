// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BitmapUtility.cs -- utility routines for GDI bitmap
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Utility routines for GDI bitmap.
    /// </summary>
    public static class BitmapUtility
    {
        #region Public methods

        /// <summary>
        /// Gets the pixel data.
        /// </summary>
        public static IntPtr GetPixelData
            (
                IntPtr dibptr
            )
        {
            var bmi = (BITMAPINFOHEADER) Marshal.PtrToStructure
                (
                    dibptr,
                    typeof(BITMAPINFOHEADER)
                );
            unchecked
            {
                if ( bmi.biSizeImage == 0 )
                {
                    bmi.biSizeImage =
                        (uint) ((((bmi.biWidth * bmi.biBitCount + 31 ) & ~31 ) >> 3)
                                 * bmi.biHeight );
                }

                var result = (int) bmi.biClrUsed;
                if (result == 0
                     && bmi.biBitCount <= 8)
                {
                    result = 1 << bmi.biBitCount;
                }

                result = (int) (result * 4 + bmi.biSize + dibptr.ToInt32 ());
                return new IntPtr ( result );
            }
        } // method GetPixelData

        #endregion

    } // class BitmapUtility

} // namespace AM.Win32
