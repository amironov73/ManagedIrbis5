// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* EqualityBinding.cs -- байндинг между двумя значениями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Data
{
    /// <summary>
    /// Байндинг между двумя значениями.
    /// Когда одно изменяется, другое устанавливается.
    /// </summary>
    class EqualityBinding : EasyBinding
    {
        object Value;

        class Trigger
        {
            public Expression Expression;
            public MemberInfo Member;
            public MemberChangeAction ChangeAction;
        }

        readonly List<Trigger> leftTriggers = new ();
        readonly List<Trigger> rightTriggers = new ();

        public EqualityBinding (Expression left, Expression right)
        {
            // Try evaling the right and assigning left
            Value = Evaluator.EvalExpression (right);
            var leftSet = SetValue (left, Value, nextChangeId);

            // If that didn't work, then try the other direction
            if (!leftSet) {
                Value = Evaluator.EvalExpression (left);
                SetValue (right, Value, nextChangeId);
            }

            nextChangeId++;

            CollectTriggers (left, leftTriggers);
            CollectTriggers (right, rightTriggers);

            Resubscribe (leftTriggers, left, right);
            Resubscribe (rightTriggers, right, left);
        }

        public override void Unbind ()
        {
            Unsubscribe (leftTriggers);
            Unsubscribe (rightTriggers);
            base.Unbind ();
        }

        void Resubscribe (List<Trigger> triggers, Expression expr, Expression dependentExpr)
        {
            Unsubscribe (triggers);
            Subscribe (triggers, changeId => OnSideChanged (expr, dependentExpr, changeId));
        }

        int nextChangeId = 1;
        readonly HashSet<int> activeChangeIds = new HashSet<int> ();

        void OnSideChanged (Expression expr, Expression dependentExpr, int causeChangeId)
        {
            if (activeChangeIds.Contains (causeChangeId))
                return;

            var v = Evaluator.EvalExpression (expr);

            if (v == null && Value == null)
                return;

            if ((v == null && Value != null) ||
                (v != null && Value == null) ||
                ((v is IComparable) && ((IComparable)v).CompareTo (Value) != 0)) {

                Value = v;

                var changeId = nextChangeId++;
                activeChangeIds.Add (changeId);
                SetValue (dependentExpr, v, changeId);
                activeChangeIds.Remove (changeId);
            }
//			else {
//				Debug.WriteLine ("Prevented needless update");
//			}
        }

        static void Unsubscribe (List<Trigger> triggers)
        {
            foreach (var t in triggers) {
                if (t.ChangeAction != null) {
                    RemoveMemberChangeAction (t.ChangeAction);
                }
            }
        }

        static void Subscribe (List<Trigger> triggers, Action<int> action)
        {
            foreach (var t in triggers) {
                t.ChangeAction = AddMemberChangeAction (Evaluator.EvalExpression (t.Expression), t.Member, action);
            }
        }

        void CollectTriggers (Expression s, List<Trigger> triggers)
        {
            if (s.NodeType == ExpressionType.MemberAccess) {

                var m = (MemberExpression)s;
                CollectTriggers (m.Expression, triggers);
                var t = new Trigger { Expression = m.Expression, Member = m.Member };
                triggers.Add (t);

            } else {
                var b = s as BinaryExpression;
                if (b != null) {
                    CollectTriggers (b.Left, triggers);
                    CollectTriggers (b.Right, triggers);
                }
            }
        }

    } // class EqualityBinding

} // namespace AM.Data
