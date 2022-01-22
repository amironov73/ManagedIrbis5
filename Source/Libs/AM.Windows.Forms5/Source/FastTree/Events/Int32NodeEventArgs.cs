// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Int32NodeEventArgs.cs -- аргумент события, позволяющий вычислить целочисленное значение
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Аргумент события, позволяющий вычислить целочисленное значение.
/// </summary>
public sealed class Int32NodeEventArgs
    : GenericNodeResultEventArgs<int>
{
    // пустое тело класса
}
