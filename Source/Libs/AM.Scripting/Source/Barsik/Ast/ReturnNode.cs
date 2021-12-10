// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* ReturnNode.cs -- возврат значения из функции
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
    /// Возврат значения из функции.
    /// </summary>
    sealed class ReturnNode : StatementNode
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="value"></param>
        public ReturnNode
            (
                AtomNode? value
            )
        {
            _value = value;
        }

        #endregion

        #region Private members

        private readonly AtomNode? _value;

        #endregion

        #region StatementNode members


        /// <inheritdoc cref="StatementNode.Execute"/>
        public override void Execute
            (
                Context context
            )
        {
            PreExecute (context);

            var value = _value?.Compute (context) ?? "(null)";

            throw new ReturnException (value);
        }

        #endregion
    }
}
