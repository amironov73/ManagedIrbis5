// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* AtomNode.cs -- узел, в котором происходят какие-то вычисления
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
    /// Узел, в котором происходят какие-то вычисления.
    /// </summary>
    public abstract class AtomNode : AstNode
    {
        #region Public methods

        /// <summary>
        /// Вычисление значения, связанного с данным узлом.
        /// </summary>
        public abstract dynamic? Compute (Context context);

        #endregion
    }
}
