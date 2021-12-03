// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* NotAssignmentNode.cs -- простой вызов функции, без присваивания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable


namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Простой вызов функции, без присваивания.
    /// </summary>
    sealed class NotAssignmentNode : StatementNode
    {
        #region Construction

        public NotAssignmentNode (AtomNode expression)
        {
            _expression = expression;
        }

        #endregion

        #region Private members

        private readonly AtomNode _expression;

        #endregion

        #region AstNode members

        public override void Execute (Context context)
        {
            _expression.Compute (context);
        }

        #endregion
    }
}
