// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CommandLineUtility.cs -- утилиты для работы с командной строкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

#endregion

#nullable enable

namespace ManagedIrbis.CommandLine;

//
// Для настройки параметров подключения из командной строки
// предусмотрены следующие опции:
//
// --host          имя или адрес хоста с сервером ИРБИС64
// --port          номер порта, который прослушивает сервер
// --username      имя пользователя (не чувствительно к регистру)
// --password      пароль (чувствительно к регистру символов!)
// --database      имя базы данных
// --workstation   однобуквенный код АРМ
//
// Остальные опции и аргументы игнорируются и могут использоваться
// в других модулях программы.
//
// Параметры подключения можно задать в строке окружения
// IRBIS64_CONNECTION одним из двух способов: либо как имитацию
// командной строки:
//
// "--host:testHost --port:5555 --username:librarian"
//
// либо в формате строки подключения
//
// "host=testHost;port=5555;username=librarian"
//
// Оба способа эквивалентны, программа сама определит,
// который из двух используется.
//

/// <summary>
/// Утилиты для работы с командной строкой,
/// специфичные для ИРБИС.
/// </summary>
public static class CommandLineUtility
{
    #region Constants

    /// <summary>
    /// Имя по умолчанию для переменной окружения,
    /// содержащей параметры подключения к серверу ИРБИС64.
    /// </summary>
    public const string DefaultIrbisEnvironment = "IRBIS64_CONNECTION";

    #endregion

    #region Public methods

    /// <summary>
    /// Получение строки подключения из переменных окружения.
    /// </summary>
    /// <param name="environmentName">Имя переменной окружения.</param>
    /// <returns>Строка подключения либо <c>null</c>.</returns>
    public static string? GetConnectionStringFromEnvironment (string? environmentName = DefaultIrbisEnvironment) =>
        environmentName is null ? null : Environment.GetEnvironmentVariable (environmentName);

    /// <summary>
    /// Настройки для подключения к серверу.
    /// </summary>
    public static RootCommand GetRootCommand()
    {
        var result = new RootCommand
        {
            new Option<string>
                (
                    "--host",
                    "host address"
                ),

            new Option<ushort>
                (
                    "--port",
                    "port number"
                ),

            new Option<string>
                (
                    "--username",
                    "user name"
                ),

            new Option<string>
                (
                    "--password",
                    "user password"
                ),

            new Option<string>
                (
                    "--workstation",
                    "workstation kind"
                ),

            new Option<string>
                (
                    "--database",
                    "initial catalog"
                )
        };

        return result;
    }

    /// <summary>
    /// Настройка синхронного подключения согласно опциям
    /// командной строки.
    /// </summary>
    /// <param name="connection">Подключение, подлежащее настройке.</param>
    /// <param name="arguments">Аргументы командной строки.</param>
    public static void ConfigureConnectionFromCommandLine
        (
            IConnectionSettings connection,
            string[] arguments
        )
    {
        var rootCommand = GetRootCommand();
        rootCommand.SetHandler
            (
                (ConnectionSettings settings) => settings.Apply (connection)
            );

        var parser = new CommandLineBuilder (rootCommand).Build();
        parser.Invoke (arguments);
    }

    /// <summary>
    /// Получение настроек подключения из строки окружения.
    /// </summary>
    /// <param name="settings">Настройки подключения.</param>
    /// <param name="environmentValue">Значение строки окружения.</param>
    /// <returns>Настройки подключения.</returns>
    public static void ParseEnvironment
        (
            IConnectionSettings settings,
            string? environmentValue
        )
    {
        if (string.IsNullOrEmpty (environmentValue))
        {
            return;
        }

        if (environmentValue.Contains ('='))
        {
            //
            // Если строка содержит символ '=', то это строка подключения.
            // Разбирать ее нужно соответственно.
            //

            // TODO: дешифровать строку подключения, если необходимо

            settings.ParseConnectionString (environmentValue);
        }

        var rootCommand = GetRootCommand();
        rootCommand.SetHandler
            (
                (ConnectionSettings theSettings) => theSettings.Apply (settings)
            );

        var parser = new CommandLineBuilder (rootCommand).Build();
        parser.Invoke (environmentValue);
    }

    /// <summary>
    /// Настройка синхронного подключения согласно опциям
    /// командной строки.
    /// </summary>
    /// <param name="connection">Подключение, подлежащее настройке.</param>
    /// <param name="environmentName">Имя переменной окружения.</param>
    public static void ConfigureConnectionFromEnvironment
        (
            IConnectionSettings connection,
            string? environmentName = DefaultIrbisEnvironment
        )
    {
        var environmentValue = GetConnectionStringFromEnvironment (environmentName);
        if (!string.IsNullOrEmpty (environmentValue))
        {
            ParseEnvironment (connection, environmentValue);
        }
    }

    #endregion
}
