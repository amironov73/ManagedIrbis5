// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* NuGetPackageDownloader.cs -- умеет скачивать пакеты NuGet
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;

using NuGet.Common;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

using FileUtility = AM.IO.FileUtility;

#endregion

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

    private static void _CleanupDownload
        (
            string? directory,
            string fileName
        )
    {
        // что-то пошло не так, удаляем неудачно скачанный файл
        FileUtility.DeleteIfExists (fileName);

        // да и всю папку заодно
        if (Directory.Exists (directory))
        {
            Directory.Delete (directory, true);

            var packageDirectory = Path.GetDirectoryName (directory);
            if (!string.IsNullOrEmpty (packageDirectory))
            {
                var containsFiles = Directory.GetFiles
                    (
                        packageDirectory,
                        "*",
                        SearchOption.AllDirectories
                    );
                if (!containsFiles.Any())
                {
                    Directory.Delete (packageDirectory);
                }
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Скачивание пакета.
    /// </summary>
    public string? DownloadPackage
        (
            PackageIdentity package
        )
    {
        Sure.NotNull (package);

        var fileName = package.HasVersion
            ? Path.Combine (TargetFolder, package.Id,
                package.Version.ToString(), package.ToString())
            : Path.Combine (TargetFolder, package.Id, package.ToString());
        fileName += ".nupkg";

        if (File.Exists (fileName))
        {
            return fileName;
        }

        var directory = Path.GetDirectoryName (fileName);
        if (directory is not null && !Directory.Exists (directory))
        {
            Directory.CreateDirectory (directory);
        }

        bool success;
        using (var stream = File.Create (fileName))
        {
            success =_resource.CopyNupkgToStreamAsync
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

        if (!success)
        {
            _CleanupDownload (directory, fileName);
            return null;
        }

        var fileInfo = new FileInfo (fileName);
        if (!fileInfo.Exists || fileInfo.Length < 100)
        {
            _CleanupDownload (directory, fileName);
            return null;
        }

        return fileName;
    }

    /// <summary>
    /// Извлечение файлов из пакета (для организации локального кеша).
    /// </summary>
    public void ExtractPackage
        (
            string packageFile
        )
    {
        Sure.FileExists (packageFile);

        var packageDirectory = Path.GetDirectoryName (packageFile) ?? ".";
        using var archiveReader = new PackageArchiveReader (packageFile);
        var files = archiveReader.GetFiles();
        foreach (var file in files)
        {
            if (file == "[Content_Types].xml"
                || file.StartsWith ("_rels/") || file.StartsWith ("_rels\\")
                || file.StartsWith ("package/") || file.StartsWith ("package\\"))
            {
                continue;
            }

            var target = Path.Combine (packageDirectory, file);
            archiveReader.ExtractFile (file, target, _logger);
        }

        dynamic metadataHolder = new ExpandoObject();
        metadataHolder.version = 2;
        metadataHolder.contentHash = archiveReader.GetContentHash (_cancellationToken);
        metadataHolder.source = _repository.PackageSource.Source;
        var options = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        var metadataJson = JsonSerializer.Serialize (metadataHolder, options);
        var metadataFile = Path.Combine (packageDirectory, ".nupkg.metadata");
        File.WriteAllText (metadataFile, metadataJson);

        var packageBytes = File.ReadAllBytes (packageFile);
        var shaBytes = SHA512.HashData (packageBytes);
        var shaText = Convert.ToBase64String (shaBytes);
        var shaFile = packageFile + ".sha512";
        File.WriteAllText (shaFile, shaText);
    }

    /// <summary>
    /// Получение актуальной версии пакета.
    /// </summary>
    public NuGetVersion? GetLatestVersion
        (
            string packageId
        )
    {
        Sure.NotNullNorEmpty (packageId);

        var found = _resource.GetAllVersionsAsync
            (
                packageId,
                _cache,
                _logger,
                _cancellationToken
            )
            .GetAwaiter().GetResult();

        if (found is null)
        {
            return null;
        }

        NuGetVersion? result = null;
        foreach (var version in found)
        {
            if (result is null)
            {
                result = version;
            }
            else
            {
                if (version.CompareTo (result) > 0)
                {
                    result = version;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Получение зависимостей из пакета.
    /// </summary>
    public List<PackageIdentity> ListDependencies
        (
            string packageFile
        )
    {
        Sure.FileExists (packageFile);

        var result = new List<PackageIdentity>();
        var fileInfo = new FileInfo (packageFile);
        if (fileInfo.Length < 100)
        {
            // подозрительно маленький файл, ну его нафиг!
            return result;
        }

        using var input = new FileStream (packageFile, FileMode.Open);
        using var reader = new PackageArchiveReader (input);
        var nuspec = reader.NuspecReader;

        foreach (var dependencyGroup in nuspec.GetDependencyGroups())
        {
            foreach (var package in dependencyGroup.Packages)
            {
                var identity = new PackageIdentity (package.Id, package.VersionRange.MinVersion);
                result.Add (identity);
            }
        }

        return result;
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
