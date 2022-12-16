// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MappedSlice.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Utils;

/// <summary>
/// Provides a mapped view of an underlying slice array, selecting arbitrary indicies
/// from the source array
/// </summary>
/// <typeparam name="T">The element type of the underlying array</typeparam>
public struct MappedSlice<T>
{
    /// <summary>
    /// Constructs a new mapped array
    /// </summary>
    /// <param name="data">The data to be mapped</param>
    /// <param name="mapping">The index map</param>
    public MappedSlice (Slice<T> data, Slice<int> mapping)
    {
        _data = data;
        _mapping = mapping;
    }

    private Slice<T> _data;
    private Slice<int> _mapping;

    /// <summary>
    /// Get the underlying slice for this mapped array
    /// </summary>
    public Slice<T> Underlying => _data;

    /// <summary>
    /// Get the index mapping for this mapped array
    /// </summary>
    public Slice<int> Mapping => _mapping;

    /// <summary>
    /// Gets the number of elements in this mapping
    /// </summary>
    public int Length => _mapping.Length;

    /// <summary>
    /// Gets a reference to a mapped element
    /// </summary>
    /// <param name="index">The mapped index to be retrieved</param>
    /// <returns>A reference to the element</returns>
    public ref T this [int index]
    {
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        get => ref _data[_mapping[index]];
    }

    /// <summary>
    /// Get the content of this mapped slice as an array
    /// </summary>
    /// <returns>The content as an array</returns>
    public T[] ToArray()
    {
        var arr = new T[Length];
        for (var i = 0; i < Length; i++)
        {
            arr[i] = this[i];
        }

        return arr;
    }
}
