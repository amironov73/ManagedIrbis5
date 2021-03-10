# ManagedIrbis5

Client framework for IRBIS64 system ported to .NET 5

Currently supports:

* Windows 7/10 x64
* MacOS 10.14
* .NET Core 3.0 SDK

```c#
using System;
using System.Threading.Tasks;

using ManagedIrbis;

using static System.Console;

#nullable enable

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        try
        {
            using var connection = ConnectionFactory.Default
                .CreateConnection();

            connection.Host = "127.0.0.1";
            connection.Username = "librarian";
            connection.Password = "secret";

            var success = await connection.ConnectAsync();
            if (!success)
            {
                Error.WriteLine("Can't connect");
                return 1;
            }

            WriteLine("Successfully connected");

            var version = await connection.GetServerVersionAsync();
            WriteLine(version);

            var processes = await connection.ListProcessesAsync();
            WriteLine("Processes: " + string.Join<ProcessInfo>(" | ", processes));

            var maxMfn = await connection.GetMaxMfnAsync();
            WriteLine($"Max MFN={maxMfn}");

            var found = await connection.SearchAsync(Search.Keyword("бетон$"));
            WriteLine("Found: " + string.Join<int>(", ", found));

            await connection.NopAsync();
            WriteLine("NOP");

            var record = await connection.ReadRecordAsync(1);
            WriteLine($"ReadRecord={record?.FM(200, 'a')}");

            var formatted = await connection.FormatRecordAsync("@brief", 1);
            WriteLine($"Formatted={formatted}");

            var files = await connection.ListFilesAsync("2.IBIS.*.mnu");
            WriteLine("Files: " + string.Join(",", files));

            var fileText = await connection.ReadTextFileAsync("2.IBIS.brief.pft");
            WriteLine($"BRIEF: {fileText}");
            WriteLine();

            await connection.DisposeAsync();
            WriteLine("Successfully disconnected");
        }
        catch (Exception exception)
        {
            WriteLine(exception);
            return 1;
        }

        return 0;
    }
}
```

### Build status

[![Build status](https://img.shields.io/appveyor/ci/AlexeyMironov/managedirbis5.svg)](https://ci.appveyor.com/project/AlexeyMironov/managedirbis5/)
[![Build status](https://api.travis-ci.org/amironov73/ManagedIrbis5.svg)](https://travis-ci.org/amironov73/ManagedIrbis5/)
[![GitHub Action](https://github.com/amironov73/ManagedIrbis5/workflows/CI/badge.svg)](https://github.com/amironov73/ManagedIrbis5/actions)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Famironov73%2FManagedIrbis5.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2Famironov73%2FManagedIrbis5?ref=badge_shield)

### License

[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Famironov73%2FManagedIrbis5.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2Famironov73%2FManagedIrbis5?ref=badge_large)

### Documentation (in russian)

[![Badge](https://readthedocs.org/projects/managedirbis5/badge/)](https://managedirbis5.readthedocs.io/) 

