// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* WindowPlacement.cs -- места на экране, где может быть расположено окно
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;

#endregion

namespace AM.Windows.Forms;

/// <summary>
/// Места на экране, где может быть расположено окно.
/// </summary>
public enum WindowPlacement
{
    /// <summary>
    /// Где угодно (местоположение безразлично).
    /// </summary>
    [Description ("Где угодно")]
    Anywhere,

    /// <summary>
    /// Центр экрана.
    /// </summary>
    [Description ("Центр экрана")]
    ScreenCenter,

    /// <summary>
    /// Верхний левый угол.
    /// </summary>
    [Description ("Верхний левый угол")]
    TopLeftCorner,

    /// <summary>
    /// Верхний правый угол.
    /// </summary>
    [Description ("Верхний правый угол")]
    TopRightCorner,

    /// <summary>
    /// Вверху по центру.
    /// </summary>
    [Description ("Вверху по центру")]
    TopSide,

    /// <summary>
    /// Слева по центру.
    /// </summary>
    [Description ("Слева по центру")]
    LeftSide,

    /// <summary>
    /// Справа по центру.
    /// </summary>
    [Description ("Справа по центру")]
    RightSide,

    /// <summary>
    /// Внизу по центру.
    /// </summary>
    [Description ("Внизу по центру")]
    BottomSide,

    /// <summary>
    /// Нижний левый угол.
    /// </summary>
    [Description ("Нижний левый угол")]
    BottomLeftCorner,

    /// <summary>
    /// Нижний правый угол.
    /// </summary>
    [Description ("Нижний правый угол")]
    BottomRightCorner
}
