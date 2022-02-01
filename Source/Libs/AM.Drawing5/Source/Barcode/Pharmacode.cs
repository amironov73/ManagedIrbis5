// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Pharmacode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace AM.Drawing.Barcodes;

/// <summary>
/// Pharmacode
/// </summary>
public class Pharmacode
    : LinearBarcodeBase
{
    #region Constants

    private const string Thin = "1";
    private const string Gap = "00";
    private const string Thick = "111";

    #endregion

    #region LinearBarcodeBase methods

    /// <inheritdoc cref="LinearBarcodeBase.Encode"/>
    public override string Encode
        (
            BarcodeData data
        )
    {
        var text = data.Message.ThrowIfNull();
        var result = new StringBuilder();
        var number = text.ParseInt32();

        do
        {
            if ((number & 1) == 0)
            {
                result.Insert (0, Thick);
                number = (number - 2) / 2;
            }
            else
            {
                result.Insert (0, Thin);
                number = (number - 1) / 2;
            }

            if (number != 0)
            {
                result.Insert (0, Gap);
            }
        } while (number != 0);

        return result.ToString();
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

        var number = message.ParseInt32();

        return number is > 2 and < 131071;
    }

    /// <inheritdoc cref="IBarcode.Symbology"/>
    public override string Symbology { get; } = "Pharmacode";

    #endregion
}
