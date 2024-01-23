// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* LprWrapper.cs -- обертка над командой LPR
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

using AM.Collections;

using JetBrains.Annotations;

#endregion

namespace AM.Linux;

/// <summary>
/// Обертка над командой LPR.
/// </summary>
[PublicAPI]
public sealed class LprWrapper
{
    #region Properties

    /// <summary>
    /// Имя команды LPR.
    /// </summary>
    public string Lpr { get; set; } = "lpr";

    /// <summary>
    /// Количество копий.
    /// </summary>
    [JsonPropertyName ("copies")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Copies { get; set; }

    /// <summary>
    /// Требовать зашифрованный канал для печати.
    /// </summary>
    [JsonPropertyName ("encryption")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool ForceEncryption { get; set; }

    /// <summary>
    /// Имя задания.
    /// </summary>
    [JsonPropertyName ("job")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? JobName { get; set; }

    /// <summary>
    /// Имя принтера.
    /// </summary>
    [JsonPropertyName ("printer")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Printer { get; set; }

    /// <summary>
    /// Сервер, на который будет направлен документ.
    /// </summary>
    [JsonPropertyName ("server")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Server { get; set; }

    /// <summary>
    /// Альтернативное имя пользователя.
    /// </summary>
    [JsonPropertyName ("user")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Username { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Печать указанных файлов.
    /// </summary>
    public void PrintFile
        (
            params string[] files
        )
    {
        if (files.IsNullOrEmpty())
        {
            return;
        }

        var arguments = new List<string>();
        if (Copies > 1)
        {
            arguments.Add ("-#");
            arguments.Add (Copies.ToInvariantString());
        }

        if (ForceEncryption)
        {
            arguments.Add ("-E");
        }

        if (!string.IsNullOrEmpty (JobName))
        {
            arguments.Add ("-J");
            arguments.Add (JobName);
        }

        if (!string.IsNullOrEmpty (Printer))
        {
            arguments.Add ("-P");
            arguments.Add (Printer);
        }

        if (!string.IsNullOrEmpty (Server))
        {
            arguments.Add ("-H");
            arguments.Add (Server);
        }

        if (!string.IsNullOrEmpty (Username))
        {
            arguments.Add ("-U");
            arguments.Add (Username);
        }

        arguments.Add ("-h"); // Disables banner printing

        foreach (var file in files)
        {
            Sure.FileExists (file);
            arguments.Add (file);
        }

        using var process = Process.Start (Lpr, arguments);
    }

    #endregion
}
