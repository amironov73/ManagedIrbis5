// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* MystemRunner.cs -- запускает mystem и разбирает результаты
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using AM.Text;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.AOT.Stemming;

/// <summary>
/// Запускает mystem и разбирает результаты.
/// </summary>
public sealed class MystemRunner
{
    #region Properties

    /// <summary>
    /// Путь до mystem.exe, включая exe-файл.
    /// По умолчанию "mystem.exe".
    /// </summary>
    public string MystemPath { get; set; }

    /// <summary>
    /// Опции, передаваемые mystem.exe.
    /// По умолчанию -i -g -d.
    /// </summary>
    public string MystemOptions { get; set; }

    /// <summary>
    /// Кодировка, используемая при передаче.
    /// По умолчанию CP866, т. к. применяется
    /// перенаправление ввода-вывода.
    /// </summary>
    public Encoding TransferEncoding { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public MystemRunner()
    {
        Utility.RegisterEncodingProviders(); // для cp866

        MystemPath = GetDefaultMystemName();
        MystemOptions = "-i -g -d";
        TransferEncoding = OperatingSystem.IsWindows()
            ? Encoding.GetEncoding ("cp866")
            : Encoding.UTF8;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор результатов.
    /// </summary>
    /// <param name="reader">Поток с результатами.</param>
    public MystemResult[] DecodeResults
        (
            StreamReader reader
        )
    {
        Sure.NotNull (reader);

        var result = new List<MystemResult>();

        while (reader.ReadLine() is { } line)
        {
            var one = JsonConvert.DeserializeObject<MystemResult[]> (line);
            if (one is not null)
            {
                result.AddRange (one);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Получение имени программы с учетом операционной системы.
    /// </summary>
    public static string GetDefaultMystemName()
    {
        return OperatingSystem.IsWindows() ? "mystem.exe" : "mystem";
    }

    /// <summary>
    /// Запускает анализ текста и выдаёт результаты.
    /// </summary>
    /// <param name="text">Текст для анализа.</param>
    public MystemResult[] Run
        (
            string text
        )
    {
        Sure.NotNullNorEmpty (text);

        var builder = StringBuilderPool.Shared.Get();
        builder.Append (MystemOptions);
        builder.Append (" -e " + TransferEncoding.HeaderName);
        builder.Append (" --format json");
        var commandLine = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        var startInfo = new ProcessStartInfo
            (
                MystemPath,
                commandLine
            )
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                StandardOutputEncoding = TransferEncoding
            };

        using var process = new Process { StartInfo = startInfo };
        process.Start();
        process.StandardInput.Write (text);
        process.StandardInput.Close();
        process.WaitForExit();
        var result = DecodeResults (process.StandardOutput);

        return result;
    }

    #endregion
}
