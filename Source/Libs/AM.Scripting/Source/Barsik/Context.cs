// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* Context.cs -- контекст исполнения программы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Контекст исполнения программы, в т. ч. значания переменных.
    /// </summary>
    public sealed class Context
    {
        #region Properties

        /// <summary>
        /// Переменные.
        /// </summary>
        public Dictionary<string, dynamic?> Variables { get; }

        /// <summary>
        /// Выходной поток.
        /// </summary>
        public TextWriter Output { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Context
            (
                Dictionary<string, dynamic?> variables,
                TextWriter output
            )
        {
            Variables = variables;
            Output = output;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Дамп переменных.
        /// </summary>
        public void DumpVariables()
        {
            var keys = Variables.Keys.ToArray();
            Array.Sort (keys);
            foreach (var key in keys)
            {
                var value = Variables[key];
                if (value is null)
                {
                    Output.WriteLine ($"{key}: (null)");
                }
                else
                {
                    Output.WriteLine ($"{key}: {value.GetType().Name} = {value}");
                }
            }
        }

        #endregion
    }
}
