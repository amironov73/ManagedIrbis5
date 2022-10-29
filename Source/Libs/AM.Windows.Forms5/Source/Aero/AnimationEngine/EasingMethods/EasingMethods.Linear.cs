// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace AeroSuite.AnimationEngine;

public static partial class EasingMethods
{
    /// <summary>
    ///     <para>An easing method with a constant velocity of 1.</para>
    ///     <para>This method is not recommended for production use as it does not look well in most cases.</para>
    ///     <para>Function: f(p) = p</para>
    ///     <para>Derivative: f'(p) = 1</para>
    /// </summary>
    /// <param name="progress">The time progress of the animation.</param>
    /// <returns>The value progress of the animation.</returns>
    public static double Linear (double progress)
    {
        return progress <= 0 ? 0 : progress >= 1 ? 1 : progress;
    }
}
