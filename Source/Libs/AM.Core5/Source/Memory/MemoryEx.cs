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

/* MemoryEx.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Buffers;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Memory;

/// <summary>
/// Утилиты для работы с памятью.
/// </summary>
public static class MemoryEx
{
    #region Public methods

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int Length<T>
        (
            this IMemoryOwner<T> that
        )
    {
        return that.Memory.Length;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="that"></param>
    /// <param name="noDefaultOwner"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static CountdownMemoryOwner<T> AsCountdown<T>
        (
            this CountdownMemoryOwner<T> that,
            bool noDefaultOwner = false
        )
    {
        return Pool<CountdownMemoryOwner<T>>.Get().Init
            (
                that,
                0,
                that.Memory.Length,
                noDefaultOwner
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="that"></param>
    /// <param name="offset"></param>
    /// <param name="noDefaultOwner"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static CountdownMemoryOwner<T> AsCountdown<T>
        (
            this CountdownMemoryOwner<T> that,
            int offset,
            bool noDefaultOwner = false
        )
    {
        return Pool<CountdownMemoryOwner<T>>.Get().Init
            (
                that,
                offset,
                that.Memory.Length - offset,
                noDefaultOwner
            );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="that"></param>
    /// <param name="offset"></param>
    /// <param name="length"></param>
    /// <param name="noDefaultOwner"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static CountdownMemoryOwner<T> AsCountdown<T>
        (
            this CountdownMemoryOwner<T> that,
            int offset,
            int length,
            bool noDefaultOwner = false
        )
    {
        return Pool<CountdownMemoryOwner<T>>.Get().Init (that, offset, length, noDefaultOwner);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="that"></param>
    /// <param name="offset"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static IMemoryOwner<T> Slice<T>
        (
            this CountdownMemoryOwner<T> that,
            int offset
        )
    {
        return Slice (that, offset, that.Memory.Length - offset);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="that"></param>
    /// <param name="offset"></param>
    /// <param name="length"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static IMemoryOwner<T> Slice<T>
        (
            this CountdownMemoryOwner<T> that,
            int offset,
            int length
        )
    {
        return that.AsCountdown (offset, length);
    }

    #endregion
}
