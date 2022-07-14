// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ConsoleAttributes.cs -- атрибуты консольного текста
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Атрибуты консольного текста.
/// </summary>
[Flags]
public enum ConsoleAttributes
    : ushort
{
    /// <summary>
    /// Цвет текста содержит синюю компоненту.
    /// </summary>
    FOREGROUND_BLUE = 0x0001,

    /// <summary>
    /// Цвет текста содержит зеленую компоненту.
    /// </summary>
    FOREGROUND_GREEN = 0x0002,

    /// <summary>
    /// Цвет текста содержит красную компоненту.
    /// </summary>
    FOREGROUND_RED = 0x0004,

    /// <summary>
    /// Интенсивный цвет текста.
    /// </summary>
    FOREGROUND_INTENSITY = 0x0008,

    /// <summary>
    /// Цвет фона содержит синюю компоненту.
    /// </summary>
    BACKGROUND_BLUE = 0x0010,

    /// <summary>
    /// Цвет фона содержит зеленую компоненту.
    /// </summary>
    BACKGROUND_GREEN = 0x0020,

    /// <summary>
    /// Цвет фона содержит красную компоненту.
    /// </summary>
    BACKGROUND_RED = 0x0040,

    /// <summary>
    /// Интенсивный цвет фона.
    /// </summary>
    BACKGROUND_INTENSITY = 0x0080,

    /// <summary>
    /// Лидирующий байт DBCS.
    /// </summary>
    COMMON_LVB_LEADING_BYTE = 0x0100,

    /// <summary>
    /// Замыкающий байт DBCS.
    /// </summary>
    COMMON_LVB_TRAILING_BYTE = 0x0200,

    /// <summary>
    /// DBCS: Grid attribute: top horizontal.
    /// </summary>
    COMMON_LVB_GRID_HORIZONTAL = 0x0400,

    /// <summary>
    /// DBCS: Grid attribute: left vertical.
    /// </summary>
    COMMON_LVB_GRID_LVERTICAL = 0x0800,

    /// <summary>
    /// DBCS: Grid attribute: right vertical.
    /// </summary>
    COMMON_LVB_GRID_RVERTICAL = 0x1000,

    /// <summary>
    /// DBCS: Reverse fore/back ground attribute.
    /// </summary>
    COMMON_LVB_REVERSE_VIDEO = 0x4000,

    /// <summary>
    /// DBCS: Underscore.
    /// </summary>
    COMMON_LVB_UNDERSCORE = 0x8000
}
