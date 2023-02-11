// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* MssqlShortener.cs -- сокращатель ссылок, работающий поверх MSSQL
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Web.Shortening;

/// <summary>
/// Сокращатель ссылок, работающий поверх MSSQL.
/// </summary>
public sealed class MsSqlShortener
    : ShortenerBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MsSqlShortener
        (
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            string? connectionString = null
        )
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = serviceProvider.GetRequiredService<ILogger<MsSqlShortener>>();

        _logger.LogTrace (nameof (MsSqlShortener) + "::Constructor");

        _connectionString = (connectionString ?? _configuration["links"]).ThrowIfNullOrEmpty();
    }

    #endregion

    #region Private members

    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    private readonly ILogger _logger;

    /// <summary>
    /// Глобальный экземпляр.
    /// </summary>
    private static MsSqlShortener? _instance;
    
    /// <summary>
    /// Подключается к MSSQL.
    /// </summary>
    private static DataConnection GetMsSqlConnection
        (
            string connectionString
        )
    {
        Sure.NotNullNorEmpty (connectionString);

        try
        {
            var result = SqlServerTools.CreateDataConnection (connectionString);

            return result;
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (MsSqlShortener) + "::" + nameof (GetMsSqlConnection)
                );
            throw;
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение (возможно, нового) экземпляра.
    /// </summary>
    public static MsSqlShortener GetInstance
        (
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            string? connnectionString = null
        )
    {
        return _instance ??= new MsSqlShortener (serviceProvider, configuration, connnectionString);
    }

    /// <summary>
    /// Получение (возможно, нового) экземпляра.
    /// </summary>
    public static MsSqlShortener GetInstance
        (
            string? connnectionString = null
        )
    {
        if (_instance is null)
        {
            var serviceProvider = Magna.Host.Services;
            var configuration = Magna.Configuration;
            _instance = new MsSqlShortener (serviceProvider, configuration, connnectionString);
        }

        return _instance;
    }

    /// <summary>
    /// Подключается к базе <c>links</c>.
    /// </summary>
    public DataConnection ConnectToDatabase() => GetMsSqlConnection (_connectionString);

    /// <summary>
    /// Получает таблицу <c>attendance</c>.
    /// </summary>
    public static ITable<LinkData> GetTable (DataConnection connection) =>
        connection.GetTable<LinkData>();

    #endregion

    #region ILinkShortener members

    /// <inheritdoc cref="ILinkShortener.ShortenLink"/>
    public override string ShortenLink
        (
            string fullLink,
            string? description
        )
    {
        Sure.NotNullNorEmpty (fullLink);

        using var connection = ConnectToDatabase();
        var table = GetTable (connection);
        var linkData = table.FirstOrDefault (row => row.FullLink == fullLink);
        if (linkData is null)
        {
            linkData = new LinkData
            {
                Moment = DateTime.Now,
                FullLink = fullLink,
                ShortLink = CreateShortLink (fullLink),
                Description = description,
                Counter = 0
            };
            connection.Insert (linkData);
        }

        _logger.LogInformation 
            (
                "ShortenLink {FullLink} to {ShortLink}", 
                fullLink, 
                linkData.ShortLink
            );

        return linkData.ShortLink.ThrowIfNullOrEmpty();
    }

    /// <inheritdoc cref="ILinkShortener.GetFullLink"/>
    public override string? GetFullLink
        (
            string shortLink,
            bool count
        )
    {
        Sure.NotNullNorEmpty (shortLink);

        using var connection = ConnectToDatabase();
        var table = GetTable (connection);
        var linkData = table
            .FirstOrDefault (row => row.ShortLink == shortLink);
        if (linkData is null)
        {
            return null;
        }

        if (count)
        {
            linkData.Counter++;
            connection.Update (linkData);
        }

        _logger.LogInformation ("GetFullLink {ShortLink} {Count}", shortLink, count);

        return linkData.ShortLink;
    }

    /// <inheritdoc cref="ILinkShortener.GetLinkData"/>
    public override LinkData? GetLinkData
        (
            string shortLink
        )
    {
        Sure.NotNullNorEmpty (shortLink);

        using var connection = ConnectToDatabase();
        var table = GetTable (connection);

        _logger.LogInformation ("GetLinkData {ShortLink}", shortLink);

        return table.FirstOrDefault (row => row.ShortLink == shortLink);
    }

    /// <inheritdoc cref="ILinkShortener.InsertLink"/>
    public override int InsertLink
        (
            LinkData linkData
        )
    {
        Sure.NotNull (linkData);

        using var connection = ConnectToDatabase();
        linkData.Id = connection.InsertWithInt32Identity (linkData);

        _logger.LogInformation ("InsertLink {ShortLink}", linkData.ShortLink);

        return linkData.Id;
    }

    /// <inheritdoc cref="ILinkShortener.DeleteLink"/>
    public override void DeleteLink
        (
            string shortLink
        )
    {
        Sure.NotNullNorEmpty (shortLink);

        using var connection = ConnectToDatabase();
        var table = GetTable (connection);
        table
            .Where (row => row.ShortLink == shortLink)
            .Delete();

        _logger.LogInformation ("DeleteLinke {ShortLink}", shortLink);
    }

    /// <inheritdoc cref="ILinkShortener.GetAllLinks"/>
    public override LinkData[] GetAllLinks()
    {
        using var connection = ConnectToDatabase();
        var table = GetTable (connection);

        _logger.LogInformation ("GetAllLinks");

        return table.ToArray();
    }

    #endregion
}
