// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Vector2Slim.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Skia.QrCoding;

/// <summary>
/// int version of Vector2, slim implementation
/// </summary>
/// <remarks>
/// ref: https://github.com/dotnet/corefx/blob/master/src/System.Numerics.Vectors/src/System/Numerics/Vector2_Intrinsics.cs
/// </remarks>
public struct Vector2Slim
{
    /// <summary>
    /// The X component of the vector.
    /// </summary>
    public int X;

    /// <summary>
    /// The Y component of the vector.
    /// </summary>
    public int Y;

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public Vector2Slim
        (
            int value
        )
        : this (value, value)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Vector2Slim
        (
            int x,
            int y
        )
    {
        X = x;
        Y = y;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="array"></param>
    public void CopyTo (int[] array) => CopyTo (array, 0);

    /// <summary>
    ///
    /// </summary>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public void CopyTo (int[] array, int index)
    {
        if (array == null)
        {
            throw new ArgumentNullException (nameof (array));
        }

        if (index < 0 || index >= array.Length)
        {
            throw new ArgumentOutOfRangeException (nameof (index));
        }

        if (array.Length - index < 2)
        {
            throw new ArgumentException ($"{index} is greater thean destination.");
        }

        array[index] = X;
        array[index + 1] = Y;
    }

    /// <inheritdoc cref="Equals"/>
    public bool Equals (Vector2Slim other) => X == other.X && Y == other.Y;

    #region statics

    /// <summary>
    /// Returns the vector (0,0).
    /// </summary>
    public static Vector2Slim Zero => new ();

    /// <summary>
    /// Returns the vector (1,1).
    /// </summary>
    public static Vector2Slim One => new (1, 1);

    /// <summary>
    /// Returns the vector (1,0).
    /// </summary>
    public static Vector2Slim UnitX => new (1, 0);

    /// <summary>
    /// Returns the vector (0,1).
    /// </summary>
    public static Vector2Slim UnitY => new (0, 1);

    #endregion
}
