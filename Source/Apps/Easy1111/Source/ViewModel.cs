// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* ViewModel.cs -- модель данных главного окна
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AM;
using AM.Skia;
using AM.StableDiffusion.Automatic;

using Avalonia.Controls;
using Avalonia.Platform.Storage;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SkiaSharp;

#endregion

namespace Easy1111;

/// <summary>
/// Модель данных главного окна.
/// </summary>
public sealed class ViewModel
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Окно, чтобы было на что ссылаться.
    /// </summary>
    public Window? Window { get; set; }

    /// <summary>
    /// Позитивная подсказка.
    /// </summary>
    [Reactive]
    public string? Positive { get; set; }

    /// <summary>
    /// Позитивная подсказка.
    /// </summary>
    [Reactive]
    public string? Negative { get; set; }

    /// <summary>
    /// Сила подсказки.
    /// </summary>
    [Reactive]
    public float Cfg { get; set; } = 7;

    /// <summary>
    /// Ширина изображения в пикселах.
    /// </summary>
    [Reactive]
    public int Width { get; set; } = 512;

    /// <summary>
    /// Высота изображения в пикселах.
    /// </summary>
    [Reactive]
    public int Height { get; set; } = 512;

    /// <summary>
    /// Количество шагов вывода.
    /// </summary>
    [Reactive]
    public int Steps { get; set; } = 20;

    #endregion

    #region Public methods

    /// <summary>
    /// Получение данных из запроса.
    /// </summary>
    public void FromRequest
        (
            TextToImageRequest request
        )
    {
        Sure.NotNull (request);

        Positive = request.Prompt;
        Negative = request.NegativePrompt;
        Width = request.Width;
        Height = request.Height;
        Cfg = request.CfgScale;
        Steps = request.Steps;
    }

    /// <summary>
    /// Генерация изображения.
    /// </summary>
    public void Generate()
    {

    }

    public TextToImageRequest ToRequest()
    {
        var result = new TextToImageRequest
        {
            Prompt = Positive,
            NegativePrompt = Negative,
            CfgScale = Cfg,
            Width = Width,
            Height = Height,
            Steps = Steps,
        };

        return result;
    }

    #endregion
}
