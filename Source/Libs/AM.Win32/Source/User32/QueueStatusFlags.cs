// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* QueueStatusFlags.cs -- типы сообщений, для которых необходимо проверить очередь
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Указывает типы сообщений, для которых необходимо проверить очередь.
/// </summary>
[Flags]
public enum QueueStatusFlags
{
    /// <summary>
    /// Сообщения <c>WM_KEYUP</c>, <c>WM_KEYDOWN</c>, <c>WM_SYSKEYUP</c>
    /// или <c>WM_SYSKEYDOWN</c>.
    /// </summary>
    QS_KEY = 0x0001,

    /// <summary>
    /// Сообщение <c>WM_MOUSEMOVE</c>.
    /// </summary>
    QS_MOUSEMOVE = 0x0002,

    /// <summary>
    /// Сообщения, относящиеся к кнопкам мыши (<c>WM_LBUTTONUP</c>,
    /// <c>WM_RBUTTONDOWN</c> и т. д.).
    /// </summary>
    QS_MOUSEBUTTON = 0x0004,

    /// <summary>
    /// Опубликованное сообщение (кроме перечисленных выше).
    /// </summary>
    QS_POSTMESSAGE = 0x0008,

    /// <summary>
    /// Сообщение  <c>WM_TIMER</c>.
    /// </summary>
    QS_TIMER = 0x0010,

    /// <summary>
    /// Сообщение <c>WM_PAINT</c>.
    /// </summary>
    QS_PAINT = 0x0020,

    /// <summary>
    /// Сообщение, отправленное другим потоком или приложением.
    /// </summary>
    QS_SENDMESSAGE = 0x0040,

    /// <summary>
    /// Сообщение <c>WM_HOTKEY</c>.
    /// </summary>
    QS_HOTKEY = 0x0080,

    /// <summary>
    /// Опубликованное сообщение (кроме перечисленных выше).
    /// </summary>
    QS_ALLPOSTMESSAGE = 0x0100,

    /// <summary>
    /// Windows XP: Необработанное сообщение, относящиееся
    /// к пользовательскому вводу.
    /// </summary>
    QS_RAWINPUT = 0x0400,

    /// <summary>
    /// Сообщение, относящееся к мыши: <c>WM_MOUSEMOVE</c>,
    /// <c>WM_LBUTTONUP</c>, <c>WM_RBUTTONDOWN</c> и т. д.
    /// </summary>
    QS_MOUSE = QS_MOUSEMOVE
               | QS_MOUSEBUTTON,

    /// <summary>
    /// <para>Сообщение, относящееся к пользовательскому вводу,
    /// т. е. комбинация <c>QS_KEY</c> и <c>QS_MOUSE</c>.</para>
    /// <para>Windows XP: Также включает <c>QS_RAWINPUT</c>.</para>
    /// </summary>
    QS_INPUT = QS_MOUSE
               | QS_KEY
               | QS_RAWINPUT,

    /// <summary>
    /// Пользовательский ввод,<c> WM_TIMER</c>, <c>WM_PAINT</c>,
    /// <c>WM_HOTKEY</c> или опубликованное сообщение.
    /// </summary>
    QS_ALLEVENTS = QS_INPUT
                   | QS_POSTMESSAGE
                   | QS_TIMER
                   | QS_PAINT
                   | QS_HOTKEY,

    /// <summary>
    /// Любое сообщение.
    /// </summary>
    QS_ALLINPUT = QS_INPUT
                  | QS_POSTMESSAGE
                  | QS_TIMER
                  | QS_PAINT
                  | QS_HOTKEY
                  | QS_SENDMESSAGE
}
