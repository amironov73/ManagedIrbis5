// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* StatementNode.cs -- базовый класс для стейтментов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Sprache;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Базовый класс для стейтментов.
    /// </summary>
    public class StatementNode
        : AstNode,
        IPositionAware<StatementNode>
    {
        #region Private members

        /// <summary>
        /// Перед выполнением стейтмента.
        /// </summary>
        protected virtual void PreExecute
            (
                Context context
            )
        {
            IBarsikDebugger? debugger = null;
            Breakpoint? breakpoint = null;

            for (var ctx = context; ctx is not null; ctx = ctx.Parent)
            {
                debugger ??= ctx.Debugger;
                context.Breakpoints.TryGetValue (this, out breakpoint);
            }

            if (breakpoint is not null && debugger is not null)
            {
                if (breakpoint.Trace)
                {
                    debugger.Trace (this);
                }

                if (breakpoint.Break)
                {
                    debugger.Raise (context, this);
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Стартовая позиция.
        /// </summary>
        public Position? StartPosition { get; private set; }

        /// <summary>
        /// Длина в символах.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Выполнение действий, связанных с данным узлом.
        /// </summary>
        /// <param name="context">Контекст исполнения программы.</param>
        public virtual void Execute
            (
                Context context
            )
        {
            PreExecute (context);
        }

        #endregion

        #region IPositionAware members

        /// <inheritdoc cref="IPositionAware{T}.SetPos"/>
        public StatementNode SetPos
            (
                Position startPosition,
                int length
            )
        {
            StartPosition = startPosition;
            Length = length;

            return this;
        }

        #endregion
    }
}
