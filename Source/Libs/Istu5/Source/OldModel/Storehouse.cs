// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Storehouse.cs -- программный интерфейс книговыдачи
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

using LinqToDB.Data;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

namespace Istu.OldModel
{
    /// <summary>
    /// Программный интерфейс книговыдачи.
    /// </summary>
    public sealed class Storehouse
        : IServiceProvider
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Storehouse
            (
                IServiceProvider serviceProvider,
                IConfiguration configuration,
                string? connectionString = null
            )
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = serviceProvider.GetRequiredService<ILogger<Storehouse>>();

            _logger.LogTrace(nameof(Storehouse) + "::Constructor");

            _kladovkaConnectionString = connectionString ?? _configuration["kladovka"];
        }

        #endregion

        #region Private members

        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly string _kladovkaConnectionString;
        private readonly ILogger _logger;

        #endregion

        #region Public methods

        /// <summary>
        /// Подключается к базе <c>kladovka</c>.
        /// </summary>
        public DataConnection GetKladovka() => IstuUtility.GetMsSqlConnection(_kladovkaConnectionString);

        #endregion

        #region IServiceProvider members

        /// <inheritdoc cref="IServiceProvider.GetService"/>
        public object? GetService(Type serviceType) => _serviceProvider.GetService(serviceType);

        #endregion

    } // class Storehouse

} // namespace Istu.OldModel
