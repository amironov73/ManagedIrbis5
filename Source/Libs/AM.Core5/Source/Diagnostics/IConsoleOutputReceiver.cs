// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IConsoleOutputReceiver.cs -- принимает вывод консоли
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM.Collections;
using AM.Runtime;

#endregion

#nullable enable

namespace AM.Diagnostics
{
    /// <summary>
    /// Receives console output.
    /// </summary>
    public interface IConsoleOutputReceiver
    {
        /// <summary>
        /// Receives the console line.
        /// </summary>
        void ReceiveConsoleOutput
            (
                string text
            );
    }
}
