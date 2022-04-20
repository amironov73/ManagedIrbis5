// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LogLine.cs -- строчка сообщения в LogBoxEx
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Строчка сообщения в <see cref="LogBoxEx"/>.
/// </summary>
public sealed class LogLine
{
    #region Properties

    /// <summary>
    /// Уровень сообщения.
    /// </summary>
    public LogLevel Level { get; set; }

    /// <summary>
    /// Момент времени, к которому относится сообщение.
    /// Опционально.
    /// </summary>
    public DateTime? Moment { get; set; }

    /// <summary>
    /// Ассоциированная иконка. Опционально.
    /// </summary>
    public Icon? Icon { get; set; }

    /// <summary>
    /// Ассоциированный цвет фона. Опционально.
    /// </summary>
    public Color? BackColor { get; set; }

    /// <summary>
    /// Ассоциированный цвет текста. Опционально.
    /// </summary>
    public Color? ForeColor { get; set; }

    /// <summary>
    /// Текст сообщения. Опционально.
    /// Лучше, чтобы здесь было что-нибудь написано,
    /// ибо непонятно, зачем это всё надо.
    /// </summary>
    public string? Message { get; set; }

    #endregion
}
