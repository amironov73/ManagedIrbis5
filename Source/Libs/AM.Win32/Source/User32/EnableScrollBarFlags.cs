// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global


/* EnableScrollBarFlags.cs -- параметры для метода EnableScrollBar
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Параметры для метода EnableScrollBar.
/// </summary>
[Flags]
public enum EnableScrollBarFlags
{
    /// <summary>
    /// Включает обе стрелки на полосе прокрутки.
    /// </summary>
    ESB_ENABLE_BOTH = 0x0000,

    /// <summary>
    /// Отключает обе стрелки на полосе прокрутки.
    /// </summary>
    ESB_DISABLE_BOTH = 0x0003,

    /// <summary>
    /// Отключает стрелку влево на горизонтальной полосе прокрутки.
    /// </summary>
    ESB_DISABLE_LEFT = 0x0001,

    /// <summary>
    /// Отключает стрелку вправо на горизонтальной полосе прокрутки.
    /// </summary>
    ESB_DISABLE_RIGHT = 0x0002,

    /// <summary>
    /// Отключает стрелку вверх на вертикальной полосе прокрутки.
    /// </summary>
    ESB_DISABLE_UP = 0x0001,

    /// <summary>
    /// Отключает стрелку вниз на вертикальной полосе прокрутки.
    /// </summary>
    ESB_DISABLE_DOWN = 0x0002,

    /// <summary>
    /// Отключает стрелку влево на горизонтальной полосе прокрутки
    /// или стрелку вверх на вертикальной полосе прокрутки.
    /// </summary>
    ESB_DISABLE_LTUP = ESB_DISABLE_LEFT,

    /// <summary>
    /// Отключает стрелку вправо на горизонтальной полосе прокрутки
    /// или стрелку вниз на вертикальной полосе прокрутки.
    /// </summary>
    ESB_DISABLE_RTDN = ESB_DISABLE_RIGHT
}
