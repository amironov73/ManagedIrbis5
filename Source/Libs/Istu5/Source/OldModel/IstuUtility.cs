// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* IstuUtility.cs -- вспомогательные методы для работы с базой книговыдачи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using Istu.OldModel.Implementation;
using Istu.OldModel.Interfaces;

using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;

using Microsoft.Extensions.DependencyInjection;

#endregion

#nullable enable

namespace Istu.OldModel
{
    /// <summary>
    /// Вспомогательные методы для работы с базой книговыдачи.
    /// </summary>
    public static class IstuUtility
    {
        #region Public methods

        /// <summary>
        /// Добавление сервисов книговыдачи.
        /// </summary>
        public static IServiceCollection AddOldModel
            (
                this IServiceCollection services
            )
        {
            services.AddTransient<Storehouse>();

            services.AddTransient<IAttendanceManager, AttendanceManager>();
            services.AddTransient<IOperatorManager, OperatorManager>();
            services.AddTransient<IReaderManager, ReaderManager>();
            services.AddTransient<ILoanManager, LoanManager>();
            services.AddTransient<IOrderManager, OrderManager>();

            return services;

        } // method AddOldModel

        /// <summary>
        /// Подключается к MSSQL.
        /// </summary>
        public static DataConnection GetMsSqlConnection
            (
                string connectionString
            )
        {
            try
            {
                var result = SqlServerTools.CreateDataConnection(connectionString);

                return result;
            }
            catch (Exception exception)
            {
                Magna.TraceException(nameof(IstuUtility) + "::" + nameof(GetMsSqlConnection), exception);
                throw;
            }

        } // method GetDatabaseConnection

        /// <summary>
        /// Получает таблицу <c>attendance</c>.
        /// </summary>
        public static ITable<Attendance> GetAttendances (this DataConnection connection) =>
            connection.GetTable<Attendance>();

        /// <summary>
        /// Получает таблицу <c>operators</c>.
        /// </summary>
        public static ITable<Operator> GetOperators (this DataConnection connection) =>
            connection.GetTable<Operator>();

        /// <summary>
        /// Получает таблицу <c>orders</c>.
        /// </summary>
        public static ITable<Order> GetOrders (this DataConnection connection) =>
            connection.GetTable<Order>();

        /// <summary>
        /// Получает таблицу <c>readers</c>.
        /// </summary>
        public static ITable<Reader> GetReaders (this DataConnection connection) =>
            connection.GetTable<Reader>();

        /// <summary>
        /// Получает таблицу <c>podsob</c>.
        /// </summary>
        public static ITable<Podsob> GetPodsob (this DataConnection connection) =>
            connection.GetTable<Podsob>();

        /// <summary>
        /// Получает таблицу <c>translator</c>.
        /// </summary>
        public static ITable<Translator> GetTranslator (this DataConnection connection) =>
            connection.GetTable<Translator>();

        /// <summary>
        /// Перечень известных событий книговыдачи.
        /// </summary>
        public static string[] KnownAttendanceEvents() => new[]
        {
            "Возврат",
            "Выдача",
            "Посещение",
            "Продление",
            "Приписка штрих-кода",
            "Регистрация",
            "СМС",
            "Списание"
        };

        /// <summary>
        /// Перевод кода типа посещения в человеко-читаемый формат.
        /// См. поле <c>type</c> в таблице <c>Attendance</c>.
        /// </summary>
        public static string TranslateAttendanceCode (char code) => code switch
            {
                'a' or 'A' => "Посещение",
                'g' or 'G' => "Выдача",
                'r' or 'R' => "Возврат",
                'p' or 'P' => "Продление",
                'w' or 'W' => "Приписка штрих-кода",
                'd' or 'D' => "Списание",
                '1' => "Регистрация",
                's' or 'S' => "СМС",
                _ => code.ToString()

            }; // switch

        #endregion

    } // class IstuUtility

} // namespace Istu.OldModel
