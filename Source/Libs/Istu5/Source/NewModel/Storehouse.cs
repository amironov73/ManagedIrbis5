﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Storehouse.cs -- программный интерфейс книговыдачи
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

using AM;

using Istu.NewModel.Implementation;
using Istu.NewModel.Interfaces;

using LinqToDB.Data;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

namespace Istu.NewModel;

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

        _logger.LogTrace (nameof (Storehouse) + "::Constructor");

        _kladovkaConnectionString =
            (
                connectionString ?? _configuration["kladovka"]
            )
            .ThrowIfNullOrEmpty();
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
    /// Создание менеджера посещений.
    /// </summary>
    public IAttendanceManager CreateAttendanceManager() =>
        new AttendanceManager (this);

    /// <summary>
    /// Создание менеджера книговыдачи.
    /// </summary>
    public ILoanManager CreateLoanManager() => new LoanManager (this);

    /// <summary>
    /// Создание менеджера операторов.
    /// </summary>
    public IOperatorManager CreateOperatorManager() =>
        new OperatorManager (this);

    /// <summary>
    /// Создание менеджера заказов.
    /// </summary>
    public IOrderManager CreateOrderManager() => new OrderManager (this);

    /// <summary>
    /// Создание менеджера читателей.
    /// </summary>
    public IReaderManager CreateReaderManager() => new ReaderManager (this);

    /// <summary>
    /// Подключается к базе <c>kladovka</c>.
    /// </summary>
    public DataConnection GetKladovka() =>
        IstuUtility.GetMsSqlConnection (_kladovkaConnectionString);

    #endregion

    #region IServiceProvider members

    /// <inheritdoc cref="IServiceProvider.GetService"/>
    public object? GetService (Type serviceType) => _serviceProvider.GetService (serviceType);

    #endregion
}
