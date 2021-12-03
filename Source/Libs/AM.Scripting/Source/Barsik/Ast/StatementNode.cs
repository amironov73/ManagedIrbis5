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

using System;
using System.Collections;
using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Базовый класс для стейтментов.
    /// </summary>
    class StatementNode : AstNode
    {
        /// <summary>
        /// Выполнение действий, связанных с данным узлом.
        /// </summary>
        /// <param name="context">Контекст исполнения программы.</param>
        public virtual void Execute (Context context)
        {
        }
    }
}
