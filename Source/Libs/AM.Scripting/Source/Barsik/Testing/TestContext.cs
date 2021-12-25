// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* TestContext.cs -- контекст прогона тестов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using AM.Text;
using AM.Scripting.Barsik;


#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Контекст прогона тестов.
    /// </summary>
    public sealed class TestContext
    {
        #region Properties

        /// <summary>
        /// Стандартный выходной проток.
        /// </summary>
        public TextWriter Output { get; }

        /// <summary>
        /// Выходной поток ошибок.
        /// </summary>
        public TextWriter Error { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public TestContext
            (
                TextWriter output,
                TextWriter? error = null
            )
        {
            Sure.NotNull (output);

            Output = output;
            Error = error ?? output;
        }

        #endregion
    }
}
