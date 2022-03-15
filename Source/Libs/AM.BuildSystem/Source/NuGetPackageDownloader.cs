// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* NuGetPackageDownloader.cs -- умеет скачивать пакеты NuGet
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Threading;

using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

#endregion

#nullable enable

namespace AM.BuildSystem;

/// <summary>
/// Умеет скачивать пакеты NuGet.
/// </summary>
public sealed class NuGetPackageDownloader
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Папка, в которую складываются файлы.
    /// </summary>
    public string TargetFolder { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public NuGetPackageDownloader
        (
            ILogger? logger = null
        )
    {
        _logger = logger ?? NullLogger.Instance;
        TargetFolder = string.Empty; // для успокоения компилятора
        SetTargetFolder ("Packages");

        _cancellationToken = CancellationToken.None;
        _cache = new SourceCacheContext();
        _repository = Repository.Factory.GetCoreV3 ("https://api.nuget.org/v3/index.json");
        _resource = _repository.GetResourceAsync<FindPackageByIdResource>().GetAwaiter().GetResult();
    }

    #endregion

    #region Private members

    private readonly ILogger _logger;
    private readonly CancellationToken _cancellationToken;
    private readonly SourceCacheContext _cache;
    private readonly SourceRepository _repository;
    private readonly FindPackageByIdResource _resource;

    #endregion

    #region Public methods

    /// <summary>
    /// Скачивание пакета.
    /// </summary>
    public bool DownloadPackage
        (
            PackageIdentity package
        )
    {
        Sure.NotNull (package);

        var fileName = package.HasVersion
            ? Path.Combine (TargetFolder, package.Id.ToLowerInvariant(),
                package.Version.ToString(), package.ToString().ToLowerInvariant())
            : Path.Combine (TargetFolder, package.Id.ToLowerInvariant(),
                package.ToString().ToLowerInvariant());
        fileName += ".nupkg";

        if (File.Exists (fileName))
        {
            return true;
        }

        var directory = Path.GetDirectoryName (fileName);
        if (directory is not null && !Directory.Exists (directory))
        {
            Directory.CreateDirectory (directory);
        }

        using var stream = File.Create (fileName);

        return _resource.CopyNupkgToStreamAsync
                (
                    package.Id,
                    package.Version,
                    stream,
                    _cache,
                    _logger,
                    _cancellationToken
                )
            .GetAwaiter()
            .GetResult();
    }

    /// <summary>
    /// Установка папки, в которую складываются пакеты.
    /// </summary>
    public void SetTargetFolder
        (
            string path
        )
    {
        Sure.NotNullNorEmpty (path);

        path = Path.GetFullPath (path);
        if (!Directory.Exists (path))
        {
            Directory.CreateDirectory (path);
        }

        TargetFolder = path;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        // Nothing to do here yet
    }

    #endregion
}
