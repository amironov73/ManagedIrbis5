// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TopazCli.cs -- интерфейс командной строки для Topaz Photo AI
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using AM.IO;
using AM.Text;

#endregion

#nullable enable

namespace AM.Topaz;

/// <summary>
/// Интерфейс командной строки для Topaz Photo AI.
/// </summary>
public class TopazCli
{
    #region Properties

    /// <summary>
    /// Путь к исполняемому файлу.
    /// </summary>
    public string Executable { get; set; } = "tpai";

    #endregion

    #region Public methods

    /// <summary>
    /// Пытается найти исполняемый файл.
    /// </summary>
    public bool FindExecutable()
    {
        if (!OperatingSystem.IsWindows())
        {
            return false;
        }

        var candidate = Path.Combine
            (
                SystemFolders.ProgramFiles,
                "Topaz Labs LLC",
                "Topaz Photo AI",
                "tpai.exe"
            );

        if (File.Exists (candidate))
        {
            Executable = candidate;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Запуск обработки файлов.
    /// </summary>
    public int Run
        (
            TopazCliOptions options,
            params string[] files
        )
    {
        return Run (options, (IList<string>) files);
    }

    /// <summary>
    /// Запуск обработки файлов.
    /// </summary>
    public int Run
        (
            TopazCliOptions options,
            IList<string> files
        )
    {
        if (files.Count == 0)
        {
            return -1;
        }

        var builder = new StringBuilder();
        builder.Append (options);
        foreach (var file in files)
        {
            builder.Append (' ');
            builder.Append (file);
        }

        builder.Trim();
        var startInfo = new ProcessStartInfo
            (
                Executable,
                builder.ToString()
            );
        var process = Process.Start (startInfo);
        if (process is null)
        {
            return -1;
        }

        process.WaitForExit();
        return process.ExitCode;
    }

    #endregion
}
