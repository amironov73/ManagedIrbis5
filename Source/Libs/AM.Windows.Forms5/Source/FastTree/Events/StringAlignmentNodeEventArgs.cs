// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* StringAlignmentNodeEventArgs.cs -- аргумент события, позволяющий вычислить горизонтальное выравнивание в строке
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Аргумент события, позволяющий вычислить горизонтальное выравнивание в строке.
/// </summary>
public class StringAlignmentNodeEventArgs
    : GenericNodeResultEventArgs<StringAlignment>
{
    // пустое тело класса
}
