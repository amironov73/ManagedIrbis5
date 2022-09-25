// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace DirectTests;

internal class Program
{
    static void Main()
    {
        Console.WriteLine (Infrastructure.TestDataPath);
        Console.WriteLine (Infrastructure.Irbis64RootPath);

        // for remote debugging
        // Console.ReadLine();

        Unifor2Test.Unifor2_GetMaxMfn_1();

        XrfFile64Test.XrfFile64_LockRecord_1();
        XrfFile64Test.XrfFile64_ReadRecord_1();
        XrfFile64Test.XrfFile64_ReopenFile_1();
    }
}
