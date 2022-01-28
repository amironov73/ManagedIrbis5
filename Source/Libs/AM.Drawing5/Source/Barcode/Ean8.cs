// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Ean8.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Drawing.Barcodes;

/// <summary>
/// EAN 8
/// </summary>
public sealed class Ean8
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

    private readonly string[] _codesB =
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

    #region LinearBarcodeBase methods

    /// <inheritdoc cref="LinearBarcodeBase.Encode"/>
    public override string Encode
        (
            BarcodeData data
        )
    {
        var text = data.Message.ThrowIfNull();
        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (3 * 3 + 7 * 8);

        builder.Append ("101"); // открывающая последовательность

        var half = text.Length / 2;

        for (var i = 0; i < half; i++)
        {
            var c = text[i] - '0';
            builder.Append (_codesA[c]);
        }

        builder.Append ("01010"); // разделитель

        for (var i = half; i < text.Length; i++)
        {
            var c = text[i] - '0';
            builder.Append (_codesB[c]);
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
    public override string Symbology { get; } = "EAN13";

    #endregion
}
