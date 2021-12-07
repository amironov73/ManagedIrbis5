// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* TargetNode.cs -- цель присваивания
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
    /// Цель присваивания (в операторе присваивания).
    /// </summary>
    sealed class TargetNode
    {
        #region Properties

        /// <summary>
        /// Имя переменной.
        /// </summary>
        public string VariableName { get; }

        /// <summary>
        /// Имя члена класса (опционально).
        /// </summary>
        public string? MemberName { get; }

        /// <summary>
        /// Индексное выражение (опционально)
        /// </summary>
        public AtomNode? Index { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public TargetNode
            (
                string variableName,
                string? memberName,
                AtomNode? index
            )
        {
            VariableName = variableName;
            MemberName = memberName;
            Index = index;
        }

        #endregion
    }
}
