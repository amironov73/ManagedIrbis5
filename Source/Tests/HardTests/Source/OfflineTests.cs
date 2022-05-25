// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.Text;

using AM;

using ManagedIrbis;
using ManagedIrbis.Formatting;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Records;

#endregion

#nullable enable

namespace HardTests;

internal static class OfflineTests
{
    public static void Run
        (
            string[] args,
            string areaSeparator,
            Action<HardFormat, StringBuilder, Record> action
        )
    {
        Sure.NotNull (action);

        try
        {
            Console.WriteLine (Infrastructure.TestDataPath);
            Console.WriteLine (Infrastructure.Irbis64RootPath);

            using var provider = Infrastructure.GetProvider();
            var maxMfn = provider.GetMaxMfn();
            Console.WriteLine ($"Max MFN={maxMfn}");

            var formatter = new HardFormat
                (
                    provider
                );
            var builder = new StringBuilder();
            builder.EnsureCapacity (4096);
            formatter.AreaSeparator = areaSeparator;

            for (var mfn = 1; mfn < maxMfn; mfn++)
            {
                var parameters = new ReadRecordParameters { Mfn = mfn };
                var record = provider.ReadRecord<Record> (parameters);
                if (record is not null)
                {
                    builder.Clear();

                    action (formatter, builder, record);

                    var formatted = builder.ToString();
                    Console.WriteLine ($"{mfn}");
                    Console.WriteLine ($"{formatter.Worksheet (record)}");
                    Console.WriteLine (formatted);
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine ("ALL DONE");
            Console.WriteLine();
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception);
        }
    }
}
