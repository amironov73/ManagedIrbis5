// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Standard2of5.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Drawing.Barcodes
{
    /// <summary>
    ///
    /// </summary>
    public class Standard2of5
        : LinearBarcodeBase
    {
        #region Private members

        private static readonly string[] _patterns =
        {
            "10101110111010",
            "11101010101110",
            "10111010101110",
            "11101110101010",
            "10101110101110",
            "11101011101010",
            "10111011101010",
            "10101011101110",
            "11101010111010",
            "10111010111010"
        };

        #endregion

        #region LinearBarcodeBase members

        /// <inheritdoc cref="LinearBarcodeBase.Encode"/>
        public override string Encode
            (
                BarcodeData data
            )
        {
            var text = data.Message.ThrowIfNull("data.Message");
            var result = new List<char>();

            result.AddRange("11011010");

            foreach (var c in text)
            {
                var digit = c - '0';
                result.AddRange(_patterns[digit]);
            }

            result.AddRange("1101011");

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
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc cref="LinearBarcodeBase.Symbology"/>
        public override string Symbology { get; } = "Standard 2 of 5";

        #endregion
    }
}
