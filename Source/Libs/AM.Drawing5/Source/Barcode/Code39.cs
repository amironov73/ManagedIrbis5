// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Code39.cs -- штрихкод, поддерживающий A-Z, 0-9 и некоторые спецсимволы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text;

using AM.Text;

#endregion

#nullable enable

namespace AM.Drawing.Barcodes
{
    //
    // https://ru.wikipedia.org/wiki/Code_39
    //
    // Code 39 (также известный как «USS Code 39», «Code 3/9»,
    // «Code 3 of 9», «USD-3», «Alpha39») — штрихкодовое обозначение,
    // которым можно закодировать большие латинские буквы (от A до Z),
    // цифры (от 0 до 9) и некоторые специальные символы (например,
    // знак доллара '$').
    //
    // Технические требования к символике штрихового кода Code 39,
    // показатели символики, кодирование знаков данных, размеры,
    // алгоритмы декодирования, параметры применения и строки-префиксы
    // и идентификатора символики в России регламентируются
    // ГОСТ 30742-2001 (ИСО/МЭК 16388—99) «Автоматическая идентификация.
    // Кодирование штриховое. Спецификация символики Code 39 (Код 39)».
    //
    // Структура штрихкода Code 39 состоит из шести зон:
    //
    // * Белое (свободное) поле;
    // * Стартовый символ (Start);
    // * Кодированная информация;
    // * Проверочный символ (контрольная сумма) – необязательный параметр;
    // * Остановочный (Stop) символ;
    // * Белое (свободное) поле.
    // * Межзнаковый интервал (пробел) отделяет знаки в символе.
    //
    // Каждый символ состоит из 9 полосок, из которых 3 широкие
    // (одна белая и 2 чёрных). Ширина широкой полоски может составлять
    // от 2 до 3 ширин узкой. Стартовый и остановочный символы одинаковые,
    // кодируются также, как и символы и обозначаются значком "*" — звёздочка.
    //
    // Штрихкод стандарта Code 39, в отличие от Code 128, может
    // не содержать контрольного знака, что допускается соответствующим стандартом.
    //
    // В случаях, когда необходимо повысить надёжность кодируемых данных,
    // используется контрольный знак, который располагается между последним
    // знаком данных и знаком «Stop». Контрольный знак вычисляется как
    // сумма значений всех знаков символа по модулю 43.
    //
    // Так как по стандарту между символами находится пробел, ширина которого
    // не несёт информации, Code 39 может быть задан шрифтом в обычном
    // текстовом редакторе. В этом случае необходимо в начале и в конце
    // кодируемой последовательности добавить звёздочку "*".
    //

    /// <summary>
    /// Штрих-код, поддерживающий A-Z, 0-9 и некоторые спецсимволы
    /// (например, знак доллара <c>$</c>).
    /// </summary>
    public class Code39
        : LinearBarcodeBase
    {
        #region Private members

        // паттерны для отображения символов
        private static readonly Dictionary<char, string> _patterns = new ()
        {
            ['0'] = "101001101101",
            ['1'] = "110100101011",
            ['2'] = "101100101011",
            ['3'] = "110110010101",
            ['4'] = "101001101011",
            ['5'] = "110100110101",
            ['6'] = "101100110101",
            ['7'] = "101001011011",
            ['8'] = "110100101101",
            ['9'] = "101100101101",
            ['A'] = "110101001011",
            ['B'] = "101101001011",
            ['C'] = "110110100101",
            ['D'] = "101011001011",
            ['E'] = "110101100101",
            ['F'] = "101101100101",
            ['G'] = "101010011011",
            ['H'] = "110101001101",
            ['I'] = "101101001101",
            ['J'] = "101011001101",
            ['K'] = "110101010011",
            ['L'] = "101101010011",
            ['M'] = "110110101001",
            ['N'] = "101011010011",
            ['O'] = "110101101001",
            ['P'] = "101101101001",
            ['Q'] = "101010110011",
            ['R'] = "110101011001",
            ['S'] = "101101011001",
            ['T'] = "101011011001",
            ['U'] = "110010101011",
            ['V'] = "100110101011",
            ['W'] = "110011010101",
            ['X'] = "100101101011",
            ['Y'] = "110010110101",
            ['Z'] = "100110110101",
            ['-'] = "100101011011",
            ['.'] = "110010101101",
            [' '] = "100110101101",
            ['$'] = "100100100101",
            ['/'] = "100100101001",
            ['+'] = "100101001001",
            ['%'] = "101001001001",
            ['*'] = "100101101101",
        };

        // таблица трансляции для расширенного набора символов
        private static readonly Dictionary<char, string> _extended = new ()
        {
            [(char)0] = "%U",
            [(char)1] = "$A",
            [(char)2] = "$B",
            [(char)3] = "$C",
            [(char)4] = "$D",
            [(char)5] = "$E",
            [(char)6] = "$F",
            [(char)7] = "$G",
            [(char)8] = "$H",
            [(char)9] = "$I",
            [(char)10] = "$J",
            [(char)11] = "$K",
            [(char)12] = "$L",
            [(char)13] = "$M",
            [(char)14] = "$N",
            [(char)15] = "$O",
            [(char)16] = "$P",
            [(char)17] = "$Q",
            [(char)18] = "$R",
            [(char)19] = "$S",
            [(char)20] = "$T",
            [(char)21] = "$U",
            [(char)22] = "$V",
            [(char)23] = "$W",
            [(char)24] = "$X",
            [(char)25] = "$Y",
            [(char)26] = "$Z",
            [(char)27] = "%A",
            [(char)28] = "%B",
            [(char)29] = "%C",
            [(char)30] = "%D",
            [(char)31] = "%E",
            ['!'] = "/A",
            ['"'] = "/",
            ['#'] = "/C",
            ['$'] = "/D",
            ['%'] = "/E",
            ['&'] = "/F",
            ['\''] = "/G",
            ['('] = "/H",
            [')'] = "/I",
            ['*'] = "/J",
            ['+'] = "/K",
            [','] = "/L",
            ['/'] = "/O",
            [':'] = "/Z",
            [';'] = "%F",
            ['<'] = "%G",
            ['='] = "%H",
            ['>'] = "%I",
            ['?'] = "%J",
            ['['] = "%K",
            ['\\'] = "%",
            [']'] = "%M",
            ['^'] = "%N",
            ['_'] = "%O",
            ['{'] = "%P",
            ['|'] = "%Q",
            ['}'] = "%R",
            ['~'] = "%S",
            ['`'] = "%W",
            ['@'] = "%V",
            ['a'] = "+A",
            ['b'] = "+B",
            ['c'] = "+C",
            ['d'] = "+D",
            ['e'] = "+E",
            ['f'] = "+F",
            ['g'] = "+G",
            ['h'] = "+H",
            ['i'] = "+I",
            ['j'] = "+J",
            ['k'] = "+K",
            ['l'] = "+L",
            ['m'] = "+M",
            ['n'] = "+N",
            ['o'] = "+O",
            ['p'] = "+P",
            ['q'] = "+Q",
            ['r'] = "+R",
            ['s'] = "+S",
            ['t'] = "+T",
            ['u'] = "+U",
            ['v'] = "+V",
            ['w'] = "+W",
            ['x'] = "+X",
            ['y'] = "+Y",
            ['z'] = "+Z",
            [(char)127] = "%T",
        };

        private static void Append
            (
                StringBuilder encoded,
                char c
            )
        {
            if (_patterns.ContainsKey (c))
            {
                var pattern = _patterns[c];
                foreach (var pc in pattern)
                {
                    encoded.Append (pc);
                }
            }
            else if (_extended.ContainsKey (c))
            {
                var extended = _extended[c];
                foreach (var ec in extended)
                {
                    Append (encoded, ec);
                }
            }
            else
            {
                throw new ArsMagnaException();
            }

        } // method Append

        #endregion

        #region LinearBarcodeBase members

        /// <inheritdoc cref="LinearBarcodeBase.Encode"/>
        public override string Encode
            (
                BarcodeData data
            )
        {
            var builder = StringBuilderPool.Shared.Get();
            var text = data.Message.ThrowIfNull().Replace ("*", string.Empty);
            //text = "*" + text + ComputeChecksum(text) + "*";
            text = "*" + text + "*";

            foreach (var c in text)
            {
                Append (builder, c);
                builder.Append ('0');
            }

            builder.Remove (builder.Length - 1, 1);

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;

        } // method Encode

        // private static char ComputeChecksum (string text)
        // {
        //     var charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%";
        //     var sum = 0;
        //
        //     //Calculate the checksum
        //     foreach (var c in text)
        //     {
        //         sum = sum + charset.IndexOf (char.ToUpper (c));
        //     }
        //
        //     //return the checksum char
        //     return charset[sum % 43];
        // }

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
                if (!_patterns.ContainsKey (c)
                    && !_extended.ContainsKey (c))
                {
                    return false;
                }
            }

            return true;

        } // method Verify

        /// <inheritdoc cref="IBarcode.Symbology"/>
        public override string Symbology { get; } = "Code39";

        #endregion

    } // class Code39

} // namespace AM.Drawing.Barcodes
