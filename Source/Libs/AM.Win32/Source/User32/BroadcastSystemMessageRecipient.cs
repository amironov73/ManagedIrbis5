// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* BroadcastSystemMessageRecipient.cs -- задает получателей системных широковещательных сообщений
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Задает получателей системных широковещательных сообщений.
/// </summary>
[Flags]
public enum BroadcastSystemMessageRecipient
{
    /// <summary>
    /// Вещание на все системные компоненты.
    /// </summary>
    BSM_ALLCOMPONENTS = 0x00000000,

    /// <summary>
    /// Windows 95/98/Me: Вещание на все системные драйверы.
    /// </summary>
    BSM_VXDS = 0x00000001,

    /// <summary>
    /// Windows 95/98/Me: Вещание в сетевые драйверы.
    /// </summary>
    BSM_NETDRIVER = 0x00000002,

    /// <summary>
    /// Windows 95/98/Me: Вещание в устанавливаемые драйверы.
    /// </summary>
    BSM_INSTALLABLEDRIVERS = 0x00000004,

    /// <summary>
    /// Вещание приложениям.
    /// </summary>
    BSM_APPLICATIONS = 0x00000008,

    /// <summary>
    /// Windows NT/2000/XP: Трансляция на все рабочие столы.
    /// Требуется привилегия SE_TCB_NAME.
    /// </summary>
    BSM_ALLDESKTOPS = 0x00000010
}
