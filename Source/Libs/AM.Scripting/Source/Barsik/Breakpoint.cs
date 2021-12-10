// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* Breakpoint.cs -- точка останова в скрипте
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
    /// Точка останова в скрипте.
    /// </summary>
    public sealed class Breakpoint
    {
        #region Properties

        /// <summary>
        /// Стейтмент, к которому относится точка останова.
        /// </summary>
        public StatementNode Statement { get; }

        /// <summary>
        /// Выполнять трассировку при выполнении стейтмента?
        /// </summary>
        public bool Trace { get; set; }

        /// <summary>
        /// Нужно ли вызывать отладчик?
        /// </summary>
        public bool Break { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Breakpoint
            (
                StatementNode statement
            )
        {
            Statement = statement;
        }

        #endregion
    }
}
