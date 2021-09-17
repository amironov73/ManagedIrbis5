// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* ValueBuffer.cs -- буфер для чтения из потока, располагающийся на стеке
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Threading.Tasks;
using AM.Memory;

#endregion

#nullable enable

namespace AM.IO
{
    /// <summary>
    /// Буфер для чтения из потока, располагающийся на стеке.
    /// </summary>
    public ref struct ValueBuffer
    {
        #region Properties

        /// <summary>
        /// Прочитанные данные.
        /// </summary>
        public ReadOnlyMemory<byte> Data { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public unsafe ValueBuffer
            (
                byte *bytes,
                int size
            )
        {
            Data = default;
            var manager = new UnmanagedMemoryManager<byte> (bytes, size);
            _memory = manager.Memory;

        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ValueBuffer
            (
                Memory<byte> memory = default
            )
            : this()
        {
            Data = default;
            _memory = memory;

        } // constructor

        #endregion

        #region Private members

        /// <summary>
        /// Указатель на блок памяти.
        /// </summary>
        private Memory<byte> _memory;

        /// <summary>
        /// Округление числа вверх до ближайшей степени двойки.
        /// </summary>
        static int RoundUp
            (
                int size
            )
        {
            var result = 8;

            if (size < 8)
            {
                size = 8;
            }

            while (result < size)
            {
                result *= 2;
            }

            return result;

        } // method RoundUp

        #endregion

        #region Public methods

        /// <summary>
        /// Чтение из потока.
        /// </summary>
        public int Read
            (
                Stream stream,
                int amount
            )
        {
            if (amount > _memory.Length)
            {
                var newSize = RoundUp (amount);
                _memory = new byte [newSize];
            }

            var result = stream.Read (_memory.Span.Slice (0, amount));
            Data = _memory.Slice (0, result);

            return result;

        } // method Read

        /// <summary>
        /// Считывание вплоть до разделителя строк или конца потока.
        /// </summary>
        public int ReadLine
            (
                Stream stream
            )
        {
            var counter = 0; // количество записанных в буфер байт
            while (true)
            {
                var chr = stream.ReadByte();
                if (chr < 0)
                {
                    if (counter == 0)
                    {
                        // достигнут конец потока,
                        // ни одного байта прочитать не удалось

                        Data = default;
                        return chr;
                    }

                    // если удалось прочитать хотя бы один байт

                    break;
                }

                if (chr == '\n')
                {
                    break;
                }

                if (chr != '\r')
                {
                    if (counter == _memory.Length)
                    {
                        var newSize = RoundUp (_memory.Length + 1);
                        var newMemory = new byte [newSize];
                        _memory.CopyTo (newMemory);
                        _memory = newMemory;
                    }

                    _memory.Span [counter] = (byte)chr;
                    ++counter;

                } // if

            } // while true

            Data = _memory.Slice (0, counter);

            return counter;

        } // method ReadLine

        #endregion

    } // struct ValueBuffer

} // namespace AM.IO
