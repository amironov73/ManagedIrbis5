// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using AM;
using AM.Collections;
using AM.IO;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

using Microsoft.AspNetCore.Mvc;

#endregion

namespace Opac2023.Controllers;

[Route ("api")]
[ApiController]
public sealed class ApiController
    : Controller
{
    #region Construction

    public ApiController
        (
            IConfiguration configuration,
            ILogger<ApiController> logger
        )
    {
        _configuration = configuration;
        _logger = logger;
    }

    #endregion

    #region Private members

    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    private SyncConnection GetConnection()
    {
        var connectionString = _configuration["irbis-connection"].ThrowIfNullOrEmpty();
        var result = ConnectionFactory.Shared.CreateSyncConnection();
        result.ParseConnectionString (connectionString);
        result.Connect();

        return result;
    }

    #endregion

    [HttpGet]
    public IActionResult Index()
    {
        return Ok ("No command specified");
    }

    [HttpGet ("db")]
    public IActionResult Db()
    {
        _logger.LogInformation ("List databases");

        using var connection = GetConnection();
        var databases = connection.ListDatabases();

        return new JsonResult (databases);
    }

    [HttpGet ("scenarios/{database?}")]
    public IActionResult Scenarios
        (
            string? database
        )
    {
        _logger.LogInformation ("Scenarios for {Database}", database);

        using var connection = GetConnection();
        IniFile iniFile;
        string? text = null;
        if (!string.IsNullOrEmpty (database))
        {
            var specification = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = connection.EnsureDatabase (database),
                FileName = database + ".ini"
            };
            text = connection.ReadTextFile (specification);
        }

        if (string.IsNullOrEmpty (text))
        {
            _logger.LogInformation ("Scenarios: common");
            iniFile = connection.IniFile.ThrowIfNull();
        }
        else
        {
            _logger.LogInformation ("Scenarios: specific");
            iniFile = new IniFile();
            iniFile.Read (new StringReader (text));
        }

        var result = SearchScenario.ParseIniFile (iniFile);
        if (result.IsNullOrEmpty())
        {
            _logger.LogInformation ("Scenarios: fallback");
            iniFile = connection.IniFile.ThrowIfNull();
            result = SearchScenario.ParseIniFile (iniFile);
        }

        return new JsonResult (result);
    }

    [HttpGet ("brief/{database}/{expression}")]
    public IActionResult SearchBrief
        (
            string database,
            string expression
        )
    {
        _logger.LogInformation ("Search {Database}: {Expression}", database, expression);

        using var connection = GetConnection();
        var parameters = new SearchParameters
        {
            Database = connection.EnsureDatabase (database),
            Format = IrbisFormat.Brief,
            Expression = expression
        };
        var found = connection.Search (parameters);
        if (found is null)
        {
            return new JsonResult (Array.Empty<FoundItem>());
        }

        return new JsonResult (found);
    }
}
