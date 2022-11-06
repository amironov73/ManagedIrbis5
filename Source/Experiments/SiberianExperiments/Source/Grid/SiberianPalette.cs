// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* SiberianPalette.cs -- палитра цветов для отображения элементов грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid;

/// <summary>
/// Палитра цветов для отображения элементов грида.
/// </summary>
public sealed class SiberianPalette
{
    #region Properties

    /// <summary>
    /// Альтернативный фоновый цвет не выбранной строки.
    /// Применяется к нечетным строкам.
    /// </summary>
    [JsonPropertyName ("alt-back-color")]
    public Color AlternativeBackColor { get; set; }

    /// <summary>
    /// Альтернативный цвет переднего плана (текста) не выбранной строки.
    /// Применяется к нечетным строкам.
    /// </summary>
    [JsonPropertyName ("alt-fore-color")]
    public Color AlternativeForeColor { get; set; }

    /// <summary>
    /// Фоновый цвет обычной (не выбранной) строки.
    /// Применяется к четным строкам.
    /// </summary>
    [JsonPropertyName ("back-color")]
    public Color BackColor { get; set; }

    /// <summary>
    /// Фоновый цвет запрещенных (неактивных) элементов.
    /// </summary>
    [JsonPropertyName ("disabled-back-color")]
    public Color DisabledBackColor { get; set; }

    /// <summary>
    /// Цвет переднего плана (текста) запрещенных (неактивных) элементов.
    /// </summary>
    [JsonPropertyName ("disabled-fore-color")]
    public Color DisabledForeColor { get; set; }

    /// <summary>
    /// Цвет переднего плана (текста) выбранной строки.
    /// Применяется к четным строкам.
    /// </summary>
    [JsonPropertyName ("fore-color")]
    public Color ForeColor { get; set; }

    /// <summary>
    /// Цвет фона для заголовка колонки.
    /// </summary>
    [JsonPropertyName ("header-back-color")]
    public Color HeaderBackColor { get; set; }

    /// <summary>
    /// Цвет текста для заголовка колонки.
    /// </summary>
    [JsonPropertyName ("header-fore-color")]
    public Color HeaderForeColor { get; set; }

    /// <summary>
    /// Цвет линий, разделяющих ячейки.
    /// </summary>
    [JsonPropertyName ("line-color")]
    public Color LineColor { get; set; }

    /// <summary>
    /// Имя палитры (произвольное).
    /// </summary>
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Цвет фона выбранной строки.
    /// </summary>
    [JsonPropertyName ("selected-back-color")]
    public Color SelectedBackColor { get; set; }

    /// <summary>
    /// Цвет переднего плана (текста) выбранной строки.
    /// </summary>
    [JsonPropertyName ("selected-fore-color")]
    public Color SelectedForeColor { get; set; }

    /// <summary>
    /// Палитра по умолчанию.
    /// </summary>
    public static SiberianPalette DefaultPalette { get; } = new ()
    {
        Name = "Default",

        AlternativeBackColor = Color.LightCyan,
        BackColor = Color.White,
        ForeColor = Color.Black,

        HeaderBackColor = Color.LightGray,
        HeaderForeColor = Color.Black,

        LineColor = Color.Gray,

        DisabledBackColor = Color.White,
        DisabledForeColor = Color.DarkGray,

        SelectedBackColor = Color.Blue,
        SelectedForeColor = Color.White
    };

    #endregion
}
