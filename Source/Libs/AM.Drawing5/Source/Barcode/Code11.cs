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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#endregion

#nullable enable

namespace AM.Drawing.Barcodes
{
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
    public class Code11
        : IBarcode
    {
        #region Constants

        /// <summary>
        /// Разрешенные символы.
        /// </summary>
        public const string Alphabet = "0123456789-";

        #endregion

        #region Properties

        /// <summary>
        /// Множитель для ширины полос.
        /// </summary>
        public float Weight { get; set; } = 3.0f;

        #endregion

        #region Private members

        // паттерны для отображения символов
        private static readonly string[] _patterns =
        {
            "101011",  // 0
            "1101011", // 1
            "1001011", // 2
            "1100101", // 3
            "1011011", // 4
            "1101101", // 5
            "1001101", // 6
            "1010011", // 7
            "1101001", // 8
            "110101",  // 9
            "101101",  // -
            "1011001"  // start/stop
        };

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encode
            (
                string text
            )
        {
            var result = new List<char>();
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

            var withChecksums = new List<char>(text.Length);
            withChecksums.AddRange(text);
            checksumC %= 11;
            withChecksums.AddRange(checksumC.ToInvariantString());

            // рекомендуется вычислять контрольную сумму K
            // для сообщений длиной >= 10
            if (text.Length >= 10)
            {
                weight = 1;
                var checksumK = 0;

                for (var i = withChecksums.Count; i >= 0; i--)
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
                withChecksums.AddRange(checksumK.ToInvariantString());
            }

            result.AddRange(_patterns[11]); // start/stop
            result.Add('0'); // space

            foreach (var c in withChecksums)
            {
                var d = c == '-' ? 10 : c - '0';
                result.AddRange(_patterns[d]);
                result.Add('0'); // space
            }

            result.AddRange(_patterns[11]); // start/stop

            return new string(result.ToArray());
        }

        #endregion

        #region Public methods

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

            foreach (var c in message)
            {
                if (!Alphabet.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region IBarcode members

        /// <inheritdoc cref="IBarcode.Symbology"/>
        public string Symbology { get; } = "Code11";

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
            using var fore = new SolidBrush(data.ForeColor);
            using var back = new SolidBrush(data.BackColor);
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
