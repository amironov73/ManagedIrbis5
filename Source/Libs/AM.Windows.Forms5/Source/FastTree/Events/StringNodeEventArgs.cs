// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* StringNodeEventArgs.cs -- аргумент события, позволяющий вычислить строковое значение
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Аргумент события, позволяющий вычислить строковое значение.
/// </summary>
public sealed class StringNodeEventArgs
    : GenericNodeResultEventArgs<string>
{
    // пустое тело класса
}
