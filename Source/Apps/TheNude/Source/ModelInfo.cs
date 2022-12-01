// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* ModelInfo.cs -- информация о модели
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks.Dataflow;

using Avalonia.Media.Imaging;

using HtmlAgilityPack;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace TheNude;

/// <summary>
/// Информация о модели.
/// </summary>
public class ModelInfo
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Основное имя модели.
    /// </summary>
    [Reactive]
    public string? Name { get; set; }

    /// <summary>
    /// Альтернативные имена модели.
    /// </summary>
    [Reactive]
    public string[]? Aka { get; set; }

    /// <summary>
    /// Ссылка на уменьшенную картинку.
    /// </summary>
    [Reactive]
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// Уменьшенная картинка.
    /// </summary>
    [Reactive]
    public Bitmap? ThumbnailBitmap { get; set; }

    /// <summary>
    /// Страница модели.
    /// </summary>
    [Reactive]
    public string? Page { get; set; }

    #endregion

    #region Private methods

    private static ActionBlock<ModelInfo>? _backgroundLoader;

    private static void _SlowLoading
        (
            ModelInfo info
        )
    {
        var url = info.ThumbnailUrl;
        if (!string.IsNullOrEmpty (url))
        {
            try
            {
                info.ThumbnailBitmap = ThumbnailLoader.Instance.GetThumbnail (url);
            }
            catch (Exception exception)
            {
                Debug.WriteLine (exception.Message);
            }
        }
    }


    private static string[]? ParseAka
        (
            string? value
        )
    {
        if (string.IsNullOrWhiteSpace (value))
        {
            return null;
        }

        if (value.StartsWith ("AKA:"))
        {
            value = value.Substring (4);
        }

        return value.Split (',')
            .Select (x => x.Trim())
            .ToArray();
    }

    #endregion

    #region Public methods

    public void EnqueueSlowLoading()
    {
        _backgroundLoader ??= new ActionBlock<ModelInfo>
            (
                _SlowLoading,
                new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 3
                }
            );
        _backgroundLoader.Post (this);
    }

    public void GotoModelPage()
    {
        if (!string.IsNullOrEmpty (Page))
        {
            var startInfo = new ProcessStartInfo (Page)
            {
                UseShellExecute = true
            };
            Process.Start (startInfo);
        }
    }

    /// <summary>
    /// Разбор сведений о модели.
    /// </summary>
    /// <param name="figure"></param>
    /// <returns></returns>
    public bool Parse
        (
            HtmlNode figure
        )
    {
        Name = figure.Descendants ("a")
            .FirstOrDefault (x => x.HasClass ("model-name"))
            ?.InnerText;
        Page = figure.Descendants ("a").FirstOrDefault()
            ?.Attributes["href"].Value;
        ThumbnailUrl = figure.Descendants ("img").FirstOrDefault()
            ?.Attributes["src"].Value;
        Aka = ParseAka (figure.Descendants ("figcaption")
            .FirstOrDefault()
            ?.Descendants ("span")
            ?.FirstOrDefault()?.InnerText);

        return true;
    }

    #endregion

    #region Object members

    public override string ToString()
    {
        return string.IsNullOrEmpty (Name)
            ? "No name"
            : Name;
    }

    #endregion
}
