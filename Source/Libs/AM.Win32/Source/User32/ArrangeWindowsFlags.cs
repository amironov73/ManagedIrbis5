// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ArrangeWindowsFlags.cs -- управляет расположением минимизированных окон
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Управляет тем, как система располагает минимизированные окна.
/// </summary>
[Flags]
public enum ArrangeWindowsFlags
{
    /// <summary>
    /// Окна располагаются, начиная с нижнего левого угла экрана
    /// (положение по умолчанию).
    /// </summary>
    ARW_BOTTOMLEFT = 0x0000,

    /// <summary>
    /// Окна располагаются, начиная с правого нижнего угла экрана.
    /// Эквивалентно ARW_STARTRIGHT.
    /// </summary>
    ARW_BOTTOMRIGHT = 0x0001,

    /// <summary>
    /// Окна располагаются, начиная с верхнего левого угла экрана.
    /// Эквивалентно ARV_STARTTOP.
    /// </summary>
    ARW_TOPLEFT = 0x0002,

    /// <summary>
    /// Окна располагаются, начиная с правого верхнего угла экрана.
    /// Эквивалентно ARW_STARTTOP | SRW_STARTRIGHT.
    /// </summary>
    ARW_TOPRIGHT = 0x0003,

    /// <summary>
    /// Окна располагаются, начиная с правого верхнего угла экрана.
    /// Эквивалентно ARW_STARTTOP | SRW_STARTRIGHT.
    /// </summary>
    ARW_STARTMASK = 0x0003,

    /// <summary>
    /// Окна располагаются, начиная с правого нижнего угла экрана.
    /// Эквивалентно ARW_BOTTOMRIGHT.
    /// </summary>
    ARW_STARTRIGHT = 0x0001,

    /// <summary>
    /// Окна располагаются, начиная с верхнего левого угла экрана.
    /// Эквивалентно ARV_TOPLEFT.
    /// </summary>
    ARW_STARTTOP = 0x0002,

    /// <summary>
    /// Окна располагаются горизонтально, слева направо.
    /// </summary>
    ARW_LEFT = 0x0000,

    /// <summary>
    /// Окна располагаются горизонтально, справа налево.
    /// </summary>
    ARW_RIGHT = 0x0000,

    /// <summary>
    /// Окна располагаются вертикально, снизу вверх.
    /// </summary>
    ARW_UP = 0x0004,

    /// <summary>
    /// Окна располагаются вертикально, сверху вниз.
    /// </summary>
    ARW_DOWN = 0x0004,

    /// <summary>
    /// Свернутые окна скрываются за счет перемещения их за пределы
    /// видимой области экрана.
    /// </summary>
    ARW_HIDE = 0x0008,
}
