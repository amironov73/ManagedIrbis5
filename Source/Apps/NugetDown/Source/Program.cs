// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM.BuildSystem;

using NuGet.Packaging.Core;
using NuGet.Versioning;

#endregion

#nullable enable

namespace Barsik;

/// <summary>
/// Вся логика программы в одном классе.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    static int Main
        (
            string[] args
        )
    {
        if (args.Length != 1 && args.Length != 2)
        {
            Console.Error.WriteLine ("USAGE: NugetDown <packageId> [packageVersion]");
            return 1;
        }

        try
        {
            var downloader = new NuGetPackageDownloader();
            downloader.SetTargetFolder ("packages");

            var packageId = args[0];
            var packageVersion = args.Length > 1
                ? NuGetVersion.Parse (args[1])
                : downloader.GetLatestVersion (packageId);

            var additional = new Dictionary<PackageIdentity, object?>();
            var requiredPackage = new PackageIdentity (packageId, packageVersion);
            var fileName = downloader.DownloadPackage (requiredPackage);
            if (fileName is not null)
            {
                var dependencies = downloader.ListDependencies (fileName);
                foreach (var dependency in dependencies)
                {
                    additional[dependency] = null;
                }

                downloader.ExtractPackage (fileName);

                while (additional.Count != 0)
                {
                    Console.Write ($"{additional.Count}> ");
                    var first = additional.Keys.First();
                    additional.Remove (first);
                    Console.WriteLine (first);
                    fileName = downloader.DownloadPackage (first);
                    if (fileName is not null)
                    {
                        dependencies = downloader.ListDependencies (fileName);
                        foreach (var dependency in dependencies)
                        {
                            additional[dependency] = null;
                        }

                        downloader.ExtractPackage (fileName);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine ("ALL DONE");
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception.ToString());
            return 2;
        }

        return 0;
    }
}
