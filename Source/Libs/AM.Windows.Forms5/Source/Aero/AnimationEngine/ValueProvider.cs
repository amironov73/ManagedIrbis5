﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* ValueProvider.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AeroSuite.AnimationEngine;

/// <summary>
/// A class for providing transition values. You can use it as a base for animations of custom-drawn controls, game objects, ...
/// </summary>
/// <typeparam name="T">The type of value that should be transitioned.</typeparam>
public class ValueProvider<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValueProvider{T}"/> class.
    /// </summary>
    /// <param name="startValue">The start value.</param>
    /// <param name="valueFactory">The value factory.</param>
    public ValueProvider
        (
            T startValue,
            ValueFactory<T> valueFactory
        )
        : this (startValue, valueFactory, EasingMethods.DefaultEase)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueProvider{T}"/> class.
    /// </summary>
    /// <param name="startValue">The start value.</param>
    /// <param name="valueFactory">The value factory.</param>
    /// <param name="easingMethod">The easing method.</param>
    public ValueProvider
        (
            T startValue,
            ValueFactory<T> valueFactory,
            EasingMethod easingMethod
        )
    {
        StartValue = startValue;
        TargetValue = StartValue;
        StartTime = DateTime.Now;
        Duration = TimeSpan.Zero;

        EasingMethod = easingMethod;
        ValueFactory = valueFactory;
    }

    /// <summary>
    /// Gets or sets the easing method used for creating a smooth transition.
    /// </summary>
    /// <value>
    /// The easing method.
    /// </value>
    public virtual EasingMethod EasingMethod { get; set; }

    /// <summary>
    /// Gets or sets the value factory used for generating a value corresponding to the progress.
    /// </summary>
    /// <value>
    /// The value factory.
    /// </value>
    public virtual ValueFactory<T> ValueFactory { get; set; }

    /// <summary>
    /// Gets or sets the start value.
    /// </summary>
    /// <value>
    /// The start value.
    /// </value>
    public virtual T StartValue { get; set; }

    /// <summary>
    /// Gets or sets the target value.
    /// </summary>
    /// <value>
    /// The target value.
    /// </value>
    public virtual T TargetValue { get; set; }

    /// <summary>
    /// Gets or sets the start time.
    /// </summary>
    /// <value>
    /// The start time.
    /// </value>
    public virtual DateTime StartTime { get; set; }

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    /// <value>
    /// The duration.
    /// </value>
    public virtual TimeSpan Duration { get; set; }

    /// <summary>
    /// Returns a value corresponding to the current progress.
    /// </summary>
    /// <value>
    /// The current value.
    /// </value>
    public virtual T CurrentValue
    {
        get
        {
            var progress = CurrentProgress;
            if (progress >= 1)
            {
                return TargetValue;
            }

            return ValueFactory (StartValue, TargetValue, EasingMethod (progress));
        }
    }

    /// <summary>
    /// Returns the current progress.
    /// </summary>
    /// <value>
    /// The current progress.
    /// </value>
    public double CurrentProgress
    {
        get
        {
            var currentDuration = DateTime.Now - StartTime;
            if (currentDuration >= Duration)
            {
                return 1;
            }

            return currentDuration.TotalMilliseconds / Duration.TotalMilliseconds;
        }
    }

    /// <summary>
    /// Starts a transition from the current value to the specified target value.
    /// </summary>
    /// <param name="targetValue">The target value.</param>
    /// <param name="duration">The duration.</param>
    public virtual void StartTransition (T targetValue, TimeSpan duration)
    {
        StartTransition (CurrentValue, targetValue, duration);
    }

    /// <summary>
    /// Starts a transition from the specified start value to the specified target value.
    /// </summary>
    /// <param name="startValue">The start value.</param>
    /// <param name="targetValue">The target value.</param>
    /// <param name="duration">The duration.</param>
    public virtual void StartTransition (T startValue, T targetValue, TimeSpan duration)
    {
        StartValue = startValue;
        TargetValue = targetValue;
        StartTime = DateTime.Now;
        Duration = duration;
    }

    /// <summary>
    /// Cancels the current transition.
    /// </summary>
    public virtual void CancelTransition()
    {
        StartValue = CurrentValue;
        TargetValue = CurrentValue;
        StartTime = DateTime.Now;
        Duration = TimeSpan.Zero;
    }
}
