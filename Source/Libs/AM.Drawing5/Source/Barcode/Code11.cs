// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Code11.cs -- штрихкод, поддерживающий цифры и дефис
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Drawing.Barcodes;

//
// https://en.wikipedia.org/wiki/Code_11
//
// Code 11 — символика штрих-кодов, разработанная компанией
// Intermec в 1977 году. В основном используется в телекоммуникациях.
// Символ может кодировать строки произвольной длины (однако
// Labeljoy ограничивает их 255 знаками), состоящие из цифр 0-9
// и символа дефис (-). Может содержать одну или две контрольные цифры.
//
//

/// <summary>
/// Штрихкод, поддерживающий цифры и дефис.
/// </summary>
public sealed class Code11
    : LinearBarcodeBase
{
    #region Constants

    /// <summary>
    /// Разрешенные символы.
    /// </summary>
    public const string Alphabet = "0123456789-";

    #endregion

    #region Private members

    // паттерны для отображения символов
    private static readonly string[] _patterns =
    {
        "101011", // 0
        "1101011", // 1
        "1001011", // 2
        "1100101", // 3
        "1011011", // 4
        "1101101", // 5
        "1001101", // 6
        "1010011", // 7
        "1101001", // 8
        "110101", // 9
        "101101", // -
        "1011001" // start/stop
    };

    #endregion

    #region LinearBarcodeBase members

    /// <inheritdoc cref="LinearBarcodeBase.Encode"/>
    public override string Encode
        (
            BarcodeData data
        )
    {
        Sure.NotNull (data);

        var text = data.Message.ThrowIfNull();
        var builder = StringBuilderPool.Shared.Get();
        var checksumC = 0;
        var weight = 1;

        // вычисляем контрольную сумму C
        for (var i = text.Length - 1; i >= 0; i--)
        {
            if (weight == 10)
            {
                weight = 1;
            }

            var c = text[i];
            var d = c == '-' ? 10 : c - '0';
            checksumC += d * weight;
            ++weight;
        }

        var withChecksums = StringBuilderPool.Shared.Get();
        withChecksums.EnsureCapacity (text.Length);
        withChecksums.Append (text);
        checksumC %= 11;
        withChecksums.Append (checksumC.ToInvariantString());

        // рекомендуется вычислять контрольную сумму K
        // для сообщений длиной >= 10
        if (text.Length >= 10)
        {
            weight = 1;
            var checksumK = 0;

            for (var i = withChecksums.Length; i >= 0; i--)
            {
                if (weight == 9)
                {
                    weight = 1;
                }

                var c = withChecksums[i];
                var d = c == '-' ? 10 : c - '0';
                checksumK += d * weight;
                ++weight;
            }

            checksumK %= 11;
            withChecksums.Append (checksumK.ToInvariantString());
        }

        builder.Append (_patterns[11]); // start/stop
        builder.Append ('0'); // space

        foreach (var c in withChecksums.ReturnShared())
        {
            var d = c == '-' ? 10 : c - '0';
            builder.Append (_patterns[d]);
            builder.Append ('0'); // space
        }

        builder.Append (_patterns[11]); // start/stop

        return builder.ReturnShared();
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
            if (!Alphabet.Contains (c))
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc cref="IBarcode.Symbology"/>
    public override string Symbology => "Code11";

    #endregion
}
