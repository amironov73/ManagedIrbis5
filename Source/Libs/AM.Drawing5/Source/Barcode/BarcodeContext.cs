// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BarcodeContext.cs -- контекст для вывода штрих-кода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;

#endregion

#nullable enable

namespace AM.Drawing.Barcodes
{
    /// <summary>
    /// Контекст для вывода штрих-кода.
    /// </summary>
    public class BarcodeContext
    {
        #region Properties

        /// <summary>
        /// Устройство для вывода.
        /// </summary>
        public Graphics? Graphics { get; set; }

        /// <summary>
        /// Позиция штрих-кода на устройстве.
        /// </summary>
        public RectangleF Bounds { get; set; }

        /// <summary>
        /// Данные для вывода.
        /// </summary>
        public BarcodeData? Data { get; set; }

        public IServiceProvider? ServiceProvider { get; set; }

        #endregion

    } // class BarcodeContext

} // namespace AM.Drawing.Barcodes
