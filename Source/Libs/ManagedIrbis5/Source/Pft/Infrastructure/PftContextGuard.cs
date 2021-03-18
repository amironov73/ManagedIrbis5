// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* PftContextGuard.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;




#endregion

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    ///
    /// </summary>

    [DebuggerDisplay("{ChildContext}")]
    public struct PftContextGuard
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Child context.
        /// </summary>
        public PftContext ChildContext { get; private set; }

        /// <summary>
        /// Parent context.
        /// </summary>
        public PftContext ParentContext { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftContextGuard
            (
                PftContext parentContext
            )
            : this()
        {
            Code.NotNull(parentContext, "parentContext");

            ParentContext = parentContext;
            ChildContext = parentContext.Push();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Push again.
        /// </summary>
        public PftContext PushAgain()
        {
            ChildContext.Pop();
            ChildContext = ParentContext.Push();

            return ChildContext;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            ChildContext.Pop();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="ValueType.ToString" />
        public override string ToString()
        {
            return ChildContext.ToString();
        }

        #endregion
    }
}
