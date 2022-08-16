// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* TreeGridAlignment.cs -- выравнивание элементов в гриде
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Windows.Forms;

/// <summary>
/// Выравнивание элементов в гриде <see cref="TreeGrid"/>.
/// </summary>
public enum TreeGridAlignment
{
    /// <summary>
    /// Влево.
    /// </summary>
    Near,

    /// <summary>
    /// По центру.
    /// </summary>
    Center,

    /// <summary>
    /// Вправо.
    /// </summary>
    Far
}
