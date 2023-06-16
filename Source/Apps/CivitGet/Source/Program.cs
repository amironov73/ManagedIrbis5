// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using AM.Net;
using AM.StableDiffusion.CivitAI;

using JetBrains.Annotations;

using Newtonsoft.Json;

using RestSharp;

#endregion

#nullable enable

namespace CivitGet;

internal class Program
{
    public static int Main (string[] args)
    {
        if (args.Length == 0)
        {
            return 0;
        }

        var downloader = new CivitDownloader();
        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "model":
                    downloader.DownloadImages (modelId: int.Parse (args[++i]));
                    break;

                case "post":
                    downloader.DownloadImages (postId: int.Parse (args[++i]));
                    break;

                case "user":
                    downloader.DownloadImages (username: args[++i]);
                    break;

                default:
                    downloader.DownloadFromUrl (args[i]);
                    break;
            }
        }

        return 0;
    }
}
