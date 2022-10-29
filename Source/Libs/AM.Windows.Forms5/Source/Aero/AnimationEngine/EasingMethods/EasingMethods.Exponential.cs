// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* EasingMethods.Exponential.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AeroSuite.AnimationEngine;

public static partial class EasingMethods
{
    /// <summary>
    ///     <para>An easing method that accelerates from almost 0 to a velocity of approximately 7 (log 1024).</para>
    ///     <para>Function: f(p) = 2 ^ (10 * (p - 1))</para>
    ///     <para>Derivative: f'(p) = 2 ^ (10 * p - 9) * log(32)</para>
    /// </summary>
    /// <param name="progress">The time progress of the animation.</param>
    /// <returns>The value progress of the animation.</returns>
    public static double ExponentialEaseIn (double progress)
    {
        return progress <= 0 ? 0 : progress >= 1 ? 1 : Math.Pow (2, 10 * (progress - 1));
    }

    /// <summary>
    ///     <para>An easing method that decelerates from a velocity of approximately 7 (log 1024) to approximately 0.</para>
    ///     <para>Function: f(p) = -(2 ^ (-10 * p)) + 1</para>
    ///     <para>Derivative: f'(p) = 2 ^ (1 - 10 * p) * log(32)</para>
    /// </summary>
    /// <param name="progress">The time progress of the animation.</param>
    /// <returns>The value progress of the animation.</returns>
    public static double ExponentialEaseOut (double progress)
    {
        return progress <= 0 ? 0 : progress >= 1 ? 1 : -Math.Pow (2, -10 * progress) + 1;
    }

    private static readonly EasingMethod _exponentialEaseInOut = Chain (ExponentialEaseIn, ExponentialEaseOut);

    /// <summary>
    ///     <para>A combination of the <see cref="EasingMethods.ExponentialEaseIn"/> and <see cref="EasingMethods.ExponentialEaseOut"/> methods.</para>
    ///     <para>It accelerates from approximately 0 to a velocity of approximately 7 (log 1024) and then decelerates back to a velocity of approximately 0.</para>
    /// </summary>
    /// <param name="progress">The time progress of the animation.</param>
    /// <returns>The value progress of the animation.</returns>
    public static double ExponentialEaseInOut (double progress)
    {
        return _exponentialEaseInOut (progress);
    }
}
