// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ConnectionUtility.cs -- разнообразные методы для работы с подключением
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Threading;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Разнообразные методы для работы с подключением.
    /// </summary>
    public static class ConnectionUtility
    {
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
        public static string[] StandardConnectionStrings =
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
        /// Получаем строку подключения в app.settings.
        /// </summary>
        public static string GetStandardConnectionString()
        {
            if (!ReferenceEquals(DefaultConnectionString, null))
            {
                return DefaultConnectionString;
            }

            /*

            var candidate = ConfigurationUtility.FindSetting
            (
                ListStandardConnectionStrings()
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

            */

            throw new NotImplementedException();
        } // method GetStandardConnectionString

        /// <summary>
        /// Получаем уже подключенного клиента.
        /// </summary>
        /// <exception cref="IrbisException">
        /// Если строка подключения в app.settings не найдена.
        /// </exception>
        public static ISyncIrbisProvider GetConnectionFromConfig()
        {
            var connectionString = GetStandardConnectionString();
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new IrbisException
                    (
                        "Connection string not specified!"
                    );
            }

            var result = ConnectionFactory.Shared.CreateSyncConnection();
            result.ParseConnectionString(connectionString);

            return result;
        } // method GetConnectionFromConfig


        /// <summary>
        /// Разбор строки подключения.
        /// </summary>
        public static void ParseConnectionString
            (
                this SyncConnection connection,
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
        }

        /// <summary>
        /// Разбор строки подключения.
        /// </summary>
        public static void ParseConnectionString
            (
                this AsyncConnection connection,
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
        }

        #endregion

    } // class ConnectionUtility

} // namespace ManagedIrbis
