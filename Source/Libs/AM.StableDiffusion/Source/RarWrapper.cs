// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* RarWrapper.cs -- обертка над консольным RAR / WinRAR
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.IO;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Utils;

/// <summary>
/// Обертка над консольным RAR / WinRAR.
/// </summary>
[PublicAPI]
public sealed class RarWrapper
{
    #region Properties

    /// <summary>
    /// Исполняемый файл.
    /// </summary>
    public string RarExecutable { get; set; } = "rar";

    /// <summary>
    /// Преобразовывать имена файлов в нижний регистр.
    /// </summary>
    public bool ConvertNamesToLowerCase { get; set; }

    /// <summary>
    /// Оставлять на диске файлы, распаковавшиеся с ошибкой.
    /// </summary>
    public bool KeepBrokenFiles { get; set; }

    /// <summary>
    /// Перезапись существующих файлов.
    /// </summary>
    public bool OverwriteExisting { get; set; }

    /// <summary>
    /// Пароль (опционально).
    /// </summary>
    public string? Password { get; set; }

    #endregion

    #region Private members

    private ProcessStartInfo CreateProcessStartInfo
        (
            string command
        )
    {
        return new ()
        {
            FileName = RarExecutable,
            ArgumentList =
            {
                command
            }
        };
    }

    private void AddExtractionOptions
        (
            ProcessStartInfo startInfo
        )
    {
        if (ConvertNamesToLowerCase)
        {
            startInfo.ArgumentList.Add ("-cl");
        }

        if (OverwriteExisting)
        {
            startInfo.ArgumentList.Add ("-o+");
        }

        if (!string.IsNullOrEmpty (Password))
        {
            startInfo.ArgumentList.Add ($"-p{Password}");
        }

        if (KeepBrokenFiles)
        {
            startInfo.ArgumentList.Add ("-kb");
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Извлечение всех файлов.
    /// </summary>
    public int ExtractAllFiles
        (
            string archiveFile,
            bool withFullPath = true,
            string? targetDirectory = default
        )
    {
        Sure.FileExists (archiveFile);

        var startInfo = CreateProcessStartInfo
            (
                withFullPath ? "x" : "e"
            );

        AddExtractionOptions (startInfo);
        startInfo.ArgumentList.Add (archiveFile);

        if (!string.IsNullOrEmpty (targetDirectory))
        {
            var separator = Path.DirectorySeparatorChar;
            if (!targetDirectory.EndsWith (separator))
            {
                targetDirectory += separator;
            }

            startInfo.ArgumentList.Add (targetDirectory);
        }

        var process = Process.Start (startInfo);
        if (process is null)
        {
            return -1;
        }

        process.WaitForExit();

        return process.ExitCode;
    }

    /// <summary>
    /// Извлечение файлов из архива в папку, имя которой
    /// совпадает с именем архива.
    /// </summary>
    public int ExtractToSubdirectory
        (
            string archiveFile,
            bool withFullPath = true
        )
    {
        Sure.FileExists (archiveFile);

        var sourceDirectory = Path.GetDirectoryName (archiveFile) ?? string.Empty;
        var targetDirectory = Path.Combine
            (
                sourceDirectory,
                Path.GetFileNameWithoutExtension (archiveFile)
                    + Path.DirectorySeparatorChar
            );

        return ExtractAllFiles (archiveFile, withFullPath, targetDirectory);
    }

    #endregion
}
