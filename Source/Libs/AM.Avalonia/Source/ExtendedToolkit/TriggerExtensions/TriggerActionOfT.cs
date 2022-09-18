// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace Avalonia.ExtendedToolkit.TriggerExtensions;

/// <summary>
/// implements TriggerAction T have to be an AvaloniaObject
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class TriggerAction<T>
    : TriggerAction
    where T : AvaloniaObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TriggerAction&lt;T&gt;"/> class.
    /// </summary>
    protected TriggerAction()
        : base (typeof (T))
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Gets the object to which this <see cref="TriggerAction"/> is attached.
    /// </summary>
    /// <value>The associated object.</value>
    protected new T? AssociatedObject => (T?) base.AssociatedObject;

    /// <summary>
    /// Gets the associated object type constraint.
    /// </summary>
    /// <value>The associated object type constraint.</value>
    protected sealed override Type AssociatedObjectTypeConstraint => base.AssociatedObjectTypeConstraint;
}
