// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ColorNodeEventArgs.cs -- аргумент события, позволяющий вычислить цвет
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Аргумент события, позволяющий вычислить цвет (например, цвет текста).
/// </summary>
public sealed class ColorNodeEventArgs
    : GenericNodeResultEventArgs<Color>
{
    // пустое тело класса
}
