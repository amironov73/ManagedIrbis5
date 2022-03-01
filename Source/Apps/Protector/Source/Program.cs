// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;
using AM.Configuration;

#endregion

#nullable enable

namespace Protector;

//
// Простейший инструмент для защиты строки подключения
// и других чувствительных данных.
//

internal static class Program
{
    static void Main (string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine ("Usage: protector <string-to-protect>");
        }
        else
        {
            var source = args[0];
            if (source == "-")
            {
                source = Console.ReadLine().ThrowIfNull();
            }

            if (source == "-u")
            {
                source = args[1];
                if (source == "-")
                {
                    source = Console.ReadLine().ThrowIfNull();
                }

                var decrypted = ConfigurationUtility.Unprotect (source);
                Console.WriteLine (decrypted);
            }
            else
            {
                var encrypted = ConfigurationUtility.Protect (source);
                Console.WriteLine (encrypted);
            }
        }
    }
}
