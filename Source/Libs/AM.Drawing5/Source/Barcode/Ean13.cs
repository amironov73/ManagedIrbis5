// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Ean13.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Drawing.Barcodes
{
    /// <summary>
    /// EAN 13.
    /// </summary>
    public sealed class Ean13
        : LinearBarcodeBase
    {
        #region Private members

        private static readonly string[] _codesA =
        {
            "0001101",
            "0011001",
            "0010011",
            "0111101",
            "0100011",
            "0110001",
            "0101111",
            "0111011",
            "0110111",
            "0001011"
        };

        private static readonly string[] _codesB =
        {
            "0100111",
            "0110011",
            "0011011",
            "0100001",
            "0011101",
            "0111001",
            "0000101",
            "0010001",
            "0001001",
            "0010111"
        };

        private static readonly string[] _codesC =
        {
            "1110010",
            "1100110",
            "1101100",
            "1000010",
            "1011100",
            "1001110",
            "1010000",
            "1000100",
            "1001000",
            "1110100"
        };

        private static readonly string[] _patterns =
        {
            "aaaaaa",
            "aababb",
            "aabbab",
            "aabbba",
            "abaabb",
            "abbaab",
            "abbbaa",
            "ababab",
            "ababba",
            "abbaba"
        };

        #endregion

        #region Public methods

        /// <summary>
        /// Расчет контрольной цифры.
        /// </summary>
        public char ComputeCheckDigit
            (
                string text
            )
        {
            unchecked
            {
                var sum = 0;

                for (var i = 0; i < 12; i++)
                {
                    var c = text[i] - '0';
                    sum += i % 2 == 0 ? c : c * 3;
                }

                var result = (10 - sum % 10) % 10;

                return (char) (result + '0');
            }
        }

        #endregion

        #region LinearBarcodeBase methods

        /// <inheritdoc cref="LinearBarcodeBase.Encode"/>
        public override string Encode
            (
                BarcodeData data
            )
        {
            var text = data.Message.ThrowIfNull();
            var builder = StringBuilderPool.Shared.Get();
            builder.EnsureCapacity (3 * 3 + 13 * 7);

            var check = ComputeCheckDigit (text);
            text = text.Substring (0, 12) + check;
            var c = text[0] - '0';
            var pattern = _patterns[c];

            builder.Append ("101"); // открывающая последовательность

            for (var i = 0; i < 6; i++)
            {
                c = text[i + 1] - '0';
                builder.Append
                    (
                        pattern[i] == 'a'
                            ? _codesA[c]
                            : _codesB[c]
                    );
            }

            builder.Append ("01010"); // разделитель

            for (var i = 7; i < text.Length; i++)
            {
                c = text[i] - '0';
                builder.Append (_codesC[c]);
            }

            builder.Append ("101"); // закрывающая последовательность

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;

        } // method Encode

        /// <inheritdoc cref="LinearBarcodeBase.Verify"/>
        public override bool Verify
            (
                BarcodeData data
            )
        {
            var message = data.Message;

            if (string.IsNullOrWhiteSpace (message))
            {
                return false;
            }

            foreach (var c in message)
            {
                if (!char.IsDigit (c))
                {
                    return false;
                }
            }

            return true;

        } // method Verify

        /// <inheritdoc cref="IBarcode.Symbology"/>
        public override string Symbology { get; } = "EAN13";

        #endregion

    } // class Ean13

} // namespace AM.Drawing.Barcodes
