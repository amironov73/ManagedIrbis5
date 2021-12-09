// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* TryNode.cs -- блок try-catch-finally
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Блок try-catch-finally
    /// </summary>
    sealed class TryNode
        : StatementNode
    {
        public TryNode
            (
                IEnumerable<StatementNode> tryBlock,
                CatchNode? catchNode,
                IEnumerable<StatementNode>? finallyBlock
            )
        {
            _tryBlock = new (tryBlock);
            _catchBlock = new ();
            _catchVariable = null;
            if (catchNode is not null)
            {
                _catchVariable = catchNode.VariableName;
                _catchBlock.AddRange (catchNode.Block);
            }
            _finallyBlock = new ();
            if (finallyBlock is not null)
            {
                _finallyBlock.AddRange (finallyBlock);
            }
        }

        private readonly List<StatementNode> _tryBlock;
        private readonly string? _catchVariable;
        private readonly List<StatementNode> _catchBlock;
        private readonly List<StatementNode> _finallyBlock;

        public override void Execute
            (
                Context context
            )
        {
            try
            {
                foreach (var statement in _tryBlock)
                {
                    statement.Execute (context);
                }
            }
            catch (Exception exception)
            {
                var nestedContext = context.CreateChild();
                nestedContext.Variables[_catchVariable!] = exception;
                foreach (var statement in _catchBlock)
                {
                    statement.Execute (nestedContext);
                }
            }
            finally
            {
                foreach (var statement in _finallyBlock)
                {
                    statement.Execute (context);
                }
            }
        }
    }
}
