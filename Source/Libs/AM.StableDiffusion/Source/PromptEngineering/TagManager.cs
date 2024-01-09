// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TagManager.cs -- централизует работу с тегами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM.StableDiffusion.PromptEngineering;

/// <summary>
/// Централизует работу с тегами.
/// </summary>
[PublicAPI]
public sealed class TagManager
{
    #region Properties

    /// <summary>
    /// Сравнение строк.
    /// </summary>
    public StringComparer TagComparer { get; set; } =
        StringComparer.InvariantCultureIgnoreCase;

    /// <summary>
    /// Требуемые теги, например, "background".
    /// </summary>
    public List<TagInfo> RequiredTags { get; } = new ();

    /// <summary>
    /// Нежелательные теги, например, "solo" или "realistic".
    /// </summary>
    public List<TagInfo> UnwantedTags { get; } = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление (клона) тега в список.
    /// </summary>
    public void AppendTag
        (
            List<TagInfo> list,
            TagInfo tag,
            bool toHead = false
        )
    {
        Sure.NotNull (list);
        Sure.NotNull (tag);

        if (!ContainsTag (list, tag))
        {
            if (toHead)
            {
                list.Insert (0, tag.IncompleteClone());
            }
            else
            {
                list.Add (tag.IncompleteClone());
            }
        }
    }

    /// <summary>
    /// Построение подсказки из указанных тегов.
    /// </summary>
    public string BuildPrompt
        (
            IEnumerable<TagInfo> tags
        )
    {
        Sure.NotNull (tags);

        var first = true;
        var builder = StringBuilderPool.Shared.Get();
        foreach (var tag in tags)
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
    /// Сравнение тегов.
    /// </summary>
    public int CompareTags (string left, string right)
        => TagComparer.Compare (left, right);

    /// <summary>
    /// Сравнение тегов.
    /// </summary>
    public int CompareTags (TagInfo left, TagInfo right)
        => TagComparer.Compare (left.Title, right.Title);

    /// <summary>
    /// Проверка, содержится ли тег в списке.
    /// </summary>
    public bool ContainsTag
        (
            IEnumerable<TagInfo> list,
            TagInfo tag
        )
    {
        Sure.NotNull (list);
        Sure.NotNull (tag);

        foreach (var one in list)
        {
            if (IsSameTags (tag, one))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Фильтрация тегов от нежелательных.
    /// </summary>
    public List<TagInfo> FilterTags
        (
            IEnumerable<TagInfo> tags
        )
    {
        Sure.NotNull (tags);

        var result = new List<TagInfo>();
        foreach (var tag in tags)
        {
            if (!IsUnwantedTag (tag))
            {
                continue;
            }

            if (!ContainsTag (result, tag))
            {
                result.Add (tag.IncompleteClone());
            }
        }

        return result;
    }

    /// <summary>
    /// Сравнение тегов.
    /// </summary>
    public bool IsSameTags (string left, string right)
        => CompareTags (left, right) == 0;

    /// <summary>
    /// Сравнение тегов.
    /// </summary>
    public bool IsSameTags (TagInfo left, TagInfo right)
        => CompareTags (left, right) == 0;

    /// <summary>
    /// Проверка: нежелательный тег?
    /// </summary>
    public bool IsUnwantedTag
        (
            string tag
        )
    {
        foreach (var unwanted in UnwantedTags)
        {
            if (IsSameTags (tag, unwanted.Title!))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Проверка: нежелательный тег?
    /// </summary>
    public bool IsUnwantedTag
        (
            TagInfo tag
        )
    {
        Sure.NotNull (tag);

        foreach (var unwanted in UnwantedTags)
        {
            if (IsSameTags (tag, unwanted))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Загрузка тегов из файла.
    /// </summary>
    public List<TagInfo> LoadTags
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var result = new List<TagInfo>();
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
            if (!ContainsTag (result, tag))
            {
                result.Add (tag);
            }
        }

        return result;
    }

    /// <summary>
    /// Сохранение тегов в файле.
    /// </summary>
    public void SaveTags
        (
            string fileName,
            IEnumerable<TagInfo> tags
        )
    {
        Sure.NotNullNorEmpty (fileName);
        Sure.NotNull (tags);

        using var stream = File.CreateText (fileName);
        foreach (var tag in tags)
        {
            var title = tag.Title;
            if (!string.IsNullOrWhiteSpace (title))
            {
                stream.WriteLine(title);
            }
        }
    }

    #endregion
}
