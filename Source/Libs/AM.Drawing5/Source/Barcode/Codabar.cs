// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Codabar.cs --
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
    public class Codabar
        : LinearBarcodeBase
    {
        #region Private members

        private static readonly Dictionary<char, string> _patterns = new()
        {
            ['0'] = "101010011",
            ['1'] = "101011001",
            ['2'] = "101001011",
            ['3'] = "110010101",
            ['4'] = "101101001",
            ['5'] = "110101001",
            ['6'] = "100101011",
            ['7'] = "100101101",
            ['8'] = "100110101",
            ['9'] = "110100101",
            ['-'] = "101001101",
            ['$'] = "101100101",
            [':'] = "1101011011",
            ['/'] = "1101101011",
            ['.'] = "1101101101",
            ['+'] = "101100110011",
            ['A'] = "1011001001",
            ['B'] = "1010010011",
            ['C'] = "1001001011",
            ['D'] = "1010011001",
            ['a'] = "1011001001",
            ['b'] = "1010010011",
            ['c'] = "1001001011",
            ['d'] = "1010011001",
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

            foreach (var c in text)
            {
                result.AddRange(_patterns[c]);
                result.Add('0'); // межсимвольный разделитель
            }

            // убираем последний межсимвольный разделитель
            result.RemoveAt(result.Count - 1);

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

            if (message.Length < 2)
            {
                return false;
            }

            char c1 = char.ToUpperInvariant(message[0]);
            if (c1 != 'A' && c1 != 'B' && c1 != 'C' && c1 != 'D')
            {
                return false;
            }

            char c2 = char.ToUpperInvariant(message[^1]);
            if (c2 != 'A' && c2 != 'B' && c2 != 'C' && c2 != 'D')
            {
                return false;
            }

            foreach (var c in message)
            {
                if (!_patterns.ContainsKey(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc cref="IBarcode.Symbology"/>
        public override string Symbology { get; } = "Codabar";

        #endregion
    }
}
