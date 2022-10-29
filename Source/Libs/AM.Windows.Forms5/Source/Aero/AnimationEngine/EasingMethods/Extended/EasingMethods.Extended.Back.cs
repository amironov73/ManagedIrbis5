// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* EasingMethods.Extended.Back.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AeroSuite.AnimationEngine;

public static partial class EasingMethods
{
    public static partial class Extended
    {
        private const double Back = 1.70158;

        /// <summary>
        ///     <para>An easing method that goes down to a value progress of -0.1 and then goes back up to 1.0.</para>
        ///     <para>The velocity starts at 0 and goes up to 4.70158.</para>
        ///     <para>Do not use this method if you do not have a proper method for handling progress values smaller than 0.0.</para>
        ///     <para>Function: f(p) = p ^ 2 * ((b + 1) * p - b)</para>
        ///     <para>Derivative: f'(p) = p * (3p + b * (-2 + 3p))</para>
        /// </summary>
        /// <param name="progress">The time progress of the animation.</param>
        /// <returns>The value progress of the animation.</returns>
        public static double BackEaseIn (double progress)
        {
            return progress <= 0 ? 0 : progress >= 1 ? 1 : Math.Pow (progress, 2) * ((Back + 1) * progress - Back);
        }

        /// <summary>
        ///     <para>An easing method that goes up to a value progress of 1.1 and then goes back to 1.0.</para>
        ///     <para>The velocity starts at 4.70158 and goes down to 0.</para>
        ///     <para>Do not use this method if you do not have a proper method for handling progress values greater than 1.0.</para>
        ///     <para>Function: f(p) = (p - 1) ^ 2 * ((b + 1) * (p - 1) + b) + 1</para>
        ///     <para>Derivative: f'(p) = (-1 + p) * (-3 - b + 3p + 3bp)</para>
        /// </summary>
        /// <param name="progress">The time progress of the animation.</param>
        /// <returns>The value progress of the animation.</returns>
        public static double BackEaseOut (double progress)
        {
            return progress <= 0
                ? 0
                : progress >= 1
                    ? 1
                    : Math.Pow (progress - 1, 2) * ((Back + 1) * (progress - 1) + Back) + 1;
        }

        private static readonly EasingMethod _backEaseInOut = Chain (BackEaseIn, BackEaseOut);

        /// <summary>
        ///     <para>A combination of the <see cref="EasingMethods.Extended.BackEaseIn"/> and <see cref="EasingMethods.Extended.BackEaseOut"/> methods.</para>
        ///     <para>Do not use this method if you do not have a proper method for handling progress values greater than 1.0 and lower than 0.0.</para>
        /// </summary>
        /// <param name="progress">The time progress of the animation.</param>
        /// <returns>The value progress of the animation.</returns>
        public static double BackEaseInOut (double progress)
        {
            return _backEaseInOut (progress);
        }
    }
}
