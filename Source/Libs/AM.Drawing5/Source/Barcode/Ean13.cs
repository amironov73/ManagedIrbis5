// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Ean13.cs --
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
    /// EAN 13
    /// </summary>
    public class Ean13
        : LinearBarcodeBase
    {
        #region LinearBarcodeBase methods

        /// <inheritdoc cref="LinearBarcodeBase.Encode"/>
        public override string Encode
            (
                string text
            )
        {
            var result = new List<char>();

            return new string(result.ToArray());
        }

        /// <inheritdoc cref="LinearBarcodeBase.Verify"/>
        public override bool Verify
            (
                BarcodeData data
            )
        {
            var message = data.Message;

            if (string.IsNullOrWhiteSpace(message))
            {
                return false;
            }

            foreach (var c in message)
            {
            }

            return true;
        }

        /// <inheritdoc cref="IBarcode.Symbology"/>
        public override string Symbology { get; } = "EAN13";

        #endregion
    }
}
