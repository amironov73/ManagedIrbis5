// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IPaintingService.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public interface IPaintingService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="color"></param>
    /// <param name="thickness"></param>
    /// <returns></returns>
    Pen GetPen (Color color, int thickness = 1);

    /// <summary>
    ///
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    SolidBrush GetBrush (Color color);

    /// <summary>
    ///
    /// </summary>
    void CleanUp();
}
