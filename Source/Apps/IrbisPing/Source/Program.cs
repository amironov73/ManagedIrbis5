// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- утилита для проверки подключения к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Diagnostics;

using ManagedIrbis;

#endregion

#nullable enable

namespace IrbisPing;

internal sealed class PingOptions
{
    public string? Host { get; set; }
    public ushort Port { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public int Count { get; set; }

    public PingOptions
        (
            string host,
            ushort port,
            string login,
            string password,
            int count
        )
    {
        Host = host;
        Port = port;
        Login = login;
        Password = password;
        Count = count;
    }
}

internal static class Program
{
    static void DescribeError
        (
            SyncConnection connection
        )
    {
        var errorCode = connection.LastError;
        var errorDescription = IrbisException.GetErrorDescription(errorCode);
        Console.WriteLine($"Can't connect: code={errorCode}, description={errorDescription}");
    }

    static bool TryToConnect
        (
            PingOptions options
        )
    {
        try
        {
            using var connection = ConnectionFactory.Shared.CreateSyncConnection();
            connection.Host = options.Host!.Trim();
            connection.Port = options.Port;
            connection.Username = options.Login!.Trim();
            connection.Password = options.Password!.Trim();

            if (!connection.Connect())
            {
                DescribeError(connection);
                return false;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            if (!connection.NoOperation())
            {
                DescribeError(connection);
                return false;
            }
            var elapsed = stopwatch.Elapsed.TotalMilliseconds;
            Console.WriteLine($"{elapsed:0} ms");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            return false;
        }

        return true;
    }

    static void Run
        (
            PingOptions options
        )
    {
        while (string.IsNullOrEmpty(options.Host))
        {
            Console.Write("Введите хост: ");
            options.Host = Console.ReadLine()?.Trim();
        }

        while (string.IsNullOrEmpty(options.Login))
        {
            Console.Write("Введите логин: ");
            options.Login = Console.ReadLine()?.Trim();
        }

        while (string.IsNullOrEmpty(options.Password))
        {
            Console.Write("Введите пароль: ");
            options.Password = Console.ReadLine()?.Trim();
        }

        var success = 0;
        var failure = 0;
        for (var i = 0; i < options.Count; i++)
        {
            Console.Write($"{i + 1}: ");
            if (TryToConnect(options))
            {
                ++success;
            }
            else
            {
                ++failure;
            }
        }

        Console.WriteLine($"Success: {success}, failure: {failure}");

        Environment.ExitCode = success != 0 ? 0 : 1;
    }

    static void Main
        (
            string[] args
        )
    {
        var hostOption = new Option<string>
            (
                "--host",
                () => "127.0.0.1",
                "хост ИРБИС64"
            );
        var portOption = new Option<ushort>
            (
                "--port",
                () => 6666,
                "порт ИРБИС64"
            );
        var loginOption = new Option<string>
            (
                "--login",
                "логин пользователя (будет запрошен, если не задан в командной строке)"
            );
        var passwordOption = new Option<string>
            (
                "--password",
                "пароль (будет запрошен, если не задан в командной строке)"
            );
        var countOption = new Option<int>
            (
                "--count",
                () => 1,
                "количество попыток"
            );
        var rootCommand = new RootCommand("IrbisPing")
        {
            hostOption,
            portOption,
            loginOption,
            passwordOption,
            countOption
        };
        rootCommand.Description = "Проверка возможности подключения к серверу ИРБИС64";
        rootCommand.SetHandler ((Action<PingOptions>) Run);

        new CommandLineBuilder(rootCommand)
            .UseDefaults()
            .Build()
            .Invoke(args);
    }
}
