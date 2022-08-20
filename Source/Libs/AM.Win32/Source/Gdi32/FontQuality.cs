// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* FontQuality.cs -- качество вывода шрифтов
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Качество вывода шрифтов. Определяет, насколько тщательно GDI
/// должен пытаться сопоставить атрибуты логического шрифта
/// с атрибутами реального физического шрифта.
/// </summary>
[Flags]
public enum FontQuality
{
    /// <summary>
    /// Внешний вид шрифта не имеет значения.
    /// </summary>
    DEFAULT_QUALITY = 0,

    /// <summary>
    /// Внешний вид шрифта менее важен, чем при использовании
    /// значения PROOF_QUALITY. Для растровых шрифтов GDI включено
    /// масштабирование, что означает, что доступно больше размеров
    /// шрифта, но качество может быть ниже. При необходимости
    /// синтезируются полужирный, курсивный, подчеркнутый
    /// и зачеркнутый шрифты.
    /// </summary>
    DRAFT_QUALITY = 1,

    /// <summary>
    /// Качество символов шрифта важнее точного соответствия атрибутам
    /// логического шрифта. Для растровых шрифтов GDI масштабирование
    /// отключено и выбирается ближайший по размеру шрифт. Хотя выбранный
    /// размер шрифта может не отображаться точно, когда используется
    /// PROOF_QUALITY, качество шрифта высокое, и нет искажения
    /// внешнего вида. При необходимости синтезируются полужирный,
    /// курсивный, подчеркнутый и зачеркнутый шрифты.
    /// </summary>
    PROOF_QUALITY = 2,

    /// <summary>
    /// Шрифт никогда не сглаживается, то есть сглаживание шрифта
    /// не выполняется.
    /// </summary>
    NONANTIALIASED_QUALITY = 3,

    /// <summary>
    /// Шрифт сглажен или сглажен, если шрифт поддерживает это,
    /// и размер шрифта не слишком мал или слишком велик.
    /// </summary>
    ANTIALIASED_QUALITY = 4,

    /// <summary>
    /// Если установлено, текст отображается (когда это возможно)
    /// с использованием метода сглаживания ClearType.
    /// </summary>
    CLEARTYPE_QUALITY = 5,

    /// <summary>
    /// Назначение флага неизвестно.
    /// </summary>
    CLEARTYPE_NATURAL_QUALITY = 6
}
