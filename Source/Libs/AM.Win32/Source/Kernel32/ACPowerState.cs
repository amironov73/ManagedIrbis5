// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ACPowerState.cs -- статус электрического питания системы
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Статус электрического питания системы.
/// </summary>
public enum ACPowerStatus
    : byte
{
    /// <summary>
    /// Оффлайн, т. е. система запитана от батарей.
    /// </summary>
    Offline = 0,

    /// <summary>
    /// Онлайн, т. е. система запистана от сети.
    /// </summary>
    Online = 1,

    /// <summary>
    /// Неизвестно.
    /// </summary>
    Unknown = 255
}
