// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* ConnectionUtility.cs -- разнообразные методы для работы с подключением
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Linq;
using System.Text;

using AM.Configuration;

using ManagedIrbis.Providers;

using Microsoft.Extensions.Configuration;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Разнообразные методы для работы с подключением.
    /// </summary>
    public static class ConnectionUtility
    {
        #region Constants

        /// <summary>
        /// Имя по умолчанию для файла, содержащего настройки подключения.
        /// </summary>
        public const string DefaultConnectionFileName = "connection.irbis";

        #endregion

        #region Public data

        /// <summary>
        /// Строка подключения по умолчанию.
        /// </summary>
        public static string? DefaultConnectionString { get; set; }

        /// <summary>
        /// Допустимые коды для чтения записей с сервера.
        /// </summary>
        public static readonly int[] GoodCodesForReadRecord = { -201, -600, -602, -603 };

        /// <summary>
        /// Допустимые коды для чтения терминов с сервера.
        /// </summary>
        public static readonly int[] GoodCodesForReadTerms = { -202, -203, -204 };

        /// <summary>
        /// Стандартные наименования для ключа строки подключения
        /// к серверу ИРБИС64.
        /// </summary>
        public static readonly string[] StandardConnectionStrings =
            {
                "irbis-connection",
                "irbis-connection-string",
                "irbis64-connection",
                "irbis64",
                "connection-string",
                "connectionString"
            };

        #endregion

        #region Public methods

        /// <summary>
        /// Удаление с сервера заданных файлов.
        /// </summary>
        /// <remarks>
        /// <para>Принимает подстановочные символы '*' и '?'.</para>
        /// <para>Requires server version 2010.1 or newer.</para>
        /// </remarks>
        public static void DeleteAnyFile
            (
                this SyncConnection connection,
                string fileName
            )
        {
            // connection.RequireServerVersion("2010.1", true);

            connection.FormatRecord
                (
                    "&uf('+9K" + fileName + "')",
                    1
                );
        }

        /// <summary>
        /// Получаем строку подключения из стандартного провайдера конфигурации.
        /// (проще говоря, из <c>appsettings.json</c>)
        /// </summary>
        public static string? GetConfiguredConnectionString
            (
                IConfiguration configuration
            )
        {
            string? result = null;

            foreach (var key in StandardConnectionStrings)
            {
                result = configuration[key];
                if (!string.IsNullOrEmpty(result))
                {
                    break;
                }
            }

            if (result is not null)
            {
                result = IrbisUtility.DecryptConnectionString
                    (
                        result,
                        null
                    );
            }

            return result;

        } // method GetConfiguredConnectionString

        /// <summary>
        /// Получаем строку подключения в AppSettings.
        /// </summary>
        public static string? GetStandardConnectionString()
        {
            if (!ReferenceEquals(DefaultConnectionString, null))
            {
                return DefaultConnectionString;
            }

            var candidate = ConfigurationUtility.FindSetting
                (
                    StandardConnectionStrings
                );
            if (string.IsNullOrEmpty(candidate))
            {
                return candidate;
            }

            var result = IrbisUtility.DecryptConnectionString
                (
                    candidate,
                    null
                );

            return result;

        } // method GetStandardConnectionString

        /// <summary>
        /// Получаем уже подключенного клиента.
        /// </summary>
        /// <exception cref="IrbisException">
        /// Если строка подключения в app.settings не найдена.
        /// </exception>
        public static ISyncProvider GetConnectionFromConfig()
        {
            var connectionString = GetStandardConnectionString();
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new IrbisException
                    (
                        "Connection string not specified!"
                    );
            }

            ISyncProvider result = ConnectionFactory.Shared.CreateSyncConnection();
            result.ParseConnectionString(connectionString);

            return result;

        } // method GetConnectionFromConfig

        /// <summary>
        /// Получаем строку подключения из (возможно, несуществующего)
        /// файла с настройками.
        /// </summary>
        /// <returns>Валидная строка подключения либо <c>null</c>.</returns>
        public static string? GetConnectionStringFromFile
            (
                string fileName = DefaultConnectionFileName
            )
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            var result = File.ReadLines(fileName, Encoding.UTF8).FirstOrDefault()?.Trim();

            if (!string.IsNullOrEmpty(result))
            {
                result = IrbisUtility.DecryptConnectionString
                (
                    result,
                    null
                );
            }

            return result;

        } // method GetConnectionStringFromFile

        /// <summary>
        /// Получаем подключение из файла.
        /// </summary>
        /// <param name="fileName">Имя файла со строкой подключения.</param>
        /// <returns>Настроенный клиент.</returns>
        public static ISyncProvider GetConnectionFromFile
            (
                string fileName = DefaultConnectionFileName
            )
        {
            var connectionString = GetConnectionStringFromFile(fileName);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new IrbisException($"Can't get connection string from file {fileName}");
            }

            ISyncProvider result = ConnectionFactory.Shared.CreateSyncConnection();
            result.ParseConnectionString(connectionString);

            return result;

        } // method GetConnectionFromFile

        /// <summary>
        /// Разбор строки подключения.
        /// </summary>
        public static void ParseConnectionString
            (
                this IConnectionSettings connection,
                string? connectionString
            )
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return;
            }

            var settings = new ConnectionSettings();
            settings.ParseConnectionString(connectionString);
            settings.Apply(connection);

        } // method ParseConnectionString

        /// <summary>
        /// Разбор строки подключения.
        /// </summary>
        public static void ParseConnectionString
            (
                this IIrbisProvider provider,
                string? connectionString
            )
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return;
            }

            var settings = new ConnectionSettings();
            settings.ParseConnectionString(connectionString);
            settings.Apply(provider);

        } // method ParseConnectionString

        /// <summary>
        /// Разбор строки подключения.
        /// </summary>
        public static void ParseConnectionString(this IAsyncConnection connection, string? connectionString) =>
            ParseConnectionString((IAsyncProvider) connection, connectionString);

        /// <summary>
        /// Разбор строки подключения.
        /// </summary>
        public static void ParseConnectionString(this ISyncConnection connection, string? connectionString) =>
            ParseConnectionString((ISyncProvider) connection, connectionString);

        #endregion

    } // class ConnectionUtility

} // namespace ManagedIrbis
