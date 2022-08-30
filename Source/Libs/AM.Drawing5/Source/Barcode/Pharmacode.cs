// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* Pharmacode.cs -- фармацевтический двоичный код
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Drawing.Barcodes;

//
// https://ru.wikipedia.org/wiki/%D0%A4%D0%B0%D1%80%D0%BC%D0%B0%D0%BA%D0%BE%D0%B4
//
// Фармакод может представляться только одним целым числом
// от 1 до 131 070. В отличие от других широко используемых линейных
// штриховых кодов, фармакод не хранит данные в виде, соответствующем
// возможности понимания человеком цифр, число кодируется в двоичном
// формате. Фармакод читается сканером справа налево и слева направо
// (если сканер всенаправленный): каждая узкая полоса обозначает ноль
// в двоичном коде и каждая широкая полоса обозначает единицу.
// Минимальная длина штрихкода — 1 узкая полоса
// и максимальная — 16 широких, так что наименьшее число, которое
// может быть закодировано — это 1 (1 узкая полоса)
// и самое большое — 131070 (16 широких полос).
// Фармакоды представляются цветами, которые доминируют на упаковке.
//
// Алгоритм кодирования
//
// 1. К числу, необходимому для кодирования, необходимо прибавить
//    единицу (пример: 69 + 1 = 70)
// 2. Необходимо перевести число в двоичный формат (70 => 1000110)
// 3. «Вычеркнуть» единицу, стоящую впереди всех остальных
//    чисел (1000110 → 000110)
// 4. Преобразовать число в линейный код, заменяя нули узкими полосками,
//    а единицы — широкими (000110 → IIIHHI)
//
// Алгоритм декодирования
//
// 1. Заменить узкие и широкие полоски числами (IHHIIHI → 0110010)
// 2. Поставить единицу спереди этого числа (10110010)
// 3. Выполнить перевод этого двоичного числа в десятичное
//    (10110010 → 178)
// 4. Вычесть из полученного числа единицу (178 — 1 = 177)
//

/// <summary>
/// Фармако́д, также известный как Фармацевтический двоичный код,
/// является стандартом штрихового кода, используемый
/// в фармацевтической промышленности в качестве системы контроля
/// упаковок. Он может быть читаемым, даже несмотря на ошибки
/// при печати. Фармакоды могут быть напечатаны в нескольких цветах,
/// чтобы убедиться, что оставшаяся часть упаковки (которую
/// фармацевтическая компания должна печатать, чтобы защитить
/// себя от юридической ответственности) правильно напечатана.
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
        Sure.NotNull (data);

        var text = data.Message.ThrowIfNull();
        var builder = StringBuilderPool.Shared.Get();
        var number = text.ParseInt32();

        do
        {
            if ((number & 1) == 0)
            {
                builder.Insert (0, Thick);
                number = (number - 2) / 2;
            }
            else
            {
                builder.Insert (0, Thin);
                number = (number - 1) / 2;
            }

            if (number != 0)
            {
                builder.Insert (0, Gap);
            }
        } while (number != 0);

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

        var number = message.ParseInt32();

        return number is > 2 and < 131071;
    }

    /// <inheritdoc cref="IBarcode.Symbology"/>
    public override string Symbology { get; } = "Pharmacode";

    #endregion
}
