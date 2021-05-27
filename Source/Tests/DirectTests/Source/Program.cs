// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using AM.PlatformAbstraction;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Direct;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

#nullable enable

namespace DirectTests
{

    internal class Program
    {
        static void Main()
        {
            Console.WriteLine(Infrastructure.TestDataPath);
            Console.WriteLine(Infrastructure.Irbis64RootPath);

            // for remote debugging
            // Console.ReadLine();

            Unifor2Test.Unifor2_GetMaxMfn_1();

            XrfFile64Test.XrfFile64_LockRecord_1();
            XrfFile64Test.XrfFile64_ReadRecord_1();
            XrfFile64Test.XrfFile64_ReopenFile_1();
        }
    }
}
