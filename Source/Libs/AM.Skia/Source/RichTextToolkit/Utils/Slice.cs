// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Slice.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Utils;

/// <summary>
/// Represents a slice of an array
/// </summary>
/// <typeparam name="T">The array type</typeparam>
[DebuggerDisplay ("Length = {Length}")]
public struct Slice<T>
    : IEnumerable<T>
{
    /// <summary>
    /// Constructs a new slice covering the entire passed array.
    /// </summary>
    /// <param name="array"></param>
    public Slice (T[] array)
        : this (array, 0, array.Length)
    {
    }

    /// <summary>
    /// Constructs a new slice for part of the passed array.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="start"></param>
    /// <param name="length"></param>
    public Slice (T[] array, int start, int length)
    {
        if (start < 0 || start + length > array.Length)
        {
            throw new ArgumentOutOfRangeException (
                $"Invalid sub-slice range ({start},{length}) with array length of {array.Length}");
        }

        _array = array;
        _start = start;
        _length = length;
    }

    /// <summary>
    /// Gets the length of the array slice.
    /// </summary>
    public int Length
    {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        get => _length;
    }

    /// <summary>
    /// Clears the entire slice content
    /// </summary>
    public void Clear()
    {
        Array.Clear (_array, _start, _length);
    }

    /// <summary>
    /// Fill the slice with a specified value
    /// </summary>
    /// <param name="value"></param>
    public void Fill (T value)
    {
        for (var i = 0; i < _length; i++)
        {
            _array[i + _start] = value;
        }
    }

    /// <summary>
    /// Copy data from another slice into this one
    /// </summary>
    /// <param name="Source">The source data</param>
    public void Set (Slice<T> Source)
    {
        if (Source.Length != Length)
        {
            throw new ArgumentException ("Slices must have the same length");
        }

        Array.Copy (Source.Underlying, Source.Start, Underlying, Start, Source.Length);
    }

    /// <summary>
    /// Gets a reference to an element in the slice
    /// </summary>
    /// <param name="index">The element index</param>
    /// <returns>A reference to the element value.</returns>
    public ref T this [int index]
    {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index < 0 || index >= _length)
            {
                throw new ArgumentOutOfRangeException (nameof (index));
            }

            return ref _array[_start + index];
        }
    }

    [DebuggerBrowsable (DebuggerBrowsableState.Never)]
    private T[] _array;

    [DebuggerBrowsable (DebuggerBrowsableState.Never)]
    private int _start;

    [DebuggerBrowsable (DebuggerBrowsableState.Never)]
    private int _length;

    /// <summary>
    /// Creates a sub-slice of this slice
    /// </summary>
    /// <param name="start">The slice start index</param>
    /// <param name="length">The slice length</param>
    /// <returns>A new array slice</returns>
    public Slice<T> SubSlice (int start, int length)
    {
        return new Slice<T> (_array, _start + start, length);
    }

    /// <summary>
    /// Creates a subslice of an array slice, from a specified position to the end
    /// </summary>
    /// <param name="start">The slice start index</param>
    /// <returns>A new array slice</returns>
    public Slice<T> SubSlice (int start)
    {
        return SubSlice (start, Length - start);
    }

    /// <summary>
    /// Gets the slice contents as a new array
    /// </summary>
    /// <returns></returns>
    public T[] ToArray()
    {
        var array = new T[_length];
        Array.Copy (_array, _start, array, 0, _length);
        return array;
    }

    /// <summary>
    /// Creates a copy of this slice on a new underlying array
    /// </summary>
    /// <returns>A slice representing the copy</returns>
    public Slice<T> Copy()
    {
        return new Slice<T> (ToArray());
    }

    /// <summary>
    /// Gets the underlying array
    /// </summary>
    public T[] Underlying => _array;

    /// <summary>
    /// Gets the offset of this slice within the underlying array
    /// </summary>
    public int Start => _start;

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return new ArraySliceEnumerator<T> (_array, _start, _length);
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return new ArraySliceEnumerator<T> (_array, _start, _length);
    }

    /// <summary>
    /// Split this slice on a delimiter
    /// </summary>
    /// <param name="delimiter">The delimiter</param>
    /// <returns>An enumeration of slices</returns>
    public IEnumerable<Slice<T>> Split (T delimiter)
    {
        var start = 0;
        for (var i = 0; i < Length; i++)
        {
            if (this[i]!.Equals (delimiter))
            {
                yield return SubSlice (start, i - start);
                start = i + 1;
            }
        }

        yield return SubSlice (start, Length - start);
    }

    /// <summary>
    /// Find the first index of a specified value
    /// </summary>
    /// <param name="value">The value to search for</param>
    /// <returns>The index of the first occurance, or -1 if not found</returns>
    public int IndexOf (T value)
    {
        for (var i = 0; i < Length; i++)
        {
            if (this[i]!.Equals (value))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Find the first index of one or more valus
    /// </summary>
    /// <param name="values">The value to search for</param>
    /// <returns>The index of the first occurance, of -1 if not found</returns>
    public int IndexOfAny (params T[] values)
    {
        for (var i = 0; i < Length; i++)
        {
            if (values.Contains (this[i]))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Replace all instances of a value with another
    /// </summary>
    /// <param name="replace">The value to replace</param>
    /// <param name="with">The replacement value</param>
    public void Replace (T replace, T with)
    {
        for (var i = 0; i < Length; i++)
        {
            if (this[i]!.Equals (replace))
            {
                this[i] = with;
            }
        }
    }


    /// <summary>
    /// A shared empty slice of type T
    /// </summary>
    public static Slice<T> Empty => new ();

    /// <summary>
    /// Get the slice as a Span
    /// </summary>
    /// <returns></returns>
    public Span<T> AsSpan() => new (_array, _start, _length);
}
