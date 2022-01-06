// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable EventNeverSubscribedTo.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBePrivate.Local

using System;
using System.Collections.Generic;
using System.Reflection;

namespace ParsingExperiments;

static class EventExperimentOne
{
    sealed class CanaryEventArgs
        : EventArgs
    {
        public string? AdditionalData { get; }

        public CanaryEventArgs
            (
                string? additionalData
            )
        {
            AdditionalData = additionalData;
        }
    }

    class Canary
    {
        public event EventHandler? OneChanged;
        public event EventHandler<CanaryEventArgs>? TwoChanged;

        private int _one;
        private string? _two;

        public int One
        {
            get => _one;
            set
            {
                _one = value;
                OneChanged?.Invoke (this, EventArgs.Empty);
            }
        }

        public string? Two
        {
            get => _two;
            set
            {
                _two = value;
                var eventArgs = new CanaryEventArgs (value);
                TwoChanged?.Invoke (this, eventArgs);
            }
        }

        public int Addition (int first, int second)
        {
            return first + second;
        }
    }

    private sealed class Context
    {
        // nothing here
    }

    /// <summary>
    /// Прокладка между событием и скриптовым методом.
    /// </summary>
    private sealed class EventPad
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
                Type type,
                object? target,
                string eventName,
                Func<Context, dynamic?[], dynamic?> handler
            )
        {
            Context = context;
            Target = target;
            Event = type.GetEvent (eventName)!;
            Handler = handler;

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

        private readonly Delegate _eventHandler;

        private void CallPoint (object sender, EventArgs eventArgs)
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
    }

    class ScriptObject
    {
        public dynamic? HandlerMethodOne
            (
                Context context,
                dynamic?[] args
            )
        {
            Console.WriteLine ("In handler method one");
            var canary = (Canary) args[0]!;
            Console.WriteLine ($"{canary.One}");

            return null;
        }

        public dynamic? HandlerMethodTwo
            (
                Context context,
                dynamic?[] args
            )
        {
            Console.WriteLine ("In handler method two");
            var canary = (Canary) args[0]!;
            Console.WriteLine ($"{canary.Two}");
            var specific = (CanaryEventArgs) args[1]!;
            Console.WriteLine ($"{specific.AdditionalData}");

            return null;
        }
    }

    /// <summary>
    /// Динамический вызов любого метода.
    /// </summary>
    public static dynamic? CallAnything
        (
            object? target,
            string methodName,
            params object?[] args
        )
    {
        if (target is null)
        {
            return null;
        }

        bool staticCall;
        Type targetType;
        if (target is Type type)
        {
            staticCall = true;
            targetType = type;
        }
        else
        {
            staticCall = false;
            targetType = target.GetType();
        }

        var argTypes = new List<Type>();
        foreach (var o in args)
        {
            var argType = o is null ? typeof (object) : o.GetType();
            argTypes.Add (argType);
        }

        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic;
        bindingFlags |= staticCall ? BindingFlags.Static : BindingFlags.Instance;
        var method = targetType.GetMethod (methodName, bindingFlags, argTypes.ToArray());
        if (method is null)
        {
            Console.Error.WriteLine ($"Can't find method {methodName}");
            return null;
        }

        var result = method.Invoke (target, args);

        return result;
    }

    public static void EventGames()
    {
        var context = new Context();
        var obj = new ScriptObject();

        var canary = new Canary();
        var type = typeof (Canary);
        var padOne = new EventPad
            (
                context,
                type,
                canary,
                "OneChanged",
                obj.HandlerMethodOne
            );
        var padTwo = new EventPad
            (
                context,
                type,
                canary,
                "TwoChanged",
                obj.HandlerMethodTwo
            );

        canary.One = 123;
        canary.Two = "сто двадцать три";

        padOne.Unsubscribe();
        padTwo.Unsubscribe();

        canary.One = 321;
        canary.Two = "триста двацать один";

        var sum = CallAnything (canary, nameof (canary.Addition), 1, 2);
        Console.WriteLine (sum);
    }
}
