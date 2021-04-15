// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Code93.cs -- штрихкод, поддерживающий A-Z, 0-9 и некоторые спецсимволы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#endregion

#nullable enable

namespace AM.Drawing.Barcodes
{
    //
    // https://www.cognex.com/ru-ru/resources/symbologies/1-d-linear-barcodes/code-93-barcodes
    //
    // Штрихкод стандарта Code 93 является обновленной, более защищенной
    // и компактной версией штрихкода стандарта Code 39, который содержит
    // как буквы, так и цифры. Данный штрихкод используется в сферах
    // военной промышленности и автомобилестроения, а также почтой Канады
    // для кодирования специальной информации об отправлениях.
    //
    //  Как и Code 39, штрихкод стандарта Code 93 имеет начальный и конечный
    // символы, которые не могут быть выражены в виде обыкновенных символов
    // ASCII, а вместо этого, как правило, обозначены «*». После начального
    // символа находятся кодированные данные. Как и в случае с кодом стандарта
    // Code 39, каждая буква представлена в виде определенного числового значения.
    // После кодированных данных находится контрольный знак, состоящий из двух
    // символов, который обеспечивает точность данных в случае их ввода ручным
    // способом. Два символа называются «Modulo-47 Контрольный знак C» и
    // «Modulo-47 Контрольный знак К». Определенное комбинирование чисел
    // в штрихкоде дает остаток, а соответствующая буква или цифра преобразуется
    // в «Контрольный знак C» или «Контрольный знак K». После контрольной цифры
    // находится остановочный символ с последующим завершающим штрихом, который
    // указывает на конец штрихкода.
    //
    // Преимущества: Штрихкод стандарта 93, в отличие от Code 39, является более
    // коротким и эффективным, и обладает большим резервом данных, что обеспечивает
    // высокий уровень защиты. Данный штрихкод также содержит 5 специальных символов,
    // которых не имеет штрихкод стандарта Code 39.
    //
    // Недостатки: В отличие от штрихкода стандарта Code 39 в спецификации Code 93
    // использование контрольного знака является обязательным, поскольку данный
    // штрихкод не содержит самоконтроля знака.
    //

    //
    // https://en.wikipedia.org/wiki/Code_93
    //
    // Code 93 is a barcode symbology designed in 1982 by Intermec to provide
    // a higher density and data security enhancement to Code 39. It is an alphanumeric,
    // variable length symbology. Code 93 is used primarily by Canada Post to encode
    // supplementary delivery information. Every symbol includes two check characters.
    //
    // Each Code 93 character is nine modules wide, and always has three bars and
    // three spaces, thus the name. Each bar and space is from 1 to 4 modules wide.
    // (For comparison, a Code 39 character consists of five bars and four spaces,
    // three of which are wide, for a total width of 13–16 modules.)
    //
    // Code 93 is designed to encode the same 26 upper case letters, 10 digits and
    // 7 special characters as code 39:
    //
    // A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
    // 0 1 2 3 4 5 6 7 8 9
    // - . $ / + % SPACE
    //
    // In addition to 43 characters, Code 93 defines 5 special characters (including
    // a start/stop character), which can be combined with other characters to unambiguously
    // represent all 128 ASCII characters.
    //
    // In an open system, the minimum value of X dimension is 7.5 mils (0.19 mm).
    // The minimum bar height is 15 percent of the symbol length or 0.25 inches (6.4 mm),
    // whichever is greater. The starting and trailing quiet zone should be at least
    // 0.25 inches (6.4 mm).
    //

    /// <summary>
    /// Штрихкод, поддерживающий A-Z, 0-9 и некоторые спецсимволы.
    /// </summary>
    public class Code93
        : IBarcode
    {
        #region Properties

        /// <summary>
        /// Множитель для ширины полос.
        /// </summary>
        public float Weight { get; set; } = 3.0f;

        #endregion

        #region Public methods

        public static string Encode
            (
                string text
            )
        {
            var result = new List<char>();

            return new string(result.ToArray());
        }

        /// <summary>
        /// Проверка, пригодны ли данные для штрих-кода.
        /// </summary>
        public bool Verify
            (
                BarcodeData data
            )
        {
            var message = data.Message;

            if (string.IsNullOrWhiteSpace(message))
            {
                return false;
            }


            return true;
        }

        #endregion

        #region IBarcode members

        /// <inheritdoc cref="IBarcode.Symbology"/>
        public string Symbology { get; } = "Code 93";

        /// <inheritdoc cref="IBarcode.DrawBarcode"/>
        public void DrawBarcode
            (
                BarcodeContext context
            )
        {
            var data = context.Data;
            if (data is null || !Verify(data))
            {
                return;
            }

            var encoded = Encode(data.Message.ThrowIfNull("data.Message"));
            encoded = "00" + encoded + "00";
            var graphics = context.Graphics.ThrowIfNull("context.Graphics");
            var bounds = context.Bounds;
            using var fore = new SolidBrush(Color.Black);
            using var back = new SolidBrush(Color.White);
            var position = bounds.Left;

            foreach (var c in encoded)
            {
                var rect = new RectangleF(position, bounds.Top, Weight, bounds.Height);
                var brush = c == '0' ? back : fore;
                graphics.FillRectangle(brush, rect);
                position += Weight;
            }

        }

        #endregion
    }
}
