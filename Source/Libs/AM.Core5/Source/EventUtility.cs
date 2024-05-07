// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EvevntUtility.cs -- манипуляции с событиями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using JetBrains.Annotations;

#endregion

namespace AM;

/// <summary>
/// Манипуляции с событиями.
/// </summary>
[PublicAPI]
public static class EventUtility
{
    #region Public methods

    /// <summary>
    /// Динамический вызов указанного события у произвольного объекта.
    /// </summary>
    /// <param name="instance">Экземпляр объекта, в котором
    /// поселились обработчики события.</param>
    /// <param name="eventName">Имя события.</param>
    /// <param name="eventArgs">Аргумент для обработчика события.</param>
    /// <typeparam name="TInstance">Тип экземпляра объекта.</typeparam>
    /// <typeparam name="TEventArgs">Тип аргумента для обработчика события.
    /// </typeparam>
    [RequiresDynamicCode ("Not compatible with native code")]
    public static void Raise<TInstance, TEventArgs>
        (
            TInstance instance,
            string eventName,
            TEventArgs? eventArgs = null
        )
        where TInstance: class
        where TEventArgs: EventArgs
    {
        Sure.NotNull (instance);
        Sure.NotNullNorEmpty (eventName);

        var type = typeof (TInstance);
        const BindingFlags eventFlags = BindingFlags.Instance
            | BindingFlags.Public | BindingFlags.NonPublic;
        var eventInfo = type.GetEvent (eventName, eventFlags).ThrowIfNull();
        var method = eventInfo.RaiseMethod.ThrowIfNull();
        method.Invoke (instance, [eventArgs]);
    }

    /// <summary>
    /// Отписка ото всех обработчиков указанного события
    /// у произвольного объекта.
    /// </summary>
    /// <param name="instance">Экземпляр объекта,
    /// в котором поселились обработчики события.</param>
    /// <param name="eventName">Имя события.</param>
    [RequiresDynamicCode ("Not compatible with native code")]
    public static void UnsubscribeAll<TType>
        (
            object instance,
            string eventName
        )
    {
        Sure.NotNull (instance);
        Sure.NotNullNorEmpty (eventName);

        var type = typeof (TType);
        const BindingFlags eventFlags = BindingFlags.Instance
            | BindingFlags.Public | BindingFlags.NonPublic;
        var eventInfo = type.GetEvent (eventName, eventFlags).ThrowIfNull();

        const BindingFlags fieldFlags = BindingFlags.Instance
            | BindingFlags.NonPublic;
        var field = type.GetField (eventName, fieldFlags).ThrowIfNull();

        var eventDelegate = (MulticastDelegate) field.GetValue (instance)
            .ThrowIfNull();
        foreach (var handler in eventDelegate.GetInvocationList())
        {
            eventInfo.RemoveEventHandler (instance, handler);
        }
    }

    #endregion
}
