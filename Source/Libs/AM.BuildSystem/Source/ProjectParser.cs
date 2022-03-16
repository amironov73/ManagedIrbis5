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

/* ProjectParser.cs -- разбирает файлы csproj
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;

using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

#endregion

#nullable enable

namespace AM.BuildSystem;

/// <summary>
/// Разбирает файлы <c>csproj</c>.
/// </summary>
public static class ProjectParser
{
    #region Private members

    private static void _CollectPackages
        (
            Dictionary<PackageIdentity, object?> dictionary,
            string rootPath,
            string pattern
        )
    {
        var projects = Directory.GetFiles
            (
                rootPath,
                "*.csproj",
                SearchOption.AllDirectories
            );

        foreach (var project in projects)
        {
            var packages = ExtractPackages (project);
            foreach (var package in packages)
            {
                dictionary[package] = null;
            }
        }
    }

    /// <summary>
    /// Чисто для тестирования.
    /// </summary>
    private static void _Main()
    {
        var packages = CollectPackages (@"D:\Projects\ManagedIrbis5");
        packages = packages.OrderBy (package => package.Id).ToList();
        var downloader = new NuGetPackageDownloader();
        var additional = new Dictionary<PackageIdentity, object?>();
        foreach (var package in packages)
        {
            Console.WriteLine (package);
            var fileName = downloader.DownloadPackage (package);
            if (fileName is not null)
            {
                var dependencies = downloader.ListDependencies (fileName);
                foreach (var dependency in dependencies)
                {
                    additional[dependency] = null;
                }
            }
        }

        while (additional.Count != 0)
        {
            var first = additional.Keys.First();
            additional.Remove (first);
            var fileName = downloader.DownloadPackage (first);
            if (fileName is not null)
            {
                var dependencies = downloader.ListDependencies (fileName);
                foreach (var dependency in dependencies)
                {
                    additional[dependency] = null;
                }
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Сбор всех ссылок на пакеты из проектов, расположенных в поддиректориях.
    /// </summary>
    public static List<PackageIdentity> CollectPackages
        (
            string rootPath
        )
    {
        var dictionary = new Dictionary<PackageIdentity, object?>();
        _CollectPackages (dictionary, rootPath, "*.csproj");
        _CollectPackages (dictionary, rootPath, "*.Build.props");
        _CollectPackages (dictionary, rootPath, "*.targets");

        return dictionary.Keys.ToList();
    }

    /// <summary>
    /// Извлечение ссылок на пакеты из проекта.
    /// </summary>
    public static List<PackageIdentity> ExtractPackages
        (
            string projectPath
        )
    {
        Sure.FileExists (projectPath);

        var result = new List<PackageIdentity>();
        var xml = XDocument.Load (projectPath);
        var elements = xml.XPathSelectElements ("/Project/ItemGroup/PackageReference");
        foreach (var element in elements)
        {
            var idAttribute = element.Attribute ("Include");
            var versionAttribute = element.Attribute ("Version");
            if (idAttribute is not null && versionAttribute is not null)
            {
                var id = idAttribute.Value;
                var version = NuGetVersion.Parse (versionAttribute.Value);
                var item = new PackageIdentity (id, version);
                result.Add (item);
            }
        }

        return result;
    }


    #endregion
}
