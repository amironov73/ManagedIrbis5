// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* SupplyUtility.cs -- вспомогательные методы для работы с базой данных книгообеспеченности
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;

using Istu.BookSupply.Implementation;
using Istu.BookSupply.Interfaces;

using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;

using Microsoft.Extensions.DependencyInjection;

#endregion

#nullable enable

namespace Istu.BookSupply
{
    /// <summary>
    /// Вспомогательные методы для работы с базой данных книгообеспеченности.
    /// </summary>
    public static class SupplyUtility
    {
        #region Public methods

        /// <summary>
        /// Добавление сервисов книгообеспеченности.
        /// </summary>
        public static IServiceCollection AddBookSupply
            (
                this IServiceCollection services
            )
        {
            services.AddTransient<University>();

            services.AddTransient<ICatalog, IrbisCatalog>();

            return services;
        } // method AddBookSupply

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
                var result = SqlServerTools.CreateDataConnection (connectionString);

                return result;
            }
            catch (Exception exception)
            {
                Magna.TraceException (nameof (SupplyUtility) + "::" + nameof (GetMsSqlConnection), exception);
                throw;
            }
        } // method GetDatabaseConnection

        /// <summary>
        /// Получает таблицу <c>book_bidings</c>.
        /// </summary>
        public static ITable<BookBinding> GetBookBindings (this DataConnection connection) =>
            connection.GetTable<BookBinding>();

        /// <summary>
        /// Получает таблицу <c>contingent</c>.
        /// </summary>
        public static ITable<Contingent> GetContingent (this DataConnection connection) =>
            connection.GetTable<Contingent>();

        /// <summary>
        /// Получает таблицу <c>departments</c>.
        /// </summary>
        public static ITable<Department> GetDepartments (this DataConnection connection) =>
            connection.GetTable<Department>();

        /// <summary>
        /// Получает таблицу <c>disciplines</c>.
        /// </summary>
        public static ITable<Discipline> GetDisciplines (this DataConnection connection) =>
            connection.GetTable<Discipline>();

        /// <summary>
        /// Получает таблицу <c>group_bidings</c>.
        /// </summary>
        public static ITable<GroupBinding> GetGroupBindings (this DataConnection connection) =>
            connection.GetTable<GroupBinding>();

        /// <summary>
        /// Получение привязок книг для указанной карточки комплектования.
        /// </summary>
        public static BookBinding[] GetBookBindings (this DataConnection connection, string card) =>
            connection.GetBookBindings().Where (binding => binding.Card == card).ToArray();

        /// <summary>
        /// Получение привязок книг для указанной дисциплины.
        /// </summary>
        public static BookBinding[] GetBookBindings (this DataConnection connection, int discipline) =>
            connection.GetBookBindings().Where (binding => binding.Discipline == discipline).ToArray();

        /// <summary>
        /// Получение привязок групп для указанного шифра группы.
        /// </summary>
        public static GroupBinding[] GetGroupBindings (this DataConnection connection, string group) =>
            connection.GetGroupBindings().Where (binding => binding.Group == group).ToArray();

        /// <summary>
        /// Получение привязок групп для указанной дисциплине.
        /// </summary>
        public static GroupBinding[] GetGroupBindings (this DataConnection connection, int discipline) =>
            connection.GetGroupBindings().Where (binding => binding.Discipline == discipline).ToArray();

        /// <summary>
        /// Количество студентов для указанного факультета.
        /// </summary>
        public static int CountContingentByDepartment (this DataConnection connection, int department) =>
            connection.GetContingent().Where (contingent => contingent.Department == department)
                .Sum (contingent => contingent.Students);

        /// <summary>
        /// Количество студентов для указанной кафедры.
        /// </summary>
        public static int CountContingentByCathedra (this DataConnection connection, int cathedra) =>
            connection.GetContingent().Where (contingent => contingent.Cathedra == cathedra)
                .Sum (contingent => contingent.Students);

        /// <summary>
        /// Количество студентов в группах с указанным кратким шифром.
        /// </summary>
        public static int CountContingentByShortcut (this DataConnection connection, string shortcut) =>
            connection.GetContingent().Where (contingent => contingent.Shortcut == shortcut)
                .Sum (contingent => contingent.Students);

        /// <summary>
        /// Количество студентов в группах с указанным полным шифром.
        /// </summary>
        public static int CountContingentByCode (this DataConnection connection, string name) =>
            connection.GetContingent().Where (contingent => contingent.Name == name)
                .Sum (contingent => contingent.Students);

        #endregion

    } // class SupplyUtility

} // namespace Istu.BookSupply
