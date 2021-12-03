// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* NegationNode.cs -- смена знака числа
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
    /// Смена знака числа.
    /// </summary>
    sealed class NegationNode : AtomNode
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public NegationNode (AtomNode inner)
        {
            _inner = inner;
        }

        #endregion

        #region Private members

        private readonly AtomNode _inner;

        #endregion

        #region AtomNode members

        /// <inheritdoc cref="AtomNode.Compute"/>
        public override dynamic? Compute (Context context)
        {
            return - _inner.Compute (context);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"negation ({_inner})";
        }

        #endregion
    }
}
