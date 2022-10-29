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

#nullable enable

namespace AeroSuite.AnimationEngine;

/// <summary>
/// A method that provides values for smooth transitions.
/// </summary>
/// <param name="progress">
///     <para>The time progress of the animation.</para>
///     <para><c>0.0</c> is the start of the animation, <c>1.0</c> is the end.</para>
///     <para>Every value lower than <c>0.0</c> will return the same
///     value as <c>0.0</c>, every value higher than <c>1.0</c> will return the
///     same value as <c>1.0</c>.</para>
/// </param>
/// <returns>The value progress of the animation.</returns>
public delegate double EasingMethod (double progress);
