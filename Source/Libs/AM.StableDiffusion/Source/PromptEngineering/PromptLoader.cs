// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* PromptLoader.cs -- загрузчик для заготовки запроса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using AM;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.StableDiffusion.PromptEngineering;

/// <summary>
/// Загрузчик для заготовки запроса.
/// </summary>
[PublicAPI]
public sealed class PromptLoader
{
    #region Properties

    /// <summary>
    /// Пути, по которым необходимо искать вложенные заготовки.
    /// </summary>
    public string[] Paths { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PromptLoader
        (
            string[]? paths = default
        )
    {
        Paths = paths ?? Array.Empty<string>();
    }

    #endregion

    #region Private members

    private static PromptItem? TryLoad
        (
            string path,
            string fileName
        )
    {
        var combinedName = Path.Combine (path, fileName);
        if (File.Exists (combinedName))
        {
            var content = File.ReadAllBytes (combinedName);
            var options = new JsonReaderOptions
            {
                CommentHandling = JsonCommentHandling.Skip
            };
            var reader = new Utf8JsonReader (content, options);

            var result = JsonSerializer.Deserialize<PromptItem>(ref reader)
                .ThrowIfNull();

            return result;
        }

        return null;
    }

    private static TItem[] AppendItem<TItem>
        (
            TItem[]? array,
            TItem appendee
        )
    {
        if (array is null)
        {
            return new [] { appendee };
        }

        var result = new List<TItem> (array) { appendee };

        return result.ToArray();
    }

    private void HandleIncludes
        (
            PromptItem item
        )
    {
        var fileName = item.Include;
        if (!string.IsNullOrEmpty (fileName))
        {
            var found = false;
            foreach (var path in Paths)
            {
                var loaded = TryLoad (path, fileName);
                if (loaded is not null)
                {
                    found = true;
                    item.SubItems = AppendItem (item.SubItems, loaded);
                    break;
                }
            }

            if (!found)
            {
                throw new FileNotFoundException (fileName);
            }
        }

        if (item.SubItems is { } subItems)
        {
            foreach (var subItem in subItems)
            {
                HandleIncludes (subItem);
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Загрузка заготовки из указанного файла.
    /// </summary>
    public Prompt LoadFromFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var content = File.ReadAllBytes (fileName);
        var options = new JsonReaderOptions
        {
            CommentHandling = JsonCommentHandling.Skip
        };
        var reader = new Utf8JsonReader (content, options);

        var result = JsonSerializer.Deserialize<Prompt>(ref reader)
            .ThrowIfNull();

        foreach (var item in result.Items)
        {
            HandleIncludes (item);
        }

        return result;
    }

    #endregion
}
