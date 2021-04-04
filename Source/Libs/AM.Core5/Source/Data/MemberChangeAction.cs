// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* MemberChangeAction.cs -- действие, привязанное к определенному члену объекта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Data
{
    /// <summary>
    /// Действие, привязанное к определенному члену объекта.
    /// Когда вызывается Notify, действие выполняется.
    /// </summary>
    internal sealed class MemberChangeAction
    {
        readonly Action<int> action;

        public object Target { get; }

        public MemberInfo Member { get; }

        public MemberChangeAction
            (
                object target,
                MemberInfo member,
                Action<int> action
            )
        {
            Target = target;
            if (member == null)
                throw new ArgumentNullException (nameof(member));
            Member = member;
            if (action == null)
                throw new ArgumentNullException (nameof(action));
            this.action = action;
        }

        public void Notify (int changeId)
        {
            action (changeId);
        }

    } // class MemberChangeAction

} // namespace AM.Data
