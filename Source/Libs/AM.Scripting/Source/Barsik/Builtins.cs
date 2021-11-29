// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* Builtins.cs -- встроенные функции
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
    /// Встроенные функции.
    /// </summary>
    public static class Builtins
    {
        #region Public methods

        /// <summary>
        /// Реестр встроенных функций.
        /// </summary>
        public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
        {
            { "list", new FunctionDescriptor ("list", List) },
            { "now", new FunctionDescriptor ("now", Now) }
        };

        /// <summary>
        /// Список.
        /// </summary>
        public static dynamic List (dynamic?[] args) => new List<dynamic?>();

        /// <summary>
        /// Текущие дата и время.
        /// </summary>
        public static dynamic Now (dynamic?[] args) => DateTime.Now;

        #endregion
    }
}
