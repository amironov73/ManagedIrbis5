// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IStringPoolProvider.cs -- провайдер пула строк
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using CommunityToolkit.HighPerformance.Buffers;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// Провайдер пула строк <see cref="StringPool"/>.
/// </summary>
public interface IStringPoolProvider
{
    /// <summary>
    /// Получение пула строк <see cref="StringPool"/>.
    /// </summary>
    StringPool GetStringPool();

    /// <summary>
    /// Возврат пула строк провайдеру. Предполагается, что строки,
    /// полученные от пула, больше не нужны.
    /// Провайдер может сделать с возвращенным пулом всё, что угодно,
    /// в том числе ничего.
    /// </summary>
    void ReturnStringPool
        (
            StringPool stringPool
        );
}
