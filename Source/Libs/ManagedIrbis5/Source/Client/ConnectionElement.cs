// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ConnectionElement.cs -- элемент строки подключения к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis.Client;

/// <summary>
/// Элемент строки подключения к серверу ИРБИС64.
/// </summary>
[Flags]
public enum ConnectionElement
{
    /// <summary>
    /// Ничего.
    /// </summary>
    None = 0,

    /// <summary>
    /// Имя или адрес хоста.
    /// </summary>
    Host = 1,

    /// <summary>
    /// Номер порта.
    /// </summary>
    Port = 2,

    /// <summary>
    /// Логин пользователя.
    /// </summary>
    Username = 4,

    /// <summary>
    /// Пароль.
    /// </summary>
    Password = 8,

    /// <summary>
    /// Код АРМ.
    /// </summary>
    Workstation = 16,

    /// <summary>
    /// Всё вышеперечисленное.
    /// </summary>
    All = 31
}
