// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BarcodeData.cs -- данные для вывода штрих-кода на устройство
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
    /// Данные для вывода штрих-кода на устройство.
    /// </summary>
    public class BarcodeData
    {
        #region Properties

        /// <summary>
        /// Сообщение для вывода.
        /// </summary>
        public string? Message;

        /// <summary>
        /// Человеко-читаемое отображение?
        /// </summary>
        public bool HumanReadable;

        /// <summary>
        ///
        /// </summary>
        public bool DrawCheckDigit;

        /// <summary>
        ///
        /// </summary>
        public bool DrawStartAndStop;

        /// <summary>
        ///
        /// </summary>
        public Color ForeColor = Color.Black;

        /// <summary>
        ///
        /// </summary>
        public Color BackColor = Color.White;

        /// <summary>
        ///
        /// </summary>
        public bool Debug;

        #endregion

    } // class BarcodeData

} // namespace AM.Drawing.Barcodes
