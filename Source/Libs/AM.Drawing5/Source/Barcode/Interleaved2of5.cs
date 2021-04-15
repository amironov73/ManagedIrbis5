// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Interleaved2of5.cs -- чередующийся штрих-код "2 из 5"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Linq;

#endregion

#nullable enable

namespace AM.Drawing.Barcodes
{
    /// <summary>
    /// Чередующийся штрих-код "2 из 5".
    /// </summary>
    public class Interleaved2of5
        : IBarcode
    {
        //
        // как кодируется рисование полос
        //
        // A = thin space
        // B = thick space
        // X = thin bar
        // Y = thick bar
        //

        #region Nested classes

        sealed class Painter
            : IDisposable
        {
            public Graphics? Graphics;
            public Brush? Fore;
            public Brush? Back;
            public float Weight;
            public float Position;
            public RectangleF Bounds;

            public bool Draw(char what)
            {
                Brush? brush;
                float width;

                if (Position >= Bounds.Right)
                {
                    return false;
                }

                switch (what)
                {
                    case 'A':
                        brush = Back;
                        width = 1;
                        break;

                    case 'B':
                        brush = Back;
                        width = 3;
                        break;

                    case 'X':
                        brush = Fore;
                        width = 1;
                        break;

                    case 'Y':
                        brush = Fore;
                        width = 3;
                        break;

                    default:
                        return false;
                }

                width *= Weight;
                var rectangle = new RectangleF
                    (
                        Position,
                        Bounds.Top,
                        width,
                        Bounds.Height
                    );
                Graphics?.FillRectangle(brush.ThrowIfNull("brush"), rectangle);
                Position += width;

                return true;
            }

            public void Dispose()
            {
                Fore?.Dispose();
                Back?.Dispose();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Множитель для ширины полос.
        /// </summary>
        public float Weight { get; set; } = 3.0f;

        #endregion

        #region Private members

        // паттерны для отображения цифр
        // в виде последовательности широких и узких полос
        // 2 из 5 полос широкие, остальные узкие
        // 0 = узкая, 1 = широкая
        private static readonly byte[][] _patterns =
        {
            new byte[] { 0, 0, 1, 1, 0 }, // 0
            new byte[] { 1, 0, 0, 0, 1 }, // 1
            new byte[] { 0, 1, 0, 0, 1 }, // 2
            new byte[] { 1, 1, 0, 0, 0 }, // 3
            new byte[] { 0, 0, 1, 0, 1 }, // 4
            new byte[] { 1, 0, 1, 0, 0 }, // 5
            new byte[] { 0, 1, 1, 0, 0 }, // 6
            new byte[] { 0, 0, 0, 1, 1 }, // 7
            new byte[] { 1, 0, 0, 1, 0 }, // 8
            new byte[] { 0, 1, 0, 1, 0 }, // 9
        };

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

            var result = message.Length % 2 == 0 && message.All(char.IsDigit);

            return result;
        }

        #endregion

        #region IBarcode members

        /// <inheritdoc cref="IBarcode.Symbology"/>
        public string Symbology { get; } = "Interleaved 2 of 5";

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

            var message = data.Message.ThrowIfNull("data.Message");
            using var painter = new Painter
            {
                Graphics = context.Graphics.ThrowIfNull("context.Graphics"),
                Fore = new SolidBrush(Color.Black),
                Back = new SolidBrush(Color.White),
                Bounds = context.Bounds,
                Weight = Weight,
                Position = context.Bounds.Left
            };

            // стартовый паттерн
            var success = painter.Draw('X')
                          && painter.Draw('A')
                          && painter.Draw('X')
                          && painter.Draw('A');
            if (!success)
            {
                return;
            }

            for (var i = 0; i < message.Length; i += 2)
            {
                var symbol1 = message[i] - '0';
                var symbol2 = message[i + 1] - '0';
                var pattern1 = _patterns[symbol1];
                var pattern2 = _patterns[symbol2];
                for (var j = 0; j < 5; j++)
                {
                    if (!painter.Draw(pattern1[j] == 0 ? 'X' : 'Y')
                        || !painter.Draw(pattern2[j] == 0 ? 'A' : 'B'))
                    {
                        return;
                    }
                }
            }

            // завершающий паттерн
            if (painter.Draw('Y'))
            {
                if (painter.Draw('A'))
                {
                    painter.Draw('X');
                }
            }
        }

        #endregion

    } // class Interleaved2of5

} // namespace AM.Drawing.Barcodes
