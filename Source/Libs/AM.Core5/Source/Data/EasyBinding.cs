// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* EasyBinding.cs -- простой байндинг свойств
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Data
{
    //
    // Заимствовано из проекта Praeclarum.Bind
    //
    // https://raw.githubusercontent.com/praeclarum/Bind/master/src/Bind.cs
    //
    // Copyright 2013-2014 Frank A. Krueger
    //

    /// <summary>
    /// Простой байндинг свойств.
    /// Абстрактный класс, представляющий привязки между значениями в приложениях.
    /// Привязки создаются с помощью Create и удаляются с помощью Unbind.
    /// </summary>
    public abstract class EasyBinding
    {
        /// <summary>
        /// Unbind this instance. This cannot be undone.
        /// </summary>
        public virtual void Unbind ()
        {
        }

        /// <summary>
        /// Uses the lambda expression to create data bindings.
        /// Equality expression (==) become data bindings.
        /// And expressions (&amp;&amp;) can be used to group the data bindings.
        /// </summary>
        /// <param name="specifications">The binding specifications.</param>
        public static EasyBinding Create<T> (Expression<Func<T>> specifications)
        {
            return BindExpression (specifications.Body);
        }

        static EasyBinding BindExpression (Expression expr)
        {
            //
            // Is this a group of bindings
            //
            if (expr.NodeType == ExpressionType.AndAlso) {

                var b = (BinaryExpression)expr;

                var parts = new List<Expression> ();

                while (b != null) {
                    var l = b.Left;
                    parts.Add (b.Right);
                    if (l.NodeType == ExpressionType.AndAlso) {
                        b = (BinaryExpression)l;
                    } else {
                        parts.Add (l);
                        b = null;
                    }
                }

                parts.Reverse ();

                return new MultipleBindings (parts.Select (BindExpression));
            }

            //
            // Are we binding two values?
            //
            if (expr.NodeType == ExpressionType.Equal) {
                var b = (BinaryExpression)expr;
                return new EqualityBinding (b.Left, b.Right);
            }

            //
            // This must be a new object binding (a template)
            //
            throw new NotSupportedException ("Only equality bindings are supported.");
        }

        /// <summary>
        /// Установка значения.
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="value"></param>
        /// <param name="changeId"></param>
        /// <returns></returns>
        protected static bool SetValue
            (
                Expression expr,
                object? value,
                int changeId
            )
        {
            if (expr.NodeType == ExpressionType.MemberAccess) {
                var m = (MemberExpression)expr;
                var mem = m.Member;

                var target = Evaluator.EvalExpression (m.Expression!);

                var f = mem as FieldInfo;
                var p = mem as PropertyInfo;

                if (f != null) {
                    f.SetValue (target, value);
                } else if (p != null) {
                    p.SetValue (target, value, null);
                } else {
                    ReportError ("Trying to SetValue on " + mem.GetType () + " member");
                    return false;
                }

                InvalidateMember (target, mem, changeId);
                return true;
            }

            ReportError ("Trying to SetValue on " + expr.NodeType + " expression");
            return false;
        }

        /// <summary>
        /// Возникает при ошибке.
        /// </summary>
        public static event Action<string> Error = delegate {};

        static void ReportError (string message)
        {
            Debug.WriteLine (message);
            Error (message);
        }

        /// <summary>
        /// Сообщает об ошибке
        /// </summary>
        /// <param name="errorObject"></param>
        static void ReportError (object errorObject)
        {
            ReportError (errorObject.ToString ()!);
        }

        #region Change Notification

        class MemberActions
        {
            readonly object? target;
            readonly MemberInfo member;

            EventInfo? eventInfo;
            Delegate? eventHandler;

            public MemberActions (object? target, MemberInfo mem)
            {
                this.target = target;
                member = mem;
            }

            void AddChangeNotificationEventHandler ()
            {
                if (target != null)
                {
                    if (target is INotifyPropertyChanged npc
                        && member is PropertyInfo)
                    {
                        npc.PropertyChanged += HandleNotifyPropertyChanged;
                    }
                    else
                    {
                        AddHandlerForFirstExistingEvent (member.Name + "Changed", "EditingDidEnd", "ValueChanged", "Changed");
//						if (!added) {
//							Debug.WriteLine ("Failed to bind to change event for " + target);
//						}
                    }
                }
            }

            bool AddHandlerForFirstExistingEvent (params string[] names)
            {
                var type = target!.GetType ();
                foreach (var name in names)
                {
                    var ev = GetEvent (type, name);

                    if (ev != null)
                    {
                        eventInfo = ev;
                        var isClassicHandler = typeof(EventHandler).GetTypeInfo ().IsAssignableFrom (ev.EventHandlerType!.GetTypeInfo ());

                        eventHandler = isClassicHandler ?
                            (EventHandler) HandleAnyEvent :
                            CreateGenericEventHandler (ev, () => HandleAnyEvent (null, EventArgs.Empty));

                        ev.AddEventHandler(target, eventHandler);
                        Debug.WriteLine ("BIND: Added handler for {0} on {1}", eventInfo.Name, target);
                        return true;
                    }
                }
                return false;
            }

            static EventInfo? GetEvent (Type type, string eventName)
            {
                var t = type;
                while (t != null && t != typeof(object))
                {
                    var ti = t.GetTypeInfo ();
                    var ev = t.GetTypeInfo ().GetDeclaredEvent (eventName);
                    if (ev != null)
                    {
                        return ev;
                    }

                    t = ti.BaseType;
                }
                return null;
            }

            static Delegate CreateGenericEventHandler (EventInfo evt, Action d)
            {
                var handlerType = evt.EventHandlerType;
                var handlerTypeInfo = handlerType!.GetTypeInfo ();
                var handlerInvokeInfo = handlerTypeInfo.GetDeclaredMethod ("Invoke");
                var eventParams = handlerInvokeInfo!.GetParameters();

                //lambda: (object x0, EventArgs x1) => d()
                var parameters = eventParams.Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray ();
                var body = Expression.Call
                    (
                        Expression.Constant(d),
                        d.GetType().GetTypeInfo ().GetDeclaredMethod ("Invoke")!
                    );
                var lambda = Expression.Lambda(body, parameters);

                var delegateInvokeInfo = lambda.Compile ().GetMethodInfo ();
                return delegateInvokeInfo.CreateDelegate (handlerType!, null);
            }

            void UnsubscribeFromChangeNotificationEvent ()
            {
                if (target is INotifyPropertyChanged npc && (member is PropertyInfo))
                {
                    npc.PropertyChanged -= HandleNotifyPropertyChanged;
                    return;
                }

                if (eventInfo == null)
                    return;

                eventInfo.RemoveEventHandler (target, eventHandler);

                Debug.WriteLine ("BIND: Removed handler for {0} on {1}", eventInfo.Name, target);

                eventInfo = null;
                eventHandler = null;
            }

            void HandleNotifyPropertyChanged
                (
                    object? sender,
                    PropertyChangedEventArgs e
                )
            {
                if (e.PropertyName == member.Name)
                    InvalidateMember (target, member);
            }

            void HandleAnyEvent (object? sender, EventArgs e)
            {
                InvalidateMember (target, member);
            }

            readonly List<MemberChangeAction> actions = new List<MemberChangeAction> ();

            /// <summary>
            /// Add the specified action to be executed when Notify() is called.
            /// </summary>
            /// <param name="action">Action.</param>
            public void AddAction (MemberChangeAction action)
            {
                if (actions.Count == 0) {
                    AddChangeNotificationEventHandler ();
                }

                actions.Add (action);
            }

            public void RemoveAction (MemberChangeAction action)
            {
                actions.Remove (action);

                if (actions.Count == 0) {
                    UnsubscribeFromChangeNotificationEvent ();
                }
            }

            /// <summary>
            /// Execute all the actions.
            /// </summary>
            /// <param name="changeId">Change identifier.</param>
            public void Notify (int changeId)
            {
                foreach (var s in actions) {
                    s.Notify (changeId);
                }
            }
        }

        static readonly Dictionary<Tuple<object?, MemberInfo>, MemberActions> objectSubs = new ();

        internal static MemberChangeAction AddMemberChangeAction
            (
                object? target,
                MemberInfo member,
                Action<int> k
            )
        {
            var key = Tuple.Create (target, member);
            if (!objectSubs.TryGetValue (key, out var subs))
            {
                subs = new MemberActions (target, member);
                objectSubs.Add (key, subs);
            }

//			Debug.WriteLine ("ADD CHANGE ACTION " + target + " " + member);
            var sub = new MemberChangeAction (target!, member, k);
            subs.AddAction (sub);
            return sub;
        }

        internal static void RemoveMemberChangeAction (MemberChangeAction sub)
        {
            var key = Tuple.Create (sub.Target, sub.Member);
            if (objectSubs.TryGetValue (key, out var subs))
            {
//				Debug.WriteLine ("REMOVE CHANGE ACTION " + sub.Target + " " + sub.Member);
                subs.RemoveAction (sub);
            }
        }

        /// <summary>
        /// Invalidate the specified object member. This will cause all actions
        /// associated with that member to be executed.
        /// This is the main mechanism by which binding values are distributed.
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="member">Member of the object that changed</param>
        /// <param name="changeId">Change identifier</param>
        public static void InvalidateMember (object? target, MemberInfo member, int changeId = 0)
        {
            var key = Tuple.Create (target, member);
            if (objectSubs.TryGetValue (key, out var subs))
            {
//				Debug.WriteLine ("INVALIDATE {0} {1}", target, member.Name);
                subs.Notify (changeId);
            }
        }

        /// <summary>
        /// A nice expression based way to invalidate the specified object member.
        /// This will cause all actions associated with that member to be executed.
        /// This is the main mechanism by which binding values are distributed.
        /// </summary>
        /// <param name="lambdaExpr">Lambda expr of the Member of the object that changed</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void InvalidateMember<T>(Expression<Func<T>> lambdaExpr)
        {
            var body = lambdaExpr.Body;
            if (body.NodeType == ExpressionType.MemberAccess)
            {
                var m = (MemberExpression)body;
                var obj = Evaluator.EvalExpression(m.Expression!);
                InvalidateMember(obj, m.Member);
            }
        }

        #endregion
    }
}
