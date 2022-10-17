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

/* UnmanagedMemoryManager.cs -- простой менеджер для неуправляемой памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Memory;

/// <summary>
/// Простой менеджер для неуправляемой памяти.
/// </summary>
/// <typeparam name="T">Неуправляемый тип, например, <c>byte</c></typeparam>
public sealed unsafe class UnmanagedMemoryManager<T>
    : MemoryManager<T>
    where T : unmanaged
{
    #region Private members

    private readonly T* _pointer;
    private readonly int _length;
    private readonly IntPtr _handle;

    #endregion

    #region Construction

    /// <summary>
    /// Создание менеджера из указанного диапазона памяти.
    /// </summary>
    ///
    /// <remarks>Предполагается, что предоставленный диапазон
    /// уже неуправляемый или закреплен извне.</remarks>
    public UnmanagedMemoryManager
        (
            Span<T> span
        )
    {
        fixed (T* ptr = &MemoryMarshal.GetReference (span))
        {
            _pointer = ptr;
            _length = span.Length;
        }

        _handle = IntPtr.Zero;
    }

    /// <summary>
    /// Создание менеджера из указателя и длины блока.
    /// </summary>
    ///
    /// <param name="pointer">Указатель на начало блока.</param>
    /// <param name="length">Длина блока в элементах (а не байтах!).</param>
    public UnmanagedMemoryManager
        (
            T* pointer,
            int length
        )
    {
        Sure.NonNegative (length);

        _pointer = pointer;
        _length = length;
        _handle = IntPtr.Zero;
    }

    /// <summary>
    /// Создание менеджера для указанного дескриптора неуправвляемой
    /// памяти, полученного от <c>Marshal.AllocHGlobal</c>.
    /// </summary>
    ///
    /// <param name="handle">Дескриптор неуправляемой памяти, полученный от
    /// <see cref="Marshal.AllocHGlobal(int)"/>.</param>
    /// <param name="length">Длина блока в элементах (а не байтах!).</param>
    ///
    /// <remarks>
    /// При очистке менеджер сам освободит <paramref name="handle"/>.
    /// </remarks>
    public UnmanagedMemoryManager
        (
            IntPtr handle,
            int length
        )
    {
        Sure.NonNegative (length);

        _handle = handle;
        _pointer = (T*)_handle.ToPointer();
        _length = length;
        Unsafe.InitBlock (_pointer, 0, unchecked ((uint)_length));
    }

    /// <summary>
    /// Создание менеджера для неуправляемой памяти, полученной
    /// от <c>Marshal.AllocHGlobal.</c>
    /// </summary>
    ///
    /// <param name="length">Длина блока в элементах.</param>
    public UnmanagedMemoryManager
        (
            int length
        )
        : this (Marshal.AllocHGlobal (length), length)
    {
        // пустое тело конструктора
    }

    #endregion

    #region MemoryManager<T> members

    /// <inheritdoc cref="MemoryManager{T}.GetSpan"/>
    public override Span<T> GetSpan()
    {
        return new (_pointer, _length);
    }

    /// <inheritdoc cref="MemoryManager{T}.Pin"/>
    public override MemoryHandle Pin
        (
            int elementIndex = 0
        )
    {
        Sure.InRange (elementIndex, 0, _length);

        return new MemoryHandle (_pointer + elementIndex);
    }

    /// <inheritdoc cref="MemoryManager{T}.Unpin"/>
    public override void Unpin()
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="MemoryManager{T}.Dispose"/>
    protected override void Dispose (bool disposing)
    {
        if (_handle != IntPtr.Zero)
        {
            Marshal.FreeHGlobal (_handle);
        }
    }

    #endregion
}
