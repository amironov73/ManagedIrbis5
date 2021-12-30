// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ITokenStream.cs -- интерфейс потока токенов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Scripting.Kitten;

/// <summary>
/// Интерфейс потока токенов.
/// </summary>
public interface ITokenStream<TToken>
{
    /// <summary>
    /// Чтение токенов в буфер.
    /// </summary>
    /// <param name="buffer">Буфер, в который должны быть помещены токены.</param>
    /// <returns>Количество реально прочитанных токенов.</returns>
    int Read (Span<TToken> buffer);

}
