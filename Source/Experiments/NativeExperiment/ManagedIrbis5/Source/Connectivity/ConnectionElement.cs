// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ConnectionElement.cs -- элемент строки подключения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis;

/// <summary>
/// Элемент строки подключения.
/// </summary>
[Flags]
public enum ConnectionElement
{
    /// <summary>
    /// Нет элементов.
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
    /// Имя пользователя (логин).
    /// </summary>
    Username = 4,

    /// <summary>
    /// Пароль.
    /// </summary>
    Password = 8,

    /// <summary>
    /// Код рабочей станции.
    /// </summary>
    Workstation = 16,

    /// <summary>
    /// Все вышеперечисленное.
    /// </summary>
    All = 31
}
