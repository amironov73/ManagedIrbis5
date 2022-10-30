// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ValueFactory.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AeroSuite.AnimationEngine;

/// <summary>
/// A method that provides corresponding values of a specified type for a progress value.
/// </summary>
/// <typeparam name="T">The type.</typeparam>
/// <param name="startValue">The start value.</param>
/// <param name="targetValue">The target value.</param>
/// <param name="progress">The progress.</param>
/// <returns>A value corresponding to the progress.</returns>
public delegate T ValueFactory<T>
    (
        T startValue,
        T targetValue,
        double progress
    );
