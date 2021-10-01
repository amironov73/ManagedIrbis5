// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- стартовый класс сервиса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;
using AM.Linq;

using ManagedIrbis;
using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

#endregion

#nullable enable

namespace IrbisCoreServer
{
    /// <summary>
    /// Стартовый класс сервиса.
    /// </summary>
    static class Program
    {

        /// <summary>
        /// Точка входа в сервис.
        /// </summary>
        public static int Main
            (
                string[] args
            )
        {
            Magna.Initialize (args, builder =>
            {
                Console.Out.WriteLine ("Initialized");
            });

            Magna.Info("Ready to run");

            return 0;

        } // class Main

    } // class Program

} // namespace IrbisCoreServer
