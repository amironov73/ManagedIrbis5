// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* Prompt.cs -- запрос
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

using AM;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace StableErection;

/// <summary>
/// Запрос.
/// </summary>
[PublicAPI]
public sealed class Prompt
{
    #region Properties

    /// <summary>
    /// Заголовок.
    /// </summary>
    [JsonPropertyName ("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Комментарий в произвольной форме.
    /// </summary>
    [JsonPropertyName ("comments")]
    public string? Comments { get; set; }

    /// <summary>
    /// Элементы.
    /// </summary>
    [JsonPropertyName ("items")]
    public PromptItem[] Items { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Prompt()
    {
        Items = Array.Empty<PromptItem>();
    }

    #endregion

    #region Private members

    private static string[] GetIncludePaths()
    {
        var result = new List<string>();
        var currentDirectory = Directory.GetCurrentDirectory();
        result.Add (currentDirectory);
        result.Add (Path.Combine (currentDirectory, "include"));

        var appDirectory = AppContext.BaseDirectory;
        if (appDirectory != currentDirectory)
        {
            result.Add (appDirectory);
            result.Add (Path.Combine (appDirectory, "include"));
        }

        return result.ToArray();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Загрузка промптов из JSON-файла.
    /// </summary>
    public static Prompt LoadFromFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var paths = GetIncludePaths();
        var loader = new PromptLoader (paths);

        return loader.LoadFromFile (fileName);
    }

    #endregion
}
