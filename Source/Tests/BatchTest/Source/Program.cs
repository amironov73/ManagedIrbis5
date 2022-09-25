// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

#region Using directives

using System;

using ManagedIrbis;
using ManagedIrbis.Batch;

#endregion

#nullable enable

namespace BatchTest;

internal static class Program
{
    static void Main()
    {
        var connectionString = "user=librarian;password=secret;";

        try
        {
            using var connection = new SyncConnection();
            connection.ParseConnectionString (connectionString);
            connection.Connect();

            var batch = BatchRecordReader.Interval
                (
                    connection,
                    1,
                    1000
                );

            var index = 0;
            foreach (var record in batch)
            {
                var title = record.FM (200, 'a');
                Console.WriteLine ($"{++index} => {title}");
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception);
        }
    }
}
