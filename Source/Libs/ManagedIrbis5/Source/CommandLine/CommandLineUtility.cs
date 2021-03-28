// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CommandLineUtility.cs -- утилиты для работы с командной строкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

#endregion

#nullable enable

namespace ManagedIrbis.CommandLine
{
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
        #region Public methods

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
        } // method GetRootCommand

        /// <summary>
        /// Настройка синхронного подключения согласно опциям
        /// командной строки.
        /// </summary>
        /// <param name="connection">Подключение, подлежащее настройке.</param>
        /// <param name="arguments">Аргументы командной строки.</param>
        public static void ConfigureConnectionFromCommandLine
            (
                IIrbisConnectionSettings connection,
                string[] arguments
            )
        {
            var rootCommand = GetRootCommand();
            rootCommand.Handler = CommandHandler.Create
                (
                    (ConnectionSettings settings) => settings.Apply(connection)
                );

            var parser = new CommandLineBuilder(rootCommand).Build();
            parser.Invoke(arguments);

        } // method ConfigureConnectionFromCommandLine

        /// <summary>
        /// Получение настроек подключения из строки окружения.
        /// </summary>
        /// <param name="environmentValue">Значение строки окружения.</param>
        /// <returns>Настройки подключения.</returns>
        public static ConnectionSettings ParseEnvironment
            (
                string? environmentValue
            )
        {
            var result = new ConnectionSettings();

            if (string.IsNullOrEmpty(environmentValue))
            {
                return result;
            }

            if (environmentValue.Contains('='))
            {
                //
                // Если строка содержит символ '=', то это строка подключения.
                // Разбирать ее нужно соответственно.
                //

                // TODO: дешифровать строку подключения, если необходимо

                result.ParseConnectionString(environmentValue);
            }

            var rootCommand = GetRootCommand();
            rootCommand.Handler = CommandHandler.Create
                (
                    (ConnectionSettings settings) =>
                    {
                        result = settings;
                    }
                );
            var parser = new CommandLineBuilder(rootCommand).Build();
            parser.Invoke(environmentValue);

            return result;
        } // method ParseEnvironment

        /// <summary>
        /// Настройка синхронного подключения согласно опциям
        /// командной строки.
        /// </summary>
        /// <param name="connection">Подключение, подлежащее настройке.</param>
        /// <param name="environmentName">Имя переменной окружения.</param>
        public static void ConfigureConnectionFromEnvironment
            (
                IIrbisConnectionSettings connection,
                string? environmentName = default
            )
        {
            environmentName ??= "IRBIS64_CONNECTION";

            var environmentValue = Environment.GetEnvironmentVariable(environmentName);
            var settings = ParseEnvironment(environmentValue);
            settings.Apply(connection);
        } // method ConfigureConnectionFromEnvironment

        #endregion
    } // class CommandLineUtility

} // namespace ManagedIrbis.CommandLine
