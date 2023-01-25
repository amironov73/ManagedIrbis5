// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* EventPad.cs -- прокладка между нативными событиями и скриптом-подписчиком
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Прокладка между нативными C#-событиями и Barsik-подписчиками.
/// </summary>
/// <remarks>
/// Рассчитан на события типа <see cref="EventHandler"/>
/// и <see cref="EventHandler{TEventArgs}"/>.
/// С другими сигнатурами не работает.
/// </remarks>
internal sealed class EventPad
{
    #region Properties

    /// <summary>
    /// Целевой объект.
    /// </summary>
    public object? Target { get; }

    /// <summary>
    /// Контекст вызова.
    /// </summary>
    public Context Context { get; }

    /// <summary>
    /// Информация о событии.
    /// </summary>
    public EventInfo Event { get; }

    /// <summary>
    /// Обработчик события.
    /// </summary>
    public Func<Context, dynamic?[], dynamic?> Handler { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public EventPad
        (
            Context context,
            object? target,
            string eventName,
            Func<Context, dynamic?[], dynamic?> handler
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (target);
        Sure.NotNullNorEmpty (eventName);
        Sure.NotNull (handler);

        _eventName = eventName;
        Context = context;
        Target = target;
        Handler = handler;
        var type = target!.GetType();
        Event = type.GetEvent (eventName)
            ?? throw new BarsikException ($"Can't find event {eventName}");
        var method = typeof (EventPad).GetMethod
            (
                nameof (CallPoint),
                BindingFlags.Instance|BindingFlags.NonPublic
            );
        _eventHandler = Delegate.CreateDelegate
            (
                Event.EventHandlerType!,
                this,
                method!
            );
        Event.AddEventHandler (target, _eventHandler);
    }

    #endregion

    #region Private members

    private readonly string _eventName;
    private readonly Delegate _eventHandler;

    /// <summary>
    /// Ради этого все и затевалось.
    /// </summary>
    private void CallPoint
        (
            object sender,
            EventArgs eventArgs
        )
    {
        var args = new dynamic[] { sender, eventArgs };
        Handler (Context, args); // возвращаемое значение игнорируем
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Отписка от события.
    /// </summary>
    public void Unsubscribe()
    {
        Event.RemoveEventHandler (Target, _eventHandler);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"EventPad {Target!.GetType()} {_eventName}";

    #endregion
}
