// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TagList.cs -- список тегов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM.StableDiffusion.PromptEngineering;

/// <summary>
/// Список тегов.
/// </summary>
[PublicAPI]
public sealed class TagList
    : List<TagInfo>
{
    #region Public methods

    /// <summary>
    /// Добавление (клона) тега в список.
    /// </summary>
    public void AppendTag
        (
            TagInfo tag,
            bool toHead = false
        )
    {
        Sure.NotNull (tag);

        if (!string.IsNullOrWhiteSpace (tag.Title)
            && !ContainsTag (tag))
        {
            if (toHead)
            {
                Insert (0, tag.IncompleteClone());
            }
            else
            {
                Add (tag.IncompleteClone());
            }
        }
    }

    /// <summary>
    /// Построение подсказки из указанных тегов.
    /// </summary>
    public string BuildPrompt()
    {
        var first = true;
        var builder = StringBuilderPool.Shared.Get();
        foreach (var tag in this)
        {
            var title = tag.Title;
            if (!string.IsNullOrWhiteSpace (title))
            {
                if (!first)
                {
                    builder.Append (", ");
                }

                builder.Append (title);
                first = false;
            }
        }

        return builder.ReturnShared();
    }

    /// <summary>
    /// Проверка, содержится ли тег в списке.
    /// </summary>
    public bool ContainsTag
        (
            TagInfo tag
        )
    {
        Sure.NotNull (tag);

        foreach (var one in this)
        {
            if (TagComparer.IsSame (tag, one))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Проверка, содержится ли тег в списке.
    /// </summary>
    public bool ContainsTag
        (
            string? tag
        )
    {
        foreach (var one in this)
        {
            if (TagComparer.IsSame (tag, one.Title))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Фильтрация списка "на месте".
    /// </summary>
    public void FilterInPlace
        (
            TagList unwantedTags
        )
    {
        Sure.NotNull (unwantedTags);

        for (var i = 0; i < Count;)
        {
            if (unwantedTags.ContainsTag (this[i]))
            {
                RemoveAt (i);
            }
            else
            {
                i++;
            }
        }
    }

    /// <summary>
    /// Фильтрация списка с созданием нового, отфильтрованного.
    /// </summary>
    public TagList FilterOut
        (
            TagList unwantedTags
        )
    {
        Sure.NotNull (unwantedTags);

        var result = new TagList();
        foreach (var one in this)
        {
            if (!unwantedTags.ContainsTag (one))
            {
                result.Add (one);
            }
        }

        return result;
    }

    /// <summary>
    /// Загрузка тегов из файла.
    /// </summary>
    public void LoadTags
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        foreach (var line in File.ReadLines (fileName))
        {
            if (string.IsNullOrWhiteSpace (line))
            {
                continue;
            }

            var trimmed = line.Trim();
            if (trimmed[0] == '#')
            {
                continue;
            }

            var tag = new TagInfo { Title = trimmed };
            if (!ContainsTag (tag))
            {
                Add (tag);
            }
        }
    }

    /// <summary>
    /// Сохранение тегов в файле.
    /// </summary>
    public void SaveTags
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        using var stream = File.CreateText (fileName);
        foreach (var tag in this)
        {
            var title = tag.Title;
            if (!string.IsNullOrWhiteSpace (title))
            {
                stream.WriteLine (title);
            }
        }
    }

    #endregion
}
