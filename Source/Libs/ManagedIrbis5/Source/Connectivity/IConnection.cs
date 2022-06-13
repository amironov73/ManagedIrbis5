// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IConnection.cs -- генерализованный интерфейс подключения к ИРБИС-серверу
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Генерализованный интерфейс подключения к ИРБИС-серверу.
/// </summary>
public interface IConnection
    : IConnectionSettings,
    IIrbisProvider
{
    // пустое тело интерфейса
}
