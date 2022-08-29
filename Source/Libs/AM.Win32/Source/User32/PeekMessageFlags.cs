// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PeekMessageFlags.cs -- указывает, как сообщения обрабатываются функцией PeekMessage
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Указывает, как сообщения обрабатываются функцией <c>PeekMessage</c>.
/// </summary>
[Flags]
public enum PeekMessageFlags
{
    /// <summary>
    /// Сообщения не удаляются из очереди после обработки
    /// <c>PeekMessage</c>.
    /// </summary>
    PM_NOREMOVE = 0x0000,

    /// <summary>
    /// Сообщения удаляются из очереди после обработки
    /// <c>PeekMessage</c>.
    /// </summary>
    PM_REMOVE = 0x0001,

    /// <summary>
    /// Предотвращает освобождение системой любого потока,
    /// ожидающего бездействия вызывающего объекта
    /// (см. <c>WaitForInputIdle</c>). Объедините это значение либо
    /// с <c>PM_NOREMOVE</c>, либо с <c>PM_REMOVE</c>.
    /// </summary>
    PM_NOYIELD = 0x0002,

    /// <summary>
    /// Windows 98/Me, Windows 2000/XP: обработка сообщений мыши
    /// и клавиатуры.
    /// </summary>
    PM_QS_INPUT = QueueStatusFlags.QS_INPUT << 16,

    /// <summary>
    /// Windows 98/Me, Windows 2000/XP: Обработка всех размещенных
    /// сообщений, включая таймеры и горячие клавиши.
    /// </summary>
    PM_QS_POSTMESSAGE = (QueueStatusFlags.QS_POSTMESSAGE
                         | QueueStatusFlags.QS_HOTKEY
                         | QueueStatusFlags.QS_TIMER) << 16,

    /// <summary>
    /// Windows 98/Me, Windows 2000/XP: обработка сообщений рисования.
    /// </summary>
    PM_QS_PAINT = QueueStatusFlags.QS_PAINT << 16,

    /// <summary>
    /// Windows 98/Me, Windows 2000/XP: обрабатывать
    /// все отправленные сообщения.
    /// </summary>
    PM_QS_SENDMESSAGE = QueueStatusFlags.QS_SENDMESSAGE << 16,
}
