// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* ExternalNode.cs -- выполнение внешнего кода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Выполнение некоторого внешнего по отношению к Барсику кода.
    /// Полагается на глобальный обработчик, заданный в контексте
    /// интерпретатора.
    /// </summary>
    public sealed class ExternalNode
        : StatementNode
    {
        #region Properties

        /// <summary>
        /// Произвольный код, понятный обработчику внешнего кода.
        /// </summary>
        public string? Code { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ExternalNode
            (
                string? code
            )
        {
            Code = code;
        }

        #endregion

        #region StatementNode example

        /// <inheritdoc cref="StatementNode.Execute"/>
        public override void Execute
            (
                Context context
            )
        {
            PreExecute (context);

            ExternalCodeHandler? handler = null;
            for (var ctx = context; ctx is not null; ctx = ctx.Parent)
            {
                handler = ctx.ExternalCodeHandler;
                if (handler is not null)
                {
                    break;
                }
            }

            if (handler is null)
            {
                context.Error.WriteLine ("Missing external code handler");
                return;
            }

            try
            {
                handler (context, this);
            }
            catch (Exception exception)
            {
                context.Error.WriteLine (exception.Message);
            }
        }

        #endregion
    }
}
