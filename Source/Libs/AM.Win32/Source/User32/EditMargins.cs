// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global


/* EditMargins.cs -- задание полей контрола текстового редактора
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Задание полей (отступов) контрола текстового редактора.
/// </summary>
[Flags]
public enum EditMargins
{
    /// <summary>
    /// Установка левого отступа.
    /// </summary>
    EC_LEFTMARGIN = 1,

    /// <summary>
    /// Установка правого отступа.
    /// </summary>
    EC_RIGHTMARGIN = 2,

    /// <summary>
    /// Устанавливает для левого и правого полей узкую ширину,
    /// рассчитанную с использованием текстовых показателей текущего
    /// шрифта элемента управления. Если для элемента управления
    /// не установлен шрифт, поля устанавливаются равными нулю.
    /// </summary>
    EC_USEFONTINFO = 0xFFFF
}
