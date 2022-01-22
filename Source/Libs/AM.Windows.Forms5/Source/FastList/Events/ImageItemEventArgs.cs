// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ImageItemEventargs.cs -- аргумент события, позволяющий вычислить изображение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Аргумент события, позволяющий вычислить изображение.
/// </summary>
public sealed class ImageItemEventArgs
    : GenericItemResultEventArgs<Image>
{
    // пустое тело класса
}
