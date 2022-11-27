// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* HatchStyle.cs -- ориентация линий, используемых для создания штриховки
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Указывает ориентацию линий, используемых для создания штриховки.
/// </summary>
public enum HatchStyle
{
    /// <summary>
    /// Горизонтальная штриховка. -----
    /// </summary>
    HS_HORIZONTAL = 0,

    /// <summary>
    /// Вертикальная штриховка. |||||
    /// </summary>
    HS_VERTICAL = 1,

    /// <summary>
    /// Наклонная штриховка слева направо сверху вниз. \\\\\
    /// </summary>
    HS_FDIAGONAL = 2,

    /// <summary>
    /// Наклонная штриховка слева направо снизу вверх. /////
    /// </summary>
    HS_BDIAGONAL = 3,

    /// <summary>
    /// Вертикальные и горизонтальные перекрестные линии. +++++
    /// </summary>
    HS_CROSS = 4,

    /// <summary>
    /// Наклонные перекрестные линии. xxxxx
    /// </summary>
    HS_DIAGCROSS = 5
}
