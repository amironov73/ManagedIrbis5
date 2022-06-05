// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* ClientRunner.cs -- запускает клиент с заданными логином и паролем.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Client;

/// <summary>
/// Запускает указанный клиент с заданными логином и паролем.
/// </summary>
[XmlRoot ("clientRunner")]
[ExcludeFromCodeCoverage]
public sealed class ClientRunner
    : IHandmadeSerializable
{
    #region Properties

    /// <summary>
    /// Database name (required if <see cref="UserName"/>
    /// and <see cref="Password"/> are specified).
    /// </summary>
    [XmlElement ("database")]
    [JsonPropertyName ("database")]
    [Description ("Имя базы данных")]
    public string? Database { get; set; }

    /// <summary>
    /// Executable file name.
    /// </summary>
    [XmlElement ("exe-name")]
    [JsonPropertyName ("exe-name")]
    [Description ("EXE-файл")]
    public string Executable { get; set; }

    /// <summary>
    /// INI-file name.
    /// </summary>
    [XmlElement ("ini-file")]
    [JsonPropertyName ("ini-file")]
    [Description ("INI-файл")]
    public string IniFileName { get; set; }

    /// <summary>
    /// Выбранный MFN.
    /// </summary>
    [XmlElement ("mfn")]
    [JsonPropertyName ("mfn")]
    [Description ("MFN")]
    public int Mfn { get; set; }

    /// <summary>
    /// Пароль.
    /// </summary>
    [XmlElement ("password")]
    [JsonPropertyName ("password")]
    [Description ("Пароль")]
    public string? Password { get; set; }

    /// <summary>
    /// Имя пользователя (логин).
    /// </summary>
    [XmlElement ("username")]
    [JsonPropertyName ("username")]
    [Description ("Логин")]
    public string? UserName { get; set; }

    /// <summary>
    /// Рабочая директория.
    /// </summary>
    [XmlElement ("directory")]
    [JsonPropertyName ("directory")]
    [Description ("Рабочая директория")]
    public string WorkingDirectory { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ClientRunner()
    {
        // Starting with 2018.1: cirbis_plus.exe
        Executable = "cirbisc_new_unicode.exe";
        IniFileName = "cirbisc.ini";
        WorkingDirectory = @"C:\IRBIS64";
    }

    #endregion

    #region Private members

    private string? _copyIniPath;

    private void _ProcessExited
        (
            object? sender,
            EventArgs e
        )
    {
        var process = (Process?)sender;

        File.Delete (_copyIniPath!);
        process?.Dispose();
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Database = reader.ReadNullableString();
        Executable = reader.ReadString();
        IniFileName = reader.ReadString();
        Mfn = reader.ReadPackedInt32();
        Password = reader.ReadNullableString();
        UserName = reader.ReadNullableString();
        WorkingDirectory = reader.ReadString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.WriteNullable (Database);
        writer.Write (Executable);
        writer.Write (IniFileName);
        writer
            .WritePackedInt32 (Mfn)
            .WriteNullable (Password)
            .WriteNullable (UserName);
        writer.Write (WorkingDirectory);
    }

    #endregion


    #region Public methods

    /// <summary>
    /// Запускает клиента и опционально дожидается окончания работы.
    /// </summary>
    public void RunClient
        (
            bool wait
        )
    {
        if (!Directory.Exists (WorkingDirectory))
        {
            throw new IrbisException ("Working directory not exists");
        }

        var executablePath = Path.Combine
            (
                WorkingDirectory,
                Executable
            );
        if (!File.Exists (executablePath))
        {
            throw new IrbisException ("Executable file not exists");
        }

        var mainIniPath = Path.Combine
            (
                WorkingDirectory,
                IniFileName
            );
        if (!File.Exists (mainIniPath))
        {
            throw new IrbisException
                (
                    "INI file not exists: "
                    + mainIniPath
                );
        }

        var copyIniName =
            "_"
            + Guid.NewGuid().ToString ("N")
            + ".ini";
        _copyIniPath = Path.Combine
            (
                WorkingDirectory,
                copyIniName
            );
        File.Copy (mainIniPath, _copyIniPath);

        using (var iniFile = new IniFile
                   (
                       _copyIniPath,
                       IrbisEncoding.Ansi
                   ))
        using (var _ = new ContextIniSection (iniFile)
               {
                   Database = Database,
                   Mfn = Mfn,
                   Password = Password,
                   UserName = UserName
               })
        {
            iniFile.Save (_copyIniPath);
        }

        var startInfo = new ProcessStartInfo
            (
                executablePath,
                copyIniName
            )
            {
                UseShellExecute = false,
                WorkingDirectory = WorkingDirectory
            };
        var process = new Process
        {
            StartInfo = startInfo
        };
        process.Exited += _ProcessExited;

        process.Start();

        if (wait)
        {
            process.WaitForExit();
        }
    }

    #endregion
}
