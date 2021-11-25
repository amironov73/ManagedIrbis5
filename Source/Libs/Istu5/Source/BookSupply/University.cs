// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* Universtiy.cs -- доступ к данным об университете
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Istu.BookSupply.Interfaces;

using LinqToDB.Data;

using ManagedIrbis;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace Istu.BookSupply
{
    /// <summary>
    /// Доступ к данным об университете
    /// (data access level).
    /// </summary>
    public sealed class University
        : IServiceProvider
    {
        #region Properties

        /// <summary>
        /// Используемая конфигурация.
        /// </summary>
        public IConfiguration Configuration => _configuration;

        /// <summary>
        /// Используемый логгер.
        /// </summary>
        public ILogger Logger => _logger;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public University
            (
                IServiceProvider serviceProvider,
                IConfiguration configuration,
                string? connectionString = null
            )
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = serviceProvider.GetRequiredService<ILogger<University>>();

            _logger.LogTrace (nameof (University) + "::Constructor");

            _booksupplyConnectionString = connectionString ?? _configuration["booksupply"];
        }

        #endregion

        #region Private members

        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly string _booksupplyConnectionString;
        private readonly ILogger _logger;

        #endregion

        #region Public methods

        /// <summary>
        /// Подключается к базе <c>booksupply</c>.
        /// </summary>
        public DataConnection GetBookSupply() => SupplyUtility.GetMsSqlConnection (_booksupplyConnectionString);

        /// <summary>
        /// Запрос интерфейса электронного каталога.
        /// </summary>
        public ICatalog GetCatalog() => _serviceProvider.GetRequiredService<ICatalog>();

        /// <summary>
        /// Форматирование записи.
        /// </summary>
        public string? FormatRecord
            (
                int mfn,
                string? format = null
            )
        {
            using var catalog = GetCatalog();

            return catalog.FormatRecord (mfn, format);
        } // method FormatRecord

        /// <summary>
        /// Перечисление терминов словаря с указанным префиксом.
        /// </summary>
        public string[] ListTerms
            (
                string prefix
            )
        {
            using var catalog = GetCatalog();

            return catalog.ListTerms (prefix);
        } // method ListTerms

        /// <summary>
        /// Чтение записи по ее MFN.
        /// </summary>
        public Record? ReadRecord
            (
                int mfn
            )
        {
            using var catalog = GetCatalog();

            return catalog.ReadRecord (mfn);
        } // method ReadRecord

        /// <summary>
        /// Прямой поиск записей (по поисковому словарю).
        /// </summary>
        public int[] SearchRecords
            (
                string expression
            )
        {
            using var catalog = GetCatalog();

            return catalog.SearchRecords (expression);
        } // method SearchRecords

        #endregion

        #region IServiceProvider members

        /// <inheritdoc cref="IServiceProvider.GetService"/>
        public object? GetService (Type serviceType)
        {
            return _serviceProvider.GetService (serviceType);
        }

        #endregion

    } // class University

} // namespace Istu.BookSupply
