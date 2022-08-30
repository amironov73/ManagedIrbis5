// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* Codabar.cs -- штриховой код, позволяющий кодировать числа от 0 до 9, символы -, $, :, /, ., + и четыре буквы (A, B, C, D)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace AM.Drawing.Barcodes;

//
// https://ru.wikipedia.org/wiki/Codabar
//
// Codabar - штриховой код, позволяющий кодировать
// числа от 0 до 9, символы -, $, :, /, ., +
// и четыре буквы (A, B, C, D). Штрихкод CODABAR,
// в зависимости от спецификации, позволяет закодировать
// только цифры (от 0 до 9), и в некоторых вариантах шесть
// спецсимволов (-, $, :, /, ., +). Четыре буквы (A, B, C, D)
// используются как стартовый и стоповые биты и не выводятся
// при дешифровании. Каждый символ содержит 7 элементов
// (4 штриха и 3 пробела).
//
// Для кодирования символа используются
// два или три широких элемента и четыре или пять узких.
// Расстояние между символами (пробелы) не содержит информации.
// Коэффициент пропорциональности (N) - отношение ширины
// узкого элемента к ширине широкого N= от 1: 2,25 до 1:3.
//
// Из преимуществ можно выделить возможность кодирования
// 6 специальных символов. Пробелы между символами
// не содержат информации.
//
// К недостаткам относится низкое распределение информации
// на единицу площади. Пример: 5,5 мм на символ с минимальной
// шириной штриха X=0,3 мм и пропорцией кода N=1:3.
//
// Рекомендуемые методы печати: офсетная, лазерная и матричная,
// гравировка, флексография, термо- термотрансферная печать,
// фотопечать.
//
// Применяется на складах, транспорте, в логистике и др.
//

/// <summary>
/// Штриховой код, позволяющий кодировать числа от 0 до 9,
/// символы -, $, :, /, ., + и четыре буквы (A, B, C, D).
/// </summary>
public sealed class Codabar
    : LinearBarcodeBase
{
    #region Private members

    private static readonly Dictionary<char, string> _patterns = new ()
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
        Sure.NotNull (data);

        var text = data.Message.ThrowIfNull();
        var builder = StringBuilderPool.Shared.Get();

        foreach (var c in text)
        {
            builder.Append (_patterns[c]);
            builder.Append ('0'); // межсимвольный разделитель
        }

        // убираем последний межсимвольный разделитель
        builder.Remove (builder.Length - 1, 1);

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

        if (message.Length < 2)
        {
            return false;
        }

        var c1 = char.ToUpperInvariant (message[0]);
        if (c1 != 'A' && c1 != 'B' && c1 != 'C' && c1 != 'D')
        {
            return false;
        }

        var c2 = char.ToUpperInvariant (message[^1]);
        if (c2 != 'A' && c2 != 'B' && c2 != 'C' && c2 != 'D')
        {
            return false;
        }

        foreach (var c in message)
        {
            if (!_patterns.ContainsKey (c))
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc cref="IBarcode.Symbology"/>
    public override string Symbology => "Codabar";

    #endregion
}
