// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* LinearBarcodeBase.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#endregion

#nullable enable

namespace AM.Drawing.Barcodes
{
    /// <summary>
    /// Базовый класс для обычных одномерных штрихкодов.
    /// </summary>
    public abstract class LinearBarcodeBase
        : IBarcode
    {
        #region Properties

        /// <summary>
        /// Множитель для ширины полос.
        /// </summary>
        public float Weight { get; set; } = 3.0f;

        #endregion

        #region Public methods

        /// <summary>
        /// Кодирование штрих-кода в последовательность нулей и единиц.
        /// </summary>
        public abstract string Encode
            (
                BarcodeData data
            );

        /// <summary>
        /// Проверка, пригодны ли данные для штрих-кода.
        /// </summary>
        public abstract bool Verify
            (
                BarcodeData data
            );

        #endregion

        #region IBarcode members

        /// <inheritdoc cref="IBarcode.Symbology"/>
        public abstract string Symbology { get; }

        /// <inheritdoc cref="IBarcode.DrawBarcode"/>
        public virtual void DrawBarcode
            (
                BarcodeContext context
            )
        {
            var data = context.Data;
            if (data is null || !Verify(data))
            {
                return;
            }

            var encoded = Encode(data);
            encoded = "00" + encoded + "00";
            var graphics = context.Graphics.ThrowIfNull("context.Graphics");
            var bounds = context.Bounds;
            using var fore = new SolidBrush(data.ForeColor);
            using var back = new SolidBrush(data.BackColor);
            var position = bounds.Left;

            foreach (var c in encoded)
            {
                var rect = new RectangleF(position, bounds.Top, Weight, bounds.Height);
                var brush = c == '0' ? back : fore;
                graphics.FillRectangle(brush, rect);
                position += Weight;
            }
        }

        #endregion
    }
}
