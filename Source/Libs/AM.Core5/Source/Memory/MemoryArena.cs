// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MemoryArena.cs -- простой распределитель памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Memory
{
    /// <summary>
    /// Простой распределитель памяти для типов, не требующих очистки.
    /// </summary>
    public sealed class MemoryArena
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="size">Общий размер арены в байтах.</param>
        /// <param name="alignment">Выравнивание. По умолчанию 8 байт.</param>
        public MemoryArena
            (
                int size = 128 * 1024,
                int alignment = 8
            )
        {
            _memory = new Memory<byte>(new byte[size]);
            _alingnemnt = alignment - 1;
            _offset = 0;
        } // constructor

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="memory">Непрерывный блок памяти любого происхождения
        /// (в т. ч. unmanaged).</param>
        /// <param name="alingnemnt">Выравнивание. По умолчанию 8 байт.</param>
        /// <remarks>
        /// Конструктор сам устанавливает правильное выравнивание,
        /// так что можно передавать в него невыравненную память.
        /// </remarks>
        public unsafe MemoryArena
            (
                Memory<byte> memory,
                int alingnemnt = 8
            )
        {
            _alingnemnt = alingnemnt - 1;
            _memory = memory;

            fixed (byte* ptr = memory.Span)
            {
                var border = new IntPtr(ptr).ToInt64();
                var begin = unchecked((border + _alingnemnt) & ~_alingnemnt);
                _offset = unchecked((int)(begin - border));
            }
        } // constructor

        #endregion

        #region Private members

        private readonly Memory<byte> _memory;
        private readonly int _alingnemnt;
        private int _offset;

        #endregion

        #region Public methods

        /// <summary>
        /// Возврат в исходное состояние.
        /// </summary>
        public void Reset()
        {
            _offset = 0;
        } // method Reset

        /// <summary>
        /// Выделение куска памяти.
        /// </summary>
        /// <param name="size">Размер в байтах.</param>
        /// <returns>Выделенная память.</returns>
        /// <exception cref="ApplicationException">При нехватке места на арене.
        /// </exception>
        public Memory<byte> Allocate
            (
                int size
            )
        {
            if (_offset + size > _memory.Length)
            {
                throw new ApplicationException();
            }

            var result = _memory.Slice(_offset, size);
            _offset = (_offset + size + _alingnemnt) & ~_alingnemnt;

            return result;
        } // method Allocate

        /// <summary>
        /// Создание структуры.
        /// </summary>
        /// <typeparam name="T">Тип структуры.</typeparam>
        /// <returns>Ссылка на созданную структуру.</returns>
        public ref T Allocate<T>()
            where T: struct
        {
            var span = Allocate(Marshal.SizeOf<T>()).Span;

            return ref MemoryMarshal.AsRef<T>(span);
        } // method Allocate<T>

        #endregion

    } // class MemoryArena

} // namespace AM.Memory
