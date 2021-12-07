// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* KeyValueNode.cs -- узел для хранения пары "ключ-значение"
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
    /// Узел для хранения пары "ключ-значение"
    /// </summary>
    sealed class KeyValueNode
    {
        #region Properties

        /// <summary>
        /// Ключ.
        /// </summary>
        public AtomNode Key { get; }

        /// <summary>
        /// Значение.
        /// </summary>
        public AtomNode Value { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public KeyValueNode
            (
                AtomNode key,
                AtomNode value
            )
        {
            Key = key;
            Value = value;
        }

        #endregion
    }
}
