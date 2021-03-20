// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftIteration.cs -- итерация для for/foreach и т. п.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using AM;

using ManagedIrbis.Pft.Infrastructure.Ast;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Итерация для <see cref="PftParallelFor"/>,
    /// <see cref="PftParallelForEach"/>, <see cref="PftParallelGroup"/>
    /// и <see cref="PftParallelWith"/>.
    /// </summary>
    internal sealed class PftIteration //-V3073
        : IDisposable
    {
        #region Properties

        public PftGroup Group { get; private set; }

        public object Data { get; private set; }

        public PftNodeCollection Nodes { get; set; }

        public PftContext Context { get; set; }

        public int Index { get; private set; }

        public Task Task { get; private set; }

        public Exception Exception { get; private set; }

        public string Result { get { return Context.Text; } }

        private Action<PftIteration, object> _Action { get; set; }

        #endregion

        #region Construction

        public PftIteration
            (
                PftContext context,
                PftNodeCollection nodes,
                int index,
                Action<PftIteration, object> action,
                object data,
                bool withGroup
            )
        {
            Context = context.Push();
            Nodes = nodes.CloneNodes(nodes.Parent);
            Index = index;
            if (withGroup)
            {
                Group = new PftGroup();
                Context.CurrentGroup = Group;
            }
            Context.Index = Index;
            Exception = null;
            _Action = action;
            Data = data;

            Task = new Task(_Run);
            Task.Start();
        }

        #endregion

        #region Private members

        private void _Run()
        {
            try
            {
                _Action(this, Data);
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        "PftIteration::_Run",
                        exception
                    );

                Exception = exception;
            }
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Context.Pop();
        }

        #endregion

        #region Object members

        /// <see cref="object.ToString"/>
        public override string ToString() => Index.ToString();

        #endregion
    }
}
