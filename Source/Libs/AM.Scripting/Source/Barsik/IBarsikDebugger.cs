// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IBarsikDebugger.cs -- интерфейс отладчика для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM.Collections;
using AM.IO;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Интерфейс отладчика для Барсика.
    /// </summary>
    public interface IBarsikDebugger
    {
        /// <summary>
        /// Пробуждение отладчика при наступлении какого-нибудь события.
        /// </summary>
        public void Raise
            (
                Context context,
                StatementNode? statement
            );

        /// <summary>
        /// Трассировка.
        /// </summary>
        public void Trace (StatementNode statement);
    }
}
