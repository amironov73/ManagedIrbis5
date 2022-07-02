// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* UpcA.cs -- UPC-A
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Drawing.Barcodes;

/// <summary>
/// UPC-A
/// </summary>
public sealed class UpcA
    : LinearBarcodeBase
{
    #region Private members

    private static readonly string[] _leftCodes =
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

    private static readonly string[] _rightCodes =
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

            for (var i = 0; i < 11; i++)
            {
                var c = text[i] - '0';
                sum += i % 2 == 0 ? c * 3 : c;
            }

            var result = (10 - sum % 10) % 10;

            return (char)(result + '0');
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
        Sure.NotNull (data);

        var text = data.Message.ThrowIfNull();
        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (3 + 5 + 3 + 12 * 7);

        var check = ComputeCheckDigit (text);
        text = text.Substring (0, 11) + check;

        builder.Append ("101"); // открывающая последовательность

        for (var i = 0; i < 6; i++)
        {
            var c = text[i] - '0';
            builder.Append (_leftCodes[c]);
        }

        builder.Append ("01010"); // разделитель

        for (var i = 6; i < text.Length; i++)
        {
            var c = text[i] - '0';
            builder.Append (_rightCodes[c]);
        }

        builder.Append ("101"); // закрывающая последовательность

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <inheritdoc cref="LinearBarcodeBase.Verify"/>
    public override bool Verify
        (
            BarcodeData data
        )
    {
        Sure.NotNull (data);

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
    }

    /// <inheritdoc cref="IBarcode.Symbology"/>
    public override string Symbology { get; } = "UPC-A";

    #endregion
}
